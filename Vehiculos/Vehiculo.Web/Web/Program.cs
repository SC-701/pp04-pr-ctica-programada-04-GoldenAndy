using Autorizacion.Abstracciones.DA;       
using Autorizacion.Abstracciones.Flujo;    
using Autorizacion.DA;                     
using Autorizacion.DA.Repositorios;      
using Autorizacion.Flujo;            
using Autorizacion.Middleware;
using Microsoft.AspNetCore.Authentication.Cookies;
using Abstracciones.Interfaces.Reglas;
using Reglas;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<IConfiguracion, Configuracion>();

//Autenticación con cookie — guarda el JWT dentro de una cookie cifrada
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Cuenta/Login";
        options.LogoutPath = "/Cuenta/Logout";
        options.AccessDeniedPath = "/Cuenta/AccesoDenegado";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
    });

//Servicios del paquete NuGet de autorización (para AutorizacionClaims)
builder.Services.AddTransient<IAutorizacionFlujo, AutorizacionFlujo>();
builder.Services.AddTransient<ISeguridadDA, SeguridadDA>();
builder.Services.AddTransient<IRepositorioDapper, RepositorioDapper>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();  
app.AutorizacionClaims();   
app.UseAuthorization();    

app.UseAuthorization();

app.MapRazorPages();

app.Run();
