using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KjqbService
{
    public class LeaveInService
    {
        private long leaveId;

        public long LeaveId
        {
            get { return leaveId; }
            set { leaveId = value; }
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