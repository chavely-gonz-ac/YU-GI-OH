using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;
using MediatR;

namespace YuGiOh.Application.Behaviors
{
    /// <summary>
    /// MediatR pipeline behavior that applies FluentValidation validators before handling a request.
    /// </summary>
    /// <typeparam name="TRequest">The type of the incoming request.</typeparam>
    /// <typeparam name="TResponse">The type of the response returned by the handler.</typeparam>
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationBehavior{TRequest, TResponse}"/> class.
        /// Supports multiple validators for a single request.
        /// </summary>
        /// <param name="validators">A collection of validators applicable to the request.</param>
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        /// <summary>
        /// Handles validation logic before the request reaches its handler.
        /// </summary>
        /// <param name="request">The incoming request to validate.</param>
        /// <param name="next">The next delegate in the pipeline (usually the handler).</param>
        /// <param name="cancellationToken">A token to observe cancellation.</param>
        /// <returns>The handler's response if validation passes; otherwise throws <see cref="ValidationException"/>.</returns>
        /// <exception cref="ValidationException">Thrown when validation fails.</exception>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                // Validate using all validators asynchronously
                var validationTasks = _validators
                    .Select(v => v.ValidateAsync(request, cancellationToken));

                var validationResults = await Task.WhenAll(validationTasks);

                var failures = validationResults
                    .SelectMany(r => r.Errors)
                    .Where(f => f != null)
                    .ToList();

                if (failures.Count != 0)
                {
                    // Aggregate and throw all validation errors
                    throw new ValidationException(failures);
                }
            }

            // Continue to the next behavior or handler
            return await next();
        }
    }
}