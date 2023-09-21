using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AppNano.Models;
using AppNano.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;


namespace AppNano.Controllers;

[Authorize]

public class AlumnosController : Controller
{
    private readonly ILogger<AlumnosController> _logger;
    private readonly ApplicationDbContext _contexto;

    public AlumnosController(ILogger<AlumnosController>? logger, ApplicationDbContext contexto)
    {
        _logger = logger;
        _contexto = contexto;
    }

    public IActionResult Index()
    {
        var carrera = _contexto.Carrera.ToList();
        ViewBag.CarreraID = new SelectList(carrera.OrderBy(p => p.NombreCarrera), "CarreraID", "NombreCarrera");

        return View();
    }

    public JsonResult BuscarAlumnos(int AlumnoID = 0)
    {
        List<VistaAlumno> AlumnoMostrar = new List<VistaAlumno>();

        var alumnos = _contexto.Alumnos.Include(s => s.Carrera).ToList();
        if (AlumnoID > 0)
        {
            alumnos = alumnos.Where( a => a.AlumnoID == AlumnoID ).ToList();
        }
        foreach (var alumno in alumnos.OrderBy( a => a.Nombre))
        {
            var alumnoMostrar = new VistaAlumno
            {
                Nombre = alumno.Nombre,
                AlumnoID = alumno.AlumnoID,
                CarreraID = alumno.Carrera.CarreraID,
                Eliminado = alumno.Eliminado,
                NombreCarrera = alumno.Carrera.NombreCarrera,
                NacimientoAlumno = alumno.NacimientoAlumno,
                NacimientoAlumnoString = alumno.NacimientoAlumnoString,
                NacimientoAlumnoStringInput = alumno.NacimientoAlumnoStringInput
                
            };
            AlumnoMostrar.Add(alumnoMostrar);
        }



        return Json(AlumnoMostrar);
    }

    public JsonResult GuardarAlumno(int AlumnoID, string Nombre, bool Eliminado, int CarreraID, DateTime NacimientoAlumno)
    {
        bool resultado = false;

        if (!string.IsNullOrEmpty(Nombre))
        {

            if (AlumnoID == 0)
            {
                var AlumnoNuevo = _contexto.Alumnos.Where(c => c.Nombre == Nombre).FirstOrDefault();
                if (AlumnoNuevo == null)
                {
                    var AlumnoGuardar = new Alumno
                    {
                        Nombre = Nombre,
                        Eliminado = Eliminado,
                        CarreraID = CarreraID,
                        NacimientoAlumno = NacimientoAlumno

                    };
                    _contexto.Add(AlumnoGuardar);
                    _contexto.SaveChanges();
                    resultado = true;
                }
            }
            else
            {

                var ValdiarAlumno = _contexto.Alumnos.Where(c => c.Nombre == Nombre && c.AlumnoID != AlumnoID).FirstOrDefault();
                if (ValdiarAlumno == null)
                {
                    var Editar = _contexto.Alumnos.Find(AlumnoID);
                    if (Editar != null)
                    {
                        Editar.Nombre = Nombre;
                        Editar.CarreraID = CarreraID;
                        Editar.NacimientoAlumno = NacimientoAlumno;
                        _contexto.SaveChanges();
                        resultado = true;
                    }
                }


            }
        }

        return Json(resultado);
    }

    public JsonResult Deshabilitar(int AlumnoID)
    {
        bool resultado = false;

        var alumno = _contexto.Alumnos.Find(AlumnoID);

        if (alumno != null)
        {
            
            if (alumno.Eliminado == true)
            {
                alumno.Eliminado = false;
                resultado = true;
                _contexto.SaveChanges();
            }
            else
            {
                alumno.Eliminado = true;
                resultado = true;
                _contexto.SaveChanges();
            }
        }

        return Json(resultado);
    }
}
