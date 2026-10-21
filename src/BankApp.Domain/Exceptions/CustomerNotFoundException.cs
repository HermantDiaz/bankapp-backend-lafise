/// <resumen>
/// Excepción lanzada cuando no encuentramos un cliente.
/// </resumen>
/// 
namespace BankApp.Domain.Exceptions;
public sealed class CustomerNotFoundException : Exception
{
    public Guid CustomerId { get; }

    public CustomerNotFoundException(Guid customerId)
        : base($"No se encontró el cliente con el identificador '{customerId}'.")
    {
        CustomerId = customerId;
    }
}
