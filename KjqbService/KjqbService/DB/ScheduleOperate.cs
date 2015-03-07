using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KjqbService.DB
{
    public class ScheduleOperate:DBoperate
    {
        public int InsertIntoScheduleEntity(ScheduleInService l)
        {
            ScheduleMessage schMess = new ScheduleMessage();

            schMess.ScheduleID = l.ScheduleId;
            schMess.ShareUserID = l.ShareUserId;
            schMess.UserID = l.WriteUserId;
            schMess.TimeStamp = l.TimeStamp;
            schMess.State = 0;
            schMess.IsRead = 0;
            context.ScheduleMessages.Add(schMess);
            context.SaveChanges();
            return schMess.Id;
        }
        public List<ScheduleMessage> SendSharedSchedule(long l)
        {
            List<ScheduleMessage> Schedulelists = new List<ScheduleMessage>();
            Schedulelists = context.ScheduleMessages.Where(m => m.ShareUserID == l && m.State == 0).ToList();
            foreach (ScheduleMessage lll in Schedulelists)
            {
                lll.State = 1;
                context.SaveChanges();
            }
            return Schedulelists;
        }

        public List<ScheduleMessage> SendSharedScheduleUnRead(long l)
        {
            List<ScheduleMessage> loglists = new List<ScheduleMessage>();
            loglists = context.ScheduleMessages.Where(m => m.ShareUserID == l && m.IsRead == 0 && m.State == 1).ToList();

            return loglists;
        }

        public List<ScheduleMessage> ChangeSharedScheduleIsRead(long l)
        {
            List<ScheduleMessage> loglists = new List<ScheduleMessage>();
            loglists = context.ScheduleMessages.Where(m => m.ShareUserID == l && m.IsRead == 0).ToList();
            foreach (ScheduleMessage lll in loglists)
            {
                lll.IsRead = 1;
                context.SaveChanges();
            }
            return loglists;
        } 
    }
}