using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppNano.Models;

public class Alumno
{
    [Key]
    public int AlumnoID { get; set; }
    public string? Nombre { get; set; }
    public string? DniAlumno { get; set; }
    public string? Correo { get; set; }
    public bool Eliminado { get; set; }
    public int CarreraID { get; set; }

    public DateTime NacimientoAlumno { get; set; }

    [NotMapped]
    public string NacimientoAlumnoString { get { return NacimientoAlumno.ToString("dd/MM/yyyy"); } }
    [NotMapped]
    public string NacimientoAlumnoStringInput { get { return NacimientoAlumno.ToString("yyyy-MM-dd"); } }

   
    public virtual Carrera? Carrera { get; set; }

}

public class VistaAlumno
{
    public int AlumnoID { get; set; }
    public string? Nombre { get; set; }
    public bool Eliminado { get; set; }
    public int CarreraID { get; set; }
    public string? DniAlumno { get; set; }
    public string? Correo { get; set; }
    public string? NombreCarrera { get; set; }
    public DateTime NacimientoAlumno { get; set;}
    public string? NacimientoAlumnoString { get; set; }
    public string? NacimientoAlumnoStringInput { get; set; }
}
