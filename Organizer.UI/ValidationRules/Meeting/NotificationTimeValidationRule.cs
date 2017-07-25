﻿using Organizer.Common.Enums;
using Organizer.UI.Helpers;
using System;
using System.Globalization;
using System.Windows.Controls;

namespace Organizer.UI.ValidationRules
{
    public class NotificationTimeValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var notificationTimeString = value?.ToString();

            var meetingTimeString = MeetingTimeWrapper.WrappedData?.ToString();

            var notificationDateString = NotificationDateWrapper.WrappedData?.ToString();

            var meetingDateString = MeetingDateWrapper.WrappedData?.ToString();

            var meetingTypeString = MeetingTypeWrapper.WrappedData?.ToString();

            if (!string.IsNullOrEmpty(notificationDateString) && !string.IsNullOrEmpty(meetingDateString)
                && !string.IsNullOrEmpty(meetingTypeString))
            {
                DateTime meetingDate, notificationDate;

                meetingDate = DateTime.Parse(meetingDateString);

                notificationDate = DateTime.Parse(notificationDateString);

                MeetingType meetingType;

                bool isMeetingTypeParsed = Enum.TryParse(meetingTypeString, out meetingType);

                if (string.IsNullOrEmpty(notificationTimeString))
                {
                    if (!string.IsNullOrEmpty(meetingTimeString))
                    {
                        return new ValidationResult(false, "Meeting time is not selected.");
                    }
                    return ValidationResult.ValidResult;
                }

                string timeFormat = @"hh\:mm";

                TimeSpan notificationTime, meetingTime;

                if (TimeSpan.TryParseExact(notificationTimeString, timeFormat, null, out notificationTime)
                    && TimeSpan.TryParseExact(meetingTimeString, timeFormat, null, out meetingTime))
                {
                    if (isMeetingTypeParsed && meetingType == MeetingType.OneTime)
                    {
                        if (notificationDate < meetingDate)
                        {
                            return ValidationResult.ValidResult;
                        }

                        if (notificationDate == meetingDate && notificationTime >= meetingTime)
                        {
                            return new ValidationResult(false, "Notification time cannot be greater or equal to meeting time.");
                        }
                    }
                    else if (notificationTime >= meetingTime)
                    {
                        return new ValidationResult(false, "Notification time cannot be greater or equal to meeting time.");
                    }
                }
            }

            return ValidationResult.ValidResult;
        }

        public Wrapper MeetingTimeWrapper { get; set; }

        public Wrapper NotificationDateWrapper { get; set; }

        public Wrapper MeetingDateWrapper { get; set; }

        public Wrapper MeetingTypeWrapper { get; set; }
    }
}