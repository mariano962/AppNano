using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AppNano.Models;
using AppNano.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

[Authorize]

public class ProfesoresController : Controller
{
    private readonly ILogger<ProfesoresController> _logger;
    private ApplicationDbContext _contexto;

    public ProfesoresController(ILogger<ProfesoresController>? logger, ApplicationDbContext contexto)
    {
        _logger = logger;
        _contexto = contexto;
    }

    public IActionResult Index()
    {
        return View();
    }

    public JsonResult BuscarProfesores(int ProfesorID, string Nombre, string CorreoElectronico, string DniProfesor, DateTime NacimientoProfesor)
    {
        var profesor = _contexto.Profesor.ToList();
        if (ProfesorID > 0)
        {

            profesor = profesor.Where(c => c.ProfesorID == ProfesorID).ToList();

        }

        return Json(profesor);
    }

    public JsonResult GuardarProfesor(int ProfesorID, string Nombre, string CorreoElectronico, string DniProfesor, DateTime NacimientoProfesor, bool Eliminado)
    {
        bool resultado = false;

        if (!string.IsNullOrEmpty(Nombre))
        {

            if (ProfesorID == 0)
            {
                var validarDni = _contexto.Profesor.Where(c => c.DniProfesor == DniProfesor).Count();
               if (validarDni == 0)
               {
                    var ProfesorNuevo = _contexto.Profesor.Where(c => c.Nombre == Nombre).FirstOrDefault();
                    if (ProfesorNuevo == null)
                    {
                        var ProfesorGuardar = new Profesor
                        {
                            ProfesorID = ProfesorID,
                            Nombre = Nombre,
                            CorreoElectronico = CorreoElectronico,
                            DniProfesor = DniProfesor,
                            Eliminado = Eliminado,
                            NacimientoProfesor = NacimientoProfesor,


                        };
                        _contexto.Add(ProfesorGuardar);
                        _contexto.SaveChanges();
                        resultado = true;
                    }
                
               }
                

                
            }
            else
            {
                var ProfesorExistenteConNuevoDni = _contexto.Profesor.FirstOrDefault(c => c.DniProfesor == DniProfesor);

                if (ProfesorExistenteConNuevoDni == null || ProfesorExistenteConNuevoDni.ProfesorID == ProfesorID)
                {
                    // No existe otro profesor con el nuevo DNI o el DNI pertenece al mismo profesor que se está editando
                    var Editar = _contexto.Profesor.Find(ProfesorID);
                    if (Editar != null)
                    {
                        Editar.Nombre = Nombre;
                        Editar.CorreoElectronico = CorreoElectronico;
                        Editar.DniProfesor = DniProfesor;
                        Editar.NacimientoProfesor = NacimientoProfesor;
                        _contexto.SaveChanges();
                        resultado = true;
                    }
                }


            }
        }

        return Json(resultado);
    }

    public JsonResult Deshabilitar(int ProfesorID)
    {
        bool resultado = false;

        var profesor = _contexto.Profesor.Find(ProfesorID);

        if (profesor != null)
        {

            if (profesor.Eliminado == true)
            {
                profesor.Eliminado = false;
                resultado = true;
                _contexto.SaveChanges();
            }
            else
            {
                profesor.Eliminado = true;
                resultado = true;
                _contexto.SaveChanges();
            }
        }

        return Json(resultado);
    }
}