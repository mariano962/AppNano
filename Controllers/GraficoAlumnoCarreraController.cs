using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AppNano.Models;
using AppNano.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace AppNano.Controllers;

[Authorize]

public class GraficoAlumnoCarreraController : Controller
{
    private readonly ILogger<GraficoAlumnoCarreraController> _logger;
    private ApplicationDbContext _contexto;

    public GraficoAlumnoCarreraController(ILogger<GraficoAlumnoCarreraController>? logger, ApplicationDbContext contexto)
    {
        _logger = logger;
        _contexto = contexto;
    }

    public IActionResult Index()
    {
        return View();
    }

    public JsonResult BuscarGrafico()
    {
        var datos =  _contexto.Alumnos.Include(s => s.Carrera).ToList();

        return Json(datos);
    }

   
public JsonResult BuscarAlumnosGrafico()
{
    
    var alumnos = _contexto.Alumnos.ToList();

    // Inicializa una lista para almacenar los datos de los diferentes rangos de edades
    var data = new List<int>() { 0, 0, 0, 0, 0, 0, 0 };

    
    foreach (var alumno in alumnos)
    {
        var años = DateTime.Now.Year - alumno.NacimientoAlumno.Year;

        // Clasifica el alumno en el rango de edad
        if (años < 18) 
        {
            data[0]++;
        }
        else if(años >= 18 && años <= 24) 
        {
            data[1]++; 
        }
        else if(años >= 25 && años <= 34) 
        {
            data[2]++; 
        }
        else if(años >= 35 && años <= 44) 
        {
            data[3]++; 
        }
        else if(años >= 45 && años <= 54) 
        {
            data[4]++; 
        }
        else if(años >= 55 && años <= 64) 
        {
            data[5]++; 
        }
        else if(años >= 65) 
        {
            data[6]++; 
        }
    }

    // Crea un objeto que contiene los datos para el gráfico
    var resultado = new { data = data };

   
    return Json(resultado);
}


       

}