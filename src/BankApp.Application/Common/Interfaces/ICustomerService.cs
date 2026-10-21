/// <resumen>
/// Contrato para los casos de uso relacionados con clientes.
/// </resumen>

using BankApp.Application.DTOs;

namespace BankApp.Application.Common.Interfaces;


public interface ICustomerService 
{
    Task<CustomerResponseDto> CreateCustomerAsync(CreateCustomerRequestDto request, CancellationToken cancellationToken = default); // Creamos un nuevo cliente y devolvemos su información.
    Task<CustomerResponseDto> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
