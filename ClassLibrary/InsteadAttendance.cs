using System;
using System.Collections;
using System.Collections.Generic;

namespace ClassLibrary
{
    public class InsteadAttendance : IEntity
    {
        private long signStartDate;

        public virtual long SignStartDate
        {
            get { return signStartDate; }
            set { signStartDate = value; }
        }
        private long signEndDate;

        public virtual long SignEndDate
        {
            get { return signEndDate; }
            set { signEndDate = value; }
        }
        private int signType;

        public virtual int SignType
        {
            get { return signType; }
            set { signType = value; }
        }
        private string signReason;

        public virtual string SignReason
        {
            get { return signReason; }
            set { signReason = value; }
        }
        private int signExamine;

        public virtual int SignExamine
        {
            get { return signExamine; }
            set { signExamine = value; }
        }
        private WkTUser insteadUser;

        public virtual WkTUser InsteadUser
        {
            get { return insteadUser; }
            set { insteadUser = value; }
        }

        private WkTUser signUser;

        public virtual WkTUser SignUser
        {
            get { return signUser; }
            set { signUser = value; }
        }
        #region 枚举变量
        
        public enum SignTypeEnum
        {
            SignIn = 0,
            SignOut = 1,
            SignInAndOut = 2
        }
        public enum ExamineEnum
        {
            None = 0,
            Agree = 1,
            NotAgree = 2
        }
        #endregion
    }
}
