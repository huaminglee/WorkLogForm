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

        public List<BusinessMessage> SendUnRead(long u)
        {
            List<BusinessMessage> lists = new List<BusinessMessage>();
            lists = context.BusinessMessages.Where(m => m.ReceiveID == u && m.IsRead == 0 && m.State == 1).ToList();

            return lists;
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