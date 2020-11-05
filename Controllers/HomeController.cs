using DeigCrud.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
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

    public class HomeController : Controller
    {       
        // 0 might be better as -1 for the int values
        const string SPUPATE = "spUpdateList";
        const string SPCREATE = "spCreateList";
        const string SPDELETE = "spDeleteList";
        const string UPDATE = "U";
        const string CREATE = "C";
        const string DELETE = "D";           

        int listId = 0;
        char b = 'a';
        int dayId = 0;
        int timeId = 0;
        string town = "";
        static string msg = "";
        string sp = "";
        
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
                TownModel = PopulateTowns(),
                DOWModel = PopulateDOW(),
                TimeModel = PopulateTime(),
                ListModel = PopulateList(listId, b, dayId, timeId, town, sp)
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
                        break;
                }
                //ViewBag.Result = $"Meeting id:  {listId} has been updated.";
            }

            dlmodel.SuspendSelect = "0";
            
            return View(dlmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(char? SuspendSelect, int? DOWSelect, int? TimeSelect, string TownSelect)
        {
            //todo: code for null vars coming in.

            b = (char)SuspendSelect;
            //dayId = (int)DOWSelection;
            //timeId = (int)TimeSelection;
            //town = TownSelection.ToString();

            var dlmodel = new DlViewModel()
            {
                TownModel = PopulateTowns(),
                DOWModel = PopulateDOW(),
                TimeModel = PopulateTime(),
                ListModel = PopulateList(listId, b, DOWSelect, TimeSelect, TownSelect, sp)
            };

            return View(dlmodel);
        }

        // Create Get      
        public IActionResult Create()
        {
            var dlmodel = new DlViewModel()
            {
                TownModel = PopulateTowns(),
                DOWModel = PopulateDOW(),
                TimeModel = PopulateTime(),
                ListModel = PopulateList(listId, b, dayId, timeId, town, sp)
            };
            return View("Create", dlmodel);
        }

        // Create Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DlViewModel dl)
        {
            //todo: Create constants for sp for create and update
            //todo: Call function, passing in the model, and sp name.
            //todo: redirect to index to display the new record with ListId
            //todo: z Create page -- add vars containers

           string rc = UpdateList(dl, listId, SPCREATE);

            // Int or string?           
            TempData["id"] = Convert.ToInt32(rc);
            TempData["sender"] = CREATE;
            return RedirectToAction("Index");
        }

       // [ValidateAntiForgeryToken]
        public IActionResult Update(int id )
        {
            listId = id;
            var dlmodel = new DlViewModel()
            {
                TownModel = PopulateTowns(),
                DOWModel = PopulateDOW(),
                TimeModel = PopulateTime(),
                ListModel = PopulateList(listId, b,dayId, timeId, town, sp)
            };

            ViewBag.Result = "Update meeting with the id:" + listId.ToString();
            TempData["id"] = listId;
            return View ("Update", dlmodel);   //  Update a meeting: " + id.ToString();
        }

        // Update Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update (DlViewModel dlModel)
        {
            int id = Convert.ToInt32(TempData["id"]);
            string rc = UpdateList(dlModel, id, SPUPATE);

            TempData["id"] =  id;
            return RedirectToAction("Index");
        }

        //  Delete get
       // [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            listId = id;
            var dlmodel = new DlViewModel()
            {
                TownModel = PopulateTowns(),
                DOWModel = PopulateDOW(),
                TimeModel = PopulateTime(),
                ListModel = PopulateList(listId, b, dayId, timeId, town, sp)
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
                SqlCommand cmd = new SqlCommand(SPDELETE, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                //todo: Finish this code
                SqlParameter listid = cmd.Parameters.Add("@ListId", SqlDbType.Int);
                listid.Value = listId;

                connection.Open();

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch(SqlException ex)
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
                TempData["sender"] = DELETE;
                return RedirectToAction("Index");
            }
        }

        // This should go into a separate file
        private static List<SelectListItem> PopulateTowns()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            using (SqlConnection connection = new SqlConnection(Startup.cnstr))
            {
                connection.Open();
                string sql = "spTowns";

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    try
                    {
                        while (dr.Read())
                        {
                            items.Add(new SelectListItem
                            {
                                Value = dr["Town"].ToString(),
                                Text = dr["Town"].ToString()
                            });
                        }
                    }
                    catch (SqlException ex)
                    {
                        msg = msg + $" spTowns: {ex.Message.ToString()}" ;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }               
            }
            return items;
        }

        //DOW
        private static List<SelectListItem> PopulateDOW()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            using (SqlConnection connection = new SqlConnection(Startup.cnstr))
            {
                connection.Open();
                string sql = "spDOW";

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    try
                    {
                        while (dr.Read())
                        {
                            items.Add(new SelectListItem
                            {
                                Value = dr["DayID"].ToString(),
                                Text = dr["DayName"].ToString()
                            });
                        }
                    }
                    catch (SqlException ex)
                    {
                        msg = msg + $" spDow: {ex.Message.ToString()} ";
                    }
                }

                connection.Close();
            }
            return items;
        }

        // Time
        private static List<SelectListItem> PopulateTime()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            using (SqlConnection connection = new SqlConnection(Startup.cnstr))
            {
                connection.Open();
                string sql = "spTime";

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.CommandType = CommandType.StoredProcedure;


                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    try
                    {
                        while (dr.Read())
                        {
                            items.Add(new SelectListItem
                            {
                                Value = dr["TimeID"].ToString(),
                                Text = dr["Time"].ToString()
                            });
                        }
                    }
                    catch (SqlException ex)
                    {
                        msg = $"spTime: {ex.Message.ToString()}";
                    }

                }
                connection.Close();
            }
            return items;
        }

