using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary
{
    public class Sharing : IEntity
    {
         #region 构造函数
        public Sharing()
        { }

        public Sharing(long LS_SHAREID, long KU_ID, long LAL_ID)
        {
            //this.id = Id;
            this.shareId = LS_SHAREID;
            this.ku_Id = KU_ID;
            this.lal_Id = LAL_ID;
        }
        #endregion

        #region 成员
        //private int id;
        private long shareId;
        private long ku_Id;
        private long lal_Id;
        #endregion


        #region 属性
        //public virtual int Id
        //{
        //    get { return id; }
        //    set { id = value; }
        //}
        public virtual long LS_SHAREID
        {
            get { return shareId; }
            set { shareId = value; }
        }

        public virtual long KU_ID
        {
            get { return ku_Id; }
            set { ku_Id = value; }
        }

        public virtual long LAL_ID
        {
            get { return lal_Id; }
            set { lal_Id = value; }
        }

        #endregion
    }
}
