using System;
using System.Collections;
using System.Collections.Generic;

namespace ClassLibrary
{
    public class Holiday : IEntity
    {

        private string name;

        public virtual string Name
        {
            get { return name; }
            set { name = value; }
        }
        private int holidayYear;

        public virtual int HolidayYear
        {
            get { return holidayYear; }
            set { holidayYear = value; }
        }
        private long startTime;

        public virtual long StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }
        private long endTime;

        public virtual long EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        private IList<WorkDay> workDays;

        public virtual IList<WorkDay> WorkDays
        {
            get { return workDays; }
            set { workDays = value; }
        }
        
    }
}
