/// <resumen>
/// Configuración Fluent API para la entidad Account con índice único en AccountNumber.
/// </resumen>

using BankApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankApp.Infrastructure.Data.Configurations;


public sealed class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        // Nombre de la tabla
        builder.ToTable("Accounts");

        // Llave primaria
        builder.HasKey(a => a.Id);

        // Índice único a nivel físico en la base de datos
        builder.Property(a => a.AccountNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(a => a.AccountNumber) // Creamos un índice único en la columna AccountNumber
            .IsUnique();

        // Configuración precisa para el saldo
        builder.Property(a => a.Balance)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(a => a.CustomerId) // Llave foránea hacia la entidad Customer
            .IsRequired();

        builder.Property(a => a.CreatedAt)
            .IsRequired();

        // Relación 1 a Muchos: En una cuenta pueden haber varias transacciones
        builder.HasMany(a => a.Transactions)
            .WithOne()
            .HasForeignKey(t => t.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        // Mapeando la colección privada _transactions encapsulada
        builder.Navigation(a => a.Transactions)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
