// Copyright (c) 2018 Bill Adams. All Rights Reserved.
// Bill Adams licenses this file to you under the MIT license.
// See the license.txt file in the project root for more information.

using System;
using Xunit;
using LavaData.DateTimeExtensions;

namespace Lava_Data.DateTimeExtensions.Test
{
    public class ParseDateTimeTest
    {

        [Fact]
        public void TryToDateTimeLongTest()
        {
            DateTime dt;
            Assert.True(ParseDateTime.TryToDateTime(20180628L, 121532L, out dt));
            Assert.Equal(new DateTime(2018, 06, 28, 12, 15, 32), dt);

            // While the 35th is not a valid day of the month, so false from failed new DateTime.
            Assert.False(ParseDateTime.TryToDateTime(20180635L, 121532L, out dt));
        }


        [Fact]
        public void TryToDateTimeIntTest()
        {
            DateTime dt;
            Assert.True(ParseDateTime.TryToDateTime((int)20180628, (int)121532, out dt));
            Assert.Equal(new DateTime(2018, 06, 28, 12, 15, 32), dt);

            // While the 35th is not a valid day of the month, so false from failed new DateTime.
            Assert.False(ParseDateTime.TryToDateTime(20180635L, 121532L, out dt));
        }

        [Fact]
        public void TryToDateTimeTest()
        {
            DateTime dt;
            Assert.True(ParseDateTime.TryToDateTime("2018-01-01", out dt));
            Assert.Equal(new DateTime(2018, 1, 1), dt);

            Assert.True(ParseDateTime.TryToDateTime("01-Jan-2018", out dt));
            Assert.Equal(new DateTime(2018, 1, 1), dt);

            // Testing the relative datetime is tricky because a new DateTime will have a different number of
            // milliseconds. Zeroing that out would be easy but then there would be the occasional fail when
            // the two DateTimes cross a second boundary during testing.
            Assert.True(ParseDateTime.TryToDateTime("5", out dt));
            Assert.Equal(5, (int)DateTime.Now.Subtract(dt).TotalDays); // This subtraction will always be slightly more than 5.

            Assert.False(ParseDateTime.TryToDateTime("blah", out dt));
            Assert.Equal(default(DateTime), dt);
        }


        [Fact]
        public void TryToNullableDateTimeTest()
        {
            DateTime? dt;
            Assert.True(ParseDateTime.TryToDateTime("2018-01-01", out dt));
            Assert.Equal(new DateTime(2018, 1, 1), dt);

            Assert.True(ParseDateTime.TryToDateTime("5", out dt));
            Assert.Equal(5, (int)DateTime.Now.Subtract((DateTime)dt).TotalDays);

            // Don't need to test all of the other format variations because any
            // non-null string will use the non-nullable TryToDateTime().

            Assert.False(ParseDateTime.TryToDateTime("blah", out dt));
            Assert.Equal(null, dt);

        }

    }
}
