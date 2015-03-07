using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KjqbService
{
    public class ScheduleInService
    {

        private long writeUserId;

        public long WriteUserId
        {
            get { return writeUserId; }
            set { writeUserId = value; }
        }
        private long shareUserId;

        public long ShareUserId
        {
            get { return shareUserId; }
            set { shareUserId = value; }
        }
        private long scheduleId;
        public long ScheduleId
        {
            get { return scheduleId; }
            set { scheduleId = value; }
        }

        private long timeStamp;
        public long TimeStamp
        {
            get { return timeStamp; }
            set { timeStamp = value; }
        }

    }
}