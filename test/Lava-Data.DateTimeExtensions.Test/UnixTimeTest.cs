// Copyright (c) 2018 Bill Adams. All Rights Reserved.
// Bill Adams licenses this file to you under the MIT license.
// See the license.txt file in the project root for more information.

using System;
using Xunit;
using LavaData.DateTimeExtensions;

namespace Lava_Data.DateTimeExtensions.Test
{
    public class UnixTimeTest
    {
        [Fact]
        public void ToUnixTimeTest()
        {
            Assert.Equal(0, (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToUnixTime()));
            Assert.Equal(1530237889, (new DateTime(2018, 06, 29, 02, 04, 49, DateTimeKind.Utc)).ToUnixTime());
        }

        [Fact]
        public void ToUnixTimeMillisecondsTest()
        {
            Assert.Equal(0, (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToUnixTimeMilliseconds()));
            Assert.Equal(1530237889000, (new DateTime(2018, 06, 29, 02, 04, 49, DateTimeKind.Utc)).ToUnixTimeMilliseconds());
        }


        [Fact]
        public void ToUnixTimeNullableTest()
        {
            Assert.Equal(0, (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToUnixTime()));
            Assert.Equal(0, ((DateTime?)null).ToUnixTime());
            Assert.Equal(1530237889, (new DateTime(2018, 06, 29, 02, 04, 49, DateTimeKind.Utc)).ToUnixTime());
        }


        [Fact]
        public void ToUnixTimeMillisecondsNullableTest()
        {
            Assert.Equal(0, (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToUnixTimeMilliseconds()));
            Assert.Equal(0, ((DateTime?)null).ToUnixTimeMilliseconds());
            Assert.Equal(1530237889000, (new DateTime(2018, 06, 29, 02, 04, 49, DateTimeKind.Utc)).ToUnixTimeMilliseconds());
        }


        [Fact]
        public void CurrentUnixTime()
        {
            Assert.True(UnixTime.CurrentUnixTime() > 1530237889);
            Assert.True((new DateTime(2018, 1, 1)).CurrentUnixTime() > 1530237889);
            Assert.True(((DateTime?)null).CurrentUnixTime() > 1530237889);
        }


        [Fact]
        public void CurrentUnixTimeMilliseconds()
        {
            Assert.True(UnixTime.CurrentUnixTimeMilliseconds() > 1530237889000);
            Assert.True((new DateTime(2018, 1, 1)).CurrentUnixTimeMilliseconds() > 1530237889000);
            Assert.True(((DateTime?)null).CurrentUnixTimeMilliseconds() > 1530237889000);
        }


        [Fact]
        public void ToUtcDateTimeIntTest()
        {
            Assert.Equal(new DateTime(2018, 06, 29, 02, 04, 49, DateTimeKind.Utc), UnixTime.UnixTimeToUtcDateTime((int)1530237889));
        }


        [Fact]
        public void ToUtcDateTimeNullableIntTest()
        {
            Assert.Equal(new DateTime(2018, 06, 29, 02, 04, 49, DateTimeKind.Utc), UnixTime.UnixTimeToUtcDateTime((int?)1530237889));
            Assert.Equal(default(DateTime), UnixTime.UnixTimeToUtcDateTime((int?)null));
        }


        [Fact]
        public void ToUtcDateTimeUIntTest()
        {
            Assert.Equal(new DateTime(2018, 06, 29, 02, 04, 49, DateTimeKind.Utc), UnixTime.UnixTimeToUtcDateTime((uint)1530237889));
        }


        [Fact]
        public void ToUtcDateTimeNullableUIntTest()
        {
            Assert.Equal(new DateTime(2018, 06, 29, 02, 04, 49, DateTimeKind.Utc), UnixTime.UnixTimeToUtcDateTime((uint?)1530237889));
            Assert.Equal(default(DateTime), UnixTime.UnixTimeToUtcDateTime((uint?)null));
        }


        [Fact]
        public void ToUtcDateTimeLongTest()
        {
            Assert.Equal(new DateTime(2018, 06, 29, 02, 04, 49, DateTimeKind.Utc), UnixTime.UnixTimeToUtcDateTime(1530237889L));
        }


        [Fact]
        public void ToUtcDateTimeNullableLongTest()
        {
            Assert.Equal(new DateTime(2018, 06, 29, 02, 04, 49, DateTimeKind.Utc), UnixTime.UnixTimeToUtcDateTime((long?)1530237889L));
            Assert.Equal(default(DateTime), UnixTime.UnixTimeToUtcDateTime((long?)null));
        }


        [Fact]
        public void ToUtcDateTimeULongTest()
        {
            Assert.Equal(new DateTime(2018, 06, 29, 02, 04, 49, DateTimeKind.Utc), UnixTime.UnixTimeToUtcDateTime(1530237889UL));
        }


        [Fact]
        public void ToUtcDateTimeNullableULongTest()
        {
            Assert.Equal(new DateTime(2018, 06, 29, 02, 04, 49, DateTimeKind.Utc), UnixTime.UnixTimeToUtcDateTime((ulong?)1530237889UL));
            Assert.Equal(default(DateTime), UnixTime.UnixTimeToUtcDateTime((ulong?)null));
        }


        [Fact]
        public void ToUtcDateTimeDoubleTest()
        {
            Assert.Equal(new DateTime(2018, 06, 29, 02, 04, 49, DateTimeKind.Utc), UnixTime.UnixTimeToUtcDateTime(1530237889d));
        }


        [Fact]
        public void ToUtcDateTimeNullableDoubleTest()
        {
            Assert.Equal(new DateTime(2018, 06, 29, 02, 04, 49, DateTimeKind.Utc), UnixTime.UnixTimeToUtcDateTime((double?)1530237889d));
            Assert.Equal(default(DateTime), UnixTime.UnixTimeToUtcDateTime((double?)null));
        }

    }
}
