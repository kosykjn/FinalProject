using System;
using System.Globalization;

namespace Calendar.Extensions
{
    public static class DateTimeExtensions
    {
        public static int GetDayOfWeekNumber(this DateTime dateTime)
        {
            var value = ((int) dateTime.DayOfWeek) - 1;
            return value < 0 ? 6 : value;
        }

        public static int GetWeekNumberOfMonth(this DateTime dateTime)
        {
            var beginningOfMonth = new DateTime(dateTime.Year, dateTime.Month, 1);

            while (dateTime.Date.AddDays(1).DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
                dateTime = dateTime.AddDays(1);

            return (int)Math.Truncate((double)dateTime.Subtract(beginningOfMonth).TotalDays / 7f);
        }
    }
}
