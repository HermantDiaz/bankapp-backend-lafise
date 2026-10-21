/// <resumen>
/// Definimos los tipos de movimientos financieros que puede tener un usuario en su cuenta.
/// </resumen>

namespace BankApp.Domain.Enums;

public enum TransactionType
{
    Deposit = 1, // inicializamos el deposito con valor 1 para evitar que algun registro tenga valor 0 y genere errores en la logica de negocio
    Withdrawal = 2
}
