using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            string queryString = "DELETE FROM Contactos WHERE Id = @id";

            try
            {
                using (SqlConnection connection
            = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand command
                        = new SqlCommand(queryString, connection);
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

                throw new Exception($"Error al eliminar contacto en la base de datos {ex.Message}");
            }

        }

        public IEnumerable<Contacto> GetAll()
        {
            List<Contacto> contactos = new List<Contacto>();

            string queryString = "SELECT Id, Nombre, Apellido, FechaNacimiento, Telefono, Email FROM Contactos";

            try
            {
                using (SqlConnection connection
            = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand command
                        = new SqlCommand(queryString, connection);
                    using (SqlDataReader reader
                        = command.ExecuteReader())
                    {
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
                }
            }
            catch (Exception ex)
            {

                throw new Exception($"Error al obtener contactos en la base de datos {ex.Message}");
            }

            return contactos;
        }

        public Contacto GetValue(int id)
        {
            string queryString = "SELECT Id, Nombre, Apellido, FechaNacimiento, Telefono, Email FROM Contactos WHERE Id = @id";
            try
            {
                using (SqlConnection connection
                        = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand command
                        = new SqlCommand(queryString, connection);
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            Contacto contacto = new Contacto
                            {
                                Id = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Apellido = reader.GetString(2),
                                FechaNacimiento = reader.GetDateTime(3),
                                Telefono = reader.GetInt32(4),
                                Email = reader.GetString(5)
                            };
                            return contacto;
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception($"Error al obtener contacto en la base de datos {ex.Message}");
            }
            return null;
        }

        public void Insert(Contacto contacto)
        {
            string queryString = "INSERT INTO Contactos(Nombre, Apellido, FechaNacimiento, Telefono, Email) VALUES (@nombre, @apellido, @fechanacimiento, @telefono, @email)";
            try
            {
                using (SqlConnection connection
            = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand command
                        = new SqlCommand(queryString, connection);
                    command.Parameters.AddWithValue("@nombre", contacto.Nombre);
                    command.Parameters.AddWithValue("@apellido", contacto.Apellido);
                    command.Parameters.AddWithValue("@fechanacimiento", contacto.FechaNacimiento);
                    command.Parameters.AddWithValue("@telefono", contacto.Telefono);
                    command.Parameters.AddWithValue("@email", contacto.Email);
                    command.ExecuteNonQuery();                    
                }
            }
            catch (Exception ex)
            {

                throw new Exception($"Error al insertar contacto en la base de datos {ex.Message}");
            }
        }

        public void Update(Contacto contacto)
        {
            string queryString = "UPDATE Contactos SET Nombre = @nombre, Apellido = @apellido, FechaNacimiento = @fechanacimiento, Telefono = @telefono, Email = @email WHERE Id = @id";
            using(SqlConnection connection 
                = new SqlConnection(_connectionString))
            {
                connection.Open(); 
                SqlCommand command 
                    = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@nombre", contacto.Nombre);
                command.Parameters.AddWithValue("@apellido", contacto.Apellido);
                command.Parameters.AddWithValue("@fechanacimiento", contacto.FechaNacimiento);
                command.Parameters.AddWithValue("@telefono", contacto.Telefono);
                command.Parameters.AddWithValue("@email", contacto.Email);
                command.Parameters.AddWithValue("@id", contacto.Id);
                command.ExecuteNonQuery();
            }
        }
    }
}
