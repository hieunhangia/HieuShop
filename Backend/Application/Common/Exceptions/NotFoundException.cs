namespace Application.Common.Exceptions;

public class NotFoundException : BaseException
{
    public NotFoundException(string detail)
    {
        Title = "Không tìm thấy tài nguyên.";
        Detail = detail;
    }

    public NotFoundException(string title, string detail) : this(detail) => Title = title;
}