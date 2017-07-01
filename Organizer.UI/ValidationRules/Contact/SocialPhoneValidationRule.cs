using Organizer.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Organizer.UI.ValidationRules
{
    public class SocialPhoneValidationRule : ValidationRule
    {
        private static readonly Regex _phoneRegex = new Regex(@"^(\+380|380)[0-9]{9}$", RegexOptions.Compiled);

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var stringValue = value as string;

            string wrapped = Wrapper.WrappedData?.ToString();

            if (wrapped != null && wrapped.Equals("Phone"))
            {
                if (string.IsNullOrEmpty(stringValue))
                {
                    return new ValidationResult(false, "Phone number should not be empty.");
                }

                if (!_phoneRegex.IsMatch(stringValue))
                {
                    return new ValidationResult(false, "Phone is not valid phone number.");
                }
            }

            return ValidationResult.ValidResult;
        }

        public Wrapper Wrapper { get; set; }
    }
}