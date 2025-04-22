


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
            using var command = new SqlCommand(query, connection);
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
        const string query = "SELECT Id, Nombre, Apellido, FechaNacimiento, Telefono, Email FROM Contactos WHERE Id = @id";

        try
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new Contacto
                {
                    Id = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    Apellido = reader.GetString(2),
                    FechaNacimiento = reader.GetDateTime(3),
                    Telefono = reader.GetInt32(4),
                    Email = reader.GetString(5)
                };
            }
        }
        catch (Exception ex)
        {

            throw new Exception($"Error al obtener el contacto: {ex.Message}", ex);
        }

        return null;
    }

    public void Insert(Contacto entity)
    {
        const string query = @"INSERT INTO Contactos
           (Nombre
           ,Apellido
           ,FechaNacimiento
           ,Telefono
           ,Email)
     VALUES
           (@nombre
           ,@apellido
           ,@fechanacimiento
           ,@telefono
           ,@email)";

        ExecuteNonQuery(query, cmd =>
        {
            cmd.Parameters.AddWithValue("@nombre", entity.Nombre);
            cmd.Parameters.AddWithValue("@apellido", entity.Apellido);
            cmd.Parameters.AddWithValue("@fechanacimiento", entity.FechaNacimiento);
            cmd.Parameters.AddWithValue("@telefono", entity.Telefono);
            cmd.Parameters.AddWithValue("@email", entity.Email);
        });
    }

    public void Update(Contacto entity)
    {
        const string query = @"UPDATE Contactos
                       SET Nombre = @nombre
                          ,Apellido = @apellido
                          ,FechaNacimiento = @fechanacimiento
                          ,Telefono = @telefono
                          ,Email = @email
                     WHERE Id = @id";

        ExecuteNonQuery(query, cmd =>
        {
            cmd.Parameters.AddWithValue("@nombre", entity.Nombre);
            cmd.Parameters.AddWithValue("@apellido", entity.Apellido);
            cmd.Parameters.AddWithValue("@fechanacimiento", entity.FechaNacimiento);
            cmd.Parameters.AddWithValue("@telefono", entity.Telefono);
            cmd.Parameters.AddWithValue("@email", entity.Email);
            cmd.Parameters.AddWithValue("@id", entity.Id);
        });
    }
}

