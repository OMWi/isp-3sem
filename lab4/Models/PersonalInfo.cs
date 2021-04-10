using DataAccessLayer;
using System.Data.SqlClient;

namespace Models
{
    public class PersonalInfo : IReadablePerson
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public PersonalInfo (string connectionString, int id)
        {
            SetName(connectionString, id);
            SetPhoneNumber(connectionString, id);
        }
        public PersonalInfo() { }
        public void SetName(string connectionString, int id)
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            var command = new SqlCommand("GetNameByBuid", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            var bidParam = new SqlParameter("@bid", id);
            command.Parameters.Add(bidParam);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Name = reader.GetString(0);
            }
            reader.Close();
            connection.Close();
        }
        public void SetPhoneNumber(string connectionString, int id)
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            var command = new SqlCommand("GetPhoneNumByBuid", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            var bidParam = new SqlParameter("@bid", id);
            command.Parameters.Add(bidParam);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                PhoneNumber = reader.GetString(0);
            }
            reader.Close();
            connection.Close();
        }
    }
}