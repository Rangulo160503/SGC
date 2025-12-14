using System.Text.Json;

namespace SGC.MVC.Services.Api
{
    public abstract class ApiClientBase
    {
        protected static async Task<string?> ReadErrorMessageAsync(HttpResponseMessage response)
        {
            try
            {
                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);

                if (doc.RootElement.TryGetProperty("mensaje", out var mensajeProp))
                {
                    return mensajeProp.GetString();
                }

                if (doc.RootElement.TryGetProperty("Mensaje", out var mensajeProp2))
                {
                    return mensajeProp2.GetString();
                }
            }
            catch
            {
                // Ignored on purpose. We'll fall back to a generic message.
            }

            return "Error al comunicarse con la API";
        }
    }
}
