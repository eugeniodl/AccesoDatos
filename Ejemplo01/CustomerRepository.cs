using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejemplo01
{
    public class CustomerRepository
        : IRepository<Customer>
    {
        private readonly string _connectionString;

        public CustomerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Customer> GetAll()
        {
            List<Customer> customers = new List<Customer>();

            try
            {
                using (SqlConnection connection =
                        new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand command =
                        new SqlCommand("SELECT * FROM Customers",
                        connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Customer customer = new Customer
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString()
                            };
                            customers.Add(customer);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al recuperar clientes " +
                    $"{ex.Message}");
            }

            return customers;
        }
    }
}
