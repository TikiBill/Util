// Copyright (c) 2018 Bill Adams. All Rights Reserved.
// Bill Adams licenses this file to you under the MIT license.
// See the license.txt file in the project root for more information.

using System;

namespace LavaData.DateTimeExtensions
{
    public static class FormatDatabase
    {

        /// <summary>
        /// An SQL friendly date, NOT QUOTED. Will return null if passed the default DateTime.
        /// 
        /// Note: Using the ToSqlQuotedString on the DateTime struct is probably what you want for actual database operations
        /// since it will put quotes around the string or return NULL for the default (or null for a null nullable DateTime)
        /// </summary>
        public static string SqlDate(this DateTime value) => value.Equals(default(DateTime)) ? null : value.ToString("yyyy-MM-dd");

        public static string SqlDate(this DateTime? value) => value == null || ((DateTime)value).Equals(default(DateTime)) ? null : ((DateTime)value).ToString("yyyy-MM-dd");

        public static string SqlDateTime(this DateTime value) => value.Equals(default(DateTime)) ? null : value.ToString("yyyy-MM-dd HH:mm:ss");

        public static string SqlDateTime(this DateTime? value) => value == null || ((DateTime)value).Equals(default(DateTime)) ? null : ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");

        /// <summary>
        /// To a YYYYMMDD string for an SAP date.
        /// </summary>
        public static string SapDate(this DateTime value) => value.ToString("yyyyMMdd");

        /// <summary>
        /// A long (int64) date. Some databases/system use this for date storage.
        /// </summary>
        public static long LongDate(this DateTime value) => value.Year * 10000 + value.Month * 100 + value.Day;

        /// <summary>
        /// A long (int64) time for the occasional database/system that stores times in an integer column.
        /// </summary>
        public static long LongTime(this DateTime value) => value.Hour * 10000 + value.Minute * 100 + value.Second;

        public static string Iso8601(this DateTime value) => value.ToString("yyyyMMddTHH:mm:ssZ");
    }
}
