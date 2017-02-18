using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

using SuperLogger.Helper;
using System.Transactions;

namespace SuperLogger.Model
{
    public class SuperLoggerDbModel : IDisposable
    {
        private SqlConnection _sqlConnection { set; get; }

        public SuperLoggerDbModel()
        {
            OpenSqlConnection();
        }

        private void OpenSqlConnection()
        {
            try
            {
                if (_sqlConnection == null || _sqlConnection.State != System.Data.ConnectionState.Open)
                {
                    string connectionString = SuperLoggerHelper.GetAppSettings(SuperLoggerHelper.SQL_CONNECTION_STRING);
                    _sqlConnection = new SqlConnection(connectionString);
                    _sqlConnection.Open();
                }
            } catch (Exception ex)
            {
                throw new Exception(string.Format("Error Opening the SQL connection: {0}", ex.Message));
            }
        }

        private void CloseSqlConnection()
        {
            try
            {
                if (_sqlConnection.State != System.Data.ConnectionState.Closed)
                {
                    _sqlConnection.Close();
                }
            }
            catch { }
        }

        public void AddLogEntry(string source, string type, string correlationId, DateTime createdOn,
                                string message, string stackTrace, IDictionary<string, string> data = null)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                int logEnteyId = ExecuteProcedureAddLogEntry(source, type, correlationId, createdOn, message, stackTrace);

                if (data != null)
                {
                    foreach (KeyValuePair<string, string> d in data)
                    {
                        ExecuteProcedureAddLogEntryData(logEnteyId, d.Key, d.Value);
                    }
                }
            }
        }

        private int ExecuteProcedureAddLogEntry(string source, string type, string correlationId, DateTime createdOn, string message, string stackTrace)
        {
            SqlCommand cmd = new SqlCommand("AddLogEntry", _sqlConnection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Source", source);
            cmd.Parameters.AddWithValue("@Type", type);
            cmd.Parameters.AddWithValue("@CorrelationID", correlationId);
            cmd.Parameters.AddWithValue("@CreatedOn", createdOn);
            cmd.Parameters.AddWithValue("@Message", message);
            cmd.Parameters.AddWithValue("@StackTrace", stackTrace);
            SqlParameter LogId = new SqlParameter("@LogEntryID", System.Data.SqlDbType.Int);
            LogId.Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters.Add(LogId);
            cmd.ExecuteNonQuery();
            int id = (int)LogId.Value;
            return id;
        }

        private void ExecuteProcedureAddLogEntryData (int logID, string name, string value)
        {
            SqlCommand cmd = new SqlCommand("AddLogEntryData", _sqlConnection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@LogEntryID", logID);
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Value", value);
            cmd.ExecuteNonQuery();
        }

        public void Dispose()
        {
            CloseSqlConnection();
        }
    }
}
