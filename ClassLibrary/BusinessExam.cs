using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary
{
    class BusinessExam
    {
        private Business busId;
        public virtual Business BusId
        {
            get { return busId; }
            set { busId = value; }
        }

        private WkTDept kd_Id;
        public virtual WkTDept KD_Id
        {
            get { return kd_Id; }
            set { kd_Id = value; }
        }

        private int examState;
        public virtual int ExamState
        {
            get { return examState; }
            set { examState = value; }
        }
    }
}
