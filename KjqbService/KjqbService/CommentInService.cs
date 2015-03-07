using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KjqbService
{
    public class CommentInService
    {
        private long logUserId;
        public long LogUserId
        {
            get { return logUserId; }
            set { logUserId = value; }
        }

        private string commentUserName;
        public string CommentUserName
        {
            get { return commentUserName; }
            set { commentUserName = value; }
        }

        private long logId;
        public long LogId
        {
            get { return logId; }
            set { logId = value; }
        }

        private long timeStamp;
        public long TimeStamp
        {
            get { return timeStamp; }
            set { timeStamp = value; }
        }


    }
}