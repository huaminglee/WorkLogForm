using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WorkLogForm.WindowUiClass;
using System.Collections;
using ClassLibrary;
using WorkLogForm.Service;
using WorkLogForm.CommonClass;

namespace WorkLogForm
{
    public partial class personal_setting : Form
    {
        BaseService baseService = new BaseService();
        private Hobby ri_zhi_hobby;
        private Hobby ri_cheng_hobby;
        private Hobby sui_bi_hobby;
        private WkTRole role;

        public WkTRole Role
        {
            get { return role; }
            set { role = value; }
        }
        private WkTUser user;

        public WkTUser User
        {
            get { return user; }
            set { user = value; }
        }
        private Point formLocation;

        public Point FormLocation
        {
            get { return formLocation; }
            set { formLocation = value; }
        }
        public personal_setting()
        {
            InitializeComponent();
            initialWindow();
        }

        #region 自定义窗体初始化方法
        /// <summary>
        /// 初始化window（界面效果）
        /// </summary>
        private void initialWindow()
        {
            creatWindow.SetFormRoundRectRgn(this, 15);
            creatWindow.SetFormShadow(this);
        }
        #endregion

        private void personal_setting_Load(object sender, EventArgs e)
        {
            if (this.formLocation != null)
            {
                this.Location = formLocation;
            }
            IList hobbyList = baseService.loadEntityList("from Hobby where STATE=" + (int)IEntity.stateEnum.Normal + " and Staff=" + user.Id);
            if (hobbyList != null && hobbyList.Count > 0)
            {
                foreach (Hobby h in hobbyList)
                {
                    if (h.VisibleFlag == "")
                    {
                        return;
                    }
                    char[] flag = h.VisibleFlag.ToArray();
                    if (h.TypeFlag.Equals((int)Hobby.hobbyTypeEnum.RiZhi))
                    {
                        ri_zhi_hobby = h;
                        initCheckBox(ri_zhi_checkBoxBT, flag[(int)Hobby.visibleTypeEnum.BT]);
                        initCheckBox(ri_zhi_checkBoxBX, flag[(int)Hobby.visibleTypeEnum.BX]);
                        initCheckBox(ri_zhi_checkBoxWT, flag[(int)Hobby.visibleTypeEnum.WT]);
                        initCheckBox(ri_zhi_checkBoxWX, flag[(int)Hobby.visibleTypeEnum.WX]);
                    }
                    if (h.TypeFlag.Equals((int)Hobby.hobbyTypeEnum.RiCheng))
                    {
                        ri_cheng_hobby = h;
                        initCheckBox(ri_cheng_checkBoxBT, flag[(int)Hobby.visibleTypeEnum.BT]);
                        initCheckBox(ri_cheng_checkBoxBX, flag[(int)Hobby.visibleTypeEnum.BX]);
                        initCheckBox(ri_cheng_checkBoxWT, flag[(int)Hobby.visibleTypeEnum.WT]);
                        initCheckBox(ri_cheng_checkBoxWX, flag[(int)Hobby.visibleTypeEnum.WX]);
                    }
                    if (h.TypeFlag.Equals((int)Hobby.hobbyTypeEnum.SuiBi))
                    {
                        sui_bi_hobby = h;
                        initCheckBox(sui_bi_checkBoxBT, flag[(int)Hobby.visibleTypeEnum.BT]);
                        initCheckBox(sui_bi_checkBoxBX, flag[(int)Hobby.visibleTypeEnum.BX]);
                        initCheckBox(sui_bi_checkBoxWT, flag[(int)Hobby.visibleTypeEnum.WT]);
                        initCheckBox(sui_bi_checkBoxWX, flag[(int)Hobby.visibleTypeEnum.WX]);
                        initCheckBox(sui_bi_checkBoxBS, flag[(int)Hobby.visibleTypeEnum.BS]);
                        initCheckBox(sui_bi_checkBoxWS, flag[(int)Hobby.visibleTypeEnum.WS]);
                    }
                }
            }
        }

