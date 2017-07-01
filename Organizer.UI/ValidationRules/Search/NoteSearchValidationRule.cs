using Organizer.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Globalization;
using Organizer.Common.Enums.SearchTypes;

namespace Organizer.UI.ValidationRules
{
    public class NoteSearchValidationRule : ValidationRule
    {
        public Wrapper Wrapper { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var stringValue = value as string;

            DiarySearchType wrappedEnum;

            if (Enum.TryParse(Wrapper.WrappedData.ToString(), out wrappedEnum))
            {
                if (string.IsNullOrEmpty(stringValue) && wrappedEnum != DiarySearchType.Default)
                {
                    return new ValidationResult(false, "Search value is empty.");
                }

                switch (wrappedEnum)
                {
                    case DiarySearchType.ByCaptionLike:
                        if (string.IsNullOrEmpty(stringValue))
                            return new ValidationResult(false, "Caption is empty.");
                        break;

                    case DiarySearchType.CreatedBetween:
                        if (!ValidateCreatedBetween(stringValue))
                        {
                            return new ValidationResult(false, "Date range is invalid. Please input right date range.");
                        }
                        break;
                }
            }

            return ValidationResult.ValidResult;
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
    }
}