using Organizer.Common.Enums.SearchTypes;
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
    public class ContactSearchValidationRule : ValidationRule
    {
        private static readonly Regex _phoneRegex = new Regex(@"^(\+380|380)[0-9]{9}$", RegexOptions.Compiled);

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var stringValue = value as string;

            ContactSearchType wrappedEnum;

            if (Wrapper.WrappedData != null && Enum.TryParse(Wrapper.WrappedData.ToString(), out wrappedEnum))
            {
                if (string.IsNullOrEmpty(stringValue) && wrappedEnum != ContactSearchType.Default)
                {
                    return new ValidationResult(false, "Search value is empty.");
                }

                switch (wrappedEnum)
                {
                    case ContactSearchType.ByPhone:
                        if (!_phoneRegex.IsMatch(stringValue))
                        {
                            return new ValidationResult(false, "String is not a valid phone number.");
                        }
                        break;
                }
            }

            return ValidationResult.ValidResult;
        }

        public Wrapper Wrapper { get; set; }
    }
}