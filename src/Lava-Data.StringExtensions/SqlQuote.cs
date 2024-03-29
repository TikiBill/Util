// Copyright (c) 2018 Bill Adams. All Rights Reserved.
// Bill Adams licenses this file to you under the MIT license.
// See the license.txt file in the project root for more information.

using System;

namespace LavaData.StringExtensions
{
    /// <summary>
    /// String and other primitive type extension methods to quote for an SQL database. Strings
    /// are surrounded by single quotes with single quotes inside the passed
    /// string doubled up (to escape them) and nulls and control characters silently removed.false
    /// <br />
    /// Primitive numeric types are not surrounded with quotes.
    /// <br />
    /// If null is passed, then the string NULL is returned.
    /// </summary>

    // Implementation Notes:
    // Originally the extension was on a DbConnection with the idea that the quote method could be
    //   changed if a database called for it. However, any RDBMS/SQL database I've come across can use this
    //   quoting method, and it also presented an engineering problem of how to support all of these methods
    //   for both a vanilla DbConnection and the Lava-Data DbConnectionPlus.
    // Some document databases such as MongoDB take JSON/BSON so the method of getting data into them or generating queries is
    //   going to vary from building up a list of files and values.
    // The "withNPrefix" is a per-function parameter in case one is dealing with two different
    //   DB engines at the same time. That is, having a global parameter to change it would
    //   make multithreading and multi-access very complicated.

    public static class ToSqlQuotedStringMethods
    {

        public static bool KeepTabsNewLinesAndCarriageReturns = true;

        /// <summary>
        /// The default string used to format a DateTime. Intended to be overridden if
        /// your application requires something different. Or simply pass in an
        /// already formatted date/time to the string quote method.
        /// </summary>
        public static string DefaultDateTimeFormat = "yyyy-MM-dd HH:mm:ss";


        /// <summary>
        /// The default format for a GUID.static Intended to be overridden if your application
        /// requires something different
        ///
        /// D = xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx (Default)
        /// B = {xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}
        /// </summary>
        public static string DefaultGuidFormat = "D";

        /// <summary>
        /// ANSI quote a string using single quotes. Will always quote the string regardless as to if
        /// it is needed, e.g. for numbers. Silently removes control characters, most notably null.
        /// By default it will keep tabs, new-lines, and carriage returns. Should your database not
        /// support that, you can change that behavior by setting KeepTabsNewLinesAndCarriageReturns
        /// <br />
        /// Optimized for short strings (less than ~512 characters), and avoids the performance hit
        /// of calling Char.IsControl.
        /// You can find the benchmarks FIXME here.
        /// </summary>
        /// <param name="str">The string to quote.</param>
        /// <returns>ANSI quoted string with control characters removed, or the string NULL if the passed value is null.</returns>
        public static string ToSqlQuotedString(this string str, bool withNPrefix = false)
        {
            if (str == null) return "NULL";

            if (str.Length > 512)
                // Data is expected to rarely have single quotes so short-circuit version is
                // faster for longer strings, although with more GC pressure.
                return ToSqlQuotedString_LongString(str, withNPrefix: withNPrefix);

            var src = str.AsSpan();
            int dest = 0;
            int i = 0;
            char c; // Temp var outside the loop gives a slight performance boost.
            unsafe
            {
                var cPtr = stackalloc char[src.Length * 2 + 2 + (withNPrefix ? 1 : 0)]; //Allocate enough as if the string was all single quotes, plus starting and ending quote.

                if (withNPrefix)
                {
                    cPtr[dest] = 'N';
                    dest += 1;
                }

                cPtr[dest] = '\'';
                dest += 1;
                for (; i < src.Length; ++i)
                {
                    c = src[i];
                    if (c == '\'')
                    {
                        cPtr[dest + 0] = '\'';
                        cPtr[dest + 1] = '\'';
                        dest += 2;
                    }
                    else if (c == '\\'
                        && ((i + 1) == src.Length // Last character is a backslash.
                        || i + 1 < src.Length && src[i + 1] == '\'')) // Next character is a single quote.
                    {
                        // MySQL allows a backslash-quote to escape a single quote (by default).
                        // Perhaps add a static config to disable this?
                        cPtr[dest + 0] = '\\';
                        cPtr[dest + 1] = '\\';
                        dest += 2;
                    }
                    else if (c <= '\x001f' || (c >= '\x007f' && c <= '\x009f'))
                    {
                        if (KeepTabsNewLinesAndCarriageReturns && (c == '\t' || c == '\n' || c == '\r'))
                        {
                            cPtr[dest++] = c;
                        }
                        // Silently Ignore.
                        // Calling IsControl adds significant time (almost doubles) the time to loop.
                    }
                    else
                    {
                        cPtr[dest++] = c;
                    }
                }
                cPtr[dest++] = '\''; //Post increment so dest represents length.
                return new string(cPtr, 0, dest);
            }
        }


