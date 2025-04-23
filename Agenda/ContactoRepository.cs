using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Agenda
{
    public class ContactoRepository : IRepository<Contacto>
    {
        private readonly string _connectionString;

        public ContactoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void Delete(int id)
        {
            const string sql = "DELETE FROM Contactos WHERE Id = @id";

            ExecuteNonQuery(sql, cmd =>
                cmd.Parameters.AddWithValue("@id", id)
            );
        }

        public IEnumerable<Contacto> GetAll()
        {
            const string sql = "SELECT Id,Nombre,Apellido,FechaNacimiento,Telefono,Email FROM Contactos";
            var contactos = new List<Contacto>();

            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, connection);
                connection.Open();
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    contactos.Add(new Contacto()
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

        public Contacto GetValue(int id)
        {
            const string sql = @"SELECT Id,
                                        Nombre,
	                                    Apellido,
	                                    FechaNacimiento,
	                                    Telefono,
	                                    Email
                                 FROM Contactos WHERE Id = @id";

            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand(sql, connection);
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
            const string sql = @"
                INSERT INTO Contactos(Nombre,Apellido,FechaNacimiento,Telefono,Email)
                VALUES (@nombre,@apellido,@fechanacimiento,@telefono,@email)";

            ExecuteNonQuery(sql, cmd =>
            {
                cmd.Parameters.AddWithValue("@nombre", entity.Nombre);
                cmd.Parameters.AddWithValue("@apellido", entity.Apellido);
                cmd.Parameters.AddWithValue("@fechanacimiento", entity.FechaNacimiento);
                cmd.Parameters.AddWithValue("@telefono", entity.Telefono);
                cmd.Parameters.AddWithValue("@email", entity.Email);
            });
        }

        private void ExecuteNonQuery(string sql, Action<SqlCommand> configureCommand)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand(sql, connection);
                configureCommand(command);
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en la operación de la base de datos: {ex.Message}",
                    ex);
            }
        }

        public void Update(Contacto entity)
        {
            const string sql = @"
                UPDATE Contactos
                   SET Nombre = @nombre,
                       Apellido = @apellido,
                       FechaNacimiento = @fechanacimiento,
                       Telefono = @telefono,
                       Email = @email
                 WHERE Id = @id";

            ExecuteNonQuery(sql, cmd => {
                cmd.Parameters.AddWithValue("@nombre", entity.Nombre);
                cmd.Parameters.AddWithValue("@apellido", entity.Apellido);
                cmd.Parameters.AddWithValue("@fechanacimiento", entity.FechaNacimiento);
                cmd.Parameters.AddWithValue("@telefono", entity.Telefono);
                cmd.Parameters.AddWithValue("@email", entity.Email);
                cmd.Parameters.AddWithValue("@id", entity.Id);
            });
        }
    }
}
