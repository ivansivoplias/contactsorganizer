using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Organizer.UI.ValidationRules
{
    public class LoginValidationRule : ValidationRule
    {
        public int MinLoginLength { get; set; }

        public int MaxLoginLength { get; set; }

        private static readonly Regex _loginRegex =
            new Regex("^(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$", RegexOptions.Compiled);

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrEmpty(value as string))
                return new ValidationResult(false, "Login cannot be empty.");

            var login = value as string;

            if (login.Length > MaxLoginLength || login.Length < MinLoginLength)
            {
                return new ValidationResult(false, $"Login cannot be less than {MinLoginLength} and greater than {MaxLoginLength} symbols.");
            }

            if (!_loginRegex.IsMatch(login))
                return new ValidationResult(false, "Login contains forbidden symbols. Please use english letters, digits . and _.");

            return ValidationResult.ValidResult;
        }
    }
}