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
            bm.Type = b.Type;
            context.BusinessMessages.Add(bm);
            context.SaveChanges();
            return bm.Id;
        }

        public List<BusinessMessage> SendBusinessUnRead(long u)
        {
            List<BusinessMessage> lists = new List<BusinessMessage>();
            lists = context.BusinessMessages.Where(m => m.ReceiveID == u && m.IsRead == 0 && m.State == 1).ToList();

            return lists;
        }

        public List<BusinessMessage> SendBusinessInfo(long u)
        {
            List<BusinessMessage> loglists = new List<BusinessMessage>();
            loglists = context.BusinessMessages.Where(m => m.ReceiveID == u && m.State == 0).ToList();
            foreach (BusinessMessage lll in loglists)
            {
                lll.State = 1;
                context.SaveChanges();
            }
            return loglists;
        
        }


        public List<BusinessMessage> ChangeIsRead(long u)
        {
            List<BusinessMessage> lists = new List<BusinessMessage>();
            lists = context.BusinessMessages.Where(m => m.ReceiveID == u && m.IsRead == 0).ToList();
            foreach (BusinessMessage lll in lists)
            {
                lll.IsRead = 1;
                context.SaveChanges();
            }
            return lists;
        }
    }
}