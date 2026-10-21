/// <resumen>
/// Registra la persistencia SQLite, DbContext y los repositorios en el contenedor de dependencias.
/// </resumen>

using BankApp.Domain.Repositories;
using BankApp.Infrastructure.Data;
using BankApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BankApp.Infrastructure;


public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Obtenemos la cadena de conexión o usamos el valor por defecto "bankapp.db"
        var connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? "Data Source=bankapp.db";

        // Registramos el contexto de SQLite
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(connectionString));

        // Registramos los repositorios
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        // Registramos IUnitOfWork reutilizando la misma instancia del DbContext
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        return services;
    }
}
