// Copyright (c) 2018 Bill Adams. All Rights Reserved.
// Bill Adams licenses this file to you under the MIT license.
// See the license.txt file in the project root for more information.

using System;

namespace LavaData.StringExtensions
{
    /// <summary>
    /// String extension methods to change the capitalization. The intent is mostly for handling the different
    /// styles of naming variables and database table fields. For example, a c# property may be named FirstName
    /// but in python it would be first_name, JavaScript it would be firstName, and an HTML id/name would be first-name.
    /// 
    /// You can override the word-boundary style by passing your own function to each method.
    /// </summary>
    public static class ChangeStringCapitalization
    {

        /// <summary>
        /// Change the capitalization of a string.
        /// </summary>
        /// <param name="str">The string to change.</param>
        /// <param name="toCase">The capitalization style. <see cref="StringCapitalizationEnum"> for options.</param>
        /// <param name="wordBoundaryTest">Optional method to determine word boundaries.</param>
        /// <returns>The new string.</returns>
        public static string ChangeCapitalization(this string str, StringCapitalization toCase, IsNewWordBoundary wordBoundaryTest = null)
        {
            switch (toCase)
            {
                case StringCapitalization.KeepCase:
                    return str;
                case StringCapitalization.UpperCase:
                    return str.ToUpper();
                case StringCapitalization.LowerCase:
                    return str.ToLower();
                case StringCapitalization.PascalCase:
                    return str.ToPascalCase(wordBoundaryTest: wordBoundaryTest);
                case StringCapitalization.CamelCase:
                    return str.ToCamelCase(wordBoundaryTest: wordBoundaryTest);
                case StringCapitalization.SnakeCase:
                    return str.ToSnakeCase(wordBoundaryTest: wordBoundaryTest);
                case StringCapitalization.HtmlIdCase:
                    return str.ToHtmlIdCase(wordBoundaryTest: wordBoundaryTest);
                default:
                    //Unreachable with the enum unless someone changes it without changing this switch statement.
                    throw new ArgumentException("INTERNAL ERROR: StringCapitalization has ENUM value not handled in the switch!");
            }
        }

        /// <summary>
        /// Delegate for determining when a character is at a world boundary.
        /// 
        /// NOTE: Cannot use Func<ReadOnlySpan<char>, int, bool> because ReadOnlySpan is a stack-only
        /// (by-ref like) type and cannot be used as a generic type as that would box it.
        /// https://github.com/dotnet/corefx/issues/25669
        /// https://github.com/VSadov/csharplang/blob/ef68acb505ad1a3310de133ef7af65c2c24da520/proposals/span-safety.md#ref-like-types-must-be-stack-only
        /// </summary>
        public delegate bool IsNewWordBoundary(ReadOnlySpan<char> chars, int index);

        /// <summary>
        /// Return true if the index looks like a word boundary
        /// </summary>
        /// <param name="chars">ReadonlySpan of characters.</param>
        /// <param name="index">Index of character in question.</param>
        /// <returns>Returns true if the index looks like a word boundary</returns>
        public static bool _IsNewWordBoundary(ReadOnlySpan<char> chars, int index)
        {
            if (char.IsUpper(chars[index]))
            {
                // Current character is upper-case...
                if (index + 1 < chars.Length && char.IsLower(chars[index + 1]))
                    // and next character is lower-case.
                    return true;
                else if (index > 0 && chars.Length > 0 && char.IsLower(chars[index - 1]))
                    // and previous character is lower-case.
                    return true;
            }
            return false;
        }


        /// <summary>
        /// Convert a string to camelCase, removing any leading or trailing spaces.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <param name="wordBoundaryTest">Optional delegate for determining word boundaries.</param>
        /// <returns>A newly created (allocated) string.</returns>
        public static string ToCamelCase(this string str, IsNewWordBoundary wordBoundaryTest = null)
        {
            if (str == null) return null;
            var chars = str.AsSpan().Trim();

            if (chars.Length == 0)
                // Trim got rid of any white space, so now it would be an empty string.
                return string.Empty;

            if (wordBoundaryTest == null)
                wordBoundaryTest = _IsNewWordBoundary;

            var newChars = new char[chars.Length]; // Cannot be longer since we remove characters.

            bool inWord = false;
            int src = 0;
            int dest = 0;

            // If the first character in the trimmed source string is an underscore, keep it.
            if (chars[0] == '_')
            {
                newChars[dest] = chars[src];
                ++dest;
                ++src;
            }

            for (; src < chars.Length; ++src)
            {
                var isLetterOrDigit = char.IsLetterOrDigit(chars[src]); // Only call it once, we reference multiple times in the worst case scenario

                if (inWord && isLetterOrDigit && wordBoundaryTest(chars, src))
                    // Found a Upper-lower character sequence in the source. Flag as no longer in the word.
                    inWord = false;

                if (!inWord && !isLetterOrDigit)
                {
                    continue;
                }
                else if (inWord && isLetterOrDigit)
                {
                    // Copy it.
                    newChars[dest] = char.ToLower(chars[src]);
                    ++dest;
                }
                else if (!inWord && isLetterOrDigit)
                {
                    if (dest > 2)
                        // We are past first character or leading underscore, new word so capitalize it.
                        newChars[dest] = char.ToUpper(chars[src]);
                    else if (dest == 0)
                        // First letter is lowercase for camel case.
                        newChars[dest] = char.ToLower(chars[src]);
                    else if (newChars[0] == '_' || newChars[0] == '-')
                        // Leading underscore or dash, first letter in dest (dest=1 is only option here), lower-case.
                        newChars[dest] = char.ToLower(chars[src]);
                    else
                        // dest=1, Capitalize this one, we just entered a new word.
                        newChars[dest] = char.ToUpper(chars[src]);
                    ++dest;
                    inWord = true;
                }
                else
                {
                    // Not in a word, and not a letter or digit.
                    inWord = false;
                }
            }
            return new string(newChars, 0, dest);
        }


