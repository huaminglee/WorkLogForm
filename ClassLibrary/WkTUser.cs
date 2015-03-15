using System;
using System.Collections;
using System.Collections.Generic;

namespace ClassLibrary
{
    public class WkTUser : BaseEntity
    {

        public WkTUser()
        {
            Itself = this;
        }

        private string kuLid;

        public virtual string KuLid
        {
            get { return kuLid; }
            set { kuLid = value; }
        }
        private string kuName;

        public virtual string KuName
        {
            get { return kuName; }
            set { kuName = value; }
        }
        private string kuPassWD;

        public virtual string KuPassWD
        {
            get { return kuPassWD; }
            set { kuPassWD = value; }
        }
        private string kuRegDate;

        public virtual string KuRegDate
        {
            get { return kuRegDate; }
            set { kuRegDate = value; }
        }
        private string kuStatus;

        public virtual string KuStatus
        {
            get { return kuStatus; }
            set { kuStatus = value; }
        }
        private string kuSex;

        public virtual string KuSex
        {
            get { return kuSex; }
            set { kuSex = value; }
        }
        private string kuBirthday;

        public virtual string KuBirthday
        {
            get { return kuBirthday; }
            set { kuBirthday = value; }
        }
        private string kuEmail;

        public virtual string KuEmail
        {
            get { return kuEmail; }
            set { kuEmail = value; }
        }
        private string kuPhone;

        public virtual string KuPhone
        {
            get { return kuPhone; }
            set { kuPhone = value; }
        }
        private string kuCompany;

        public virtual string KuCompany
        {
            get { return kuCompany; }
            set { kuCompany = value; }
        }
        private int kuLevel;

        public virtual int KuLevel
        {
            get { return kuLevel; }
            set { kuLevel = value; }
        }
        private string kuLtime;

        public virtual string KuLtime
        {
            get { return kuLtime; }
            set { kuLtime = value; }
        }
        private string kuRtime;

        public virtual string KuRtime
        {
            get { return kuRtime; }
            set { kuRtime = value; }
        }
        private string kuLastAddr;

        public virtual string KuLastAddr
        {
            get { return kuLastAddr; }
            set { kuLastAddr = value; }
        }
        private int kuOnline;

        public virtual int KuOnline
        {
            get { return kuOnline; }
            set { kuOnline = value; }
        }
        private int kuLtimes;

        public virtual int KuLtimes
        {
            get { return kuLtimes; }
            set { kuLtimes = value; }
        }
        private int kuLimit;

        public virtual int KuLimit
        {
            get { return kuLimit; }
            set { kuLimit = value; }
        }
        private string kuStyle;

        public virtual string KuStyle
        {
            get { return kuStyle; }
            set { kuStyle = value; }
        }
        private string kuAutoShow;

        public virtual string KuAutoShow
        {
            get { return kuAutoShow; }
            set { kuAutoShow = value; }
        }
        private string kuBindType;

        public virtual string KuBindType
        {
            get { return kuBindType; }
            set { kuBindType = value; }
        }
        private string kuBindAddr;

        public virtual string KuBindAddr
        {
            get { return kuBindAddr; }
            set { kuBindAddr = value; }
        }
        private string kuQuestion;

        public virtual string KuQuestion
        {
            get { return kuQuestion; }
            set { kuQuestion = value; }
        }
        private string kuAnswer;

        public virtual string KuAnswer
        {
            get { return kuAnswer; }
            set { kuAnswer = value; }
        }
        private string kuAUTOENTER;

        public virtual string KuAUTOENTER
        {
            get { return kuAUTOENTER; }
            set { kuAUTOENTER = value; }
        }
        private WkTDept kdid;

        public virtual WkTDept Kdid
        {
            get { return kdid; }
            set { kdid = value; }
        }
        private IList<WkTRole> userRole;

        public virtual IList<WkTRole> UserRole
        {
            get { return userRole; }
            set { userRole = value; }
        }


        private WkTUser itself;

        public virtual WkTUser Itself
        {
            get { return itself; }
            set { itself = value; }
        }


        private int dutyTimes;
        public virtual int DutyTimes
        {
            get { return dutyTimes; }
            set { dutyTimes = value; }
        }



    }
}
