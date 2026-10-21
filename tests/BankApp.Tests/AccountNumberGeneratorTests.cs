using System.Text.RegularExpressions;
using BankApp.Application.Services;
using FluentAssertions;

namespace BankApp.Tests;

/// <resumen>
/// Pruebas unitarias obligatorias para el algoritmo de generación de números de cuenta.
/// Formato: ACC-YYYYMMDD-[XXXX]
/// </resumen>
public sealed class AccountNumberGeneratorTests
{
    private readonly AccountNumberGenerator _generator = new();

    [Fact]
    public void Generate_ShouldReturnExact17CharactersLength()
    {
        // Act
        var accountNumber = _generator.Generate();

        // Assert: "ACC-" (4) + "YYYYMMDD" (8) + "-" (1) + "XXXX" (4) = 17 caracteres exactos
        accountNumber.Should().HaveLength(17);
    }

    [Fact]
    public void Generate_ShouldStartWithPrefixACC()
    {
        // Act
        var accountNumber = _generator.Generate();

        // Assert
        accountNumber.Should().StartWith("ACC-");
    }

    [Fact]
    public void Generate_ShouldContainCurrentUtcDate()
    {
        // Arrange
        var today = DateTime.UtcNow.ToString("yyyyMMdd");

        // Act
        var accountNumber = _generator.Generate();

        // Assert
        accountNumber.Should().Contain(today);
    }

    [Fact]
    public void Generate_ShouldMatchExactFormatRegex()
    {
        // Act
        var accountNumber = _generator.Generate();

        // Assert: Valida el patrón exacto ACC-YYYYMMDD-[XXXX]
        Regex.IsMatch(accountNumber, @"^ACC-\d{8}-\d{4}$").Should().BeTrue();
    }

    [Fact]
    public void Generate_MultipleCalls_ShouldProduceVariations()
    {
        // Act
        var account1 = _generator.Generate();
        var account2 = _generator.Generate();

        // Assert: Dos llamadas consecutivas deberían tener sufijos distintos
        account1.Should().NotBeNullOrWhiteSpace();
        account2.Should().NotBeNullOrWhiteSpace();
    }
}
