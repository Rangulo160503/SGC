using System.Net.Http.Json;
using SGC.BLL.Dtos;

namespace SGC.MVC.Services.Api
{
    public interface IAuthApiClient
    {
        Task<(bool Ok, string? Mensaje, UsuarioDto? Usuario)> RegisterAsync(UsuarioDto dto);
        Task<(bool Ok, string? Mensaje, UsuarioDto? Usuario)> LoginAsync(string email, string password);
    }

    public class AuthApiClient : ApiClientBase, IAuthApiClient
    {
        private readonly HttpClient _httpClient;

        public AuthApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(bool Ok, string? Mensaje, UsuarioDto? Usuario)> RegisterAsync(UsuarioDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", dto);

            if (response.IsSuccessStatusCode)
            {
                var payload = await response.Content.ReadFromJsonAsync<AuthResponse>();
                return (payload?.Ok ?? false, payload?.Mensaje, payload?.Usuario);
            }

            var mensaje = await ReadErrorMessageAsync(response);
            return (false, mensaje, null);
        }

        public async Task<(bool Ok, string? Mensaje, UsuarioDto? Usuario)> LoginAsync(string email, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", new { email, password });

            if (response.IsSuccessStatusCode)
            {
                var payload = await response.Content.ReadFromJsonAsync<AuthResponse>();
                return (payload?.Ok ?? false, payload?.Mensaje, payload?.Usuario);
            }

            var mensaje = await ReadErrorMessageAsync(response);
            return (false, mensaje, null);
        }

        private class AuthResponse
        {
            public bool Ok { get; set; }
            public string? Mensaje { get; set; }
            public UsuarioDto? Usuario { get; set; }
        }
    }
}
