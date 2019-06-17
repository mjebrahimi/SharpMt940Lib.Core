using System;
using System.Globalization;

namespace Raptorious.SharpMt940Lib
{
    /// <summary>
    /// Responsible for parsing input to a specific date time.
    /// </summary>
    public static class DateParser
    {
        /// <summary>
        /// Parses the given string parts into a datetime object.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="cultureInfo">The culture to use</param>
        /// <returns></returns>
        public static DateTime ParseDate(string year, string month, string day, CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace(year))
            {
                throw new ArgumentException("year can not be empty", nameof(year));
            }

            if (string.IsNullOrWhiteSpace(month))
            {
                throw new ArgumentException("month can not be empty", nameof(month));
            }

            if (string.IsNullOrWhiteSpace(day))
            {
                throw new ArgumentException("day can not be empty", nameof(day));
            }

            if (cultureInfo == null)
            {
                throw new ArgumentNullException(nameof(cultureInfo));
            }

            var parsedFourDigitYear = cultureInfo.Calendar.ToFourDigitYear(ValueConverter.ParseInteger(year, cultureInfo));
            var parsedMonth = ValueConverter.ParseInteger(month, cultureInfo);
            var parsedDay = ValueConverter.ParseInteger(day, cultureInfo);

            return new DateTime(parsedFourDigitYear, parsedMonth, parsedDay);
        }
    }
}
