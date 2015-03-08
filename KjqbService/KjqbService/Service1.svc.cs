using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Timers;
using KjqbService.DB;
namespace KjqbService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Service1”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 Service1.svc 或 Service1.svc.cs，然后开始调试。
    public class Service1 : IService1
    {

        KjqbService.DB.LogOperate lop = new LogOperate();
        KjqbService.DB.ScheduleOperate sop = new ScheduleOperate();
        KjqbService.DB.CommentOperate cop = new CommentOperate();
        KjqbService.DB.TimeArrangeForManagerOperate top = new TimeArrangeForManagerOperate();
        KjqbService.DB.BusinessOperate bop = new BusinessOperate();
        KjqbService.DB.LeaveOperate leop = new LeaveOperate();
        #region 分享日志推送
        public bool SaveInLogListInService(LogInService log)
        {
            lop.InsertIntoEntity(log);
            return true;
        }

        public List<LogInService> SearchShareLog(int Id)
        {
            List<LogInService> l = new List<LogInService>();

            List<LogMessage> lm = lop.SendSharedLog(Id);

            foreach (LogMessage lo in lm)
            {
                LogInService ll = new LogInService();
                ll.LogId = (long)lo.LogId;
                ll.ShareUserId = (long)lo.ShareUserId;
                ll.TimeStamp = (long)lo.TimeStamp;
                ll.WriteUserId = (long)lo.UserId;
                l.Add(ll);
            }

            return l;
        }

        public void SetShareLogIsRead(int Id)
        {
            lop.ChangeSharedLogIsRead(Id);
        }

        public List<LogInService> SearchShareLogUnRead(int Id)
        {
            List<LogInService> l = new List<LogInService>();

            List<LogMessage> lm = lop.SendSharedLogUnRead(Id);

            foreach (LogMessage lo in lm)
            {
                LogInService ll = new LogInService();
                ll.LogId = (long)lo.LogId;
                ll.ShareUserId = (long)lo.ShareUserId;
                ll.TimeStamp = (long)lo.TimeStamp;
                ll.WriteUserId = (long)lo.UserId;
                l.Add(ll);
            }

            return l;

        }
        #endregion

        #region 分享安排日程推送

        public bool SaveInScheduleListInService(ScheduleInService log)
        {
            sop.InsertIntoScheduleEntity(log);
            return true;
        }


        public List<ScheduleInService> SearchShareSchedule(int Id)
        {
            List<ScheduleInService> l = new List<ScheduleInService>();

            List<ScheduleMessage> lm = sop.SendSharedSchedule(Id);

            foreach (ScheduleMessage lo in lm)
            {
                ScheduleInService ll = new ScheduleInService();
                ll.ScheduleId = (long)lo.ScheduleID;
                ll.ShareUserId = (long)lo.ShareUserID;
                ll.TimeStamp = (long)lo.TimeStamp;
                ll.WriteUserId = (long)lo.UserID;
                l.Add(ll);
            }

            return l;

        }

        public void SetShareScheduleIsRead(int Id)
        {
            sop.ChangeSharedScheduleIsRead(Id);
        }

        public List<ScheduleInService> SearchShareScheduleUnRead(int Id)
        {

            List<ScheduleInService> l = new List<ScheduleInService>();

            List<ScheduleMessage> lm = sop.SendSharedScheduleUnRead(Id);

            foreach (ScheduleMessage lo in lm)
            {
                ScheduleInService ll = new ScheduleInService();
                ll.ScheduleId = (long)lo.ScheduleID;
                ll.ShareUserId = (long)lo.ShareUserID;
                ll.TimeStamp = (long)lo.TimeStamp;
                ll.WriteUserId = (long)lo.UserID;
                l.Add(ll);
            }

            return l;
        }
        #endregion

        #region 评论日志推送

        public bool SaveInCommentListInService(CommentInService log)
        {
            cop.InsertIntoCommentEntity(log);
            return true;
        }

        public List<CommentInService> SearchCommentlog(int Id)
        {
            List<CommentInService> l = new List<CommentInService>();

            List<CommentMessage> lm = cop.SendComment(Id);

            foreach (CommentMessage lo in lm)
            {
                CommentInService ll = new CommentInService();
                ll.LogUserId = (long)lo.UserId;
                ll.CommentUserName = lo.CommentUserName;
                ll.TimeStamp = (long)lo.TimeStamp;
                ll.LogId = (long)lo.LogId;
                l.Add(ll);
            }

            return l;

        }
        public void SetCommentLogIsRead(int Id)
        {
            cop.ChangeCommnentLogIsRead(Id);
        }

        public List<CommentInService> SearchCommentlogUnRead(int Id)
        {
            List<CommentInService> l = new List<CommentInService>();

            List<CommentMessage> lm = cop.SendCommentLogUnRead(Id);

            foreach (CommentMessage lo in lm)
            {
                CommentInService ll = new CommentInService();
                ll.LogUserId = (long)lo.UserId;
                ll.CommentUserName = lo.CommentUserName;
                ll.TimeStamp = (long)lo.TimeStamp;
                ll.LogId = (long)lo.LogId;
                l.Add(ll);
            }
            return l;
        }

        #endregion

        #region 值班审批推送

       public bool SaveInTimeArrangeForManagerInService(TimeArrangeForManagerInService log)
       {
           top.InsertIntoTFMEntity(log);
           return true;
       }

        public List<TimeArrangeForManagerInService> SearchTimeArrangeForManager(int Id)
        {

            List<TimeArrangeForManagerInService> l = new List<TimeArrangeForManagerInService>();

            List<TimeArrangeForManagerMessage> lm = top.SendTimeArrangeForManager(Id);

            foreach (TimeArrangeForManagerMessage lo in lm)
            {
                TimeArrangeForManagerInService ll = new TimeArrangeForManagerInService();
                ll.TimeArrangeForManagerId = (long)lo.TimeArrangeForManagerId;
                ll.UserId = (long)lo.UserId;
                ll.SendUserId = (long)lo.SendUserId;
                ll.ExamineOrExamineresult = (int)lo.ExamineOrExamineResult;
                
                l.Add(ll);
            }

            return l;
        }

       public  void SetTimeArrangeForManagerIsRead(int Id)
       {
           top.ChangeTimeArrangeForManagerIsRead(Id);
       }

       public List<TimeArrangeForManagerInService> SearchTimeArrangeForManagerUnRead(int Id)
       {
           List<TimeArrangeForManagerInService> l = new List<TimeArrangeForManagerInService>();

           List<TimeArrangeForManagerMessage> lm = top.SendTimeArrangeForManagerUnRead(Id);

           foreach (TimeArrangeForManagerMessage lo in lm)
           {
               TimeArrangeForManagerInService ll = new TimeArrangeForManagerInService();
               ll.TimeArrangeForManagerId = (long)lo.TimeArrangeForManagerId;
               ll.UserId = (long)lo.UserId;
               ll.SendUserId = (long)lo.SendUserId;
               ll.ExamineOrExamineresult = (int)lo.ExamineOrExamineResult;

               l.Add(ll);
           }

           return l;
       
       }
       


        #endregion

       #region  出差审批推送
       public bool SaveInBusinessListInService(BusinessService bs)
       {
           bop.InsertIntoEntity(bs);
           return true;
       }


       public List<BusinessService> SearchBusinessInfo(long Id)
       {
           List<BusinessService> l = new List<BusinessService>();

           List<BusinessMessage> lm = bop.SendBusinessInfo(Id);

           foreach (BusinessMessage lo in lm)
           {
               BusinessService ll = new BusinessService();
               ll.ReceiveID = (long)lo.ReceiveID;
               ll.Type = (int)lo.Type;
               ll.BusinessID = (long)lo.BusinessID;
               l.Add(ll);
           }

           return l;
       }
       public void SetBusinessInfoIsRead(long Id)
       {
           bop.ChangeIsRead(Id);
       }
       public List<BusinessService> SearchBusinessInfoUnRead(long Id)
       {
           List<BusinessService> l = new List<BusinessService>();

           List<BusinessMessage> lm = bop.SendBusinessUnRead(Id);

           foreach (BusinessMessage lo in lm)
           {
               BusinessService ll = new BusinessService();
               ll.ReceiveID = (long)lo.ReceiveID;
               ll.Type = (int)lo.Type;
               ll.BusinessID = (long)lo.BusinessID;
               l.Add(ll);
           }

           return l;
       
       }


       #endregion


       #region 请假推送


       public bool SaveInLeaveInfoInService(LeaveInService log)
       {
           leop.InsertIntoLeaveEntity(log);
           return true;
       }

       public List<LeaveInService> SearchLeaveInfo(int Id)
       {
           List<LeaveInService> l = new List<LeaveInService>();

           List<LeaveMessage> lm = leop.SendLeaveInfo(Id);

           foreach (LeaveMessage lo in lm)
           {
               LeaveInService ll = new LeaveInService();
               ll.LeaveId = (long)lo.LeaveId;
               ll.UserId = (long)lo.UserId;
               ll.SendUserId = (long)lo.SendUserId;
               ll.ExamineOrExamineresult = (int)lo.ExamineOrExamineResult;

               l.Add(ll);
           }

           return l;
       
       }

       public void SetLeaveInfoIsRead(int Id)
       {
           leop.ChangeLeaveInfoIsRead(Id);
       }

       public List<LeaveInService> SearchLeaveInfoUnRead(int Id)
       {

           List<LeaveInService> l = new List<LeaveInService>();

           List<LeaveMessage> lm = leop.SendLeaveInfoUnRead(Id);

           foreach (LeaveMessage lo in lm)
           {
               LeaveInService ll = new LeaveInService();
               ll.LeaveId = (long)lo.LeaveId;
               ll.UserId = (long)lo.UserId;
               ll.SendUserId = (long)lo.SendUserId;
               ll.ExamineOrExamineresult = (int)lo.ExamineOrExamineResult;

               l.Add(ll);
           }

           return l;
       }




        #endregion
    }
}
