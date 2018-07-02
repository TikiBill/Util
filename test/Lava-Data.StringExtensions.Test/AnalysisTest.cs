// Copyright (c) 2018 Bill Adams. All Rights Reserved.
// Bill Adams licenses this file to you under the MIT license.
// See the license.txt file in the project root for more information.

using System;
using Xunit;
using LavaData.StringExtensions;

namespace LavaData.StringExtensions.Test
{
    public class AnalysisTest
    {

        [Fact]
        /// <summary>
        /// Make sure that the int parsing will match our results, with the exception
        /// of trailing .000...
        /// </summary>
        public void TryParseIntTests()
        {
            int value;
            Assert.True(Int32.TryParse(" 0 ", out value));
            Assert.True(Int32.TryParse("-100", out value));

            // TryParse does not like the trailing .0
            Assert.False(Int32.TryParse("100.0", out value));

            // c# style for an int, unfortunately it does not parse.
            Assert.False(Int32.TryParse("100_000", System.Globalization.NumberStyles.Integer, System.Globalization.NumberFormatInfo.InvariantInfo, out value));

        }

        [Fact]
        public void LooksLikeAnIntegerTest()
        {
            Assert.True("0".LooksLikeAnInteger());
            Assert.True("+0".LooksLikeAnInteger());
            Assert.True("100".LooksLikeAnInteger());
            Assert.True("-100".LooksLikeAnInteger());

            //Spaces are allowed.
            Assert.True(" -100".LooksLikeAnInteger());
            Assert.True("-100 ".LooksLikeAnInteger());
            Assert.True(" -100 ".LooksLikeAnInteger());

            Assert.False("100x".LooksLikeAnInteger());
            Assert.False("x100".LooksLikeAnInteger());
            Assert.False("100x0".LooksLikeAnInteger());

            // Fractions not allowed by default.
            Assert.False("100.0000".LooksLikeAnInteger());

            // Too many digits, would overflow even a 64-bit int.
            Assert.False("123456789012345678901".LooksLikeAnInteger());

            //Special case to allow it.
            Assert.True("100.0000".LooksLikeAnInteger(allowDotZeros: true));

            //Make sure a trailing period does not crash AND is also false.
            Assert.False("100.".LooksLikeAnInteger(allowDotZeros: true));

            //Not an integer value.
            Assert.False("100.000000000001".LooksLikeAnInteger(allowDotZeros: true));

            //Double dot in number is not an integer even when zeros are allowed.
            Assert.False("100.00.0".LooksLikeAnInteger(allowDotZeros: true));

        }

        [Fact]
        public void LooksLikeADecimalTest()
        {
            Assert.True("0".LooksLikeADecimal());
            Assert.True("+0".LooksLikeADecimal());
            Assert.True("100".LooksLikeADecimal());
            Assert.True("-100".LooksLikeADecimal());
            Assert.True("100.000".LooksLikeADecimal());
            Assert.True("100.0000009".LooksLikeADecimal());

            // Spaces are allowed.
            Assert.True(" 100.01".LooksLikeADecimal());
            Assert.True("100.01 ".LooksLikeADecimal());
            Assert.True(" 100.01 ".LooksLikeADecimal());

            //We allow a single trailing decimal point w/o any digits.
            Assert.True("100.".LooksLikeADecimal());
            Assert.True("-100.".LooksLikeADecimal());

            Assert.True(".100".LooksLikeADecimal());
            Assert.True("+.100".LooksLikeADecimal());

            Assert.False(".100.".LooksLikeADecimal());
            Assert.False("+.100.".LooksLikeADecimal());
            Assert.False("0.100.".LooksLikeADecimal());
            Assert.False("0.100.001".LooksLikeADecimal());

        }
    }
}
