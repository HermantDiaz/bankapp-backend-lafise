namespace BankApp.Domain.Entities;

/// <resumen>
/// Perfil del cliente titular.
/// </resumen>
public sealed class Customer
{
    public Guid Id { get; private set; } // Identificador único del cliente.
    public string FullName { get; private set; } = string.Empty; // Nombre completo del cliente.
    public DateOnly DateOfBirth { get; private set; } // Fecha de nacimiento del cliente.
    public string Gender { get; private set; } = string.Empty; // Género del cliente.
    public decimal MonthlyIncome { get; private set; } // Ingresos mensuales del cliente.
    public DateTime CreatedAt { get; private set; } // Fecha de creación del cliente.

    // Constructor privado para Entity Framework Core
    private Customer() { }

    // Constructor de negocio
    public Customer(string fullName, DateOnly dateOfBirth, string gender, decimal monthlyIncome)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("El nombre completo del cliente es obligatorio.", nameof(fullName));

        if (dateOfBirth > DateOnly.FromDateTime(DateTime.UtcNow))
            throw new ArgumentException("La fecha de nacimiento no puede ser una fecha futura.", nameof(dateOfBirth));

        if (string.IsNullOrWhiteSpace(gender))
            throw new ArgumentException("El género del cliente es obligatorio.", nameof(gender));

        if (monthlyIncome < 0)
            throw new ArgumentException("Los ingresos mensuales no pueden ser negativos.", nameof(monthlyIncome));

        Id = Guid.NewGuid();
        FullName = fullName.Trim();
        DateOfBirth = dateOfBirth;
        Gender = gender.Trim();
        MonthlyIncome = monthlyIncome;
        CreatedAt = DateTime.UtcNow;
        // Inicializamos la lista de cuentas asociadas al cliente
    }
}
