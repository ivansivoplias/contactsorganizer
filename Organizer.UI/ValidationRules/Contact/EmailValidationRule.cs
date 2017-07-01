using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Organizer.UI.ValidationRules
{
    public class EmailValidationRule : ValidationRule
    {
        private static readonly Regex _emailRegex = new Regex(@"[\w\.\-\+']+@([\w\-\+']\.?)+\.([\w\-])+",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var email = value as string;

            if (!string.IsNullOrEmpty(email) && !_emailRegex.IsMatch(email))
            {
                return new ValidationResult(false, "Email string is not valid. Valid examples: email@google.com.");
            }

            return ValidationResult.ValidResult;
        }
    }
}