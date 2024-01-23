using System;
using System.Data.SqlClient;
using biZTrack.Databases;
using biZTrack.Factory;
using biZTrack.Interfaces;
using biZTrack.Models;

namespace biZTrack.DataAccess
{
    public class DALogin : ILogin
    {
        public Response LoginByPhone(string phoneNo) {
            Response result = new Response();
            UserModel res = new UserModel();

            ConnectionFactory ConFactory = new ConnectionFactory();
            IDatabaseManager DatabaseManager = ConFactory.GetConnection("MSSQL");
            DatabaseManager.OpenConnection("BizTrack", "biztrackuser", "biztrack123");

            try
            {
                SqlCommand command;

                string Query = "SELECT " +
                                    "sed_service_no, " +
                                    "sed_phone_no, " +
                                    "sed_first_name, " +
                                    "sed_last_name " +
                               "FROM " +
                                    "sms_employee_details " +
                               "WHERE " +
                                    "sed_phone_no='" + phoneNo + "'";

                command = new SqlCommand(Query, DatabaseManager.getSqlConnection());
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result.statusCode = 200;
                        res.fName = reader["sed_first_name"].ToString();
                        res.lName = reader["sed_last_name"].ToString();
                        res.phoneNo = reader["sed_phone_no"].ToString();
                        res.serviceNo = reader["sed_service_no"].ToString();
                        result.resultSet = res;
                    }
                    else
                        result.statusCode = 404;
                }
            }
            catch (Exception ex)
            {
                result.result = ex.Message;
                result.statusCode = 500;
            }
            
            return result;

        }
    }
}
