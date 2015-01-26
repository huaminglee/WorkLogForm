using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary
{
    public class LeaveManage : IEntity
    {

        private WkTUser ku_Id;//定义用户实体,这个id对应的是请假人的id号

        public virtual WkTUser Ku_Id
        {
            get { return ku_Id; }
            set { ku_Id = value; }
        }

        private long startTime;//请假开始时间

        public virtual long StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        private long endTime;//请假结束时间

        public virtual long EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        private string leaveType;//请假类型

        public virtual string LeaveType
        {
            get { return leaveType; }
            set { leaveType = value; }
        }

        private string leaveReason;//请假原因

        public virtual string LeaveReason
        {
            get { return leaveReason; }
            set { leaveReason = value; }
        }

        private string leaveResult;//通过状态（通过/没通过;在数据库中保存为1 /0）

        public virtual string LeaveResult
        {
            get { return leaveResult; }
            set { leaveResult = value; }
        }

        private string leaveStage;//审核阶段   含义说明（0：未审核阶段，1：负责人审核阶段，2：副院长审核阶段，3：院长审核阶段）

        public virtual string LeaveStage
        {
            get { return leaveStage; }
            set { leaveStage = value; }
        }

        private IList<WkTUser> leaveChargeId;//指定临时负责人（是一个用户ID）

        public virtual IList<WkTUser> LeaveChargeId
        {
            get { return leaveChargeId; }
            set { leaveChargeId = value; }
        }
     
    }
}
