using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

using SuperLogger.Helper;

namespace SuperLogger.Model
{
    public class SuperLoggerDbModel
    {
        private string SQL_CONNECTION_STRING = "Superlog.SQL.ConnectionString";

        private SqlConnection sqlConnection { set; get; }

        public SuperLoggerDbModel()
        {
            OpenSqlConnection();
        }

        private void OpenSqlConnection()
        {
            sqlConnection = new SqlConnection(SuperLoggerHelper.GetAppSettings(SQL_CONNECTION_STRING));
        }
    }
}
