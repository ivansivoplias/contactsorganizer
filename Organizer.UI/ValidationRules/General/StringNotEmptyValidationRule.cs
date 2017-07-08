using System.Globalization;
using System.Windows.Controls;

namespace Organizer.UI.ValidationRules
{
    public class StringNotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var strValue = value as string;

            if (strValue == null || string.IsNullOrEmpty(strValue) || string.IsNullOrWhiteSpace(strValue))
            {
                return new ValidationResult(false, "Field cannot be empty or whitespace.");
            }

            return ValidationResult.ValidResult;
        }
    }
}