using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KjqbService
{
    public class LogInService
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
        private long logId;

        public long LogId
        {
            get { return logId; }
            set { logId = value; }
        }

    }
}