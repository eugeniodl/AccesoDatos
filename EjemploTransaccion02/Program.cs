using System.Configuration;
using System.Data.SqlClient;

// Crear una conexión a la Base de datos
SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["connstring"].ToString());

// Abrir la conexión
connection.Open();

const string X_ORIGIN_ACCOUNT = "10";
const string X_DESTINATION_ACCOUNT = "20";

// Transferir fondos de una cuenta a otra (acreditar 200 de la cuenta 10 a la cuenta 20)
decimal quantityToTransfer = 200;

// Preaparar consultas
// Obtener si la cuenta 1 realmente tiene dinero
var sqlAccount1Fund = "SELECT (Sum(Credit) - Sum(Debit)) as Saldo FROM Accounts WHERE AccountNumber = @OriginAccount";

// Consulta para extraer el valor de la cuenta de origen
var sqlWithdrawCredit = "INSERT INTO Accounts(AccountNumber, Debit, Credit) VALUES (@OriginAccount, @Debit, 0)";

// Consulta para depositar el dinero en destino
var sqlFundCredit = "INSERT INTO Accounts(AccountNumber, Debit, Credit) VALUES(@DestinationAccount, 0, @Credit)";

// Crear la transacción
SqlTransaction transaction = connection.BeginTransaction("TransferFunds");

try
{
    // Preparar comando que participan en la transacción
    SqlCommand command1 = new SqlCommand(sqlAccount1Fund, connection, transaction);
    SqlCommand command2 = new SqlCommand(sqlWithdrawCredit, connection, transaction);
    SqlCommand command3 = new SqlCommand(sqlFundCredit, connection, transaction);

    // Leer la cuenta de origen para determinar si hay fondos.
    command1.Parameters.AddWithValue("@OriginAccount", X_ORIGIN_ACCOUNT);
    var funds = Convert.ToDecimal(command1.ExecuteScalar());
    Console.WriteLine($"Total Origen Fondo Inicial: {funds:C}");

    if (funds < quantityToTransfer)
    {
        transaction.Rollback();
        Console.WriteLine($"Fondos insuficientes en la cuenta 1: {funds:C}. ¡Transacción abortada!");
        Console.ReadLine();
        return;
    }
}
catch (Exception ex)
{
    // Si algo sale mal, deshacer la transacción
    transaction.Rollback();
    Console.WriteLine($"Ha ocurrido un error, los fondos no se han transferido: {ex.Message}");
}
finally
{
    connection.Close();
}