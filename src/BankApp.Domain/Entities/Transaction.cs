/// <resumen>
/// Definimos los tipos de movimientos financieros que puede tener un usuario en su cuenta.
/// </resumen>

using BankApp.Domain.Enums; // importamos nuestro enum TransactionType para poder usarlo en la clase Transaction

namespace BankApp.Domain.Entities;


public sealed class Transaction
{
    
    public Guid Id { get; private set; }
    public Guid AccountId { get; private set; }
    public TransactionType Type { get; private set; }
    public decimal Amount { get; private set; }
    public decimal ResultingBalance { get; private set; }
    public DateTime Timestamp { get; private set; }

    // Constructor privado para Entity Framework Core
    private Transaction() { }

    // Constructor de negocio para crear transacciones válidas
    public Transaction(Guid accountId, TransactionType type, decimal amount, decimal resultingBalance)
    {
        Id = Guid.NewGuid();
        AccountId = accountId;
        Type = type;
        Amount = amount;
        ResultingBalance = resultingBalance;
        Timestamp = DateTime.UtcNow;
    }


}
