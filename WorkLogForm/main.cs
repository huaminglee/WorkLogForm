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



        #region 鼠标不动关闭系统用到字段

        private DCIEngine.FrameWork.Snap.Hook.KeyboardMouseHook keyboardHook = new DCIEngine.FrameWork.Snap.Hook.KeyboardMouseHook(DCIEngine.FrameWork.Snap.Hook.HookTypeEnum.KeyboardHook);
        private DCIEngine.FrameWork.Snap.Hook.KeyboardMouseHook mouseHook = new DCIEngine.FrameWork.Snap.Hook.KeyboardMouseHook(DCIEngine.FrameWork.Snap.Hook.HookTypeEnum.MouseHook);

        private int theXPosition = -100;
        /// <summary>
        /// 用于计时;
        /// </summary>
        private int timeCount;

        #endregion


        private List<KjqbService.LogInService> loglistfromService;
        private List<KjqbService.ScheduleInService> schedulelistfromService;
        private List<KjqbService.CommentInService> commentlistfromService;
        private List<KjqbService.TimeArrangeForManagerInService> tfmListfromservice;
        private List<KjqbService.LeaveInService> levlistfromservice;
        private List<KjqbService.BusinessService> businessfromservice;
        private List<KjqbService.ChatInService> chatinservice;

        private List<WkTUser> chattinguserlist = new List<WkTUser>();

        private IList scheduleList;
        private EventHandler mouseLeave;
        private EventHandler mouseEnter;
        private int height,width;
        private BaseService baseService = new BaseService();
        private WkTUser user;
        private WkTRole role;
        Secretary sec;
       
        int isWriteLog;
        /// <summary>
        ///判断是否写过日志
        /// </summary>
        public int IsWriteLog
        {
            get { return isWriteLog; }
            set { isWriteLog = value; }
        }
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
        private NewMessageWindow newMessageWindow;
        private InstantMessenger InstantMessengerWindows;
        KjqbService.Service1Client ser = new KjqbService.Service1Client();
        #endregion
        
        public main()
        {
            InitializeComponent();
            this.Visible = false;
            this.Opacity = 0;
            timer_show.Start();
        }
        #region 界面滚动
        private void MouseWeelTest(object sender, MouseEventArgs e)
        {
            panelScroll(sender, e.Delta);
        }

        private void panelScroll(object label, int delta)
        {
            Panel panel = (Panel)label;
            int mVSValue = panel.VerticalScroll.Value;
            int pScrollValueDelta = delta;

            if ((mVSValue - pScrollValueDelta) <= panel.VerticalScroll.Minimum)
            {
                panel.VerticalScroll.Value = panel.VerticalScroll.Minimum;
            }
            else if ((mVSValue - pScrollValueDelta) >= panel.VerticalScroll.Maximum)
            {
                panel.VerticalScroll.Value = panel.VerticalScroll.Maximum;
            }
            else
            {
                panel.VerticalScroll.Value -= pScrollValueDelta;
            }

            if (panel.VerticalScroll.Value != mVSValue)
            {
                return;
            }
            panel.Refresh();
            panel.Invalidate();
            panel.Update();
        }

        #endregion

        private void main_Load(object sender, EventArgs e)
        {

            initialWindow();
            initialData();//显示日程 日志 考勤
            schedule_listen_timer.Start();//监听日程提醒
            listen_ri_cheng();//监测日程表变动

            this.rc_flowLayoutPanel.MouseWheel += new MouseEventHandler(MouseWeelTest);
            this.rz_flowLayoutPanel.MouseWheel += new MouseEventHandler(MouseWeelTest);
            this.Show_SuiBi_flowPanel.MouseWheel += new MouseEventHandler(MouseWeelTest);

            if(this.user.Kdid.KdName.Trim() != "综合办公室" )
            {
                sjgl_pictureBox.Visible = false;
            }

             IList staffLogList = baseService.loadEntityList
                 ("from StaffLog where State=" 
                 + (int)IEntity.stateEnum.Normal + 
                 " and WriteTime=" + DateTime.Now.Date.Ticks + " and Staff=" + user.Id);

             if (staffLogList != null && staffLogList.Count > 0)
             {
                 isWriteLog = 1;
             }
            #region 开启时读取未读的推送信息
            ////////////////////////////////

             try
             {
                 loglistfromService = new List<KjqbService.LogInService>();
                 schedulelistfromService = new List<KjqbService.ScheduleInService>();
                 commentlistfromService = new List<KjqbService.CommentInService>();
                 tfmListfromservice = new List<KjqbService.TimeArrangeForManagerInService>();
                 levlistfromservice = new List<KjqbService.LeaveInService>();
                 businessfromservice = new List<KjqbService.BusinessService>();
                 chatinservice = new List<KjqbService.ChatInService>();
                 KjqbService.LogInService[] lists;
                 lists = ser.SearchShareLogUnRead((int)this.user.Id);
                 this.labelNewMEssageCount.Text = (int.Parse(this.labelNewMEssageCount.Text) + lists.Length).ToString();
                 for (int i = 0; i < lists.Length; i++)
                 {
                     loglistfromService.Add(lists[i]);
                 }

                 KjqbService.ScheduleInService[] list2;
                 list2 = ser.SearchShareScheduleUnRead((int)this.user.Id);
                 this.labelNewMEssageCount.Text = (int.Parse(this.labelNewMEssageCount.Text) + list2.Length).ToString();
                 for (int i = 0; i < list2.Length; i++)
                 {
                     schedulelistfromService.Add(list2[i]);
                 }

                 KjqbService.CommentInService[] list3;
                 list3 = ser.SearchCommentlogUnRead((int)this.user.Id);
                 this.labelNewMEssageCount.Text = (int.Parse(this.labelNewMEssageCount.Text) + list3.Length).ToString();
                 for (int i = 0; i < list3.Length; i++)
                 {
                     commentlistfromService.Add(list3[i]);
                 }

                 KjqbService.TimeArrangeForManagerInService[] list4;
                 list4 = ser.SearchTimeArrangeForManagerUnRead((int)this.user.Id);
                 this.labelNewMEssageCount.Text = (int.Parse(this.labelNewMEssageCount.Text) + list4.Length).ToString();
                 for (int i = 0; i < list4.Length; i++)
                 {
                     tfmListfromservice.Add(list4[i]);
                 }

                 KjqbService.LeaveInService[] lists5;
                 lists5 = ser.SearchLeaveInfoUnRead((int)this.user.Id);
                 for (int i = 0; i < lists5.Length; i++)
                 {
                     levlistfromservice.Add(lists5[i]);
                 }

                 this.labelNewMEssageCount.Text = (int.Parse(this.labelNewMEssageCount.Text) + lists5.Length).ToString();
                 KjqbService.BusinessService[] lists6;
                 lists6 = ser.SearchBusinessInfoUnRead((int)this.user.Id);
                 for (int i = 0; i < lists6.Length; i++)
                 {
                     businessfromservice.Add(lists6[i]);
                 }

                 this.labelNewMEssageCount.Text = (int.Parse(this.labelNewMEssageCount.Text) + lists6.Length).ToString();

                 #region 接受聊天信息
                 try
                 {
                     KjqbService.ChatInService[] lists7;
                     lists7 = ser.SearchChatInfoUnRead((int)this.user.Id);
                     for (int i = 0; i < lists7.Length; i++)
                     {
                         chatinservice.Add(lists7[i]);

                     }
                     if (this.LabelofChatttingCount.Text == "")
                         this.LabelofChatttingCount.Text = lists7.Length == 0 ? "" : lists7.Length.ToString();
                     else
                         this.LabelofChatttingCount.Text = (int.Parse(this.LabelofChatttingCount.Text) + lists7.Length).ToString();

                     if (lists7.Length > 0)
                     {
                         timerchattingflesh.Enabled = true;
                     }
                 }
                 catch { }
                 #endregion

             }
             catch
             {
                 MessageBox.Show("未能与服务器建立连接……");
             }


            #endregion

             mouseHook.InstallHook(OnMousePress);
             keyboardHook.InstallHook(OnKeyboardPress);
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
            //sec = new Secretary(); //右下角的
        }

       
        /// <summary>
        /// 
        /// </summary>
        private void initialData()
        {
            if (user != null)
            {
                #region 向数据库发送登陆信息
                user.KuOnline = 1;
                baseService.SaveOrUpdateEntity(user);

                #endregion

                #region 登陆签到及显示考勤


                DateTime today ;
                try
                {
                    today = ser.GetServiceTime();

                }
                catch
                {
                    today = DateTime.Now;
                }

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
                        try
                        {
                            todaySignStart.TimeStamp = ser.GetServiceTime().Ticks;
                        }
                        catch
                        {
                            todaySignStart.TimeStamp = DateTime.Now.Ticks;
                        }
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
                IList staffScheduleList = baseService.loadEntityList("from StaffSchedule where STATE=" + (int)IEntity.stateEnum.Normal + " and Staff=" + user.Id + " and ScheduleTime>=" + thisDay + " order by ScheduleTime asc");
                if (scheduleList != null)
                {
                    scheduleList.Clear();
                }
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
                    int height = ((ss.Content.Length / 14) + 1) * 20;
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
            Point newPoint = new Point(0, this.rz_flowLayoutPanel.Height - rz_flowLayoutPanel.AutoScrollPosition.Y);
            rz_flowLayoutPanel.AutoScrollPosition = newPoint;
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
                if (newMessageWindow != null && newMessageWindow.Created)
                {
                    newMessageWindow.Top = MousePosition.Y - y;
                    newMessageWindow.Left = MousePosition.X - x - newMessageWindow.Width;
                }
                if (InstantMessengerWindows != null && InstantMessengerWindows.Created)
                {
                    InstantMessengerWindows.Top = MousePosition.Y - y;
                    InstantMessengerWindows.Left = MousePosition.X - x - InstantMessengerWindows.Width;
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
                if (newMessageWindow != null && newMessageWindow.Created)
                {
                    newMessageWindow.Top = MousePosition.Y - y;
                    newMessageWindow.Left = MousePosition.X - x - newMessageWindow.Width;
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




        

        #endregion

        #region 最小化关闭按钮
        private void min_pictureBox_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }
        private void close_pictureBox_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要退出吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                this.Close();
            }
        }
        #endregion

        #region 图片渐变效果

        private void min_pictureBox_MouseEnter(object sender, EventArgs e)
        {
            min_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.最小化渐变;
        }
        private void min_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            min_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.最小化2;
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

        #region 右下角的图标点击事件
        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            //EventArgs继承自MouseEventArgs,所以可以强转  
            MouseEventArgs Mouse_e = (MouseEventArgs)e;

            //点鼠标右键,return  
            if (Mouse_e.Button == MouseButtons.Left)
            {
                this.Visible = true;
                this.Activate();
            }
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
            if (SuiBi_flowLayoutPanel.Visible == false)
            {
                tong_xun_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.个人随笔1;
            }
        }
        #endregion  

        #region 主界面切换
        private void ri_cheng_pictureBox1_Click(object sender, EventArgs e)
        {   /*************测试代码*************************/
            //if (sec == null)
            //{
            //    sec = new Secretary(this);
            //    sec.Show();
            //    sec.AddMessageLabelInFlowPanel1("程倩");
            //    sec.AddRiChengInFlow2(DateTime.Now.ToString("MM-dd HH:mm"), "测0试大白！！！！");
            //}
            //else
            //{
            //    if (sec.IsDisposed)
            //    {
            //        sec = new Secretary(this);
            //        sec.Show();
            //    }
            //    sec.AddMessageLabelInFlowPanel1("程倩1");
            //    sec.AddMessageLabelInFlowPanel1("程倩2");
            //    sec.AddMessageLabelInFlowPanel1("程倩3");
            //    sec.AddRiChengInFlow2(DateTime.Now.ToString("MM-dd HH:mm"), "测1试大白！！！！");
            //}
           /*************测试代码*************************/

            ri_zhi_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.日志分享;
            rz_flowLayoutPanel.Visible = false;
            rc_flowLayoutPanel.Visible = true;
            SuiBi_flowLayoutPanel.Visible = false;
            tong_xun_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.个人随笔1;
            init_rc_Panel();
          
            
        }
        private void ri_zhi_pictureBox_Click(object sender, EventArgs e)
        {

            ri_cheng_pictureBox1.BackgroundImage = WorkLogForm.Properties.Resources.我的日程;
            rz_flowLayoutPanel.Visible = true;
            rc_flowLayoutPanel.Visible = false;
            SuiBi_flowLayoutPanel.Visible = false;
            tong_xun_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.个人随笔1;
            init_rz_Panel();
          
        }

        private void tong_xun_pictureBox_Click(object sender, EventArgs e)
        {
            tong_xun_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.个人随笔_副本;
            rz_flowLayoutPanel.Visible = false;
            rc_flowLayoutPanel.Visible = false;
            SuiBi_flowLayoutPanel.Visible = true;
            ri_zhi_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.日志分享;
            ri_cheng_pictureBox1.BackgroundImage = WorkLogForm.Properties.Resources.我的日程;
            RefreshSuiBi();

        }




        #endregion



        #region 业务逻辑控件事件

        private void pictureBoxOfInstantMessenger_Click(object sender, EventArgs e)
        {
            if(sec != null)
            {
                sec.ClearMessagePanel();
                this.chattinguserlist.Clear();
            }
            pictureBoxOfInstantMessenger.Cursor = Cursors.WaitCursor;
            this.timerOfReceiveChattingMessage.Enabled = false;

            this.timerchattingflesh.Enabled = false;
            this.pictureBoxOfInstantMessenger.BackgroundImage = WorkLogForm.Properties.Resources.InstantMessengerLogo;
            this.LabelofChatttingCount.Text = "";
            if (InstantMessengerWindows == null || InstantMessengerWindows.IsDisposed)
            {
                InstantMessengerWindows = new InstantMessenger();
            }
            if (!InstantMessengerWindows.Created)
            {
                InstantMessengerWindows.FormLocation = new Point(this.Location.X - InstantMessengerWindows.Width, this.Location.Y);
                InstantMessengerWindows.User = this.user;
                InstantMessengerWindows.MainReceiveMessage = this.timerOfReceiveChattingMessage;
                InstantMessengerWindows.Chattinguserlist = this.chatinservice;
                InstantMessengerWindows.Show();

            }
            else
            {
                InstantMessengerWindows.WindowState = FormWindowState.Normal;
                InstantMessengerWindows.Chattinguserlist = this.chatinservice;
                InstantMessengerWindows.Visible = true;
                
            }
            pictureBoxOfInstantMessenger.Cursor = Cursors.Hand;

        }
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
            if (write_log.DialogResult == DialogResult.OK)
            {
                if (isWriteLog == 0)
                {
                    isWriteLog = 1;
                }
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
            if (suibiguanli == null || suibiguanli.IsDisposed)
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
            try
            {
                if (user != null)
                {
                    user.KuOnline = 0;
                    baseService.SaveOrUpdateEntity(user);

                    DateTime today;
                    try
                    {
                        today = ser.GetServiceTime();
                    }
                    catch
                    {
                        today = DateTime.Now;

                    }

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
            catch 
            {
            
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
            try
            {
                long thisDay = DateTime.Now.Ticks;
                long nextDay = DateTime.Now.Date.Ticks + new DateTime(1, 1, 2).Date.Ticks;

                //设置监测
                OnChangeEventHandler onChange = new OnChangeEventHandler(ri_cheng_onChange);
                BaseService.autoUpdateForm(onChange, "select ID from [dbo].LOG_T_STAFFSCHEDULE where WkTUserId=" + user.Id + " and STATE=" + (int)IEntity.stateEnum.Normal + " and IfRemind=" + (int)StaffSchedule.IfRemindEnum.Renmind + " and ScheduleTime>=" + thisDay + " and ShceduleTime<" + nextDay);
            }
            catch 
            { 
            
            }
        }
        private void ri_cheng_onChange(object sender, SqlNotificationEventArgs e)
        {
            try
            {
                //MessageBox.Show(e.Info.ToString());
                long thisDay = DateTime.Now.Ticks;
                long nextDay = DateTime.Now.Date.Ticks + new DateTime(1, 1, 2).Date.Ticks;
                scheduleList = baseService.loadEntityList("from StaffSchedule where STATE=" + (int)IEntity.stateEnum.Normal + " and Staff=" + user.Id + " and ScheduleTime>=" + thisDay + "  order by ScheduleTime asc");

                //循环监测
                listen_ri_cheng();
            }
            catch
            {
 
            
            }
        }
        #endregion

        #region 日程提醒
        private void schedule_listen_timer_Tick(object sender, EventArgs e)//设置日程提醒
        {
            IList sl = scheduleList;
            if (sl != null && sl.Count > 0)
            {
                foreach (StaffSchedule ss in sl)
                {
                    DateTime scheduleTime = new DateTime(ss.ScheduleTime);
                    if (scheduleTime.Hour ==DateTime.Now.Hour && scheduleTime.Minute == DateTime.Now.Minute &&DateTime.Now.Year == scheduleTime.Year)
                    {
                        if (sec == null)
                        {
                            sec = new Secretary(this);
                            sec.Show();
                            sec.AddRiChengInFlow2(scheduleTime.ToString("MM-dd HH:mm"), ss.Subject);
                        }
                        else
                        {
                            if (sec.IsDisposed)
                            {
                                sec = new Secretary(this);
                                sec.Show();
                            }
                            sec.AddRiChengInFlow2(scheduleTime.ToString("MM-dd HH:mm"), ss.Subject);
                        }
                        
                        if (scheduleList.Contains(ss))
                        {
                            scheduleList.Remove(ss);
                            break;
                        }
                    }
                }
            }
        }
        #endregion

        #region 边栏图片效果
        private void spgl_pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            spgl_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.个人考勤副本;
        }

        private void spgl_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            spgl_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.个人考勤;
        }

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
                this.linkLabel10.Enabled = true;
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

               
                this.linkLabel10.Enabled = false;
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
            this.linkLabel10.Enabled = true;
        }
        private void RefreshSuiBi()
        {
            this.Show_SuiBi_flowPanel.Controls.Clear();
            ShowSuiBiInFlowPanel(10);
            this.linkLabel10.Enabled = true;
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


        #region 刷新 
        /// <summary>
        /// 手动更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBoxOfrefresh_Click(object sender, EventArgs e)
        {
            init_rc_Panel();
            init_rz_Panel();
            RefreshSuiBi();
        }
        /// <summary>
        ///自动更新 //更新之后无法记录看到什么位置了，会回到最顶端 暂时搁置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshAll_Tick(object sender, EventArgs e)
        {
            //init_rc_Panel();
            //init_rz_Panel();
            //RefreshSuiBi();
        }

        #endregion

        

        #region 提醒写日志
        /// <summary>
        /// 提醒写日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemindwriteLog_Tick(object sender, EventArgs e)
        {
            if(DateTime.Now.Hour == 17 && DateTime.Now.Minute == 10)
            {
                if (sec == null)
                {
                    sec = new Secretary(this);
                    sec.Show();
                    sec.SetWriteIsVis();
                }
                else
                {
                    if (sec.IsDisposed)
                    {
                        sec = new Secretary(this);
                        sec.Show();
                    }
                    sec.SetWriteIsVis();
                }
            }
        }
        #endregion

        private void rc_flowLayoutPanel_MouseEnter(object sender, EventArgs e)
        {
            Panel p = (Panel)sender;
            p.Focus();
        }

        #region 消息推送
        private void panelNewMessage_Click(object sender, EventArgs e)
        {
            panelNewMessage.Cursor = Cursors.WaitCursor;

            this.labelNewMEssageCount.Text = "0";

            try
            {
                ser.SetShareLogIsRead((int)this.user.Id);
                ser.SetShareScheduleIsRead((int)this.user.Id);
                ser.SetCommentLogIsRead((int)this.user.Id);
                ser.SetTimeArrangeForManagerIsRead((int)this.user.Id);
                ser.SetLeaveInfoIsRead((int)this.user.Id);
                ser.SetBusinessInfoIsRead((long)this.user.Id);
            }
            catch
            {
                
            }
            
            //同时刷新窗体
            init_rc_Panel();
            init_rz_Panel();
            RefreshSuiBi();

            if (newMessageWindow == null || newMessageWindow.IsDisposed)
            {
                newMessageWindow = new NewMessageWindow();
            }
            if (!newMessageWindow.Created)
            {
                newMessageWindow.FormLocation = new Point(this.Location.X - newMessageWindow.Width, this.Location.Y);

                 
                newMessageWindow.Loglist = loglistfromService;
                newMessageWindow.Schedulelist = schedulelistfromService;
                newMessageWindow.CommentList = commentlistfromService;
                newMessageWindow.Tfmlist = tfmListfromservice;
                newMessageWindow.Levlist = levlistfromservice;
                newMessageWindow.Buslist = businessfromservice;
                newMessageWindow.User = this.user;
                newMessageWindow.Role = this.role;
                newMessageWindow.LeaveWindow = leave;
                newMessageWindow.BusinessManagement = businessManagement;
                newMessageWindow.Show();

            }
            else
            {
                newMessageWindow.WindowState = FormWindowState.Normal;
                newMessageWindow.Focus();
            }

            panelNewMessage.Cursor = Cursors.Hand;
        }

        private void timerMessageSend_Tick(object sender, EventArgs e)
        {
            try
            {
                KjqbService.LogInService[] lists;
                lists = ser.SearchShareLog((int)this.user.Id);
                for (int i = 0; i < lists.Length; i++)
                {
                    loglistfromService.Add(lists[i]);
                }

                this.labelNewMEssageCount.Text = (int.Parse(this.labelNewMEssageCount.Text) + lists.Length).ToString();

                KjqbService.ScheduleInService[] lists2;
                lists2 = ser.SearchShareSchedule((int)this.user.Id);
                for (int i = 0; i < lists2.Length; i++)
                {
                    schedulelistfromService.Add(lists2[i]);
                }

                this.labelNewMEssageCount.Text = (int.Parse(this.labelNewMEssageCount.Text) + lists2.Length).ToString();

                KjqbService.CommentInService[] lists3;
                lists3 = ser.SearchCommentlog((int)this.user.Id);
                for (int i = 0; i < lists3.Length; i++)
                {
                    commentlistfromService.Add(lists3[i]);
                }

                this.labelNewMEssageCount.Text = (int.Parse(this.labelNewMEssageCount.Text) + lists3.Length).ToString();

                KjqbService.TimeArrangeForManagerInService[] lists4;
                lists4 = ser.SearchTimeArrangeForManager((int)this.user.Id);
                for (int i = 0; i < lists4.Length; i++)
                {
                    tfmListfromservice.Add(lists4[i]);
                }

                this.labelNewMEssageCount.Text = (int.Parse(this.labelNewMEssageCount.Text) + lists4.Length).ToString();

                KjqbService.LeaveInService[] lists5;
                lists5 = ser.SearchLeaveInfo((int)this.user.Id);
                for (int i = 0; i < lists5.Length; i++)
                {
                    levlistfromservice.Add(lists5[i]);
                }

                this.labelNewMEssageCount.Text = (int.Parse(this.labelNewMEssageCount.Text) + lists5.Length).ToString();

                KjqbService.BusinessService[] lists6;
                lists6 = ser.SearchBusinessInfo((int)this.user.Id);
                for (int i = 0; i < lists6.Length; i++)
                {
                    businessfromservice.Add(lists6[i]);
                }

                this.labelNewMEssageCount.Text = (int.Parse(this.labelNewMEssageCount.Text) + lists6.Length).ToString();
            }
            catch
            {

                this.timerMessageSend.Stop();
                MessageBox.Show("与服务器失去建立连接，可能是由于网络原因，程序将退出，未记录本次签退时间，请在网络正常后再次登录。");
                this.Close();
            
            }
        }
        /// <summary>
        /// 图标闪烁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerchattingflesh_Tick(object sender, EventArgs e)
        {
            if (this.pictureBoxOfInstantMessenger.BackgroundImage != null)
            this.pictureBoxOfInstantMessenger.BackgroundImage = null;
            else
                this.pictureBoxOfInstantMessenger.BackgroundImage = WorkLogForm.Properties.Resources.InstantMessengerLogo;
        }



        private bool IsInChatUserlist(long id)
        {
            if (chattinguserlist != null && chattinguserlist.Count > 0)
            {
                foreach (WkTUser w in chattinguserlist)
                {
                    if (w.Id == id)
                        return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        
        }
        #endregion



        #region 接受聊天信息
        private void timerOfReceiveChattingMessage_Tick(object sender, EventArgs e)
        {
            #region 接受聊天信息
            try
            {
                KjqbService.ChatInService[] lists7;
                lists7 = ser.SearchChatInfo((int)this.user.Id);
                for (int i = 0; i < lists7.Length; i++)
                {
                    chatinservice.Add(lists7[i]);
                    if(!IsInChatUserlist(lists7[i].SendUserId))
                    {
                        WkTUser w = new WkTUser(); ;
                        w = (WkTUser)baseService.loadEntity(w, lists7[i].SendUserId);
                        this.chattinguserlist.Add(w);
                        if (sec == null)
                        {
                            sec = new Secretary(this);
                            sec.Show();
                            sec.AddMessageLabelInFlowPanel1(w.KuName);
                        }
                        else
                        {
                            if (sec.IsDisposed)
                            {
                                sec = new Secretary(this);
                                sec.Show();
                            }
                            sec.AddMessageLabelInFlowPanel1(w.KuName);
                        }
                        this.chattinguserlist.Add(w);
                    }

                    
                }
                if (this.LabelofChatttingCount.Text == "")
                    this.LabelofChatttingCount.Text = lists7.Length == 0 ? "" : lists7.Length.ToString();
                else
                    this.LabelofChatttingCount.Text = (int.Parse(this.LabelofChatttingCount.Text) + lists7.Length).ToString();

                if (lists7.Length > 0)
                {
                    timerchattingflesh.Enabled = true;
                }
            }
            catch { }
            #endregion
        }
        
        #endregion

        /// <summary>
        /// 右键退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 右键还原
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            this.Activate();
        }




        public void OnKeyboardPress(WorkLogForm.DCIEngine.FrameWork.Snap.Hook.KeyboardMouseHook.KeyboardHookStruct hookStruct, out bool isNeedStop)
        {

            isNeedStop = false;

            if (hookStruct.vkCode >= (int)Keys.A && hookStruct.vkCode <= (int)Keys.Z)
            {
                // 读取
                Keys key = (Keys)hookStruct.vkCode;
                //MessageBox.Show("你输入了 " + (key == Keys.None ? "" : key.ToString()) + " 键");
                this.timeCount = 0;
                this.theXPosition = -100;

                // 修改
                //hookStruct.vkCode = (int)Keys.NumPad0;

                // 拦截
                //isNeedStop = true;
            }
        }

        public void OnMousePress(WorkLogForm.DCIEngine.FrameWork.Snap.Hook.KeyboardMouseHook.MouseHookStruct hookStruct, out bool isNeedStop)
        {
            isNeedStop = false;

            // 读取鼠标坐标
            //this.user_label.Text = "（" + hookStruct.pt.x.ToString() + "，" + hookStruct.pt.y.ToString() + "）";

            if (hookStruct.pt.x != this.theXPosition)
            {
                theXPosition = hookStruct.pt.x;
                timeCount = 0;
                this.timerOfMouseOrKeyUnDo.Stop();
            }
            else
            {
                this.timerOfMouseOrKeyUnDo.Start();
            }

            if (hookStruct.mouseAction == WorkLogForm.DCIEngine.FrameWork.Snap.Hook.KeyboardMouseHook.MouseHookStruct.MouseActionEnum.RightButtonUp)
            {
                //读取鼠标动作
                //MessageBox.Show("右击");

                //TODO:修改捕获的鼠标消息

                //慎用，如果同时屏蔽键盘所有键，则只能重启才能退出
                //可以修改成定时自动退出或捕获一定次数后自动退出。。。
                //isNeedStop = true; 
            }
        }


        /// <summary>
        /// 1秒执行一次
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerOfMouseOrKeyUnDo_Tick(object sender, EventArgs e)
        {
            timeCount++;
            if (this.timeCount > 6)//timecount单位秒 40分钟
            {
                //MessageBox.Show("……");
                this.timerOfMouseOrKeyUnDo.Stop();
                this.Close();
            }
        }











    }
}
