namespace Application.Common.Exceptions;

public class ForbiddenAccessException : BaseException
{
    public ForbiddenAccessException(string detail)
    {
        Title = "Yêu cầu bị từ chối.";
        Detail = detail;
    }

    public ForbiddenAccessException(string title, string detail) : this(detail) => Title = title;
}