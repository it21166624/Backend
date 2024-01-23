using System;
using System.Collections.Generic;
using biZTrack.Databases;
using biZTrack.Factory;
using biZTrack.Interfaces;
using biZTrack.Models;
using biZTrack.Static;
using System.Data.SqlClient;

namespace biZTrack.DataAccess
{
    public class DAAccess : IAccess
    {
        public Response GetAccessHeadComponent(string encryptKey)
        {
            Response result = new Response();

            ConnectionFactory ConFactory = new ConnectionFactory();
            IDatabaseManager DatabaseManager = ConFactory.GetConnection("MSSQL");
            DatabaseManager.OpenConnection("BizTrack", "biztrackuser", "biztrack123");

            List<AccessHeadComponents> componentArray = new List<AccessHeadComponents>();
            try
            {
                SqlCommand command;

                string Query = "SELECT " +
                                    "sum_component_code, " +
                                    "sum_component_description " +
                               "FROM " +
                                    "sms_access_control ac, " +
                                    "sms_uicomponent c " +
                               "WHERE ac.sac_service_no= " + StaticClass.DecryptStringAES(encryptKey) + " " +
                               "AND c.sum_head_component_code = 0 " +
                               "AND c.sum_component_code=ac.sac_component_code";

                command = new SqlCommand(Query, DatabaseManager.getSqlConnection());
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    result.statusCode = 404;
                    while (reader.Read())
                    {
                        AccessHeadComponents empObj = new AccessHeadComponents();
                        empObj.code = reader["sum_component_code"].ToString();
                        empObj.description = reader["sum_component_description"].ToString();
                        componentArray.Add(empObj);
                        result.statusCode = 200;
                    }
                    result.resultSet = componentArray;
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                result.statusCode = 500;
                result.result = ex.Message;
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            DatabaseManager.CloseConnection();

            return result;
        }
        public Response GetAccessSubComponent(string encryptKey, string head_comp_id)
        {
            Response result = new Response();
            ConnectionFactory ConFactory = new ConnectionFactory();
            IDatabaseManager DatabaseManager = ConFactory.GetConnection("MSSQL");
            DatabaseManager.OpenConnection("BizTrack", "biztrackuser", "biztrack123");
            List<AccessHeadComponents> componentArray = new List<AccessHeadComponents>();

            try
            {
                SqlCommand command;

                string Query = "SELECT " +
                                    "sum_component_code, " +
                                    "sum_component_description " +
                               "FROM " +
                                    "sms_access_control ac, " +
                                    "sms_uicomponent c " +
                               "WHERE ac.sac_service_no= " + StaticClass.DecryptStringAES(encryptKey) + " " +
                               "AND c.sum_head_component_code = " + head_comp_id + " " +
                               "AND c.sum_component_code=ac.sac_component_code";

                command = new SqlCommand(Query, DatabaseManager.getSqlConnection());
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    result.statusCode = 404;
                    while (reader.Read())
                    {
                        AccessHeadComponents empObj = new AccessHeadComponents();
                        empObj.code = reader["sum_component_code"].ToString();
                        empObj.description = reader["sum_component_description"].ToString();
                        componentArray.Add(empObj);
                        result.statusCode = 200;
                    }
                    result.resultSet = componentArray; 
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                result.statusCode = 500;
                result.result = ex.Message;
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            DatabaseManager.CloseConnection();

            return result;
        }
        public Response GetAccessComponentList()
        {
            Response result = new Response();
            ConnectionFactory ConFactory = new ConnectionFactory();
            IDatabaseManager DatabaseManager = ConFactory.GetConnection("MSSQL");
            DatabaseManager.OpenConnection("BizTrack", "biztrackuser", "biztrack123");

            List<AccessComponentsArray> componentArray = new List<AccessComponentsArray>();

            try
            {
                SqlCommand command;

                string Query = "SELECT * " +
                               "FROM " +
                                    "sms_uicomponent " +
                               "WHERE " +
                                    "sum_head_component_code = 0";

                command = new SqlCommand(Query, DatabaseManager.getSqlConnection());
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    result.statusCode = 404;
                    while (reader.Read())
                    {
                        AccessComponentsArray empObj = new AccessComponentsArray();
                        empObj.code = reader["sum_component_code"].ToString();
                        empObj.description = reader["sum_component_description"].ToString();
                        componentArray.Add(empObj);
                        result.statusCode = 500;
                    }
                    reader.Close();
                }

                foreach (var headComponent in componentArray)
                {
                    List<AccessHeadComponents> subComponentArray = new List<AccessHeadComponents>();

                    string query = "SELECT * " +
                                   "FROM " +
                                        "sms_uicomponent " +
                                   "WHERE " +
                                        "sum_head_component_code = " + headComponent.code;

                    command = new SqlCommand(query, DatabaseManager.getSqlConnection());

                    using (SqlDataReader reader1 = command.ExecuteReader())
                    {
                        while (reader1.Read())
                        {
                            AccessHeadComponents Obj = new AccessHeadComponents();
                            Obj.code = reader1["sum_component_code"].ToString();
                            Obj.description = reader1["sum_component_description"].ToString();
                            subComponentArray.Add(Obj);
                        }
                        reader1.Close();
                    }

                    headComponent.subComponents = subComponentArray;
                }
                result.resultSet = componentArray;
            }
            catch (Exception ex)
            {
                result.statusCode = 500;
                result.result = ex.Message;
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return result;
        }

    }
}
