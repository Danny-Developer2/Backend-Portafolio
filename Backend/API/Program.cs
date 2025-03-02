using Microsoft.OpenApi.Models;
using API.Data;
using Microsoft.EntityFrameworkCore;
using API.Middleware;
using API.Extensions;
using API.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});


var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
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
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin","User"));
});
// Configurar la conexión a la base de datos

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddScoped<ExperienceRepository, ExperienceRepository>();

builder.Services.AddScoped<SkillRepository,SkillRepository>();

// Agregar servicios de Swagger
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

// Agregar el servicio de controladores
builder.Services.AddControllers();  // Aquí se agrega AddControllers

// Agregar autorización (si es necesario)
builder.Services.AddAuthorization();

builder.Services.AddScoped<UserRepository, UserRepository>();




var app = builder.Build();

app.UseExceptionMiddleware(); 

// Habilitar Swagger y la UI de Swagger en Desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi API v1");
        c.RoutePrefix = string.Empty; // Hace que Swagger esté disponible en la raíz
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();  // Habilitar autorización (si es necesario)
app.MapControllers();    // Mapear controladores
app.Run();
