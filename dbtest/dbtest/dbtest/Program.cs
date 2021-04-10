using System;
using System.Data.SqlClient;

namespace dbtest
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Data Source=DESKTOP-8DL0E0U;Initial Catalog=testdb;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Подключение открыто");
                SqlCommand command = new SqlCommand();

            }
            Console.WriteLine("Подключение закрыто...");
            Console.ReadKey(true);
        }
    }
}
