using System;
using System.Collections;

namespace ClassLibrary
{
    public class WkTDept : BaseEntity//部门
    {

        public WkTDept()
        {
            Itself = this;
        }

        private string kdName;

        public virtual string KdName
        {
            get { return kdName; }
            set { kdName = value; }
        }
        private string kdDESC;

        public virtual string KdDESC
        {
            get { return kdDESC; }
            set { kdDESC = value; }
        }
        private long kdOrder;

        public virtual long KdOrder
        {
            get { return kdOrder; }
            set { kdOrder = value; }
        }
        private string kdType;

        public virtual string KdType
        {
            get { return kdType; }
            set { kdType = value; }
        }
        private long kdPid;

        public virtual long KdPid
        {
            get { return kdPid; }
            set { kdPid = value; }
        }

        private  WkTDept itself;

        public virtual WkTDept Itself
        {
            get { return itself; }
            set { itself = value; }
        }
    }
}
