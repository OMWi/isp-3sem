using System;
using System.Data.SqlClient;

namespace LoggerLayer
{
    public class Logger
    {
        public string ConnectionString = 
            @"Data Source=DESKTOP-8DL0E0U;Initial Catalog=ErrorLogging;Integrated Security=True";
        public void AddError(Exception e, DateTime time)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            var commands = new SqlCommand("AddError", connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            var errorParam = new SqlParameter
            {
                ParameterName = "@errorMess",
                Value = e.Message
            };
            var timeParam = new SqlParameter
            {
                ParameterName = "@time",
                Value = time
            };
            commands.Parameters.Add(errorParam);
            commands.Parameters.Add(timeParam);
            commands.ExecuteNonQuery();
        }
    }
}