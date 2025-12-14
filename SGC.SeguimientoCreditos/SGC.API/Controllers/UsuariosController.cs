using Microsoft.AspNetCore.Mvc;
using SGC.BLL.Dtos;
using SGC.BLL.Servicios;

namespace SGC.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuariosServicio _usuariosServicio;

        public UsuariosController(IUsuariosServicio usuariosServicio)
        {
            _usuariosServicio = usuariosServicio;
        }

        // GET: api/usuarios
        [HttpGet]
        public async Task<IActionResult> Listar()
            => Ok(await _usuariosServicio.ListarAsync());

        // GET: api/usuarios/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var u = await _usuariosServicio.ObtenerAsync(id);
            return u is null ? NotFound() : Ok(u);
        }

        // POST: api/usuarios
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] UsuarioDto dto)
        {
            var resp = await _usuariosServicio.CrearAsync(dto);

            var payload = new
            {
                ok = resp.Ok,
                mensaje = resp.Mensaje,
                usuario = resp.Usuario
            };

            return resp.Ok ? Ok(payload) : BadRequest(payload);
        }

        // PUT: api/usuarios/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Editar(int id, [FromBody] UsuarioDto dto)
        {
            dto.Id = id;
            var resp = await _usuariosServicio.ActualizarAsync(dto);

            var payload = new
            {
                ok = resp.Ok,
                mensaje = resp.Mensaje,
                usuario = resp.Usuario
            };

            return resp.Ok ? Ok(payload) : BadRequest(payload);
        }

        // DELETE: api/usuarios/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var resp = await _usuariosServicio.EliminarAsync(id);

            var payload = new
            {
                ok = resp.Ok,
                mensaje = resp.Mensaje
            };

            return resp.Ok ? Ok(payload) : BadRequest(payload);
        }
    }
}
