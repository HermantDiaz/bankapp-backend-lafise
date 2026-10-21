/// <resumen>
/// Contexto de Entity Framework Core para SQLite que implementa IUnitOfWork.
/// </resumen>

using BankApp.Domain.Entities;
using BankApp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BankApp.Infrastructure.Data;


public sealed class ApplicationDbContext : DbContext, IUnitOfWork
{
    public DbSet<Customer> Customers => Set<Customer>(); // DbSet para la entidad Customer, que representa la tabla de clientes en la base de datos.
    public DbSet<Account> Accounts => Set<Account>(); // DbSet para la entidad Account, que representa la tabla de cuentas en la base de datos.
    public DbSet<Transaction> Transactions => Set<Transaction>(); // DbSet para la entidad Transaction, que representa la tabla de transacciones en la base de datos.

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) // Constructor que recibe opciones de configuración para el contexto de base de datos y las pasa a la clase base DbContext.
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) // Con este metodo se configura el modelo de la base de datos y permite configurar las entidades y sus relaciones mediante Fluent API.
    {
        base.OnModelCreating(modelBuilder);

        // Aplica automáticamente todas las configuraciones Fluent API del ensamblado actual
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
