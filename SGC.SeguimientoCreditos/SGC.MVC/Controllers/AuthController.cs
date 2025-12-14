using Microsoft.AspNetCore.Mvc;
using SGC.BLL.Dtos;
using SGC.MVC.Services.Api;

namespace SGC.MVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthApiClient _authApiClient;

        public AuthController(IAuthApiClient authApiClient)
        {
            _authApiClient = authApiClient;
        }

        // LOGIN
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var (ok, mensaje, user) = await _authApiClient.LoginAsync(email, password);

            if (!ok || user == null)
            {
                ModelState.AddModelError(string.Empty, mensaje ?? "Credenciales incorrectas");
                return View();
            }

            // Guardar sesión
            HttpContext.Session.SetString("UsuarioId", user.Id.ToString());
            HttpContext.Session.SetString("UsuarioNombre", user.Nombre);
            HttpContext.Session.SetString("UsuarioRol", user.Rol);

            return RedirectToAction("Index", "Home");
        }

        // REGISTRO
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UsuarioDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var (ok, mensaje, _) = await _authApiClient.RegisterAsync(dto);

            if (!ok)
            {
                ModelState.AddModelError(string.Empty, mensaje ?? "No se pudo registrar el usuario");
                return View(dto);
            }

            TempData["Success"] = mensaje ?? "Usuario registrado correctamente";
            return RedirectToAction("Login");
        }

        // LOGOUT
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
