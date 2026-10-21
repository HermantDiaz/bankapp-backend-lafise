/// <resumen>
/// Configuración Fluent API para el historial de transacciones.
/// </resumen>

using BankApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankApp.Infrastructure.Data.Configurations;


public sealed class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");

        builder.HasKey(t => t.Id); // Llave primaria

        builder.Property(t => t.AccountId) // Llave foránea hacia la entidad Account
            .IsRequired();

        builder.Property(t => t.Type)
            .IsRequired()
            .HasConversion<int>(); // Almacenamos el enum como número entero en SQLite

        builder.Property(t => t.Amount) // Cantidad de la transacción
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(t => t.ResultingBalance)// Saldo resultante después de la transacción
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(t => t.Timestamp)// Fecha y hora de la transacción
            .IsRequired();
    }
}
