/// <resumen>
/// Registramos todos los servicios de la capa de aplicación en el contenedor de inversión de control.
/// </resumen>
using BankApp.Application.Common.Interfaces;
using BankApp.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BankApp.Application;


public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // El generador es stateless y thread-safe, por lo que puede ser Singleton
        services.AddSingleton<IAccountNumberGenerator, AccountNumberGenerator>();

        // Los servicios de negocio se registran Scoped (una instancia por cada petición HTTP)
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IAccountService, AccountService>();

        return services;
    }
}
