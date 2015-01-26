using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WorkLogForm.WindowUiClass;
using ClassLibrary;
using System.Collections;
using WorkLogForm.Service;

namespace WorkLogForm
{
    public partial class Examine_Allograph : Form
    {
        BaseService baseService = new BaseService();
        private WkTUser user;
        private WkTRole role;
        public WkTRole Role
        {
            get { return role; }
            set { role = value; }
        }
        public WkTUser User
        {
            get { return user; }
            set { user = value; }
        }
        public Examine_Allograph()
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

        #region 最小化关闭按钮
        private void min_pictureBox_MouseEnter(object sender, EventArgs e)
        {
            min_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.最小化_副本;
        }
        private void min_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            min_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.最小化渐变;
        }
        private void min_pictureBox_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void close_pictureBox_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void close_pictureBox_MouseEnter(object sender, EventArgs e)
        {
            close_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.关闭渐变_副本;
        }
        private void close_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            close_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.关闭渐变;
        }
        #endregion

        #region 窗体移动代码
        private int x_point, y_point;
        private void review_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.x_point = e.X;
                this.y_point = e.Y;
            }
        }

        private void review_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && this.Location.Y > 0)
            {
                Top = MousePosition.Y - y_point;
                Left = MousePosition.X - x_point;
            }
            else if (e.Button == MouseButtons.Left && e.Y > this.y_point)
            {
                Top = MousePosition.Y - y_point;
                Left = MousePosition.X - x_point;
            }
        }
        #endregion

        private void Examine_Allograph_Load(object sender, EventArgs e)
        {
            IList deptList = baseService.loadEntityList("from WkTDept");
            staff_comboBox.Items.Add("请选择");
            if (deptList != null && deptList.Count > 0)
            {
                foreach (WkTDept dept in deptList)
                {
                    staff_comboBox.Items.Add(dept.KdName.Trim());
                }
            }
            staff_comboBox.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }

        private void initListView(ListView listView, IList userList)
        {
            if (userList != null && userList.Count > 0)
            {
                foreach (InsteadAttendance ia in userList)
                {
                    ListViewItem item = new ListViewItem();
                    item.Tag = ia;
                    item.ToolTipText = CommonClass.CommonUtil.toolTipFormat(ia.SignReason);
                    ListViewItem.ListViewSubItem signName = new ListViewItem.ListViewSubItem();
                    ListViewItem.ListViewSubItem dept = new ListViewItem.ListViewSubItem();
                    ListViewItem.ListViewSubItem signType = new ListViewItem.ListViewSubItem();
                    ListViewItem.ListViewSubItem name = new ListViewItem.ListViewSubItem();
                    ListViewItem.ListViewSubItem signStart = new ListViewItem.ListViewSubItem();
                    ListViewItem.ListViewSubItem signEnd = new ListViewItem.ListViewSubItem();
                    ListViewItem.ListViewSubItem signReason = new ListViewItem.ListViewSubItem();
                    ListViewItem.ListViewSubItem signState = new ListViewItem.ListViewSubItem();
                    signName.Text = ia.SignUser.KuName.Trim();
                    dept.Text = ia.SignUser.Kdid.KdName.Trim();
                    signType.Text = ia.SignType == (int)InsteadAttendance.SignTypeEnum.SignIn ? "签到" : (ia.SignType == (int)InsteadAttendance.SignTypeEnum.SignOut ? "签退" : "签到&签退");
                    name.Text = ia.InsteadUser.KuName.Trim();
                    DateTime startDate = new DateTime(ia.SignStartDate);
                    DateTime endDate = new DateTime(ia.SignEndDate);
                    signStart.Text = startDate.Year + "-" + startDate.Month + "-" + startDate.Day;
                    signEnd.Text = endDate.Year + "-" + endDate.Month + "-" + endDate.Day;
                    signReason.Text = ia.SignReason.Trim();
                    signState.Text = ia.SignExamine == (int)InsteadAttendance.ExamineEnum.None ? "未审批" : (ia.SignExamine == (int)InsteadAttendance.ExamineEnum.Agree ? "已通过" : "不通过");
                    item.SubItems.Add(signName);
                    item.SubItems.Add(dept);
                    item.SubItems.Add(name);
                    item.SubItems.Add(signType);
                    item.SubItems.Add(signStart);
                    item.SubItems.Add(signEnd);
                    item.SubItems.Add(signReason);
                    item.SubItems.Add(signState);
                    listView.Items.Add(item);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = "select ia from InsteadAttendance ia left join ia.SignUser su left join su.Kdid dept left join ia.InsteadUser iu where dept.KdName like '%" + (staff_comboBox.Text != "请选择" ? staff_comboBox.Text : "") + "%' and su.KuName like '%" + textBox2.Text.Trim() + "%' and iu.KuName like '%" + textBox3.Text.Trim() + "%'";
            if (comboBox2.SelectedIndex != 0)
            {
                sql += " and ia.SignExamine=" + (comboBox2.SelectedIndex - 1);
            }
            IList iaList = baseService.loadEntityList(sql);
            listView1.Items.Clear();
            initListView(listView1, iaList);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.CheckedItems == null)
            {
                return;
            }
            String sql = "update LOG_T_INSTEADATTENDANCE set SignExamine=" + (int)InsteadAttendance.ExamineEnum.Agree + " where";
            foreach (ListViewItem item in listView1.CheckedItems)
            {
                InsteadAttendance ia = (InsteadAttendance)item.Tag;
                sql += " id=" + ia.Id + " or";
            }
            sql = sql.Substring(0, sql.Length - 3);
            baseService.ExecuteSQL(sql);
            button1_Click(button1, new EventArgs());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            String sql = "update LOG_T_INSTEADATTENDANCE set SignExamine=" + (int)InsteadAttendance.ExamineEnum.NotAgree + " where";
            foreach (ListViewItem item in listView1.CheckedItems)
            {
                InsteadAttendance ia = (InsteadAttendance)item.Tag;
                sql += " id=" + ia.Id + " or";
            }
            sql.Substring(0, sql.Length - 3);
            baseService.ExecuteSQL(sql);
            button1_Click(button1, new EventArgs());
        }
    }
}
