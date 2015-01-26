using System;
using System.Collections;
using System.Collections.Generic;

namespace ClassLibrary
{
    public class Attendance : IEntity
    {

        private long signStartTime;
        /// <summary>
        /// 标志开始时间
        /// </summary>
        public virtual long SignStartTime
        {
            get { return signStartTime; }
            set { signStartTime = value; }
        }


        private long signEndTime;
        /// <summary>
        /// 标志结束时间
        /// </summary>
        public virtual long SignEndTime
        {
            get { return signEndTime; }
            set { signEndTime = value; }
        }


        /// <summary>
        /// 标志日期
        /// </summary>
        private long signDate;
        public virtual long SignDate
        {
            get { return signDate; }
            set { signDate = value; }
        }


        /// <summary>
        /// 标志年份
        /// </summary>
        private int signYear;

        public virtual int SignYear
        {
            get { return signYear; }
            set { signYear = value; }
        }
        
        
        /// <summary>
        /// 标志月份
        /// </summary>
        private int signMonth;

        public virtual int SignMonth
        {
            get { return signMonth; }
            set { signMonth = value; }
        }
        
        /// <summary>
        /// 标志天
        /// </summary>
        private int signDay;

        public virtual int SignDay
        {
            get { return signDay; }
            set { signDay = value; }
        }
        
        
        /// <summary>
        /// 标志迟到早退
        /// </summary>
        private int lateOrLeaveEarly;

        public virtual int LateOrLeaveEarly
        {
            get { return lateOrLeaveEarly; }
            set { lateOrLeaveEarly = value; }
        }
        
        
        private WkTUser user;

        public virtual WkTUser User
        {
            get { return user; }
            set { user = value; }
        }
        
        
        #region 枚举变量

        public enum lateOrLeaveEarlyEnum
        {
            Normal = 0,
            Late = 1,
            Early = 2, // 正常？
            LateAndEarly = 3 //迟到？
        }

        #endregion
    }
}
