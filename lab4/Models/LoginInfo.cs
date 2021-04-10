using DataAccessLayer;
using System.Data.SqlClient;

namespace Models
{
    public class LoginInfo : IReadableLogin
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public LoginInfo(string connectionString, int id)
        {
            SetEmail(connectionString, id);
            SetPassword(connectionString, id);
        }
        public LoginInfo() { }
        public void SetEmail(string connectionString, int id)
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            var command = new SqlCommand("GetEmailByBuid", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            var bidParam = new SqlParameter("@bid", id);
            command.Parameters.Add(bidParam);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Email = reader.GetString(0);                
            }
            reader.Close();
            connection.Close();
        }
        public void SetPassword(string connectionString, int id)
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            var command = new SqlCommand("GetPasswordByBuid", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            var bidParam = new SqlParameter("@bid", id);
            command.Parameters.Add(bidParam);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Password = reader.GetString(0);
            }
            reader.Close();
            connection.Close();
        }
    }
}