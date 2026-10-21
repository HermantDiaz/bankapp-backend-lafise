/// <resumen>
/// Entidad que representa una cuenta bancaria, incluyendo su saldo y el historial de transacciones.
/// </resumen>

using BankApp.Domain.Enums;
using BankApp.Domain.Exceptions;
using System.Text.RegularExpressions;


namespace BankApp.Domain.Entities;


public sealed class Account
{
    private readonly List<Transaction> _transactions = new();

    public Guid Id { get; private set; }
    public string AccountNumber { get; private set; } = string.Empty; //El numero de cuenta es unico y solo se asigna al momento de crear la cuenta.
    public decimal Balance { get; private set; }
    public Guid CustomerId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Exponemos el historial como solo lectura para proteger la inmutabilidad
    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

    // Constructor privado para Entity Framework Core
    private Account() { }

    // Constructor de negocio para aperturar una nueva cuenta
    public Account(string accountNumber, Guid customerId, decimal initialDeposit = 0)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
            throw new ArgumentException("El número de cuenta no puede estar vacío.", nameof(accountNumber));

        // Validación estricta del formato LAFISE: ACC-YYYYMMDD-[XXXX]
        if (!Regex.IsMatch(accountNumber, @"^ACC-\d{8}-\d{4}$"))
            throw new ArgumentException("El número de cuenta debe cumplir con el formato 'ACC-YYYYMMDD-[XXXX]'.", nameof(accountNumber));

        if (customerId == Guid.Empty)
            throw new ArgumentException("El identificador del cliente no es válido.", nameof(customerId));

        if (initialDeposit < 0)
            throw new ArgumentException("El depósito inicial no puede ser negativo.", nameof(initialDeposit));


        Id = Guid.NewGuid();
        AccountNumber = accountNumber;
        CustomerId = customerId;
        CreatedAt = DateTime.UtcNow;
        Balance = 0;

        // Si el cliente apertura la cuenta con dinero, ponemos su primer depósito
        if (initialDeposit > 0)
        {
            Deposit(initialDeposit);
        }
    }

    /// <resumen>
    /// Haciendo un depósito en la cuenta incrementa el saldo.
    /// </resumen>
    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("El monto a depositar debe ser mayor a cero.", nameof(amount));

        Balance += amount;

        // Registramos la transacción con el saldo resultante inmediato, cumpliendo con uno de los requisitos de negocio.
        var transaction = new Transaction(Id, TransactionType.Deposit, amount, Balance);
        _transactions.Add(transaction);
    }

    /// <resumen>
    /// Haciendo un retiro de la cuenta si existen fondos suficientes.
    /// </resumen>
    public void Withdraw(decimal amount)
    {


        if (amount <= 0)
            throw new ArgumentException("El monto a retirar debe ser mayor a cero.", nameof(amount));

        if (amount > Balance)
            throw new InsufficientFundsException(Balance, amount); // la entidad valida si el retiro es posible, y si no, lazamos la excepcion que creamos para este caso, cumpliendo con otro de los requisitos de negocio.


        Balance -= amount;

       
       
        // Registramos la transacción con el saldo resultante inmediato
        var transaction = new Transaction(Id, TransactionType.Withdrawal, amount, Balance);
        _transactions.Add(transaction);
    }

}
