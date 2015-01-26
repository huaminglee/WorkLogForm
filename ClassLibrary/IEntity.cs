using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary
{
    public class IEntity : BaseEntity
    {

        #region 成员
        private int state;
        private long timeStamp;
        #endregion
        

        #region 属性
        
        public virtual long TimeStamp
        {
            get { return timeStamp; }
            set { timeStamp = value; }
        }
        public virtual int State
        {
            get { return state; }
            set { state = value; }
        }
        #endregion

        #region 枚举变量

        public enum stateEnum
        {
            Normal = 0,
            Deleted = 1 
        }

        #endregion

    }
}
