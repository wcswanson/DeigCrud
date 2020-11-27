using DeigCrud.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DeigCrud.Infrastructure
{
    public class OnlineHelpers
    {
        const string SPUPDATE = "spUpdateOnlineList";
        const string SPCREATE = "spCreateOnlineList";
        const string SPDELETE = "spDeleteOnlineList";
        const string SPGETONLINE = "spGetOnlineList";
        const string DELETE = "D";

        int ZoomId = 0;
        int dayId = 0;
        int timeId = 0;
        static string msg = "";
       
#nullable enable
        // Update List
        public static string UpdateOnlineList(DoViewModel dol, int id, string sp)
        {
            using (SqlConnection connection = new SqlConnection(Startup.cnstr))
            {
                SqlCommand cmd = new SqlCommand(sp, connection);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add Parms                
                    if (id == 0)
                    {
                        cmd.Parameters.Add("@new_id", SqlDbType.Int).Direction = ParameterDirection.Output;
                       
                    }
                    else
                    {
                        SqlParameter zoomid = cmd.Parameters.Add("@ZoomId", SqlDbType.Int);
                        zoomid.Value = id;

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

                // Time Id
                int districtnumber = Convert.ToInt32(dol.DistrictSelect);
                SqlParameter district = cmd.Parameters.Add("@District", SqlDbType.Int);
                if (districtnumber > 0)
                {
                    district.Value = (int)districtnumber;
                }
                else
                {
                    district.Value =  -1;
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
        public static List<OnlineMeetingsModel> PopulateOnlineList(int? ZoomId, int? DayId, int? TimeId, int? DistrictNumber)
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

                // District                
                SqlParameter districtnumber = cmd.Parameters.Add("@District", SqlDbType.Int);
                if (DistrictNumber == 0)
                {
                    districtnumber.Value = null;
                }
                else
                {
                    districtnumber.Value = DistrictNumber;
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
                            ol.District = Convert.ToInt32(dr["district"]);
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
        public static string DeleteFunction(int ZoomId)
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
