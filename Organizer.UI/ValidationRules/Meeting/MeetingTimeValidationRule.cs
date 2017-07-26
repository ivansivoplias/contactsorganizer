using Organizer.UI.Helpers;
using System;
using System.Globalization;
using System.Windows.Controls;

namespace Organizer.UI.ValidationRules
{
    public class MeetingTimeValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var timeString = value?.ToString();

            var meetingDateString = MeetingDateWrapper.WrappedData?.ToString();

            if (!string.IsNullOrEmpty(timeString) && !string.IsNullOrEmpty(meetingDateString))
            {
                DateTime meetingDate;

                TimeSpan time;

                if (DateTime.TryParse(meetingDateString, out meetingDate)
                    && meetingDate.Date == DateTime.Today
                    && TimeSpan.TryParseExact(timeString, @"hh\:mm", null, out time)
                    && time <= DateTime.Now.TimeOfDay)
                {
                    return new ValidationResult(false, "Meeting time cannot be less than or equal to current time.");
                }
            }

            return ValidationResult.ValidResult;
        }

        public Wrapper MeetingDateWrapper { get; set; }
    }
}