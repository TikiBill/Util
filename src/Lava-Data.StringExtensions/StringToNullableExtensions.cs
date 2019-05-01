// Copyright (c) 2018 Bill Adams. All Rights Reserved.
// Bill Adams licenses this file to you under the MIT license.
// See the license.txt file in the project root for more information.

using System;

namespace LavaData.StringExtensions
{
    /// <summary>
    /// Wrappers around various x.Parse() methods that will return
    /// null if the string.IsNullOrWhiteSpace() returns null.
    /// </summary>
    public static class StringToNullableExtensions
    {
        public static DateTime? ToNullableDateTime(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return (DateTime?)null;
            }
            else
            {
                return DateTime.Parse(str);
            }
        }
    }
}
