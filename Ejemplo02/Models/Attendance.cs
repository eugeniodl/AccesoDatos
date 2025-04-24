using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ejemplo02.Models;

[Index("StudentId", Name = "IX_Attendances_StudentId")]
public partial class Attendance
{
    [Key]
    public int AttendanceId { get; set; }

    public int StudentId { get; set; }

    public DateOnly Date { get; set; }

    public bool Present { get; set; }

    [ForeignKey("StudentId")]
    [InverseProperty("Attendances")]
    public virtual Student Student { get; set; } = null!;
}
