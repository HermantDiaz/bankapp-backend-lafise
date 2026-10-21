using BankApp.Application.Common.Interfaces;
using BankApp.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;

    // Solo inyectamos IAccountService (Thin Controller sin persistencia directa)
    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    
    /// Crear cuenta bancaria con número autogenerado ACC-YYYYMMDD-[XXXX] y saldo inicial.
    
    [HttpPost]
    [ProducesResponseType(typeof(AccountResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequestDto request, CancellationToken cancellationToken)
    {
        var account = await _accountService.CreateAccountAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetBalance), new { accountNumber = account.AccountNumber }, account);
    }

    
    /// Consultar saldo actual de la cuenta por su número único.
    
    [HttpGet("{accountNumber}/balance")]
    [ProducesResponseType(typeof(BalanceResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBalance(string accountNumber, CancellationToken cancellationToken)
    {
        var balance = await _accountService.GetBalanceAsync(accountNumber, cancellationToken);
        return Ok(balance);
    }

    
    /// Registrar un depósito en la cuenta.
    
    [HttpPost("{accountNumber}/deposit")]
    [ProducesResponseType(typeof(TransactionResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deposit(string accountNumber, [FromBody] TransactionRequestDto request, CancellationToken cancellationToken)
    {
        var transaction = await _accountService.DepositAsync(accountNumber, request, cancellationToken);
        return Ok(transaction);
    }

    
    /// Registrar un retiro con validación de fondos (Rechazo limpio con HTTP 400).
    
    [HttpPost("{accountNumber}/withdraw")]
    [ProducesResponseType(typeof(TransactionResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Withdraw(string accountNumber, [FromBody] TransactionRequestDto request, CancellationToken cancellationToken)
    {
        var transaction = await _accountService.WithdrawAsync(accountNumber, request, cancellationToken);
        return Ok(transaction);
    }

    /// Consultar historial cronológico con saldo histórico resultante.
    [HttpGet("{accountNumber}/history")]
    [ProducesResponseType(typeof(AccountHistoryResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHistory(string accountNumber, CancellationToken cancellationToken)
    {
        var history = await _accountService.GetHistoryAsync(accountNumber, cancellationToken);
        return Ok(history);
    }
}
