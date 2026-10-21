using BankApp.Domain.Entities;
using BankApp.Domain.Enums;
using BankApp.Domain.Exceptions;
using FluentAssertions;

namespace BankApp.Tests;

/// <resumen>
/// Pruebas unitarias indispensables de las reglas financieras del banco (Regla Crítica #5 de LAFISE).
/// </resumen>
public sealed class AccountTests
{
    private readonly string _validAccountNumber = "ACC-20261021-1234";
    private readonly Guid _customerId = Guid.NewGuid();

    [Fact]
    public void Deposit_WithValidAmount_ShouldIncreaseBalanceAndRecordTransaction()
    {
        // Arrange: Creamos una cuenta con saldo inicial 0
        var account = new Account(_validAccountNumber, _customerId, initialDeposit: 0);
        decimal depositAmount = 500m;

        // Act: Realizamos el depósito
        account.Deposit(depositAmount);

        // Assert: El saldo debe ser 500 y debe haber exactamente 1 transacción
        account.Balance.Should().Be(500m);
        account.Transactions.Should().HaveCount(1);

        var tx = account.Transactions.First();
        tx.Type.Should().Be(TransactionType.Deposit);
        tx.Amount.Should().Be(depositAmount);
        tx.ResultingBalance.Should().Be(500m); // Saldo histórico
    }

    [Fact]
    public void Withdraw_WithSufficientFunds_ShouldDecreaseBalanceAndRecordTransaction()
    {
        // Arrange: Cuenta con depósito inicial de $1,000
        var account = new Account(_validAccountNumber, _customerId, initialDeposit: 1000m);
        decimal withdrawAmount = 400m;

        // Act: Retiramos $400
        account.Withdraw(withdrawAmount);

        // Assert: El saldo debe quedar en $600
        account.Balance.Should().Be(600m);
        account.Transactions.Should().HaveCount(2); // El depósito inicial + el retiro

        var lastTx = account.Transactions.Last();
        lastTx.Type.Should().Be(TransactionType.Withdrawal);
        lastTx.Amount.Should().Be(withdrawAmount);
        lastTx.ResultingBalance.Should().Be(600m); // Saldo histórico resultante
    }

    [Fact]
    public void Withdraw_WithInsufficientFunds_ShouldThrowInsufficientFundsException()
    {
        // Arrange: Cuenta con solo $200 de saldo
        var account = new Account(_validAccountNumber, _customerId, initialDeposit: 200m);
        decimal withdrawAmount = 500m;

        // Act: Intentamos retirar más de lo que hay
        Action act = () => account.Withdraw(withdrawAmount);

        // Assert: Debe rechazar limpiamente disparando InsufficientFundsException
        act.Should().Throw<InsufficientFundsException>()
            .Where(ex => ex.CurrentBalance == 200m && ex.AttemptedAmount == 500m);

        // El saldo no debe haber cambiado
        account.Balance.Should().Be(200m);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-50)]
    public void Deposit_WithZeroOrNegativeAmount_ShouldThrowArgumentException(decimal invalidAmount)
    {
        // Arrange
        var account = new Account(_validAccountNumber, _customerId);

        // Act
        Action act = () => account.Deposit(invalidAmount);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_WithInvalidAccountNumberFormat_ShouldThrowArgumentException()
    {
        // Act: Intentamos crear una cuenta con formato inválido
        Action act = () => new Account("CUENTA-INVALIDA", _customerId);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*ACC-YYYYMMDD-[XXXX]*");
    }
}
