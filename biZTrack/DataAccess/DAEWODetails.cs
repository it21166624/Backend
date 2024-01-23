using System;
using System.Collections.Generic;
using biZTrack.Databases;
using biZTrack.Factory;
using biZTrack.Interfaces;
using biZTrack.Models;
using Oracle.ManagedDataAccess.Client;

namespace biZTrack.DataAccess
{
    public class DAEWODetails : IEWODetails
    {
        public Response RecieveEWODetails(EWODetails EWODetails) {

            Response res = new Response();

            DBconnect.connect();
            try
            {
                string Query = "INSERT " +
                               "INTO " +
                                    "sms_ewotracking_details (sed_ewo_no," +
                                                             "sed_serial_no," +
                                                             //"sed_doc_type," +
                                                             "sed_received_by," +
                                                             "sed_received_date," +
                                                             "created_by," +
                                                             "created_date," +
                                                             "sed_remarks) " +
                               "VALUES(" +
                                    "'"+ EWODetails.ewo_no + "'," +
                                    "'"+ GetMaxSerial(EWODetails.ewo_no) + "'," +
                                    //"@sed_doc_type," +
                                    "'"+ EWODetails.recieved_by + "'," +
                                    "TO_DATE('" + EWODetails.recieved_date + "','RRRR-MM-DD hh24:mi:ss')," +
                                    "'"+ EWODetails.recieved_by + "'," +
                                    "TO_DATE('" + EWODetails.recieved_date + "','RRRR-MM-DD hh24:mi:ss')," +
                                    "'"+ EWODetails.remarks + "')";

                if (DBconnect.AddEditDel(Query)) {
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
        public Response SendEWODetails(EWODetails EWODetails) {

            Response res = new Response();

            DBconnect.connect();

            try
            {
                string Query = "UPDATE " +
                                    "sms_ewotracking_details " +
                               "SET " +
                                    "updated_by = '" + EWODetails.issued_by + "', " +
                                    "updated_date = TO_DATE('" + EWODetails.issued_date + "','RRRR - MM - DD hh24: mi: ss'), " +
                                    "sed_issued_by = '" + EWODetails.issued_by + "', " +
                                    "sed_issued_date = TO_DATE('"+ EWODetails.issued_date + "','RRRR - MM - DD hh24: mi: ss') " +
                               "WHERE " +
                                    "sed_serial_no = '"+ EWODetails.serial_no + "' " +
                               "AND " +
                                    "sed_ewo_no ='"+ EWODetails.ewo_no + "'";

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
        public Response GetEWODetails(string EWONo) {

            Response result = new Response();

            ConnectionFactory ConFactory = new ConnectionFactory();
            IDatabaseManager DatabaseManager = ConFactory.GetConnection("ORACLE");
            DatabaseManager.OpenConnection("prod19c", "0004086","");

            try
            {
                OracleCommand command;

                string Query = "SELECT " +
                                    "sed_ewo_no, " +
                                    "sed_wdreceive_status, " +
                                    "medpack.get_vname(sed_authorized_by) authorized_by, " +
                                    "medpack.get_vname(pep_authorized_by) authorized_person, " +
                                    "sed_ewo_status, " +
                                    "sed_wdestimated_amount, " +
                                    "sed_billed_amount, " +
                                    "medpack.get_vname(sed_evaluation_by) evaluation_by " +
                               "FROM " +
                                    "sms_EWO_details " +
                               "LEFT JOIN " +
                                    "pms_ewoauthorized_person " +
                               "ON " +
                               "sed_ewo_no = pep_ewo_no " +
                               "WHERE " +
                                    "sed_ewo_no='" + EWONo + "'";

                command = new OracleCommand(Query, DatabaseManager.getOracleConnection());
                EWODetails res = new EWODetails();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        res.ewo_no = reader["sed_ewo_no"].ToString();
                        res.authorize_by = reader["authorized_by"].ToString();
                        res.authorize_person = reader["authorized_person"].ToString();
                        res.WD_status = reader["sed_wdreceive_status"].ToString()!=""?"Yes":"No";

                        if (reader["sed_ewo_status"].ToString() == "P"){
                            res.ewo_status = "Pending";
                            res.status_bckcolor = "#00BFFF";
                            res.status_txtcolor = "#000000";
                        }

                        else if (reader["sed_ewo_status"].ToString() == "D"){
                            res.ewo_status = "Job Cancelled";
                            res.status_bckcolor = "#000000";
                            res.status_txtcolor = "#ffffff";
                        }

                        else if (reader["sed_ewo_status"].ToString() == "C"){
                            res.ewo_status = "Forwarded for Costing";
                            res.status_bckcolor = "#FFFF00";
                            res.status_txtcolor = "#000000";
                        }

                        else if (reader["sed_ewo_status"].ToString() == "E"){
                            res.ewo_status = "Costing Completed";
                            res.status_bckcolor = "#00FFFF";
                            res.status_txtcolor = "#000000";
                        }

                        else if (reader["sed_ewo_status"].ToString() == "W"){
                            res.ewo_status = "Work Done Submitted";
                            res.status_bckcolor = "#005A9C";
                            res.status_txtcolor = "#ffffff";
                        }

                        else if (reader["sed_ewo_status"].ToString() == "V"){
                            res.ewo_status = "Evaluation Completed";
                            res.status_bckcolor = "#0000FF";
                            res.status_txtcolor = "#ffffff";
                        }

                        else if (reader["sed_ewo_status"].ToString() == "B"){
                            res.ewo_status = "Forward for Billing";
                            res.status_bckcolor = "#00008B";
                            res.status_txtcolor = "#ffffff";
                        }

                        else if (reader["sed_ewo_status"].ToString() == "A"){
                            res.ewo_status = "Accepted by S.Procurement";
                            res.status_bckcolor = "#013220";
                            res.status_txtcolor = "#ffffff";
                        }

                        else if (reader["sed_ewo_status"].ToString() == "R"){
                            res.ewo_status = "Returned by S.Procurement";
                            res.status_bckcolor = "#ffa500";
                            res.status_txtcolor = "#000000";
                        }

                        else if (reader["sed_ewo_status"].ToString() == "F"){
                            res.ewo_status = "Sent to Finance";
                            res.status_bckcolor = "#013220";
                            res.status_txtcolor = "#ffffff";
                        }

                        else if (reader["sed_ewo_status"].ToString() == "I"){
                            res.ewo_status = "Billing in Progress";
                            res.status_bckcolor = "#00ff00";
                            res.status_txtcolor = "#ffffff";
                        }
                        
                        res.estimated_amount = (reader["sed_wdestimated_amount"].ToString() != "" ? double.Parse(reader["sed_wdestimated_amount"].ToString()):0.00);
                        res.billed_amount = (reader["sed_billed_amount"].ToString() != "" ? double.Parse(reader["sed_billed_amount"].ToString()):0.00);
                        res.evaluation_by = reader["evaluation_by"].ToString();

                        EWOTrackingDetails TrackingDetails = new EWOTrackingDetails();
                        TrackingDetails = getEWOTrackingDetails(EWONo);
                        res.recieved_by = TrackingDetails.recieved_by;
                        res.recieved_date = TrackingDetails.recieved_date.ToString("yyyy-MM-dd hh:mm:ss tt");
                        res.issued_by = TrackingDetails.issued_by;
                        res.issued_date = TrackingDetails.issued_date.ToString("yyyy-MM-dd hh:mm:ss tt");
                        res.serial_no = TrackingDetails.serial_no; 
                        result.resultSet = res;
                        result.statusCode = 200;
                    }
                    else
                        result.statusCode = 404;
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
        public int GetMaxSerial(string EWOno)
        {
            int MaxSerial = 0;
            string Query = "SELECT MAX(sed_serial_no) AS MaxSerial " +
                           "FROM sms_ewotracking_details " +
                           "WHERE sed_ewo_no = '" + EWOno + "'";
            DBconnect.connect();
            OracleDataReader odr = DBconnect.readtable(Query);
            if (odr.Read())
            {
                MaxSerial = odr["MaxSerial"].ToString()!=""? int.Parse(odr["MaxSerial"].ToString()) : 0;
            }
            return MaxSerial+1;
        }
        public EWOTrackingDetails getEWOTrackingDetails(string ewoNo)
        {
            string Query = "SELECT sed_received_by, " +
                                  "sed_serial_no, " +
                                  "medpack.get_vname(sed_received_by) received_by_name, " +
                                  "sed_received_date, " +
                                  "sed_issued_by, " +
                                  "sed_issued_date " +
                           "FROM sms_ewotracking_details " +
                           "WHERE sed_serial_no = (SELECT MAX(sed_serial_no) FROM sms_ewotracking_details WHERE sed_ewo_no ='" + ewoNo + "') " +
                           "AND sed_ewo_no ='"+ewoNo+"'";

            DBconnect.connect();

            OracleDataReader odr = DBconnect.readtable(Query);
            EWODetails res = new EWODetails();
            EWOTrackingDetails trackingDetails = new EWOTrackingDetails();

            if (odr.Read())
            {
                trackingDetails.recieved_by = odr["sed_received_by"].ToString();
                trackingDetails.recieved_by_name = odr["received_by_name"].ToString();
                if (odr["sed_received_date"].ToString() != "")
                    trackingDetails.recieved_date = DateTime.Parse(odr["sed_received_date"].ToString());
                
                trackingDetails.issued_by = odr["sed_issued_by"].ToString();
                if (odr["sed_issued_date"].ToString() != "")
                    trackingDetails.issued_date = DateTime.Parse(odr["sed_issued_date"].ToString());

                trackingDetails.serial_no = odr["sed_serial_no"].ToString();
            }

            DBconnect.disconnect();
            return trackingDetails;
        }
            }
}
