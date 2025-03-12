using Microsoft.OpenApi.Models;
using API.Data;
using Microsoft.EntityFrameworkCore;
using API.Middleware;
using API.Extensions;
using API.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using API.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Configuración para los controladores y la serialización JSON
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});

// Configuración de JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKeyString = jwtSettings["SecretKey"];
var secretKey = Encoding.UTF8.GetBytes(secretKeyString!);

// Configuración de autenticación con JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var token = context.Request.Cookies["auth_token"];
            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token;
            }
            return Task.CompletedTask;
        }
    };

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Configuración de la base de datos
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Servicios adicionales
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy =>
        {
            policy.WithOrigins("https://192.168.100.12:4200", "https://localhost:4200","https://localhost:4200","wss://192.168.100.12:4200/","http://localhost:4200") // Cambia según tu frontend
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials(); // Si usas autenticación con cookies o tokens
        });
});

// Repositorios
builder.Services.AddScoped<ExperienceRepository, ExperienceRepository>();
builder.Services.AddScoped<SkillRepository, SkillRepository>();
builder.Services.AddScoped<UserRepository, UserRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Configuración de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Mi API",
        Version = "v1",
        Description = "Ejemplo de API con Swagger en ASP.NET Core"
    });
});

builder.Services.AddControllers(); // Para agregar servicios de controladores

// Configuración de la autorización
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors("AllowSpecificOrigin");

// Middleware de excepciones personalizadas
app.UseExceptionMiddleware();

// Habilitar Swagger en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi API v1");
        c.RoutePrefix = string.Empty; // Hace que Swagger esté disponible en la raíz
    });
}

app.UseAuthentication();  // Asegúrate de usar la autenticación antes de la autorización
app.UseAuthorization();   // Llamada a UseAuthorization una sola vez

app.UseHttpsRedirection();
app.MapControllers();    // Mapea los controladores
app.Run();
