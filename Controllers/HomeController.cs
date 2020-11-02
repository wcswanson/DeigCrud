using DeigCrud.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
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
    //todo: 11/2/2020 Chect the use of TempData
    //todo: 11/2/2020 Remove not needed  code
    //todo: z Disable input button

    public class HomeController : Controller
    {
        // Vars for holding information to pass to the spList
        // 0 might be better as -1 for the int values
        int listId = 0;
        char b = 'a';
        int dayId = 0;
        int timeId = 0;
        string town = "";
        static string msg = "";
        //private Stream fileStream;

        public IActionResult Index()
        {
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
                ListModel = PopulateList(listId, b, dayId, timeId, town)
            };

            dlmodel.SuspendSelect = "a";

            return View(dlmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(char? SuspendSelect, int? DOWSelect, int? TimeSelect, string TownSelect)
        {
            //todo: code for null vars coming in.
            //var list = TempData["id"];
            //if (list == null)
            //{
            //    listId = Convert.ToInt32(list);
            //}
            b = (char)SuspendSelect;
            //dayId = (int)DOWSelection;
            //timeId = (int)TimeSelection;
            //town = TownSelection.ToString();

            var dlmodel = new DlViewModel()
            {
                TownModel = PopulateTowns(),
                DOWModel = PopulateDOW(),
                TimeModel = PopulateTime(),
                ListModel = PopulateList(listId, b, DOWSelect, TimeSelect, TownSelect)
            };

            return View(dlmodel);
        }

        public IActionResult Create()
        {
            return View("Create");
        }

        public IActionResult Update(int id )
        {
            listId = id;
            var dlmodel = new DlViewModel()
            {
                TownModel = PopulateTowns(),
                DOWModel = PopulateDOW(),
                TimeModel = PopulateTime(),
                ListModel = PopulateList(listId, b,dayId, timeId, town)
            };

            TempData["ListId"] = listId;
            return View ("Update", dlmodel);   //  Update a meeting: " + id.ToString();
        }

        [HttpPost]
        public IActionResult Update (DlViewModel dlModel)
        {
            int id = Convert.ToInt32(TempData["ListId"]);
            int rc = UpdateList(dlModel, id);

            TempData["id"] =  id;
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            return View("Delete"); // Delete meeting: " + id.ToString();
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
                        msg = msg + " spTowns: " + ex.Message.ToString();
                    }
                }
                connection.Close();
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
                        msg = msg + " spDow: " + ex.Message.ToString();
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
                        msg = "sptime: " + ex.Message.ToString();
                    }

                }
                connection.Close();
            }
            return items;
        }

#nullable enable
        private static List<MeetingListModel> PopulateList(int listId, char? b, int? dow, int? timeId, string? town)
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

                string sql = "spList";
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
                        msg = msg + " spList: " + ex.Message.ToString();
                    }
                    connection.Close();
                }

                return meetingList;
            }
        }

#nullable enable
        // Update List
        public static int UpdateList(DlViewModel dl,int id)
        {
          
            int rc = -1; // Is this needed?            

            using (SqlConnection connection = new SqlConnection(Startup.cnstr))
            {
                string sql = "spUpdateList";    // This is the only thing that needs to bechanged to do inserts. Add a constant for the procedure name and pass it in to this function.
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add Parms
                // ListId
                SqlParameter listid = cmd.Parameters.Add("@ListId", SqlDbType.Int);
                if (id == 0)
                {
                    listid.Value = null;
                }
                else
                {
                    listid.Value = id;
                }

                // Suspend
                SqlParameter bsuspend = cmd.Parameters.Add("@Suspend", SqlDbType.Bit);
                if (dl.SuspendSelect == "False")
                {
                    bsuspend.Value = false;
                }
                else if (dl.SuspendSelect == "True")
                {
                    bsuspend.Value = true;
                }
                else
                {
                    bsuspend.Value = null;
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
                    dowid.Value = null;
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
                    timeid.Value = null;
                }

                // Town
                SqlParameter townname = cmd.Parameters.Add("@Town", SqlDbType.NVarChar);
                if (dl.TownSelect.Length == 0)
                {
                    townname.Value = null;
                }
                else
                {
                    townname.Value = dl.TownSelect.ToString();
                }

                // Group Name
                SqlParameter groupname = cmd.Parameters.Add("@GroupName", SqlDbType.NVarChar);
                if (dl.GroupNameSelect.Length == 0)
                {
                    groupname.Value = null;
                }
                else
                {
                    groupname.Value = dl.GroupNameSelect.ToString();
                }

                // Informantion
                SqlParameter information = cmd.Parameters.Add("@Information", SqlDbType.NVarChar);
                if (dl.InformationSelect.Length == 0)
                {
                    information.Value = null;
                }
                else
                {
                    information.Value = dl.InformationSelect.ToString();
                }

                // Location
                SqlParameter location = cmd.Parameters.Add("@Location", SqlDbType.NVarChar);
                if (dl.LocationSelect.Length == 0)
                {
                    location.Value = null;
                }
                else
                {
                    location.Value = dl.LocationSelect.ToString();
                }

                // Type
                SqlParameter type = cmd.Parameters.Add("@Type", SqlDbType.NVarChar);
                if (dl.TypeSelect.Length == 0)
                {
                    type.Value = null;
                }
                else
                {
                    type.Value = dl.TypeSelect.ToString();
                }

                connection.Open();
                try
                {
                    rc = cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    msg = ex.ToString();
                }
                connection.Close();
               
            }
           
            return rc;
        }

        // Nullables
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
                    msg = ex.ToString();
                }
                StringBuilder stringBuilder = new StringBuilder();
                foreach (DataColumn column in dataTable.Columns)
                {
                    stringBuilder.Append(column.ColumnName + comma);
                }
                stringBuilder.AppendLine();
                foreach (DataRow row in dataTable.Rows)
                {
                    foreach (DataColumn column2 in dataTable.Columns)
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