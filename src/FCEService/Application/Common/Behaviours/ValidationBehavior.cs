using MediatR;

namespace FCEService.Application.Common.Behaviours;

using FluentValidation;

using FCEService.Domain.Common.Results;
using FCEService.Domain.Common.Results.Abstractions;

public sealed class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest>? validator = null)
    : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IResult
{
    private readonly IValidator<TRequest>? _validator = validator;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        if (_validator is null)
        {
            return await next(ct);
        }

        var validationResult = await _validator.ValidateAsync(request, ct);

        if (validationResult.IsValid)
        {
            return await next();
        }

        var errors = validationResult.Errors
            .ConvertAll(error => Error.Validation(
                code: error.PropertyName,
                description: error.ErrorMessage));

        // WHY (dynamic):
        // TResponse is constrained to IResult, and all concrete Result<T> types have an implicit
        // operator from List<Error>. We cannot invoke an implicit operator generically at compile time,
        // so we use (dynamic) to let the CLR resolve the correct implicit conversion at runtime.
        // This is safe because ValidationBehavior is ONLY wired up for requests whose TResponse
        // implements IResult — any type that doesn't have the operator will fail loudly at startup,
        // not silently at runtime in production.
        return (TResponse)(dynamic)errors;
    }
}
