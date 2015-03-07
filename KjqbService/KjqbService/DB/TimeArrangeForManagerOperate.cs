using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KjqbService.DB
{
    public class TimeArrangeForManagerOperate:DBoperate
    {
        public int InsertIntoTFMEntity(TimeArrangeForManagerInService l)
        {
            if (l.ExamineOrExamineresult == 0)
            {
                List<TimeArrangeForManagerMessage> loglists = new List<TimeArrangeForManagerMessage>();
                loglists = context.TimeArrangeForManagerMessages.Where(m => m.TimeArrangeForManagerId == l.TimeArrangeForManagerId && m.ExamineOrExamineResult == 0).ToList();
                if (loglists.Count > 0) //存在则改变推送状态
                {
                    foreach (TimeArrangeForManagerMessage tt in loglists)
                    {
                        tt.State = 0;
                        tt.IsRead = 0;
                        context.SaveChanges();
                    }

                }
                else //是否存在 不存在则插入
                {
                    TimeArrangeForManagerMessage lm = new TimeArrangeForManagerMessage();
                    lm.UserId = l.UserId;
                    lm.SendUserId = l.SendUserId;
                    lm.State = 0;
                    lm.IsRead = 0;
                    lm.TimeArrangeForManagerId = l.TimeArrangeForManagerId;
                    lm.ExamineOrExamineResult = l.ExamineOrExamineresult;
                    lm.TimeStamp = DateTime.Now.Ticks;

                    context.TimeArrangeForManagerMessages.Add(lm);
                    context.SaveChanges();

                }
            }
            else
            {
                TimeArrangeForManagerMessage lm = new TimeArrangeForManagerMessage();
                lm.UserId = l.UserId;
                lm.SendUserId = l.SendUserId;
                lm.State = 0;
                lm.IsRead = 0;
                lm.TimeArrangeForManagerId = l.TimeArrangeForManagerId;
                lm.TimeStamp = DateTime.Now.Ticks;
                lm.ExamineOrExamineResult = l.ExamineOrExamineresult;

                context.TimeArrangeForManagerMessages.Add(lm);
                context.SaveChanges();
            }
                        
            return 1;
        }
        public List<TimeArrangeForManagerMessage> SendTimeArrangeForManager(long l)
        {
            List<TimeArrangeForManagerMessage> loglists = new List<TimeArrangeForManagerMessage>();
            loglists = context.TimeArrangeForManagerMessages.Where(m => m.UserId == l && m.State == 0).ToList();
            foreach (TimeArrangeForManagerMessage lll in loglists)
            {
                lll.State = 1;
                context.SaveChanges();
            }
            return loglists;
        }

        public List<TimeArrangeForManagerMessage> SendTimeArrangeForManagerUnRead(long l)
        {
            List<TimeArrangeForManagerMessage> loglists = new List<TimeArrangeForManagerMessage>();
            loglists = context.TimeArrangeForManagerMessages.Where(m => m.UserId == l && m.IsRead == 0 && m.State == 1).ToList();

            return loglists;
        }

        public List<TimeArrangeForManagerMessage> ChangeTimeArrangeForManagerIsRead(long l)
        {
            List<TimeArrangeForManagerMessage> loglists = new List<TimeArrangeForManagerMessage>();
            loglists = context.TimeArrangeForManagerMessages.Where(m => m.UserId == l && m.IsRead == 0).ToList();
            foreach (TimeArrangeForManagerMessage lll in loglists)
            {
                lll.IsRead = 1;
                context.SaveChanges();
            }
            return loglists;
        }

        
    }
}