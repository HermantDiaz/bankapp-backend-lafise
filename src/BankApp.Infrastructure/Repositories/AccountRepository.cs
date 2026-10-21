/// <resumen>
/// Implementación de persistencia para cuentas bancarias con la carga ansiosa del historial de transacciones.
/// </resumen>

using BankApp.Domain.Entities;
using BankApp.Domain.Repositories;
using BankApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BankApp.Infrastructure.Repositories;


public sealed class AccountRepository : IAccountRepository
{
    private readonly ApplicationDbContext _context; // Inyección de dependencia 

    public AccountRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Account?> GetByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default)
    {
        // Usamos .Include(a => a.Transactions) para traer la cuenta junto con su historial completo
        return await _context.Accounts
            .Include(a => a.Transactions)
            .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber, cancellationToken);
    }

    public async Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) // Consultamos una cuenta por su id, incluyendo su historial de transacciones
    {
        return await _context.Accounts
            .Include(a => a.Transactions)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task AddAsync(Account account, CancellationToken cancellationToken = default) // Agregamos una nueva cuenta a la base de datos
    {
        await _context.Accounts.AddAsync(account, cancellationToken);
    }

    public Task UpdateAsync(Account account, CancellationToken cancellationToken = default) // Actualizamos una cuenta existente en la base de datos, incluyendo su historial de transacciones
    {
        _context.Accounts.Update(account);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default) // Verificamos si existe una cuenta con el número de cuenta dado
    {
        return await _context.Accounts
            .AnyAsync(a => a.AccountNumber == accountNumber, cancellationToken);
    }
}
