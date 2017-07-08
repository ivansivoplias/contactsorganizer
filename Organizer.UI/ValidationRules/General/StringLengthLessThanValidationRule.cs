using System.Globalization;
using System.Windows.Controls;

namespace Organizer.UI.ValidationRules
{
    public class StringLengthLessThanValidationRule : ValidationRule
    {
        public int MaxLength { get; set; }

        public string PropertyName { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var text = value as string;

            if (!string.IsNullOrEmpty(text) && text.Length > MaxLength)
            {
                return new ValidationResult(false, $"{PropertyName} length is greater than {MaxLength}.");
            }

            return ValidationResult.ValidResult;
        }
    }
}