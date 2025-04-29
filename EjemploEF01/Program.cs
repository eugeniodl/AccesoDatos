

using EjemploEF01.Data;

using (var context = new NorthwindContext())
{
    // Todos los productos
    var productos = context.Products.ToList();

    foreach (var producto in productos)
    {
        Console.WriteLine($"Nombre del Producto: {producto.ProductName}, " +
            $"Cantidad: {producto.QuantityPerUnit}, Precio: {producto.UnitPrice}");
    }

    // Productos cuyo precio sea mayor a 50
    var productosCaros = context.Products.Where(p => p.UnitPrice > 50).ToList();

    // Listar categorías de productos
    var categorias = context.Categories
        .Select(c => new { c.CategoryId, c.CategoryName}).ToList();

    // Productos y su categoría
    var productosConCategoria = context.Products
        .Select(p => new { p.ProductName, Categoria = p.Category.CategoryName })
        .ToList();

    // Pedidos hechos en 1997
    var pedidos1997 = context.Orders
        .Where(o => o.OrderDate.HasValue && o.OrderDate.Value.Year == 1997).ToList();

    // Primer pedido de cada cliente
    var primerPedidoCliente = context.Orders
        .GroupBy(o => o.CustomerId)
        .Select(g => g.OrderBy(o => o.OrderDate).FirstOrDefault())
        .ToList();

    // Clientes sin pedidos
    var clientesSinPedidos = context.Customers.Where(c => !c.Orders.Any()).ToList();

    // Suma de ventas por cliente
    var ventasPorCliente = context.Orders
        .Where(o => o.OrderDetails.Any())
        .GroupBy(o => o.CustomerId)
        .Select(g => new
        {
            ClienteID = g.Key,
            TotalVentas = g.Sum(o => o.OrderDetails.Sum(d => d.Quantity * d.UnitPrice))
        })
        .ToList();

    // Clientes cuyo nombre empieza con 'A'
    var clientesEmpiezanA = context.Customers
        .Where(c => c.CompanyName.StartsWith("A"))
        .ToList();

   // Productos cuyo nombre contiene "Sea"
   var productosSea = context.Products
        .Where(p => p.ProductName.Contains("Sea"))
        .ToList();
}