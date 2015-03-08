using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KjqbService.DB
{
    public class BusinessOperate:DBoperate
    {
        public int InsertIntoEntity(BusinessService b)
        {
            BusinessMessage bm = new BusinessMessage();
            //bm.StarterID = b.StaterID;
            bm.ReceiveID = b.ReceiveID;
            bm.BusinessID = b.BusinessID;
            bm.TimeStamp = b.TimeStamp;
            bm.State = 0;
            bm.IsRead = 0;
            context.BusinessMessages.Add(bm);
            context.SaveChanges();
            return bm.Id;
        }

        public List<BusinessMessage> SendUnRead(long u)
        {
            List<BusinessMessage> lists = new List<BusinessMessage>();
            lists = context.BusinessMessages.Where(m => m.ReceiveID == u && m.IsRead == 0 && m.State == 1).ToList();

            return lists;
        }

        public List<LogMessage> ChangeSharedLogIsRead(long l)
        {
            List<LogMessage> loglists = new List<LogMessage>();
            loglists = context.LogMessages.Where(m => m.ShareUserId == l && m.IsRead == 0).ToList();
            foreach (LogMessage lll in loglists)
            {
                lll.IsRead = 1;
                context.SaveChanges();
            }
            return loglists;
        }
    }
}