using InscriptionApp.Api.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InscriptionApp.Api.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger) => _logger = logger;

    public void OnException(ExceptionContext context)
    {
        var (status, title) = context.Exception switch
        {
            NotFoundException => (StatusCodes.Status404NotFound, "Ressource introuvable"),
            EmailDejaUtiliseException => (StatusCodes.Status409Conflict, "Conflit"),
            TransitionEtatInvalideException => (StatusCodes.Status409Conflict, "Transition d'état invalide"),
            _ => (StatusCodes.Status500InternalServerError, "Erreur interne")
        };

        if (status == StatusCodes.Status500InternalServerError)
            _logger.LogError(context.Exception, "Erreur non gérée");

        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = status == StatusCodes.Status500InternalServerError
                ? "Une erreur inattendue est survenue."
                : context.Exception.Message
        };

        context.Result = new ObjectResult(problem) { StatusCode = status };
        context.ExceptionHandled = true;
    }
}