using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KjqbService
{
    public class BusinessService
    {
        //private long staterID;

        //public long StaterID
        //{
        //    get { return staterID; }
        //    set { staterID = value; }
        //}


        private long receiveID;

        public long ReceiveID
        {
            get { return receiveID; }
            set { receiveID = value; }
        }


        private long businessID;

        public long BusinessID
        {
            get { return businessID; }
            set { businessID = value; }
        }


        private long timeStamp;

        public long TimeStamp
        {
            get { return timeStamp; }
            set { timeStamp = value; }
        }

        private int type;

        public int Type//0：审批 1：退回  2：撤销 3：通过
        {
            get { return type; }
            set { type = value; }
        }
    }
}