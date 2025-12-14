using SGC.BLL.Servicios;
using SGC.MVC.Services.Api;

var builder = WebApplication.CreateBuilder(args);

// HttpClient hacia la API
var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? throw new InvalidOperationException("ApiBaseUrl no está configurado");

builder.Services.AddHttpClient<IAuthApiClient, AuthApiClient>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

builder.Services.AddHttpClient<IUsuariosServicio, UsuariosApiClient>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

builder.Services.AddHttpClient<IClientesServicio, ClientesApiClient>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

builder.Services.AddHttpClient<ISolicitudesServicio, SolicitudesApiClient>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

// MVC
builder.Services.AddControllersWithViews();

// Sesiones
builder.Services.AddSession();

// HttpContextAccessor (para leer la sesin en el Layout)
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.MapDefaultControllerRoute();

app.Run();
