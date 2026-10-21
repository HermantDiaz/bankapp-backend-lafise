/// <resumen>
/// Definimos el contrato para la gestión de clientes en la base de datos.
/// </resumen>
using BankApp.Domain.Entities;

namespace BankApp.Domain.Repositories;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default); // buscamos un cliente por su id.
    Task AddAsync(Customer customer, CancellationToken cancellationToken = default); // agregamos un nuevo cliente a la base de datos.
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default); // verificamos si existe un cliente con el id.
}
