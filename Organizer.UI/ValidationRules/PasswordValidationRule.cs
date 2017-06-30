using Organizer.UI.Helpers;
using System.Globalization;
using System.Security;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Organizer.UI.ValidationRules
{
    public class PasswordValidationRule : ValidationRule
    {
        private static readonly Regex _passwordRegex = new Regex("^[A-Za-z0-9_-]+$", RegexOptions.Compiled);

        public int MinLength { get; set; }

        public int MaxLength { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || (value as SecureString).Length == 0)
                return new ValidationResult(false, "Password cannot be empty.");

            var password = (value as SecureString)?.SecureStringToString();

            if (password.Length < MinLength || password.Length > MaxLength)
                return new ValidationResult(false, $"Password length are not in range ({MinLength}, {MaxLength}).");

            if (!_passwordRegex.IsMatch(password))
                return new ValidationResult(false, "Passoword contains forbidden symbols. Please use english letters, digits _ and -.");

            return ValidationResult.ValidResult;
        }
    }
}