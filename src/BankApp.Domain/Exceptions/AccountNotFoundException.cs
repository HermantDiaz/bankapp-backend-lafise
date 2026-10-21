/// <resumen>
/// Excepción lanzada cuando no encuentramos una cuenta bancaria con el número especificado.
/// </resumen>
namespace BankApp.Domain.Exceptions;
public sealed class AccountNotFoundException : Exception
{
    public string AccountNumber { get; }

    public AccountNotFoundException(string accountNumber)
        : base($"No se encontró la cuenta bancaria con el número '{accountNumber}'.")
    {
        AccountNumber = accountNumber;
    }
}
