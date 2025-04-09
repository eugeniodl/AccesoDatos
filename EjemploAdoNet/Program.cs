





using System.Configuration;

string sqlServerConnectionString =
    ConfigurationManager.ConnectionStrings["constring"].ConnectionString;

IRepository<Cliente> repository
    = new ClienteRepository(sqlServerConnectionString);

IEnumerable<Cliente> list = repository.GetAll();

Console.WriteLine("Cliente de SQL Server");

foreach (Cliente cliente in list)
    Console.WriteLine($"Id Cliente: {cliente.IdCliente}, " +
        $"RUT: {cliente.RUT}, Nombre Completo: {cliente.NombreCompleto}, " +
        $"Teléfono: {cliente.Telefono}");