        public static string ToSqlQuotedString_LongString(this string str, bool withNPrefix = false)
        {
            if (str == null) return "NULL";

            char c; // Temp var outside the loop gives a slight performance boost.

            var src = str.AsSpan();
            var singleQuotes = 0;
            var controlChars = 0;
            for (int i = 0; i < src.Length; ++i)
            {
                c = str[i];
                if (c == '\'') ++singleQuotes;
                else if (c <= '\x001f' || (c >= '\x007f' && c <= '\x009f')) ++controlChars;
            }

            if (singleQuotes + controlChars == 0)
            {
                //Short-circuit, nothing to alter about the string.
                if (withNPrefix)
                {
                    return "N'" + str + "'";
                }
                else
                {
                    return "'" + str + "'";
                }
            }

            // Single quotes become two quotes, control characters removed, plus start and end quote.
            var bufLen = src.Length + singleQuotes - controlChars + 2 + (withNPrefix ? 1 : 0);
            int dest = 0;
            var newChars = new char[bufLen];

            if (withNPrefix)
            {
                newChars[dest] = 'N';
                dest += 1;
            }

            newChars[dest] = '\'';
            dest += 1;

            for (int i = 0; i < src.Length; ++i)
            {
                c = src[i];
                if (c == '\'')
                {
                    newChars[dest + 0] = '\'';
                    newChars[dest + 1] = '\'';
                    dest += 2;
                }
                else if (c <= '\x001f' || (c >= '\x007f' && c <= '\x009f'))
                {
                    // Calling IsControl adds significant time (almost doubles) the time to loop.
                }
                else
                {
                    newChars[dest++] = c;
                }
            }
            newChars[dest] = '\''; //No post increment because the array length is known, and the increment is effectively a nop performance hit.
            return new string(newChars);
        }

        // Prevent boxing of common primitives, along with handling boxed versions.
        // Numbers do not need to be (and generally should not be) quoted
        // Make sure there is documentation for design decisions, e.g. we pick the format of
        // the date/time to use 'though someone else's design may require something else.

        /// <summary>
        /// Return a 1 (one) or 0 (zero) for a boolean as that is generally how the value gets stored in a database.
        /// </summary>
        public static string ToSqlQuotedString(this Boolean b, bool withNPrefix = false) => b ? "1" : "0";

        /// <summary>
        /// Return a NULL, 1 (one) or 0 (zero) for a nullable (boxed) boolean as that is generally how the value gets stored in a database.
        /// </summary>
        public static string ToSqlQuotedString(this Boolean? b, bool withNPrefix = false) => b == null ? "NULL" : (b == true ? "1" : "0");
        public static string ToSqlQuotedString(this Byte b, bool withNPrefix = false) => b.ToString();
        public static string ToSqlQuotedString(this Byte? b, bool withNPrefix = false) => b == null ? "NULL" : b.ToString();
        public static string ToSqlQuotedString(this SByte s, bool withNPrefix = false) => s.ToString();
        public static string ToSqlQuotedString(this SByte? s, bool withNPrefix = false) => s == null ? "NULL" : s.ToString();
        public static string ToSqlQuotedString(this Int16 i, bool withNPrefix = false) => i.ToString();
        public static string ToSqlQuotedString(this Int16? i, bool withNPrefix = false) => i == null ? "NULL" : i.ToString();
        public static string ToSqlQuotedString(this UInt16 i, bool withNPrefix = false) => i.ToString();
        public static string ToSqlQuotedString(this UInt16? i, bool withNPrefix = false) => i == null ? "NULL" : i.ToString();
        public static string ToSqlQuotedString(this Int32 i, bool withNPrefix = false) => i.ToString();
        public static string ToSqlQuotedString(this Int32? i, bool withNPrefix = false) => i == null ? "NULL" : i.ToString();
        public static string ToSqlQuotedString(this UInt32 i, bool withNPrefix = false) => i.ToString();
        public static string ToSqlQuotedString(this UInt32? i, bool withNPrefix = false) => i == null ? "NULL" : i.ToString();
        public static string ToSqlQuotedString(this Int64 i, bool withNPrefix = false) => i.ToString();
        public static string ToSqlQuotedString(this Int64? i, bool withNPrefix = false) => i == null ? "NULL" : i.ToString();
        public static string ToSqlQuotedString(this UInt64 i, bool withNPrefix = false) => i.ToString();
        public static string ToSqlQuotedString(this UInt64? i, bool withNPrefix = false) => i == null ? "NULL" : i.ToString();
        public static string ToSqlQuotedString(this Char c, bool withNPrefix = false) => ToSqlQuotedString(c.ToString(), withNPrefix);
        public static string ToSqlQuotedString(this Char? c, bool withNPrefix = false) => c == null ? "NULL" : ToSqlQuotedString(c.ToString(), withNPrefix);
        public static string ToSqlQuotedString(this Single f, bool withNPrefix = false) => f.ToString();
        public static string ToSqlQuotedString(this Single? f, bool withNPrefix = false) => f == null ? "NULL" : f.ToString();
        public static string ToSqlQuotedString(this Double d, bool withNPrefix = false) => d.ToString();
        public static string ToSqlQuotedString(this Double? d, bool withNPrefix = false) => d == null ? "NULL" : d.ToString();


