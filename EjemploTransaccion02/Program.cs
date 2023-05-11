using System.Configuration;
using System.Data.SqlClient;

const string X_ORIGIN_ACCOUNT = "10";
const string X_DESTINATION_ACCOUNT = "20";

// Transferir fondos de una cuenta a otra (200 de la cuenta 10 a la cuenta 20)
decimal quantityToTransfer = 200;

// Preparar consultas
// Obtener si la cuenta 1 realmente tiene dinero
var sqlAccount1Fund = "SELECT (Sum(Credit) - Sum(Debit)) AS Saldo FROM Accounts WHERE AccountNumber = @OriginAccount";

// Consulta para extraer el valor de la cuenta de origen
var sqlWithdrawCredit = "INSERT INTO Accounts(AccountNumber, Debit, Credit) VALUES(@OriginAccount, @Debit, 0)";

// Consulta para depositar el dinero en la cuenta de destino
var sqlFundCredit = "INSERT INTO Accounts(AccountNumbe, Debit, Credit) VALUES(@DestinationAccount, 0, @Credit)";

// Crear una conexión a la Base de datos
string connectionString = ConfigurationManager.ConnectionStrings["connstring"].ToString();

using (SqlConnection conn = new SqlConnection(connectionString))
{
    conn.Open();
    // Crear la transacción
    SqlTransaction transaction = conn.BeginTransaction("TransferFunds");

    try
    {
        // Preparar comandos que participan en la transacción
        SqlCommand command1 = new SqlCommand(sqlAccount1Fund, conn, transaction);
        SqlCommand command2 = new SqlCommand(sqlWithdrawCredit, conn, transaction);
        SqlCommand command3 = new SqlCommand(sqlFundCredit, conn, transaction);

        // Leer la cuenta de origen para determinar si hay fondos
        command1.Parameters.AddWithValue("@OriginAccount", X_ORIGIN_ACCOUNT);
        decimal funds = Convert.ToDecimal(command1.ExecuteScalar());
        Console.WriteLine($"Origen Fondo Inicial: {funds:C}");

        if(funds < quantityToTransfer)
        {
            transaction.Rollback();
            Console.WriteLine($"Fondos insuficientes en la cuenta {X_ORIGIN_ACCOUNT}: {funds:C}. ¡Transacción abortada!");
            Console.ReadLine();
            return;
        }

        command2.Parameters.AddWithValue("@Debit", quantityToTransfer);
        command2.Parameters.AddWithValue("@OriginAccount", X_ORIGIN_ACCOUNT);
        command2.ExecuteNonQuery();

        command3.Parameters.AddWithValue("@Credit", quantityToTransfer);
        command3.Parameters.AddWithValue("@DestinationAccount", X_DESTINATION_ACCOUNT);
        command3.ExecuteNonQuery();

        transaction.Commit();
        Console.WriteLine("¡Fondos transferidos con éxito!");
    }
    catch (Exception ex)
    {
        transaction.Rollback();
        Console.WriteLine($"Error, los fondos no se han transferido: {ex.Message}");
    }
}