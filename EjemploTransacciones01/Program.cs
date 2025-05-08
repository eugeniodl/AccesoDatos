using EjemploTransacciones01.Data;
using EjemploTransacciones01.Models;

const string X_ORIGIN_ACCOUNT = "10";
const string X_DESTINATION_ACCOUNT = "20";

// Iniciar una transacción
using (var dbContext = new BankContext())
{
    using(var transaction = dbContext.Database.BeginTransaction())
    {
        try
        {
            // Transferir fondos de una cuenta a otra
            decimal quantityToTransfer = 200M;

            // Leer el saldo de la cuenta de origen
            var originAccount = dbContext.AccountTransactions
                .Where(a => a.AccountNumber == X_ORIGIN_ACCOUNT)
                .GroupBy(a => a.AccountNumber)
                .Select(g => new { Balance = g.Sum(a => a.Credit) - g.Sum(a => a.Debit) })
                .FirstOrDefault();

            if (originAccount == null || originAccount.Balance < quantityToTransfer)
            {
                throw new Exception($"Fondos insuficientes en la cuenta {X_ORIGIN_ACCOUNT}. " +
                    $"¡Transacción abortada!");
            }

            // Retirar fondos de la cuenta de origen
            var debitTransaction = new AccountTransaction
            {
                AccountNumber = X_ORIGIN_ACCOUNT,
                Debit = quantityToTransfer,
                Credit = 0M
            };
            dbContext.AccountTransactions.Add(debitTransaction);

            // Depositar fondos en la cuenta de destino
            var creditTransaction = new AccountTransaction
            {
                AccountNumber = X_DESTINATION_ACCOUNT,
                Debit = 0M,
                Credit = quantityToTransfer
            };
            dbContext.AccountTransactions.Add(creditTransaction);

            // Guardar cambios en la base de datos
            dbContext.SaveChanges();

            // Provocar una excepción de conexión
            //throw new Exception("Simulación de error de conexión a la base de datos");

            // Confirmar la transacción
            transaction.Commit();
            Console.WriteLine("Fondos transferidos con éxito");
        }
        catch (Exception ex)
        {
            // Revertir la transacción en caso de error
            transaction.Rollback();
            Console.WriteLine($"Ha ocurrido un error, " +
                $"los fondos no se han transferido: {ex.Message}");
        }
    }
}