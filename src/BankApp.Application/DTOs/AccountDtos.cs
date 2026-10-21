namespace BankApp.Application.DTOs;

// Datos de entrada para aperturar una nueva cuenta.

public sealed record CreateAccountRequestDto(
    Guid CustomerId,
    decimal InitialDeposit = 0
);

/// Respuesta que regresa tras aperturar una cuenta.
public sealed record AccountResponseDto(
    Guid Id,
    string AccountNumber,
    decimal Balance,
    Guid CustomerId,
    DateTime CreatedAt
);

/// Respuesta para la consulta de saldo.
public sealed record BalanceResponseDto(
    string AccountNumber,
    decimal Balance,
    DateTime AsOfDate
);
