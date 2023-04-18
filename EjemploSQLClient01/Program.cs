using System.Data.SqlClient;

string connectionString = @"Data Source=DESKTOP-KQLF513\SQLSERVER2019;Initial Catalog=Northwind;Integrated Security=True";

// Proporciona la cadena de consulta con un marcador de posición de parámetro.
string queryString = "SELECT ProductID, UnitPrice, ProductName " +
    "FROM Products WHERE UnitPrice > @pricePoint " +
    "ORDER BY UnitPrice DESC;";

// Especifica el valor del parámetro
int paramValue = 30;

// Crea y abre la conexión en un bloque using.
// Esto garantiza que todos los recursos se cerrarán
// y eliminarán cuando el código salga
using(SqlConnection connection = new SqlConnection(connectionString))
{
    // Crea los objetos Command y Parameters
    SqlCommand command = new SqlCommand(queryString, connection);
    command.Parameters.AddWithValue("@pricePoint", paramValue);

    // Abra la conexión en un bloque try/catch
    // Cree y ejecute el DataReader, escribiendo
    // el conjunto de resultados en la ventana de la consola.
    try
    {

        connection.Open();
        SqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Console.WriteLine("\t{0}\t{1}\t{2}", reader[0], reader[1], reader[2]);
        }
    }
    catch (Exception ex)
    {

        Console.WriteLine(ex.Message);
    }
    Console.ReadLine();
}