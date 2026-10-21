/// <resumen>
/// Definimos el contrato para la gestión de cuentas bancarias en la base de datos.
/// </resumen>

using BankApp.Domain.Entities;

namespace BankApp.Domain.Repositories;


public interface IAccountRepository
{
    Task<Account?> GetByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default); //buscamos una cuenta por su numero de cuenta, si no existe retornamos null.
    Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default); //buscamos una cuenta por su id, si no existe retornamos null.
    Task AddAsync(Account account, CancellationToken cancellationToken = default); // agregamos una nueva cuenta a la base de datos
    Task UpdateAsync(Account account, CancellationToken cancellationToken = default); //actualizamos una cuenta existente en la base de datos.
    Task<bool> ExistsAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default); // verificamos si existe una cuenta con el numero de cuenta.
}
