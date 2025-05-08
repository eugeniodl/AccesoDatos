using EjemploTransacciones02.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjemploTransacciones02.Data
{
    public class Bank2Context : DbContext
    {
        public DbSet<AccountTransaction> AccountTransactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configurar la cadena de conexión para Bank2Context
            string connection =
                ConfigurationManager.ConnectionStrings["constring2"].ConnectionString;
            optionsBuilder.UseSqlServer(connection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountTransaction>().HasData(
                new AccountTransaction()
                {
                    Id = 1,
                    AccountNumber = "20",
                    Credit = 400M,
                    Debit = 0M
                });


            modelBuilder.Entity<AccountTransaction>()
                .Property(a => a.Credit)
                .HasColumnType("decimal(18,2)");


            modelBuilder.Entity<AccountTransaction>()
                .Property(a => a.Debit)
                .HasColumnType("decimal(18,2)");
        }
    }
}
