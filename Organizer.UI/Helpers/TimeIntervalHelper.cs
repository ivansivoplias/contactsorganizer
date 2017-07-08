using System;
using System.Collections.Generic;

namespace Organizer.UI.Helpers
{
    public static class TimeIntervalHelper
    {
        private static List<TimeSpan> _cachedIntervals;

        public static ICollection<TimeSpan> GetTimeIntervals()
        {
            if (_cachedIntervals == null)
            {
                var start = new TimeSpan(0, 0, 0);

                var step = 15;

                var count = 1;

                TimeSpan current = start;

                _cachedIntervals = new List<TimeSpan>();

                _cachedIntervals.Add(start);

                while (current.Days != 1)
                {
                    current = start.Add(TimeSpan.FromMinutes(step * count));
                    _cachedIntervals.Add(current);
                    count++;
                }
            }

            return _cachedIntervals;
        }
    }
}