        /// <summary>
        /// Return a quoted date-time string using the string in DefaultDateTimeFormat.
        ///
        /// Note: a default DateTime value is considered, and returned as, NULL.
        /// </summary>
        public static string ToSqlQuotedString(this DateTime dt, bool withNPrefix = false) =>
            dt.Equals(default(DateTime)) ? "NULL" : ToSqlQuotedString(dt.ToString(DefaultDateTimeFormat));

        /// <summary>
        /// Return a quoted date-time string using the format string in DefaultDateTimeFormat.
        ///
        /// Note: a default DateTime value is considered, and returned as, NULL
        /// since it is not a valid date, and any database which validates a date
        /// is valid will throw an error.
        /// </summary>
        public static string ToSqlQuotedString(this DateTime? dt, bool withNPrefix = false) =>
            dt == null || dt.Equals(default(DateTime)) ? "NULL" : ToSqlQuotedString(((DateTime)dt).ToString(DefaultDateTimeFormat));

        /// <summary>
        /// Return a quoted Guid string using the format string in DefaultGuidFormat.
        ///
        /// Note: a default Guid value is returned as a quoted value (and not NULL).
        /// </summary>
        public static string ToSqlQuotedString(this Guid guid, bool withNPrefix = false)
            => ToSqlQuotedString(guid.ToString(DefaultGuidFormat));

        /// <summary>
        /// Return a quoted Guid string using the format string in DefaultGuidFormat.
        ///
        /// Note: a default Guid value is returned as a quoted value (and not NULL).
        /// </summary>
        public static string ToSqlQuotedString(this Guid? guid, bool withNPrefix = false) =>
            guid is null ? "NULL" : ToSqlQuotedString(((Guid)guid).ToString(DefaultGuidFormat));


        /// <summary>
        /// Return a string that is quoted (or not) depending on the object type.
        /// I.e. numbers do not (and should not) be quoted, but everything else should be.
        /// Loosely calls ToString() on the object, i.e. it will not work with complex types.
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="obj"></param>
        /// <param name="withNPrefix">Indicator to start strings with the SQL Server "N" to indicate unicode.</param>
        /// <returns>A safe string to use in a SQL statement, NULL if the object is null.</returns>
        public static string ToSqlQuotedString(this object obj, bool withNPrefix = false)
        {
            if (obj == null)
            {
                // This will catch nullable/boxed value types as well.
                return "NULL";
            }

            // Console.WriteLine("Object is a " + obj.GetType().ToString());

            switch (obj)
            {
                // The most common case first.
                case String s: return ToSqlQuotedString(s, withNPrefix: withNPrefix);
                case Char c: return ToSqlQuotedString(c, withNPrefix: withNPrefix);
                case Int16 i: return ToSqlQuotedString(i, withNPrefix: withNPrefix);
                case UInt16 i: return ToSqlQuotedString(i, withNPrefix: withNPrefix);
                case Int32 i: return ToSqlQuotedString(i, withNPrefix: withNPrefix);
                case UInt32 i: return ToSqlQuotedString(i, withNPrefix: withNPrefix);
                case Int64 i: return ToSqlQuotedString(i, withNPrefix: withNPrefix);
                case UInt64 i: return ToSqlQuotedString(i, withNPrefix: withNPrefix);
                case Single s: return ToSqlQuotedString(s, withNPrefix: withNPrefix);
                case Double d: return ToSqlQuotedString(d, withNPrefix: withNPrefix);
                case DateTime dt: return ToSqlQuotedString(dt, withNPrefix: withNPrefix);
                case Boolean b: return ToSqlQuotedString(b, withNPrefix: withNPrefix);
                case Guid guid: return ToSqlQuotedString(guid, withNPrefix: withNPrefix);

                default:
                    return ToSqlQuotedString(obj.ToString(), withNPrefix: withNPrefix);
            }
        }
    }
}
