using Microsoft.AspNetCore.Mvc;
using SGC.BLL.Dtos;
using SGC.BLL.Servicios;

namespace SGC.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuariosServicio _usuariosServicio;

        public AuthController(IUsuariosServicio usuariosServicio)
        {
            _usuariosServicio = usuariosServicio;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UsuarioDto dto)
        {
            var respuesta = await _usuariosServicio.RegistrarAsync(dto);

            if (!respuesta.Ok)
            {
                return BadRequest(new
                {
                    ok = false,
                    mensaje = respuesta.Mensaje
                });
            }

            return Ok(new
            {
                ok = true,
                mensaje = respuesta.Mensaje,
                usuario = respuesta.Dato
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var usuario = await _usuariosServicio.LoginAsync(request.Email, request.Password);

            if (usuario == null)
            {
                return Unauthorized(new { ok = false, mensaje = "Credenciales incorrectas" });
            }

            return Ok(new
            {
                ok = true,
                usuario
            });
        }

        public class LoginRequest
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }
    }
}
