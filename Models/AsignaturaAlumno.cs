using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppNano.Models
{

    public class AsignaturaAlumno
    {
        [Key]
        public int AsignaturaAlumnoID { get; set; }
        public int AlumnoID { get; set; }
        public int AsignaturaID { get; set; }

    }

    public class VistaAsignaturaAlumno
    {
        public int AsignaturaAlumnoID { get; set; }
        public int AlumnoID { get; set; }
        public string? NombreAlumno { get; set; }
        public string? NombreAsignatura { get; set; }

        public int AsignaturaID { get; set; }
        

    }
}