using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClassLibrary;//实体所在的命名空间
using WorkLogForm.Service;
using System.Collections;
using NHibernate;
using NHibernate.Cfg;
using WorkLogForm.CommonClass;
using System.Data.SqlClient;
using WorkLogForm.WindowUiClass;
using System.Web.Script.Serialization;
using System.Web;
using System.Web.Configuration;
using System.Net;
using System.IO;

namespace WorkLogForm
{
    public partial class shou_ye : Form
    {
        //testGit
        private BaseService baseService = new BaseService();
        IList attendanceList;
        char[] usuallyDay;
        IList holidayList;
        IList workDayList;
        IList leaveList;


        private WkTUser user;//当前使用用户
        public WkTUser User
        {
            get { return user; }
            set { user = value; }
        }


        private List<Label> dateLabel = new List<Label>();
        public shou_ye()
        {
            InitializeComponent();
            creatWindow.SetFormRoundRectRgn(this, 15);
            creatWindow.SetFormShadow(this);

        }

        #region 程序运行部分

        private void shou_ye_Load(object sender, EventArgs e)
        {
            loadData();//封装label
            year_comboBoxEx.SelectedIndex = DateTime.Now.Year - 2012;
            month_comboBoxEx.SelectedIndex = DateTime.Now.Month - 1;
            this.Visible = false;
            this.Opacity = 0;
            timer1.Start();
        }
        private void loadData()
        {
            dateLabel.Add(label111); dateLabel.Add(label122); dateLabel.Add(label133); dateLabel.Add(label144);
            dateLabel.Add(label155); dateLabel.Add(label166); dateLabel.Add(label177); dateLabel.Add(label211);
            dateLabel.Add(label222); dateLabel.Add(label233); dateLabel.Add(label244); dateLabel.Add(label255);
            dateLabel.Add(label266); dateLabel.Add(label277); dateLabel.Add(label311); dateLabel.Add(label322);
            dateLabel.Add(label333); dateLabel.Add(label344); dateLabel.Add(label355); dateLabel.Add(label366);
            dateLabel.Add(label377); dateLabel.Add(label411); dateLabel.Add(label422); dateLabel.Add(label433);
            dateLabel.Add(label444); dateLabel.Add(label455); dateLabel.Add(label466); dateLabel.Add(label477);
            dateLabel.Add(label511); dateLabel.Add(label522); dateLabel.Add(label533); dateLabel.Add(label544);
            dateLabel.Add(label555); dateLabel.Add(label566); dateLabel.Add(label577); dateLabel.Add(label611);
            dateLabel.Add(label622); dateLabel.Add(label633); dateLabel.Add(label644); dateLabel.Add(label655);
            dateLabel.Add(label666); dateLabel.Add(label677);
            //initTodayInfo();
        }

        private void yearAndMonth_comboBoxEx_SelectedIndexChanged(object sender, EventArgs e)//初始化日历总入口
        {
            DevComponents.Editors.ComboItem yearItem = (DevComponents.Editors.ComboItem)year_comboBoxEx.SelectedItem;
            DevComponents.Editors.ComboItem monthItem = (DevComponents.Editors.ComboItem)month_comboBoxEx.SelectedItem;
            if (yearItem != null && monthItem != null && yearItem.Text != "" && monthItem.Text != "")
            {
                this.initCalendar(Convert.ToInt32(yearItem.Text), Convert.ToInt32(monthItem.Text));//1初始化日历日期数字
                initAttendanceDate();//2初始化出勤信息attendanceList
                initPanelDate();//3初始化假日信息holidayList 上班时间usuallyDayList 请假信息leaveList 调休信息workDayList
                updateComponent();//4attendenceLabel_Paint更新label  attendencePanel_Paint更新panel
            }
        }

