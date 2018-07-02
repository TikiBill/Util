// Copyright (c) 2018 Bill Adams. All Rights Reserved.
// Bill Adams licenses this file to you under the MIT license.
// See the license.txt file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;

namespace LavaData.DateTimeExtensions
{
    public static class FormatHuman
    {

        /// <summary>
        /// A nicely short-formatted date for humans, in dd-MMM-yyyy, e.g. 01-Jan-2018.
        /// 
        /// The formatting string does change with the culture, however, it can produce strange results in some
        /// cases. For example, the French version of Wed, 11-Apr-2018 comes back as mer., 11-avr.-2018.
        /// </summary>
        public static string HumanDate(this DateTime value) => value == default(DateTime) ? string.Empty : value.ToString("dd-MMM-yyyy");


        /// <summary>
        /// A nicely short-formatted date for humans, in dd-MMM-yyyy, e.g. 01-Jan-2018. Empty string if the DateTime is null or the default.
        /// 
        /// The formatting string does change with the culture, however, it can produce strange results in some
        /// cases. For example, the French version of Wed, 11-Apr-2018 comes back as mer., 11-avr.-2018.
        /// </summary>
        public static string HumanDate(this DateTime? value)
        {
            if (value == null)
                return string.Empty;

            DateTime dt = (DateTime)value; // Cast/un-box once for performance.

            if (dt.Equals(default(DateTime)))
                return string.Empty;

            return dt.ToString("dd-MMM-yyyy");
        }


        /// <summary>
        /// A nicely short-formatted date for humans, in ddd, dd-MMM-yyyy, e.g. Mon, 01-Jan-2018. Empty string if DateTime is default.
        /// 
        /// The formatting string does change with the culture, however, it can produce strange results in some
        /// cases. For example, the French version of Wed, 11-Apr-2018 comes back as mer., 11-avr.-2018.
        /// </summary>
        public static string HumanDateWithDowUs(this DateTime value) => value == default(DateTime) ? string.Empty : value.ToString("ddd, dd-MMM-yyyy");


        /// <summary>
        /// A nicely short-formatted date for humans, in ddd, dd-MMM-yyyy, e.g. Mon, 01-Jan-2018. Empty string if the DateTime is null or the default.
        /// 
        /// The formatting string does change with the culture, however, it can produce strange results in some
        /// cases. For example, the French version of Wed, 11-Apr-2018 comes back as mer., 11-avr.-2018.
        /// </summary>
        public static string HumanDateWithDowUs(this DateTime? value)
        {
            if (value == null)
                return string.Empty;

            DateTime dt = (DateTime)value; // Cast/un-box once for performance.

            if (dt.Equals(default(DateTime)))
                return string.Empty;

            return dt.ToString("ddd, dd-MMM-yyyy");
        }

        public static string UnixTimeToHumanDeltaTime(long ts, int precision = 2)
        {
            if (ts <= 0)
            {
                // Invalid time, or the beginning of UNIX time.
                return "Never";
            }
            return DeltaSecondsToHumanDeltaTime(UnixTime.CurrentUnixTime() - ts, precision);
        }

        /// <summary>
        /// A human friendly time-span string like 1 day, 5 hours.
        /// 
        /// The precision determines how many time measurements in the string. Generally
        /// one would not want to go all the way down the line like 1 year, 3 weeks, 2 days, 5 hours, 4 minutes, 3 seconds.
        /// With the default of two, the returned string for this example would be 1 year, 3 weeks.
        /// 
        /// TODO: Currently only English. Start adding other cultures.
        /// </summary>
        /// <param name="deltaSeconds">The number of seconds that have passed.</param>
        /// <param name="precision">How many time units.</param>
        /// <returns>A string for human consumption.</returns>
        public static string DeltaSecondsToHumanDeltaTime(long deltaSeconds, int precision = 2)
        {

            List<string> text = new List<string>();

            var times = new YearTimeSpan(deltaSeconds);

            if (text.Count < precision && times.Years > 0)
                text.Add(times.Years + " Year" + (times.Years > 1 ? "s" : ""));

            if (text.Count < precision && times.Weeks > 0)
                text.Add(times.Weeks + " Week" + (times.Weeks > 1 ? "s" : ""));

            if (text.Count < precision && times.Days > 0)
                text.Add(times.Days + " Day" + (times.Days > 1 ? "s" : ""));

            if (text.Count < precision && times.Hours > 0)
                text.Add(times.Hours + " Hour" + (times.Hours > 1 ? "s" : ""));

            if (text.Count < precision && times.Minutes > 0)
                text.Add(times.Minutes + " Minute" + (times.Minutes > 1 ? "s" : ""));

            if (text.Count < precision && times.Seconds > 0)
                text.Add(times.Seconds + " Second" + (times.Seconds > 1 ? "s" : ""));

            if (text.Count > 0)
                return String.Join(", ", text);
            else
                return "Right Now";
        }
    }
}
