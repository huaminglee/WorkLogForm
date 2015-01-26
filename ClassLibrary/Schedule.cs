using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary
{
    public class Schedule : BaseEntity
    {
         #region 构造函数
        public Schedule()
        { }

        public Schedule(long KU_ID, string LSD_CONTENT, string LSD_TITLE, int LSD_WARNING, DateTime LSD_WARNTIME)
        {
            //this.id = Id;
            this.ku_Id = KU_ID;
            this.content = LSD_CONTENT;
            this.title = LSD_TITLE;
            this.warning = LSD_WARNING;
            this.warnTime = LSD_WARNTIME;
        }
        #endregion

        #region 成员
        //private int id;
        private long ku_Id;
        private string content;
        private string title;
        private int warning;
        private DateTime warnTime;
        #endregion


        #region 属性
        //public virtual int Id
        //{
        //    get { return id; }
        //    set { id = value; }
        //}
        public virtual long KU_ID
        {
            get { return ku_Id; }
            set { ku_Id = value; }
        }

        public virtual string LSD_CONTENT
        {
            get { return content; }
            set { content = value; }
        }

        public virtual string LSD_TITLE
        {
            get { return title; }
            set { title = value; }
        }

        public virtual int LSD_WARNING
        {
            get { return warning; }
            set { warning = value; }
        }

        public virtual DateTime LSD_WARNTIME
        {
            get { return warnTime; }
            set { warnTime = value; }
        }

        #endregion
    }
}
