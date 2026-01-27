using System.Security.Claims;
using UnauthorizedAccessException = Application.Common.Exceptions.UnauthorizedAccessException;

namespace API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var guid))
        {
            throw new UnauthorizedAccessException("User ID không hợp lệ hoặc không tìm thấy.");
        }

        return guid;
    }
}