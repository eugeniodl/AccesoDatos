

using EjemploEF02.Data;
using EjemploEF02.Models;

using (var db = new LibraryContext())
{
    // Crear autores
    var author1 = new Author { Name = "Stephen King" };
    var author2 = new Author { Name = "J.K. Rowling" };
    db.Authors.AddRange(author1, author2);
    db.SaveChanges();

    var book1 = new Book { Title = "IT", AuthorId = 1 };
    var book2 = new Book
    {
        Title = "Harry Potter and The Philopher's Stone",
        AuthorId = 2
    };
    db.Books.Add(book1);
    db.Books.Add(book2);
    db.SaveChanges();
}