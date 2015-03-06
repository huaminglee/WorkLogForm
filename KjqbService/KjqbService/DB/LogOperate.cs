using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KjqbService.DB
{
    public class LogOperate:DBoperate
    {
        public int InsertIntoEntity(LogInService l)
        {
            LogMessage lm = new LogMessage();

            lm.LogId = l.LogId;
            lm.ShareUserId = l.ShareUserId;
            lm.UserId = l.WriteUserId;
            lm.TimeStamp = l.TimeStamp;
            lm.State = 0;
            lm.IsRead = 0;
            context.LogMessages.Add(lm);
            context.SaveChanges();
            return lm.Id;
        }
        public List<LogMessage> SendSharedLog(long l)
        {
            List<LogMessage> loglists = new List<LogMessage>();
            loglists = context.LogMessages.Where(m => m.ShareUserId == l&&m.State == 0).ToList();
            foreach (LogMessage lll in loglists)
            {
                lll.State = 1;
                context.SaveChanges();
            }
            return loglists;
        }

        public List<LogMessage> SendSharedLogUnRead(long l)
        {
            List<LogMessage> loglists = new List<LogMessage>();
            loglists = context.LogMessages.Where(m => m.ShareUserId == l && m.IsRead == 0&&m.State == 1).ToList();
           
            return loglists;
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