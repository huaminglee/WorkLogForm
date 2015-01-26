using ClassLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkLogForm.Service
{
    class OpOndutyDateBase:BaseService
    {

        /// <summary>
        /// 获取全部部门
        /// </summary>
        /// <returns></returns>
        public IList GetAllDept()
        {
            IList i;
            string sql = "from WkTDept u where u.Id <>1 ";
            i = loadEntityList(sql);
            return i;
        }


        /// <summary>
        /// 按部门与名字查询员工
        /// </summary>
        /// <param name="name"></param>
        /// <param name="seldetp"></param>
        /// <returns></returns>
        public IList GetSelectYuanGong(string name ,WkTDept seldetp)
        {
            IList i;
            string sql = "from WkTUser u where u.KuName like '%" + name + "%' and u.Kdid = " + seldetp.Id; 
            i = loadEntityList(sql);
            return i;
        }

        /// <summary>
        /// 查询部门负责人
        /// </summary>
        /// <param name="seldetp"></param>
        /// <returns></returns>
        public IList GetDeptLeader(WkTDept seldetp)
        {
            IList i;
            string sql = "select u from WkTUser u left join u.UserRole r where  u.Kdid = " + seldetp.Id +
                " and r.KrOrder = 2 ";
            i = loadEntityList(sql);
            return i;
        }



        /// <summary>
        /// 判断是否已经安排
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns>true代表已经安排过了</returns>
        public bool IsSurInSheetManager(long t1, long t2)
        {

            string sql = " from TimeArrangeForManager u where u.Startime > " + t2.ToString() + " or u.Endtime <= " + t1.ToString();

            IList i = loadEntityList(sql);

            //没有查到
            if (i != null && i.Count >= 0)
            {
                return false;
            }

            //查到了 
            else
            {
                return true;
            }

        }


        /// <summary>
        /// 从时间安排表中按时间与负责人查找
        /// </summary>
        /// <param name="user"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public IList SelectManagerTime(WkTUser user , long t1, long t2)
        {
            IList i;
            string sql = "from TimeArrangeForManager u.Startime <= " + t1 +
                " and u.Endtime > " + t2 +
                " and u.UserId = " + user.Id;
            i = loadEntityList(sql);
            return i;
        }


        public IList GetMonthDuty(long t1,long t2) 
        {
            IList i;
            string sql = " from OnDuty u whwere u.Time < " + t2 +
                " and u.Time >=  " + t1;
            i = loadEntityList(sql);
            return i;
        }



    }
}
