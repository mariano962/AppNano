using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppNano.Models;

public class Tarea
{
    public int TareaID { get; set; }
    public string? Descripcion { get; set; }
    public string? Titulo { get; set; }

    public DateTime FechaCarga { get; set; }

    [NotMapped]
    public string FechaCargaString { get { return FechaCarga.ToString("dd/MM/yyyy"); } }
    [NotMapped]
    public string FechaCargaStringInput { get { return FechaCarga.ToString("yyyy-MM-dd"); } }

    public DateTime FechaVencimiento { get; set; }

    [NotMapped]
    public string FechaVencimientoString { get { return FechaVencimiento.ToString("dd/MM/yyyy"); } }
    [NotMapped]
    public string FechaVencimientoStringInput { get { return FechaVencimiento.ToString("yyyy-MM-dd"); } }

    public int AsignaturaID { get; set; }

    public virtual Asignatura? Asignatura { get; set; }
}




public class VistaTarea
{
    public int TareaID { get; set; }
    public string? Descripcion { get; set; }
    public string? Titulo { get; set; }
    public DateTime FechaCarga { get; set; }
    public string? FechaCargaString { get; set; }
    public string? FechaCargaStringInput { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public string? FechaVencimientoString { get; set; }
    public string? FechaVencimientoStringInput { get; set; }
    public int AsignaturaID { get; set; }
    public string? NombreAsignatura { get; set; }
}


