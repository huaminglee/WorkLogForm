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
        public bool SaveInLogListInService(LogInService log)
        {
            lop.InsertIntoEntity(log);
            return true;
        }


       public List<LogInService> SearchShareLog(int Id)
        {
            List<LogInService> l = new List<LogInService>();
            
           List<LogMessage> lm =  lop.SearchLog(Id);

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

       
       
        
    }
}
