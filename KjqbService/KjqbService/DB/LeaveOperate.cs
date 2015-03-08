using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KjqbService.DB
{
    public class LeaveOperate:DBoperate
    {

        public int InsertIntoLeaveEntity(LeaveInService l)
        {
            LeaveMessage lm = new LeaveMessage();

            lm.IsRead = 0;
            lm.State = 0;
            lm.TimeStamp = DateTime.Now.Ticks;
            lm.UserId = l.UserId;
            lm.SendUserId = l.SendUserId;
            lm.ExamineOrExamineResult = l.ExamineOrExamineresult;
            lm.LeaveId = l.LeaveId;
            context.LeaveMessages.Add(lm);
            context.SaveChanges();

            return lm.Id;
        }
        public List<LeaveMessage> SendLeaveInfo(long l)
        {
            List<LeaveMessage> loglists = new List<LeaveMessage>();
            loglists = context.LeaveMessages.Where(m => m.UserId == l && m.State == 0).ToList();
            foreach (LeaveMessage lll in loglists)
            {
                lll.State = 1;
                context.SaveChanges();
            }
            return loglists;
        }

        public List<LeaveMessage> SendLeaveInfoUnRead(long l)
        {
            List<LeaveMessage> loglists = new List<LeaveMessage>();
            loglists = context.LeaveMessages.Where(m => m.UserId == l && m.IsRead == 0 && m.State == 1).ToList();

            return loglists;
        }

        public List<LeaveMessage> ChangeLeaveInfoIsRead(long l)
        {
            List<LeaveMessage> loglists = new List<LeaveMessage>();
            loglists = context.LeaveMessages.Where(m => m.UserId == l && m.IsRead == 0).ToList();
            foreach (LeaveMessage lll in loglists)
            {
                lll.IsRead = 1;
                context.SaveChanges();
            }
            return loglists;
        }
    }
}