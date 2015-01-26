using System;
using System.Collections;
using System.Collections.Generic;

namespace ClassLibrary
{
    public class StaffLog : IEntity
    {

        private string content;

        public virtual string Content
        {
            get { return content; }
            set { content = value; }
        }
        private WkTUser staff;

        public virtual WkTUser Staff
        {
            get { return staff; }
            set { staff = value; }
        }
        private long writeTime;

        public virtual long WriteTime
        {
            get { return writeTime; }
            set { writeTime = value; }
        }

        private IList<WkTUser> sharedStaffs;

        public virtual IList<WkTUser> SharedStaffs
        {
            get { return sharedStaffs; }
            set { sharedStaffs = value; }
        }

        private IList<Comments> comments;

        public virtual IList<Comments> Comments
        {
            get { return comments; }
            set { comments = value; }
        }
        
    }
}
