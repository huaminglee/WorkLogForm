using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary
{
    public class AttenceLog : IEntity
    {
         #region 构造函数
        public AttenceLog()
        { }

        public AttenceLog(long KU_ID, long LAL_SIGNINTIME, long LAL_SIGNBACKTIME, int LAL_ISLATE, int LAL_ISLEAVEEARLY, int LAL_YEAR, int LAL_MONTH, int LAL_DAY, DateTime LAL_TIMESTAMP, String LAL_LOG)
        {
            //this.id = Id;
            this.ku_Id = KU_ID;
            this.signInTime = LAL_SIGNINTIME;
            this.signBackTime = LAL_SIGNBACKTIME;
            this.isLate = LAL_ISLATE;
            this.isLeaveEarly = LAL_ISLEAVEEARLY;
            this.year = LAL_YEAR;
            this.month = LAL_MONTH;
            this.day = LAL_DAY;
            this.timeStamp = LAL_TIMESTAMP;
            this.log = LAL_LOG;
        }
        #endregion

        #region 成员
        //private int id;
        private long ku_Id;
        private long signInTime;
        private long signBackTime;
        private int isLate;
        private int isLeaveEarly;
        private int year;
        private int month;
        private int day;
        private DateTime timeStamp;
        private string log;
        #endregion


        #region 属性
        //public virtual int Id
        //{
        //    get { return id; }
        //    set { id = value; }
        //}
        public virtual long KU_ID
        {
            get { return ku_Id; }
            set { ku_Id = value; }
        }

        public virtual long LAL_SIGNINTIME
        {
            get { return signInTime; }
            set { signInTime = value; }
        }

        public virtual long LAL_SIGNBACKTIME
        {
            get { return signBackTime; }
            set { signBackTime = value; }
        }

        public virtual int LAL_ISLATE
        {
            get { return isLate; }
            set { isLate = value; }
        }

        public virtual int LAL_ISLEAVEEARLY
        {
            get { return isLeaveEarly; }
            set { isLeaveEarly = value; }
        }

        public virtual int LAL_YEAR
        {
            get { return year; }
            set { year = value; }
        }

        public virtual int LAL_MONTH
        {
            get { return month; }
            set { month = value; }
        }

        public virtual int LAL_DAY
        {
            get { return day; }
            set { day = value; }
        }

        public virtual DateTime LAL_TIMESTAMP
        {
            get { return timeStamp; }
            set { timeStamp = value; }
        }

        public virtual string LAL_LOG
        {
            get { return log; }
            set { log = value; }
        }
        #endregion

    }
}
