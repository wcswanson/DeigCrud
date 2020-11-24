using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DeigCrud.Infrastructure
{
    public class HelperFunctions
    {
        const string SPDISTRICT = "spDistrict";
        static string msg = "";

        public static List<SelectListItem> PopulateDistricts()
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

    }


}
