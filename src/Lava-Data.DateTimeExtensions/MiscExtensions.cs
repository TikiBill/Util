// Copyright (c) 2018 Bill Adams. All Rights Reserved.
// Bill Adams licenses this file to you under the MIT license.
// See the license.txt file in the project root for more information.

using System;

namespace LavaData.DateTimeExtensions
{
    /// <summary>
    /// Miscellaneous DateTime extensions.
    /// </summary>
    public static class MiscExtensions
    {


        /// <summary>
        /// Zero out the milliseconds so two date/times can be compared as the same without milliseconds.
        /// </summary>
        public static DateTime TrimMilliseconds(this DateTime dt) => new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, 0, dt.Kind);
    }
}
