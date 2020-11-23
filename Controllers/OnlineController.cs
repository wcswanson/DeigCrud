using DeigCrud.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DeigCrud.Controllers
{
    [Authorize(Roles = "Admin, List")]
    public class OnlineController : Controller
    {
        const string SPUPATE = "spUpdateOnlineList";
        const string SPCREATE = "spCreateOnlineList";
        const string SPDELETE = "spDeleteOnlineList";
        const string SPGETONLINE = "spGetOnlineList";
        const string UPDATE = "U";
        const string CREATE = "C";
        const string DELETE = "D";

        // ZoomId and TempData["id"] is used to pass the id from controller function to controller function
        // But not the helper functions
        int ZoomId = 0;
        int dayId = 0;
        int timeId = 0;
        static string msg = "";
        //string sp = "";


#nullable enable
        [HttpGet]
        public IActionResult Index()
        {

            ZoomId = Convert.ToInt32(TempData["id"]);
            if (ZoomId == 0)
            {
                ZoomId = 0;
            }

            var doViewmodel = new DoViewModel()
            {
                DOWModel = PopulateDOW(),
                TimeModel = PopulateTime(),
                OnlineListModel = (IEnumerable<OnlineMeetingsModel>)PopulateOnlineList(ZoomId, dayId, timeId)
            };

            // For displaying rssults
            if (ZoomId > 0)
            {
                switch (TempData["sender"])
                {
                    case "U":
                        ViewBag.Result = $"Zoom Id:  {ZoomId} has been updated.";
                        break;
                    case "C":
                        ViewBag.Result = $"Zoom Id:  {ZoomId} has been created.";
                        break;
                    case "D":
                        ViewBag.Result = $"Zoom Id:  { ZoomId} has been deleted.";
                        break;
                    default:
                        ViewBag.Result = $"Zoom Id:  { ZoomId} has been ???";
                        break;
                }
                //ViewBag.Result = $"Meeting id:  {listId} has been updated.";
            }

            return View(doViewmodel);
        }

        // Index Post
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public IActionResult Index(int? ZoomId, int? DOWSelect, int? TimeSelect)
        {

            var doViewmodel = new DoViewModel()
            {
                DOWModel = PopulateDOW(),
                TimeModel = PopulateTime(),
                OnlineListModel = (IEnumerable<OnlineMeetingsModel>)PopulateOnlineList(ZoomId, DOWSelect, TimeSelect)
            };

            return View(doViewmodel);
        }

        // Get Display
        [HttpGet]
        public IActionResult Display()
        {

            ZoomId = Convert.ToInt32(TempData["id"]);
            if (ZoomId == 0)
            {
                ZoomId = 0;
            }

            var doViewmodel = new DoViewModel()
            {
                DOWModel = PopulateDOW(),
                TimeModel = PopulateTime(),
                OnlineListModel = (IEnumerable<OnlineMeetingsModel>)PopulateOnlineList(ZoomId, dayId, timeId)
            };

            return View(doViewmodel);
        }

        [HttpPost]
        public IActionResult Display(int? ZoomId, int? DOWSelect, int? TimeSelect)
        {

            var doViewmodel = new DoViewModel()
            {
                DOWModel = PopulateDOW(),
                TimeModel = PopulateTime(),
                OnlineListModel = (IEnumerable<OnlineMeetingsModel>)PopulateOnlineList(ZoomId, DOWSelect, TimeSelect)
            };

            return View(doViewmodel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var doViewmodel = new DoViewModel()
            {
                DOWModel = PopulateDOW(),
                TimeModel = PopulateTime(),
                OnlineListModel = (IEnumerable<OnlineMeetingsModel>)PopulateOnlineList(ZoomId, dayId, timeId)
            };
            return View("Create", doViewmodel);
        }

        [HttpPost]
        public IActionResult Create(DoViewModel dol)
        {
            //todo: Create constants for sp for create and update
            //todo: Call function, passing in the model, and sp name.
            //todo: redirect to index to display the new record with ListId
            //todo: z Create page -- add vars containers

            string rc = "err";
            rc = UpdateOnlineList(dol, ZoomId, SPCREATE);

            // Int or string?           
            TempData["id"] = Convert.ToInt32(rc);
            TempData["sender"] = CREATE;
            return RedirectToAction("Index");
        }

        // [ValidateAntiForgeryToken]
        public IActionResult Update(int id)
        {
            ZoomId = id;
            var domodel = new DoViewModel()
            {
                DOWModel = PopulateDOW(),
                TimeModel = PopulateTime(),
                OnlineListModel = PopulateOnlineList(ZoomId, dayId, timeId)
            };

            ViewBag.Result = $"To update online meeting id: {ZoomId.ToString()}";
            TempData["id"] = ZoomId;
            return View("Update", domodel);   //  Update a meeting: " + id.ToString();
        }

        // Post update
        [HttpPost]
        public IActionResult Update(DoViewModel dol)
        {
            int id = Convert.ToInt32(TempData["id"]);
            string rc = UpdateOnlineList(dol, id, SPUPATE);

            ViewBag.Result = $"Updated online meeting id: {id.ToString()}";
            TempData["id"] = id;
            TempData["sender"] = UPDATE;
            return RedirectToAction("Index");
        }

        // Get delete
        public IActionResult Delete(int id)
        {
            ZoomId = id;
            var domodel = new DoViewModel()
            {
                DOWModel = PopulateDOW(),
                TimeModel = PopulateTime(),
                OnlineListModel = PopulateOnlineList(ZoomId, dayId, timeId)
            };

            TempData["id"] = ZoomId;
            ViewBag.Message = $"ZoomId: {ZoomId.ToString()} is staged for delete.";
            TempData["sender"] = UPDATE;
            return View("Delete", domodel);   //  Update a meeting: " + id.ToString();
        }

        // Post delete
        [HttpPost]
        public IActionResult Delete()
        {
            int ZoomId = Convert.ToInt32(TempData["id"]);
            if (ZoomId > 0)
            {
                //ZoomId = Convert.ToInt32(strId);
                string rc = DeleteFunction(ZoomId);
                TempData["id"] = ZoomId;
                TempData["sender"] = DELETE;
            }
            else
            {
                // ViewBag error msg
                ZoomId = 0;
                return View();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("[controller]/Cancel")]
        public IActionResult Cancel()
        {

            ViewBag.Message = "Cancel delete request: ";
            TempData["id"] = "0";

            return RedirectToAction("Index");

        }

        /*   Helper functions  */
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

        /*   Support functions */
#nullable enable
        // Update List
        public static string UpdateOnlineList(DoViewModel dol, int id, string sp)
        {
            using (SqlConnection connection = new SqlConnection(Startup.cnstr))
            {
                //string sql = sp;    // This is the only thing that needs to bechanged to do inserts. Add a constant for the procedure name and pass it in to this function.
                SqlCommand cmd = new SqlCommand(sp, connection);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add Parms
                // ZoomId
                if (sp == SPUPATE)
                {
                    SqlParameter zoomid = cmd.Parameters.Add("@ZoomId", SqlDbType.Int);
                    if (id == 0)
                    {
                        zoomid.Value = null;
                    }
                    else
                    {
                        zoomid.Value = id;
                    }
                }
                // Create and get the return ZoomId
                else
                {
                    cmd.Parameters.Add("@new_id", SqlDbType.Int).Direction = ParameterDirection.Output;
                }

                //DOW(day of week id)
                int dow = Convert.ToInt32(dol.DOWSelect);
                SqlParameter dowid = cmd.Parameters.Add("@DayId", SqlDbType.Int);

                if (dow > 0 && dow < 8)
                {
                    dowid.Value = (int)dow;
                }
                else
                {
                    dowid.Value = 8;
                }

                // Time Id
                int timeId = Convert.ToInt32(dol.TimeSelect);
                SqlParameter timeid = cmd.Parameters.Add("@TimeId", SqlDbType.Int);
                if (timeId > 0 && timeId < 370)
                {
                    timeid.Value = (int)timeId;
                }
                else
                {
                    timeid.Value = 0;
                }

                // Group Name
                SqlParameter groupname = cmd.Parameters.Add("@GroupName", SqlDbType.VarChar);
                if (String.IsNullOrEmpty(dol.groupnameSelect))
                {
                    groupname.Value = "";
                }
                else
                {
                    groupname.Value = dol.groupnameSelect.ToString();
                }

                // Meeting Id
                SqlParameter meetingid = cmd.Parameters.Add("@MeetingId", SqlDbType.VarChar);
                if (String.IsNullOrEmpty(dol.meetingidSelect))
                {
                    meetingid.Value = "";
                }
                else
                {
                    meetingid.Value = dol.meetingidSelect.ToString();
                }

                // Pswd
                SqlParameter pswd = cmd.Parameters.Add("@Pswd", SqlDbType.VarChar);
                if (String.IsNullOrEmpty(dol.pswdSelect))
                {
                    pswd.Value = "";
                }
                else
                {
                    pswd.Value = dol.pswdSelect.ToString();
                }

                // Telephone
                SqlParameter telephone = cmd.Parameters.Add("@Telephone", SqlDbType.VarChar);
                if (String.IsNullOrEmpty(dol.telephoneSelect))
                {
                    telephone.Value = "";
                }
                else
                {
                    telephone.Value = dol.telephoneSelect.ToString();
                }

                // Notes                
                SqlParameter notes = cmd.Parameters.Add("@Notes", SqlDbType.VarChar);
                if (String.IsNullOrEmpty(dol.notesSelect))
                {
                    notes.Value = "";
                }
                else
                {
                    notes.Value = dol.notesSelect.ToString();
                }

                connection.Open();

                try
                {
                    cmd.ExecuteNonQuery();
                    if (sp == SPCREATE)
                    {
                        msg = cmd.Parameters["@new_id"].Value.ToString();
                    }
                }
                catch (SqlException ex)
                {
                    msg = msg + $" spUpdate OnlineList: {ex.Message.ToString()}";
                }
                finally
                {
                    connection.Close();
                }

            }
            return msg.ToString();
        }
#nullable enable
        // Populate the online list
        private static List<OnlineMeetingsModel> PopulateOnlineList(int? ZoomId, int? DayId, int? TimeId)
        {
            List<OnlineMeetingsModel> onlineList = new List<OnlineMeetingsModel>();

            using (SqlConnection connection = new SqlConnection(Startup.cnstr))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand(SPGETONLINE, connection);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add Parms
                // ZoomtId
                SqlParameter zoomid = cmd.Parameters.Add("@ZoomId", SqlDbType.Int);
                if (ZoomId == 0 || ZoomId == null)
                {
                    zoomid.Value = null;
                    ZoomId = 0;
                }
                else
                {
                    zoomid.Value = ZoomId;
                }

                // DayId
                SqlParameter dayid = cmd.Parameters.Add("@DayId", SqlDbType.Int);
                if (DayId == 0 || DayId == 8)
                {
                    dayid.Value = null;
                    DayId = 0;
                }
                else
                {
                    dayid.Value = DayId;
                }

                // TimeId
                SqlParameter timeid = cmd.Parameters.Add("@TimeId", SqlDbType.Int);
                if (TimeId == 0)
                {
                    timeid.Value = null;
                }
                else
                {
                    timeid.Value = TimeId;
                }

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    try
                    {
                        while (dr.Read())
                        {
                            OnlineMeetingsModel ol = new OnlineMeetingsModel();
                            ol.zoomid = Convert.ToInt32(dr["zoomid"]);
                            ol.dayid = Convert.ToInt32(dr["dayid"]);
                            ol.day = Convert.ToString(dr["DayName"]);
                            ol.timeid = Convert.ToInt32(dr["timeid"]);
                            ol.time = Convert.ToString(dr["Time"]);
                            ol.meetingid = Convert.ToString(dr["meetingid"]);
                            // "password" is not allowed for it throws an out of range exception.
                            ol.pswd = Convert.ToString(dr["pswd"]);
                            ol.telephone = Convert.ToString(dr["telephone"]);
                            ol.groupname = Convert.ToString(dr["groupname"]);
                            ol.notes = Convert.ToString(dr["notes"]);

                            onlineList.Add(ol);
                        }
                    }
                    catch (SqlException ex)
                    {
                        msg = msg + $" onlineList: {ex.Message.ToString()}";
                    }
                    connection.Close();
                }

                return onlineList;
            }
        }

        // Delete function
        private static string DeleteFunction(int ZoomId)
        {



            using (SqlConnection connection = new SqlConnection(Startup.cnstr))
            {
                SqlCommand cmd = new SqlCommand(SPDELETE, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                //todo: Finish this code
                SqlParameter zoomid = cmd.Parameters.Add("@ZoomId", SqlDbType.Int);
                zoomid.Value = ZoomId;

                connection.Open();

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    msg = $" spOnlineDelete{ex.Message.ToString()}";
                }
                finally
                {
                    connection.Close();
                }
            }

            return DELETE;
        }
    }
}
