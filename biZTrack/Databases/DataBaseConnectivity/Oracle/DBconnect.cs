/**
 * Class : connect
 * Created Date : 2011/08/02
 * Creatred : CDPLC ITTEAM
 * Purpose : Connection class for the Oracle DataBase
 * Req : Oracle.DataAccess.dll
 * */


using Oracle.ManagedDataAccess.Client;
using System;


public class DBconnect
{
    public static OracleConnection conn_new = new OracleConnection();

    /**
     * Purpose : method to connect to the database
     * Parameter : Constr
     * DataType : String
     * Value : Database Connection String
     * Returns : Connection
     * */

    public static OracleConnection connect(string str)
    {
        var conStr = str;

        try
        {
            if (conn_new.State != System.Data.ConnectionState.Open)
            {
                conn_new.ConnectionString = conStr;
                conn_new.Open();
            }
            else
            {
                return conn_new;
            }

            return conn_new;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    //Overload connect method
    public static bool connect()
    {
        var conStr = string.Format("DATA SOURCE={0};USER ID={1};PASSWORD={2}", "prod19c", "0004086", "0004086");

        if (conn_new.State != System.Data.ConnectionState.Open)
        {
            conn_new.ConnectionString = conStr;
            try
            {
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

    /**
     * Purpose : method to disconnect to the database
     * Returns : Connection
     * */

    public static OracleConnection disconnect()
    {
        try
        {
            if (conn_new.State == System.Data.ConnectionState.Open)
            {
                conn_new.Close();
                OracleConnection.ClearPool(conn_new);
            }
            else
            {
                return conn_new;
            }

            return conn_new;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    /**
    * Purpose : Read Tables 
    * Parameter : readStr
    * DataType : String
    * Value : Query String
     * Returns : Oracle Data Reader
    * */

    public static OracleDataReader readtable(String readStr)
    {
        var comma = new OracleCommand(readStr, conn_new);
        var NewOdr = comma.ExecuteReader();
        return NewOdr;
    }

    /** 
   * Purpose : Read Table to a data adapter 
   * Parameter : readStr                        
   * DataType : String
   * Value : Query String
    * Returns : Oracle Data Adapter
   * */
    public static OracleDataAdapter readtable2(String readStr)
    {
        var NewOdr = new OracleDataAdapter(readStr, conn_new);
        return NewOdr;
    }

    /**
    * Purpose : Insert Update Delete Tables
    * Parameter : AddEditDelStr
    * DataType : String
    * Value : Insert Update Delete Query
    * Returns : True if success false if error
    * */
    public static Boolean AddEditDel(String AddEditDelStr)
    {
        var addComm = new OracleCommand(AddEditDelStr, conn_new);
        var a = addComm.ExecuteNonQuery();
        if (a == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
        DBconnect.conn_new.Close();
    }

    /**
   * Purpose : get Record Count
   * Parameter : odr
   * DataType : OracleDataReader
   * Value : DataReader
   * Returns : Number Of records in the given table(given select command)
   * */
    public static double RecCount(OracleDataReader odr)
    {
        var a = 0.0d;
        while (odr.Read())
        {
            a++;
        }
        odr.Close();
        return a;
    }
}



