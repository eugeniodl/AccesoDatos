using NorthwindEF.Models;

using (var context = new NorthwindContext())
{
    //Insertar
    //var p = new Product();
    //p.ProductName = "queso";
    //p.UnitPrice = 60;
    //context.Products.Add(p);
    //context.SaveChanges();

    //Actualizar
    //var p = context.Products.Find(78);
    //p.ProductName = "Queso";
    //context.Entry(p).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
    //context.SaveChanges();

    // Eliminar
    var p = context.Products.Find(78);
    context.Remove(p);
    context.SaveChanges();

    foreach (var item in context.Products.ToList())
    {
        Console.WriteLine("Nombre del producto: {0}, Precio unitario: {1}",item.ProductName,
            item.UnitPrice);
    }
}