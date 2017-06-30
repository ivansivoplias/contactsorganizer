using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Organizer.UI.ValidationRules
{
    public class LoginValidationRule : ValidationRule
    {
        private static readonly Regex _loginRegex =
            new Regex("^(?=.{8,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$", RegexOptions.Compiled);

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrEmpty(value as string))
                return new ValidationResult(false, "Login cannot be empty.");

            var login = value as string;

            if (!_loginRegex.IsMatch(login))
                return new ValidationResult(false, "Login contains forbidden symbols. Please use english letters, digits . and _.");

            return ValidationResult.ValidResult;
        }
    }
}