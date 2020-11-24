using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using DeigCrud.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DeigCrud.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TownController : Controller
    {
        const string SPUPDATE = "spUpdateTowns";
        const string SPCREATE = "spCreateTown";
        const string SPDELETE = "spDeleteTown";
        const string SPGETTOWN = "spTownsAndDistricts";
        const string SPDISTRICT = "spDistrict";
        const string SPTOWN = "spTableTowns";       
        const string UPDATE = "U";
        const string CREATE = "C";
        const string DELETE = "D";

        int districtnumber = -1;
        // string townname = "";
        static string msg = "";
        int TownId = 0;

        [HttpGet]
        public IActionResult Index()
        {
            var list = TempData["id"];
            if (list != null)
            {
                TownId = Convert.ToInt32(list);
            }
            
            // Deig town distrct model
            var dtdmodel = new DTownDistrictViewModel()
            {
                TownModel = PopulateTowns(),
                DistrictModel = PopulateDistrict(),

                DistrictTownModel = PopulateDistrictsTown(TownId, districtnumber)
            };

            if (TownId > 0)
            {
                switch (TempData["sender"])
                {
                    case "U":
                        ViewBag.Result = $"Town Id:  {TownId} has been updated.";
                        break;
                    case "C":
                        ViewBag.Result = $"Town Id:  {TownId} has been created.";
                        break;
                    case "D":
                        ViewBag.Result = $"Poof! Town id:  {TownId} has been deleted."; ;
                        break;
                    default:
                        ViewBag.Result = $"Town Id:  { TownId} has been ???";
                        break;
                }
               
            }

            return View(dtdmodel);
        }

        // Post Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(int? TownIdSelect, int? DistrictSelect)
        {
            var dtdmodel = new DTownDistrictViewModel()
            {
                TownModel = PopulateTowns(),
                DistrictModel = PopulateDistrict(),

                DistrictTownModel = PopulateDistrictsTown(TownIdSelect, DistrictSelect)
            };

            return View(dtdmodel);
        }

        // Create Get 
        [HttpGet]
        public IActionResult Create()
        {
            var dtdmodel = new DTownDistrictViewModel()
            {
                TownModel = PopulateTowns(),
                DistrictModel = PopulateDistrict(),

                DistrictTownModel = PopulateDistrictsTown(TownId, districtnumber)
            };

            return View("Create", dtdmodel);
        }

        // Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DTownDistrictViewModel dtd)
        {
            //todo: Create constants for sp for create and update
            //todo: Call function, passing in the model, and sp name.
            //todo: redirect to index to display the new record with ListId
            //todo: z Create page -- add vars containers
            string rc = "";
            if (ModelState.IsValid)
            {
                rc = UpdateDistrictTowns(dtd, TownId, CREATE);
            }

            // Int or string?
            if (rc.Length > 0) { TempData["id"] = Convert.ToInt32(rc); }           
            TempData["sender"] = CREATE;
            return RedirectToAction("Index");
        }

        // Update
        [HttpGet]
        public IActionResult Update(int id)
        {
            TownId = id;
            var dtd = new DTownDistrictViewModel()
            {
                DistrictModel = PopulateDistrict(),
                TownModel = PopulateTowns(),

                DistrictTownModel = PopulateDistrictsTown(TownId, districtnumber)
            };

            ViewBag.Result =  $"Update towns with the id: {TownId.ToString()}" ;
            TempData["id"] = TownId;
            return View("Update", dtd);   //  Update a meeting: " + id.ToString();
        }

        // Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(DTownDistrictViewModel dtdView)
        {
            int id = Convert.ToInt32(TempData["id"]);
            string rc = UpdateDistrictTowns(dtdView, id, UPDATE);

            TempData["id"] = id;
            return RedirectToAction("Index");
        }

        // Delete
        [HttpGet]
        //[ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            
            if ( id > 0)
            {
                TownId = Convert.ToInt32(id);
            }
            else
            {
                // ViewBag error msg
                return View("Index");
            }

            using (SqlConnection connection = new SqlConnection(Startup.cnstr))
            {
                SqlCommand cmd = new SqlCommand(SPDELETE, connection);
                cmd.CommandType = CommandType.StoredProcedure;
               
                SqlParameter townid = cmd.Parameters.Add("@TownId", SqlDbType.Int);
                townid.Value = TownId;

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

                TempData["id"] = TownId;
                TempData["sender"] = DELETE;
                return RedirectToAction("Index");
            }
        }

        // Functions

        // District and Towns
