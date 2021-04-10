using DataAccessLayer;
using System.Data.SqlClient;

namespace Models
{
    public class AddressInfo : IReadableAddress
    {
        public string Address { get; set; }
        public AddressInfo(string connectionString, int id)
        {
            SetAddress(connectionString, id);
        }
        public AddressInfo() { }
        public void SetAddress(string connectionString, int id)
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            var command = new SqlCommand("GetAddresById", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            var idParam = new SqlParameter("@id", id);
            command.Parameters.Add(idParam);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Address = reader.GetString(0);
            }
            reader.Close();
            connection.Close();
        }
    }
}