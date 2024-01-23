using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using biZTrack.Databases;

namespace biZTrack.Factory
{
    class ConnectionFactory
    {
        public IDatabaseManager GetConnection(string conType) {

            IDatabaseManager returnValue = null;

            if (conType == "MSSQL")
                returnValue = new SqlConnectionManager();

            else if(conType == "ORACLE")
                returnValue = new OracleConnectionManager();

            return returnValue;
        }
    }
}
