using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary
{
    public class LeaderSign : IEntity
    {
        #region 构造函数
        public LeaderSign()
        { }

        public LeaderSign(long LLS_LEADERID, long LLS_STAFFID, long LLS_SIGNINTIME, long LLS_SIGNBACKTIME, DateTime LLS_TIMESTAMP, int LLS_APPROVALSTATUS)
        {
            //this.id = Id;
            this.leaderId = LLS_LEADERID;
            this.staffId = LLS_STAFFID;
            this.signInTime = LLS_SIGNINTIME;
            this.signBackTime = LLS_SIGNBACKTIME;
            this.timeStamp = LLS_TIMESTAMP;
            this.approvalStatus = LLS_APPROVALSTATUS;
        }
        #endregion

        #region 成员
        //private int id;
        private long leaderId;
        private long staffId;
        private long signInTime;
        private long signBackTime;
        private DateTime timeStamp;
        private int approvalStatus;
        #endregion


        #region 属性
        //public virtual int Id
        //{
        //    get { return id; }
        //    set { id = value; }
        //}
        public virtual long LLS_LEADERID
        {
            get { return leaderId; }
            set { leaderId = value; }
        }

        public virtual long LLS_STAFFID
        {
            get { return staffId; }
            set { staffId = value; }
        }

        public virtual long LLS_SIGNINTIME
        {
            get { return signInTime; }
            set { signInTime = value; }
        }

        public virtual long LLS_SIGNBACKTIME
        {
            get { return signBackTime; }
            set { signBackTime = value; }
        }

        public virtual DateTime LLS_TIMESTAMP
        {
            get { return timeStamp; }
            set { timeStamp = value; }
        }

        public virtual int LLS_APPROVALSTATUS
        {
            get { return approvalStatus; }
            set { approvalStatus = value; }
        }

        #endregion
    }
}
