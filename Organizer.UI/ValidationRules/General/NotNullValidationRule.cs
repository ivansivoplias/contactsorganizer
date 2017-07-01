using System.Globalization;
using System.Windows.Controls;

namespace Organizer.UI.ValidationRules
{
    public class NotNullValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return new ValidationResult(false, "Value cannot be empty");
            }

            return ValidationResult.ValidResult;
        }
    }
}