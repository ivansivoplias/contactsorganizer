using System.Globalization;
using System.Security;
using System.Windows.Controls;

namespace Organizer.UI.ValidationRules
{
    public class SecureStringNotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var secure = value as SecureString;

            if (secure == null || secure.Length == 0)
            {
                return new ValidationResult(false, "Field cannot be empty.");
            }

            return ValidationResult.ValidResult;
        }
    }
}