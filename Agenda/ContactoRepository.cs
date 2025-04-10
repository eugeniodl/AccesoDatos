


using System.Data.SqlClient;

public class ContactoRepository : IRepository<Contacto>
{
    private readonly string _connectionString;

    public ContactoRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void Delete(int id)
    {
        const string query = "DELETE FROM Contactos WHERE Id = @id";

        ExecuteNonQuery(query, 
            cmd => cmd.Parameters.AddWithValue("@id", id));
    }

    private void ExecuteNonQuery(string query, 
        Action<SqlCommand> configureCommad)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            configureCommad(command);
            connection.Open();
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error en la operación de base de datos: " +
                $"{ex.Message}", ex);
        }
    }

    public IEnumerable<Contacto> GetAll()
    {
        const string query = "SELECT Id, Nombre, Apellido, FechaNacimiento, Telefono, Email FROM Contactos";
        var contactos = new List<Contacto>();

        try
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            connection.Open();
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                contactos.Add(new Contacto
                {
                    Id = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    Apellido = reader.GetString(2),
                    FechaNacimiento = reader.GetDateTime(3),
                    Telefono = reader.GetInt32(4),
                    Email = reader.GetString(5)
                });
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener contactos: {ex.Message}", ex);
        }
        return contactos;
    }

    public Contacto GetT(int id)
    {
        throw new NotImplementedException();
    }

    public void Insert(Contacto entity)
    {
        throw new NotImplementedException();
    }

    public void Update(Contacto entity)
    {
        throw new NotImplementedException();
    }
}

