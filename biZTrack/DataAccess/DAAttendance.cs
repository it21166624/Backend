using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using biZTrack.Databases;
using biZTrack.Factory;
using biZTrack.Interfaces;
using biZTrack.Models;
using Oracle.ManagedDataAccess.Client;

namespace biZTrack.DataAccess
{
    public class DAAttendance : IAttendance
    {
        public Response GetSupervisorList(string serviceNo)
        {
            Response result = new Response();

            ConnectionFactory ConFactory = new ConnectionFactory();
            IDatabaseManager DatabaseManager = ConFactory.GetConnection("ORACLE");
            DatabaseManager.OpenConnection("DTS", serviceNo, "");
            
            try
            {
                OracleCommand command;

                string Query = "SELECT " +
                                    "hev_service_no, " +
                                    "hev_report_name  " +
                               "FROM " +
                                    "hrm_employee_details_view " +
                               "WHERE " +
                                    "hev_newold = 'A' " +
                               "AND hev_work_category = 'S' " +
                               "AND hev_division_code = (select hev_division_code from hrm_employee_details_view where hev_service_no = '" + serviceNo + "')";

                command = new OracleCommand(Query, DatabaseManager.getOracleConnection());
                List<SupervisorDetails> SupervisorDetailsList = new List<SupervisorDetails>();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    result.statusCode = 404;
                    while (reader.Read())
                    {
                        SupervisorDetails SupervisorDetails = new SupervisorDetails();
                        SupervisorDetails.reportName = reader["hev_report_name"].ToString();
                        SupervisorDetails.serviceNo = reader["hev_service_no"].ToString();

                        SupervisorDetailsList.Add(SupervisorDetails);

                        result.statusCode = 200;
                    }

                    result.resultSet = SupervisorDetailsList;
                }
            }
            catch (Exception ex)
            {

                result.statusCode = 500;
                result.result = ex.Message;
            }
            DatabaseManager.CloseConnection();
            return result;
        }
        public Response getEmpByPhone(string phoneNo)
        {
            Response result = new Response();

            ConnectionFactory ConFactory = new ConnectionFactory();
            IDatabaseManager DatabaseManager = ConFactory.GetConnection("MSSQL");
            DatabaseManager.OpenConnection("DTS_ATTEND", "DTSUSER", "Rayan@123");

            try
            {
                SqlCommand command;

                string Query = "SELECT * FROM employee_details WHERE ced_mobile_no='" + phoneNo + "'";

                command = new SqlCommand(Query, DatabaseManager.getSqlConnection());
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result.statusCode = 200;
                        string serviceNo = String.Format("{0}", reader["ced_service_no"]);
                        string userName = String.Format("{0}", reader["ced_report_name"]);
                        string company = String.Format("{0}", reader["company"]);
                        string location = String.Format("{0}", reader["location"]);
                        
                        if (serviceNo != "")
                        {
                            result.resultSet = serviceNo + "/" + userName + "/" + company + "/" + location;
                        }
                    }
                    else
                        result.statusCode = 404;
                }
                
            }
            catch (Exception ex)
            {
                result.statusCode = 500;
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            DatabaseManager.CloseConnection();
            return result;
        }
        public Response PostAttendance(AttendanceModel attendance)
        {
            Response res = new Response();

            DBconnect.connect();
            try
            {
                string Query = "INSERT INTO hrm_barcode_times " +
                                          "(hbt_barcode_no," +
                                           "hbt_clock_no," +
                                           "hbt_barcode_date," +
                                           "hbt_employee_type," +
                                           "hbt_file_name," +
                                           "hbt_barcode_script," +
                                           "created_by," +
                                           "created_date," +
                                           "hbt_inout_status) " +
                               "VALUES('" + attendance.serviceNo.Remove(0,2) + "'," +
                                            "'01'," +
                                            "to_date('" + Convert.ToDateTime(attendance.dateTime).ToString("yyyy-MM-dd HH:mm ss") + "','RRRR-MM-DD HH24:MI SS')," +
                                            "'0'," +
                                            "'" + Convert.ToDateTime(attendance.dateTime).ToString("yyyyMMdd") + "'," +
                                            "'01" + Convert.ToDateTime(attendance.dateTime).ToString("ddMMyyyyHHmm") + "0" + attendance.serviceNo.Remove(0, 2) + "'," +
                                            "user," +
                                            "sysdate," +
                                            "'" + attendance.InOut + "')";

                if (DBconnect.AddEditDel(Query))
                {
                    res.statusCode = 200;
                    res.result = "Success!!";
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                res.statusCode = 500;
                res.result = "Failed!!";
            }
            DBconnect.disconnect();
            return res;
        }
        public Response GetAttendanceCard(string serviceNo, string month)
        {
            Response result = new Response();

            string Query = "SELECT TO_CHAR(hav_date, 'RRRR-MM-DD') AS dte, " +
                                   "hav_day, " +
                                   "TO_CHAR(hav_clock_in, 'HH24:MI') AS inx, " +
                                   "TO_CHAR(hav_clock_out, 'HH24:MI') AS outx, " +
                                   "hav_continued_status, " +
                                   "DECODE(hav_leave_type, 'CL', 'Casual', 'AS', 'Annual', 'AL', 'Annual', 'SL', 'Sick', 'SK', 'Sick', 'DO', 'Day Off', 'DL', 'Duty', 'TB', 'TB Leave', 'SD', 'Shift Day-Off', 'MT', 'MT Leave', 'SP', 'Special', 'AC', 'Accident', 'SS', 'Suspension', 'IN', 'Interdiction', 'NE', 'Not Entered', 'NP', 'No Pay', 'NL', 'Not Approved', 'CN', 'Cancelled') AS lt, " +
                                   "hav_leave_days " +
                           "FROM " +
                                "hrm_attendancecard_view " +
                           "WHERE " +
                                "hav_barcode_no = hrmpack.get_barcodeno('" + serviceNo + "') " +
                           "AND " +
                                "TO_CHAR(hav_date, 'RRRR-MM') = '"+ month + "' " +
                           "AND hav_newold = 'A'";

            DBconnect.connect();

            OracleDataReader odr = DBconnect.readtable(Query);
            EWODetails res = new EWODetails();
            List<AttendanceCardModel> PunctualityList = new List<AttendanceCardModel>();
            result.statusCode = 404;
            while (odr.Read())
            {
                AttendanceCardModel AttendanceCard = new AttendanceCardModel();
                AttendanceCard.date = odr["dte"].ToString();
                AttendanceCard.day = odr["hav_day"].ToString();
                AttendanceCard.in_time = odr["inx"].ToString();
                AttendanceCard.out_time = odr["outx"].ToString();
                AttendanceCard.continued_status = odr["hav_continued_status"].ToString();
                AttendanceCard.leave_type = odr["lt"].ToString();
                AttendanceCard.leave_days = odr["hav_leave_days"].ToString();
                PunctualityList.Add(AttendanceCard);

                result.statusCode = 200;
            }

            result.resultSet = PunctualityList;
            DBconnect.disconnect();

            return result;
        }
    }
}