        private void initCheckBox(CheckBox cb, char flag)
        {
            if (flag.Equals('0'))
            {
                cb.Checked = true;
            }
            else if (flag.Equals('1'))
            {
                cb.Checked = false;
            }
        }

        private string getCheckBox(CheckBox cb)
        {
            if (cb.Checked == true)
            {
                return "0";
            }
            else if (cb.Checked == false)
            {
                return "1";
            }
            return null;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            IList BS = baseService.loadEntityList("select u from WkTUser u left join u.UserRole r where u.Kdid=" + user.Kdid.Id + " and r.KrDESC='" + CommonStaticParameter.RoleDesc + "' and r.KrOrder<" + role.KrOrder);
            IList BT = baseService.loadEntityList("select u from WkTUser u left join u.UserRole r where u.Kdid=" + user.Kdid.Id + " and r.KrDESC='" + CommonStaticParameter.RoleDesc + "' and r.KrOrder=" + role.KrOrder);
            IList BX = baseService.loadEntityList("select u from WkTUser u left join u.UserRole r where u.Kdid=" + user.Kdid.Id + " and r.KrDESC='" + CommonStaticParameter.RoleDesc + "' and r.KrOrder>" + role.KrOrder);
            IList WS = baseService.loadEntityList("select u from WkTUser u left join u.UserRole r where u.Kdid!=" + user.Kdid.Id + " and r.KrDESC='" + CommonStaticParameter.RoleDesc + "' and r.KrOrder<" + role.KrOrder);
            IList WT = baseService.loadEntityList("select u from WkTUser u left join u.UserRole r where u.Kdid!=" + user.Kdid.Id + " and r.KrDESC='" + CommonStaticParameter.RoleDesc + "' and r.KrOrder=" + role.KrOrder);
            IList WX = baseService.loadEntityList("select u from WkTUser u left join u.UserRole r where u.Kdid!=" + user.Kdid.Id + " and r.KrDESC='" + CommonStaticParameter.RoleDesc + "' and r.KrOrder>" + role.KrOrder);
            if (ri_zhi_hobby == null)
            {
                ri_zhi_hobby = new Hobby();
            }
            if (ri_cheng_hobby == null)
            {
                ri_cheng_hobby = new Hobby();
            }
            if (sui_bi_hobby == null)
            {
                sui_bi_hobby = new Hobby();
            }
            #region 保存日志偏好
            ri_zhi_hobby.Staff = user;
            ri_zhi_hobby.State = (int)IEntity.stateEnum.Normal;
            ri_zhi_hobby.TimeStamp = DateTime.Now.Ticks;
            ri_zhi_hobby.TypeFlag = (int)Hobby.hobbyTypeEnum.RiZhi;
            ri_zhi_hobby.VisibleFlag = "0"+getCheckBox(ri_zhi_checkBoxBT)+getCheckBox(ri_zhi_checkBoxBX)+"0"+getCheckBox(ri_zhi_checkBoxWT)+getCheckBox(ri_zhi_checkBoxWX);
            if (ri_zhi_hobby.SharedStaffs == null)
            {
                ri_zhi_hobby.SharedStaffs = new List<WkTUser>();
            }else
            {
                ri_zhi_hobby.SharedStaffs.Clear();
            }
            if (ri_zhi_checkBoxBT.Checked)
            {
                foreach(WkTUser u in BT)
                {
                    ri_zhi_hobby.SharedStaffs.Add(u);
                }
            }
            if (ri_zhi_checkBoxBX.Checked)
            {
                foreach (WkTUser u in BX)
                {
                    ri_zhi_hobby.SharedStaffs.Add(u);
                }
            }
            if (ri_zhi_checkBoxWT.Checked)
            {
                foreach (WkTUser u in WT)
                {
                    ri_zhi_hobby.SharedStaffs.Add(u);
                }
            }
            if (ri_zhi_checkBoxWX.Checked)
            {
                foreach (WkTUser u in WX)
                {
                    ri_zhi_hobby.SharedStaffs.Add(u);
                }
            }
            baseService.SaveOrUpdateEntity(ri_zhi_hobby);
            #endregion
            #region 保存日程偏好
            ri_cheng_hobby.Staff = user;
            ri_cheng_hobby.State = (int)IEntity.stateEnum.Normal;
            ri_cheng_hobby.TimeStamp = DateTime.Now.Ticks;
            ri_cheng_hobby.TypeFlag = (int)Hobby.hobbyTypeEnum.RiCheng;
            ri_cheng_hobby.VisibleFlag = "0" + getCheckBox(ri_cheng_checkBoxBT) + getCheckBox(ri_cheng_checkBoxBX) + "0" + getCheckBox(ri_cheng_checkBoxWT) + getCheckBox(ri_cheng_checkBoxWX);
            if (ri_cheng_hobby.SharedStaffs == null)
            {
                ri_cheng_hobby.SharedStaffs = new List<WkTUser>();
            }
            else
            {
                ri_cheng_hobby.SharedStaffs.Clear();
            }
            if (ri_cheng_checkBoxBT.Checked)
            {
                foreach (WkTUser u in BT)
                {
                    ri_cheng_hobby.SharedStaffs.Add(u);
                }
            }
            if (ri_cheng_checkBoxBX.Checked)
            {
                foreach (WkTUser u in BX)
                {
                    ri_cheng_hobby.SharedStaffs.Add(u);
                }
            }
            if (ri_cheng_checkBoxWT.Checked)
            {
                foreach (WkTUser u in WT)
                {
                    ri_cheng_hobby.SharedStaffs.Add(u);
                }
            }
            if (ri_cheng_checkBoxWX.Checked)
            {
                foreach (WkTUser u in WX)
                {
                    ri_cheng_hobby.SharedStaffs.Add(u);
                }
            }
            baseService.SaveOrUpdateEntity(ri_cheng_hobby);
            #endregion
            #region 保存随笔偏好
            sui_bi_hobby.Staff = user;
            sui_bi_hobby.State = (int)IEntity.stateEnum.Normal;
            sui_bi_hobby.TimeStamp = DateTime.Now.Ticks;
            sui_bi_hobby.TypeFlag = (int)Hobby.hobbyTypeEnum.SuiBi;
            sui_bi_hobby.VisibleFlag = getCheckBox(sui_bi_checkBoxBS) + getCheckBox(sui_bi_checkBoxBT) + getCheckBox(sui_bi_checkBoxBX) + getCheckBox(sui_bi_checkBoxWS) + getCheckBox(sui_bi_checkBoxWT) + getCheckBox(sui_bi_checkBoxWX);
            if (sui_bi_hobby.SharedStaffs == null)
            {
                sui_bi_hobby.SharedStaffs = new List<WkTUser>();
            }
            else
            {
                sui_bi_hobby.SharedStaffs.Clear();
            }
            if (sui_bi_checkBoxBS.Checked)
            {
                foreach (WkTUser u in BS)
                {
                    sui_bi_hobby.SharedStaffs.Add(u);
                }
            }
            if (sui_bi_checkBoxWS.Checked)
            {
                foreach (WkTUser u in WS)
                {
                    sui_bi_hobby.SharedStaffs.Add(u);
                }
            }
            if (sui_bi_checkBoxBT.Checked)
            {
                foreach (WkTUser u in BT)
                {
                    sui_bi_hobby.SharedStaffs.Add(u);
                }
            }
            if (sui_bi_checkBoxBX.Checked)
            {
                foreach (WkTUser u in BX)
                {
                    sui_bi_hobby.SharedStaffs.Add(u);
                }
            }
            if (sui_bi_checkBoxWT.Checked)
            {
                foreach (WkTUser u in WT)
                {
                    sui_bi_hobby.SharedStaffs.Add(u);
                }
            }
            if (sui_bi_checkBoxWX.Checked)
            {
                foreach (WkTUser u in WX)
                {
                    sui_bi_hobby.SharedStaffs.Add(u);
                }
            }
            baseService.SaveOrUpdateEntity(sui_bi_hobby);
            #endregion
        }


        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
