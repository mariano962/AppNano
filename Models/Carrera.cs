using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppNano.Models{

    public class Carrera{

        [Key]
        public int CarreraID { get; set; }
        public string? NombreCarrera { get; set; }
        public string? Duracion { get; set; }

        public bool Eliminado { get; set; }

        //Relacion virtual con alumno
        public ICollection<Alumno>? Alumnos{ get; set; }
    }
    

  

    public class VistaCarrera {
       public int CarreraID { get; set; }
       public string? NombreCarrera { get; set; }
       public string? Duracion { get; set; }
       public bool Eliminado { get; set; }

    }
}