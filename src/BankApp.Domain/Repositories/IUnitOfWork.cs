/// <resumen>
/// Definimos el contrato para la gestión de la unidad de trabajo, que permite confirmar los cambios en la base de datos.
/// </resumen>
namespace BankApp.Domain.Repositories;
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default); // confirmamos los cambios en la base de datos y retornamos el numero de registros afectados.
}
