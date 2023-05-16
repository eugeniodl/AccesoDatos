﻿using SchoolEF.Data;
using SchoolEF.Models;

using (var context = new SchoolContext())
{
    using (var trans = context.Database.BeginTransaction())
    {
        try
        {
            Grade grade = new Grade();
            grade.GradeName = "Primero";
            grade.Section = "F";
            context.Grades.Add(grade);
            context.SaveChanges();

            Student student = new Student();
            student.FirstName = "Leonardo";
            student.LastName = "Téllez";
            student.DateOfBirth = DateTime.Now;
            student.Photo = Convert.FromBase64String("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");
            student.Height = 150;
            student.Weight = 150;
            student.GradeId = 4;
            context.Students.Add(student);
            context.SaveChanges();

            trans.Commit();
        }
        catch (Exception ex)
        {
            trans.Rollback();
            Console.WriteLine($"Error: {ex.Message}");
        } 
    }

    // Mostrar estudiantes del Primero A
    //var innerJoin = context.Students.Join(
    //    context.Grades,
    //    s => s.GradeId,
    //    g => g.Id,
    //    (s, g) => new
    //    {
    //        StudentFirstName = s.FirstName,
    //        StudentLastName = s.LastName,
    //        GradeName = g.GradeName,
    //        Section = g.Section
    //    }).Where(g => g.GradeName == "Primero" && g.Section == "A");

    //foreach (var inner in innerJoin.ToList())
    //{
    //    Console.WriteLine($"Nombre: {inner.StudentFirstName} {inner.StudentLastName}\t" +
    //        $"Grado: {inner.GradeName} {inner.Section}");
    //}
}