using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AppNano.Models;
using AppNano.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace AppNano.Controllers;

public class TareasController : Controller
{
    private readonly ILogger<TareasController> _logger;
    private readonly ApplicationDbContext _contexto;

    public TareasController(ILogger<TareasController>? logger, ApplicationDbContext contexto)
    {
        _logger = logger;
        _contexto = contexto;
    }

    public IActionResult Index()
    {
         var asignatura = _contexto.Asignaturas.ToList();
         ViewBag.AsignaturaID = new SelectList(asignatura.OrderBy(p => p.NombreAsignatura), "AsignaturaID", "NombreAsignatura");

        return View();
    }

     [Authorize]

    public JsonResult BuscarTareas(int TareaID = 0)
    {
        List<VistaTarea> TareaMostrar = new List<VistaTarea>();

        var tareas = _contexto.Tareas.Include(s => s.Asignatura).ToList();
        if (TareaID > 0)
        {
            tareas = tareas.Where( a => a.TareaID == TareaID ).ToList();
        }
        foreach (var tarea in tareas.OrderBy( a => a.Titulo))
        {
            var tareaMostrar = new VistaTarea
            {
                TareaID = tarea.TareaID,
                Titulo = tarea.Titulo,
                Descripcion = tarea.Descripcion,
                FechaCarga = tarea.FechaCarga,
                FechaCargaString = tarea.FechaCargaString,
                FechaCargaStringInput = tarea.FechaCargaStringInput,
                FechaVencimiento = tarea.FechaVencimiento,
                FechaVencimientoString = tarea.FechaVencimientoString,
                FechaVencimientoStringInput = tarea.FechaVencimientoStringInput
                
            };
            TareaMostrar.Add(tareaMostrar);
        }



        return Json(TareaMostrar);
    }

    public JsonResult GuardarTarea(int TareaID, string Descripcion, string Titulo, DateTime FechaCarga, DateTime FechaVencimiento)
    {
     

        bool resultado = false;
        if (!string.IsNullOrEmpty(Descripcion))
        {
            //SI ES 0 QUIERE DECIR QUE ESTA CREANDO LA LOCALIDAD
            if (TareaID == 0)
            {
                //BUSCAMOS EN LA TABLA SI EXISTE UNA CON EL MISMO NOMBRE
                var TareaOriginal = _contexto.Tareas.Where(b => b.Descripcion == Descripcion).Count();
                if (TareaOriginal == 0)
                {
                    //DECLAMOS EL OBJETO DANDO EL VALOR 
                    var TareaGuardar = new Tarea
                    {
                        TareaID = TareaID,
                        Descripcion = Descripcion.ToUpper(),
                        Titulo = Titulo.ToUpper(),
                        FechaCarga = FechaCarga,
                        FechaVencimiento = FechaVencimiento,
                    };

                    TareaGuardar.Descripcion = TareaGuardar.Descripcion.ToUpper();
                    _contexto.Add(TareaGuardar);
                    _contexto.SaveChanges();
                    resultado = true;
                }
            }

            else
            {
                //BUSCAMOS EN LA TABLA SI EXISTE UNA CON EL MISMO NOMBRE Y DISTINTO ID DE REGISTRO AL QUE ESTAMOS EDITANDO
                var tareaOriginal = _contexto.Tareas.Where(b => b.Descripcion == Descripcion && b.TareaID != TareaID).FirstOrDefault();
                if (tareaOriginal == null)
                {
                    //CREAR VARIABLE QUE GUARDE EL OBJETO SEGUN EL ID DESEADO
                    var tareaEditar = _contexto.Tareas.Find(TareaID);
                    if (tareaEditar != null)
                    {
                        tareaEditar.Descripcion = Descripcion.ToUpper();
                        tareaEditar.Titulo = Titulo.ToUpper();
                        tareaEditar.FechaCarga = FechaCarga;
                        tareaEditar.FechaVencimiento = FechaVencimiento;
                        _contexto.SaveChanges();
                        resultado = true;
                    }
                }


            }

        }

        return Json(resultado);
    }


}

