using Oracle.ManagedDataAccess.Client;
using System.Data.SqlClient;


namespace biZTrack.Databases
{
    interface IDatabaseManager
    {
        string getConnectionString();
        bool OpenConnection(string stabase, string user, string password);
        bool OpenConnection();
        void CloseConnection();
        SqlConnection getSqlConnection();
        OracleConnection getOracleConnection();
    }
}
