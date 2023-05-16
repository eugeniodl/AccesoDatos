using System.Configuration;
using System.Data.SqlClient;

SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connstring"].ToString());

conn.Open();

SqlTransaction trans = conn.BeginTransaction("TransferFunds");

const string X_ORIGIN_ACCOUNT = "10";
const string X_DESTINATION_ACCOUNT = "20";

// Transferir fondos de una cuenta a otra (acreditar 200 de la cuenta 10 a la 20)
decimal quantityToTransfer = 200;

// Preparar consultas
// Obtener si la cuenta 1 realmente tiene el dinero
var sqlAccount1Fund = "SELECT (Sum(Credit) - Sum(Debit)) as Saldo FROM Accounts WHERE AccountNumber = @OriginAccount";

// Consulta para extraer el valor de la cuenta de origen
var sqlWithdrawCredit = "INSERT INTO Accounts(AccountNumber, Debit, Credit) VALUES(@OriginAccount, @Debit, 0)";

// Consulta para depositar el dinero en la cuenta destino
var sqlFundCredit = "INSERT INTO Accounts(AccountNumbe, Debit, Credit) VALUES(@DestinationAccount, 0, @Credit)";

try
{
    // Preparar los comandos que participan en la transacción
    SqlCommand command1 = new SqlCommand(sqlAccount1Fund, conn, trans);
    SqlCommand command2 = new SqlCommand(sqlWithdrawCredit, conn, trans);
    SqlCommand command3 = new SqlCommand(sqlFundCredit, conn, trans);

    // Leer la cuenta de origen para determinar si hay fondos.
    command1.Parameters.AddWithValue("@OriginAccount", X_ORIGIN_ACCOUNT);
    var funds = Convert.ToDecimal(command1.ExecuteScalar());
    Console.WriteLine($"Total Origen Fondo Inicial: {funds:C}");

    if(funds < quantityToTransfer)
    {
        trans.Rollback();
        Console.WriteLine($"Fondos insuficientes en la cuenta {X_ORIGIN_ACCOUNT}: {funds:C}. ¡Transacción abortada!");
        Console.ReadLine();
        return;
    }

    // Los fondos son correctos, retirar los fondos de la cuenta de origen
    command2.Parameters.AddWithValue("@Debit", quantityToTransfer);
    command2.Parameters.AddWithValue("@OriginAccount", X_ORIGIN_ACCOUNT);
    var result = command2.ExecuteNonQuery();

    // Deposita el fondo en la cuenta de destino
    command3.Parameters.AddWithValue("@Credit", quantityToTransfer);
    command3.Parameters.AddWithValue("@DestinationAccount", X_DESTINATION_ACCOUNT);
    result = command3.ExecuteNonQuery();

    trans.Commit();
    Console.WriteLine("Fondos transferidos con éxito");
}
catch (Exception ex)
{
    trans.Rollback();
    Console.WriteLine($"Ha ocurrido un error, los fondos no se ha transferido: {ex.Message}");
}
finally
{
    conn.Close();
}