using System.Text.RegularExpressions;
using System.Linq;
namespace API.Services;

public static class PasswordValidatorUtility
{
    public static bool IsValidPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return false;

        // Check length
        if (password.Length < 8)
            return false;

        // Check for at least one digit
        if (!password.Any(char.IsDigit))
            return false;

        // Check for at least one uppercase letter
        if (!password.Any(char.IsUpper))
            return false;

        // Check for at least one lowercase letter
        if (!password.Any(char.IsLower))
            return false;

        // Check for at least one special character
        var specialChars = "!@#$%^&*()_+[]{}|;:,.<>?";
        if (!password.Any(c => specialChars.Contains(c)))
            return false;

        return true;
    }
}