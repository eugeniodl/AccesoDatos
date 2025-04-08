using System.Data.SqlClient;

public class TablaRepository : IRepository<Tabla>
{
    private readonly string _connectionString;

    public TablaRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    public IEnumerable<Tabla> GetAll()
    {
        List<Tabla> list = new List<Tabla>();

        try
        {
            using (SqlConnection connection =
                new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Tabla", connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Tabla tabla = new Tabla
                        {
                            IdRow = Convert.ToInt32(reader["IdRow"]),
                            Nombre = reader["Nombre"].ToString(),
                            Apellido = reader["Apellido"].ToString()
                        };
                        list.Add(tabla);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al recuperar Tabla {ex.Message}");
        }
        return list;
    }
}

