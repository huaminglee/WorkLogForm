using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KjqbService
{
    public class TimeArrangeForManagerInService
    {
        private long timeArrangeForManagerId;

        public long TimeArrangeForManagerId
        {
            get { return timeArrangeForManagerId; }
            set { timeArrangeForManagerId = value; }
        }
        private long userId;

        public long UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        private long sendUserId;

        public long SendUserId
        {
            get { return sendUserId; }
            set { sendUserId = value; }
        }
        private int examineOrExamineresult;

        public int ExamineOrExamineresult
        {
            get { return examineOrExamineresult; }
            set { examineOrExamineresult = value; }
        }

    }
}