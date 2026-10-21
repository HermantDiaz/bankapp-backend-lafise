/// <resumen>
/// Contrato para los casos de uso financieros y operativos de las cuentas.
/// </resumen>

using BankApp.Application.DTOs;

namespace BankApp.Application.Common.Interfaces;


public interface IAccountService 
{
    Task<AccountResponseDto> CreateAccountAsync(CreateAccountRequestDto request, CancellationToken cancellationToken = default); // Creamos una nueva cuenta y devolvemos su información.
    Task<BalanceResponseDto> GetBalanceAsync(string accountNumber, CancellationToken cancellationToken = default); // Consultamos el saldo de una cuenta y devolvemos su información.
    Task<TransactionResponseDto> DepositAsync(string accountNumber, TransactionRequestDto request, CancellationToken cancellationToken = default); // Realizamos un depósito en una cuenta y devolvemos su información.
    Task<TransactionResponseDto> WithdrawAsync(string accountNumber, TransactionRequestDto request, CancellationToken cancellationToken = default); // Realizamos un retiro de una cuenta y devolvemos su información.
    Task<AccountHistoryResponseDto> GetHistoryAsync(string accountNumber, CancellationToken cancellationToken = default); // Consultamos el historial de transacciones de una cuenta y devolvemos su información.
}
