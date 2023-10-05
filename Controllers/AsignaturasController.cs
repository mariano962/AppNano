using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AppNano.Models;
using AppNano.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace AppNano.Controllers;

public class AsignaturasController : Controller
{
    private readonly ILogger<AsignaturasController> _logger;
    private readonly ApplicationDbContext _contexto;

    public AsignaturasController(ILogger<AsignaturasController>? logger, ApplicationDbContext contexto)
    {
        _logger = logger;
        _contexto = contexto;
    }
        [Authorize]
    public IActionResult Index()
    {
         var carrera = _contexto.Carrera.ToList();
         ViewBag.CarreraID = new SelectList(carrera.OrderBy(p => p.NombreCarrera), "CarreraID", "NombreCarrera");

        return View();
    }

    public JsonResult BuscarAsignatura(int AsignaturaID = 0)
    {
        List<VistaAsignatura> AsignaturaMostrar = new List<VistaAsignatura>();

        var asignaturas = _contexto.Asignaturas.Include(s => s.Carrera).ToList();
        if (AsignaturaID > 0)
        {
            asignaturas = asignaturas.Where(a => a.AsignaturaID == AsignaturaID).ToList();
        }
        foreach (var asignatura in asignaturas.OrderBy(a => a.NombreAsignatura))
        {
            var asignaturaMostrar = new VistaAsignatura
            {
                NombreAsignatura = asignatura.NombreAsignatura,
                AsignaturaID = asignatura.AsignaturaID,
                CarreraID = asignatura.CarreraID,
                NombreCarrera = asignatura.Carrera.NombreCarrera,
                Eliminado = asignatura.Eliminado

            };
            AsignaturaMostrar.Add(asignaturaMostrar);
        }



        return Json(AsignaturaMostrar);
    }

    public JsonResult GuardarAsignatura(int CarreraID, string NombreAsignatura, int AsignaturaID, bool Eliminado)
    {
        bool resultado = false;

        if (!string.IsNullOrEmpty(NombreAsignatura))
        {

            if (AsignaturaID == 0)
            {
                var AsignaturaNueva = _contexto.Asignaturas.Where(c => c.NombreAsignatura == NombreAsignatura).FirstOrDefault();
                if (AsignaturaNueva == null)
                {
                    var AsignaturaGuardar = new Asignatura
                    {
                        CarreraID = CarreraID,
                        NombreAsignatura = NombreAsignatura,
                        AsignaturaID = AsignaturaID,
                        Eliminado = Eliminado
                      
                        

                    };
                    _contexto.Add(AsignaturaGuardar);
                    _contexto.SaveChanges();
                    resultado = true;
                }
            }
            else
            {

                var ValdiarAsignatura = _contexto.Asignaturas.Where(c => c.NombreAsignatura == NombreAsignatura && c.AsignaturaID != AsignaturaID).FirstOrDefault();
                if (ValdiarAsignatura == null)
                {
                    var Editar = _contexto.Asignaturas.Find(AsignaturaID);
                    if (Editar != null)
                    {
                        Editar.NombreAsignatura = NombreAsignatura.ToUpper();
                        Editar.AsignaturaID = AsignaturaID;
                        Editar.CarreraID = CarreraID;
                        _contexto.SaveChanges();
                        resultado = true;
                    }
                }


            }
        }

        return Json(resultado);
    }

     public JsonResult Deshabilitar(int AsignaturaID)
    {
        bool resultado = false;

        var alumno = _contexto.Asignaturas.Find(AsignaturaID);

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