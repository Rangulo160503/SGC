using System.Net.Http.Json;
using SGC.BLL;
using SGC.BLL.Dtos;
using SGC.BLL.Servicios;

namespace SGC.MVC.Services.Api
{
    public class SolicitudesApiClient : ApiClientBase, ISolicitudesServicio
    {
        private readonly HttpClient _httpClient;

        public SolicitudesApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<SolicitudDto>> ListarPorRolAsync(string rol)
        {
            var lista = await _httpClient.GetFromJsonAsync<List<SolicitudDto>>($"api/gestiones?rol={Uri.EscapeDataString(rol)}");
            return lista ?? new List<SolicitudDto>();
        }

        public async Task<CustomResponse<SolicitudDto>> CrearAsync(int clienteId, string identificacionCliente, decimal monto, string? comentarios, string usuarioNombre, string rol)
        {
            var response = await _httpClient.PostAsJsonAsync("api/solicitudes", new
            {
                IdentificacionCliente = identificacionCliente,
                Monto = monto,
                Comentarios = comentarios,
                UsuarioNombre = usuarioNombre,
                Rol = rol
            });

            if (response.IsSuccessStatusCode)
            {
                var payload = await response.Content.ReadFromJsonAsync<CustomResponse<SolicitudDto>>();
                if (payload != null)
                {
                    return payload;
                }
            }

            var mensaje = await ReadErrorMessageAsync(response);
            return CustomResponse<SolicitudDto>.Fail(mensaje ?? "No se pudo crear la solicitud.");
        }

        public async Task<CustomResponse<SolicitudDto>> EnviarAprobacionAsync(int gestionId, string usuarioNombre, string rol)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/gestiones/{gestionId}/enviar-aprobacion", new { UsuarioNombre = usuarioNombre, Rol = rol });
            return await BuildBasicResponseAsync(response, "No se pudo enviar a aprobación.");
        }

        public async Task<CustomResponse<SolicitudDto>> AprobarAsync(int gestionId, string usuarioNombre, string rol)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/gestiones/{gestionId}/aprobar", new { UsuarioNombre = usuarioNombre, Rol = rol });
            return await BuildBasicResponseAsync(response, "No se pudo aprobar la gestión.");
        }

        public async Task<CustomResponse<SolicitudDto>> DevolverAsync(int gestionId, string comentario, string usuarioNombre, string rol)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/gestiones/{gestionId}/devolver", new { UsuarioNombre = usuarioNombre, Rol = rol, Comentario = comentario });
            return await BuildBasicResponseAsync(response, "No se pudo devolver la gestión.");
        }

        public async Task<List<TrackingDto>> ObtenerTrackingAsync(int gestionId)
        {
            var lista = await _httpClient.GetFromJsonAsync<List<TrackingDto>>($"api/gestiones/{gestionId}/tracking");
            return lista ?? new List<TrackingDto>();
        }

        public async Task<CustomResponse<bool>> EliminarAsync(int gestionId, string usuarioNombre, string rol)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/gestiones/{gestionId}/eliminar", new { UsuarioNombre = usuarioNombre, Rol = rol });
            if (response.IsSuccessStatusCode)
            {
                var payload = await response.Content.ReadFromJsonAsync<BasicResponse>();
                return payload?.Ok == true
                    ? CustomResponse<bool>.Success(true, payload.Mensaje)
                    : CustomResponse<bool>.Fail(payload?.Mensaje ?? "No se pudo eliminar la gestión.");
            }

            var mensaje = await ReadErrorMessageAsync(response);
            return CustomResponse<bool>.Fail(mensaje ?? "No se pudo eliminar la gestión.");
        }

        private async Task<CustomResponse<SolicitudDto>> BuildBasicResponseAsync(HttpResponseMessage response, string defaultMessage)
        {
            if (response.IsSuccessStatusCode)
            {
                var payload = await response.Content.ReadFromJsonAsync<BasicResponse>();
                return payload?.Ok == true
                    ? CustomResponse<SolicitudDto>.Success(new SolicitudDto(), payload.Mensaje)
                    : CustomResponse<SolicitudDto>.Fail(payload?.Mensaje ?? defaultMessage);
            }

            var mensaje = await ReadErrorMessageAsync(response);
            return CustomResponse<SolicitudDto>.Fail(mensaje ?? defaultMessage);
        }

        private class BasicResponse
        {
            public bool Ok { get; set; }
            public string? Mensaje { get; set; }
        }
    }
}
