namespace BankApp.Application.DTOs;


// Datos de entrada para crear un perfil de cliente.

public sealed record CreateCustomerRequestDto(
    string FullName,
    DateOnly DateOfBirth,
    string Gender,
    decimal MonthlyIncome
);


/// Datos que regresan tras registrar o consultar un cliente.


public sealed record CustomerResponseDto(
    Guid Id,
    string FullName,
    DateOnly DateOfBirth,
    string Gender,
    decimal MonthlyIncome,
    DateTime CreatedAt
);
