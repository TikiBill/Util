// Copyright (c) 2018 Bill Adams. All Rights Reserved.
// Bill Adams licenses this file to you under the MIT license.
// See the license.txt file in the project root for more information.

namespace LavaData.StringExtensions
{
    /// <summary>
    /// String capitalization enum, used to indicate how the capitalization of a string should be
    /// changed.
    /// 
    /// https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/capitalization-conventions
    /// </summary>
    public enum StringCapitalization
    {
        ///<summary> Do not change the case of the given string. </summary>
        KeepCase,
        ///<summary> UPPERCASE (All uppercase letters) </summary>
        UpperCase,
        ///<summary> lowercase (All lowercase letters) </summary>
        LowerCase,
        ///<summary> camelCase (first letter lower-case, then upper-case letters for words) </summary>
        CamelCase,
        ///<summary> PascalCase (upper case first letters of words) </summary>
        PascalCase,
        ///<summary> snake_case (lower case words separated by underscore) </summary>
        SnakeCase,
        ///<summary> html-id-case (lower case words separated by dashes) </summary>
        HtmlIdCase,
    }
}
