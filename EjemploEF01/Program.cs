


using EjemploEF01.Data;

using (var db = new NorthwindContext())
{
    var customers = db.Customers.ToList();

    foreach (var customer in customers)
    {
        Console.WriteLine($"Company Name = {customer.CompanyName}, " +
            $"Address = {customer.Address}, Phone = {customer.Phone}");
    }
}
