// Copyright (c) 2018 Bill Adams. All Rights Reserved.
// Bill Adams licenses this file to you under the MIT license.
// See the license.txt file in the project root for more information.

using System;
using Xunit;
using LavaData.StringExtensions;

using System.Data.Common;
using System.Reflection;

namespace LavaData.StringExtensions.Test
{
    public class SqlQuoteTests
    {
        /// <summary>
        /// Test object including boxed value types. Every case statement must
        /// be tested to ensure we do not accidently get into an infinite loop.
        /// </summary>
        [Fact]
        public void QuoteTestObject()
        {

            Assert.Equal("'Hello'", ((object)"Hello").ToSqlQuotedString());
            Assert.Equal("'Boo'", (new BooClass()).ToSqlQuotedString());

            //Char
            Assert.Equal("'x'", ((object)'x').ToSqlQuotedString());
            Assert.Equal("'x'", ((object)(char?)'x').ToSqlQuotedString());
            Assert.Equal("NULL", ((object)(char?)null).ToSqlQuotedString());
            Assert.Equal("''''", ((object)'\'').ToSqlQuotedString());  // Make sure a single quote character is handled correctly!

            // Int16
            Assert.Equal("16", ((object)(short)16).ToSqlQuotedString());
            Assert.Equal("-16", ((object)(short)-16).ToSqlQuotedString());
            Assert.Equal("16", ((object)(short?)16).ToSqlQuotedString());
            Assert.Equal("NULL", ((object)(short?)null).ToSqlQuotedString());

            // UInt16
            Assert.Equal("16", ((object)(ushort)16).ToSqlQuotedString());
            Assert.Equal("16", ((object)(ushort?)16).ToSqlQuotedString());
            Assert.Equal("NULL", ((object)(ushort?)null).ToSqlQuotedString());

            // Int32
            Assert.Equal("32", ((object)(int)32).ToSqlQuotedString());
            Assert.Equal("-32", ((object)(int)-32).ToSqlQuotedString());
            Assert.Equal("32", ((object)(int?)32).ToSqlQuotedString());
            Assert.Equal("NULL", ((object)(int?)null).ToSqlQuotedString());

            // Int64
            Assert.Equal("64", ((object)(int)64).ToSqlQuotedString());
            Assert.Equal("-64", ((object)(int)-64).ToSqlQuotedString());
            Assert.Equal("64", ((object)(int?)64).ToSqlQuotedString());
            Assert.Equal("NULL", ((object)(int?)null).ToSqlQuotedString());

            // Single
            Assert.Equal("5.5", ((object)(float)5.5).ToSqlQuotedString());
            Assert.Equal("-5.5", ((object)(float)-5.5).ToSqlQuotedString());
            Assert.Equal("5.5", ((object)(float?)5.5).ToSqlQuotedString());
            Assert.Equal("NULL", ((object)(float?)null).ToSqlQuotedString());

            // Double
            Assert.Equal("5.5", ((object)(double)5.5).ToSqlQuotedString());
            Assert.Equal("-5.5", ((object)(double)-5.5).ToSqlQuotedString());
            Assert.Equal("5.5", ((object)(double?)5.5).ToSqlQuotedString());
            Assert.Equal("NULL", ((object)(double?)null).ToSqlQuotedString());

            //DateTime
            Assert.Equal("'2018-04-01 12:15:13'", ((object)(new DateTime(2018, 04, 01, 12, 15, 13))).ToSqlQuotedString());
            Assert.Equal("'2018-04-01 12:15:13'", ((object)((DateTime?)(new DateTime(2018, 04, 01, 12, 15, 13)))).ToSqlQuotedString());
            Assert.Equal("NULL", ((object)((DateTime?)null)).ToSqlQuotedString());
            Assert.Equal("NULL", ((object)(default(DateTime))).ToSqlQuotedString());
            Assert.Equal("NULL", ((object)((DateTime?)(default(DateTime)))).ToSqlQuotedString());

            //Guid
            Guid guid = System.Guid.NewGuid();
            Guid? nullableGuid = guid;
            var guidStr = "'" + guid.ToString("D") + "'";
            Assert.Equal(guidStr, ((object)guid).ToSqlQuotedString());
            Assert.Equal(guidStr, ((object)nullableGuid).ToSqlQuotedString());
            nullableGuid = null;
            Assert.Equal("NULL", ((object)nullableGuid).ToSqlQuotedString());
            Assert.Equal("NULL", ((object)default(Guid)).ToSqlQuotedString());
            Assert.Equal("NULL", ((object)((Guid?)default(Guid))).ToSqlQuotedString());
        }

        [Fact]
        public void QuoteTestStrings()
        {
            Assert.Equal("'Hello'", "Hello".ToSqlQuotedString());
            Assert.Equal(7, "Hello".ToSqlQuotedString().Length);
            Assert.Equal("'Hello'", "Hello".ToSqlQuotedString_LongString());
            Assert.Equal(7, "Hello".ToSqlQuotedString_LongString().Length);

            Assert.Equal("'Hello'", "Hel\0lo".ToSqlQuotedString());
            Assert.Equal(7, "Hel\0lo".ToSqlQuotedString().Length);
            Assert.Equal("'Hello'", "Hel\0lo".ToSqlQuotedString_LongString());
            Assert.Equal(7, "Hel\0lo".ToSqlQuotedString_LongString().Length);
        }


        [Fact]
        public void QuoteViaPropertyTest()
        {
            var all = new AllTypes();
            Assert.Equal("32", all.Int32Type.ToSqlQuotedString());

            PropertyInfo[] allProperties = all.GetType().GetProperties();
            foreach (var prop in allProperties)
            {
                var valueObj = prop.GetValue(all);
                string str;
                switch (valueObj)
                {
                    case Int32 i: str = i.ToSqlQuotedString(); break;
                    default: str = "?"; break;
                }
            }
        }

    }

    class BooClass
    {
        public override string ToString() => "Boo";
    }

    class AllTypes
    {
        public int Int32Type { get; set; } = 32;
    }

}
