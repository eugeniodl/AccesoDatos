


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

    // 35. Empleados con cargo 'Sales Representative'

    var empleadoVentas = db.Employees
        .Where(e => e.Title == "Sales Representative")
        .ToList();

    Console.WriteLine("Empleados con cargo 'Sales Representative'");
    foreach (var empleado in empleadoVentas)
    {
        Console.WriteLine($"{empleado.FirstName} " +
            $"{empleado.LastName}, {empleado.Title}");
    }

    // 5. Productos y su categoría
    var productosConCategoria = db.Products
        .Select(p => new {
            p.ProductName,
            Categoria = p.Category.CategoryName
        })
        .ToList();

    // 8. Primer pedido de cada cliente
    var primerPedidoCliente = db.Orders
        .GroupBy(o => o.CustomerId)
        .Select(g => g.OrderBy(o => o.OrderDate)
        .FirstOrDefault())
        .ToList();


    // 17. Suma de ventas por cliente
    var ventasPorCliente = db.Orders
        .Where(o => o.OrderDetails.Any())
        .GroupBy(o => o.CustomerId)
        .Select(g => new
        {
            ClienteID = g.Key,
            TotalVentas = g.Sum(o => o.OrderDetails
            .Sum(d => d.Quantity * d.UnitPrice))
        }).ToList();

    // 21. Productos descontinuados
    var productosDescontinuados = db.Products
        .Where(p => p.Discontinued).ToList();

    // 28. Órdenes que fueron enviadas tarde (ShippedDate > RequiredDate) 
    var ordenesTarde = db.Orders
        .Where(o => o.ShippedDate.HasValue
        && o.RequiredDate.HasValue
        && o.ShippedDate > o.RequiredDate).ToList();

    // 24. Empleados que reportan directamente al jefe con ID 2 
    var empleadosReportan2 = db.Employees
        .Where(e => e.ReportsTo == 2).ToList();

}
