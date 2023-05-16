using DataLibrary;
using System.Data;

var transact1 = new TransactionOperation(10, 20, 200);
var transact2 = new TransactionOperation(20, 30, 300);
var t1 = transact1.ExecuteTransaction(false, 5000, IsolationLevel.ReadCommitted, "T1");

Thread.Sleep(1500);
var t2 = transact2.ExecuteTransaction(true, 0, IsolationLevel.ReadUncommitted, "T2");

Task.WhenAll(t1, t2).ContinueWith(t =>
{
    Console.WriteLine($"Transaction1: {t1.Result} Transaction2: {t2.Result}");
});

Console.ReadLine();