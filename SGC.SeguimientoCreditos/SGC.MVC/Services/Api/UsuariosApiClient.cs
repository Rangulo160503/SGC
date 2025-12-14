using System.Net.Http.Json;
using SGC.BLL;
using SGC.BLL.Dtos;
using SGC.BLL.Servicios;

namespace SGC.MVC.Services.Api
{
    public class UsuariosApiClient : ApiClientBase, IUsuariosServicio
    {
        private readonly HttpClient _httpClient;

        public UsuariosApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<UsuarioDto>> ListarAsync()
        {
            var lista = await _httpClient.GetFromJsonAsync<List<UsuarioDto>>("api/usuarios");
            return lista ?? new List<UsuarioDto>();
        }

        public async Task<UsuarioDto?> ObtenerAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<UsuarioDto>($"api/usuarios/{id}");
        }

        public async Task<CustomResponse<UsuarioDto>> CrearAsync(UsuarioDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/usuarios", dto);

            if (response.IsSuccessStatusCode)
            {
                var payload = await response.Content.ReadFromJsonAsync<BasicResponse<UsuarioDto>>();
                return payload?.Ok == true
                    ? CustomResponse<UsuarioDto>.Success(payload.Usuario ?? dto, payload.Mensaje)
                    : CustomResponse<UsuarioDto>.Fail(payload?.Mensaje ?? "No se pudo crear el usuario.");
            }

            var mensaje = await ReadErrorMessageAsync(response);
            return CustomResponse<UsuarioDto>.Fail(mensaje ?? "Error al crear usuario");
        }

        public async Task<CustomResponse<UsuarioDto>> ActualizarAsync(UsuarioDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/usuarios/{dto.Id}", dto);

            if (response.IsSuccessStatusCode)
            {
                var payload = await response.Content.ReadFromJsonAsync<BasicResponse>();
                return payload?.Ok == true
                    ? CustomResponse<UsuarioDto>.Success(dto, payload.Mensaje)
                    : CustomResponse<UsuarioDto>.Fail(payload?.Mensaje ?? "No se pudo actualizar el usuario.");
            }

            var mensaje = await ReadErrorMessageAsync(response);
            return CustomResponse<UsuarioDto>.Fail(mensaje ?? "Error al actualizar usuario");
        }

        public async Task<CustomResponse<bool>> EliminarAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/usuarios/{id}");

            if (response.IsSuccessStatusCode)
            {
                var payload = await response.Content.ReadFromJsonAsync<BasicResponse>();
                return payload?.Ok == true
                    ? CustomResponse<bool>.Success(true, payload.Mensaje)
                    : CustomResponse<bool>.Fail(payload?.Mensaje ?? "No se pudo eliminar el usuario.");
            }

            var mensaje = await ReadErrorMessageAsync(response);
            return CustomResponse<bool>.Fail(mensaje ?? "Error al eliminar usuario");
        }

        public async Task<UsuarioDto?> LoginAsync(string email, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", new { email, password });

            if (response.IsSuccessStatusCode)
            {
                var payload = await response.Content.ReadFromJsonAsync<AuthResponse>();
                return payload?.Usuario;
            }

            return null;
        }

        public async Task<CustomResponse<UsuarioDto>> RegistrarAsync(UsuarioDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", dto);

            if (response.IsSuccessStatusCode)
            {
                var payload = await response.Content.ReadFromJsonAsync<AuthResponse>();
                if (payload?.Ok == true && payload.Usuario != null)
                {
                    return CustomResponse<UsuarioDto>.Success(payload.Usuario, payload.Mensaje);
                }
            }

            var mensaje = await ReadErrorMessageAsync(response);
            return CustomResponse<UsuarioDto>.Fail(mensaje ?? "No se pudo registrar el usuario.");
        }

        private class BasicResponse
        {
            public bool Ok { get; set; }
            public string? Mensaje { get; set; }
        }

        private class BasicResponse<T> : BasicResponse
        {
            public T? Usuario { get; set; }
        }

        private class AuthResponse : BasicResponse<UsuarioDto>
        {
        }
    }
}