#nullable enable
        private static List<MeetingListModel> PopulateList(int listId, char? b, int? dow, int? timeId, string town, string? sp)
        {
            //@ListId int
            //@Suspend bit
            //@DOWID int 
            //@GroupName string
            //@Information string
            //@Location string 
            //@Type = string
            //@TimeID int,
            //@Town string

            List<MeetingListModel> meetingList = new List<MeetingListModel>();
            using (SqlConnection connection = new SqlConnection(Startup.cnstr))
            {
                connection.Open();

                string sql = "spMaintenanceList";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add Parms
                // ListId
                SqlParameter listid = cmd.Parameters.Add("@ListId", SqlDbType.Int);
                if ( listId == 0)
                {
                    listid.Value = null;
                }
                else
                {
                    listid.Value = listId;
                }

                // Suspend
                SqlParameter bsuspend = cmd.Parameters.Add("@Suspend", SqlDbType.Bit);
                if (b == '0')
                {
                    bsuspend.Value = false;
                }
                else if (b == '1')
                {
                    bsuspend.Value = true;
                }
                else
                {
                    bsuspend.Value = null;
                }

                // DOW (day of week id)
                SqlParameter dowid = cmd.Parameters.Add("@DOWID", SqlDbType.Int);

                if (dow > 0 && dow < 8)
                {
                    dowid.Value = (int)dow;
                }
                else
                {
                    dowid.Value = null;
                }

                // Time Id
                SqlParameter timeid = cmd.Parameters.Add("@TimeID", SqlDbType.Int);
                if (timeId > 0 && timeId < 370)
                {
                    timeid.Value = (int)timeId;
                }
                else
                {
                    timeid.Value = null;
                }

                // Town
                SqlParameter townname = cmd.Parameters.Add("@Town", SqlDbType.NVarChar);
                if (town.Length < 4)
                {
                    townname.Value = null;
                }
                else
                {
                    townname.Value = town.ToString();
                }

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    try
                    {
                        while (dr.Read())
                        {
                            MeetingListModel ml = new MeetingListModel();
                            ml.ListID = Convert.ToInt32(dr["ListID"]);
                            ml.DOW = Convert.ToInt32(dr["DOW"]);
                            ml.Day = Convert.ToString(dr["Day"]);
                            ml.TimeID = Convert.ToInt32(dr["TimeID"]);
                            ml.Time = Convert.ToString(dr["Time"]);
                            ml.Town = Convert.ToString(dr["Town"]);
                            ml.GroupName = Convert.ToString(dr["GroupName"]);
                            ml.Information = Convert.ToString(dr["Information"]);
                            ml.Location = Convert.ToString(dr["Location"]);
                            ml.Type = Convert.ToString(dr["Type"]);
                            ml.suspend = Convert.ToBoolean(dr["suspend"]);

                            meetingList.Add(ml);
                        }
                    }
                    catch (SqlException ex)
                    {
                        msg = msg + $" spmaintenanceList: {ex.Message.ToString()}";
                    }
                    connection.Close();
                }

                return meetingList;
            }
        }

