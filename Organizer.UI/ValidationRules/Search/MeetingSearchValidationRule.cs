using Organizer.Common.Enums.SearchTypes;
using Organizer.UI.Helpers;
using System;
using System.Globalization;
using System.Windows.Controls;

namespace Organizer.UI.ValidationRules
{
    public class MeetingSearchValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var stringValue = value as string;

            MeetingSearchType wrappedEnum;

            if (Enum.TryParse(Wrapper.WrappedData.ToString(), out wrappedEnum))
            {
                if (string.IsNullOrEmpty(stringValue) && wrappedEnum != MeetingSearchType.Default)
                {
                    return new ValidationResult(false, "Search value is empty.");
                }

                switch (wrappedEnum)
                {
                    case MeetingSearchType.ByMeetingName:
                        if (string.IsNullOrEmpty(stringValue))
                            return new ValidationResult(false, "Caption is empty.");
                        break;

                    case MeetingSearchType.ByMeetingDate:
                        if (!ValidateDate(stringValue))
                        {
                            return new ValidationResult(false, "Meeting date is not valid.");
                        }
                        break;
                }
            }

            return ValidationResult.ValidResult;
        }

        private bool ValidateDate(string value)
        {
            DateTime date;

            return DateTime.TryParse(value, out date);
        }

        public Wrapper Wrapper { get; set; }
    }
}