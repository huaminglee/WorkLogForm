using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary
{
    public class TimeArrangeForManager : BaseEntity
    {

        private long startime;
        public virtual long Startime
        {
            get { return startime; }
            set { startime = value; }
        }

        private long endtime;
        public virtual long Endtime
        {
            get { return endtime; }
            set { endtime = value; }
        }

        private WkTUser userId;
        public virtual WkTUser UserId
        {
            get { return userId; }
            set { userId = value; }
        }

    }
}
