using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;

public class TransactionOperation
{
    private readonly int originAccount;
    private readonly int destinationAccount;
    private readonly decimal amount;

    // Obtener si se encuentra dinero en la cuenta 1
    const string sqlAccount1Fund = "SELECT (Sum(Credit) - Sum(Debit)) as Saldo FROM AccountTransactions WHERE AccountNumber = @OriginAccount";

    // Consulta para extraer el valor de la cuenta de origen
    const string sqlWithdrawCredit = "INSERT INTO AccountTransactions (AccountNumber, Debit, Credit) VALUES (@OriginAccount, @Debit, 0)";

    // Consulta para depositar el dinero en el destino
    const string sqlFundCredit = "INSERT INTO AccountTransactions (AccountNumber, Debit, Credit) VALUES (@DestinationAccount, 0, @Credit)";

    public TransactionOperation(int origin, int destination, decimal amount)
    {
        originAccount = origin;
        destinationAccount = destination;
        this.amount = amount;
    }

    /// <summary>
    /// Esta operación obtiene la cantidad de dinero en la cuenta de origen
    /// y si esta es más que el monto a ser extraído, permite la operación
    /// Extrae la cantidad de origen como débito y depósito en destino como crédito
    /// La operación puede ser ejecutada asíncrona
    /// </summary>
    /// <param name="commit">Si la transacción es verdadera commit si no rollback</param>
    /// <param name="delayEndTransaction">Demora artificial para terminar la transacción</param>
    /// <param name="level">El nivel de aislamiento</param>
    /// <param name="TransactionName">Un nombre arbitrario para la transacción</param>
    /// <return>La tarea. Cuando la tarea es finalizada devuelve el valor verdadero si está bien</return>
    public Task<bool> ExecuteTransaction(bool commit, int delayEndTransaction, IsolationLevel level, string TransactionName)
    {
        return Task.Run(
            () =>
            {
                // Obtener la cadena de conexión
                string connStr1 = ConfigurationManager.ConnectionStrings["constring"].ConnectionString;

                // Crear la conexión
                var conn = new SqlConnection(connStr1);

                // Preparar para leer la disponibilidad de dinero en origen
                var command = new SqlCommand(sqlAccount1Fund, conn);
                command.Parameters.AddWithValue("@OriginAccount", this.originAccount);

                // Abrir la conexión con la BD
                conn.Open();

                // Crea la transacción y se alista con eso el comando
                var transaction = conn.BeginTransaction(level, TransactionName);
                command.Transaction = transaction;
                try
                {
                    var funds = Convert.ToDecimal(command.ExecuteScalar());
                    Console.WriteLine($"{TransactionName} => fund: {funds} amount: {amount}");
                    if (funds > amount)
                    // Si son
                    {
                        // Extrae dinero de origen
                        command = new SqlCommand(sqlWithdrawCredit, conn)
                        {
                            Transaction = transaction
                        };
                        command.Parameters.AddWithValue("@OriginAccount", this.originAccount);
                        command.Parameters.AddWithValue("@Debit", this.amount);
                        command.ExecuteNonQuery();

                        // Deposita dinero en destino
                        command = new SqlCommand(sqlFundCredit, conn)
                        {
                            Transaction = transaction
                        };
                        command.Parameters.AddWithValue("@DestinationAccount", this.destinationAccount);
                        command.Parameters.AddWithValue("@Credit", this.amount);
                        command.ExecuteNonQuery();
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    //transaction.Rollback();
                    Console.WriteLine(ex.Message);
                    return false;
                }
                finally
                {
                    // Si commit, commit no es rollback
                    if (commit)
                        transaction.Commit();
                    else
                    {
                        Thread.Sleep(delayEndTransaction);
                        transaction.Rollback();
                    }
                }
            });
    }
}