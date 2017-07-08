using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Organizer.UI.ValidationRules
{
    public class PhoneValidationRule : ValidationRule
    {
        private static readonly Regex _phoneRegex = new Regex(@"^\+?[1-9]{1}[0-9]{3,14}$", RegexOptions.Compiled);

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var phone = value as string;

            if (string.IsNullOrEmpty(phone))
                return new ValidationResult(false, "Phone cannot be empty.");

            if (!_phoneRegex.IsMatch(phone))
            {
                return new ValidationResult(false, "Phone number in not valid. Valid numbers are: +380955555226, 3040");
            }

            return ValidationResult.ValidResult;
        }
    }
}