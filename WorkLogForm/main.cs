using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ClassLibrary;
using WorkLogForm.WindowUiClass;
using WorkLogForm.Service;
using NHibernate;
using NHibernate.Cfg;
using System.Collections;
using WorkLogForm.CommonClass;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace WorkLogForm
{


    public partial class main : Form
    {
        private IList scheduleList;
        private EventHandler mouseLeave;
        private EventHandler mouseEnter;
        private int height,width;
        private BaseService baseService = new BaseService();
        private WkTUser user;
        private WkTRole role;
        

        /// <summary>
        /// 存储用户角色 用来传值
        /// </summary>
        public WkTRole Role
        {
            get { return role; }
            set { role = value; }
        }

        /// <summary>
        /// 存储具体人员
        /// </summary>
        public WkTUser User
        {
            get { return user; }
            set { user = value; }
        }

        #region 子功能窗体变量
        private statistics_Attendance statisticsAttendance;
        private Examine_Allograph examineAllograph;
        private Apply_Allograph applyAllorapg;
        private writeLog write_log;
        private writeSchedule write_schedule;
        private personal_setting personalSetting;
        private shou_ye shouYe;
        private TimeManagement time_management;
        private WorkLogForm.Leave leave;
        private staff_LogLeader staffLogLeader;
        private schedule_Manage scheduleManage;
        private OnDuty onDuty;
        private BusinessManagement businessManagement;
        private WorkOvertime workOvertime;
        private SuiBiGuanLi_New suibiguanli;
        #endregion
        
        public main()
        {
            InitializeComponent();
            this.Visible = false;
            this.Opacity = 0;
            timer_show.Start();
        }
        private void main_Load(object sender, EventArgs e)
        {
            initialWindow();
            initialData();//显示日程 日志 考勤
            schedule_listen_timer.Start();//监听日程提醒
            listen_ri_cheng();//监测日程表变动

            if(this.user.Kdid.KdName.Trim() != "综合办公室" )
            {
                sjgl_pictureBox.Visible = false;
            }
            
        }

        #region 自定义窗体初始化方法


        /// <summary>
        /// 初始化window（界面效果）
        /// </summary>
        private void initialWindow()
        {
            mouseLeave = new System.EventHandler(main_MouseLeave);
            mouseEnter = new System.EventHandler(main_MouseEnter);
            height = this.Height;
            width = this.Width;
            creatWindow.SetFormRoundRectRgn(this, 15);
            creatWindow.SetFormShadow(this);
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width * 3 / 4, Screen.PrimaryScreen.WorkingArea.Height / 8);
            Secretary sec = new Secretary();
            sec.Show();

        }


        /// <summary>
        /// 
        /// </summary>
        private void initialData()
        {
            if (user != null)
            {
                #region 登陆签到及显示考勤
                DateTime today = DateTime.Now;
                this.user_label.Text = "欢迎登陆，" + user.KuName;
                if (CNDate.isworkDay(today.Date.Ticks))//工作日登录
                {
                    //查询最近的工作起始时间安排
                    IList attendanceList = baseService.loadEntityList("from Attendance where STATE=" + (int)IEntity.stateEnum.Normal + " and User=" + user.Id + " and SignDate=" + today.Date.Ticks);
                    
                    if (attendanceList != null && attendanceList.Count == 1)//今天登录过
                    {
                        Attendance atd = (Attendance)attendanceList[0];
                        attendance_label.Text += atd.SignStartTime != 0 ? CNDate.getTimeByTimeTicks(atd.SignStartTime) : "";
                        attendance_label.Text += "~";
                        attendance_label.Text += atd.SignEndTime != 0 ? CNDate.getTimeByTimeTicks(atd.SignEndTime) : "";
                    }


                    else // 今天没有登陆过
                    {
                        Attendance todaySignStart = new Attendance();//用于记录考勤信息
                        IList usuallyDayList = baseService.loadEntityList("from UsuallyDay where STATE=" 
                            + (int)IEntity.stateEnum.Normal + " and StartTime<=" + today.Date.Ticks +
                            " order by StartTime desc"); //查询作息时间
                        if (usuallyDayList != null && usuallyDayList.Count == 1) //存在作息时间设置
                        {
                            UsuallyDay u = (UsuallyDay)usuallyDayList[0];
                            if (u.WorkTimeStart >= today.TimeOfDay.Ticks)
                            {
                                todaySignStart.LateOrLeaveEarly = (int)Attendance.lateOrLeaveEarlyEnum.Early; // 正常签到
                            }
                            else
                            {
                                todaySignStart.LateOrLeaveEarly = (int)Attendance.lateOrLeaveEarlyEnum.LateAndEarly; // 迟到
                            }
                        }
                        todaySignStart.SignStartTime = today.TimeOfDay.Ticks;
                        todaySignStart.SignDate = today.Date.Ticks;
                        todaySignStart.SignDay = today.Day;
                        todaySignStart.SignMonth = today.Month;
                        todaySignStart.SignYear = today.Year;
                        todaySignStart.State = (int)IEntity.stateEnum.Normal;
                        todaySignStart.TimeStamp = DateTime.Now.Ticks;
                        todaySignStart.User = this.user;
                        try
                        {
                            baseService.SaveOrUpdateEntity(todaySignStart);
                        }
                        catch
                        {
                            MessageBox.Show("签到失败！");
                            return;
                        }
                        attendance_label.Text = CNDate.getTimeByTimeTicks(todaySignStart.SignStartTime) + "~";
                    }
                }
                #endregion

                #region 调用日程显示页
                init_rc_Panel();
                #endregion

                #region 调用日志显示页
                init_rz_Panel();
                #endregion

                #region 调用随笔显示
                ChangeLocationAftercancel();
                ShowSuiBiInFlowPanel(10);
                #endregion
            }
        }


      
        #endregion

        #region 日程显示页
        public void init_rc_Panel()
        {
            if (this.InvokeRequired)
            {
                writeSchedule.ParentFormChange formChangeDelegate = new writeSchedule.ParentFormChange(init_rc_Panel);
                this.Invoke(formChangeDelegate);
            }
            else
            {
                bool rcVisible = rc_flowLayoutPanel.Visible;
                rc_flowLayoutPanel.Visible = false;
                while (rc_flowLayoutPanel.Controls.Count > 0)
                {
                    rc_flowLayoutPanel.Controls.RemoveAt(0); // 删除所有日程消息
                }
                long thisDay = DateTime.Now.Date.Ticks;
                long nextDay = DateTime.Now.Date.Ticks + new DateTime(1, 1, 2).Date.Ticks;
                IList staffScheduleList = baseService.loadEntityList("from StaffSchedule where STATE=" + (int)IEntity.stateEnum.Normal + " and Staff=" + user.Id + " and ScheduleTime>=" + thisDay + " and ScheduleTime<" + nextDay + " order by ScheduleTime asc");
                scheduleList = staffScheduleList; //把查询出来的日程列表付给全局变量
                creat_ri_cheng_Panel(staffScheduleList);
                rc_flowLayoutPanel.Visible = rcVisible;
            }
        }
        /// <summary>
        /// 把日程附加到panel中
        /// </summary>
        /// <param name="rcList"></param>
        private void creat_ri_cheng_Panel(IList rcList)
        {
            rc_flowLayoutPanel.Controls.Clear();
            if (rcList != null && rcList.Count > 0)
            {

                foreach (StaffSchedule ss in rcList)
                {
                    Panel p = new Panel();

                    Label l1Time = new Label();
                    l1Time.AutoSize = true;
                    //l1.Size = new System.Drawing.Size(127, 26);
                    l1Time.Font = new System.Drawing.Font("微软雅黑", 15, FontStyle.Bold);
                    l1Time.Location = new Point(7, 5);
                    l1Time.Text = new DateTime(ss.ScheduleTime).ToString("yyyy-MM-dd HH:mm");
                    l1Time.Parent = p;
                    l1Time.ForeColor = Color.FromArgb(128, 128, 255);
                    
                    if (ss.ArrangeMan.Id != user.Id)
                    {
                        l1Time.ForeColor = Color.Red;
                    }


                    Label l2Name = new Label();
                    l2Name.AutoSize = false;
                    l2Name.Size = new System.Drawing.Size(206, 21);
                    l2Name.Font = new System.Drawing.Font("微软雅黑", 10);
                    l2Name.Text = "安排人:" + ss.ArrangeMan.KuName + "   执行人:"+ ss.Staff.KuName;
                    l2Name.Location = new Point(6, 35);
                    l2Name.Parent = p;

                    Label l3Title = new Label();
                    l3Title.Location = new Point(4, 61);
                    l3Title.Font = new System.Drawing.Font("微软雅黑", 15);
                    l3Title.Parent = p;
                    l3Title.Text = ss.Subject.ToString();

                    Label l4Content = new Label();
                    l4Content.Font = new Font("微软雅黑", 10);
                    l4Content.Location = new Point(2, 90);
                    l4Content.AutoSize = false;
                    l4Content.Text = ss.Content;
                    int height = ((ss.Content.Length / 14) + 1) * 18;
                    l4Content.Size = new Size(210, height);
                    l4Content.Parent = p;

                    p.Size = new Size(219, l4Content.Location.Y + l4Content.Height + 10);

                    p.BorderStyle = BorderStyle.FixedSingle;
                    p.BackColor = Color.FromArgb(224, 224, 224);

                    p.Parent = rc_flowLayoutPanel;

                }

            }
        }
        private void onDeleteLinkLabelClickEventHandler(object sender, EventArgs e)
        {
            LinkLabel linkLabel = (LinkLabel)sender;
            StaffSchedule ss = (StaffSchedule)linkLabel.Tag;
            baseService.deleteEntity(ss);
            init_rc_Panel();
        }
        #endregion

        #region   日志显示页
        public void init_rz_Panel()//mainpage日志显示
        {
            //if (this.InvokeRequired)
            //{
            //    writeSchedule.ParentFormChange formChangeDelegate = new writeSchedule.ParentFormChange(init_rc_Panel);
            //    this.Invoke(formChangeDelegate);
            //}
            //else
            //{
            bool rzVisible = rz_flowLayoutPanel.Visible;
            rz_flowLayoutPanel.Visible = false;
            while (rz_flowLayoutPanel.Controls.Count > 0)
            {
                rz_flowLayoutPanel.Controls.RemoveAt(0);
            }

            string sql = "select t.c,t.t ,w.KU_NAME,t.id  from WK_T_USER w right join " +
                        "(select sl.Contents c,sl.WriteTime t,u.KU_NAME n ,sl.id id,sl.WkTUserId from  " +
                        "StaffLog_M_WkTUser m,LOG_T_STAFFLOG sl,WK_T_USER u where m.KU_ID=u.KU_ID " +
                        "and m.StaffLogId=sl.id and m.KU_ID= " + user.Id + " ) t on t.WkTUserId = w.KU_ID order by t.t desc";
            IList staffLogList = baseService.ExecuteSQL(sql); //查询自己的日志与其他人分享的日志
            creat_ri_zhi_Panel(staffLogList);
            rz_flowLayoutPanel.Visible = rzVisible;
            //}
        }


        private void creat_ri_zhi_Panel(IList rzList)
        {
            if (rzList != null && rzList.Count > 0)
            {
                Font contentLabelFont = new Font("微软雅黑", (float)9, FontStyle.Regular);
                Font otherLabelFont = new Font("宋体", (float)9, FontStyle.Regular);
                for (int i = 0; (i < 3) && (i < rzList.Count); i++)
                {
                    object[] sf = (object[])rzList[i];
                    Panel bgPanel = new Panel();
                    bgPanel.BackColor = System.Drawing.Color.Transparent;
                    bgPanel.BorderStyle = BorderStyle.FixedSingle;
                    bgPanel.Size = new Size(221, 143);
                   
                    PictureBox personImage = new PictureBox();
                    personImage.Size = new Size(44, 44);
                    personImage.Location = new Point(7, 1);
                    personImage.Parent = bgPanel;
                    Label nameLabel = new Label();
                    nameLabel.Font = otherLabelFont;
                    nameLabel.Size = new System.Drawing.Size(41,12);
                    nameLabel.Text = sf[2].ToString();
                    nameLabel.Location = new Point(58, 9);
                    nameLabel.Parent = bgPanel;
                    Label timeLabel = new Label();
                    timeLabel.Font = otherLabelFont;
                    DateTime writeTime = new DateTime(Convert.ToInt64(sf[1].ToString()));
                    timeLabel.Text = writeTime.Year + "年" + writeTime.Month + "月" + writeTime.Day + "日" + " " + CNDate.getTimeByTimeTicks(writeTime.TimeOfDay.Ticks);
                    timeLabel.Location = new Point(58, 28);
                    timeLabel.Parent = bgPanel;
                    Panel contentPanel = new Panel();
                    contentPanel.Size = new Size(213, 69);
                    contentPanel.Location = new Point(2, 51);
                    contentPanel.BackColor = System.Drawing.Color.Transparent;
                    contentPanel.Parent = bgPanel;
                    Label contentLabel = new Label();
                     Regex r = new Regex("<[^<]*>");
                    MatchCollection mc = r.Matches(sf[0].ToString());
                    String contentText = sf[0].ToString().Replace("&nbsp;", " ");
                    for (int j = 0; j < mc.Count; j++)
                    {
                        contentText = contentText.Replace(mc[j].Value, "");
                    }
                    contentLabel.Text = contentText;
                    contentLabel.Dock = DockStyle.Fill;
                    contentLabel.AutoSize = false;
                    contentLabel.Font = contentLabelFont;
                    contentLabel.Padding = new Padding(5, 5, 5, 5);
                    contentLabel.Parent = contentPanel;
                    LinkLabel moreLink = new LinkLabel();
                    moreLink.Text = "点击查看全文";
                    moreLink.Location = new Point(140, 123);
                    moreLink.Parent = bgPanel;
                    moreLink.Click += onMoreLinkLabelClickEventHandler;
                    moreLink.Tag = sf;

                    bgPanel.Parent = rz_flowLayoutPanel;
                }
                if (rzList.Count > 0)
                    rzList.RemoveAt(0);
                if (rzList.Count > 0)
                    rzList.RemoveAt(0);
                if (rzList.Count > 0)
                    rzList.RemoveAt(0);
                if (rzList.Count > 0)
                {
                    Panel morePanel = new Panel();
                    morePanel.Size = new Size(220, 25); //rz_flowLayoutPanel.Controls.Count == 3 ? new Size(2, 25) : new Size(223, 25);
                    morePanel.Parent = rz_flowLayoutPanel;
                    morePanel.BackColor = System.Drawing.Color.Transparent;
                    morePanel.Tag = rzList;
                    
                    LinkLabel moreLinkLabel = new LinkLabel();
                    moreLinkLabel.Text = "点击查看更多";
                    moreLinkLabel.Location = rz_flowLayoutPanel.Controls.Count == 3 ? new Point(75, 12) : new Point(75, 7);
                    moreLinkLabel.Parent = morePanel;
                    moreLinkLabel.Click += moreLinkLabel_Click;
                    //moreLinkLabel.Tag = rzList;
                    //moreLinkLabel.Left = 75;
                }
            }
        }

        void moreLinkLabel_Click(object sender, EventArgs e)
        {
            Panel panel = (Panel)((LinkLabel)sender).Parent;
            IList rzList = (IList)panel.Tag;
            if (panel.Parent.Controls.Count == 4)
            {
                panel.Parent.Controls[0].Size = new Size(221, 143);
                panel.Parent.Controls[1].Size = new Size(221, 143);
                panel.Parent.Controls[2].Size = new Size(221, 143);
            }
            panel.Parent.Controls.Remove(panel);
            creat_ri_zhi_Panel(rzList);
        }

       
        private void onMoreLinkLabelClickEventHandler(object sender, EventArgs e)
        {
            LinkLabel ll = (LinkLabel)sender;
            Object[] obj = (Object[])ll.Tag;
            StaffLog sl = new StaffLog();
            baseService.loadEntity(sl,Convert.ToInt64(obj[3].ToString()));
            writeLog log = new writeLog();
            log.IsComment = true;
            log.User = sl.Staff;
            log.CommentPersonName = User.KuName;
            log.LogDate = new DateTime(sl.WriteTime);
            log.ShowDialog();
        }
      
        #endregion
       
        
        #region 窗体特效事件：窗口拖拽到最上端自动隐藏
        private int x, y;
        private void main_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.x = e.X;
                this.y = e.Y;
            }
        }
        private void main_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left&&this.Location.Y>0)
            {
                if (personalSetting != null && personalSetting.Created)
                {
                    personalSetting.Top = MousePosition.Y - y;
                    personalSetting.Left = MousePosition.X - x - personalSetting.Width;
                }
                Top = MousePosition.Y - y;
                Left = MousePosition.X - x;
            }
            else if (e.Button == MouseButtons.Left && e.Y > this.y)
            {
                if (personalSetting != null && personalSetting.Created)
                {
                    personalSetting.Top = MousePosition.Y - y;
                    personalSetting.Left = MousePosition.X - x - personalSetting.Width;
                }
                Top = MousePosition.Y - y;
                Left = MousePosition.X - x;
            }
        }
        private void main_Move(object sender, EventArgs e)//实现窗口拖拽到最上端自动隐藏
        {
            if (this.Location.Y <= 0)
            {
                this.MouseLeave += mouseLeave;
                this.MouseEnter += mouseEnter;
                this.Top = 0;
            }
        }
        private void main_MouseLeave(object sender, EventArgs e)
        {

            if (this.Location.Y == 0 && (MousePosition.Y >= this.Location.Y + this.height || MousePosition.X <= this.Location.X || this.PointToClient(new Point(MousePosition.X, MousePosition.Y)).X >= this.width))
            {
                timer2.Stop();
                timer1.Start();
            }
        }
        private void main_MouseEnter(object sender, EventArgs e)
        {
            if (this.Location.Y == 0&&(MousePosition.Y<=5||MousePosition.X>=this.Location.X||MousePosition.X<=this.Location.X+this.width))
            {
                timer1.Stop();
                timer2.Start();
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.Height < 10)
            {
                this.Height = 2;
                timer1.Stop();
                return;
            }
            this.Height = this.Height / 5;
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            int h = Height * 5;
            if (h >= this.height)
            {
                this.Height = this.height;
                timer2.Stop();
            }
            else
            {
                this.Height = h;
            }
        }
        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            this.Activate();
        }
        private void min_pictureBox_MouseEnter(object sender, EventArgs e)
        {
            min_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.最小化渐变;
        }
        private void min_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            min_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.最小化2;
        }
        private void min_pictureBox_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }
        private void close_pictureBox_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void close_pictureBox_MouseEnter(object sender, EventArgs e)
        {
            close_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.关闭渐变;
        }
        private void close_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            close_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.关闭1;
        }
        private void setting_pictureBox_MouseEnter(object sender, EventArgs e)
        {
            setting_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.设置;
        }
        private void setting_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            setting_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.设置1;
        }
        private void wdrl_pictureBox_MouseEnter(object sender, EventArgs e)
        {
            wdrl_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.我的日历1;
        }
        private void wdrl_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            wdrl_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.我的日历;
        }
        private void rcgl_pictureBox_MouseEnter(object sender, EventArgs e)
        {
            rcgl_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.日程管理1;
        }
        private void rcgl_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            rcgl_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.日程管理;
        }
        private void rzgl_pictureBox_MouseEnter(object sender, EventArgs e)
        {
            rzgl_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.统计考勤1;
        }

        private void SuiBiGuanLi_pictureBox_MouseEnter(object sender, EventArgs e)
        {
            SuiBiGuanLi_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.随笔1;
        }

        private void SuiBiGuanLi_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            SuiBiGuanLi_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.随笔;
        }


        private void rzgl_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            rzgl_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.统计考勤2;
        }
        private void qjgl_pictureBox_MouseEnter(object sender, EventArgs e)
        {
            qjgl_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.请假管理11;
        }
        private void qjgl_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            qjgl_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.请假管理1;
        }
        private void jbgl_pictureBox_MouseEnter(object sender, EventArgs e)
        {
            jbgl_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.加班管理11;
        }
        private void jbgl_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            jbgl_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.加班管理1;
        }
        private void zbgl_pictureBox_MouseEnter(object sender, EventArgs e)
        {
            zbgl_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.值班管理11;
        }
        private void zbgl_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            zbgl_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.值班管理22;
        }
        private void ccgl_pictureBox_MouseEnter(object sender, EventArgs e)
        {
            ccgl_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.出差管理1;
        }
        private void ccgl_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            ccgl_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.出差管理;
        }
        #endregion
        
        #region 主界面切换按钮
        private void ri_zhi_pictureBox_MouseEnter(object sender, EventArgs e)
        {
            ri_zhi_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.日志分享_副本;
        }
        private void ri_zhi_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            if (rz_flowLayoutPanel.Visible == true)
            {
                return;
            }
            ri_zhi_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.日志分享;
        }
        private void ri_cheng_pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            ri_cheng_pictureBox1.BackgroundImage = WorkLogForm.Properties.Resources.我的日程_副本;
        }
        private void ri_cheng_pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            if (rc_flowLayoutPanel.Visible == true)
            {
                return;
            }
            ri_cheng_pictureBox1.BackgroundImage = WorkLogForm.Properties.Resources.我的日程;
        }
        private void tong_xun_pictureBox_MouseEnter(object sender, EventArgs e)
        {
            tong_xun_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.个人随笔_副本;
        }
        private void tong_xun_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            tong_xun_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.个人随笔1;
        }
        #endregion  

        #region 主界面切换
        private void ri_cheng_pictureBox1_Click(object sender, EventArgs e)
        {
            ri_zhi_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.日志分享;
            rz_flowLayoutPanel.Visible = false;
            rc_flowLayoutPanel.Visible = true;
            SuiBi_flowLayoutPanel.Visible = false;
            
        }
        private void ri_zhi_pictureBox_Click(object sender, EventArgs e)
        {
            ri_cheng_pictureBox1.BackgroundImage = WorkLogForm.Properties.Resources.我的日程;
            rz_flowLayoutPanel.Visible = true;
            rc_flowLayoutPanel.Visible = false;
            SuiBi_flowLayoutPanel.Visible = false;
        }

        private void tong_xun_pictureBox_Click(object sender, EventArgs e)
        {
            tong_xun_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.个人随笔_副本;
            rz_flowLayoutPanel.Visible = false;
            rc_flowLayoutPanel.Visible = false;
            SuiBi_flowLayoutPanel.Visible = true;
        }




        #endregion



        #region 业务逻辑控件事件
         private void spgl_pictureBox_Click(object sender, EventArgs e)
        {
            if (statisticsAttendance == null || statisticsAttendance.IsDisposed)
            {
                statisticsAttendance = new statistics_Attendance();
                statisticsAttendance.User = this.user;
                statisticsAttendance.Role = role;
            }
            if (!statisticsAttendance.Created)
            {
                statisticsAttendance.Show();
            }
            else
            {
                statisticsAttendance.WindowState = FormWindowState.Normal;
                statisticsAttendance.Focus();
            }
        }
        private void dai_qian_pictureBox_Click(object sender, EventArgs e)
        {
            if (applyAllorapg == null || applyAllorapg.IsDisposed)
            {
                applyAllorapg = new Apply_Allograph();
                applyAllorapg.User = this.user;
                applyAllorapg.Role = role;
            }
            if (!applyAllorapg.Created)
            {
                applyAllorapg.Show();
            }
            else
            {
                applyAllorapg.WindowState = FormWindowState.Normal;
                applyAllorapg.Focus();
            }
        }
        private void dai_qian_sp_pictureBox_Click(object sender, EventArgs e)
        {
            if (examineAllograph == null || examineAllograph.IsDisposed)
            {
                examineAllograph = new Examine_Allograph();
                examineAllograph.User = this.user;
                examineAllograph.Role = role;
            }
            if (!examineAllograph.Created)
            {
                examineAllograph.Show();
            }
            else
            {
                examineAllograph.WindowState = FormWindowState.Normal;
                examineAllograph.Focus();
            }
        }
        private void setting_pictureBox_Click(object sender, EventArgs e)
        {
            setting_pictureBox.Cursor = Cursors.WaitCursor;
            if (personalSetting==null||personalSetting.IsDisposed)
            {
                personalSetting = new personal_setting();
                personalSetting.User = user;
                personalSetting.Role = role;
            }
            if (!personalSetting.Created)
            {
                personalSetting.FormLocation = new Point(this.Location.X - personalSetting.Width, this.Location.Y);
                personalSetting.Show();
            }
            else
            {
                personalSetting.WindowState = FormWindowState.Normal;
                personalSetting.Focus();
            }
            setting_pictureBox.Cursor = Cursors.Hand;

        }
        private void log_pictureBox_Click(object sender, EventArgs e)
        {
            if (write_log == null || write_log.IsDisposed)
            {
                write_log = new writeLog();
                write_log.User = this.user;
            }
            if (!write_log.Created)
            {
                write_log.Show();
            }
            else
            {
                write_log.WindowState = FormWindowState.Normal;
                write_log.Focus();
            }
        }
        private void schedule_pictureBox_Click(object sender, EventArgs e)
        {
            if (write_schedule == null || write_schedule.IsDisposed)
            {
                writeSchedule.ParentFormChange formChangeDelegate = new writeSchedule.ParentFormChange(init_rc_Panel);
                write_schedule = new writeSchedule(formChangeDelegate);
                write_schedule.User = this.user;
                write_schedule.Role = this.role;
                write_schedule.ScheduleDate = DateTime.Now;
            }
            if (!write_schedule.Created)
            {
                write_schedule.Show();
            }
            else
            {
                write_schedule.WindowState = FormWindowState.Normal;
                write_schedule.Focus();
            }
        }
        private void wdrl_pictureBox_Click(object sender, EventArgs e)
        {
            wdrl_pictureBox.Cursor = Cursors.WaitCursor;
            if (shouYe == null || shouYe.IsDisposed)
            {
                shouYe = new shou_ye();
            }
            if (!shouYe.Created)
            {
                shouYe.User = this.user;
                shouYe.Show();
            }
            else
            {
                shouYe.WindowState = FormWindowState.Normal;
                shouYe.Focus();
            }
            wdrl_pictureBox.Cursor = Cursors.Hand;
        }
        private void sjgl_pictureBox_Click(object sender, EventArgs e)
        {
            sjgl_pictureBox.Cursor = Cursors.WaitCursor;
            if (time_management == null || time_management.IsDisposed)
            {
                time_management = new TimeManagement();
            }
            if (!time_management.Created)
            {
                time_management.User = user;
                time_management.Show();
            }
            else
            {
                time_management.WindowState = FormWindowState.Normal;
                time_management.Focus();
            }
            sjgl_pictureBox.Cursor = Cursors.Hand;
        }
        private void qjgl_pictureBox_Click(object sender, EventArgs e)
        {
            if (leave == null || leave.IsDisposed)
            {
                leave = new Leave();
                leave.Leaveman = this.user;
                leave.Role = role;
            }
            if (!leave.Created)
            {
                leave.Show();
            }
            else
            {
                leave.WindowState = FormWindowState.Normal;
                leave.Focus();
            }
        }
        private void rzgl_pictureBox_Click(object sender, EventArgs e)
        {
            if (staffLogLeader == null || staffLogLeader.IsDisposed)
            {
                staffLogLeader = new staff_LogLeader();
                staffLogLeader.User = this.user;
                staffLogLeader.Role = role;
            }
            if (!staffLogLeader.Created)
            {
                staffLogLeader.Show();
            }
            else
            {
                staffLogLeader.WindowState = FormWindowState.Normal;
                staffLogLeader.Focus();
            }
        }
        private void rcgl_pictureBox_Click(object sender, EventArgs e)
        {
            if (scheduleManage == null || scheduleManage.IsDisposed)
            {
                scheduleManage = new schedule_Manage();
                scheduleManage.User = this.user;
                scheduleManage.Role = role;
            }
            if (!scheduleManage.Created)
            {
                scheduleManage.Show();
            }
            else
            {
                scheduleManage.WindowState = FormWindowState.Normal;
                scheduleManage.Focus();
            }
        }
        private void jbgl_pictureBox_Click(object sender, EventArgs e)
        {
            if (workOvertime == null || workOvertime.IsDisposed)
            {
                workOvertime = new WorkOvertime();
            }
            if (!workOvertime.Created)
            {
                workOvertime.User = this.User;
                workOvertime.Role = this.Role;
                workOvertime.Show();
            }
            else
            {
                workOvertime.WindowState = FormWindowState.Normal;
                workOvertime.Focus();
            }
        }
        private void zbgl_pictureBox_Click(object sender, EventArgs e)
        {
            zbgl_pictureBox.Cursor = Cursors.WaitCursor;
            if (onDuty == null || onDuty.IsDisposed)
            {
                onDuty = new OnDuty();
                
            }
            if (!onDuty.Created)
            {
                onDuty.User = this.User;
                onDuty.Role = this.Role;
                onDuty.Show();
            }
            else
            {
                onDuty.WindowState = FormWindowState.Normal;
                onDuty.Focus();
               
            }
            zbgl_pictureBox.Cursor = Cursors.Hand;

        }
        private void ccgl_pictureBox_Click(object sender, EventArgs e)
        {
            if (businessManagement == null || businessManagement.IsDisposed)
            {
                businessManagement = new BusinessManagement();
            }
            if (!businessManagement.Created)
            {
                businessManagement.User = this.User;
                businessManagement.Role = this.Role;
                businessManagement.Show();
            }
            else
            {
                businessManagement.WindowState = FormWindowState.Normal;
                businessManagement.Focus();
            }
        }
       
        private void SuiBiGuanLi_pictureBox_Click(object sender, EventArgs e)
        {
            SuiBiGuanLi_pictureBox.Cursor = Cursors.WaitCursor;
            if (shouYe == null || shouYe.IsDisposed)
            {
                suibiguanli = new SuiBiGuanLi_New();
            }
            if (!suibiguanli.Created)
            {
                suibiguanli.User = this.user;
                suibiguanli.Show();
            }
            else
            {
                suibiguanli.WindowState = FormWindowState.Normal;
                suibiguanli.Focus();
            }
            SuiBiGuanLi_pictureBox.Cursor = Cursors.Hand;

            //SuiBiGuanLi_New suibiguanli = new SuiBiGuanLi_New();
           // suibiguanli.User = this.user;
            //suibiguanli.ShowDialog();
        }

        #endregion





        #region 退出时签到相关事件及方法
        /// <summary>
        /// 退出时签到
        /// </summary>
        private void signExit()
        {
            if (user != null)
            {
                DateTime today = DateTime.Now;
                if (CNDate.isworkDay(today.Date.Ticks))
                {
                    IList attendanceList = baseService.loadEntityList("from Attendance where STATE=" + (int)IEntity.stateEnum.Normal + " and User=" + user.Id + " and SignDate=" + today.Date.Ticks);
                    if (attendanceList != null && attendanceList.Count == 1)
                    {
                        Attendance todaySignStart = (Attendance)attendanceList[0];
                        IList usuallyDayList = baseService.loadEntityList("from UsuallyDay where STATE=" + (int)IEntity.stateEnum.Normal + " and StartTime<=" + today.Date.Ticks + " order by StartTime desc");
                        if (usuallyDayList != null && usuallyDayList.Count == 1)
                        {
                            UsuallyDay u = (UsuallyDay)usuallyDayList[0];

                            if (u.WorkTimeEnd <= today.TimeOfDay.Ticks)//未早退
                            {
                                if (todaySignStart.LateOrLeaveEarly == (int)Attendance.lateOrLeaveEarlyEnum.LateAndEarly)  //登陆为LateAndEarly表示迟到
                                {
                                    todaySignStart.LateOrLeaveEarly = (int)Attendance.lateOrLeaveEarlyEnum.Late; // 只是迟到
                                }
                                else
                                {
                                    todaySignStart.LateOrLeaveEarly = (int)Attendance.lateOrLeaveEarlyEnum.Normal;  //  正常签到
                                }
                            }


                            else //早退
                            {
                                if (todaySignStart.LateOrLeaveEarly == (int)Attendance.lateOrLeaveEarlyEnum.LateAndEarly)
                                {
                                    todaySignStart.LateOrLeaveEarly = (int)Attendance.lateOrLeaveEarlyEnum.LateAndEarly; //迟到并且早退
                                }
                                else
                                {
                                    todaySignStart.LateOrLeaveEarly = (int)Attendance.lateOrLeaveEarlyEnum.Early; //只是早退
                                }
                            }
                            
                        }
                        todaySignStart.SignEndTime = today.TimeOfDay.Ticks;
                        todaySignStart.SignDate = today.Date.Ticks;
                        todaySignStart.SignDay = today.Day;
                        todaySignStart.SignMonth = today.Month;
                        todaySignStart.SignYear = today.Year;
                        todaySignStart.State = (int)IEntity.stateEnum.Normal;
                        todaySignStart.TimeStamp = DateTime.Now.Ticks;
                        todaySignStart.User = this.user;
                        try
                        {
                            baseService.SaveOrUpdateEntity(todaySignStart);
                        }
                        catch
                        {
                            MessageBox.Show("签退失败！");
                            return;
                        }
                        attendance_label.Text = CNDate.getTimeByTimeTicks(todaySignStart.SignStartTime) + "~";
                    }
                }
            }
        }
        public const int WM_QUERYENDSESSION = 0x11;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_QUERYENDSESSION)
            {
                signExit();
                m.Result = new IntPtr(1);
            }
            base.WndProc(ref m);
        }
        private void main_FormClosing(object sender, FormClosingEventArgs e)
        {
            string ip = IniReadAndWrite.IniReadValue("connect", "ip");
            string id = IniReadAndWrite.IniReadValue("connect", "id");
            string pwd = IniReadAndWrite.IniReadValue("connect", "pwd");
            string db = IniReadAndWrite.IniReadValue("connect", "db");
            SqlDependency.Stop("UID=" + Securit.DeDES(id) + ";PWD=" + Securit.DeDES(pwd) + ";Database=" + Securit.DeDES(db) + ";server=" + Securit.DeDES(ip));
            notifyIcon1.Dispose();
            signExit();
        }
        #endregion


        private void timer_show_Tick(object sender, EventArgs e)//显示界面
        {
            this.Visible = true;
            this.Opacity = 1;
            timer_show.Stop();
        }



        #region 监测日程更新
        private void listen_ri_cheng()
        {
            long thisDay = DateTime.Now.Ticks;
            long nextDay = DateTime.Now.Date.Ticks + new DateTime(1, 1, 2).Date.Ticks;

            //设置监测
            OnChangeEventHandler onChange = new OnChangeEventHandler(ri_cheng_onChange);
            BaseService.autoUpdateForm(onChange, "select ID from [dbo].LOG_T_STAFFSCHEDULE where WkTUserId=" + user.Id + " and STATE=" + (int)IEntity.stateEnum.Normal + " and IfRemind=" + (int)StaffSchedule.IfRemindEnum.Renmind+" and ScheduleTime>="+thisDay+" and ShceduleTime<"+nextDay);
        }
        private void ri_cheng_onChange(object sender, SqlNotificationEventArgs e)
        {
            //MessageBox.Show(e.Info.ToString());
            long thisDay = DateTime.Now.Ticks;
            long nextDay = DateTime.Now.Date.Ticks + new DateTime(1, 1, 2).Date.Ticks;
            scheduleList = baseService.loadEntityList("from StaffSchedule where STATE=" + (int)IEntity.stateEnum.Normal + " and Staff=" + user.Id + " and ScheduleTime>=" + thisDay + " and ScheduleTime<" + nextDay + " order by ScheduleTime asc");
            
            //循环监测
            listen_ri_cheng();
        }
        #endregion

        private void schedule_listen_timer_Tick(object sender, EventArgs e)//设置日程提醒
        {
            IList sl = scheduleList;
            if (sl != null && sl.Count > 0)
            {
                foreach (StaffSchedule ss in sl)
                {
                    DateTime scheduleTime = new DateTime(ss.ScheduleTime);
                    if (scheduleTime.Hour == DateTime.Now.Hour && scheduleTime.Minute == DateTime.Now.Minute)
                    {
                        MessageBox.Show(ss.Content);
                        if (scheduleList.Contains(ss))
                        {
                            scheduleList.Remove(ss);
                            break;
                        }
                    }
                }
            }
        }

        #region 边栏图片效果
        private void spgl_pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            spgl_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.个人考勤副本;
        }

        private void spgl_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            spgl_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.个人考勤;
        }

        //private void dai_qian_sp_pictureBox_MouseMove(object sender, MouseEventArgs e)
        //{
        //    dai_qian_sp_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.代签审批副本;
        //}

        //private void dai_qian_sp_pictureBox_MouseLeave(object sender, EventArgs e)
        //{
        //    dai_qian_sp_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.代签审批;
        //}

        //private void dai_qian_pictureBox_MouseMove(object sender, MouseEventArgs e)
        //{
        //    dai_qian_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.补签1;
        //}

        //private void dai_qian_pictureBox_MouseLeave(object sender, EventArgs e)
        //{
        //    dai_qian_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.补签;
        //}

        private void sjgl_pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            sjgl_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.系统管理1;
        }

        private void sjgl_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            sjgl_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.系统管理;
        }
        #endregion


        #region 随笔功能
        private void Write_SUiBi_textBox_TextChanged(object sender, EventArgs e)
        {
            if (this.Write_SUiBi_textBox.Text.Length > 140 && this.Write_SUiBi_textBox.Text.Length == 0)
            {
                this.Pulish_button.Enabled = false;
            }
            else
            {
                this.Pulish_button.Enabled = true;
            
            }
        }

        /// <summary>
        /// 在库中插入一条随笔
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pulish_button_Click(object sender, EventArgs e)
        {
            if (this.Write_SUiBi_textBox.Text.Length == 0)
            {
                MessageBox.Show("您还没有输入内容！");
            }
            else
            {
                //注入数据
                SuiBi newsuibi = new SuiBi();
                newsuibi.WkTUserId = user;
                newsuibi.WriteTime = DateTime.Now.Ticks;
                newsuibi.Contents = this.Write_SUiBi_textBox.Text;
                newsuibi.State = (int)IEntity.stateEnum.Normal;
                newsuibi.TimeStamp = DateTime.Now.Ticks;

                //插入数据库

                try
                {
                    baseService.SaveOrUpdateEntity(newsuibi);
                }
                catch
                {
                    MessageBox.Show("发布随笔失败！");
                    return;
                }
                //清空内容
                this.Write_SUiBi_textBox.Text = "";
                MessageBox.Show("发布成功！");


                //更新展示栏中内容

                while (Show_SuiBi_flowPanel.Controls.Count > 0)
                {
                    Show_SuiBi_flowPanel.Controls.RemoveAt(0); // 删除所有日程消息
                }
                ShowSuiBiInFlowPanel(10);
                this.SuiBi_SeeMore.Enabled = true;
                ChangeLocationAftercancel();
            }

        }

        /// <summary>
        /// 点击查看更多
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowSuiBiInFlowPanel(this.Show_SuiBi_flowPanel.Controls.Count+10);

        }

        public void ShowSuiBiInFlowPanel(int end)
        { 


            //从数据库中查出区间内容

            String listCountSql = "select COUNT(*) from SuiBi;";

            IList CountSuibi = baseService.ExecuteSQL(listCountSql);
            int count  = 0;
            foreach (object[] o in CountSuibi)
            {
                count = Convert.ToInt32(o[0].ToString());
            }
            string sql;
            if (end < count)
            {

                sql = "select * from ( " +
                             "select top 10 * from ( " +
                             "select top " + end.ToString() + " * from  " +
                             "(Select  WK_T_USER.KU_NAME as name , _t.Contents as content, " +
                             "_t.WriteTime as time , _t.Id as id  from (select top 100 percent * " +
                             "from SuiBi where SuiBi.STATE = 0 order by SuiBi.WriteTime desc) as _t left join " +
                             "WK_T_USER on _t.WkTUserId = WK_T_USER.KU_ID ) as t order by t.time desc ) as tt order by tt.time asc ) as ttt order by ttt.time desc";

            }
            else
            {

                sql = "select * from ( " +
                                "select top " + (count % 10).ToString() + " * from ( " +
                                "select top " + (count+10).ToString() + " * from  " +
                                "(Select  WK_T_USER.KU_NAME as name , _t.Contents as content, " +
                                "_t.WriteTime as time , _t.Id as id  from (select top 100 percent * " +
                                "from SuiBi order by SuiBi.WriteTime desc) as _t left join " +
                                "WK_T_USER on _t.WkTUserId = WK_T_USER.KU_ID ) as t order by t.time desc ) as tt order by tt.time asc ) as ttt order by ttt.time desc";

               
                this.SuiBi_SeeMore.Enabled = false;
            }


            
            IList SuiBiList = baseService.ExecuteSQL(sql);
            

            if(SuiBiList!= null && SuiBiList.Count != 0)
            {
                foreach (object[] o in SuiBiList)
                {
                   //造控件

                    Panel newpanel = new Panel();
                    
                    Label Name = new Label();
                    Name.Font = new Font(new FontFamily("微软雅黑"), 12, FontStyle.Bold);
                    Name.AutoSize = true;
                    Name.Text = o[0].ToString();
                    Name.Location = new Point(5, 5);

                    Label content = new Label();
                    content.Font = new Font(new FontFamily("微软雅黑"), 10);
                    content.AutoSize = false;
                    
                    
                    int contentheight = ((o[1].ToString().Length /14)+1) * 21;
                    content.Size = new Size(209, contentheight);
                    content.Text = o[1].ToString();
                    content.Location = new Point(5 , 5 + Name.Height);

                    Label time = new Label();
                    time.Font = new Font(new FontFamily("微软雅黑"), 9);
                    time.AutoSize = true;
                    time.Location = new Point(68, Name.Height + content.Height + 15);
                    time.Text = new DateTime(Convert.ToInt64(o[2].ToString())).ToString("yyyy年MM月dd日 HH:mm");

                    newpanel.Size = new Size(215, Name.Height + content.Height + time.Height + 25);
                    newpanel.Parent = Show_SuiBi_flowPanel;

                    Name.Parent = newpanel;
                    content.Parent = newpanel;
                    time.Parent = newpanel;

                   
                }
            }
        
        }

        /// <summary>
        /// 随笔刷新功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel11_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Show_SuiBi_flowPanel.Controls.Clear();
            ShowSuiBiInFlowPanel(10);
        }

        /// <summary>
        /// 写随笔
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel12_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Show_SuiBi_flowPanel.Height = 293;
            this.SuiBi_SeeMore.Height = 169;
            this.write_SuiBi.Visible = true;
            this.panel8.Location = new Point(panel8.Location.X, write_SuiBi.Height+5);
        }

        /// <summary>
        /// 取消发布按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            ChangeLocationAftercancel();
        }

        public void ChangeLocationAftercancel()
        {
            this.SuiBi_SeeMore.Height = 45;
            this.Show_SuiBi_flowPanel.Height = 420;
            this.write_SuiBi.Visible = false;
            this.panel8.Location = new Point(this.panel8.Location.X, 7);
        }

        #endregion

        

       



        

      

      

       

      

       

    }
}
