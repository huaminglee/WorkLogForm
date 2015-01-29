using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary
{
    class Wktuser_M_Dept : IEntity
    {
        private WkTUser wktuserId;

        public virtual WkTUser WktuserId
        {
            get { return wktuserId; }
            set { wktuserId = value; }
        }
        private WkTDept deptId;

        public virtual WkTDept DeptId
        {
            get { return deptId; }
            set { deptId = value; }
        }

      



    }
}
