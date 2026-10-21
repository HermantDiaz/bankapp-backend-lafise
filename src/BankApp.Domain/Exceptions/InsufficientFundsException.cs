namespace BankApp.Domain.Exceptions;

/// <resumen>
/// Excepción cuando una cuenta no tiene saldo suficiente para un retiro.
/// Integrando una clase dedicada para esta excepcion, podemos mejorar la gestion de errores.
/// </resumen>

public sealed class InsufficientFundsException : Exception
{
    public decimal CurrentBalance { get; } // Cuanto dinero tiene actualmente el usuario
    public decimal AttemptedAmount { get; } // Cuanto dinero intento retirar el usuario
    
    public InsufficientFundsException(decimal currentBalance, decimal attemptedAmount)
        : base($"Fondos insuficientes. Saldo actual: {currentBalance:C2}, monto solicitado: {attemptedAmount:C2}.") // Mensaje de error que se mostrara al usuario
    {
        CurrentBalance = currentBalance;
        AttemptedAmount = attemptedAmount;
    }
    
}