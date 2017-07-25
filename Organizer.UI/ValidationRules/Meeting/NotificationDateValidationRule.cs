using Organizer.UI.Helpers;
using System;
using System.Globalization;
using System.Windows.Controls;

namespace Organizer.UI.ValidationRules
{
    public class NotificationDateValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var dateString = value?.ToString();

            var wrappedString = Wrapper.WrappedData?.ToString();
            bool isActive = false;

            bool.TryParse(IsActiveRule.WrappedData?.ToString(), out isActive);
            if (isActive)
            {
                if (string.IsNullOrEmpty(dateString))
                {
                    if (!string.IsNullOrEmpty(wrappedString))
                    {
                        return new ValidationResult(false, "Notification date is not selected.");
                    }
                    else
                    {
                        return ValidationResult.ValidResult;
                    }
                }

                DateTime date, wrappedDate;

                if (DateTime.TryParse(dateString, out date)
                    && DateTime.TryParse(wrappedString, out wrappedDate) && (date.Date > wrappedDate.Date))
                {
                    return new ValidationResult(false, "Notification date cannot be greater than meeting date.");
                }
            }

            return ValidationResult.ValidResult;
        }

        public Wrapper IsActiveRule { get; set; }

        public Wrapper Wrapper { get; set; }
    }
}