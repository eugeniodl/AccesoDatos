using Microsoft.Data.SqlClient;

public class TablaRepository : IRepository<Tabla>
{
    private readonly string _connectionString;

    public TablaRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IEnumerable<Tabla> GetAll()
    {
        var lista = new List<Tabla>();

        try
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var cmd = new SqlCommand("SELECT IdRow, Nombre, Apellido FROM Tabla", connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var tabla = new Tabla
                {
                    IdRow = reader.GetInt32(reader.GetOrdinal("IdRow")),
                    Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                    Apellido = reader.GetString(reader.GetOrdinal("Apellido"))
                };
                lista.Add(tabla);
            }
        }
        catch (SqlException sqlEx)
        {
            Console.Error.WriteLine($"Error de SQL al recuperar datos: {sqlEx.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error inesperado: {ex.Message}");
            throw;
        }

        return lista;
    }
}
