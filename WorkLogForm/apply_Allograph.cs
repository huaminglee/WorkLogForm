using System;
using System.Windows.Forms;
using WorkLogForm.WindowUiClass;
using ClassLibrary;
using System.Collections;
using System.Collections.Generic;
using WorkLogForm.Service;
using WorkLogForm.CommonClass;

namespace WorkLogForm
{
    public partial class Apply_Allograph : Form
    {
        private BaseService baseService = new BaseService();
        private IList signUserList;
        private List<WkTUser> signedUserList;
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
        public Apply_Allograph()
        {
            InitializeComponent();
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
        private void apply_Allograph_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.x_point = e.X;
                this.y_point = e.Y;
            }
        }

        private void apply_Allograph_MouseMove(object sender, MouseEventArgs e)
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
        private void apply_Allograph_Load(object sender, EventArgs e)
        {
            initialWindow();
            initDate();
        }
        private void initDate()
        {
            textBox1.Text = "";
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            if (role.KrOrder == 2)
            {
                signUserList = baseService.loadEntityList("select u from WkTUser u left join u.KdId dept where dept.KdId=" + user.Kdid);
            }
            else if(role.KrOrder == 0||role.KrOrder==1)
            {
                signUserList = baseService.loadEntityList("select u from WkTUser u left join u.UserRole r where r.KrOrder<3 and r.KrDESC='" + CommonStaticParameter.RoleDesc + "'");
            }
            signedUserList = new List<WkTUser>();
            listView1.Items.Clear();
            listView2.Items.Clear();
            initListView(listView1, signUserList);
        }
        private void initListView(ListView listView, IList userList)
        {
            if (userList != null && userList.Count > 0)
            {
                foreach (Object o in userList)
                {
                    WkTUser u = (WkTUser)o;
                    ListViewItem item = new ListViewItem();
                    item.Tag = u;
                    ListViewItem.ListViewSubItem name = new ListViewItem.ListViewSubItem();
                    ListViewItem.ListViewSubItem dept = new ListViewItem.ListViewSubItem();
                    ListViewItem.ListViewSubItem role = new ListViewItem.ListViewSubItem();
                    name.Text = u.KuName.Trim();
                    dept.Text = u.Kdid.KdName.Trim();
                    foreach (WkTRole r in u.UserRole)
                    {
                        if (r.KrDESC.Trim().Equals("工作小秘书角色"))
                        {
                            role.Text = r.KrName;
                        }
                    }
                    item.SubItems.Add(name);
                    item.SubItems.Add(dept);
                    item.SubItems.Add(role);
                    listView.Items.Add(item);
                }
            }
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.CheckedItems)
            {
                signUserList.Remove((WkTUser)item.Tag);
                signedUserList.Add((WkTUser)item.Tag);
                listView1.Items.Remove(item);
                listView2.Items.Add(item);
                item.Checked = false;
            }
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            foreach(ListViewItem item in listView2.CheckedItems)
            {
                signUserList.Add((WkTUser)item.Tag);
                signedUserList.Remove((WkTUser)item.Tag);
                listView2.Items.Remove(item);
                listView1.Items.Add(item);
                item.Checked = false;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            foreach (WkTUser u in signedUserList)
            {
                InsteadAttendance ia = new InsteadAttendance();
                ia.State = (int)IEntity.stateEnum.Normal;
                ia.TimeStamp = DateTime.Now.Ticks;
                ia.SignStartDate = dateTimePicker1.Value.Date.Ticks;
                ia.SignEndDate = dateTimePicker2.Value.Date.Ticks;
                ia.InsteadUser = user;
                ia.SignReason = textBox1.Text.Trim();
                if (checkBox1.Checked && !checkBox2.Checked)
                {
                    ia.SignType = 0;
                }
                else if (!checkBox1.Checked && checkBox2.Checked)
                {
                    ia.SignType = 1;
                }
                else if (checkBox1.Checked && checkBox2.Checked)
                {
                    ia.SignType = 2;
                }
                ia.SignExamine = (int)InsteadAttendance.ExamineEnum.None;
                ia.SignUser = u;
                baseService.SaveOrUpdateEntity(ia);
            }
            initDate();
            MessageBox.Show("已提交申请！");
        }
        private void button3_Click(object sender, EventArgs e)
        {
            initDate();
        }
    }
}
