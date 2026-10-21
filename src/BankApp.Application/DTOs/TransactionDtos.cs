namespace BankApp.Application.DTOs;

/// Datos de entrada para depositar o retirar dinero.
public sealed record TransactionRequestDto(
    decimal Amount
);

/// Datos de salida de un movimiento con saldo histórico.
public sealed record TransactionResponseDto(
    Guid Id,
    string Type,
    decimal Amount,
    decimal ResultingBalance,
    DateTime Timestamp
);

/// Resumen del historial cronológico de movimientos de la cuenta.
public sealed record AccountHistoryResponseDto(
    string AccountNumber,
    decimal CurrentBalance,
    IReadOnlyList<TransactionResponseDto> Transactions
);
