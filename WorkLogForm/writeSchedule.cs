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
        public writeSchedule(ParentFormChange parentChangeDelegate)
        {
            this.parentChangeDelegate = parentChangeDelegate;
            InitializeComponent();
            initialWindow();
        }
        private void writeSchedule_Load(object sender, EventArgs e)
        {
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
    }
}
