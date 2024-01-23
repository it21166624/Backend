using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Data.SqlClient;

namespace biZTrack.Databases
{
    class SqlConnectionManager:IDatabaseManager
    {
        DBconnect DBconnect = new DBconnect();
        private static SqlConnection conn = new SqlConnection();
        private static string database = "";
        private static string user = "";
        private static string password = "";
        public string getConnectionString(){
            return "Data Source=dmd-test020;Initial Catalog="+ database + ";User id="+ user + ";Password="+ password + "; MultipleActiveResultSets=true ; Encrypt=False";
        }
        public bool OpenConnection()
        {
            throw new NotImplementedException();
        }
        public bool OpenConnection(string p_database, string p_user, string p_password) {

            if (conn.State != ConnectionState.Open)
            {
                
                try
                {
                    database = p_database;
                    user = p_user;
                    password = p_password;
                    conn.ConnectionString = getConnectionString();
                    conn.Open();
                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
        public void CloseConnection()
        {

            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }
        public SqlConnection getSqlConnection() {
            return conn;
        }
        public OracleConnection getOracleConnection()
        {
            throw new NotImplementedException();
        }

        
    }
}
