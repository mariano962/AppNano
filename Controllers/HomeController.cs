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


    // public async Task<IActionResult> Index()
    // {
    //     await InicializarPermisosUsuario();
    //     return View();
    // }

    public async Task<IActionResult> Index()
    {
        InicializarPermisosUsuario();


        string email = "admin@gmail.com";
        string password = "123456";
        string rolNombre = "Admin";

        await RegistrarUsuario(email, password, rolNombre);

        return View();
    }



    public async Task<JsonResult> RegistrarUsuario(string email, string password, string rolNombre)
    {

        bool creado = false;

        var usuario = _contexto.Users.Where(u => u.Email == email).SingleOrDefault();
        if (usuario == null)
        {
            var user = new IdentityUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            await _userManager.AddToRoleAsync(user, rolNombre);
            creado = result.Succeeded;
        }

        return Json(creado);

    }



    public async void InicializarPermisosUsuario()
    {
        //CREAR ROLES SI NO EXISTEN
        bool creado = false;
        var roles = _contexto.Users.ToList();

        var ProfesorCrearExiste = _contexto.Roles.Where(r => r.Name == "Profesor").SingleOrDefault();
        if (ProfesorCrearExiste == null)
        {
            var roleResult1 = await _rolManager.CreateAsync(new IdentityRole("Profesor"));
        }

        var alumnoCrearExiste = _contexto.Roles.Where(r => r.Name == "Estudiante").SingleOrDefault();
        if (alumnoCrearExiste == null)
        {
            var roleResult2 = await _rolManager.CreateAsync(new IdentityRole("Estudiante"));
        }
        var AdminCrearExiste = _contexto.Roles.Where(r => r.Name == "Admin").SingleOrDefault();
        if (AdminCrearExiste == null)
        {
            var roleResult3 = await _rolManager.CreateAsync(new IdentityRole("Admin"));
        }

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
