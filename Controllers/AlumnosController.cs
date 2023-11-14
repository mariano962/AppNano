using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AppNano.Models;
using AppNano.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


namespace AppNano.Controllers;

[Authorize]

public class AlumnosController : Controller
{
    private readonly ILogger<AlumnosController> _logger;
    private readonly ApplicationDbContext _contexto;
    private readonly UserManager<IdentityUser> _userManager;

    public AlumnosController(ILogger<AlumnosController>? logger, ApplicationDbContext contexto, UserManager<IdentityUser> userManager)
    {
        _logger = logger;
        _contexto = contexto;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        var carrera = _contexto.Carrera.ToList();
        ViewBag.CarreraID = new SelectList(carrera.OrderBy(p => p.NombreCarrera), "CarreraID", "NombreCarrera");

        var asignatura = _contexto.Asignaturas.ToList();
        ViewBag.AsignaturaID = new SelectList(asignatura.OrderBy(p => p.NombreAsignatura), "AsignaturaID", "NombreAsignatura");

        return View();
    }

    public JsonResult BuscarAlumnos(int AlumnoID = 0)
    {
        List<VistaAlumno> AlumnoMostrar = new List<VistaAlumno>();

        var alumnos = _contexto.Alumnos.Include(s => s.Carrera).ToList();
        if (AlumnoID > 0)
        {
            alumnos = alumnos.Where(a => a.AlumnoID == AlumnoID).ToList();
        }
        foreach (var alumno in alumnos.OrderBy(a => a.Carrera.NombreCarrera).ThenBy(a => a.Nombre))
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
                NacimientoAlumnoStringInput = alumno.NacimientoAlumnoStringInput,
                DniAlumno = alumno.DniAlumno,
                Correo = alumno.Correo,
                Direccion = alumno.Direccion,

            };
            AlumnoMostrar.Add(alumnoMostrar);
        }



        return Json(AlumnoMostrar);
    }

    public async Task<JsonResult> GuardarAlumno(int AlumnoID, string Nombre, bool Eliminado, int CarreraID, DateTime NacimientoAlumno, string DniAlumno, string Correo, string Direccion)
    {
        bool resultado = false;

        if (!string.IsNullOrEmpty(Nombre))
        {

            if (AlumnoID == 0)
            {
                var AlumnoNuevo = _contexto.Alumnos.Where(c => c.DniAlumno == DniAlumno).FirstOrDefault();
                var usuarioAlumno = await _contexto.Users.Where(u => u.Email == Correo).FirstOrDefaultAsync();
                if (AlumnoNuevo == null && usuarioAlumno == null)
                {
                    // crear el usuario de profesor
                    var user = new IdentityUser { UserName = Correo, Email = Correo };
                    var result = await _userManager.CreateAsync(user, DniAlumno.ToString());
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "Estudiante");
                        var AlumnoGuardar = new Alumno
                        {
                            Nombre = Nombre,
                            Eliminado = Eliminado,
                            CarreraID = CarreraID,
                            NacimientoAlumno = NacimientoAlumno,
                            DniAlumno = DniAlumno,
                            Correo = Correo,
                            Direccion = Direccion,
                            UsuarioID = user.Id
                        };
                        _contexto.Add(AlumnoGuardar);
                        _contexto.SaveChanges();

                        resultado = true;
                    } 
                }
            }
            else
            {

                var ValdiarAlumno = _contexto.Alumnos.Where(c => c.DniAlumno == DniAlumno && c.AlumnoID != AlumnoID).FirstOrDefault();
                if (ValdiarAlumno == null)
                {
                    var Editar = _contexto.Alumnos.Find(AlumnoID);
                    if (Editar != null)
                    {
                        Editar.Nombre = Nombre;
                        Editar.CarreraID = CarreraID;
                        Editar.NacimientoAlumno = NacimientoAlumno;
                        Editar.DniAlumno = DniAlumno;
                        // Editar.Correo = Correo;
                        Editar.Direccion = Direccion;
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

//ASIGNATURAS ALUMNO

    public JsonResult BuscarMaterias(int AsignaturaAlumnoID = 0)
    {
        List<VistaAsignaturaAlumno> asignaturaAlumnoMostrar = new List<VistaAsignaturaAlumno>();
        var asignaturas = _contexto.AsignaturaAlumnos.Where(a => a.AlumnoID == AsignaturaAlumnoID).ToList();


        foreach (var asignatura in asignaturas.OrderBy(a => a.AsignaturaAlumnoID))
        {
            var asignaturaNombre = _contexto.Asignaturas.Where(a => a.AsignaturaID == asignatura.AsignaturaID).Select(a => a.NombreAsignatura).SingleOrDefault();
            var asignaturaAlumnomostrar = new VistaAsignaturaAlumno
            {
                AlumnoID = asignatura.AlumnoID,
                AsignaturaID = asignatura.AsignaturaID,
                AsignaturaAlumnoID = asignatura.AsignaturaAlumnoID,
                NombreAsignatura = asignaturaNombre,



            };
            asignaturaAlumnoMostrar.Add(asignaturaAlumnomostrar);
        }



        return Json(asignaturaAlumnoMostrar);
    }

public JsonResult GuardarMateria(int AlumnoID, int AsignaturaID)
{
    bool resultado = false;

    // Verificar si el alumno ya tiene la misma materia asignada
    var asignaturaExistente = _contexto.AsignaturaAlumnos.FirstOrDefault(aa => aa.AlumnoID == AlumnoID && aa.AsignaturaID == AsignaturaID);

    if (asignaturaExistente == null)
    {
        // No existe la misma materia asignada, se puede agregar
        var AlumnoAsignaturaGuardar = new AsignaturaAlumno
        {
            AlumnoID = AlumnoID,
            AsignaturaID = AsignaturaID,
        };

        _contexto.Add(AlumnoAsignaturaGuardar);
        _contexto.SaveChanges();
        resultado = true;
    }
    // Si ya existe la misma materia asignada, resultado permanece como false

    return Json(resultado);
}


    public JsonResult EliminarMateria(int AsignaturaAlumnoID)
    {


        var eliminarAsignaturaAlumno = _contexto.AsignaturaAlumnos.Where(b => b.AsignaturaAlumnoID == AsignaturaAlumnoID).FirstOrDefault();

        var resultado = 0;

        if (eliminarAsignaturaAlumno != null)
        {


            _contexto.AsignaturaAlumnos.Remove(eliminarAsignaturaAlumno);
            _contexto.SaveChanges();
            resultado = 1;

        }
        return Json(resultado);
    }


}