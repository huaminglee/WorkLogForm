using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary
{
    public class InterimLeader : IEntity
    {
         #region 构造函数
        public InterimLeader()
        { }

        public InterimLeader(long LIL_INTERIMLEADERID, long KU_ID, int LIL_LEAVETYPE, string LIL_LEAVEREASON, int LIL_LEAVESJZ, int LIL_APPROVALSTATUS, int LIL_APPROVALSTAGE, DateTime LIL_TIMESTAMP, DateTime LIL_STARTTIME, int LIL_STARTSTAGE, DateTime LIL_ENDTIME, int LIL_ENDSTAGE)
        {
            //this.id = Id;
            this.interimLeaderId = LIL_INTERIMLEADERID;
            this.ku_Id = KU_ID;
            this.leaveType = LIL_LEAVETYPE;
            this.leaveReason = LIL_LEAVEREASON;
            this.leaveSjz = LIL_LEAVESJZ;
            this.approvalStatus = LIL_APPROVALSTATUS;
            this.approvalStage = LIL_APPROVALSTAGE;
            this.timeStamp = LIL_TIMESTAMP;
            this.startTime = LIL_STARTTIME;
            this.startStage = LIL_STARTSTAGE;
            this.endTime = LIL_ENDTIME;
            this.endStage = LIL_ENDSTAGE;
        }
        #endregion

        #region 成员
        //private int id;
        private long interimLeaderId;
        private long ku_Id;
        private int leaveType;
        private string leaveReason;
        private int leaveSjz;
        private int approvalStatus;
        private int approvalStage;
        private DateTime timeStamp;
        private DateTime startTime;
        private int startStage;
        private DateTime endTime;
        private int endStage;
        #endregion


        #region 属性
        //public virtual int Id
        //{
        //    get { return id; }
        //    set { id = value; }
        //}
        public virtual long LIL_INTERIMLEADERID
        {
            get{ return interimLeaderId; }
            set { interimLeaderId = value; }
        }

        public virtual long KU_ID
        {
            get { return ku_Id; }
            set { ku_Id = value; }
        }

        public virtual int LIL_LEAVETYPE
        {
            get { return leaveType; }
            set { leaveType = value; }
        }

        public virtual string LIL_LEAVEREASON
        {
            get { return leaveReason; }
            set { leaveReason = value; }
        }

        public virtual int LIL_LEAVESJZ
        {
            get { return leaveSjz; }
            set { leaveSjz = value; }
        }

        public virtual int LIL_APPROVALSTATUS
        {
            get { return approvalStatus; }
            set { approvalStatus = value; }
        }

        public virtual int LIL_APPROVALSTAGE
        {
            get { return approvalStage; }
            set { approvalStage = value; }
        }

        public virtual DateTime LIL_TIMESTAMP
        {
            get { return timeStamp; }
            set { timeStamp = value; }
        }

        public virtual DateTime LIL_STARTTIME
        {
            get { return startTime; }
            set { startTime = value; }
        }

        public virtual int LIL_STARTSTAGE
        {
            get { return startStage; }
            set { startStage = value; }
        }

        public virtual DateTime LIL_ENDTIME
        {
            get { return endTime; }
            set { endTime = value; }
        }

        public virtual int LIL_ENDSTAGE
        {
            get { return endStage; }
            set { endStage = value; }
        }
        #endregion

    }
}
