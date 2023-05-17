using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary
{
    public class TransactionOperation
    {
        private readonly int _originAccount;
        private readonly int _destinationAccount;
        private readonly decimal _amount;

        // Obtener si la cuenta 10 realmente tiene el dinero
        const string sqlAccount1Fund = "SELECT (Sum(Credit) - Sum(Debit)) as Saldo FROM Accounts WHERE AccountNumber = @OriginAccount";

        // Consulta para extraer el valor de la cuenta de origen
        const string sqlWithdrawCredit = "INSERT INTO Accounts(AccountNumber, Debit, Credit) VALUES (@OriginAccount, @Debit, 0)";

        // Consulta para depositar el dinero en destino
        const string sqlFundCredit = "INSERT INTO Accounts(AccountNumber, Debit, Credit) VALUES(@DestinationAccount, 0, @Credit)";

        public TransactionOperation(int origin, int destination, decimal amount)
        {
            _originAccount = origin;
            _destinationAccount = destination;
            _amount = amount;
        }

        ///<summary>
        /// Esta operación obtiene la cantidad de dinero en la cuenta de origen
        /// y si esta es más que el monto a ser extraído, permite la operación
        /// Extrae la cantidad de origen como débito y depósito en destino como crédito
        /// La operación puede ser ejecutada asíncrona
        ///</summary>
        ///<param name="commit">Si la transacción es verdadera commit, si no rollback</param>
        ///<param name="delayEndTransaction">Demora artificial para terminar la transacción</param>
        ///<param name="level">El nivel de aislamiento</param>
        ///<param name="TransactionName">Un nombre arbitrario para la transacción</param>
        ///<return>La tarea. Cuando la tarea es finalizada devuelve el valor verdadero si está bien</return>
        public Task<bool> ExecuteTransaction(bool commit, int delayEndTransaction, IsolationLevel level, string TransactionName)
        {
            return Task<bool>.Run(() =>
            {
                // Obtener la cadena de conexión
                string connStr1 = ConfigurationManager.ConnectionStrings["Bank1"].ConnectionString;

                // Crear la conexión
                var conn = new SqlConnection(connStr1);

                // Prepara para leer la disponibilidad de dinero en origen
                var command = new SqlCommand(sqlAccount1Fund, conn);
                command.Parameters.AddWithValue("@OriginAccount", _originAccount);

                // Abrir la conexión con la BD
                conn.Open();

                // Crea la transacción y se alista con eso el comando
                var transaction = conn.BeginTransaction(level, TransactionName);
                command.Transaction = transaction;
                try
                {
                    var funds = Convert.ToDecimal(command.ExecuteScalar());
                    Console.WriteLine($"{TransactionName} => fund: {funds} amount: {_amount}");
                    if(funds > _amount)
                    {
                        // Extrae el dinero de origen
                        command = new SqlCommand(sqlWithdrawCredit, conn)
                        {
                            Transaction = transaction
                        };
                        command.Parameters.AddWithValue("@OriginAccount", _originAccount);
                        command.Parameters.AddWithValue("@Debit", _amount);
                        command.ExecuteNonQuery();

                        // Deposita el dinero en destino
                        command = new SqlCommand(sqlFundCredit, conn)
                        {
                            Transaction = transaction
                        };
                        command.Parameters.AddWithValue("@DestinationAccount", _destinationAccount);
                        command.Parameters.AddWithValue("@Credit", _amount);
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
