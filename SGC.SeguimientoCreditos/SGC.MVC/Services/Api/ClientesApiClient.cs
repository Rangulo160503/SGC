using System.Net.Http.Json;
using SGC.BLL;
using SGC.BLL.Dtos;
using SGC.BLL.Servicios;

namespace SGC.MVC.Services.Api
{
    public class ClientesApiClient : ApiClientBase, IClientesServicio
    {
        private readonly HttpClient _httpClient;

        public ClientesApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CustomResponse<ClienteDto>> CrearAsync(ClienteDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/clientes", dto);

            if (response.IsSuccessStatusCode)
            {
                var payload = await response.Content.ReadFromJsonAsync<BasicResponse>();
                return payload?.Ok == true
                    ? CustomResponse<ClienteDto>.Success(dto, payload.Mensaje)
                    : CustomResponse<ClienteDto>.Fail(payload?.Mensaje ?? "No se pudo crear el cliente.");
            }

            var mensaje = await ReadErrorMessageAsync(response);
            return CustomResponse<ClienteDto>.Fail(mensaje ?? "Error al crear el cliente.");
        }

        public async Task<CustomResponse<bool>> ActualizarAsync(ClienteDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/clientes/{dto.Id}", dto);

            if (response.IsSuccessStatusCode)
            {
                var payload = await response.Content.ReadFromJsonAsync<BasicResponse>();
                return payload?.Ok == true
                    ? CustomResponse<bool>.Success(true, payload.Mensaje)
                    : CustomResponse<bool>.Fail(payload?.Mensaje ?? "No se pudo actualizar el cliente.");
            }

            var mensaje = await ReadErrorMessageAsync(response);
            return CustomResponse<bool>.Fail(mensaje ?? "Error al actualizar el cliente.");
        }

        public async Task<CustomResponse<bool>> EliminarAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/clientes/{id}");

            if (response.IsSuccessStatusCode)
            {
                var payload = await response.Content.ReadFromJsonAsync<BasicResponse>();
                return payload?.Ok == true
                    ? CustomResponse<bool>.Success(true, payload.Mensaje)
                    : CustomResponse<bool>.Fail(payload?.Mensaje ?? "No se pudo eliminar el cliente.");
            }

            var mensaje = await ReadErrorMessageAsync(response);
            return CustomResponse<bool>.Fail(mensaje ?? "Error al eliminar el cliente.");
        }

        public async Task<List<ClienteDto>> ListarAsync()
        {
            var lista = await _httpClient.GetFromJsonAsync<List<ClienteDto>>("api/clientes");
            return lista ?? new List<ClienteDto>();
        }

        public async Task<ClienteDto?> ObtenerAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<ClienteDto>($"api/clientes/{id}");
        }

        public async Task<ClienteDto?> ObtenerPorIdentificacionAsync(string identificacion)
        {
            return await _httpClient.GetFromJsonAsync<ClienteDto>($"api/clientes/identificacion/{identificacion}");
        }

        private class BasicResponse
        {
            public bool Ok { get; set; }
            public string? Mensaje { get; set; }
        }
    }
}
