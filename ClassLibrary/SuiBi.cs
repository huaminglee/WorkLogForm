using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary
{
    public class SuiBi : IEntity
    {
        private string contents;
        public virtual string Contents
        {
            get { return contents; }
            set { contents = value; }
        }


        private WkTUser wkTUserId;
        public virtual WkTUser WkTUserId
        {
            get { return wkTUserId; }
            set { wkTUserId = value; }
        }


        private long writeTime;
        public virtual long WriteTime
        {
            get { return writeTime; }
            set { writeTime = value; }
        }
    }
}
