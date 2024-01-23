using System.Collections.Generic;
using biZTrack.Interfaces;
using biZTrack.Models;
using Oracle.ManagedDataAccess.Client;

namespace biZTrack.DataAccess
{
    public class DALeave : ILeave
    {
        public Response GetLeaveBalance(string serviceNo, string year)
        {
            Response result = new Response();

            string Query = "SELECT " +
                                "hlv_description, " +
                                "nvl(hlv_allocation, 0) Total, " +
                                "nvl(hlv_leave_taken, 0) Taken, " +
                                "nvl(hlv_balance_leave, 0) Balance " +
                            "FROM " +
                                "hrm_leavebalances_view " +
                            "WHERE " +
                                "hlv_year = '" + year + "' " +
                            "AND " +
                                "hlv_barcode_no = hrmpack.Get_Barcodeno('" + serviceNo + "')";

            DBconnect.connect();

            OracleDataReader odr = DBconnect.readtable(Query);
            EWODetails res = new EWODetails();
            List<LeaveBalanceModel> LeaveBalanceList = new List<LeaveBalanceModel>();
            result.statusCode = 404;
            while (odr.Read())
            {
                LeaveBalanceModel LeaveBalance = new LeaveBalanceModel();
                LeaveBalance.description = odr["hlv_description"].ToString();
                LeaveBalance.total = odr["Total"].ToString();
                LeaveBalance.taken = odr["Taken"].ToString();
                LeaveBalance.balance = odr["Balance"].ToString();
                LeaveBalanceList.Add(LeaveBalance);

                result.statusCode = 200;
            }

            result.resultSet = LeaveBalanceList;
            DBconnect.disconnect();

            return result;
        }
        public Response GetNotEnteredLeave(string serviceNo, string year)
        {
            Response result = new Response();

            string Query = "SELECT " +
                                "TO_CHAR(a.hav_date, 'RRRR-MM-DD') dt, " +
                                "a.hav_days dys, " +
                                "TO_CHAR(l.HAV_CLOCK_IN, 'HH24:MI') clockin, " +
                                "TO_CHAR(l.HAV_CLOCK_out, 'HH24:MI') clockout " +
                            "FROM " +
                                "hrm_automateleave_view a, " +
                                "hrm_attendancecard_view l " +
                            "WHERE " +
                                "a.HAV_BARCODE_NO = l.hav_barcode_no " +
                            "AND " +
                                "TO_CHAR(a.hav_date, 'RRRR-MM-DD') = TO_CHAR(l.hav_date, 'RRRR-MM-DD') " +
                            "AND " +
                                "a.hav_year = '" + year + "' " +
                            "AND " +
                                "a.hav_barcode_no = hrmpack.get_barcodeno('" + serviceNo + "') " +
                            "AND " +
                                "hav_newold = 'A' " +
                            "ORDER BY " +
                                "a.hav_date DESC";

            DBconnect.connect();

            OracleDataReader odr = DBconnect.readtable(Query);
            EWODetails res = new EWODetails();
            List<NotEnteredLeaveModel> NotEnteredLeaveList = new List<NotEnteredLeaveModel>();
            result.statusCode = 404;
            while (odr.Read())
            {
                NotEnteredLeaveModel NotEnteredLeaveBalance = new NotEnteredLeaveModel();
                NotEnteredLeaveBalance.date = odr["dt"].ToString();
                NotEnteredLeaveBalance.day = odr["dys"].ToString();
                NotEnteredLeaveBalance.clockin = odr["clockin"].ToString();
                NotEnteredLeaveBalance.clockout = odr["clockout"].ToString();
                NotEnteredLeaveList.Add(NotEnteredLeaveBalance);

                result.statusCode = 200;
            }

            result.resultSet = NotEnteredLeaveList;
            DBconnect.disconnect();

            return result;
        }
        public Response GetLeaveSummary(string serviceNo, string year)
        {
            Response result = new Response();

            string Query = "SELECT " +
                                "hlv_leaveform_no, " +
                                "hlv_service_no, " +
                                "TO_CHAR(hlv_date, 'RRRR-MM-DD') dte, " +
                                "TO_CHAR(hlv_date, 'DAY') day, " +
                                "hlv_reason, " +
                                "hlv_leave_type, " +
                                "hlv_no_days, " +
                                "TO_CHAR(hlv_approved_date, 'RRRR-MM-DD') ad " +
                           "FROM " +
                                "hrm_leaverecords_view " +
                           "WHERE " +
                                "hlv_service_no = '" + serviceNo + "' " +
                           "AND " +
                                "TO_CHAR(hlv_date, 'RRRR') = '"+year+"' " +
                           "ORDER BY " +
                                "hlv_date DESC";

            DBconnect.connect();

            OracleDataReader odr = DBconnect.readtable(Query);
            EWODetails res = new EWODetails();
            List<LeaveSummaryModel> LeaveSummaryList = new List<LeaveSummaryModel>();
            result.statusCode = 404;
            while (odr.Read())
            {
                LeaveSummaryModel LeaveSummary = new LeaveSummaryModel();
                LeaveSummary.leaveform_no = odr["hlv_leaveform_no"].ToString();
                LeaveSummary.service_no = odr["hlv_service_no"].ToString();
                LeaveSummary.date = odr["dte"].ToString();
                LeaveSummary.day = odr["day"].ToString();
                LeaveSummary.reason = odr["hlv_reason"].ToString();
                LeaveSummary.leave_type = odr["hlv_leave_type"].ToString();
                LeaveSummary.no_days = odr["hlv_no_days"].ToString();
                LeaveSummary.approved_date = odr["ad"].ToString();
                LeaveSummaryList.Add(LeaveSummary);

                result.statusCode = 200;
            }

            result.resultSet = LeaveSummaryList;
            DBconnect.disconnect();

            return result;
        }
        public Response GetPunctuality(string serviceNo, string year)
        {
            Response result = new Response();

            string Query = "SELECT " +
                                "TO_CHAR(hav_date, 'RRRR-MM') mon, " +
                                "hav_rule_type, " +
                                "CASE " +
                                        "WHEN hav_rule_type = 'NAD' " +
                                            "THEN 'Not Attendance Days'  " +
                                        "WHEN hav_rule_type = 'MHD' " +
                                            "THEN 'Morning Half Day'  " +
                                        "WHEN hav_rule_type = 'EHD'  " +
                                            "THEN 'Evening Half Day' " +
                                        "WHEN hav_rule_type = 'MSL'  " +
                                            "THEN 'Morning Short Leave' " +
                                        "WHEN hav_rule_type = 'ESL'  " +
                                            "THEN 'Evening Short Leave' " +
                                        "WHEN hav_rule_type = 'GRS'  " +
                                            "THEN 'Grace Occation' " +
                                        "WHEN hav_rule_type = 'MLT'  " +
                                            "THEN 'Late Occation' " +
                                        "END AS rule_description, " +
                                "COUNT(*) cnt " +
                           "FROM " +
                                "hrm_attendancecard_view " +
                           "WHERE " +
                                "hav_barcode_no = hrmpack.get_barcodeno('" + serviceNo + "') " +
                           "AND " +
                                "TO_CHAR(hav_date, 'RRRR') = '" + year + "' " +
                           "AND " +
                                "hav_rule_type IS NOT NULL " +
                           "AND " +
                                "hav_rule_type NOT IN('NAD', 'MHD', 'EHD') " +
                           "AND " +
                                "hav_newold = 'A' " +
                           "GROUP BY " +
                                "hav_rule_type, " +
                                "TO_CHAR(hav_date, 'RRRR-MM') " +
                           "ORDER BY " +
                                "TO_CHAR(hav_date, 'RRRR-MM')";

            DBconnect.connect();

            OracleDataReader odr = DBconnect.readtable(Query);
            EWODetails res = new EWODetails();
            List<PunctualityModel> PunctualityList = new List<PunctualityModel>();
            result.statusCode = 404;
            while (odr.Read())
            {
                PunctualityModel Punctuality = new PunctualityModel();
                Punctuality.month = odr["mon"].ToString();
                Punctuality.rule_type = odr["hav_rule_type"].ToString();
                Punctuality.rule_description = odr["rule_description"].ToString();
                Punctuality.cnt = odr["cnt"].ToString();
                PunctualityList.Add(Punctuality);

                result.statusCode = 200;
            }

            result.resultSet = PunctualityList;
            DBconnect.disconnect();

            return result;
        }
    }
}
