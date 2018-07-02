// Copyright (c) 2018 Bill Adams. All Rights Reserved.
// Bill Adams licenses this file to you under the MIT license.
// See the license.txt file in the project root for more information.

using System;
using System.Globalization;

namespace LavaData.DateTimeExtensions
{
    /// <summary>
    /// Helper methods for getting ISO 8601 calendar week.
    /// The ISO 8601 standard for CW 1 is the first week with a Thursday. https://en.wikipedia.org/wiki/ISO_8601
    /// 
    /// Note that Jan 1 can fall into calendar week 52/53 of the previous year since the standard defines
    /// calendar week as the first week with Thursday in it.
    /// 
    /// Anything that is an extension method will end with ISO8601 to indicate the standard used
    /// to calculate the vales. Other static methods will not end with ISO8601 since the class name
    /// in the method call will make it clear, i.e. CalendarWeekISO8601.Method()
    /// </summary>
    public static class CalendarWeekIso8601
    {

        public static readonly DateTimeFormatInfo dateTimeFormatInfo = DateTimeFormatInfo.CurrentInfo;


        // Culture is irrelevant since we specify the start day of the week in our calculations.
        public static readonly Calendar calendar = CultureInfo.InvariantCulture.Calendar;


        /// <summary>
        /// The separator for the year + week string. The ISO standard uses W or -W, however if a
        /// company has a financial calendar that is different than the wall calendar, it may be
        /// of use to use "CW" for calendar week and "FW" for financial week.
        /// </summary>
        public static string CalendarWeekSeparator = "W";

        /// <summary>
        /// A string for the year and calendar week in the format YYYYWww. While many managers
        /// prefer something like CW01 2018 as a label, that manager version does not string-sort well.
        /// So at least when sorting or saving to a database where it might be used to sort, use this format.
        /// 
        /// Returns the empty string if the DateTime is the default.
        ///
        /// Named with ISO8601 since this is an extension method and someone else might have a different
        /// idea of what calendar week a DateTime should be.
        ///  </summary>
        public static string CalendarWeekStringIso8601(this DateTime dt)
        {
            if (dt == default(DateTime))
                return string.Empty;

            var (year, week) = dt.GetIso8601YearWeek();
            if (year == 0 || week == 0)
                return string.Empty;
            else
                return string.Format("{0}{1}{2,2:d2}", year, CalendarWeekSeparator, week);
        }


        /// <summary>
        /// A string for the year and calendar week in the format YYYYCWWW. While many managers
        /// prefer something like CW01 2018 as a label, that manager version does not string-sort well.
        /// So at least when sorting or saving to a database where it might be used to sort, use this format.
        /// 
        /// Returns the empty string if the DateTime is the default or null.
        ///
        /// Named with ISO8601 since this is an extension method and someone else might have a different
        /// idea of what calendar week a DateTime should be.
        ///  </summary>
        public static string CalendarWeekStringISO8601(this DateTime? dt) => dt == null ? string.Empty : CalendarWeekStringIso8601((DateTime)dt);


