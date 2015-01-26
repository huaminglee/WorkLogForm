using System;
using System.Collections;

namespace ClassLibrary
{
    public class WorkDay : IEntity
    {
        private long workDateTime;

        public virtual long WorkDateTime
        {
            get { return workDateTime; }
            set { workDateTime = value; }
        }
        private Holiday holidayId;

        public virtual Holiday HolidayId
        {
            get { return holidayId; }
            set { holidayId = value; }
        }
    }
}
