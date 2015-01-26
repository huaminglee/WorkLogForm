using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary
{
    public  class BusinessEmployee:IEntity
    {
        private Business businessId;
        public virtual Business BusinessId
        {
            get { return businessId; }
            set { businessId = value; }
        }

        private WkTUser employeeId;
        public virtual  WkTUser EmployeeId
        {
            get { return employeeId; }
            set { employeeId = value; }
        }

        private int passExam;

        public virtual  int PassExam
        {
          get { return passExam; }
          set { passExam = value; }
        }


        public enum ExamState { waiting, pass, npass };
    }
}
