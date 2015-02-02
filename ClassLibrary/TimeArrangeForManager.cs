using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary
{
    public class TimeArrangeForManager : IEntity
    {

        private WkTUser userId;
        public virtual WkTUser UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        private WkTUser arrangeUserId;
        public virtual WkTUser ArrangeUserId
        {
            get { return arrangeUserId; }
            set { arrangeUserId = value; }
        }

        private long timeMonth;
        public virtual long TimeMonth
        {
            get { return timeMonth; }
            set { timeMonth = value; }
        }

        private int isDone;
        public virtual int IsDone
        {
            get { return isDone; }
            set { isDone = value; }
        }

        private int dutyType;
        public virtual int DutyType
        {
            get { return dutyType; }
            set { dutyType = value; }
        }

        private int examineState;
        public virtual int ExamineState
        {
            get { return examineState; }
            set { examineState = value; }
        }


    }
}
