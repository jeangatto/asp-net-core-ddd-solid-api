using FluentResults;
using FluentValidation.Results;
using SGP.Shared.Errors;
using SGP.Shared.Messages;
using System.Collections.Generic;
using System.Linq;

namespace SGP.Shared.Extensions
{
    public static class BaseRequestExtensions
    {
        public static Result ToFail(this BaseRequest request)
        {
            return new Result()
                .WithErrors(request.ValidationResult?.ToErrors());
        }

        public static Result<TResponse> ToFail<TResponse>(this BaseRequest request)
        {
            return new Result<TResponse>()
                .WithErrors(request.ValidationResult?.ToErrors());
        }

        private static IEnumerable<Error> ToErrors(this ValidationResult validationResult)
        {
            if (validationResult.IsValid)
            {
                return Enumerable.Empty<Error>();
            }

            return validationResult.Errors.ConvertAll(f => new ValidationError(f));
        }
    }
}