        #region 1初始化日历数字
        /// <summary>
        /// 初始化日历日期
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        private void initCalendar(int year, int month)//初始化日历日期数字
        {
            DateTime selectDay = new DateTime(year, month, 1);
            CNDate selectDateTool = new CNDate(selectDay);
            CNDate beforeDateTool = new CNDate(month == 1 ? new DateTime(year - 1, 12, 1) : new DateTime(year, month - 1, 1));
            switch (selectDay.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    initDayOfCalendar(0, selectDateTool.GetDayNumOfMonth(), beforeDateTool.GetDayNumOfMonth()); break;
                case DayOfWeek.Monday:
                    initDayOfCalendar(1, selectDateTool.GetDayNumOfMonth(), beforeDateTool.GetDayNumOfMonth()); break;
                case DayOfWeek.Tuesday:
                    initDayOfCalendar(2, selectDateTool.GetDayNumOfMonth(), beforeDateTool.GetDayNumOfMonth()); break;
                case DayOfWeek.Wednesday:
                    initDayOfCalendar(3, selectDateTool.GetDayNumOfMonth(), beforeDateTool.GetDayNumOfMonth()); break;
                case DayOfWeek.Thursday:
                    initDayOfCalendar(4, selectDateTool.GetDayNumOfMonth(), beforeDateTool.GetDayNumOfMonth()); break;
                case DayOfWeek.Friday:
                    initDayOfCalendar(5, selectDateTool.GetDayNumOfMonth(), beforeDateTool.GetDayNumOfMonth()); break;
                case DayOfWeek.Saturday:
                    initDayOfCalendar(6, selectDateTool.GetDayNumOfMonth(), beforeDateTool.GetDayNumOfMonth()); break;
            }
        }
        /// <summary>
        /// 初始化日历日期子函数
        /// </summary>
        /// <param name="startDay"></param>
        /// <param name="allDay"></param>
        /// <param name="lastMonthLastDay"></param>
        private void initDayOfCalendar(int startDay, int allDay, int lastMonthLastDay)//上一个函数的子函数，将DateTime信息存放在各panel的tag中
        {
            for (int i = 0; i < dateLabel.Count; i++)
            {
                if (i < startDay)//第一行上个月的日期
                {
                    dateLabel[i].Text = (lastMonthLastDay - (startDay - i) + 1).ToString();
                    dateLabel[i].Parent.ForeColor = SystemColors.ControlDark;
                    if (!month_comboBoxEx.Text.Equals("01"))
                    {
                        dateLabel[i].Parent.Tag = new DateTime(Convert.ToInt32(year_comboBoxEx.Text), Convert.ToInt32(month_comboBoxEx.Text) - 1, lastMonthLastDay - (startDay - i) + 1);
                    }
                    else
                    {
                        dateLabel[i].Parent.Tag = new DateTime(Convert.ToInt32(year_comboBoxEx.Text) - 1, 12, lastMonthLastDay - (startDay - i) + 1);
                    }
                }
                else if (i >= (startDay + allDay))
                {
                    dateLabel[i].Text = (i - (startDay + allDay) + 1).ToString();
                    dateLabel[i].Parent.ForeColor = SystemColors.ControlDark;
                    if (!month_comboBoxEx.Text.Equals("12"))
                    {
                        dateLabel[i].Parent.Tag = new DateTime(Convert.ToInt32(year_comboBoxEx.Text), Convert.ToInt32(month_comboBoxEx.Text) + 1, i - (startDay + allDay) + 1);
                    }
                    else
                    {
                        dateLabel[i].Parent.Tag = new DateTime(Convert.ToInt32(year_comboBoxEx.Text) + 1, 1, i - (startDay + allDay) + 1);
                    }

                }
                else
                {
                    dateLabel[i].Text = (i - startDay + 1).ToString();
                    dateLabel[i].Parent.ForeColor = SystemColors.ControlText;
                    dateLabel[i].Parent.Tag = new DateTime(Convert.ToInt32(year_comboBoxEx.Text), Convert.ToInt32(month_comboBoxEx.Text), i - startDay + 1);
                }
            }

        }
        #endregion
        #region 2初始化出勤信息
        private void initAttendanceDate()//初始化出勤信息放入attendanceList
        {
            if (dateLabel[dateLabel.Count - 1].Parent.Tag != null && dateLabel[0].Parent.Tag != null)
            {
                DateTime startTime = (DateTime)dateLabel[0].Parent.Tag;
                DateTime endTime = (DateTime)dateLabel[dateLabel.Count - 1].Parent.Tag;
                attendanceList = baseService.loadEntityList("from Attendance where STATE=" + (int)IEntity.stateEnum.Normal + " and User=" + user.Id + " and SignDate>=" + startTime.Date.Ticks + " and SignDate<=" + endTime.Date.Ticks);
            }
        }
        #endregion
        #region 3初始化假日信息holidayList 上班时间usuallyDayList 请假信息leaveList 调休信息workDayList
        private void initPanelDate()//初始化假日信息holidayList 上班时间usuallyDayList 请假信息leaveList 调休信息workDayList
        {
            if (dateLabel[dateLabel.Count - 1].Parent.Tag != null && dateLabel[0].Parent.Tag != null)
            {
                DateTime startTime = (DateTime)dateLabel[0].Parent.Tag;
                DateTime endTime = (DateTime)dateLabel[dateLabel.Count - 1].Parent.Tag;
                holidayList = baseService.loadEntityList("from Holiday where STATE=" + (int)IEntity.stateEnum.Normal + " and ((StartTime>=" + startTime.Date.Ticks + " and StartTime<=" + endTime.Date.Ticks + ") or (EndTime>=" + startTime.Date.Ticks + " and EndTime<=" + endTime.Date.Ticks + ") or (StartTime>=" + startTime.Date.Ticks + " and EndTime<=" + endTime.Date.Ticks + ") or (StartTime<=" + startTime.Date.Ticks + " and EndTime>=" + endTime.Date.Ticks + "))");
                workDayList = baseService.loadEntityList("from WorkDay where STATE=" + (int)IEntity.stateEnum.Normal + " and workDateTime>=" + startTime.Date.Ticks + " and workDateTime<=" + endTime.Date.Ticks);
                leaveList = baseService.loadEntityList("from LeaveManage where State=" + (int)IEntity.stateEnum.Normal + " and Ku_Id=" + user.Id + " and LeaveResult=2s and ((StartTime>=" + startTime.Date.Ticks + " and StartTime<=" + endTime.Date.Ticks + ") or (EndTime>=" + startTime.Date.Ticks + " and EndTime<=" + endTime.Date.Ticks + ") or (StartTime>=" + startTime.Date.Ticks + " and EndTime<=" + endTime.Date.Ticks + ") or (StartTime<=" + startTime.Date.Ticks + " and EndTime>=" + endTime.Date.Ticks + "))");
                IList usuallyDayList = baseService.loadEntityList("from UsuallyDay where STATE=" + (int)IEntity.stateEnum.Normal + " and StartTime<=" + startTime.Date.Ticks + " order by StartTime desc");


                ///<summary>
                ///？？只能适用一条信息
                ///</summary>
                if (usuallyDayList != null && usuallyDayList.Count == 1)
                {
                    UsuallyDay u = (UsuallyDay)usuallyDayList[0];
                    usuallyDay = u.WorkDay.ToCharArray();
                }
            }
        }
        #endregion
        #region 4attendenceLabel_Paint更新label  attendencePanel_Paint更新panel
        private void updateComponent()
        {
            attendenceLabel_Paint(label11); attendenceLabel_Paint(label21); attendenceLabel_Paint(label31); attendenceLabel_Paint(label41); attendenceLabel_Paint(label51); attendenceLabel_Paint(label61);
            attendenceLabel_Paint(label12); attendenceLabel_Paint(label22); attendenceLabel_Paint(label32); attendenceLabel_Paint(label42); attendenceLabel_Paint(label52); attendenceLabel_Paint(label62);
            attendenceLabel_Paint(label13); attendenceLabel_Paint(label23); attendenceLabel_Paint(label33); attendenceLabel_Paint(label43); attendenceLabel_Paint(label53); attendenceLabel_Paint(label63);
            attendenceLabel_Paint(label14); attendenceLabel_Paint(label24); attendenceLabel_Paint(label34); attendenceLabel_Paint(label44); attendenceLabel_Paint(label54); attendenceLabel_Paint(label64);
            attendenceLabel_Paint(label15); attendenceLabel_Paint(label25); attendenceLabel_Paint(label35); attendenceLabel_Paint(label45); attendenceLabel_Paint(label55); attendenceLabel_Paint(label65);
            attendenceLabel_Paint(label16); attendenceLabel_Paint(label26); attendenceLabel_Paint(label36); attendenceLabel_Paint(label46); attendenceLabel_Paint(label56); attendenceLabel_Paint(label66);
            attendenceLabel_Paint(label17); attendenceLabel_Paint(label27); attendenceLabel_Paint(label37); attendenceLabel_Paint(label47); attendenceLabel_Paint(label57); attendenceLabel_Paint(label67);

            attendencePanel_Paint(panel11); attendencePanel_Paint(panel21); attendencePanel_Paint(panel31); attendencePanel_Paint(panel41); attendencePanel_Paint(panel51); attendencePanel_Paint(panel61);
            attendencePanel_Paint(panel12); attendencePanel_Paint(panel22); attendencePanel_Paint(panel32); attendencePanel_Paint(panel42); attendencePanel_Paint(panel52); attendencePanel_Paint(panel62);
            attendencePanel_Paint(panel13); attendencePanel_Paint(panel23); attendencePanel_Paint(panel33); attendencePanel_Paint(panel43); attendencePanel_Paint(panel53); attendencePanel_Paint(panel63);
            attendencePanel_Paint(panel14); attendencePanel_Paint(panel24); attendencePanel_Paint(panel34); attendencePanel_Paint(panel44); attendencePanel_Paint(panel54); attendencePanel_Paint(panel64);
            attendencePanel_Paint(panel15); attendencePanel_Paint(panel25); attendencePanel_Paint(panel35); attendencePanel_Paint(panel45); attendencePanel_Paint(panel55); attendencePanel_Paint(panel65);
            attendencePanel_Paint(panel16); attendencePanel_Paint(panel26); attendencePanel_Paint(panel36); attendencePanel_Paint(panel46); attendencePanel_Paint(panel56); attendencePanel_Paint(panel66);
            attendencePanel_Paint(panel17); attendencePanel_Paint(panel27); attendencePanel_Paint(panel37); attendencePanel_Paint(panel47); attendencePanel_Paint(panel57); attendencePanel_Paint(panel67);
        }

