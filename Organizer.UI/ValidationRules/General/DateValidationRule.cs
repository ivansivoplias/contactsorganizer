using System;
using System.Globalization;
using System.Windows.Controls;

namespace Organizer.UI.ValidationRules
{
    public class DateValidationRule : ValidationRule
    {
        public string PropertyName { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var dateString = value?.ToString();

            DateTime date;
            if (string.IsNullOrEmpty(dateString) || !DateTime.TryParse(dateString, out date))
            {
                return new ValidationResult(false, $"{PropertyName} is not valid date.");
            }

            return ValidationResult.ValidResult;
        }
    }
}