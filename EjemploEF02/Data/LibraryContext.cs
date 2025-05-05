using EjemploEF02.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjemploEF02.Data
{
    public class LibraryContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connection =
                ConfigurationManager.ConnectionStrings["constring"].ConnectionString;
            optionsBuilder.UseSqlServer(connection);
        }
    }
}
