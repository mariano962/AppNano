using AppNano.Controllers;
using AppNano.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppNano.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Alumno> Alumnos { get; set; }
    public DbSet<Carrera> Carrera { get; set; }
    public DbSet<Profesor> Profesor { get; set; }
    public DbSet<Asignatura> Asignaturas { get; set; }
    public DbSet<Tarea> Tareas { get; set; }
    public DbSet<AsignaturaProfesor> AsignaturaProfesores { get; set; }
    public DbSet<AsignaturaAlumno> AsignaturaAlumnos { get; set; }
    
}

