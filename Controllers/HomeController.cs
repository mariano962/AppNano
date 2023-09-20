using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AppNano.Models;
using AppNano.Data;
using Microsoft.AspNetCore.Identity;

namespace AppNano.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _contexto;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _rolManager;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext contexto, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> rolManager)
    {
        _logger = logger;
        _contexto = contexto;
        _userManager = userManager;
        _rolManager = rolManager;
    }


    public async Task<IActionResult> Index()
    {
        await InicializarPermisosUsuario();
        return View();
    }

    public async Task<JsonResult> InicializarPermisosUsuario()
    {
        //CREAR ROLES SI NO EXISTEN
        var adminExiste = _contexto.Roles.Where(r => r.Name == "Admin").SingleOrDefault();
        if (adminExiste == null)
        {
            var roleResult = await _rolManager.CreateAsync(new IdentityRole("Admin"));
        }
        var profesorExiste = _contexto.Roles.Where(r => r.Name == "Profesor").SingleOrDefault();
        if (profesorExiste == null)
        {
            var roleResult = await _rolManager.CreateAsync(new IdentityRole("Profesor"));
        }
        var estudianteExiste = _contexto.Roles.Where(r => r.Name == "Estudiante").SingleOrDefault();
        if (estudianteExiste == null)
        {
            var roleResult = await _rolManager.CreateAsync(new IdentityRole("Estudiante"));
        }


        // //CREAR USUARIO PRINCIPAL
        bool creado = false;
        // //BUSCAR POR MEDIO DE CORREO ELECTRONICO SI EXISTE EL USUARIO
        // var usuario = _contextUsuario.Users.Where(u => u.Email == "usuario@sistema.com").SingleOrDefault();
        // if (usuario == null)
        // {
        //     var user = new IdentityUser { UserName = "usuario@sistema.com", Email = "usuario@sistema.com" };
        //     var result = await _userManager.CreateAsync(user, "password");

        //     await _userManager.AddToRoleAsync(user, "NombreRolCrear");
        //     creado = result.Succeeded;
        // }

        // //CODIGO PARA BUSCAR EL USUARIO EN CASO DE NECESITARLO
        // var superusuario = _contextUsuario.Users.Where(r => r.Email == "usuario@sistema.com").SingleOrDefault();
        // if (superusuario != null)
        // {

        //     //var personaSuperusuario = _contexto.Personas.Where(r => r.UsuarioID == superusuario.Id).Count();

        //     var usuarioID = superusuario.Id;

        // }

        return Json(creado);
    }







    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
