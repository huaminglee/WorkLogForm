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
        #region 分享日志推送
        [OperationContract]
        bool SaveInLogListInService(LogInService log);

        [OperationContract]
        List<LogInService> SearchShareLog(int Id);

        [OperationContract]
        void SetShareLogIsRead(int Id);

        [OperationContract]
        List<LogInService> SearchShareLogUnRead(int Id);
        #endregion


        #region 分享日程推送
        [OperationContract]
        bool SaveInScheduleListInService(ScheduleInService log);

        [OperationContract]
        List<ScheduleInService> SearchShareSchedule(int Id);

        [OperationContract]
        void SetShareScheduleIsRead(int Id);

        [OperationContract]
        List<ScheduleInService> SearchShareScheduleUnRead(int Id);
        #endregion

        #region 评论日志推送
        [OperationContract]
        bool SaveInCommentListInService(CommentInService log);

        [OperationContract]
        List<CommentInService> SearchCommentlog(int Id);

        [OperationContract]
        void SetCommentLogIsRead(int Id);

        [OperationContract]
        List<CommentInService> SearchCommentlogUnRead(int Id);
        
        #endregion

        #region 值班审批推送
        [OperationContract]
        bool SaveInTimeArrangeForManagerInService(TimeArrangeForManagerInService log);

        [OperationContract]
        List<TimeArrangeForManagerInService> SearchTimeArrangeForManager(int Id);

        [OperationContract]
        void SetTimeArrangeForManagerIsRead(int Id);

        [OperationContract]
        List<TimeArrangeForManagerInService> SearchTimeArrangeForManagerUnRead(int Id);
        #endregion

        #region 出差审批推送
        [OperationContract]
        bool SaveInBusinessListInService(BusinessService bs);

        #endregion
        #region 请假推送


        [OperationContract]
        bool SaveInLeaveInfoInService(LeaveInService log);

        [OperationContract]
        List<LeaveInService> SearchLeaveInfo(int Id);

        [OperationContract]
        void SetLeaveInfoIsRead(int Id);

        [OperationContract]
        List<LeaveInService> SearchLeaveInfoUnRead(int Id);



        #endregion


    }
}
