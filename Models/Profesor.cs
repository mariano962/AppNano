using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppNano.Models;

public class Profesor{
    [Key]
    public int ProfesorID { get; set; }
    public string? Nombre { get; set; }
    public string? DniProfesor { get; set; }
    public DateTime NacimientoProfesor { get; set; }
    public string? CorreoElectronico { get; set; }
    public string? Direccion { get; set; }
    public bool Eliminado { get; set; }

    public ICollection<AsignaturaProfesor>? AsignaturaProfesores { get; set; }
    public ICollection<Tarea>? Tareas { get; set; }

    [NotMapped]
    public string NacimientoProfesorString { get { return NacimientoProfesor.ToString("dd/MM/yyyy"); } }
    [NotMapped]
    public string NacimientoProfesorStringInput { get { return NacimientoProfesor.ToString("yyyy-MM-dd"); } }

   
}