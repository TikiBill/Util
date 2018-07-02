// Copyright (c) 2018 Bill Adams. All Rights Reserved.
// Bill Adams licenses this file to you under the MIT license.
// See the license.txt file in the project root for more information.

namespace LavaData.DateTimeExtensions
{
    public struct YearTimeSpan
    {
        public int Seconds { get; set; }

        public int Minutes { get; set; }

        public int Hours { get; set; }

        public int Days { get; set; }

        public int Weeks { get; set; }

        public int Years { get; set; }


        /// <summary>
        /// Structure for holding a time span that goes up to years. Mostly intended for producing 
        /// a human readable string like 5 days, 1 hour.
        /// 
        /// Note: The year calculation does not account for leap years nor leap seconds.
        /// It cannot without knowing the year(s) being spanned. And since this is intended
        /// for producing human consumable strings, hopefully a day does not matter once the
        /// span gets to a year or more. For example, "Your training is overdue by 1 year and 1 day"
        /// vs. "Your training is overdue by 1 year and 2 days".
        /// </summary>
        public YearTimeSpan(long seconds)
        {
            this.Years = (int)(seconds / (365 * 24 * 3600));
            seconds -= this.Years * 365 * 24 * 3600;

            this.Weeks = (int)(seconds / (7 * 24 * 3600));
            seconds -= this.Weeks * 7 * 24 * 3600;

            this.Days = (int)(seconds / (24 * 3600));
            seconds -= this.Days * 24 * 3600;

            this.Hours = (int)(seconds / 3600);
            seconds -= this.Hours * 3600;

            this.Minutes = (int)(seconds / 60);
            seconds -= this.Minutes * 60;

            this.Seconds = (int)seconds;
        }
    }
}

