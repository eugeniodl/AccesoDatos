using Ejemplo02.Data;

using (var db = new SchoolContext())
{
    var students = db.Students.ToList();

    foreach (var student in students)
    {
        Console.WriteLine($"Nombre = {student.Name}, " +
            $"Registered = {student.Registered}");
    }

    var attendance = db.Attendances.ToList();

    foreach (var item in attendance)
    {
        Console.WriteLine("Fecha: {0}, Estudiante: {1}, Presente {2} ",
            item.Date, item.StudentId, item.Present);
    }

    // Filtro y Ordenación
    Console.WriteLine("Estudiantes registrados, " +
        "ordenados alfabéticamente por su nombre");
    var registeredStudents = db.Students
                                  .Where(s => s.Registered)
                                  .OrderBy(s => s.Name)
                                  .ToList();
    foreach (var student in registeredStudents)
    {
        Console.WriteLine($"{student.Name}");
    }

    // Coincidencia de patrones con LIKE
    Console.WriteLine("Estudiantes cuyo nombre comience con 'B'");
    var studentsStartingWithB = db.Students
                                   .Where(s => s.Name.StartsWith("B"))
                                   .ToList();
    foreach (var student in studentsStartingWithB)
    {
        Console.WriteLine($"{student.Name}");
    }

    // Filtros globales

    registeredStudents = db.GetRegisteredStudents().ToList();

    studentsStartingWithB = db.GetRegisteredStudents()
                              .Where(s => s.Name.StartsWith("B"))
                              .ToList();

    // Obtener los estudiantes cuyo nombre contenga 'a'
    // ordenados alfabéticamente por su nombre

    var studentsWithAInName = db.Students
                                .Where(s => s.Name.Contains("a"))
                                .OrderBy(s => s.Name)
                                .ToList();
    foreach (var item in studentsWithAInName)
    {
        Console.WriteLine($"{item.Name}");
    }

    Console.WriteLine("\nObtener los estudiantes que hayan asistido el día de hoy," +
        " ordenados por su ID de asistencia (AttendanceID)");
    var studentsPresentToday = db.GetPresentStudents()
        .Where(a => a.Date == DateOnly.FromDateTime(DateTime.Today))
        .OrderBy(a => a.AttendanceId)
        .ToList();
    foreach (var student in studentsPresentToday)
    {
        Console.WriteLine($"{student.Student.Name}");
    }

    var studentsPresent = db.GetPresentStudents()
        .Join(db.Students,
        a => a.StudentId,
        s => s.StudentId,
        (a, s) => new
        {
            a.Date,
            Name = s.Name
        })
        .Where(a => a.Date == DateOnly.FromDateTime(DateTime.Today))
        .ToList();

    foreach (var student in studentsPresent)
    {
        Console.WriteLine($"{student.Name}");
    }

    Console.WriteLine("\nMostrar los detalles de las asistencias " +
        "(AttendanceID, StudentID, Date, Present) " +
        "junto con los nombres de los estudiantes " +
        "correspondientes de la tabla Attendance y Students respectivamente");

    var attendanceDetailsWithStudentName = db.Attendances
        .Select(a => new
        {
            a.AttendanceId,
            a.StudentId,
            a.Date,
            a.Present,
            StudentName = a.Student.Name
        }).ToList();

    Console.WriteLine("\nObtener todos los estudiantes " +
        "junto con el número total de asistencia que han registrado cada uno");
    var studentsWithTotalAttendance = db.GetPresentStudents()
                                      .GroupBy(a => a.Student)
                                      .Select(g => new
                                      {
                                          Estudiante = g.Key.Name,
                                          Asistencia = g.Count()
                                      }).ToList();

    foreach (var item in studentsWithTotalAttendance)
    {
        Console.WriteLine($"{item.Estudiante} {item.Asistencia}");
    }

    Console.WriteLine("\nEncontrar el día con la mayor cantidad de asistencia");
    var dayWithMostAttendance = db.GetPresentStudents()
                                .GroupBy(a => a.Date)
                                .OrderByDescending(g => g.Count())
                                .Select(g => g.Key)
                                .FirstOrDefault();
    Console.WriteLine(dayWithMostAttendance);
}