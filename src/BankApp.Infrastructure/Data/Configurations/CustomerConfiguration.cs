/// <resumen>
/// Configuración Fluent API para la tabla de Clientes (Requisito 1).
/// </resumen>

using BankApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankApp.Infrastructure.Data.Configurations;


public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id); // Llave primaria

        builder.Property(c => c.FullName) // Nombre completo
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(c => c.DateOfBirth) // Fecha de nacimiento
            .IsRequired();

        builder.Property(c => c.Gender) // Género
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(c => c.MonthlyIncome) // Ingresos mensuales
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(c => c.CreatedAt) // Fecha de creación
            .IsRequired();
    }
}
