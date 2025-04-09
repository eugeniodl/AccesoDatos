using Ejemplo01;
using System.Configuration;

string sqlServerConnectionString =
    ConfigurationManager.ConnectionStrings["constring"].ConnectionString;

IRepository<Customer> repository 
    = new CustomerRepository(sqlServerConnectionString);

IEnumerable<Customer> customers = repository.GetAll();

Console.WriteLine("Clientes de SQL Server");

foreach (Customer customer in customers)
    Console.WriteLine($"Id: {customer.Id}, " +
        $"Nombre: {customer.FirstName}, " +
        $"Apellido: {customer.LastName}");
