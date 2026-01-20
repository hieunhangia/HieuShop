namespace Application.Common.Exceptions;

public class BadRequestException : BaseException
{
    public BadRequestException(string detail)
    {
        Title = "Yêu cầu không hợp lệ.";
        Detail = detail;
    }

    public BadRequestException(string title, string detail) : this(detail) => Title = title;
}