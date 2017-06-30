using Organizer.UI.Helpers;
using System.Globalization;
using System.Security;
using System.Windows.Controls;

namespace Organizer.UI.ValidationRules
{
    public class RepeatedPasswordValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var secureValue = value as SecureString;

            var wrappedPassword = Wrapper.WrappedData as SecureString;

            if (secureValue.IsNullOrEmpty())
            {
                if (!wrappedPassword.IsNullOrEmpty())
                {
                    return new ValidationResult(false, "Repeated password cannot be empty.");
                }
                else
                {
                    return ValidationResult.ValidResult;
                }
            }

            var repeatedPassword = secureValue.SecureStringToString();

            if (wrappedPassword != null)
            {
                if (!wrappedPassword.SecureStringToString().Equals(repeatedPassword))
                {
                    return new ValidationResult(false, "Repeated password are not equal to password.");
                }
            }
            else
            {
                return new ValidationResult(false, "Repeated password are not equal to password.");
            }

            return ValidationResult.ValidResult;
        }

        public Wrapper Wrapper { get; set; }
    }
}