using DeigCrud.Infrastructure;
using DeigCrud.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DeigCrud.Controllers
{

    //todo: Change background on buttons
    //todo: Change font size to make it smaller
    //todo: Remove titles in cshtml to make the presentation iframe ready
    //todo:     z Correct spelling in Group name in sp
    //todo:     z Remove empty space for first 2 columns in sp.
    //todo: bootstrap styling
    //todo: error log add
    //todo: change default int values from 0 to -1

    // Store Procedure to add:
    // Insert or create
    //  z Update
    // Delete

    //todo: z 11/2/2020 Clean up updates -- remove not needed cells from update form. 
    //todo: z 11/2/2020 Chect the use of TempData
    //todo: z 11/2/2020 Remove not needed  code
    //todo: z Disable input button
    //todo: Add loogging to a file.
    //todo: Check for vars floating around that have values that can cause a crash.

    // asp-controller="Physical"

    [Authorize(Roles = "Admin, List")]
    public class PhysicalController : Controller
    {
        // 0 might be better as -1 for the int values
        //const string SPUPATE = "spUpdateList";
        //const string SPCREATE = "spCreateList";
        //const string SPDELETE = "spDeleteList";
        //const string SPDISTRICT = "spDistrict";
        //const string UPDATE = "U";
        //const string CREATE = "C";
        //const string DELETE = "D";

        int listId = 0;
        char b = 'a';
        int dayId = 0;
        int timeId = 0;
        string town = "";
        static string msg = "";
        string sp = "";
        int districtNumber = -1;

        //private Stream fileStream;
        
        public IActionResult Index()
        {
            
            //todo: Add TempData["sender"] for update, created, deleted
            var list = TempData["id"];
            if (list != null)
            {
                listId = Convert.ToInt32(list);
            }
            var dlmodel = new DlViewModel()
            {
                
                DistrictModel = PhysicalHelpers.PopulateDistricts(),
                TownModel = PhysicalHelpers.PopulateTowns(),
                DOWModel = PhysicalHelpers.PopulateDOW(),
                TimeModel = PhysicalHelpers.PopulateTime(),
                ListModel = PhysicalHelpers.PopulateList(listId, b, dayId, timeId, town, sp, districtNumber)
            };

            if (listId > 0)
            {
                switch (TempData["sender"])
                {
                    case "U":
                        ViewBag.Result = $"Meeting id:  {listId} has been updated.";
                        break;
                    case "C":
                        ViewBag.Result = $"Meeting id:  {listId} has been created.";
                        break;
                    case "D":
                        ViewBag.Result = $"Meeting id:  {listId} has been deleted.";
                        break;
                    default:
                        ViewBag.Result = "";
                        break;
                }
                //ViewBag.Result = $"Meeting id:  {listId} has been updated.";
            }

            dlmodel.SuspendSelect = "0";

            ViewBag.Message = TempData["Message"];

            return View(dlmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(char? SuspendSelect, int? DOWSelect, int? TimeSelect, string TownSelect, int? DistrictSelect)
        {
            //todo: code for null vars coming in.

            b = (char)SuspendSelect;
            //dayId = (int)DOWSelection;
            //timeId = (int)TimeSelection;
            //town = TownSelection.ToString();

            var dlmodel = new DlViewModel()
            {
                DistrictModel = PhysicalHelpers.PopulateDistricts(),
                TownModel = PhysicalHelpers.PopulateTowns(),
                DOWModel = PhysicalHelpers.PopulateDOW(),
                TimeModel = PhysicalHelpers.PopulateTime(),
                ListModel = PhysicalHelpers.PopulateList(listId, b, DOWSelect, TimeSelect, TownSelect, sp, DistrictSelect)
            };

            return View(dlmodel);
        }

        // Create Get      
        public IActionResult Create()
        {
            var dlmodel = new DlViewModel()
            {
                DistrictModel = PhysicalHelpers.PopulateDistricts(),
                TownModel = PhysicalHelpers.PopulateTowns(),
                DOWModel = PhysicalHelpers.PopulateDOW(),
                TimeModel = PhysicalHelpers.PopulateTime(),
                ListModel = PhysicalHelpers.PopulateList(listId, b, dayId, timeId, town, sp, districtNumber)
            };
            return View("Create", dlmodel);
        }

        // Create Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DlViewModel dl)
        {
            //todo:  z Create constants for sp for create and update
            //todo: z Call function, passing in the model, and sp name.
            //todo:  z redirect to index to display the new record with ListId
            //todo: z Create page -- add vars containers

            string rc = PhysicalHelpers.UpdateList(dl, listId, PhysicalHelpers.SPCREATE);

            // Int or string?           
            TempData["id"] = Convert.ToInt32(rc);
            TempData["sender"] = PhysicalHelpers.CREATE;
            return RedirectToAction("Index");
        }

        // [ValidateAntiForgeryToken]
        public IActionResult Update(int id)
        {
            listId = id;
            var dlmodel = new DlViewModel()
            {
                DistrictModel = PhysicalHelpers.PopulateDistricts(),
                TownModel = PhysicalHelpers.PopulateTowns(),
                DOWModel = PhysicalHelpers.PopulateDOW(),
                TimeModel = PhysicalHelpers.PopulateTime(),
                ListModel = PhysicalHelpers.PopulateList(listId, b, dayId, timeId, town, sp, districtNumber)
            };

            ViewBag.Result = $"Update meeting with the id: {listId.ToString()}";
            TempData["id"] = listId;
            return View("Update", dlmodel);   //  Update a meeting: " + id.ToString();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(DlViewModel dlModel)
        {
            int id = Convert.ToInt32(TempData["id"]);
            string rc = PhysicalHelpers.UpdateList(dlModel, id, PhysicalHelpers.UPDATE);

            TempData["id"] = id;
            return RedirectToAction("Index");
        }

        //  Delete get
        // [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {

            // How to display only without the drop downs?
            listId = id;
            var dlmodel = new DlViewModel()
            {
                DistrictModel = PhysicalHelpers.PopulateDistricts(),
                TownModel = PhysicalHelpers.PopulateTowns(),
                DOWModel = PhysicalHelpers.PopulateDOW(),
                TimeModel = PhysicalHelpers.PopulateTime(),
                ListModel = PhysicalHelpers.PopulateList(listId, b, dayId, timeId, town, sp, districtNumber)
            };

            TempData["id"] = listId;
            //  TempData["sender"] = UPDATE;
            return View("Delete", dlmodel);   //  Update a meeting: " + id.ToString();
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Delete()
        {
            string strId = TempData["id"].ToString();
            if (!String.IsNullOrEmpty(strId))
            {
                listId = Convert.ToInt32(strId);
            }
            else
            {
                // ViewBag error msg
                return View();
            }

            using (SqlConnection connection = new SqlConnection(Startup.cnstr))
            {
                SqlCommand cmd = new SqlCommand(PhysicalHelpers.SPDELETELIST, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                //todo: Finish this code
                SqlParameter listid = cmd.Parameters.Add("@ListId", SqlDbType.Int);
                listid.Value = listId;

                connection.Open();

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    msg = $" spDelete{ex.Message.ToString()}";
                }
                finally
                {
                    connection.Close();
                }

                // Set this to null or Index will not display data.
                // ViewBag.Result = "The meeting with the id: " + listId.ToString() + " is staged to be deleted";
                TempData["id"] = listId;
                TempData["sender"] = PhysicalHelpers.DELETE;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Route("[controller]/Cancel")]
        public IActionResult Cancel()
        {
            TempData["Message"] = $"Cancel delete request: {TempData["id"].ToString()}";
            TempData["id"] = "0";

            return RedirectToAction("Index");
        }

        // This should go into a separate file
        
#nullable enable
        private static string ExportMeetingList()
        {
            string comma = ", ";

            using (SqlConnection connection = new SqlConnection(Startup.cnstr))
            {

                connection.Open();
                string sql = "GetMeetingList";

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                cmd.Connection = connection;
                sqlDataAdapter.SelectCommand = cmd;
                DataTable dataTable = new DataTable();
                try
                {
                    sqlDataAdapter.Fill(dataTable);
                }
                catch (InvalidCastException ex)
                {
                    msg = $" Export to Meeting: {ex.ToString()}";
                }
                StringBuilder stringBuilder = new StringBuilder();
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                foreach (DataColumn column in dataTable.Columns)
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                {
                    stringBuilder.Append(column.ColumnName + comma);
                }
                stringBuilder.AppendLine();
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                foreach (DataRow row in dataTable.Rows)
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    foreach (DataColumn column2 in dataTable.Columns)
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    {
                        stringBuilder.Append(row[column2.ColumnName].ToString() + comma);
                    }
                    stringBuilder.AppendLine();
                }

                return stringBuilder.ToString();
            }
        }

    } // controller class
} // Namespance