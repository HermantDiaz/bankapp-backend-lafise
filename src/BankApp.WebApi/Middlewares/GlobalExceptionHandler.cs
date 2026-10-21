/// <resumen>
/// Manejador global centralizado de excepciones.
/// Intercepta excepciones de dominio y las traduce a respuestas ProblemDetails (RFC 7807) sin try-catch en controladores.
/// </resumen>

using BankApp.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.WebApi.Middlewares;


public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)// Inyección de dependencia para el logger
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Excepción capturada globalmente: {Message}", exception.Message);

        // Mapeo semántico de códigos HTTP según el tipo de excepción de negocio
        var (statusCode, title, detail) = exception switch
        {
            InsufficientFundsException ex => (
                StatusCodes.Status400BadRequest,
                "Fondos Insuficientes",
                ex.Message
            ),
            AccountNotFoundException ex => (
                StatusCodes.Status404NotFound,
                "Cuenta No Encontrada",
                ex.Message
            ),
            CustomerNotFoundException ex => (
                StatusCodes.Status404NotFound,
                "Cliente No Encontrado",
                ex.Message
            ),
            ArgumentException ex => (
                StatusCodes.Status400BadRequest,
                "Solicitud Inválida",
                ex.Message
            ),
            _ => (
                StatusCodes.Status500InternalServerError,
                "Error Interno del Servidor",
                "Ocurrió un error inesperado al procesar su transacción bancaria. Por favor intente más tarde."
            )
        };

        // Formato de error en APIs REST empresariales (RFC 7807 Problem Details)
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        // Retornamos true para indicar a ASP.NET Core que el error fue gestionado y cerrado exitosamente
        return true;
    }
}
