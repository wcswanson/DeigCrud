using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using DeigCrud.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DeigCrud.Controllers
{
    public class TownController : Controller
    {
        const string SPUPDATE = "spUpdateTowns";
        const string SPCREATE = "spCreateTown";
        const string SPDELETE = "spDeleteTown";
        const string SPGETTOWN = "spTownsAndDistricts";
        const string SPDISTRICT = "spDistrict";
        const string SPTOWN = "spTowns";
        const string UPDATE = "U";
        const string CREATE = "C";
        const string DELETE = "D";

        int districtnumber = -1;
        string townname = "";
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

                DistrictTownModel = PopulateDistrictsTown(TownId, districtnumber, townname)
            };

            return View(dtdmodel);
        }

        // Post Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(int? DistrictSelect, string? TownSelect)
        {
            // if (ModelState.IsValid)
            //todo: code for null vars coming in.
            //private string _town = TownSelect;
            //private int _district = DistrictSelect;

            var dtdmodel = new DTownDistrictViewModel()
            {
                TownModel = PopulateTowns(),
                DistrictModel = PopulateDistrict(),

                DistrictTownModel = PopulateDistrictsTown(TownId,districtnumber, townname)
            };

            return View(dtdmodel);
        }

        // Create Get 
        [HttpGet]
        //public IActionResult Create()
        //{
        //    var dtdmodel = new DTownDistrictViewModel()
        //    {
        //        TownModel = PopulateTowns(),
        //        DistrictModel = PopulateDistrict(),

        //        DistrictTownModel = PopulateDistrictsTown(TownId, districtnumber, townname)
        //    };
   
        //    return View("Create", dtdmodel);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Update(DlViewModel dlModel)
        //{
        //    int id = Convert.ToInt32(TempData["id"]);
        //    string rc = UpdateList(dlModel, id, SPUPATE);

        //    TempData["id"] = id;
        //    return RedirectToAction("Index");
        //}

        // Functions

        // District and Towns
#nullable enable
        private static List<TownDistrictModel> PopulateDistrictsTown(int? TownId, int? districtnumber, string? town)  
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
                if (districtnumber <= 0)
                {
                    district.Value = null;
                }
                else
                {
                    district.Value = districtnumber;
                }

                // Town
                SqlParameter townname = cmd.Parameters.Add("@Town", SqlDbType.VarChar);
                if ( town == null || town.Length < 4)
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
                                Value = dr["Town"].ToString(),
                                Text = dr["Town"].ToString()
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

//#nullable enable
//        public static string UpdateList( DTownDistrictViewModel dtd, int TownId)
//        {
//            // If TownId == 0 then it is a insert else update

//            using (SqlConnection connection = new SqlConnection(Startup.cnstr))
//            {
//                string sql = "";
//                if (TownId == 0)
//                {
//                    sql = SPCREATE;
//                }
//                else
//                {
//                    sql = UPDATE;

//                }
//                SqlCommand cmd = new SqlCommand(sql, connection);
//                cmd.CommandType = CommandType.StoredProcedure;

//                // Town Id
//                if (sql == SPUPDATE)
//                {
//                    SqlParameter townid = cmd.Parameters.Add("@TownId", SqlDbType.Int);
//                    if (TownId == 0)
//                    {
//                        townid.Value = null;
//                    }
//                    else
//                    {
//                        townid.Value = TownId;
//                    }
//                }
//                else
//                {
//                    cmd.Parameters.Add("@new_id", SqlDbType.Int).Direction = ParameterDirection.Output;
//                }

//                // Distirct
//                int districtnumber = Convert.ToInt32(dtd.DistrictSelect);
//                SqlParameter district = cmd.Parameters.Add("@District", SqlDbType.Int);

//                if (districtnumber > 0 )
//                {
//                    district.Value = (int)districtnumber;
//                }
//                else
//                {
//                    district.Value = null;
//                }

//                // Town name
//                string townname = Convert.ToString(dtd.TownSelect);
//                SqlParameter town = cmd.Parameters.Add("@Town", SqlDbType.VarChar);

//                if (townname == null)
//                {
//                    town.Value = null;
//                }
//                else
//                {
//                    town.Value = townname;
//                }

//                connection.Open();
//                try
//                {
//                    cmd.ExecuteNonQuery();
//                    if (sql == SPCREATE)
//                    {
//                        msg = cmd.Parameters["@new_id"].Value.ToString();

//                    }
//                }
//                catch (SqlException ex)
//                {
//                    msg = msg + $" {sql}: {ex.Message.ToString()}";
//                }
//                finally
//                {
//                    connection.Close();
//                }
//            }
//            return "";
//        }
    }
}
