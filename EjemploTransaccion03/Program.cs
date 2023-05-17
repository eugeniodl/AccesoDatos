using System.Configuration;
using System.Data;
using System.Data.SqlClient;

string connStr1 = ConfigurationManager.ConnectionStrings["connStr1"].ToString();
string connStr2 = ConfigurationManager.ConnectionStrings["connStr2"].ToString();

using(var conn1 = new SqlConnection(connStr1))
using(var conn2 = new SqlConnection(connStr2))
{
    conn1.Open();
    conn2.Open();

    // Iniciar la transacción principal
    SqlTransaction transaccion1 = conn1.BeginTransaction(IsolationLevel.ReadCommitted);
    try
    {
        // Crea un comando para la transacción principal
        SqlCommand command1 = conn1.CreateCommand();
        command1.Transaction = transaccion1;

        // Ejecutar una serie de comandos dentro de la transacción principal
        command1.CommandText = "INSERT INTO Customers(CustomerID,CompanyName,ContactName,Country) VALUES('KARVA','Karla Valladares','Lesther Vallecillo','Nicaragua')";
        command1.ExecuteNonQuery();

        // Iniciar la transacción anidada
        SqlTransaction transaction2 = conn2.BeginTransaction(IsolationLevel.ReadUncommitted);

        try
        {
            // Crear un comando para la transacción anidada
            SqlCommand command2 = conn2.CreateCommand();
            command2.Transaction = transaction2;

            // Ejecutar una serie de comandos dentro de la transacción anidada
            command2.CommandText = "UPDATE Customers SET Country='España' WHERE CustomerID='KARVA'";
            command2.ExecuteNonQuery();

            // Confirmar la transacción anidada
            transaction2.Commit();
        }
        catch (Exception ex)
        {
            // Si algo sale mal en la transacción anidada, deshacerla
            transaction2.Rollback();
            Console.WriteLine($"Error: {ex.Message}");
        }

        // Confirmar la transacción principal
        transaccion1.Commit();
    }
    catch (Exception ex)
    {
        // Si algo sale mal en la transacción principal, deshacerla
        transaccion1.Rollback();
        Console.WriteLine($"Error: {ex.Message}");
    }
}