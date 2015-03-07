using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace KjqbService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IService1”。
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        bool SaveInLogListInService(LogInService log);

        [OperationContract]
        List<LogInService> SearchShareLog(int Id);

        [OperationContract]
        void SetShareLogIsRead(int Id);

        [OperationContract]
        List<LogInService> SearchShareLogUnRead(int Id);

        [OperationContract]
        bool SaveInScheduleListInService(ScheduleInService log);

        [OperationContract]
        List<ScheduleInService> SearchShareSchedule(int Id);

        [OperationContract]
        void SetShareScheduleIsRead(int Id);

        [OperationContract]
        List<ScheduleInService> SearchShareScheduleUnRead(int Id);

    }
}
