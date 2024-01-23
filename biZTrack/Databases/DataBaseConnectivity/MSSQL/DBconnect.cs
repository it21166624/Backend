using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace biZTrack.DataBaseConnectivity.MSSQL
{
    class DBconnect { 

     private static SqlConnection conn_new = new SqlConnection();


    internal static bool connectionStatus { get; set; }

    /*
     * Purpose : method to connect to the database
     * Parameter : Constr
     * DataType : String
     * Value : Database Connection String
     * Returns : Connection
     */
    internal static SqlConnection connect(String conStr)
    {
        try
        {
            if (conn_new.State != System.Data.ConnectionState.Open)
            {
                conn_new = new SqlConnection();
                conn_new.ConnectionString = conStr;
                conn_new.Open();
                connectionStatus = true;
            }
            else { /*   return conn_new;  */ }

        }
        catch (SqlException exceptionRead)
        {
            //DevExpress.XtraEditors.XtraMessageBox.Show(exceptionRead.Message, "Sub Contract Work Management", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            //throw;
        }
        return conn_new;
    }


    /*
    * Purpose : method to disconnect connection of database 
    * Parameter : zero parameters
    * DataType : not datatype
    * Value : null
    * Returns : null
    */
    internal static void disconnect()
    {
        conn_new.Close();
    }


    /*
    * Purpose : Read Tables 
    * Parameter : readStr
    * DataType : String
    * Value : Query String
    * Returns : Oracle Data Reader
    */
    internal static SqlDataReader readtable(String readStr)
    {
        SqlDataReader NewOdr = null;
        try
        {
            using (SqlCommand comma = new SqlCommand(readStr, conn_new))
            {
                if (conn_new.State == ConnectionState.Open)
                {
                    NewOdr = comma.ExecuteReader();
                }
            }
        }
        catch (Exception exceptionRead)
        {
            //DevExpress.XtraEditors.XtraMessageBox.Show(exceptionRead.Message, " Sub Contract Work Management", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
        }
        return NewOdr;
    }

    #region Add Edit Delete For Procedures ....

    ///*
    //* Purpose : Insert Update Delete Tables
    //* Parameter : AddEditDelStr
    //* DataType : String
    //* Value : Insert Update Delete Query
    //* Returns : True if success, false if error
    //*/
    //internal static OracleCommand createOracleCommand(String AddEditDelStr, CommandType commandType)
    //{
    //    OracleCommand addComm = new OracleCommand(/*AddEditDelStr, conn_new*/);
    //    addComm.CommandType = commandType;
    //    addComm.Connection = conn_new;
    //    addComm.CommandText = AddEditDelStr;

    //    return addComm;
    //}

    ///*
    //* Purpose : Insert Update Delete Tables
    //* Parameter : AddEditDelStr
    //* DataType : String
    //* Value : Insert Update Delete Query
    //* Returns : True if success, false if error
    //*/
    //internal static Boolean AddEditDelParam(OracleCommand command)
    //{
    //    int a = 0;
    //    try
    //    {
    //        //using (command)
    //        //{
    //        if (conn_new.State == ConnectionState.Open)
    //        {
    //            a = command.ExecuteNonQuery();

    //        }
    //        //}
    //    }
    //    catch (Exception exceptionSave)
    //    {
    //        DevExpress.XtraEditors.XtraMessageBox.Show(exceptionSave.Message, " Sub Contract Work Management", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
    //    }
    //    if (a == 0)
    //        return false;
    //    else// if (a == 1)  **** SELECT Query 1 k damma vita a = -1 wei.....
    //        return true;
    //}

    #endregion

    /*
    * Purpose : Insert Update Delete Tables
    * Parameter : AddEditDelStr
    * DataType : String
    * Value : Insert Update Delete Query
    * Returns : True if success, false if error
    */
    internal static Boolean AddEditDel(String AddEditDelStr)
    {
        int a = 0;
        try
        {
            using (SqlCommand addComm = new SqlCommand(AddEditDelStr, conn_new))
            {
                if (conn_new.State == ConnectionState.Open)
                {
                    a = addComm.ExecuteNonQuery();
                }
            }
        }
        catch (Exception exceptionSave)
        {
            //DevExpress.XtraEditors.XtraMessageBox.Show(exceptionSave.Message, " Sub Contract Work Management", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
        }

        if (a == 0)
            return false;
        else// if (a == 1)  **** SELECT Query 1 k damma vita a = -1 wei.....
            return true;
    }


    /*
   * Purpose : get Record Count
   * Parameter : odr
   * DataType : OracleDataReader
   * Value : DataReader
   * Returns : Number Of records in the given table(given select command)
   */
    internal static double RecCount(SqlDataReader odr)
    {
        double a = 0.0;
        while (odr.Read())
        {
            a++;
        }
        odr.Close();
        return a;
    }

    /*
     * Purpose : Read Table to a data adapter 
     * Parameter : readStr
     * DataType : String
     * Value : Query String
     * Returns : Oracle Data Adapter
     */
    internal static SqlDataAdapter readtableAdap(string readStr)
    {
        SqlDataAdapter NewOdr;

        NewOdr = new SqlDataAdapter(readStr, conn_new);
        return NewOdr;
    }

    internal static void sqlConnectionString_Y()
    {
        //string connectionString = @"Data Source=DMD-ITO029;Initial Catalog=FAM2;User ID=sa;Password=k9cdl@123";
        //string connectionString = @"Data Source=CDPLC-AVMS;Initial Catalog=FAM2;Integrated Security=True";
        //string connectionString = @"Data Source=DMD-TEST020\CDplcsqldb;Initial Catalog=RIMS;User ID=sa;Password=admin@123";
        string connectionString = @"Data Source=DESKTOP-FD5NJ3O;Initial Catalog=BizTrack;User ID=sa;Password=admin@123;MultipleActiveResultSets=True";
        connect(connectionString);
        //
    }

    //internal static void sqlConnectionString_B()
    //{
    //    string connectionString = @"Data Source=cdplc-mssql\cdplcsqldb;Initial Catalog=unis2;User ID=unisuser;Password=unisamho";
    //    connect(connectionString);
    //}



}
}
