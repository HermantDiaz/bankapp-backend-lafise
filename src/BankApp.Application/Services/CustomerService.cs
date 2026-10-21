/// <resumen>
/// Orquesta los casos de uso para la creación y consulta de perfiles de clientes.
/// </resumen>

using BankApp.Application.Common.Interfaces;
using BankApp.Application.DTOs;
using BankApp.Domain.Entities;
using BankApp.Domain.Exceptions;
using BankApp.Domain.Repositories;

namespace BankApp.Application.Services;


public sealed class CustomerService : ICustomerService 
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork; 

    // Inyección de Dependencias mediante el constructor
    public CustomerService(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CustomerResponseDto> CreateCustomerAsync(CreateCustomerRequestDto request, CancellationToken cancellationToken = default)
    {
        // Instanciamos la entidad pura de Dominio
        var customer = new Customer(
            request.FullName,
            request.DateOfBirth,
            request.Gender,
            request.MonthlyIncome
        );

        // Registramos en el repositorio
        await _customerRepository.AddAsync(customer, cancellationToken);

        // Confirmamos la transacción atómica
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Mapeamos la entidad hacia el DTO de salida
        return new CustomerResponseDto(
            customer.Id,
            customer.FullName,
            customer.DateOfBirth,
            customer.Gender,
            customer.MonthlyIncome,
            customer.CreatedAt
        );
    }

    public async Task<CustomerResponseDto> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken = default) // Consultamos un cliente por su id
    {
        var customer = await _customerRepository.GetByIdAsync(id, cancellationToken);

        // Si no existe, mandamos la excepción de dominio (el middleware global la traducirá a 404 Not Found)
        if (customer is null)
        {
            throw new CustomerNotFoundException(id);
        }

        return new CustomerResponseDto(
            customer.Id,
            customer.FullName,
            customer.DateOfBirth,
            customer.Gender,
            customer.MonthlyIncome,
            customer.CreatedAt
        );
    }
}
