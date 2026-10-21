using BankApp.Application.Common.Interfaces;
using BankApp.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    // Solo inyectamos la interfaz del servicio (¡Cero DbContext!)
    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    /// Crear perfil del cliente (Nombre completo, fecha de nacimiento, género e ingresos).
    [HttpPost]
    [ProducesResponseType(typeof(CustomerResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequestDto request, CancellationToken cancellationToken)
    {
        var customer = await _customerService.CreateCustomerAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, customer);
    }

    /// Consultar perfil de cliente por su ID.
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CustomerResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCustomerById(Guid id, CancellationToken cancellationToken)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id, cancellationToken);
        return Ok(customer);
    }
}
