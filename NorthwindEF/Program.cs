using Microsoft.EntityFrameworkCore;
using NorthwindEF.Models;

using (var context = new NorthwindContext())
{
    // Para insertar
    //var p = new Product();
    //p.ProductName = "Arroz";
    //p.UnitPrice = 20;
    //context.Products.Add(p);
    //context.SaveChanges();

    // Para modificar
    //var p = context.Products.Find(79);
    //p.UnitPrice = 30;
    //context.Entry(p).State = EntityState.Modified;
    //context.SaveChanges();

    // Para eliminar
    var p = context.Products.Find(79);
    context.Remove(p);
    context.SaveChanges();


    foreach (var product in context.Products.ToList())
    {
        Console.WriteLine("Nombre del producto: {0}, Precio unitario: {1}", 
            product.ProductName, product.UnitPrice);
    }
}