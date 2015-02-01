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
using WorkLogForm.Service;
using System.Collections;

namespace WorkLogForm
{
    public partial class writeSchedule : Form
    {
        private BaseService baseService = new BaseService();
        public delegate void ParentFormChange();
        private ParentFormChange parentChangeDelegate;
        private WkTUser user;
        private DateTime scheduleDate;
        private List<WkTUser> sharedUser;
        public List<WkTUser> SharedUser
        {
            get { return sharedUser; }
            set { sharedUser = value; }
        }
        public DateTime ScheduleDate
        {
            get { return scheduleDate; }
            set { scheduleDate = value; }
        }
        public WkTUser User
        {
            get { return user; }
            set { user = value; }
        }
        private WkTRole role;

        public WkTRole Role
        {
            get { return role; }
            set { role = value; }
        }
        public writeSchedule(ParentFormChange parentChangeDelegate)
        {
            this.parentChangeDelegate = parentChangeDelegate;
            InitializeComponent();
            initialWindow();
        }
        private void writeSchedule_Load(object sender, EventArgs e)
        {
            if(role.KrOrder.Equals(3))
            {
                this.tabControl1.TabPages.RemoveAt(1);
            }
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - this.Width / 2, Screen.PrimaryScreen.WorkingArea.Height / 2 - this.Height / 2);
            initData();
        }
        private void initData()
        {
            IList ssList = baseService.loadEntityList("select distinct Subject from StaffSchedule where State=" + (int)IEntity.stateEnum.Normal);
            if (ssList != null && ssList.Count > 0)
            {
                foreach (String s in ssList)
                {
                    if (s != null && !s.Equals(""))
                    {
                        comboBox1.Items.Add(s);
                    }
                }
            }
            IList staffHobbyList = baseService.loadEntityList("from Hobby where State=" + (int)IEntity.stateEnum.Normal + " and Staff=" + user.Id + " and TypeFlag=" + (int)Hobby.hobbyTypeEnum.RiCheng);
            if (staffHobbyList != null && staffHobbyList.Count > 0)
            {
                Hobby s = (Hobby)staffHobbyList[0];
                SharedUser = new List<WkTUser>();
                SharedUser.AddRange(s.SharedStaffs);
            }
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
        private void writeSchedule_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.x_point = e.X;
                this.y_point = e.Y;
            }
        }

        private void writeSchedule_MouseMove(object sender, MouseEventArgs e)
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
        private void button1_Click(object sender, EventArgs e)
        {
            //日程编写
            StaffSchedule staffSchedule = new StaffSchedule();
            staffSchedule.IfRemind = checkBox1.Checked ? (int)StaffSchedule.IfRemindEnum.Renmind : (int)StaffSchedule.IfRemindEnum.NotRemind;
            //会议时间
            staffSchedule.ScheduleTime = scheduleDate.Date.Ticks + dateTimePicker1.Value.TimeOfDay.Ticks;
            //提醒时间
            staffSchedule.RemindTime = scheduleDate.Date.Ticks + dateTimePicker2.Value.TimeOfDay.Ticks;
            staffSchedule.Staff = this.user;
            staffSchedule.StaffScheduleStaffs = sharedUser;
            staffSchedule.Subject = comboBox1.Text.Trim();
            staffSchedule.TimeStamp = DateTime.Now.Ticks;
            staffSchedule.State = (int)IEntity.stateEnum.Normal;
            staffSchedule.Content = textBox1.Text.Trim();
            staffSchedule.ArrangeMan = user;
            try
            {
                baseService.SaveOrUpdateEntity(staffSchedule);
            }
            catch
            {
                MessageBox.Show("保存失败！");
                return;
            }
            MessageBox.Show("保存成功！");
            this.Invoke(parentChangeDelegate, null);
            parentChangeDelegate();
            this.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            InterimHead interimHead = new InterimHead();
            interimHead.User = this.user;
            interimHead.ParentForm1 = this;
            interimHead.ShowDialog();
        }

        public void createTree(TreeView tv)
        {
            #region 加载树结构
            TreeNode Gt = new TreeNode();
            Gt.Text = "部门";
            tv.Nodes.Add(Gt);
            string sql = "select u.DeptId from Wktuser_M_Dept u where u.WktuserId = " + user.Id + " and u.State = " + (int)IEntity.stateEnum.Normal;
            IList depts = baseService.loadEntityList(sql);
            if (depts != null && depts.Count > 0)
            {
                foreach (WkTDept o in depts)
                {
                    TreeNode t1 = new TreeNode();
                    t1.Tag = o;
                    t1.Text = o.KdName;

                    string sql1 = "select u from WkTUser u left join u.Kdid dept where dept.Id = " + o.Id;
                    IList userlist = baseService.loadEntityList(sql1);

                    if (userlist != null && userlist.Count > 0)
                    {
                        foreach (WkTUser oo in userlist)
                        {
                            if (oo.Id != user.Id)
                            {
                                TreeNode t2 = new TreeNode();
                                t2.Text = oo.KuName;
                                t2.Tag = oo;
                               

                                t1.Nodes.Add(t2);
                            }
                        }
                    }
                    Gt.Nodes.Add(t1);
                }
            }
            #endregion

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabControl1.TabPages.Count > 1)
            {
                if (tabControl1.SelectedIndex == 1)
                {
                    //向树中加载数据
                    createTree(treeView1);
                }
            }
           
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            TreeNode t = e.Node;
            SelectTree(t);
        }

        private void SelectTree(TreeNode t)
        {
            if (t.Text == "部门")
            {
                TreeNode tt = treeView1.Nodes[0];
                if (t.Checked == true)
                {

                    foreach (TreeNode t2 in tt.Nodes)
                    {
                        t2.Checked = true;
                    }

                }
                else if (t.Checked == false)
                {
                    foreach (TreeNode t2 in tt.Nodes)
                    {
                        t2.Checked = false;
                        foreach (TreeNode t3 in t2.Nodes)
                        {
                            t3.Checked = false;
                        }
                    }
                }
            }
            else if (t.Text != "部门")
            {
                if (t.Tag is WkTDept)
                {

                    if (t.Checked == true)
                    {
                        foreach (TreeNode t3 in t.Nodes)
                        {
                            t3.Checked = true;
                        }
                    }
                    else if (t.Checked == false)
                    {
                        foreach (TreeNode t3 in t.Nodes)
                        {
                            t3.Checked = false;
                        }
                    }


                }
                else if (t.Tag is WkTUser)
                {

                }

            }

        }


        /// <summary>
        /// 员工安排日程确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            this.button3.Cursor = Cursors.WaitCursor;
            TreeNode t = treeView1.Nodes[0];

            foreach (TreeNode t1 in t.Nodes)
            {
                foreach (TreeNode t2 in t1.Nodes)
                {
                    if (t2.Checked == true)
                    {
                        WkTUser u = (WkTUser)t2.Tag;


                        //日程编写
                        StaffSchedule staffSchedule = new StaffSchedule();
                        staffSchedule.IfRemind = checkBox2.Checked ? (int)StaffSchedule.IfRemindEnum.Renmind : (int)StaffSchedule.IfRemindEnum.NotRemind;
                        //会议时间
                        staffSchedule.ScheduleTime = dateTimePicker3.Value.Ticks;//scheduleDate.Date.Ticks + dateTimePicker1.Value.TimeOfDay.Ticks;
                        //提醒时间
                        staffSchedule.RemindTime = dateTimePicker4.Value.Ticks ;//scheduleDate.Date.Ticks + dateTimePicker2.Value.TimeOfDay.Ticks;
                        staffSchedule.Staff = u;
                        //staffSchedule.StaffScheduleStaffs = sharedUser;
                        staffSchedule.Subject = textBox3.Text.Trim();
                        staffSchedule.TimeStamp = DateTime.Now.Ticks;
                        staffSchedule.State = (int)IEntity.stateEnum.Normal;
                        staffSchedule.Content = textBox2.Text.Trim();
                        staffSchedule.ArrangeMan = user;
                        try
                        {
                            baseService.SaveOrUpdateEntity(staffSchedule);
                        }
                        catch
                        {
                            MessageBox.Show("保存失败！");
                            return;
                        }
                        
                    }

                }
            }

            this.button3.Cursor = Cursors.Hand;

            MessageBox.Show("保存成功!");

        }



    }
}
