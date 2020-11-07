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
    public class OnlineController : Controller
    {
        const string SPUPATE = "spUpdateOLine";
        const string SPCREATE = "spCreateOLine";
        const string SPDELETE = "spDeleteOnline";
        const string UPDATE = "U";
        const string CREATE = "C";
        const string DELETE = "D";

        static string msg = "";

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create()
        {
            return View();
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

        // Populate the online list
        private static List<OnLineMeetingsModel> PopulateList(int listId, char? b, int? dow, int? timeId, string town, string? sp)
        {
            List<OnLineMeetingsModel>onlineList = new List<OnLineMeetingsModel>();
            return onlineList;
        }

    }
}
