using System;
using biZTrack.Databases;
using biZTrack.Factory;
using biZTrack.Interfaces;
using System.Data.SqlClient;

namespace biZTrack.DataAccess
{
    public class DAAuthenticate : IAuthenticate
    {
        public bool Authenticate(string phone_no)
        {
            bool res = false;

            ConnectionFactory ConFactory = new ConnectionFactory();
            IDatabaseManager DatabaseManager = ConFactory.GetConnection("MSSQL");
            DatabaseManager.OpenConnection("BizTrack", "biztrackuser", "biztrack123");

            try
            {
                SqlCommand command;
                
                string Query = "SELECT * " +
                               "FROM " +
                                    "sms_employee_details " +
                               "WHERE " +
                                    "sed_phone_no='" + phone_no +"'";

                command = new SqlCommand(Query, DatabaseManager.getSqlConnection());
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        res = true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            
            return res;
        }
       
    }
}
