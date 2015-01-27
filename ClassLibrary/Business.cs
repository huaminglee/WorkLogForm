using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary
{
    public class Business:IEntity
    {
        private WkTUser ku_Id;//出差安排者（领导）（是一个用户ID）

        public virtual WkTUser Ku_Id
        {
            get { return ku_Id; }
            set { ku_Id = value; }
        }
       
        //private IList <WkTUser> businessId;//出差人员的id(可以指定多名出差人员，因此是个人员列表)

        //public virtual IList <WkTUser> BusinessId
        //{
        //    get { return businessId; }
        //    set { businessId = value; }
        //}

        private long startTime;//出差开始时间

        public virtual long StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        private long endTime;//出差结束时间

        public virtual long EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        private string businessReason;//出差事由

        public virtual string BusinessReason
        {
            get { return businessReason; }
            set { businessReason = value; }
        }
        private string businessDestination;//出差地点
        public virtual string BusinessDestination
        {
            get { return businessDestination; }
            set { businessDestination = value; }
        }

        private string businessNote;//出差备注
        public virtual string BusinessNote
        {
            get { return businessNote; }
            set { businessNote = value; }
        }

        private int passExam;//审核审批状态
        public virtual int PassExam
        {
            get { return passExam; }
            set { passExam = value; }
        }


        private int waitingNum;//等待审核的员工个数
        public virtual int WaitingNum
        {
            get { return waitingNum; }
            set { waitingNum = value; }
        }

        private string refuseReason;//被退回时记录原因
        public virtual  string RefuseReason
        {
            get { return refuseReason; }
            set { refuseReason = value; }
        }

        private WkTUser boss;//审批指定领导
        public virtual WkTUser Boss
        {
            get { return boss; }
            set { boss = value; }
        }

        private IList<BusinessEmployee> businessEmployee;

        public virtual IList<BusinessEmployee> BusinessEmployee
        {
            get { return businessEmployee; }
            set { businessEmployee = value; }
        }
        
        public enum ExamState{waiting,pass,npass,redo};
    }
}
