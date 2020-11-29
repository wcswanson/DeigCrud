using DeigCrud.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DeigCrud.Infrastructure
{
    public class PhysicalHelpers
    {
      
        //const string SPCREATE = "spCreateList";
        public const string SPDELETELIST = "spDeleteList";
        //const string SPDISTRICT = "spDistrict";
        //const string SPUPDATE = "spUpdate";
        public const string SPUPDATELIST = "spUpdateList";
        public const string SPCREATE = "spCreateList";

        public const string UPDATE = "U";
        public const string CREATE = "C";
        public const string DELETE = "D";

        //static int listId = 0;
        //static char b = 'a';
        //static int dayId = 0;
        //static int timeId = 0;
        //static string town = "";
        //static string msg = "";
        //static string sp = "";
        static int districtNumber = -1;
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
                //string sql = "spTowns";

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
                                Value = dr["Town"].ToString(),
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
                //string sql = "spDOW";

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
                //string sql = "spTime";

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

#nullable enable
        public static List<MeetingListModel> PopulateList(int listId, char? b, int? dow, int? timeId, string town, string? sp, int? districtnumber)  // remove sp?
        {

            const string SPMAINTENANCELIST = "spMaintenanceList";
            List<MeetingListModel> meetingList = new List<MeetingListModel>();
            using (SqlConnection connection = new SqlConnection(Startup.cnstr))
            {
                connection.Open();

                // string sql = "spMaintenanceList*/";
                SqlCommand cmd = new SqlCommand(SPMAINTENANCELIST, connection);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add Parms
                // ListId
                SqlParameter listid = cmd.Parameters.Add("@ListId", SqlDbType.Int);
                if (listId == 0)
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

                // District
                SqlParameter district = cmd.Parameters.Add("@District", SqlDbType.Int);
                if (districtnumber > 0)
                {
                    district.Value = (int)districtnumber;
                }
                else
                {
                    district.Value = null;
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
                            ml.District = Convert.ToInt32(dr["District"]);
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
                        msg = msg + $" {SPMAINTENANCELIST}: {ex.Message.ToString()}";
                    }
                    connection.Close();
                }

                return meetingList;
            }
        }

#nullable enable
        // Update List
        public static string UpdateList(DlViewModel dl, int id, string sp)  // Remove sp from signature?
        {
            using (SqlConnection connection = new SqlConnection(Startup.cnstr))
            {
                string sql = sp;    // This is the only thing that needs to bechanged to do inserts. Add a constant for the procedure name and pass it in to this function.
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add Parms
                // ListId
                if (sp == SPUPDATELIST)
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

                // District
                int districtnumber = Convert.ToInt32(dl.DistrictSelect);
                SqlParameter district = cmd.Parameters.Add("@District", SqlDbType.Int);
                if (districtnumber > 0)
                {
                    district.Value = (int)districtnumber;
                }
                else
                {
                    district.Value = 0;
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
                SqlParameter location = cmd.Parameters.Add("@Location", SqlDbType.VarChar);
                if (String.IsNullOrEmpty(dl.LocationSelect))
                {
                    location.Value = "";
                }
                else
                {
                    location.Value = dl.LocationSelect.ToString();
                }

                // Type
                SqlParameter type = cmd.Parameters.Add("@Type", SqlDbType.VarChar);
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
                    if (sp == SPCREATE)
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
    }
}
