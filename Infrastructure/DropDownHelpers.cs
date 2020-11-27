using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DeigCrud.Infrastructure
{
    public class DropDownHelpers
    {
        public static string msg = "";
        public static List<SelectListItem> PopulateDistricts()
        {
            const string SPDISTRICT = "spDistrict";
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

        // Towns
        public static List<SelectListItem> PopulateTowns()
        {
            // Get the towns from the table towns and not the distinct towns from Lists
            const string SPTOWNS = "spTableTowns";
            List<SelectListItem> items = new List<SelectListItem>();

            using (SqlConnection connection = new SqlConnection(Startup.cnstr))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand(SPTOWNS, connection);
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
                        msg = msg + $" s{SPTOWNS}: {ex.Message.ToString()}";
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
        public static List<SelectListItem> PopulateDOW()
        {
            const string SPDOW = "spDOW";
            List<SelectListItem> items = new List<SelectListItem>();

            using (SqlConnection connection = new SqlConnection(Startup.cnstr))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand(SPDOW, connection);
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
                        msg = msg + $" {SPDOW}: {ex.Message.ToString()} ";
                    }
                }
                connection.Close();
            }
            return items;
        }

        // Time
        public static List<SelectListItem> PopulateTime()
        {
            //TOD: Add 12:15 & 5:15 to time table
            const string SPTIME = "spTime";
            List<SelectListItem> items = new List<SelectListItem>();

            using (SqlConnection connection = new SqlConnection(Startup.cnstr))
            {
                connection.Open();              

                SqlCommand cmd = new SqlCommand(SPTIME, connection);
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
                        msg = $"{SPTIME}: {ex.Message.ToString()}";
                    }

                }
                connection.Close();
            }
            return items;
        }
    }
}
