

using System.Configuration;
using System.Data.SqlClient;

const string X_ORIGIN_ACCOUNT = "10";
const string X_DESTINATION_ACCOUNT = "20";

// Transferir fondos de una cuenta a otra (acreditar 200 de la cuenta 10 a la 20)
decimal quantityToTransfer = 200;

// Preparar consultas
// Obtener si la cuenta 10 realmente tiene el dinero
var sqlAccount1Fund = "SELECT (Sum(Credit) - Sum(Debit)) as Saldo FROM Accounts WHERE AccountNumber = @OriginAccount";

// Consulta para extraer el valor de la cuenta de origen
var sqlWithdrawCredit = "INSERT INTO Accounts(AccountNumber, Debit, Credit) VALUES (@OriginAccount, @Debit, 0)";

// Consulta para depositar el dinero en destino
var sqlFundCredit = "INSERT INTO Accounts(AccountNumber, Debit, Credit) VALUES(@DestinationAccount, 0, @Credit)";

string connectionString = ConfigurationManager.ConnectionStrings["constring"].ToString();

using (var conn = new SqlConnection(connectionString))
{
    conn.Open();
    SqlTransaction tran = conn.BeginTransaction("TransferFunds");
    try
    {
        using(var command1 = new SqlCommand(sqlAccount1Fund, conn, tran))
        {
            command1.Parameters.AddWithValue("@OriginAccount", X_ORIGIN_ACCOUNT);
            decimal funds = Convert.ToDecimal(command1.ExecuteScalar());
            Console.WriteLine($"Total Origen Fondo Inicial: {funds:C}");

            if(funds < quantityToTransfer)
            {
                tran.Rollback();
                Console.WriteLine($"Fondos insuficientes en la cuenta {X_ORIGIN_ACCOUNT}: {funds:C}. ¡Transacción abortada!");
                Console.ReadLine();
                return;
            }
        }

        using(var command2 = new SqlCommand(sqlWithdrawCredit, conn, tran))
        {
            command2.Parameters.AddWithValue("@OriginAccount", X_ORIGIN_ACCOUNT);
            command2.Parameters.AddWithValue("@Debit", quantityToTransfer);
            command2.ExecuteNonQuery();
        }

        using(var command3 = new SqlCommand(sqlFundCredit, conn, tran))
        {
            command3.Parameters.AddWithValue("DestinationAccount", X_DESTINATION_ACCOUNT);
            command3.Parameters.AddWithValue("@Credit", quantityToTransfer);
            command3.ExecuteNonQuery();
        }

        tran.Commit();
        Console.WriteLine("Fondos transferidos con éxito");
    }
    catch (Exception ex)
    {
        tran.Rollback();
        Console.WriteLine($"Error, los fondos no se han transferido: {ex.Message}");
    }
}
    