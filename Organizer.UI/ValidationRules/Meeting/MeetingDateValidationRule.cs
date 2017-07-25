using System;
using System.Globalization;
using System.Windows.Controls;

namespace Organizer.UI.ValidationRules
{
    public class MeetingDateValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            DateTime date;

            if (DateTime.TryParse(value.ToString(), out date))
            {
                if (date < DateTime.Today)
                {
                    return new ValidationResult(false, "Date cannot be less than today.");
                }
            }
            else
            {
                return new ValidationResult(false, "Value is not date. Pick a right date and try again.");
            }

            return ValidationResult.ValidResult;
        }
    }
}