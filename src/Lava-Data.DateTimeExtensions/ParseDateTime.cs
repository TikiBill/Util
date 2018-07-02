// Copyright (c) 2018 Bill Adams. All Rights Reserved.
// Bill Adams licenses this file to you under the MIT license.
// See the license.txt file in the project root for more information.

using System;

namespace LavaData.DateTimeExtensions
{
    public static class ParseDateTime
    {

        /// <summary>
        /// Go from a integer/long date and integer/long time back to a DateTime. Some old systems (e.g. Workstream) store dates and times
        /// in integer fields.
        /// </summary>
        /// <param name="date">Int64 (or Int32) of the date.</param>
        /// <param name="time">Int64 (or Int32) of the time.</param>
        /// <param name="resultDateTime">The return value of the DateTime. Default DateTime if conversion failed.</param>
        /// <returns>True if the values could be converted to a date/time.</returns>
        public static bool TryToDateTime(long date, long time, out DateTime resultDateTime)
        {
            try
            {
                resultDateTime = new DateTime(
                    (int)(date / 10000),
                    (int)((date / 100) % 100),
                    (int)(date % 100),
                    (int)(time / 10000),
                    (int)((time / 100) % 100),
                    (int)(time % 100)
                );
                return true;
            }
            catch
            {
                resultDateTime = default(DateTime);
                return false;
            }
        }


        /// <summary>
        /// Try to convert a string to a DateTime. A little different than the built-in parser in that
        /// we want to allow relative days -- e.g. "5" to return a DateTime from 5 days ago. This is
        /// useful for query forms when, for example, someone wants events from 5 days ago it saves her
        /// from having to do the math in her head.
        /// 
        /// If the string is null/empty, or otherwise cannot be parsed by DateTime, then
        /// the default DateTime put in the returnDateTime and the method returns false.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="returnDateTime"></param>
        /// <returns>True if the string could be converted to a DateTime.</returns>
        public static bool TryToDateTime(this string value, out DateTime returnDateTime)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                returnDateTime = default(DateTime);
                return false;
            }

            int daysAgo;
            if (Int32.TryParse(value, out daysAgo))
            {
                returnDateTime = DateTime.Now.AddDays(-1 * daysAgo);
                return true;
            }

            return DateTime.TryParse(value, out returnDateTime);
        }


        public static bool TryToDateTime(this string value, out DateTime? returnDateTime)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                returnDateTime = null;
                return true;
            }


            DateTime parsedValue; // Non-nullable.
            var success = TryToDateTime(value, out parsedValue);
            if (success)
                returnDateTime = parsedValue;
            else
                returnDateTime = null;  // Nullable so return null since it did not work.

            return success;
        }
    }
}
