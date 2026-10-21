/// <resumen>
/// Generador de números de cuenta con formato: ACC-YYYYMMDD-[XXXX].
/// </resumen>

using BankApp.Application.Common.Interfaces;
namespace BankApp.Application.Services;

public sealed class AccountNumberGenerator : IAccountNumberGenerator
{
    private readonly TimeProvider _timeProvider;

    public AccountNumberGenerator(TimeProvider? timeProvider = null)
    {
        // Si no se pasa un proveedor de tiempo (producción), usamos el reloj del sistema
        _timeProvider = timeProvider ?? TimeProvider.System; // Dependencia inyectable para facilitar pruebas unitarias y control del tiempo.
    }

    public string Generate()
    {
        // 1. Obtenemos la fecha UTC en formato YYYYMMDD (ej: 20261021)
        var datePart = _timeProvider.GetUtcNow().ToString("yyyyMMdd");

        // 2. Generamos 4 dígitos aleatorios garantizados (del 0000 al 9999), obligamos a que siempre tenga 4 dígitos usando ToString("D4")
        var randomPart = Random.Shared.Next(0, 10000).ToString("D4");

        // 3. Ensamblamos el formato final: ACC-YYYYMMDD-[XXXX]
        return $"ACC-{datePart}-{randomPart}";
    }
}
