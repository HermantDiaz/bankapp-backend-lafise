/// <resumen>
/// Lógica de negocio para cuentas, saldo, transferencias e historial bancario.
/// </resumen>
 
using BankApp.Application.Common.Interfaces;
using BankApp.Application.DTOs;
using BankApp.Domain.Entities;
using BankApp.Domain.Exceptions;
using BankApp.Domain.Repositories;

namespace BankApp.Application.Services;


public sealed class AccountService : IAccountService 
{
    private readonly IAccountRepository _accountRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IAccountNumberGenerator _accountNumberGenerator;
    private readonly IUnitOfWork _unitOfWork;

    public AccountService( // Inyección de Dependencias mediante el constructor
        IAccountRepository accountRepository,
        ICustomerRepository customerRepository,
        IAccountNumberGenerator accountNumberGenerator,
        IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _customerRepository = customerRepository;
        _accountNumberGenerator = accountNumberGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<AccountResponseDto> CreateAccountAsync(CreateAccountRequestDto request, CancellationToken cancellationToken = default) 
    {
        // Validamos que el cliente exista en el banco 
        var customerExists = await _customerRepository.ExistsAsync(request.CustomerId, cancellationToken);
        if (!customerExists)
        {
            throw new CustomerNotFoundException(request.CustomerId);
        }

        // Generamos un número de cuenta único que cumpla con ACC-YYYYMMDD-[XXXX]
        string accountNumber;
        do
        {
            accountNumber = _accountNumberGenerator.Generate(); 
        } 
        while (await _accountRepository.ExistsAccountNumberAsync(accountNumber, cancellationToken)); // Validamos que el número de cuenta generado sea único en la base de datos

        // Creamos la entidad de Dominio (si initialDeposit > 0, genera su primera transacción)
        var account = new Account(accountNumber, request.CustomerId, request.InitialDeposit);

        // Guardar en repositorio y confirmar de forma atómica
        await _accountRepository.AddAsync(account, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Mapeamos la entidad hacia el DTO de salida
        return new AccountResponseDto(
            account.Id,
            account.AccountNumber,
            account.Balance,
            account.CustomerId,
            account.CreatedAt
        );
    }

    public async Task<BalanceResponseDto> GetBalanceAsync(string accountNumber, CancellationToken cancellationToken = default)
    {
        // Consultar saldo por número de cuenta
        var account = await _accountRepository.GetByAccountNumberAsync(accountNumber, cancellationToken);
        if (account is null)
        {
            throw new AccountNotFoundException(accountNumber);
        }

        return new BalanceResponseDto(
            account.AccountNumber,
            account.Balance,
            DateTime.UtcNow
        );
    }

    public async Task<TransactionResponseDto> DepositAsync(string accountNumber, TransactionRequestDto request, CancellationToken cancellationToken = default)
    {
        // Depósitos
        var account = await _accountRepository.GetByAccountNumberAsync(accountNumber, cancellationToken);
        if (account is null)
        {
            throw new AccountNotFoundException(accountNumber);
        }

        account.Deposit(request.Amount); // la entidad valida si el depósito es posible, y si no, manda la excepción que creamos para este caso, cumpliendo con uno de los requisitos.

        await _accountRepository.UpdateAsync(account, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Conseguimos la última transacción registrada
        var transaction = account.Transactions.Last();

        return new TransactionResponseDto(
            transaction.Id,
            transaction.Type.ToString(),
            transaction.Amount,
            transaction.ResultingBalance,
            transaction.Timestamp
        );
    }

    public async Task<TransactionResponseDto> WithdrawAsync(string accountNumber, TransactionRequestDto request, CancellationToken cancellationToken = default)
    {
        // Retiros con validación de fondos
        var account = await _accountRepository.GetByAccountNumberAsync(accountNumber, cancellationToken);
        if (account is null)
        {
            throw new AccountNotFoundException(accountNumber);
        }

        // Si no hay saldo suficiente, la entidad manda InsufficientFundsException
        account.Withdraw(request.Amount);

        await _accountRepository.UpdateAsync(account, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var transaction = account.Transactions.Last();

        return new TransactionResponseDto(
            transaction.Id,
            transaction.Type.ToString(),
            transaction.Amount,
            transaction.ResultingBalance,
            transaction.Timestamp
        );
    }

    public async Task<AccountHistoryResponseDto> GetHistoryAsync(string accountNumber, CancellationToken cancellationToken = default)
    {
        // Historial ordenado cronológicamente con el saldo resultante
        var account = await _accountRepository.GetByAccountNumberAsync(accountNumber, cancellationToken);
        if (account is null)
        {
            throw new AccountNotFoundException(accountNumber);
        }

        var transactionDtos = account.Transactions
            .OrderBy(t => t.Timestamp)
            .Select(t => new TransactionResponseDto(
                t.Id,
                t.Type.ToString(),
                t.Amount,
                t.ResultingBalance,
                t.Timestamp
            ))
            .ToList();

        return new AccountHistoryResponseDto(
            account.AccountNumber,
            account.Balance,
            transactionDtos
        );
    }
}
