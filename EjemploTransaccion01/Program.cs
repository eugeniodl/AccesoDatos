
using System.Configuration;
using System.Data.SqlClient;

// Crear una conexión a la Base de datos
string connectionString = ConfigurationManager.ConnectionStrings["connstring"].ToString();
SqlConnection conn = new SqlConnection(connectionString);

// Abrir la conexión
conn.Open();

// Iniciar la transacción
SqlTransaction transaction = conn.BeginTransaction();

try
{
    // Crear un comando para la transacción
    SqlCommand cmd = conn.CreateCommand();
    cmd.Transaction = transaction;

    // Ejecutar una serie de comandos dentro de la transacción
    cmd.CommandText = "INSERT INTO Customers(CustomerID, CompanyName, ContactName, Country) VALUES('ALLAG', 'Allan Aguilar', 'Pablo Salazar', 'Nicaragua')";
    cmd.ExecuteNonQuery();

    cmd.CommandText = "INSERT INTO Orders(CustomerID, OrderDate) VALUES('WWWWW', '2023-05-08')";
    cmd.ExecuteNonQuery();

    // Confirmar la transacción
    transaction.Commit();
}
catch (Exception ex)
{
    // Si algo sale mal, deshacer la transacción
    transaction.Rollback();
    Console.WriteLine($"Error: {ex.Message}");
}
finally
{
    // Cerrar la conexión
    conn.Close();
}