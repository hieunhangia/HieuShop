namespace Application.Common.Exceptions;

public abstract class BaseException : Exception
{
    public string Title { get; protected init; } = "Lỗi không xác định.";
    public string Detail { get; protected init; } = "Đã xảy ra lỗi không xác định trong quá trình xử lý yêu cầu.";
}