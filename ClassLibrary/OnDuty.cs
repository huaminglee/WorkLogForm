using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary
{
    //排班实体
   public class OnDuty:IEntity
    {

        private long id;
        public virtual long Id
        {
            get { return id; }
            set { id = value; }
        }

        private WkTUser ku_Id;//定义用户实体,这个id对应的安排值班的人（部门负责人）

        public virtual WkTUser Ku_Id
        {
            get { return ku_Id; }
            set { ku_Id = value; }
        }

      
     

        WkTUser staff_Id;//值班人员（一个班可能存在多个人员值）

        public virtual WkTUser Staff_Id
        {
            get { return staff_Id; }
            set { staff_Id = value; }
        }



        private long time;//值班时间
        public virtual long Time
        {
            get { return time; }
            set { time = value; }
        }


        private int times;//值班次数
        public virtual int Times
        {
            get { return times; }
            set { times = value; }
        }


        private int type;//值班类型（分为：白班和夜班，在ONDUTYTIME表中记录着白班和夜班分别对应得值班时间）
        public virtual int Type
        {
            get { return type; }
            set { type = value; }
        }

        public enum DutyType
        {
            day = 0,
            night = 1
        }

       

    }
}
