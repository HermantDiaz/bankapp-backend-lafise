using BankApp.Application;
using BankApp.Infrastructure;
using BankApp.Infrastructure.Data;
using BankApp.WebApi.Middlewares;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Registro de Servicios de nuestras capas (Inversión de Control)
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Controladores
builder.Services.AddControllers();

// Regla Crítica #3: Manejo global de excepciones nativo de .NET
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Swagger UI para documentación interactiva de la prueba
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BankApp API - Sistemática / Grupo LAFISE",
        Version = "v1",
        Description = "API REST de Banca Digital desarrollada por Gabriel en .NET 10 con Clean Architecture."
    });
});

var app = builder.Build();

// Pipeline HTTP: Activamos el ExceptionHandler antes que cualquier otra cosa
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BankApp API v1");
        c.RoutePrefix = string.Empty; // Abre Swagger directamente en la raíz (http://localhost:5000/)
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Creación automática de la base de datos SQLite y tablas al iniciar
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
}

app.Run();
