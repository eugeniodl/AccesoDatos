using System.Configuration;
using System.Data;

string sqlServerConnectionString =
    ConfigurationManager.ConnectionStrings["constring"].ConnectionString;

IRepository<Tabla> repository = new TablaRepository(sqlServerConnectionString);

IEnumerable<Tabla> tabs = repository.GetAll();

Console.WriteLine("Tabla de SQL Server");

foreach (Tabla tab in tabs)
{
    Console.WriteLine($"IdRow: {tab.IdRow}, Nombre: {tab.Nombre}, " +
        $"Apellido: {tab.Apellido}");
}