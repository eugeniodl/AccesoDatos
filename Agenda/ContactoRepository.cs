using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
        }

        public IEnumerable<Contacto> GetAll()
        {
            throw new NotImplementedException();
        }

        public Contacto GetValue(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(Contacto entity)
        {
            const string sql = @"
                INSERT INTO Contactos(Nombre,Apellido,FechaNacimiento,Telefono,Email)
                VALUES (@nombre,@apellido,@fechanacimiento,@telefono,@email)";

            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = new SqlCommand(sql, connection);
        }

        public void Update(Contacto entity)
        {
            throw new NotImplementedException();
        }
    }
}
