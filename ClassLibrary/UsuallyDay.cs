using System;
using System.Collections;
using System.Collections.Generic;

namespace ClassLibrary
{

    /// <summary>
    /// 日常工作日作息时间
    /// </summary>
    public class UsuallyDay : IEntity
    {

        /// <summary>
        /// 安排名称
        /// </summary>
        private string name;
        public virtual string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// 生效时间
        /// </summary>
        private long startTime;
        public virtual long StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        /// <summary>
        /// 工作日上班开始时间
        /// </summary>
        private long workTimeStart;

        public virtual long WorkTimeStart
        {
            get { return workTimeStart; }
            set { workTimeStart = value; }
        }

        /// <summary>
        /// 工作日上班结束时间
        /// </summary>
        private long workTimeEnd;

        public virtual long WorkTimeEnd
        {
            get { return workTimeEnd; }
            set { workTimeEnd = value; }
        }

        /// <summary>
        /// 工作还是假期
        /// </summary>
        private string workDay;

        public virtual string WorkDay
        {
            get { return workDay; }
            set { workDay = value; }
        }

        #region 枚举变量

        public enum workDayEnum
        {
            WorkDay = '1',
            Holiday = '0'
        }

        #endregion
    }
}
