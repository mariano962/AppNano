using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AppNano.Models;
using AppNano.Data;

namespace AppNano.Controllers;

public class CarrerasController : Controller
{
    private readonly ILogger<CarrerasController> _logger;
    private ApplicationDbContext _contexto;
    
     public CarrerasController(ILogger<CarrerasController>? logger, ApplicationDbContext contexto)
    {
        _logger = logger;
        _contexto = contexto;
    }

    public IActionResult Index()
    {
        return View();
    }

    public JsonResult BuscarCarreras (int CarreraID, string NombreCarrera, string Duracion)
    {
        var carrera = _contexto.Carrera.ToList();
        if (CarreraID > 0){

            carrera = carrera.Where(c => c.CarreraID == CarreraID).ToList();

        }
       
        return Json (carrera);
    }


    public JsonResult GuardarCarrera(int CarreraID , string NombreCarrera, string Duracion, bool Eliminado)
    {
        bool resultado = false;

        if(!string.IsNullOrEmpty(NombreCarrera))
        {
         
            if (CarreraID == 0)
            {
                var CarreraNueva = _contexto.Carrera.Where(c => c.NombreCarrera == NombreCarrera).FirstOrDefault();
                if(CarreraNueva == null)
                {
                    var CarreraGuardar = new Carrera
                    {
                        CarreraID = CarreraID,
                        NombreCarrera = NombreCarrera,
                        Duracion = Duracion,
                        Eliminado = Eliminado
                        
                        
                    };
                    _contexto.Add(CarreraGuardar);
                    _contexto.SaveChanges();
                    resultado = true;
                }
            }
              else
            {
              
                var ValdiarCarrera = _contexto.Carrera.Where(c => c.NombreCarrera == NombreCarrera && c.CarreraID != CarreraID).FirstOrDefault();
                if (ValdiarCarrera == null)
                {
                    var Editar = _contexto.Carrera.Find(CarreraID);
                    if (Editar != null)
                    {
                        Editar.NombreCarrera = NombreCarrera;
                        Editar.Duracion = Duracion ;
                        _contexto.SaveChanges();
                        resultado = true;
                    }
                }


            }
        }

       return Json(resultado);
    }

    public JsonResult Deshabilitar (int CarreraID){
        bool resultado =  false;

        var carrera = _contexto.Carrera.Find(CarreraID);

        if (carrera != null)
        {
            if (carrera.Eliminado == true)
            {
                carrera.Eliminado = false;
                resultado = true;
                _contexto.SaveChanges();
            }
            else 
            {
                carrera.Eliminado = true;
                resultado = true;
                _contexto.SaveChanges();
            }
        }

        return Json(resultado);
    }

}
