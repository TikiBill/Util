// Copyright (c) 2018 Bill Adams. All Rights Reserved.
// Bill Adams licenses this file to you under the MIT license.
// See the license.txt file in the project root for more information.

using System;

namespace LavaData.DateTimeExtensions
{
    /// <summary>
    /// Some convenience methods.
    /// 
    /// Like many of the formatting methods, these are especially useful for razor/blazor pages.
    /// </summary>
    public static class DateTimeCalc
    {

        public static long AgeMinutes(this DateTime value) => (long)DateTime.Now.Subtract(value).TotalMinutes;

        public static long AgeMinutes(this DateTime? value) => value == null ? 0 : (long)DateTime.Now.Subtract((DateTime)value).TotalMinutes;

        public static long? NullableAgeMinutes(this DateTime? value) => value == null || value == default(DateTime) ? null : (long?)DateTime.Now.Subtract((DateTime)value).TotalMinutes;

        public static DateTime DateTimeDaysAgo(this int daysBack) => DateTime.Now.AddDays(-1 * daysBack);

        public static DateTime DateTimeDaysAgo(this int? daysBack) => daysBack == null ? default(DateTime) : DateTime.Now.AddDays(-1 * (int)daysBack);

        public static DateTime? NullableDateTimeDaysAgo(this int? daysBack) => daysBack == null ? (DateTime?)null : DateTime.Now.AddDays(-1 * (int)daysBack);
    }
}
