using System;
using System.Collections.Generic;
using System.Configuration;
using Ejemplo02.Models;
using Microsoft.EntityFrameworkCore;

namespace Ejemplo02.Data;

public partial class SchoolContext : DbContext
{
    public SchoolContext()
    {
    }

    public SchoolContext(DbContextOptions<SchoolContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attendance> Attendances { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public IQueryable<Student> GetRegisteredStudents()
        => Students.Where(s => s.Registered);

    public IQueryable<Attendance> GetPresentStudents()
        => Attendances.Where(s => s.Present);

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connection =
            ConfigurationManager.ConnectionStrings["constring"].ConnectionString;
        optionsBuilder.UseSqlServer(connection);
    }




    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
