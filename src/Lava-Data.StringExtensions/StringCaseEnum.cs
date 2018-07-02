// Copyright (c) 2018 Bill Adams. All Rights Reserved.
// Bill Adams licenses this file to you under the MIT license.
// See the license.txt file in the project root for more information.

namespace LavaData.StringExtensions
{
    public enum StringCase
    {
        ///<summary> Case-insensitive </summary>
        AnyCase,
        ///<summary> Do not change the case from how it was derived. </summary>
        KeepCase,
        UpperCase,
        LowerCase,
        CamelCase,
        PascalCase,
        SnakeCase,
        HtmlIdCase,
    }
}
