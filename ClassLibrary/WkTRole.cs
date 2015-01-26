using System;
using System.Collections;

namespace ClassLibrary
{
    public class WkTRole : BaseEntity
    {

        private string krName;

        public virtual string KrName
        {
            get { return krName; }
            set { krName = value; }
        }
        private string krDESC;

        public virtual string KrDESC
        {
            get { return krDESC; }
            set { krDESC = value; }
        }
        private string krDefault;

        public virtual string KrDefault
        {
            get { return krDefault; }
            set { krDefault = value; }
        }
        private string krShare;

        public virtual string KrShare
        {
            get { return krShare; }
            set { krShare = value; }
        }
        private long krOrder;

        public virtual long KrOrder
        {
            get { return krOrder; }
            set { krOrder = value; }
        }
        private long krPid;

        public virtual long KrPid
        {
            get { return krPid; }
            set { krPid = value; }
        }
        private WkTDept kdid;

        public virtual WkTDept Kdid
        {
            get { return kdid; }
            set { kdid = value; }
        }

    }
}
