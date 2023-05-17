using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DataLibrary
{
    public class TransactionOperation
    {
        private readonly int originAccount;
        private readonly int destinationAccount;
        private readonly decimal amount;

        // Obtener si la cuenta 1 realmente tiene el dinero
        const string sqlAccount1Fund = "SELECT (Sum(Credit) - Sum(Debit)) as Saldo FROM Accounts WHERE AccountNumber = @OriginAccount";

        // Consulta para extraer el valor de la cuenta de origen
        const string sqlWithdrawCredit = "INSERT INTO Accounts(AccountNumber, Debit, Credit) VALUES(@OriginAccount, @Debit, 0)";

        // Consulta para depositar el dinero en la cuenta destino
        const string sqlFundCredit = "INSERT INTO Accounts(AccountNumber, Debit, Credit) VALUES(@DestinationAccount, 0, @Credit)";

        public TransactionOperation(int origin, int destination, decimal amount)
        {
            this.originAccount = origin;
            this.destinationAccount = destination;
            this.amount = amount;
        }

        /// <summary>
        /// Esta operación obtiene la cantidad de dinero en la cuenta de origen
        /// y si esta es más que el monto a ser extraído, permite la operación.
        /// Extrae la cantidad de origen como débito y depósito en destino como crédito.
        /// La operación puede ser ejecutada asíncrona.
        /// </summary>
        /// <param name="commit">Si la transacción es verdadera commit, si no rollback</param>
        /// <param name="delayEndTransaction">Demora artificial para terminar la transacción</param>
        /// <param name="level">El nivel de aislamiento</param>
        /// <param name="TransactionName">Un nombre arbitrario para la transacción</param>
        /// <return>La tarea. Cuando la tarea es finalizada devuelve el valor verdadero si está bien</return>
        public Task<bool> ExecuteTransaction(bool commit, int delayEndTransaction, IsolationLevel level, string TransactionName)
        {
            return Task<bool>.Run(() =>
            {
                // Obtener la cadena de conexión
                string connStr1 = ConfigurationManager.ConnectionStrings["Bank1"].ConnectionString;

                // Crear la conexión
                var conn = new SqlConnection(connStr1);

                // Prepara para leer la disponiblidad de dinero en origen
                var command = new SqlCommand(sqlAccount1Fund, conn);
                command.Parameters.AddWithValue("@OriginAccount", originAccount);

                // Abrir la conexión con la BD
                conn.Open();

                // Crea la transacción y se alista con eso el comando
                var transaction = conn.BeginTransaction(level, TransactionName);
                command.Transaction = transaction;
                try
                {
                    var funds = Convert.ToDecimal(command.ExecuteScalar());
                    Console.WriteLine($"{TransactionName} => fund: {funds} amount: {amount}");
                    if(funds > amount)
                    {
                        // Extrae dinero de cuenta de origen
                        command = new SqlCommand(sqlWithdrawCredit, conn)
                        {
                            Transaction = transaction
                        };
                        command.Parameters.AddWithValue("@OriginAccount", originAccount);
                        command.Parameters.AddWithValue("@Debit", amount);
                        command.ExecuteNonQuery();

                        // Deposita dinero en destino
                        command = new SqlCommand(sqlFundCredit, conn)
                        {
                            Transaction = transaction
                        };
                        command.Parameters.AddWithValue("@DestinationAccount", destinationAccount);
                        command.Parameters.AddWithValue("@Credit", amount);
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
                    if(commit)
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
}