        /// <summary>
        /// Convert a string to PascalCase, removing any leading and trailing spaces.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <param name="wordBoundaryTest">Optional delegate for determining word boundaries.</param>
        /// <returns>A newly created (allocated) string.</returns>
        public static string ToPascalCase(this string str, IsNewWordBoundary wordBoundaryTest = null)
        {
            if (str == null) return null;
            var chars = str.AsSpan().Trim();

            if (chars.Length == 0)
                // Trim got rid of any white space, so now it would be an empty string.
                return string.Empty;

            if (wordBoundaryTest == null)
                wordBoundaryTest = _IsNewWordBoundary;

            var newChars = new char[chars.Length]; // Cannot be longer.

            bool inWord = false;
            int src = 0;
            int dest = 0;

            // If the first character in the trimmed source string is an underscore, keep it.
            if (chars[0] == '_')
            {
                newChars[dest] = chars[src];
                ++dest;
                ++src;
            }

            for (; src < chars.Length; ++src)
            {
                var isLetterOrDigit = char.IsLetterOrDigit(chars[src]); // Only call it once, we reference multiple times in the worst case scenario

                if (inWord && isLetterOrDigit && wordBoundaryTest(chars, src))
                    // Found a Upper-lower character sequence in the source. Flag as no longer in the word.
                    inWord = false;

                if (!inWord && !isLetterOrDigit)
                {
                    continue;
                }
                else if (inWord && isLetterOrDigit)
                {
                    // Copy it.
                    newChars[dest] = char.ToLower(chars[src]);
                    ++dest;
                }
                else if (!inWord && isLetterOrDigit)
                {
                    // Capitalize this one, we just entered a new word.
                    newChars[dest] = char.ToUpper(chars[src]);
                    ++dest;
                    inWord = true;
                }
                else
                {
                    // Not in a word, and not a letter or digit.
                    inWord = false;
                }
            }
            return new string(newChars, 0, dest);
        }


        /// <summary>
        /// Convert a string to lower-case and separated with the given character, removing any leading and trailing spaces.
        /// Used by snake_case and html-id-case.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <param name="wordBoundaryTest">Optional delegate for determining word boundaries.</param>
        /// <param name="separator">Optional separator. Underscore for snake case, dash for html ids.
        /// <returns>A newly created (allocated) string.</returns>
        public static string ToLowercaseSeparated(this string str, IsNewWordBoundary wordBoundryTest = null, char separator = '_')
        {
            if (str == null) return null;
            var chars = str.AsSpan().Trim();

            if (chars.Length == 0)
                // Trim got rid of any white space, so now it would be an empty string.
                return string.Empty;

            if (wordBoundryTest == null)
                wordBoundryTest = _IsNewWordBoundary;

            var newChars = new char[chars.Length * 2 + 1]; // Could be every other letter gets an underscore.

            bool inWord = false;
            int src = 0;
            int dest = 0;

            // If the first character in the trimmed source string is the separator, dash, or underscore, make it the separator.
            if (chars[0] == separator || chars[0] == '_' || chars[0] == '-')
            {
                newChars[dest] = separator;
                ++dest;
                ++src;
            }

            for (; src < chars.Length; ++src)
            {
                var isLetterOrDigit = char.IsLetterOrDigit(chars[src]); // Only call it once, we reference multiple times in the worst case scenario

                if (inWord && isLetterOrDigit && wordBoundryTest(chars, src))
                    // Found a Upper-lower character sequence in the source. Flag as no longer in the word.
                    inWord = false;

                if (!inWord && !isLetterOrDigit)
                {
                    continue;
                }
                else if (inWord && isLetterOrDigit)
                {
                    // Copy it.
                    newChars[dest] = char.ToLower(chars[src]);
                    ++dest;
                }
                else if (!inWord && isLetterOrDigit)
                {
                    if (dest > 1)
                    {
                        // Greater than one because otherwise we would double a leading underscore.
                        newChars[dest] = separator;
                        ++dest;
                    }

                    newChars[dest] = char.ToLower(chars[src]);
                    ++dest;
                    inWord = true;
                }
                else
                {
                    // Not in a word, and not a letter or digit.
                    inWord = false;
                }
            }
            return new string(newChars, 0, dest);
        }


        /// <summary>
        /// Convert a string to snake_case, removing any leading or trailing spaces.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <param name="wordBoundaryTest">Optional delegate for determining word boundaries. </param>
        /// <returns>A newly created (allocated) string.</returns>
        public static string ToSnakeCase(this string str, IsNewWordBoundary wordBoundaryTest = null, char separator = '-') => ToLowercaseSeparated(str, wordBoundaryTest, '_');


        /// <summary>
        /// Convert a string to html-id-case, removing any leading or trailing spaces.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <param name="wordBoundaryTest">Optional delegate for determining word boundaries.</param>
        /// <returns>A newly created (allocated) string.</returns>
        public static string ToHtmlIdCase(this string str, IsNewWordBoundary wordBoundaryTest = null) => ToLowercaseSeparated(str, wordBoundaryTest, '-');


    }
}
