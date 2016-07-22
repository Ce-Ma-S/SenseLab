using System;
using System.Collections.Generic;
using System.Linq;

namespace CeMaS.Common.ValueDomains.Time
{
    public class DaysOfWeek :
        TimeDomain
    {
        public bool Monday
        {
            get { return monday; }
            set { SetPropertyValue(ref monday, value); }
        }
        public bool Tuesday
        {
            get { return tuesday; }
            set { SetPropertyValue(ref tuesday, value); }
        }
        public bool Wednesday
        {
            get { return wednesday; }
            set { SetPropertyValue(ref wednesday, value); }
        }
        public bool Thursday
        {
            get { return thursday; }
            set { SetPropertyValue(ref thursday, value); }
        }
        public bool Friday
        {
            get { return friday; }
            set { SetPropertyValue(ref friday, value); }
        }
        public bool Saturday
        {
            get { return saturday; }
            set { SetPropertyValue(ref saturday, value); }
        }
        public bool Sunday
        {
            get { return sunday; }
            set { SetPropertyValue(ref sunday, value); }
        }

        public IEnumerable<DayOfWeek> Value
        {
            get
            {
                if (Monday)
                    yield return DayOfWeek.Monday;
                if (Tuesday)
                    yield return DayOfWeek.Tuesday;
                if (Wednesday)
                    yield return DayOfWeek.Wednesday;
                if (Thursday)
                    yield return DayOfWeek.Thursday;
                if (Friday)
                    yield return DayOfWeek.Friday;
                if (Saturday)
                    yield return DayOfWeek.Saturday;
                if (Sunday)
                    yield return DayOfWeek.Sunday;
            }
        }

        public override bool Contains(DateTimeOffset value)
        {
            return Value.Contains(value.DayOfWeek);
        }

        private bool monday;
        private bool tuesday;
        private bool wednesday;
        private bool thursday;
        private bool friday;
        private bool saturday;
        private bool sunday;
    }
}
