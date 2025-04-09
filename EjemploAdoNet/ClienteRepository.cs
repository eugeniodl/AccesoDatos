




using System.Data.SqlClient;

public class ClienteRepository : IRepository<Cliente>
{
    private readonly string _connectionString;

    public ClienteRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    public IEnumerable<Cliente> GetAll()
    {
        List<Cliente> list = new List<Cliente>();

        try
        {
            using (SqlConnection connection =
        new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command =
                    new SqlCommand("SELECT * FROM Cliente", connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Cliente cliente = new Cliente
                        {
                            IdCliente = Convert.ToInt32(reader["IdCliente"]),
                            RUT = reader["RUT"].ToString(),
                            NombreCompleto = reader["NombreCompleto"].ToString(),
                            Telefono = reader["Telefono"].ToString()
                        };
                        list.Add(cliente);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al recuperar clientes {ex.Message}");
        }
        return list;
    }
}

