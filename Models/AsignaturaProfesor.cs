using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppNano.Models
{

    public class AsignaturaProfesor
    {
        [Key]
        public int AsignaturaProfesorID { get; set; }
        public int ProfesorID { get; set; }
        public int AsignaturaID { get; set; }
 
    }

    public class VistaAsignaturaProfesor
    {
        public int AsignaturaProfesorID { get; set; }
        public int ProfesorID { get; set; }
        public string? NombreProfesor { get; set; }
        public string? NombreAsignatura { get; set; }

        public int AsignaturaID { get; set; }
        

    }
}