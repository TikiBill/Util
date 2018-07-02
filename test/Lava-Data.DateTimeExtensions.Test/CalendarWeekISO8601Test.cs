// Copyright (c) 2018 Bill Adams. All Rights Reserved.
// Bill Adams licenses this file to you under the MIT license.
// See the license.txt file in the project root for more information.

using System;
using Xunit;
using LavaData.DateTimeExtensions;
using System.Globalization;

namespace Lava_Data.DateTimeExtensions.Test
{
    public class CalendarWeekIso8601Tests
    {
        public static readonly Calendar calendar = CultureInfo.InvariantCulture.Calendar;

        /// <summary>
        /// Verify that Sunday is zero and that we can map every day of the week to Thursday.
        /// If this starts failing, it means everything will fail and our methods need to be updated.
        /// 
        /// The DayOfWeek ENUM should never change as I'd imagine it will break a whole lot of code if it did.
        /// At least we'll be aware if it does change....
        /// </summary>
        [Fact]
        public void VerifyDayOfWeekMath()
        {
            DateTime sunday = new DateTime(2018, 7, 1);
            Assert.Equal(0, (int)DayOfWeek.Sunday);
            Assert.Equal(DayOfWeek.Sunday, calendar.GetDayOfWeek(sunday));
            Assert.Equal(DayOfWeek.Sunday, sunday.DayOfWeek);
            Assert.Equal(DayOfWeek.Thursday, calendar.GetDayOfWeek(sunday.AddDays(DayOfWeek.Thursday - calendar.GetDayOfWeek(sunday))));

            DateTime monday = new DateTime(2018, 7, 2);
            Assert.Equal(1, (int)DayOfWeek.Monday);
            Assert.Equal(DayOfWeek.Monday, calendar.GetDayOfWeek(monday));
            Assert.Equal(DayOfWeek.Thursday, calendar.GetDayOfWeek(monday.AddDays(DayOfWeek.Thursday - calendar.GetDayOfWeek(monday))));

            DateTime tuesday = new DateTime(2018, 7, 3);
            Assert.Equal(2, (int)DayOfWeek.Tuesday);
            Assert.Equal(DayOfWeek.Tuesday, calendar.GetDayOfWeek(tuesday));
            Assert.Equal(DayOfWeek.Thursday, calendar.GetDayOfWeek(tuesday.AddDays(DayOfWeek.Thursday - calendar.GetDayOfWeek(tuesday))));

            DateTime wednesday = new DateTime(2018, 7, 4);
            Assert.Equal(3, (int)DayOfWeek.Wednesday);
            Assert.Equal(DayOfWeek.Wednesday, calendar.GetDayOfWeek(wednesday));
            Assert.Equal(DayOfWeek.Thursday, calendar.GetDayOfWeek(wednesday.AddDays(DayOfWeek.Thursday - calendar.GetDayOfWeek(wednesday))));

            DateTime thursday = new DateTime(2018, 7, 5);
            Assert.Equal(4, (int)DayOfWeek.Thursday);
            Assert.Equal(DayOfWeek.Thursday, calendar.GetDayOfWeek(thursday));
            Assert.Equal(DayOfWeek.Thursday, calendar.GetDayOfWeek(thursday.AddDays(DayOfWeek.Thursday - calendar.GetDayOfWeek(thursday))));

            DateTime friday = new DateTime(2018, 7, 6);
            Assert.Equal(5, (int)DayOfWeek.Friday);
            Assert.Equal(DayOfWeek.Friday, calendar.GetDayOfWeek(friday));
            Assert.Equal(DayOfWeek.Thursday, calendar.GetDayOfWeek(friday.AddDays(DayOfWeek.Thursday - calendar.GetDayOfWeek(friday))));

            DateTime saturday = new DateTime(2018, 7, 7);
            Assert.Equal(6, (int)DayOfWeek.Saturday);
            Assert.Equal(DayOfWeek.Saturday, calendar.GetDayOfWeek(saturday));
            Assert.Equal(DayOfWeek.Thursday, calendar.GetDayOfWeek(saturday.AddDays(DayOfWeek.Thursday - calendar.GetDayOfWeek(saturday))));
        }


        [Fact]
        public void GetIso8601WeekOfYearTest()
        {
            Assert.Equal(1, (new DateTime(2007, 12, 31)).GetIso8601WeekOfYear());
            Assert.Equal(1, (new DateTime(2017, 12, 31)).GetIso8601WeekOfYear());
            Assert.Equal(1, (new DateTime(2018, 1, 1)).GetIso8601WeekOfYear());
        }


