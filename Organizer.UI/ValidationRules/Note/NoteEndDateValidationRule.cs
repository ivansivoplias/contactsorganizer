using Organizer.UI.Helpers;
using System;
using System.Globalization;
using System.Windows.Controls;

namespace Organizer.UI.ValidationRules
{
    public class NoteEndDateValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var dateString = value?.ToString();

            var wrappedString = Wrapper.WrappedData?.ToString();

            if (string.IsNullOrEmpty(dateString))
            {
                if (!string.IsNullOrEmpty(wrappedString))
                {
                    return new ValidationResult(false, "Note end date is not selected.");
                }
                else
                {
                    return ValidationResult.ValidResult;
                }
            }

            DateTime date, wrappedDate;

            if (DateTime.TryParse(dateString, out date)
                && DateTime.TryParse(wrappedString, out wrappedDate) && date.Date <= wrappedDate.Date)
            {
                return new ValidationResult(false, "End date cannot be less than or equal to start date.");
            }

            return ValidationResult.ValidResult;
        }

        public Wrapper Wrapper { get; set; }
    }
}