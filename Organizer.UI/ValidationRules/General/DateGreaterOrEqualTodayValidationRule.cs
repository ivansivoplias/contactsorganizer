using System;
using System.Globalization;
using System.Windows.Controls;

namespace Organizer.UI.ValidationRules
{
    public class DateGreaterOrEqualTodayValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var dateString = value?.ToString();

            DateTime date;
            if (!string.IsNullOrEmpty(dateString) && DateTime.TryParse(dateString, out date))
            {
                if (date >= DateTime.Today)
                {
                    return ValidationResult.ValidResult;
                }
            }

            return new ValidationResult(false, "Input date is not valid. Please input date in right format.");
        }
    }
}