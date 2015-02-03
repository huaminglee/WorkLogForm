using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary
{
    //排班实体
   public class OnDutyTable:IEntity
    {
        private WkTUser daiBanID;
        public virtual WkTUser DaiBanID
        {
            get { return daiBanID; }
            set { daiBanID = value; }
        }

        private WkTUser baiBanID;
        public virtual WkTUser BaiBanID
        {
            get { return baiBanID; }
            set { baiBanID = value; }
        }

        private WkTUser yeBanID;
        public virtual WkTUser YeBanID
        {
            get { return yeBanID; }
            set { yeBanID = value; }
        }

        private TimeArrangeForManager tFMId;
        public virtual TimeArrangeForManager TFMId
        {
            get { return tFMId; }
            set { tFMId = value; }
        }

        private long time;
        public virtual long Time
        {
            get { return time; }
            set { time = value; }
        }

        private int type;
        public virtual int Type
        {
            get { return type; }
            set { type = value; }
        }
    }
}
