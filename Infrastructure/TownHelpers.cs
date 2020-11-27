using DeigCrud.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DeigCrud.Infrastructure
{
    public class TownHelpers
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
        const string ERR = "err";

        int districtnumber = -1;
        string townname = "";
        public static string msg = "";

        //District and Towns
#nullable enable
        public static List<TownDistrictModel> PopulateDistrictsTown(int? TownId, int? DistrictNumber)
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

#nullable enable
        public static string UpdateDistrictTowns(DTownDistrictViewModel dtd, int TownId)
        {
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

                // Town Id == o Create else update
                if (sql == SPCREATE)
                {
                    cmd.Parameters.Add("@new_id", SqlDbType.Int).Direction = ParameterDirection.Output;
                }
                else
                {
                    SqlParameter dtownid = cmd.Parameters.Add("@TownId", SqlDbType.Int);
                    dtownid.Value = TownId;
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

        // Delete
        public static string DeleteTown(int id)
        {

            int TownId = 0;
            if (id > 0)
            {
                TownId = Convert.ToInt32(id);
            }
            else
            {
                // ViewBag error msg
                msg = "err"; // View("Index");
            }

            if (msg != ERR)
            {
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
                }
            }
            return msg;
        }
    }
}
