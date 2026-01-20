namespace Application.Common.Exceptions;

public class UnauthorizedAccessException : BaseException
{
    public UnauthorizedAccessException(string detail)
    {
        Title = "Quyền truy cập bị từ chối.";
        Detail = detail;
    }

    public UnauthorizedAccessException(string title, string detail) : this(detail) => Title = title;
}