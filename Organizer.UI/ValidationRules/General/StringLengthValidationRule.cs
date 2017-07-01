using System.Globalization;
using System.Windows.Controls;

namespace Organizer.UI.ValidationRules
{
    public class StringLengthValidationRule : ValidationRule
    {
        public int MinLength { get; set; }

        public int MaxLength { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var text = value as string;

            if (!string.IsNullOrEmpty(text) && (text.Length > MaxLength || text.Length < MinLength))
            {
                return new ValidationResult(false, $"String length is not in range ({MinLength}, {MaxLength}).");
            }

            return ValidationResult.ValidResult;
        }
    }
}