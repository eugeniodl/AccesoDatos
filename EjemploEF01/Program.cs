


using EjemploEF01.Data;

using (var db = new NorthwindContext())
{
    var customers = db.Customers.ToList();

    foreach (var customer in customers)
    {
        Console.WriteLine($"Company Name = {customer.CompanyName}, " +
            $"Address = {customer.Address}, Phone = {customer.Phone}");
    }

    // Productos que cuestan entre 20 y 50
    var productosPrecioMedio = db.Products
        .Where(p => p.UnitPrice >= 20 && p.UnitPrice <= 50)
        .ToList();

    foreach (var producto in productosPrecioMedio)
    {
        Console.WriteLine($"Producto = {producto.ProductName}, " +
            $"Precio = {producto.UnitPrice}");
    }
}
