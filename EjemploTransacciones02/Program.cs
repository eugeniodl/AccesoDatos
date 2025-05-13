using EjemploTransacciones02.Data;
using EjemploTransacciones02.Models;
using System.Transactions;

const string X_ORIGIN_ACCOUNT = "10"; // María
const string X_DESTINATION_ACCOUNT = "20"; // Juan
const decimal quantityToTransfer = 200M; // Cantidad a transferir

// Habilitar transacciones distribuidas implícitas
TransactionManager.ImplicitDistributedTransactions = true; // <--- for .NET 7

try
{
    using(var scope = new TransactionScope(TransactionScopeOption.Required,
        new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
    {
        using(var dbContext1 = new Bank1Context())
        {
            // Obtener el saldo de la cuenta de origen
            var originAccountBalance = dbContext1.Transactions
                .Where(t => t.AccountNumber == X_ORIGIN_ACCOUNT)
                .GroupBy(t => t.AccountNumber)
                .Select(g => new { Balance = g.Sum(t => t.Credit) - g.Sum(t => t.Debit)})
                .FirstOrDefault();

            if(originAccountBalance == null 
                || originAccountBalance.Balance < quantityToTransfer)
            {
                Console.WriteLine($"Transacción Fallida: " +
                    $"El saldo es menor que el dinero necesario {originAccountBalance?.Balance}");
                return;
            }

            dbContext1.Transactions.Add(new AccountTransaction
            {
                AccountNumber = X_ORIGIN_ACCOUNT,
                Credit = 0M,
                Debit = quantityToTransfer
            });
            dbContext1.SaveChanges();
            Console.WriteLine($"Retiro de la cuenta 10 con éxito");
        }
        // Realizar la operación de depósito en la cuenta 20 en Bank2
        using(var dbContext2 = new Bank2Context())
        {
            // Depositar el dinero en la cuenta de destino
            dbContext2.Transactions.Add(new AccountTransaction
            {
                AccountNumber = X_DESTINATION_ACCOUNT,
                Credit = quantityToTransfer,
                Debit = 0M
            });
            dbContext2.SaveChanges();
            Console.WriteLine($"Depósito en la cuenta 20 con éxito");
        }
        // Confirmar la transacción
        scope.Complete();
        Console.WriteLine($"Transacción confirmada");
    }
}
catch(Exception ex)
{
    Console.WriteLine($"Ha ocurrido un error, la transacción no se ha completado: " +
        $"{ex.Message}");
    // La transacción se revierte automáticamente si ocurre una excepción
}

// Recuperar los saldos actualizados
using (var dbContext1 = new Bank1Context())
using (var dbContext2 = new Bank2Context())
{
    var emitterBalance = dbContext1.Transactions
                .Where(a => a.AccountNumber == X_ORIGIN_ACCOUNT)
                .GroupBy(a => a.AccountNumber)
                .Select(g => new { Balance = g.Sum(a => a.Credit) - g.Sum(a => a.Debit) })
                .FirstOrDefault();

    var receiverBalance = dbContext2.Transactions
                .Where(a => a.AccountNumber == X_DESTINATION_ACCOUNT)
                .GroupBy(a => a.AccountNumber)
                .Select(g => new { Balance = g.Sum(a => a.Credit) - g.Sum(a => a.Debit) })
                .FirstOrDefault();

    Console.WriteLine($"Fondos del emisor: {emitterBalance?.Balance} " +
        $"Fondos del receptor: {receiverBalance?.Balance}");
}
