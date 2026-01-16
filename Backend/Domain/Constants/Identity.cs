namespace Domain.Constants;

public static class Identity
{
    public static class LoginProvider
    {
        public const string Google = "Google";
    }
    
    public static class TokenExpiredTime
    {
        public const int EmailConfirmationHours = 2;
        public const int PasswordResetCodeMinutes = 15;
    }
}