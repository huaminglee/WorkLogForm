using System;
using System.Collections;
using System.Collections.Generic;

namespace ClassLibrary
{
    public class Hobby : IEntity
    {
        private WkTUser staff;
        public virtual WkTUser Staff
        {
            get { return staff; }
            set { staff = value; }
        }

        private IList<WkTUser> sharedStaffs;
        public virtual IList<WkTUser> SharedStaffs
        {
            get { return sharedStaffs; }
            set { sharedStaffs = value; }
        }

        private int typeFlag;
        public virtual int TypeFlag
        {
            get { return typeFlag; }
            set { typeFlag = value; }
        }
        private string visibleFlag;
        public virtual string VisibleFlag
        {
            get { return visibleFlag; }
            set { visibleFlag = value; }
        }
        #region 枚举变量
        public enum hobbyTypeEnum
        {
            RiZhi = 0,
            RiCheng = 1,
            SuiBi = 2
        }
        public enum visibleTypeEnum
        {
            /// <summary>
            /// 本部门上级
            /// </summary>
            BS = 0,
            /// <summary>
            /// 本部门同级
            /// </summary>
            BT = 1,
            /// <summary>
            /// 本部门下级
            /// </summary>
            BX = 2,
            /// <summary>
            /// 本部门上级
            /// </summary>
            WS = 3,
            /// <summary>
            /// 本部门同级
            /// </summary>
            WT = 4,
            /// <summary>
            /// 本部门下级
            /// </summary>
            WX = 5
        }
        #endregion
    }
}
