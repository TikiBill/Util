// Copyright (c) 2018 Bill Adams. All Rights Reserved.
// Bill Adams licenses this file to you under the MIT license.
// See the license.txt file in the project root for more information.

using System;

namespace LavaData.StringExtensions
{
    public static class Analysis
    {

        /// <summary>
        /// Check if a string looks like an integer with optional leading plus or minus sign. Ignores leading and trailing spaces.
        /// </summary>
        /// <param name="checkString">The string to inspect.</param>
        /// <param name="allowDotZeros">When set to true, will still return to true if there is a decimal and only zeros following the decimal.</param>
        /// <returns></returns>
        public static bool LooksLikeAnInteger(this string checkString, bool allowDotZeros = false)
        {
            if (checkString == null || checkString == string.Empty)
                return false;

            var chars = checkString.AsSpan().Trim();
            if (chars.Length == 0)
                return false;

            // A 64-bit signed integer is max 9,223,372,036,854,775,807, so accounting
            //   for an optional sign, the max string length is 20. If we are allowing
            //   trailing dot zeros, then skip the test.
            if (chars.Length > 20 && !allowDotZeros)
                return false;


            // We allow the first character to be a + or - sign, or a digit.
            if (!(chars[0] == '-' || chars[0] == '+' || char.IsDigit(chars[0])))
                return false;

            for (int i = 1; i < chars.Length; ++i)
            {
                if (!char.IsDigit(chars[i]))
                {
                    if (allowDotZeros && chars[i] == '.' && chars.Length > i + 1)
                    {
                        for (int j = i + 1; j < chars.Length; ++j)
                        {
                            if (chars[j] != '0')
                                return false;
                        }

                        //Only looks like an int if it still passes the length test.
                        return i <= 20 ? true : false;
                    }
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// Check if a string looks like a decimal number. Does not convert nor check range. Allows
        /// a leading or trailing period, e.g. .100  100. 100.0  0.100  are all valid decimals. Ignores
        /// leading/trailing spaces.
        /// </summary>
        /// <param name="checkString">String to check.</param>
        public static bool LooksLikeADecimal(this string checkString)
        {
            if (checkString == null || checkString == string.Empty)
                return false;

            var chars = checkString.AsSpan().Trim();
            if (chars.Length == 0)
                return false;

            // We allow the first character to be a ., + or - sign, or a digit.            
            if (!(chars[0] == '-' || chars[0] == '+' || chars[0] == '.' || char.IsDigit(chars[0])))
                return false;

            for (int i = 1; i < chars.Length; ++i)
            {
                if (!char.IsDigit(chars[i]))
                {
                    if (chars[i] == '.' && chars[0] != '.')
                    {  // Double period not allowed, otherwise e.g. .100. would count.
                        for (int j = i + 1; j < chars.Length; ++j)
                        {
                            if (!char.IsDigit(chars[j]))
                                return false;
                        }
                        return true;
                    }
                    return false;
                }
            }
            return true;
        }
    }
}
