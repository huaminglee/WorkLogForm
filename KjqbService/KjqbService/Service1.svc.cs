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
        
    }
}
