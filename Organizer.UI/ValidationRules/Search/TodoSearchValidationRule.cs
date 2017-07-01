using Organizer.Common.Enums;
using Organizer.Common.Enums.SearchTypes;
using Organizer.UI.Helpers;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;

namespace Organizer.UI.ValidationRules
{
    public class TodoSearchValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var stringValue = value as string;

            TodoSearchType wrappedEnum;

            if (Enum.TryParse(Wrapper.WrappedData.ToString(), out wrappedEnum))
            {
                if (string.IsNullOrEmpty(stringValue) && wrappedEnum != TodoSearchType.Default)
                {
                    return new ValidationResult(false, "Search value is empty.");
                }

                switch (wrappedEnum)
                {
                    case TodoSearchType.ByCaptionLike:
                        if (!string.IsNullOrEmpty(stringValue))
                            return new ValidationResult(false, "Caption is empty.");
                        break;

                    case TodoSearchType.ByState:
                        if (!ValidateState(stringValue))
                        {
                            return new ValidationResult(false, "State is invalid.");
                        }
                        break;

                    case TodoSearchType.ByPriority:
                        if (!ValidatePriority(stringValue))
                        {
                            return new ValidationResult(false, "Priority is invalid.");
                        }
                        break;

                    case TodoSearchType.CreatedBetween:
                        if (!ValidateCreatedBetween(stringValue))
                        {
                            return new ValidationResult(false, "Date range is invalid. Please input right date range.");
                        }
                        break;

                    case TodoSearchType.ByStartDate:
                        if (!ValidateDate(stringValue))
                        {
                            return new ValidationResult(false, "Start date is invalid.");
                        }
                        break;

                    case TodoSearchType.ByEndDate:
                        if (!ValidateDate(stringValue))
                        {
                            return new ValidationResult(false, "End date is invalid.");
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

        private bool ValidateCreatedBetween(string value)
        {
            var dates = value.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                            .ToArray();

            DateTime start, end;

            bool res = false;

            if (dates.Length == 2 && DateTime.TryParse(dates[0], out start)
                && DateTime.TryParse(dates[1], out end))
            {
                res = start < end;
            }

            return res;
        }

        private bool ValidateState(string value)
        {
            State state;
            return Enum.TryParse(value, out state);
        }

        private bool ValidatePriority(string value)
        {
            Priority priority;
            return Enum.TryParse(value, out priority);
        }

        public Wrapper Wrapper { get; set; }
    }
}