        /// <summary>
        /// 渲染考勤信息
        /// </summary>
        /// <param name="label"></param>
        private void attendenceLabel_Paint(Label label)
        {
            label.Text = "";
            DateTime date = (DateTime)label.Parent.Tag;
            if (attendanceList != null && attendanceList.Count > 0)
            {
                foreach (Attendance a in attendanceList)
                {
                    if (a.SignDate == date.Date.Ticks)
                    {
                        label.Text += a.SignStartTime != 0 ? CNDate.getTimeByTimeTicks(a.SignStartTime) : "";
                        label.Text += "~";
                        label.Text += a.SignEndTime != 0 ? CNDate.getTimeByTimeTicks(a.SignEndTime) : "";
                        attendanceList.Remove(a);
                        break;
                    }
                }
            }
            if (holidayList != null && holidayList.Count > 0)//判断是否节假日
            {
                foreach (Holiday a in holidayList)
                {
                    if (a.StartTime <= date.Date.Ticks && a.EndTime > date.Date.Ticks)
                    {
                        label.Text = a.Name;
                        return;
                    }
                }
            }
        }
        /// <summary>
        /// 渲染图片信息
        /// </summary>
        /// <param name="label"></param>
        private void attendencePanel_Paint(Panel panel)
        {
            try
            {
                panel.BackgroundImage = WorkLogForm.Properties.Resources.日历小方块2;
                DateTime date = (DateTime)panel.Tag;
                int dayOfWeek = (int)date.DayOfWeek == 0 ? 6 : (int)date.DayOfWeek - 1;
                if (leaveList != null && leaveList.Count > 0)
                {
                    foreach (LeaveManage a in leaveList)
                    {
                        if (a.StartTime <= date.Date.Ticks && a.EndTime >= date.Date.Ticks)
                        {
                            //请假图片！！！@！！！！！！！
                            if (a.LeaveType == "婚假")
                            {
                                panel.BackgroundImage = WorkLogForm.Properties.Resources.日历小方块2_婚;
                            }
                            else if (a.LeaveType == "探亲假")
                            {
                                panel.BackgroundImage = WorkLogForm.Properties.Resources.日历小方块2_亲;
                            }
                            else if (a.LeaveType == "年休假")
                            {
                                panel.BackgroundImage = WorkLogForm.Properties.Resources.日历小方块2_年;
                            }
                            else if (a.LeaveType == "事假")
                            {
                                panel.BackgroundImage = WorkLogForm.Properties.Resources.日历小方块2_事;
                            }
                            else if (a.LeaveType == "病假")
                            {
                                panel.BackgroundImage = WorkLogForm.Properties.Resources.日历小方块2_病;
                            }
                            else if (a.LeaveType == "产假")
                            {
                                panel.BackgroundImage = WorkLogForm.Properties.Resources.日历小方块2_产;
                            }
                            break;
                        }
                    }
                }
                if (workDayList != null && workDayList.Count > 0)//判断是不是补班
                {
                    foreach (WorkDay a in workDayList)
                    {
                        if (a.WorkDateTime == date.Date.Ticks)
                        {
                            return;
                        }
                    }
                }
                if (holidayList != null && holidayList.Count > 0)//判断是否节假日
                {
                    foreach (Holiday a in holidayList)
                    {
                        if (a.StartTime <= date.Date.Ticks && a.EndTime >= date.Date.Ticks)
                        {
                            //Image img = new Bitmap(panel.BackgroundImage);
                            //Graphics panelG = Graphics.FromImage(img);
                            //panelG.DrawString(a.Name, new Font("华文楷体",(float)28.0,FontStyle.Regular),System.Drawing.Brushes.Red, new PointF(22, 45));
                            panel.BackgroundImage = WorkLogForm.Properties.Resources.日历小方块2_休;//节假日休息图片，还没有做！！！@！！！！！！！
                            return;
                        }
                    }
                }
                if (usuallyDay[dayOfWeek].Equals((char)UsuallyDay.workDayEnum.Holiday))//判断是否周六日
                {
                    panel.BackgroundImage = WorkLogForm.Properties.Resources.日历小方块2_休;//周六日休息图片，还没有做！！！@！！！！！！！
                }
            }
            catch
            { }
        }
        #endregion

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
            //this.Visible = false;
            this.Opacity = 0;
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
        private void shou_ye_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.x_point = e.X;
                this.y_point = e.Y;
            }
        }

        private void shou_ye_MouseMove(object sender, MouseEventArgs e)
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


        private void calendar_ovalShape_left_Click(object sender, EventArgs e)
        {
            if (month_comboBoxEx.SelectedIndex != 0)
            {
                month_comboBoxEx.SelectedIndex = month_comboBoxEx.SelectedIndex - 1;
            }
            else if (month_comboBoxEx.SelectedIndex == 0)
            {
                if (year_comboBoxEx.SelectedIndex != 0)
                {
                    year_comboBoxEx.SelectedIndex = year_comboBoxEx.SelectedIndex - 1;
                    month_comboBoxEx.SelectedIndex = 11;
                }
            }
        }
        private void calendar_ovalShape_right_Click(object sender, EventArgs e)
        {
            if (month_comboBoxEx.SelectedIndex != 11)
            {
                month_comboBoxEx.SelectedIndex = month_comboBoxEx.SelectedIndex + 1;
            }
            else if (month_comboBoxEx.SelectedIndex == 11)
            {
                if (year_comboBoxEx.SelectedIndex != year_comboBoxEx.Items.Count - 1)
                {
                    year_comboBoxEx.SelectedIndex = year_comboBoxEx.SelectedIndex + 1;
                    month_comboBoxEx.SelectedIndex = 0;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Visible = true;
            this.Opacity = 1;
            timer1.Stop();
        }
        private void shou_ye_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.timer1.Start();
            }
        }
        private void pictureBox_richeng_Click(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            viewSchedule schedule = new viewSchedule();
            schedule.User = this.user;
            schedule.ScheduleDate = (DateTime)p.Parent.Tag;
            schedule.ShowDialog();
        }
        private void pictureBox_richeng_MouseEnter(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            p.BackgroundImage = WorkLogForm.Properties.Resources.rcOn;
        }
        private void pictureBox_richeng_MouseLeave(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            p.BackgroundImage = WorkLogForm.Properties.Resources._263;
        }
        private void pictureBox_richeng_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip((Control)sender, "查看日程");
        }
        private void pictureBox_rizhi_Click(object sender, EventArgs e)
        {
            PictureBox p = new PictureBox();
            p = (PictureBox)sender;
            writeLog log = new writeLog();
            log.IsView = true;
            log.User = this.user;
            log.LogDate = (DateTime)p.Parent.Tag;
            log.ShowDialog();
        }
        private void pictureBox_rizhi_MouseEnter(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            p.BackgroundImage = WorkLogForm.Properties.Resources.rzOn;
        }
        private void pictureBox_rizhi_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip((Control)sender, "查看日志");
        }
        private void pictureBox_rizhi_MouseLeave(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            p.BackgroundImage = WorkLogForm.Properties.Resources._257;
        }
        /// <summary>
        /// 加载今日阴历、阳历、天气信息
        /// </summary>
        private void initTodayInfo()
        {
            CNDate cnDate = new CNDate(DateTime.Now);
            today_label1.Text = cnDate.GetDateAsShort() + " " + cnDate.GetDayOfWeek();
            today_label2.Text = DateTime.Now.Day.ToString();
            today_label3.Text = cnDate.GetLunarHolDay();
            today_label4.Text = cnDate.FormatLunarYear();
            WeatherService.WeatherWebServiceSoapClient wwsc = new WeatherService.WeatherWebServiceSoapClient();
            string[] weathers = wwsc.getWeatherbyCityName("石家庄");
            today_weather_label.Text = weathers[6];
            today_weather_label1.Text = weathers[5];
            today_weather_label2.Text = weathers[7];
            tomorrow_weather_label.Text = weathers[13];
            tomorrow_weather_label1.Text = weathers[12];
            tomorrow_weather_label2.Text = weathers[14];
            tomorrow2_weather_label.Text = weathers[18];
            tomorrow2_weather_label1.Text = weathers[17];
            tomorrow2_weather_label2.Text = weathers[19];
            today_weather_pictureBox1.BackgroundImage = new Bitmap(Application.StartupPath.ToString() + "\\Weather\\" + weathers[8]);
            today_weather_pictureBox2.BackgroundImage = new Bitmap(Application.StartupPath.ToString() + "\\Weather\\" + weathers[9]);
            tomorrow_weather_pictureBox1.BackgroundImage = new Bitmap(Application.StartupPath.ToString() + "\\Weather\\" + weathers[15]);
            tomorrow_weather_pictureBox2.BackgroundImage = new Bitmap(Application.StartupPath.ToString() + "\\Weather\\" + weathers[16]);
            tomorrow2_weather_pictureBox1.BackgroundImage = new Bitmap(Application.StartupPath.ToString() + "\\Weather\\" + weathers[20]);
            tomorrow2_weather_pictureBox2.BackgroundImage = new Bitmap(Application.StartupPath.ToString() + "\\Weather\\" + weathers[21]);
        }
    }
}
