
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data.SqlClient;

namespace biZTrack.Databases
{
    class OracleConnectionManager : IDatabaseManager
    {
        private static OracleConnection conn_new = new OracleConnection();
        private static string database = "";
        private static string user = "";
        private static string password = "";
        public string getConnectionString()
        {
            return string.Format("DATA SOURCE="+ database + ";USER ID=3000186;PASSWORD=3000186");
        }
        public bool OpenConnection(string p_database, string p_user, string p_password) {

            if (conn_new.State != System.Data.ConnectionState.Open)
            {
                
                try
                {
                    database = p_database;
                    user = p_user;
                    conn_new = new OracleConnection();
                    conn_new.ConnectionString = getConnectionString();
                    conn_new.Open();
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
        public bool OpenConnection()
        {

            if (conn_new.State != System.Data.ConnectionState.Open)
            {

                try
                {
                    conn_new = new OracleConnection();
                    conn_new.ConnectionString = getConnectionString();
                    conn_new.Open();
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
            conn_new.Close();
        }
        public OracleConnection getOracleConnection() {
            return conn_new;
        }
        public SqlConnection getSqlConnection()
        {
            throw new NotImplementedException();
        }
    }
}