#nullable enable
        private static List<TownDistrictModel> PopulateDistrictsTown(int? TownId, int? DistrictNumber)  
        {

            List<TownDistrictModel> TownDistrictList = new List<TownDistrictModel>();
            using (SqlConnection connection = new SqlConnection(Startup.cnstr))
            {
                connection.Open();
                               
                SqlCommand cmd = new SqlCommand(SPGETTOWN, connection);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add Parms
                // TownId
                SqlParameter townid = cmd.Parameters.Add("@TownId", SqlDbType.Int);
                if (TownId == 0)
                {
                    townid.Value = null;
                }
                else
                {
                    townid.Value = TownId;
                }

                // District
                SqlParameter district = cmd.Parameters.Add("@District", SqlDbType.Int);
                if (DistrictNumber <= 0)
                {
                    district.Value = null;
                }
                else
                {
                    district.Value = DistrictNumber;
                }

                //// Town
                //SqlParameter townname = cmd.Parameters.Add("@Town", SqlDbType.VarChar);
                //if ( town == null || town.Length < 4)
                //{
                //    townname.Value = null;
                //}
                //else
                //{
                //    townname.Value = town.ToString();
                //}

                

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    try
                    {
                        while (dr.Read())
                        {
                            TownDistrictModel tdm = new TownDistrictModel();
                            tdm.TownId = Convert.ToInt32(dr["TownId"]);
                            tdm.District = Convert.ToInt32(dr["District"]);                             
                            tdm.Town = Convert.ToString(dr["Town"]);

                            TownDistrictList.Add(tdm);
                        }
                    }
                    catch (SqlException ex)
                    {
                        msg = msg + $" {SPGETTOWN}: {ex.Message.ToString()}";
                    }
                    connection.Close();
                }

                return TownDistrictList;
            }
        }

        // Dropdown for towns
        private static List<SelectListItem> PopulateTowns()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            using (SqlConnection connection = new SqlConnection(Startup.cnstr))
            {
                connection.Open();
                
                SqlCommand cmd = new SqlCommand(SPTOWN, connection);
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    try
                    {
                        while (dr.Read())
                        {
                            items.Add(new SelectListItem
                            {
                                Value = dr["TownId"].ToString(),
                                Text = dr["Town"].ToString()
                            });
                        }
                    }
                    catch (SqlException ex)
                    {
                        msg = msg + $" {SPTOWN}: {ex.Message.ToString()}";
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return items;
        }

        // Dropdown for districts
        private static List<SelectListItem> PopulateDistrict()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            using (SqlConnection connection = new SqlConnection(Startup.cnstr))
            {
                connection.Open();                

                SqlCommand cmd = new SqlCommand(SPDISTRICT, connection);
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    try
                    {
                        while (dr.Read())
                        {
                            items.Add(new SelectListItem
                            {
                                Value = dr["District"].ToString(),
                                Text = dr["District"].ToString()
                            });
                        }
                    }
                    catch (SqlException ex)
                    {
                        msg = msg + $" {SPDISTRICT}: {ex.Message.ToString()}";
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return items;
        }

#nullable enable
        public static string UpdateDistrictTowns(DTownDistrictViewModel dtd, int TownId, string sp)
        {
            // If TownId == 0 then it is a insert else update


            using (SqlConnection connection = new SqlConnection(Startup.cnstr))
            {
                string sql = "";
                if (TownId == 0)
                {
                    sql = SPCREATE;
                }
                else
                {
                    sql = SPUPDATE;

                }
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.CommandType = CommandType.StoredProcedure;

                // Town Id 
                // For an update TownId does not change
                if (sp == UPDATE)
                {
                    SqlParameter dtownid = cmd.Parameters.Add("@TownId", SqlDbType.Int);
                    if (TownId == 0 && sp == CREATE)
                    {
                        dtownid.Value = null;
                        cmd.Parameters.Add("@new_id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    }
                    else if (sql == SPUPDATE && TownId > 0)
                    {
                        dtownid.Value = TownId;
                    }
                    else
                    {
                        dtownid.Value = null;
                    }
                }               

                // District
                int districtnumber = Convert.ToInt32(dtd.DistrictSelect);
                SqlParameter district = cmd.Parameters.Add("@District", SqlDbType.Int);

                if (districtnumber > 0)
                {
                    district.Value = (int)districtnumber;
                }
                else
                {
                    district.Value = districtnumber;
                }

                // Town name
                string townname = Convert.ToString(dtd.TownNameSelect);
                SqlParameter dtownname = cmd.Parameters.Add("@Town", SqlDbType.VarChar);

                if (townname == null)
                {
                    dtownname.Value = null;
                }
                else
                {
                    dtownname.Value = townname;
                }

                connection.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                    if (sql == SPCREATE)
                    {
                        msg = cmd.Parameters["@new_id"].Value.ToString();

                    }
                }
                catch (SqlException ex)
                {
                    msg = msg + $" {sql}: {ex.Message.ToString()}";
                }
                finally
                {
                    connection.Close();
                }
            }
            return msg;
        }
    }
}