#nullable enable
        // Update List
        public static string UpdateList(DlViewModel dl,int id, string sp)
        {
            using (SqlConnection connection = new SqlConnection(Startup.cnstr))
            {
                string sql = sp;    // This is the only thing that needs to bechanged to do inserts. Add a constant for the procedure name and pass it in to this function.
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.CommandType = CommandType.StoredProcedure;
               
                // Add Parms
                // ListId
                if (sp == SPUPATE)
                {
                    SqlParameter listid = cmd.Parameters.Add("@ListId", SqlDbType.Int);
                    if (id == 0)
                    {                        
                        listid.Value = null;
                    }
                    else
                    {                        
                        listid.Value = id;
                    }
                }
                else
                {
                    cmd.Parameters.Add("@new_id", SqlDbType.Int).Direction = ParameterDirection.Output;                  
                }

                // Suspend
                SqlParameter bsuspend = cmd.Parameters.Add("@Suspend", SqlDbType.Bit);
                if (dl.SuspendSelect == "1")
                {
                    bsuspend.Value = true;
                }
                else 
                {
                    bsuspend.Value = false;
                }
               
                //DOW(day of week id)
                int dow = Convert.ToInt32(dl.DOWSelect);
                SqlParameter dowid = cmd.Parameters.Add("@DOWID", SqlDbType.Int);

                if (dow > 0 && dow < 8)
                {
                    dowid.Value = (int)dow;
                }
                else
                {
                    dowid.Value = 8;
                }

                // Time Id
                int timeId = Convert.ToInt32(dl.TimeSelect);
                SqlParameter timeid = cmd.Parameters.Add("@TimeID", SqlDbType.Int);
                if (timeId > 0 && timeId < 370)
                {
                    timeid.Value = (int)timeId;
                }
                else
                {
                    timeid.Value = 0;
                }

                // Town
                SqlParameter townname = cmd.Parameters.Add("@Town", SqlDbType.NVarChar);              
                if (String.IsNullOrEmpty(dl.TownSelect))
                {
                     townname.Value = "";
                }
                else
                {
                    townname.Value = dl.TownSelect.ToString();
                }

                // Group Name
                SqlParameter groupname = cmd.Parameters.Add("@GroupName", SqlDbType.NVarChar);
                if (String.IsNullOrEmpty(dl.GroupNameSelect))                    
                {
                    groupname.Value = "";
                }
                else
                {
                    groupname.Value = dl.GroupNameSelect.ToString();
                }

                // Informantion
                SqlParameter information = cmd.Parameters.Add("@Information", SqlDbType.NVarChar);
               if (String.IsNullOrEmpty(dl.InformationSelect))
                    {
                    information.Value = "";
                }
                else
                {
                    information.Value = dl.InformationSelect.ToString();
                }

                // Location
                SqlParameter location = cmd.Parameters.Add("@Location", SqlDbType.NVarChar);               
               if (String.IsNullOrEmpty(dl.LocationSelect))
                    {
                    location.Value = "";
                }
                else
                {
                    location.Value = dl.LocationSelect.ToString();
                }

                // Type
                SqlParameter type = cmd.Parameters.Add("@Type", SqlDbType.NVarChar);
                if (String.IsNullOrEmpty(dl.TypeSelect))               
                {
                    type.Value = "";
                }
                else
                {
                    type.Value = dl.TypeSelect.ToString();
                }

                connection.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                    if ( sp == SPCREATE)
                    {
                        msg = cmd.Parameters["@new_id"].Value.ToString();                      
                        
                    }                   
                }
                catch (SqlException ex)
                {
                    msg = msg + $" spList: {ex.Message.ToString()}";
                }
                finally
                {
                    connection.Close();
                }
               
            }
           
            return msg.ToString();
        }
               
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