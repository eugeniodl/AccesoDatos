using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ejemplo02.Models;

public partial class Student
{
    [Key]
    public int StudentId { get; set; }

    [StringLength(50)]
    public string? Name { get; set; }

    public bool Registered { get; set; }

    [InverseProperty("Student")]
    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
}
