// Copyright (c) 2018 Bill Adams. All Rights Reserved.
// Bill Adams licenses this file to you under the MIT license.
// See the license.txt file in the project root for more information.

using System;

namespace LavaData.DateTimeExtensions
{
    /// <summary>
    /// Methods (mostly extension methods) to convert to and from UNIX time.
    /// </summary>
    public static class UnixTime
    {

        /// <summary>
        /// The beginning of time, according to UNIX, 1970-01-01 00:00:00 +0000 (UTC).
        /// </summary>
        public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);


        /// <summary>
        /// Convert a DateTime into a UNIX time (the number of seconds since midnight on Jan 1, 1970).
        /// This would be the same value returned by time(2).
        /// </summary>
        public static long ToUnixTime(this DateTime value) => (long)(value.ToUniversalTime().Subtract(UnixEpoch)).TotalSeconds;


        /// <summary>
        /// Convert a DateTime into a milliseconds time (the number of milliseconds since midnight on Jan 1, 1970), such as
        /// the timeval struct returned by gettimeofday.
        /// 
        /// The primary use of this extension method is for opportunistic locking database updates, where all of the
        /// servers making updates are synced to a single clock.
        /// </summary>
        public static long ToUnixTimeMilliseconds(this DateTime value) => (long)(value.ToUniversalTime().Subtract(UnixEpoch)).TotalMilliseconds;


        /// <summary>
        /// Convert a nullable DateTime into a UNIX time (the number of seconds since midnight on Jan 1, 1970).
        /// Returns 0 when the DateTime is null.
        /// </summary>
        public static long ToUnixTime(this DateTime? value) => value == null ? 0 : (long)(((DateTime)value).ToUniversalTime().Subtract(UnixEpoch)).TotalSeconds;


        /// <summary>
        /// Convert a DateTime into a milliseconds time (the number of milliseconds since midnight on Jan 1, 1970), such as
        /// the timeval struct returned by gettimeofday.
        /// 
        /// The primary use of this extension method is for opportunistic locking database updates, where all of the
        /// servers making updates are synced to a single clock.
        ///
        /// Returns 0 when the DateTime is null.
        /// </summary>
        public static long ToUnixTimeMilliseconds(this DateTime? value) => value == null ? 0 : (long)(((DateTime)value).ToUniversalTime().Subtract(UnixEpoch)).TotalMilliseconds;


        /// <summary>
        /// The current UNIX time (the number of seconds since midnight on Jan 1, 1970).
        /// </summary>
        public static long CurrentUnixTime() => (long)(DateTime.UtcNow.Subtract(UnixEpoch)).TotalSeconds;


        /// <summary>
        /// The current UNIX time in milliseconds (the number of milliseconds since midnight on Jan 1, 1970).
        ///
        /// The primary use of this method is for opportunistic locking database updates, where all of the
        /// servers making updates are synced to a single clock.
        /// </summary>
        public static long CurrentUnixTimeMilliseconds() => (long)(DateTime.UtcNow.Subtract(UnixEpoch)).TotalMilliseconds;


        /// <summary>
        /// The current UNIX time. The datetime used is ignored and only a placeholder so this can be used
        /// as an extension method.
        /// </summary>
        /// <param name="ignored">DateTime which is ignored.</param>
        public static long CurrentUnixTime(this DateTime ignored) => (long)(DateTime.UtcNow.Subtract(UnixEpoch)).TotalSeconds;


        /// <summary>
        /// The current UNIX time in milliseconds (the number of milliseconds since midnight on Jan 1, 1970), such as
        /// the timeval struct returned by gettimeofday.
        /// 
        /// The datetime used is ignored and only a placeholder so this can be used as an extension method.
        ///
        /// The primary use of this method is for opportunistic locking database updates, where all of the
        /// servers making updates are synced to a single clock.
        /// </summary>
        /// <param name="ignored">DateTime which is ignored.</param>
        public static long CurrentUnixTimeMilliseconds(this DateTime ignored) => (long)(DateTime.UtcNow.Subtract(UnixEpoch)).TotalMilliseconds;


        /// <summary>
        /// The current UNIX time. The datetime used is ignored and only a placeholder so this can be used
        /// as an extension method.
        /// </summary>
        /// <param name="ignored">DateTime which is ignored.</param>
        public static long CurrentUnixTime(this DateTime? ignored) => (long)(DateTime.UtcNow.Subtract(UnixEpoch)).TotalSeconds;


