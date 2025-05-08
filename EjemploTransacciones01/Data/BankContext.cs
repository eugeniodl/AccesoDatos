using EjemploTransacciones01.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjemploTransacciones01.Data
{
    // Context de base de datos BankContext
    public class BankContext : DbContext
    {
        public DbSet<AccountTransaction> AccountTransactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configurar la cadena de conexión para BankContext
            string connection =
                ConfigurationManager.ConnectionStrings["constring"].ConnectionString;
            optionsBuilder.UseSqlServer(connection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountTransaction>().HasData(
                new AccountTransaction()
                {
                    Id = 1,
                    AccountNumber = "10",
                    Credit = 400M,
                    Debit = 0M
                });
        }
    }
}