        /// <summary>
        /// Get the ISO-8601 week of the year, which is slightly different than the .NET week of the year.
        /// 
        /// Presumes Sunday is the first day of the week. Also of note is that this method is
        /// nearly useless since it does not return the year for when the correction is made, e.g.
        /// Dec 31, 2007 should be in 2008W01.
        /// 
        /// Returns zero if passed the default DateTime.
        /// </summary>
        public static int GetIso8601WeekOfYear(this DateTime dt)
        {
            if (dt == default(DateTime))
                return 0;

            // See: https://blogs.msdn.microsoft.com/shawnste/2006/01/24/iso-8601-week-of-year-format-in-microsoft-net/
            //
            // Our version we map any day of the week to Thursday. This will always put Sunday in the week as well
            // since Sunday is zero. And this code makes assumptions about the underlying ENUM. But help us if the ENUM
            // gets changed for some reason. Of course, this assumption should be extra unit tested! (Which it is.)
            dt = dt.AddDays(DayOfWeek.Thursday - calendar.GetDayOfWeek(dt));

            // Return the day of the week of our adjusted DateTime.
            return calendar.GetWeekOfYear(dt, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }


        /// <summary>
        /// Get the ISO-8601 week of the year, which is slightly different than the .NET week of the year.static
        /// 
        /// Presumes Sunday is the first day of the week. Also of note is that this method is
        /// nearly useless since it does not return the year for when the correction is made, e.g.
        /// Dec 31, 2007 should be in 2008W01.
        /// 
        /// Returns zero if passed the default DateTime or null.
        /// </summary>
        public static int GetIso8601WeekOfYear(this DateTime? dt) => dt == null ? 0 : GetIso8601WeekOfYear((DateTime)dt);


        /// <summary>
        /// Get the ISO-8601 week of the year, which is slightly different than the .NET week of the year.
        /// 
        /// Presumes Sunday is the first day of the week. Returns a tuple of year, week.
        /// 
        /// Returns zero, zero if passed the default DateTime.
        /// </summary>
        public static (int, int) GetIso8601YearWeek(this DateTime dt)
        {
            if (dt == default(DateTime))
                return (0, 0);

            // See: https://blogs.msdn.microsoft.com/shawnste/2006/01/24/iso-8601-week-of-year-format-in-microsoft-net/
            //
            // Our version we map any day of the week to Thursday. This will always put Sunday in the week as well
            // since Sunday is zero. And this code makes assumptions about the underlying ENUM. But help us if the ENUM
            // gets changed for some reason. Of course, this assumption should be extra unit tested! (Which it is.)
            dt = dt.AddDays(DayOfWeek.Thursday - calendar.GetDayOfWeek(dt));

            // Return the day of the week of our adjusted DateTime.
            return (dt.Year, calendar.GetWeekOfYear(dt, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday));
        }


        /// <summary>
        /// Get the ISO-8601 week of the year, which is slightly different than the .NET week of the year.static
        /// 
        /// Presumes Sunday is the first day of the week. Also of note is that this method is
        /// nearly useless since it does not return the year for when the correction is made, e.g.
        /// Dec 31, 2007 should be in 2008W01.
        /// 
        /// Returns zero if passed the default DateTime or null.
        /// </summary>
        public static (int, int) GetIso8601YearWeek(this DateTime? dt) => dt == null ? (0, 0) : GetIso8601YearWeek((DateTime)dt);



        /// <summary>
        /// Get a DateTime for the Thursday that falls in the given week of the year. This is
        /// one indexed (i.e. no week zero). If the year and/or the weekOfYear are zero or less,
        /// it will return the default date.
        /// 
        /// Inspired from http://stackoverflow.com/questions/662379/calculate-date-from-week-number
        /// </summary>
        public static DateTime ThursdayForWeekOfYear(int year, int weekOfYear)
        {
            if (year <= 0 || weekOfYear <= 0)
                return default(DateTime);

            var jan1 = new DateTime(year, 1, 1);
            int daysOffset;
            if (jan1.DayOfWeek > DayOfWeek.Thursday)
            {
                // In this case, Jan 1 is after the Thursday of the complete week. As such, to get
                // to the first Thursday of the year, we have to go past the weekend and then to Thursday.
                // Math here is go a week and then back to Thursday.
                daysOffset = 7 - (jan1.DayOfWeek - DayOfWeek.Thursday);
            }
            else
            {
                // Jan 1 is Thursday or before (Sunday - Thursday). We just need to move forward
                // to get to the first Thursday of the year.
                daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;
            }

            // Week of year is 1 indexed but we want to calculate based on zero, so subtract one week.
            return jan1.AddDays(daysOffset + (weekOfYear * 7) - 7);
        }


        /// <summary>
        /// Get a DateTime for the Thursday that falls in the given week of the year, with
        /// a tuple as the argument.
        /// </summary>
        public static DateTime ThursdayForWeekOfYear((int year, int weekOfYear) yearAndWeek) => ThursdayForWeekOfYear(yearAndWeek.year, yearAndWeek.weekOfYear);


        /// <summary>
        /// For the given DateTime, get the Thursday that falls in the same week, with Sunday
        /// as the first day of the week.
        /// </summary>
        public static DateTime ThursdayInWeekOfDate(this DateTime forDate) => ThursdayForWeekOfYear(GetIso8601YearWeek(forDate));


        /// <summary>
        /// Get at DateTime for the Sunday starting the given year and week of the year.
        /// </summary>
        public static DateTime StartDateOfWeek(int year, int weekOfYear) => ThursdayForWeekOfYear(year, weekOfYear).AddDays(-4);


        /// <summary>
        /// Get a DateTime for the Sunday starting the week containing the given date.
        /// 
        /// Will return the default DateTime if passed the default DateTime.
        /// </summary>
        // Note: This relies on the order/value of the ENUM, which we unit test to ensure nothing changed.
        // Also, this might be better in the miscellaneous extension methods, but is here to be next to the year/week version.
        public static DateTime StartDateOfWeek(this DateTime dt) => dt == default(DateTime) ? default(DateTime) : dt.AddDays(-1 * (int)dt.DayOfWeek);


        /// <summary>
        /// Get a DateTime for the Sunday starting the week containing the given date.
        /// 
        /// Will return the default DateTime if passed the default DateTime, and null if passed null.
        /// </summary>
        public static DateTime? StartDateOfWeek(this DateTime? dt)
        {
            if (dt == null)
                return null;

            DateTime unboxedDt = (DateTime)dt; //Unbox once.

            if (unboxedDt == default(DateTime))
                return default(DateTime);

            return unboxedDt.AddDays(-1 * (int)unboxedDt.DayOfWeek);
        }


        /// <summary>
        /// Get a DateTime for the Saturday ending the given date and week of the year.
        /// </summary>
        public static DateTime EndDateOfWeek(int year, int weekOfYear) => ThursdayForWeekOfYear(year, weekOfYear).AddDays(+2);


        /// <summary>
        /// Get a DateTime for the Saturday ending the week containing the given date.
        /// 
        /// Will return the default DateTime if passed the default DateTime.
        /// </summary>
        // Note: This relies on the order/value of the ENUM, which we unit test to ensure nothing changed.
        // Also, this might be better in the miscellaneous extension methods, but is here to be next to the year/week version.
        public static DateTime EndDateOfWeek(this DateTime dt) => dt == default(DateTime) ? default(DateTime) : dt.AddDays(DayOfWeek.Saturday - dt.DayOfWeek);


        /// <summary>
        /// Get a nullable DateTime for the Saturday ending the week containing the given date.
        /// 
        /// Will return the default DateTime if passed the default DateTime, or null if passed null.
        /// </summary>
        public static DateTime? EndDateOfWeek(this DateTime? dt)
        {
            if (dt == null)
                return null;

            DateTime unboxedDt = (DateTime)dt; //Unbox once.

            if (unboxedDt == default(DateTime))
                return default(DateTime);

            return unboxedDt.AddDays(DayOfWeek.Saturday - unboxedDt.DayOfWeek);
        }
    }
}
