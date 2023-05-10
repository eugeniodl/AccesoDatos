using System.Configuration;
using System.Data.SqlClient;

// Crear una conexión a la base de datos
SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["constring"].ToString());

// Abrir la conexión
conn.Open();

// Iniciar la transacción
SqlTransaction trans = conn.BeginTransaction();

try
{
    // Crear un comando para la transacción
    SqlCommand cmd = conn.CreateCommand();
    cmd.Transaction = trans;

    // Ejecutar una serie de comandos dentro de la transacción
    cmd.CommandText = "INSERT INTO Customers(CustomerID, CompanyName, ContactName, Country) VALUES ('MARAN', 'Maria Andrade', 'Alfredo Fonseca', 'Nicaragua')";
    cmd.ExecuteNonQuery();

    cmd.CommandText = "INSERT INTO Orders(CustomerID, OrderDate) VALUES ('WWWW', '2023-05-08')";
    cmd.ExecuteNonQuery();

    // Confirmar la transacción
    trans.Commit();
}
catch (Exception ex)
{
    // Si algo sale mal, deshacer la transacción
    trans.Rollback();
    Console.WriteLine("Error: " + ex.Message);
}
finally
{
    // Cerrar la conexión
    conn.Close();
}
