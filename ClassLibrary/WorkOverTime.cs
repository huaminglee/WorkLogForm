using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary
{
    //加班实体
    public class WorkOverTime:IEntity
    {
        private WkTUser ku_Id;//定义用户实体,这个id对应的安排加班的人（部门负责人）

        public virtual WkTUser Ku_Id
        {
            get { return ku_Id; }
            set { ku_Id = value; }
        }

        private WkTUser workManId;//指定加班人员

        public virtual WkTUser WorkManId
        {
            get { return workManId; }
            set { workManId = value; }
        }

        

        private long startTime;//加班开始时间

        public virtual long StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        private long endTime;//加班结束时间

        public virtual long EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        private string workContent;//加班内容

        public virtual string WorkContent
        {
            get { return workContent; }
            set { workContent = value; }
        }

        private long dayTime;//本个工作日加班时长

        public virtual long DayTime
        {
            get { return dayTime; }
            set { dayTime = value; }
        }
        private long date;//加班日期

        public virtual long Date
        {
            get { return date; }
            set { date = value; }
        }

        private WorkOverTime itself;

        public virtual WorkOverTime Itself
        {
            get { return itself; }
            set { itself = this; }
        }
        //private long monthTime;//本月加班时长

        //public virtual long MonthTime
        //{
        //    get { return monthTime; }
        //    set { monthTime = value; }
        //}



    }
}
