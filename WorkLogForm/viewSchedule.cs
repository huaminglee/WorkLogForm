using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WorkLogForm.WindowUiClass;
using ClassLibrary;
using WorkLogForm.CommonClass;
using WorkLogForm.Service;
using System.Collections;

namespace WorkLogForm
{
    public partial class viewSchedule : Form
    {
        private BaseService baseService = new BaseService();
        private WkTUser user;
        private DateTime scheduleDate;
        private List<StaffSchedule> scheduleList = new List<StaffSchedule>();
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
        public viewSchedule()
        {
            InitializeComponent();
            creatWindow.SetFormRoundRectRgn(this, 15);
            creatWindow.SetFormShadow(this);
        }
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
        private void viewSchedule_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.x_point = e.X;
                this.y_point = e.Y;
            }
        }

        private void viewSchedule_MouseMove(object sender, MouseEventArgs e)
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


        private void initData()
        {
            if (scheduleDate != null && !scheduleDate.Equals(new DateTime(0)))
            {
                title_label.Text = "查看日程-" + scheduleDate.Year + "年" + scheduleDate.Month + "月" + scheduleDate.Day + "日";
                long thisDay = scheduleDate.Date.Ticks;
                long nextDay = scheduleDate.Date.Ticks + new DateTime(1, 1, 2).Ticks;
                IList ssList = baseService.loadEntityList("from StaffSchedule where State=" + (int)IEntity.stateEnum.Normal + " and ScheduleTime>=" + thisDay + "and ScheduleTime<" + nextDay + " and Staff=" + user.Id + " order by ScheduleTime asc");
                if (ssList != null && ssList.Count > 0)
                {
                    foreach (StaffSchedule s in ssList)
                    {
                        scheduleList.Add(s);
                    }
                }
            }
        }
        private void label_MouseHover(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            if (label.Tag != null)
            {
                String tip = "";
                List<StaffSchedule> ssList = (List<StaffSchedule>)label.Tag;
                foreach (StaffSchedule s in ssList)
                {
                    DateTime dateTime = new DateTime(s.ScheduleTime);
                    tip += CNDate.getTimeByTimeTicks(dateTime.TimeOfDay.Ticks) + Environment.NewLine;
                    tip += s.Content + Environment.NewLine;
                }
                this.toolTip1.SetToolTip(label, tip);
            }
        }
        private void label_Paint(object sender, PaintEventArgs e)
        {
            Label label = (Label)sender;
            List<StaffSchedule> ssList = new List<StaffSchedule>();
            long thisTime = scheduleDate.Date.Ticks + new DateTime(1, 1, 1, label.TabIndex, 0, 0).Ticks;
            long nextTime = 0;
            if (label.TabIndex.Equals(23))
            {
                nextTime = scheduleDate.Date.Ticks + new DateTime(1, 1, 2, 0, 0, 0).Ticks;
            }
            else
            {
                nextTime = scheduleDate.Date.Ticks + new DateTime(1, 1, 1, label.TabIndex + 1, 0, 0).Ticks;
            }
            foreach (StaffSchedule s in scheduleList)
            {
                if (s.ScheduleTime >= thisTime && s.ScheduleTime < nextTime)
                {
                    ssList.Add(s);
                }
            }
            if (ssList.Count > 0)
            {
                label.Tag = ssList;
                string str = "";
                foreach (StaffSchedule s in ssList)
                {
                    str = str+s.Content+"  ";
                }
               
                label.Text = str.Length > 14 ? str.Substring(0, 14) + "..." : str;
                
            }
        }
        private void viewSchedule_Load(object sender, EventArgs e)
        {
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - this.Width / 2, Screen.PrimaryScreen.WorkingArea.Height / 2 - this.Height / 2);
            initData();
        }
    }
}
