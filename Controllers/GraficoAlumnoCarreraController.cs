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
        var data = new List<int>() { 0, 0, 0, 0, 0,   };
        foreach (var alumno in alumnos)
        {
            var años = DateTime.Now.Year - alumno.NacimientoAlumno.Year;
            if (años < 18) //menor de 20
            {
                data[0]++;
            }
            else if(años >= 18 && años <= 24) //21 a 25
            {
                data[1]++; 
            }
            else if(años >= 25 && años <= 34) //26 a 30
            {
                data[2]++; 
            }
            else if(años >= 35 && años <= 44) // 30 a 35
            {
                data[3]++; 
            }
             else if(años >= 45 && años <= 54) // 30 a 35
            {
                data[4]++; 
            }
             else if(años >= 55 && años <= 64) // 30 a 35
            {
                data[5]++; 
            }
            else if(años > 65) // >35
            {
                data[6]++; 
            }
        }
        var resultado = new { data = data };
        return Json(resultado);
        }


        // private int CalcularEdad(DateTime NacimientoAlumno)
        // {
        //     int edad = DateTime.Now.Year - NacimientoAlumno.Year;
        //     if (DateTime.Now.DayOfYear < NacimientoAlumno.DayOfYear)
        //     {
        //         edad--; // Ajustar la edad si aún no ha cumplido años en este año
        //     }
        //     return edad;
        // }

        // private string ObtenerRangoEdad(int edad)
        // {
        //     if (edad < 18)
        //     {
        //         return "Menos de 18";
        //     }
        //     else if (edad >= 18 && edad <= 24)
        //     {
        //         return "18-24";
        //     }
        //     else if (edad >= 25 && edad <= 34)
        //     {
        //         return "25-34";
        //     }
        //     else if (edad >= 35 && edad <= 44)
        //     {
        //         return "35-44";
        //     }
        //     else if (edad >= 45 && edad <= 54)
        //     {
        //         return "45-54";
        //     }
        //     else if (edad >= 55 && edad <= 64)
        //     {
        //         return "55-64";
        //     }
        //     else
        //     {
        //         return "65 y más";
        //     }
        // }

}