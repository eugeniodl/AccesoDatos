using System.Configuration;
using System.Data.SqlClient;

// Crear una conexión a la Base de datos
SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connstring"].ToString());

// Abrir la conexión
connection.Open();

//Iniciar la transacción
SqlTransaction transaction = connection.BeginTransaction();

try
{
    // Crear un comando para la transacción
    SqlCommand command = connection.CreateCommand();
    command.Transaction = transaction;

    // Ejecutar una serie de comandos dentro de la transacción
    command.CommandText = "INSERT INTO Customers(CustomerID,CompanyName,ContactName,Country) VALUES('MARSO','Martha Sotelo','Pedro Ramírez','Nicaragua')";
    command.ExecuteNonQuery();

    command.CommandText = "INSERT INTO Orders(CustomerID, OrderDate) VALUES('WWWWW','2023-05-08')";
    command.ExecuteNonQuery();

    // Confirmar la transacción
    transaction.Commit();
}
catch (Exception ex)
{
    // Si algo sale mal, deshacer la transacción
    transaction.Rollback();
    Console.WriteLine("Error: " + ex.Message);
}
finally
{
    // Cerrar la conexión
    connection.Close();
}