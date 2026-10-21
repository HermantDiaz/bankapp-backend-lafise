/// <resumen>
/// Servicio encargado de generar números de cuenta con el formato requerido por el banco.
/// </resumen>
namespace BankApp.Application.Common.Interfaces;


public interface IAccountNumberGenerator
{
    string Generate();
}