        /// The current UNIX time in milliseconds (the number of milliseconds since midnight on Jan 1, 1970), such as
        /// the timeval struct returned by gettimeofday.
        /// 
        /// The datetime used is ignored and only a placeholder so this can be used as an extension method.
        ///
        /// The primary use of this method is for opportunistic locking database updates, where all of the
        /// servers making updates are synced to a single clock.
        /// </summary>
        /// <param name="ignored">DateTime which is ignored.</param>
        public static long CurrentUnixTimeMilliseconds(this DateTime? ignored) => (long)(DateTime.UtcNow.Subtract(UnixEpoch)).TotalMilliseconds;


        /// <summary>
        /// Don't use Int32 to save a UNIX time, unless you plan on being retired
        /// by Mon Jan 18 19:14:07 2038 UTC and otherwise hate the people after you.
        /// </summary>
        public static DateTime UnixTimeToUtcDateTime(this int ts) => UnixEpoch.AddSeconds((double)ts);


        /// <summary>
        /// Don't use Int32 to save a UNIX time, unless you plan on being retired
        /// by Mon Jan 18 19:14:07 2038 UTC and otherwise hate the people after you.
        /// </summary>
        public static DateTime UnixTimeToUtcDateTime(this int? ts) => ts == null ? default(DateTime) : UnixEpoch.AddSeconds((double)ts);


        /// <summary>
        /// While a UInt32 will buy you a lot of time (Sat Feb  6 22:28:15 2106 UTC), it is not really
        /// portable to every other language (e.g. Java). Just use an Int64 for UNIX times.
        /// </summary>
        public static DateTime UnixTimeToUtcDateTime(this uint ts) => UnixEpoch.AddSeconds((double)ts);


        /// <summary>
        /// While a UInt32 will buy you a lot of time (Sat Feb  6 22:28:15 2106 UTC), it is not really
        /// portable to every other language (e.g. Java). Just use an Int64 for UNIX times.
        /// </summary>
        public static DateTime UnixTimeToUtcDateTime(this uint? ts) => ts == null ? default(DateTime) : UnixEpoch.AddSeconds((double)ts);


        /// <summary>
        /// Convert an Int64 UNIX time into a UTC DateTime.
        /// </summary>
        public static DateTime UnixTimeToUtcDateTime(this long ts) => UnixEpoch.AddSeconds((double)ts);


        /// <summary>
        /// Convert an Int64 UNIX milliseconds time into a UTC DateTime.
        /// </summary>
        public static DateTime UnixTimeMillisecondsToUtcDateTime(this long ts) => UnixEpoch.AddMilliseconds((double)ts);


        /// <summary>
        /// Convert a nullable Int64 UNIX time into a UTC DateTime. Will return the default DateTime
        /// if the value passed is null.
        /// </summary>
        public static DateTime UnixTimeToUtcDateTime(this long? ts) => ts == null ? default(DateTime) : UnixEpoch.AddSeconds((double)ts);


        /// <summary>
        /// Convert a nullable Int64 UNIX milliseconds time into a UTC DateTime. Will return the default DateTime
        /// if the value passed is null.
        /// </summary>
        public static DateTime UnixTimeMillisecondsToUtcDateTime(this long? ts) => ts == null ? default(DateTime) : UnixEpoch.AddSeconds((double)ts);


        /// <summary>
        /// Convert a UInt64 UNIX time into a UTC DateTime. It is recommended against using
        /// a UInt64 for UNIX times since it is not portable to many other languages (e.g. Java).
        /// </summary>
        public static DateTime UnixTimeToUtcDateTime(this ulong ts) => UnixEpoch.AddSeconds((double)ts);


        /// <summary>
        /// Convert a nullable UInt64 UNIX time into a UTC DateTime. It is recommended against using
        /// a UInt64 for UNIX times since it is not portable to many other languages (e.g. Java).
        /// </summary>
        public static DateTime UnixTimeToUtcDateTime(this ulong? ts) => ts == null ? default(DateTime) : UnixEpoch.AddSeconds((double)ts);


        /// <summary>
        /// Convert a 64-bit float UNIX time into a UTC DateTime.
        /// </summary>
        public static DateTime UnixTimeToUtcDateTime(this double ts) => UnixEpoch.AddSeconds(ts);


        /// <summary>
        /// Convert a nullable 64-bit float UNIX time into a UTC DateTime. Returns the default DateTime
        /// when passed null.
        /// </summary>
        public static DateTime UnixTimeToUtcDateTime(this double? ts) => ts == null ? default(DateTime) : UnixEpoch.AddSeconds((double)ts);

    }
}
