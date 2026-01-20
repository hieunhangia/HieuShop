using FluentValidation.Results;

namespace Application.Common.Exceptions;

public class ValidationException : BaseException
{
    public IDictionary<string, string[]> Errors { get; }

    public ValidationException(IDictionary<string, string[]> errors)
    {
        Title = "Một hoặc nhiều lỗi xác thực đã xảy ra.";
        Detail = string.Empty;
        Errors = errors;
    }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this(failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray()))
    {
    }
}