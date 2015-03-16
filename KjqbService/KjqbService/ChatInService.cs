using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KjqbService
{
    public class ChatInService
    {
        private long sendUserId;
        public long SendUserId
        {
            get { return sendUserId; }
            set { sendUserId = value; }
        }
        
        private long receiveUserId;
        public long ReceiveUserId
        {
            get { return receiveUserId; }
            set { receiveUserId = value; }
        }
        
        private string chatContent;
        public string ChatContent
        {
            get { return chatContent; }
            set { chatContent = value; }
        }
        
        private DateTime timeStamp;
        public DateTime TimeStamp
        {
            get { return timeStamp; }
            set { timeStamp = value; }
        }
    }
}