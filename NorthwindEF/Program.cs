using Microsoft.EntityFrameworkCore;
using NorthwindEF.Models;

using (var db = new NorthwindContext())
{
    // Para insertar
    //var p = new Product();
    //p.ProductName = "Jabón";
    //p.UnitPrice = 40;
    //db.Products.Add(p);
    //db.SaveChanges();

    // Para modificar
    //var p = db.Products.Find(78);
    //p.UnitPrice = 50;
    //db.Entry(p).State = EntityState.Modified;
    //db.SaveChanges();

    // Para eliminar
    var p = db.Products.Find(78);
    db.Remove(p);
    db.SaveChanges();

    var productos = db.Products.ToList();
    foreach (var producto in productos)
    {
        Console.WriteLine("Nombre: {0}, Precio: {1}",
            producto.ProductName, producto.UnitPrice);
    }
}