        [Fact]
        public void GetIso8601YearWeekTest()
        {
            Assert.Equal((2008, 1), (new DateTime(2007, 12, 31)).GetIso8601YearWeek());
            Assert.Equal((2017, 52), (new DateTime(2017, 12, 30)).GetIso8601YearWeek());
            Assert.Equal((2018, 1), (new DateTime(2018, 1, 4)).GetIso8601YearWeek());
            Assert.Equal((2018, 1), (new DateTime(2017, 12, 31)).GetIso8601YearWeek());
        }


        [Fact]
        public void CalendarWeekStringISO8601()
        {
            Assert.Equal("2008W01", (new DateTime(2007, 12, 31)).CalendarWeekStringIso8601());
            Assert.Equal("2017W52", (new DateTime(2017, 12, 30)).CalendarWeekStringIso8601());
            Assert.Equal("2018W01", (new DateTime(2018, 1, 4)).CalendarWeekStringIso8601());
            Assert.Equal("2018W01", (new DateTime(2017, 12, 31)).CalendarWeekStringIso8601());
        }


        [Fact]
        public void ThursdayForWeekOfYearTest()
        {
            //Jan 1 2011 = Saturday, so first Thursday is Jan 6.
            Assert.Equal(new DateTime(2011, 1, 6), CalendarWeekIso8601.ThursdayForWeekOfYear(2011, 1));
            Assert.Equal(new DateTime(2012, 1, 5), CalendarWeekIso8601.ThursdayForWeekOfYear(2012, 1)); //Jan 1 = Sunday
            Assert.Equal(new DateTime(2014, 1, 2), CalendarWeekIso8601.ThursdayForWeekOfYear(2014, 1));
            Assert.Equal(new DateTime(2015, 1, 1), CalendarWeekIso8601.ThursdayForWeekOfYear(2015, 1));

            // In 2016, Jan 1 is Friday, so the first Thursday is Jan 7.
            Assert.Equal(new DateTime(2016, 1, 7), CalendarWeekIso8601.ThursdayForWeekOfYear(2016, 1));
            Assert.Equal(new DateTime(2017, 1, 5), CalendarWeekIso8601.ThursdayForWeekOfYear(2017, 1));
            Assert.Equal(new DateTime(2018, 1, 4), CalendarWeekIso8601.ThursdayForWeekOfYear(2018, 1));

        }


        [Fact]
        public void ThursdayInWeekOfDateTest()
        {
            Assert.Equal(new DateTime(2018, 01, 18), CalendarWeekIso8601.ThursdayInWeekOfDate(new DateTime(2018, 01, 14)));
            Assert.Equal(new DateTime(2018, 01, 18), CalendarWeekIso8601.ThursdayInWeekOfDate(new DateTime(2018, 01, 16)));
            Assert.Equal(new DateTime(2018, 01, 18), CalendarWeekIso8601.ThursdayInWeekOfDate(new DateTime(2018, 01, 18)));
            Assert.Equal(new DateTime(2018, 01, 18), CalendarWeekIso8601.ThursdayInWeekOfDate(new DateTime(2018, 01, 20)));
        }


        [Fact]
        public void StartDateOfWeekYearWeekTest()
        {
            Assert.Equal(new DateTime(2017, 12, 31), CalendarWeekIso8601.StartDateOfWeek(2018, 1));
            Assert.Equal(new DateTime(2018, 01, 07), CalendarWeekIso8601.StartDateOfWeek(2018, 2));
        }


        [Fact]
        public void StartDateOfWeekDateTimeTest()
        {
            Assert.Equal(new DateTime(2017, 12, 31), CalendarWeekIso8601.StartDateOfWeek(new DateTime(2018, 01, 02)));
            Assert.Equal(new DateTime(2018, 01, 07), CalendarWeekIso8601.StartDateOfWeek(new DateTime(2018, 01, 09)));
        }


        [Fact]
        public void EndDateOfWeekYearWeekTest()
        {
            Assert.Equal(new DateTime(2017, 12, 30), CalendarWeekIso8601.EndDateOfWeek(2017, 52));
            Assert.Equal(new DateTime(2018, 01, 06), CalendarWeekIso8601.EndDateOfWeek(2018, 1));
            Assert.Equal(new DateTime(2018, 01, 13), CalendarWeekIso8601.EndDateOfWeek(2018, 2));
        }


        [Fact]
        public void EndDateOfWeekDateTimeTest()
        {
            Assert.Equal(new DateTime(2017, 12, 30), CalendarWeekIso8601.EndDateOfWeek(new DateTime(2017, 12, 28)));
            Assert.Equal(new DateTime(2018, 01, 06), CalendarWeekIso8601.EndDateOfWeek(new DateTime(2018, 01, 02)));
            Assert.Equal(new DateTime(2018, 01, 13), CalendarWeekIso8601.EndDateOfWeek(new DateTime(2018, 01, 09)));
        }
    }
}
