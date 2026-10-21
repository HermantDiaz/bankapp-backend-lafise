/// <resumen>
/// Implementación de persistencia para clientes usando Entity Framework Core y SQLite.
/// </resumen>

using BankApp.Domain.Entities;
using BankApp.Domain.Repositories;
using BankApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BankApp.Infrastructure.Repositories;


public sealed class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _context;

    public CustomerRepository(ApplicationDbContext context) 
    {
        _context = context;
    }

    public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) // Consultamos un cliente por su id
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task AddAsync(Customer customer, CancellationToken cancellationToken = default) // Agregamos un nuevo cliente a la base de datos
    {
        await _context.Customers.AddAsync(customer, cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default) // Verificamos si existe un cliente con el id
    {
        return await _context.Customers
            .AnyAsync(c => c.Id == id, cancellationToken);
    }
}
