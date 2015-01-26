using System;
using System.Collections;
using System.Collections.Generic;

namespace ClassLibrary
{
    public class TimeAndDay : IEntity
    {
        private DateTime startTime;

        public virtual DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }
        private DateTime endTime;

        public virtual DateTime EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }
        private string workDay;

        public virtual string WorkDay
        {
            get { return workDay; }
            set { workDay = value; }
        }

        private IList<Holiday> holidays;

        public virtual IList<Holiday> Holidays
        {
            get { return holidays; }
            set { holidays = value; }
        }
        private IList<WorkDay> workDays;

        public virtual IList<WorkDay> WorkDays
        {
            get { return workDays; }
            set { workDays = value; }
        }

    }
}
