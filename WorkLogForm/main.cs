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
using CCWin;
using ChattingCtrl._ChatListBox;
using System.Net;
using CommonClass;
using System.IO;
using ChattingCtrl;

namespace WorkLogForm
{


    public partial class main : SkinMain
    {


        #region
        /// <summary>
        /// 版本号
        /// </summary>
        private string versionNum;
        #endregion

        #region 鼠标不动关闭系统用到字段

        private DCIEngine.FrameWork.Snap.Hook.KeyboardMouseHook keyboardHook = new DCIEngine.FrameWork.Snap.Hook.KeyboardMouseHook(DCIEngine.FrameWork.Snap.Hook.HookTypeEnum.KeyboardHook);
        private DCIEngine.FrameWork.Snap.Hook.KeyboardMouseHook mouseHook = new DCIEngine.FrameWork.Snap.Hook.KeyboardMouseHook(DCIEngine.FrameWork.Snap.Hook.HookTypeEnum.MouseHook);

        private int theXPosition = -100;
        /// <summary>
        /// 用于计时;
        /// </summary>
        private int timeCount;

        #endregion


        FileUpDown fileUpDown;
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
        int WheelCount = 0;


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

        private delegate void LoadUserlistDele();
        private delegate void LoadBaseInfoDele();
        private delegate void LoadUnreadMessage();
        private delegate void LoadOtherDele();


        System.Timers.Timer timerofOnline = new System.Timers.Timer(1);
        System.Timers.Timer CreativeFileUpDownClass = new System.Timers.Timer(1);
    
      
        #region 子功能窗体变量
        private statistics_Attendance statisticsAttendance;
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
        int loadCount;
        #endregion
        
        public main(string versionNum)
        {
            InitializeComponent();
            loadCount = 0;
            //CheckForIllegalCrossThreadCalls = false;  
            
            timerofOnline.Elapsed += timerofOnline_Elapsed;
            CreativeFileUpDownClass.Elapsed += CreativeFileUpDownClass_Elapsed;
           
            

            timerofOnline.AutoReset = false;
            timerofOnline.Enabled = true;
          
           


            CreativeFileUpDownClass.AutoReset = false;
            CreativeFileUpDownClass.Enabled = true;
            
            this.versionNum = versionNum;
        }

       

        private void TimerOfShowWindow_Elapsed(object sender, EventArgs e)
        {
            #region 页面渐显
            if (this.Opacity != 1)
            {
                this.Opacity = this.Opacity + 0.1;  //((double)(255 - this.SkinOpacity) / (double)255);
                //this.SkinOpacity = this.SkinOpacity - 1;
            }
            else
            {
                this.TimerOfShowWindow.Stop();
                this.TimerOfShowWindow.Enabled = false;
            }
            #endregion
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

            int i1 = panel.VerticalScroll.Value + panel.Height;
            int i2 = panel.VerticalScroll.Maximum;
            if (i1 /i2 ==1)
            {
                WheelCount++;
                if (WheelCount == 3)
                {
                    panel.Cursor = Cursors.WaitCursor;
                    if (panel.Name == "Show_SuiBi_flowPanel")
                    {
                        ToSeeMoreSuiBi();
                    }
                    else if (panel.Name == "rc_flowLayoutPanel")
                    {
                        init_rc_Panel();
                    }
                    else if (panel.Name == "rz_flowLayoutPanel")
                    {
                        init_rz_Panel();
                    }
                    WheelCount = 0;
                    panel.Cursor = Cursors.Default;
                }
               
            }

            panel.Refresh();
            panel.Invalidate();
            panel.Update();
        }

        #endregion


        #region 窗体加载函数
        private void main_Load(object sender, EventArgs e)
        {
            this.Opacity = 0;
            this.labelVersionNum.Text = versionNum;
            initialWindow();
           
            this.backgroundWorkerLoadBaseInfo.RunWorkerAsync();
            this.backgroundWorkerofOtherWork.RunWorkerAsync();
            this.backgroundWorkerLoadUnreadMessage.RunWorkerAsync();
            this.backgroundWorkerLoadUserList.RunWorkerAsync();
            
            
            schedule_listen_timer.Start();//监听日程提醒
            
            listen_ri_cheng();//监测日程表变动
            backgroundWorkerOfDownPicture.RunWorkerAsync();
           
            //LoadUnReadMessage();
            //initialData();
            //LoadUserList();
            //LoadofInstallHookAndReadDeptAndIsWriteLog();//安装钩子 判断部门 添加滚轮事件 判断是否写日志
           
        }

       

    

        #endregion

        private void LoadUnReadMessage()
        {

            if (this.InvokeRequired)
            {
                LoadUnreadMessage d = new LoadUnreadMessage(LoadUnReadMessage);
                this.Invoke(d);
            }
            else
            {
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


                    //this.labelNewMEssageCount.Text = (int.Parse(this.labelNewMEssageCount.Text) + lists.Length).ToString();


                    for (int i = 0; i < lists.Length; i++)
                    {
                        loglistfromService.Add(lists[i]);
                    }

                    KjqbService.ScheduleInService[] list2;
                    list2 = ser.SearchShareScheduleUnRead((int)this.user.Id);

                    //this.labelNewMEssageCount.Text = (int.Parse(this.labelNewMEssageCount.Text) + list2.Length).ToString();

                    for (int i = 0; i < list2.Length; i++)
                    {
                        schedulelistfromService.Add(list2[i]);
                    }

                    KjqbService.CommentInService[] list3;
                    list3 = ser.SearchCommentlogUnRead((int)this.user.Id);
                    //this.labelNewMEssageCount.Text = (int.Parse(this.labelNewMEssageCount.Text) + list3.Length).ToString();
                    for (int i = 0; i < list3.Length; i++)
                    {
                        commentlistfromService.Add(list3[i]);
                    }

                    KjqbService.TimeArrangeForManagerInService[] list4;
                    list4 = ser.SearchTimeArrangeForManagerUnRead((int)this.user.Id);
                    //this.labelNewMEssageCount.Text = (int.Parse(this.labelNewMEssageCount.Text) + list4.Length).ToString();
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

                    //this.labelNewMEssageCount.Text = (int.Parse(this.labelNewMEssageCount.Text) + lists5.Length).ToString();

                    KjqbService.BusinessService[] lists6;
                    lists6 = ser.SearchBusinessInfoUnRead((int)this.user.Id);
                    for (int i = 0; i < lists6.Length; i++)
                    {
                        businessfromservice.Add(lists6[i]);
                    }

                    //this.labelNewMEssageCount.Text = (int.Parse(this.labelNewMEssageCount.Text) + lists6.Length).ToString();


                    SetMessageCount(meaaageCountLabelOfRiZhi, lists.Length);

                    SetMessageCount(this.meaaageCountLabelOfRicheng, list2.Length);

                    SetMessageCount(this.meaaageCountLabelOfZhiBan, list4.Length);


                    SetMessageCount(this.messageCountLabelOfCommentLog, list3.Length);


                    SetMessageCount(this.meaaageCountLabelOFQingJia, lists5.Length);

                    SetMessageCount(this.meaaageCountLabelCHuChai, lists6.Length);


                    #region 接受聊天信息
                    this.ReceiveChattingMessage();
                    #endregion

                }
                catch
                {
                    MessageBox.Show("未能与服务器建立连接……");
                }


                #endregion
            }
        }


        #region 自定义窗体初始化方法


        private void backgroundWorkerLoadUserList_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            LoadUserList();
        }



        private void backgroundWorkerLoadBaseInfo_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            initialData();
        }

        private void backgroundWorkerLoadUnreadMessage_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            LoadUnReadMessage();
        }

        private void backgroundWorkerofOtherWork_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            LoadofInstallHookAndReadDeptAndIsWriteLog();//安装钩子 判断部门 添加滚轮事件 判断是否写日志
        }


        private void backgroundWorkerLoad_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            loadCount++;
            whenLoadDone();
        }

        private void whenLoadDone()
        {
            if (loadCount == 4)
            {
                this.TimerOfShowWindow.Enabled = true;
                this.TimerOfShowWindow.Start();
            }

        }

        void CreativeFileUpDownClass_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            string _ip = Securit.DeDES(FileReadAndWrite.IniReadValue("ftpfile", "ip"));
            string _id = Securit.DeDES(FileReadAndWrite.IniReadValue("ftpfile", "id"));
            string _pwd = Securit.DeDES(FileReadAndWrite.IniReadValue("ftpfile", "pwd"));
            fileUpDown = new FileUpDown(_ip, _id, _pwd);
        }

        void timerofOnline_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            #region 向数据库发送登陆信息

            user.KuOnline = 1;
            baseService.SaveOrUpdateEntity(user);

            #endregion
        }


        private void LoadofInstallHookAndReadDeptAndIsWriteLog()
        {
            if (this.InvokeRequired)
            {
                LoadOtherDele d = new LoadOtherDele(LoadofInstallHookAndReadDeptAndIsWriteLog);
                this.BeginInvoke(d);
            }
            else
            {

                this.rc_flowLayoutPanel.MouseWheel += new MouseEventHandler(MouseWeelTest);
                this.rz_flowLayoutPanel.MouseWheel += new MouseEventHandler(MouseWeelTest);
                this.Show_SuiBi_flowPanel.MouseWheel += new MouseEventHandler(MouseWeelTest);

                string affairsDept = IniReadAndWrite.IniReadValue("AdministrationSection", "affairs");

                if (this.user.Kdid.KdName.Trim() != affairsDept)
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

                //mouseHook.InstallHook(OnMousePress);
                //keyboardHook.InstallHook(OnKeyboardPress);
            }
        }
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
            //creatWindow.SetFormShadow(this);
            //this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width * 3 / 5, Screen.PrimaryScreen.WorkingArea.Height / 8);
            
        }

       
        
        private void initialData()
        {
            if (this.InvokeRequired)
            {
                LoadBaseInfoDele d = new LoadBaseInfoDele(initialData);
                this.BeginInvoke(d);
            }
            else
            {
                LoadHeadIcon();
                if (user != null)
                {
                    #region 登陆签到及显示考勤


                    DateTime today;
                    try
                    {
                        today = ser.GetServiceTime();

                    }
                    catch
                    {
                        today = DateTime.Now;
                    }

                    this.user_label.Text = "你好，" + user.KuName;
                    if (CNDate.isworkDay(today.Date.Ticks))//工作日登录
                    {
                        //查询最近的工作起始时间安排
                        IList attendanceList = baseService.loadEntityList("from Attendance where STATE=" + (int)IEntity.stateEnum.Normal + " and User=" + user.Id + " and SignDate=" + today.Date.Ticks);

                        if (attendanceList != null && attendanceList.Count == 1)//今天登录过
                        {
                            Attendance atd = (Attendance)attendanceList[0];
                            attendance_label.Text += atd.SignStartTime != 0 ? CNDate.getTimeByTimeTicks(atd.SignStartTime) : "";
                            attendance_label.Text += "-";
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
                            attendance_label.Text = CNDate.getTimeByTimeTicks(todaySignStart.SignStartTime) + "-";
                        }
                    }
                    else
                    {
                        this.attendance_label.Text = "今天是休息日";

                    }
                    #endregion
                }
            }
        }

        private void LoadUserList()
        {
            if (this.InvokeRequired)
            {
                LoadUserlistDele d = new LoadUserlistDele(LoadUserList);
                this.BeginInvoke(d);
            }
            else
            {

                this.pictureBoxOfInstantMessenger.Cursor = Cursors.WaitCursor;
                SetTheContent5ButtonIsGray();
                SetTheContent4PanelIsUnvisible();

                Showcontacts();

                pictureBoxOfInstantMessenger.BackgroundImage = WorkLogForm.Properties.Resources.消息白;
                panelofMessage.Visible = true;
                this.pictureBoxOfInstantMessenger.Cursor = Cursors.Hand;
            }
        }

        #region 加载自己的头像
        private void LoadHeadIcon()
        {
            //string address = CommonStaticParameter.ICONS + @"\" + this.user.Id.ToString() + ".png";
            string[] files = Directory.GetFiles(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"icons", this.user.Id.ToString() + "myicon" + "*.png", System.IO.SearchOption.AllDirectories);
            if (files.Length > 0)
            {
                if (File.Exists(files[files.Length -1]))
                {
                    string filename = files[files.Length - 1];
                    System.Drawing.Bitmap ybitmap = new System.Drawing.Bitmap(filename);
                    this.pictureBoxofHeadIcon.BackgroundImage = ybitmap;
                }
            }
           
            else
            {
                this.pictureBoxofHeadIcon.BackgroundImage = WorkLogForm.Properties.Resources.AutoIconBigWhite;
            }
        }
        public void RefreshHeaderPic()
        {
            string address = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"icons" + @"\" + this.user.Id.ToString() + ".png";
            string[] files = Directory.GetFiles(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"icons", this.user.Id.ToString() + "__" + "*.png", System.IO.SearchOption.AllDirectories);
            if (files.Length > 0)
            {
                string myicon = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"icons" + @"\" + this.user.Id.ToString() + "myicon" + DateTime.Now.Ticks.ToString() + ".png";
                File.Copy(files[0], myicon, true);
                this.pictureBoxofHeadIcon.BackgroundImage = new Bitmap(myicon);
            }
        }

        #endregion

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


        #region  关闭最小化设置按钮



        #region 按钮功能
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
        private void setting_pictureBox_Click(object sender, EventArgs e)
        {
            setting_pictureBox.Cursor = Cursors.WaitCursor;
            if (personalSetting == null || personalSetting.IsDisposed)
            {
                personalSetting = new personal_setting();
                personalSetting.User = user;
                personalSetting.Role = role;
                personalSetting.themain = this;
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

        #endregion //end最小化关闭按钮功能

        #region 按钮效果

        #region 最小化按钮效果
        private void min_pictureBox_MouseEnter(object sender, EventArgs e)
        {
            min_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.Minenter;
        }
        private void min_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            min_pictureBox.BackgroundImage = null;
        }
        #endregion

        #region 关闭按钮效果
        private void close_pictureBox_MouseEnter(object sender, EventArgs e)
        {
            close_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.Closeenter;
        }
        private void close_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            close_pictureBox.BackgroundImage = null;
        }
        #endregion

        #region 个人设置效果
        private void setting_pictureBox_MouseEnter(object sender, EventArgs e)
        {
            setting_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.Minenter;
        }
        private void setting_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            setting_pictureBox.BackgroundImage = null;
        }
        #endregion

        #region 系统设置效果
        private void sjgl_pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            sjgl_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.Minenter;
        }
        private void sjgl_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            sjgl_pictureBox.BackgroundImage = null;
        }
        #endregion

        #endregion end按钮效果


        #endregion end 关闭最小化设置按钮


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


        #region 主界面四个按钮

        #region 主界面按钮效果的效果

        private void ri_zhi_pictureBox_MouseEnter(object sender, EventArgs e)
        {
            ri_zhi_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.日志白;
        }
        private void ri_zhi_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            if (panelOfRiZhi.Visible == true)
            {
                return;
            }
            ri_zhi_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.日志;
        }


        private void ri_cheng_pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            ri_cheng_pictureBox1.BackgroundImage = WorkLogForm.Properties.Resources.日程白;
        }

        private void ri_cheng_pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            if (paneOfRiCheng.Visible == true)
            {
                return;
            }
            ri_cheng_pictureBox1.BackgroundImage = WorkLogForm.Properties.Resources.日程;
        }


        private void tong_xun_pictureBox_MouseEnter(object sender, EventArgs e)
        {
            tong_xun_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.随笔白;
        }
        private void tong_xun_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            
            if (panelOfSuibi.Visible == false)
            {
                tong_xun_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.随笔;
            }
        }

        private void pictureBoxOfrefresh_MouseEnter(object sender, EventArgs e)
        {
            pictureBoxOfrefresh.BackgroundImage = WorkLogForm.Properties.Resources.刷新白;
        }

        private void pictureBoxOfrefresh_MouseLeave(object sender, EventArgs e)
        {
            pictureBoxOfrefresh.BackgroundImage = WorkLogForm.Properties.Resources.刷新;
        }

        private void pictureBoxOfInstantMessenger_MouseLeave(object sender, EventArgs e)
        {
            if(panelofMessage.Visible == false)
            pictureBoxOfInstantMessenger.BackgroundImage = WorkLogForm.Properties.Resources.消息;
        }

        private void pictureBoxOfInstantMessenger_MouseEnter(object sender, EventArgs e)
        {
            pictureBoxOfInstantMessenger.BackgroundImage = WorkLogForm.Properties.Resources.消息白;

        }




        #endregion  

        #region 主界面切换
        
        /// <summary>
        /// 设置中间五个按钮全部变成灰色
        /// </summary>
        private void SetTheContent5ButtonIsGray()
        {
            pictureBoxOfInstantMessenger.BackgroundImage = WorkLogForm.Properties.Resources.消息;
            ri_zhi_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.日志;
            ri_cheng_pictureBox1.BackgroundImage = WorkLogForm.Properties.Resources.日程;
            tong_xun_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.随笔;
            pictureBoxOfrefresh.BackgroundImage = WorkLogForm.Properties.Resources.刷新;
        }

        /// <summary>
        /// 设置四个模块全部看不见
        /// </summary>
        private void SetTheContent4PanelIsUnvisible()
        {
            panelOfRiZhi.Visible = false;
            paneOfRiCheng.Visible = false;
            panelOfSuibi.Visible = false;
            panelofMessage.Visible = false;
        }

        private void ri_cheng_pictureBox1_Click(object sender, EventArgs e)
        {
            #region 测试代码
            /*************测试代码*************************/
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
            #endregion 测试代码

            if (paneOfRiCheng.Visible == false)
            {
                SetTheContent5ButtonIsGray();
                SetTheContent4PanelIsUnvisible();

                init_rc_Panel();

                this.schedulelistfromService.Clear();
                this.meaaageCountLabelOfRicheng.MessageCount = 0;
                ser.SetShareScheduleIsRead((int)this.user.Id);

                ri_cheng_pictureBox1.BackgroundImage = WorkLogForm.Properties.Resources.日程白;
                paneOfRiCheng.Visible = true;
            }
        }

        private void ri_zhi_pictureBox_Click(object sender, EventArgs e)
        {
            if (panelOfRiZhi.Visible == false)
            {
                SetTheContent5ButtonIsGray();
                SetTheContent4PanelIsUnvisible();

                ser.SetCommentLogIsRead((int)this.user.Id);
                ser.SetShareLogIsRead((int)this.user.Id);
                this.loglistfromService.Clear();
                
                this.messageCountLabelOfCommentLog.MessageCount = 0;
                this.meaaageCountLabelOfRiZhi.MessageCount = 0;

                AddInPanelOfLogCommentMessage();

                this.commentlistfromService.Clear();
                this.rz_flowLayoutPanel.Tag = null;
                this.rz_flowLayoutPanel.Controls.Clear();
                init_rz_Panel();

                ri_zhi_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.日志白;


                panelOfRiZhi.Visible = true;
            }
            else 
            {
                if(this.panelOfLogCommentMessage1.ItemsCount >0)
                {
                    this.panelOfLogCommentMessage1.Visible = true;
                }
            }
        }



        private void tong_xun_pictureBox_Click(object sender, EventArgs e)
        {
            if (panelOfSuibi.Visible == false)
            {
                SetTheContent5ButtonIsGray();
                SetTheContent4PanelIsUnvisible();

                RefreshSuiBi();

                tong_xun_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.随笔白;
                panelOfSuibi.Visible = true;
            }
        }

        private void pictureBoxOfInstantMessenger_Click(object sender, EventArgs e)
        {
            #region 注视的代码
            //if (sec != null)
            //{
            //    sec.ClearMessagePanel();
            //    this.chattinguserlist.Clear();
            //}
            //pictureBoxOfInstantMessenger.Cursor = Cursors.WaitCursor;
            //this.timerOfReceiveChattingMessage.Enabled = false;

            //this.timerchattingflesh.Enabled = false;
            //this.pictureBoxOfInstantMessenger.BackgroundImage = WorkLogForm.Properties.Resources.InstantMessengerLogo;
            //this.LabelofChatttingCount.Text = "";
            //if (InstantMessengerWindows == null || InstantMessengerWindows.IsDisposed)
            //{
            //    InstantMessengerWindows = new InstantMessenger();
            //}
            //if (!InstantMessengerWindows.Created)
            //{
            //    InstantMessengerWindows.FormLocation = new Point(this.Location.X - InstantMessengerWindows.Width, this.Location.Y);
            //    InstantMessengerWindows.User = this.user;
            //    InstantMessengerWindows.MainReceiveMessage = this.timerOfReceiveChattingMessage;
            //    InstantMessengerWindows.Chattinguserlist = this.chatinservice;
            //    InstantMessengerWindows.Show();

            //}
            //else
            //{
            //    InstantMessengerWindows.WindowState = FormWindowState.Normal;
            //    InstantMessengerWindows.Chattinguserlist = this.chatinservice;
            //    InstantMessengerWindows.Visible = true;

            //}
            //pictureBoxOfInstantMessenger.Cursor = Cursors.Hand;


            #endregion


            if (panelofMessage.Visible == false)
            {
                this.pictureBoxOfInstantMessenger.Cursor = Cursors.WaitCursor;
                SetTheContent5ButtonIsGray();
                SetTheContent4PanelIsUnvisible();
                

                pictureBoxOfInstantMessenger.BackgroundImage = WorkLogForm.Properties.Resources.消息白;
                panelofMessage.Visible = true;
                this.pictureBoxOfInstantMessenger.Cursor = Cursors.Hand;
            }

        }

        #endregion

        #region 界面的实现


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
                bool rcVisible = panelOfSuibi.Visible;
                panelOfSuibi.Visible = false;
               
                long thisDay = DateTime.Now.Date.Ticks;
                long nextDay = DateTime.Now.Date.Ticks + new DateTime(1, 1, 2).Date.Ticks;
                IList staffScheduleList = baseService.loadEntityList("from StaffSchedule where STATE=" + (int)IEntity.stateEnum.Normal + " and Staff=" + user.Id + " and ScheduleTime>=" + thisDay + " order by ScheduleTime asc");
                if (staffScheduleList.Count > rc_flowLayoutPanel.Controls.Count)
                {
                    while (rc_flowLayoutPanel.Controls.Count > 0)
                    {
                        rc_flowLayoutPanel.Controls.RemoveAt(0); // 删除所有日程消息
                    }
                    if (scheduleList != null)
                    {
                        scheduleList.Clear();
                    }
                    scheduleList = staffScheduleList; //把查询出来的日程列表付给全局变量
                    creat_ri_cheng_Panel(staffScheduleList);
                }
                if (this.rc_flowLayoutPanel.Controls.Count == 0)
                {
                    this.rc_flowLayoutPanel.BackgroundImage = WorkLogForm.Properties.Resources.NoCntentBg;
                }
                else
                {
                    this.rc_flowLayoutPanel.BackgroundImage = null; 
                }
                panelOfSuibi.Visible = rcVisible;
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
                    RiChengRect r1 = new RiChengRect();
                    r1.HeaderId = int.Parse(ss.ArrangeMan.Id.ToString());
                    r1.ArrangePersonNameText = ss.ArrangeMan.KuName.Trim();
                    r1.SubTitleText = ss.Subject.ToString();
                    r1.TimeText = new DateTime(ss.ScheduleTime).ToString("yyyy-MM-dd HH:mm");
                    if (ss.Content.Length > 140)
                    {
                        r1.ContentText = ss.Content.Substring(0, 140) + "……";
                        r1.ContentClicked += r1_ContentClicked;
                        r1.Tag = ss;
                    }
                    else
                    {
                        r1.ContentText = ss.Content;
                    }
                    r1.Parent = rc_flowLayoutPanel;
                }

            }
        }

        void r1_ContentClicked(object sender, EventArgs e)
        {
            this.panelOfRIchengAllInfo1.Visible = true;
            //if (this.panelOfRIchengAllInfo1.RechengREct != null)
            //{
                //this.panelOfRIchengAllInfo1.RechengREct = null;
            //}
            Label l = (Label)sender;
            StaffSchedule ss = l.Parent.Parent.Tag as StaffSchedule;
            RiChengRect r1 = new RiChengRect();
            r1.HeaderId = int.Parse(ss.ArrangeMan.Id.ToString());
            r1.ArrangePersonNameText = ss.ArrangeMan.KuName.Trim();
            r1.SubTitleText = ss.Subject.ToString();
            r1.TimeText = new DateTime(ss.ScheduleTime).ToString("yyyy-MM-dd HH:mm");
            r1.ContentText = ss.Content;

            this.panelOfRIchengAllInfo1.SetRechengREct(r1);

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
            
            int logid = 0;
            if (rz_flowLayoutPanel.Tag!= null)
            {
                logid = int.Parse(rz_flowLayoutPanel.Tag.ToString());
            }


            string sql = "with cte as " +
                            "( " +
                            " select row=row_number()over(order by getdate()), * from WktuserShareUserId where SharePresonid =  " + user.Id.ToString() + " and WktuserShareUserId.STATE = 0 and Id > " +  logid.ToString()+
                            ") " +
                            " select * from cte where row between "+"1"+" and "+"10";

            IList staffLogList = baseService.ExecuteSQL(sql); //查询自己的日志与其他人分享的日志
            creat_ri_zhi_Panel(staffLogList);
            if (this.rz_flowLayoutPanel.Controls.Count > 0)
            {
                this.rz_flowLayoutPanel.BackgroundImage = null;

            }
            else 
            {
                this.rz_flowLayoutPanel.BackgroundImage = WorkLogForm.Properties.Resources.NoCntentBg;
            
            }

            //}
        }


        private void creat_ri_zhi_Panel(IList rzList)
        {
            if (rzList != null && rzList.Count > 0)
            {
                for (int i = 0; i < rzList.Count; i++)
                {
                    object[] sf = (object[])rzList[i];
                    RiZhiRect rizhi = new RiZhiRect();
                    rizhi.HeaderId = int.Parse(sf[3].ToString());//谁分享的
                    rizhi.Name = sf[6].ToString();//分享人的姓名
                    rizhi.Contenttext = CommonClass.CommonUtil.HtmlToReguFormat(sf[5].ToString());
                    rizhi.TimeText = new DateTime(Convert.ToInt64(sf[4].ToString())).ToString("yyyy年MM月dd日 HH:mm");
                   
                    rizhi.Tag = sf[7]; //日志id
                    rizhi.ContentClicked += rizhi_ContentClicked;
                    rizhi.Parent = rz_flowLayoutPanel;
                    rz_flowLayoutPanel.Tag = sf[1];//Id号;
                }
            }
        }

        #region 查看全部内容日志
        void rizhi_ContentClicked(object sender, EventArgs e)
        {
            Label p = (Label)sender;
            long logid = long.Parse(p.Parent.Parent.Tag.ToString());
            p.Parent.Parent.Cursor = Cursors.WaitCursor;
            StaffLog sl = new StaffLog();
            baseService.loadEntity(sl, logid);
            writeLog log = new writeLog();
            log.IsComment = true;
            log.User = sl.Staff;
            log.CommentPersonName = User.KuName;
            log.LogDate = new DateTime(sl.WriteTime);
            log.ShowDialog();
            p.Parent.Parent.Cursor = Cursors.Hand;
        }
        #endregion
        #endregion

        #region 随笔功能
        private void Write_SUiBi_textBox_TextChanged(object sender, EventArgs e)
        {
            if (this.Write_SUiBi_textBox.Text.Length > 140 && this.Write_SUiBi_textBox.Text.Length == 0)
            {
                this.pictureBox7.Enabled = false;
            }
            else
            {
                this.pictureBox7.Enabled = true;

            }
        }

      
        /// <summary>
        /// 点击查看更多
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToSeeMoreSuiBi()
        {
            ShowSuiBiInFlowPanel(this.Show_SuiBi_flowPanel.Controls.Count + 10);
        }

        public void ShowSuiBiInFlowPanel(int end)
        {
            //从数据库中查出区间内容
            String listCountSql = "select COUNT(*) from SuiBi;";

            IList CountSuibi = baseService.ExecuteSQL(listCountSql);
            int count = 0;
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
                             "_t.WriteTime as time , _t.Id as id ,WK_T_USER.KU_ID as userid from (select top 100 percent * " +
                             "from SuiBi where SuiBi.STATE = 0 order by SuiBi.WriteTime desc) as _t left join " +
                             "WK_T_USER on _t.WkTUserId = WK_T_USER.KU_ID ) as t order by t.time desc ) as tt order by tt.time asc ) as ttt order by ttt.time desc";

            }
            else
            {

                sql = "select * from ( " +
                                "select top " + (count % 10).ToString() + " * from ( " +
                                "select top " + (count + 10).ToString() + " * from  " +
                                "(Select  WK_T_USER.KU_NAME as name , _t.Contents as content, " +
                                "_t.WriteTime as time , _t.Id as id ,WK_T_USER.KU_ID as userid from (select top 100 percent * " +
                                "from SuiBi order by SuiBi.WriteTime desc) as _t left join " +
                                "WK_T_USER on _t.WkTUserId = WK_T_USER.KU_ID ) as t order by t.time desc ) as tt order by tt.time asc ) as ttt order by ttt.time desc";


            }



            IList SuiBiList = baseService.ExecuteSQL(sql);


            if (SuiBiList != null && SuiBiList.Count != 0)
            {
                foreach (object[] o in SuiBiList)
                {
                    //造控件

                    SuibiRect suibi = new SuibiRect();
                    suibi.HeaderId = int.Parse(o[4].ToString());
                    suibi.Name = o[0].ToString();
                    suibi.TimeText = new DateTime(Convert.ToInt64(o[2].ToString())).ToString("yyyy年MM月dd日 HH:mm");
                    suibi.Contenttext = o[1].ToString();

                    suibi.Parent = Show_SuiBi_flowPanel;
                }
            }

        }

        private void RefreshSuiBi()
        {
            this.Show_SuiBi_flowPanel.Controls.Clear();
            ShowSuiBiInFlowPanel(10);
            if (this.Show_SuiBi_flowPanel.Controls.Count > 0)
            {
                this.Show_SuiBi_flowPanel.BackgroundImage = null;
            }
            else 
            {
                this.Show_SuiBi_flowPanel.BackgroundImage = WorkLogForm.Properties.Resources.NoCntentBg;
            }
        }

        #region 对勾按钮
        private void pictureBox7_Click(object sender, EventArgs e)
        {
            if (this.Write_SUiBi_textBox.Text.Length == 0)
            {
                this.labelMessageBox1.MessageageShow("您还没有输入内容！");
                //MessageBox.Show("");
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
                    this.labelMessageBox1.MessageageShow("发布随笔失败！");
                    //MessageBox.Show("");
                    return;
                }
                //清空内容
                this.Write_SUiBi_textBox.Text = "";
                this.labelMessageBox1.MessageageShow("发布成功！");

                //更新展示栏中内容

                while (Show_SuiBi_flowPanel.Controls.Count > 0)
                {
                    Show_SuiBi_flowPanel.Controls.RemoveAt(0); //
                }
                ShowSuiBiInFlowPanel(10);

               
            }
        }

        private void pictureBox7_MouseEnter(object sender, EventArgs e)
        {
            this.pictureBox7.Image = WorkLogForm.Properties.Resources.DuiGouDarkBlack;
        }

        private void pictureBox7_MouseLeave(object sender, EventArgs e)
        {
            this.pictureBox7.Image = WorkLogForm.Properties.Resources.DuigouLightBlack;
        }

        



        #endregion



        #region 写日志按钮
        private void pictureBox6_MouseEnter(object sender, EventArgs e)
        {
            if (this.pictureBox6.Image != null)
                this.pictureBox6.Image = WorkLogForm.Properties.Resources.WritesuibiOn;
            else
            {
                this.pictureBox6.BackgroundImage = WorkLogForm.Properties.Resources.ChahaoDB;
            }
        }

        private void pictureBox6_MouseLeave(object sender, EventArgs e)
        {
            if (this.pictureBox6.Image != null)
                this.pictureBox6.Image = WorkLogForm.Properties.Resources.WriteSuibi;
            else
            {
                this.pictureBox6.BackgroundImage = WorkLogForm.Properties.Resources.ChahaoLB;
            }
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            if (this.panel12.Visible == false)
            {
                this.panel12.Visible = true;
                this.write_SuiBi.Visible = true;
                this.pictureBox6.Image = null;
                this.pictureBox6.BackgroundImage = WorkLogForm.Properties.Resources.ChahaoDB;

            }
            else if (this.panel12.Visible == true)
            {
                this.panel12.Visible = false;
                this.write_SuiBi.Visible = false;
                this.pictureBox6.Image =  WorkLogForm.Properties.Resources.WriteSuibi;
                this.pictureBox6.BackgroundImage = null;
            }

            //this.Show_SuiBi_flowPanel.Height = 293;
            //this.SuiBi_SeeMore.Height = 169;
            //this.write_SuiBi.Visible = true;
            //this.panel8.Location = new Point(panel8.Location.X, write_SuiBi.Height + 5);
        }

        #endregion


        #endregion

        #region 刷新
        /// <summary>
        /// 手动更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBoxOfrefresh_Click(object sender, EventArgs e)
        {
            this.rz_flowLayoutPanel.Tag = null; //更新从0开始查库
            init_rc_Panel();

            init_rz_Panel();
            RefreshSuiBi();

            if (this.backgroundWorkerOfDownPicture.IsBusy == false)
            {
                this.backgroundWorkerOfDownPicture.RunWorkerAsync();
            }
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

        /// <summary>
        /// 后台下载图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorkerOfDownPicture_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            string mysql = "select w.KU_ID,w.KU_ONLINE from WK_T_USER w";
            IList userList = baseService.ExecuteSQL(mysql);
            if (userList != null && userList.Count > 0)
            {
                foreach (object[] obj in userList)
                {
                    string address = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"icons" + @"\" + obj[0].ToString() + ".png";

                    if (fileUpDown.DirectoryExist(obj[0].ToString() + ".png", "Iconpics")) //服务器里是否有他的图片
                    {
                        if (File.Exists(address))//本地是否存在 存在
                        {
                            FileInfo fi = new FileInfo(address);
                            if (fi.CreationTime.Ticks < fileUpDown.GetFileModifyDateTime(obj[0].ToString() + ".png", "Iconpics").Ticks)//与本地图片比对时间 本地创建时间晚于服务器则下载
                            {
                                //fi.Delete();
                                string[] files = Directory.GetFiles(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"icons", obj[0].ToString() + "__" + "*.png", System.IO.SearchOption.AllDirectories);
                                if (files.Length > 0) //删除原来的临时文件
                                {
                                    for (int i = 0; i < files.Length;i++ )
                                    {
                                        FileInfo oldfi = new FileInfo(files[i]);
                                        oldfi.Delete();
                                    }
                                }

                                fileUpDown.Download(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"icons", obj[0].ToString() +"__"+DateTime.Now.Ticks.ToString()+".png", "Iconpics");
                            }
                        }
                        else
                        {
                            fileUpDown.Download(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"icons", obj[0].ToString() + ".png", "Iconpics");
                        }
                    }
                }
            }
        }
        private void backgroundWorkerOfDownPicture_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            //MessageBox.Show("");
        }


        #endregion

        #region 联系人列表显示

        private void Showcontacts()
        {
            chatListBox1.Items.Clear();
            
            string sql = "select u from WkTDept u";
            IList depts = baseService.loadEntityList(sql);
            if (depts != null && depts.Count > 0)
            {
                foreach (WkTDept o in depts)
                {
                    ChatListItem item = new ChatListItem(o.KdName.Trim());
                    string sql1 = "select u from WkTUser u left join u.Kdid dept where dept.Id = " + o.Id + " order by u.KuOnline desc";
                    IList userlist = baseService.loadEntityList(sql1);

                    if (userlist != null && userlist.Count > 0)
                    {
                        #region 二层循环
                        foreach (WkTUser oo in userlist)
                        {
                            if (oo.Id != user.Id)
                            {
                                ChatListSubItem subItem = new ChatListSubItem("", oo.KuName.Trim(), "");
                                subItem.userid =int.Parse(oo.Id.ToString());
                              
                                if (oo.KuOnline == 1)
                                {
                                    subItem.Status = (ChatListSubItem.UserStatus)(1);
                                }
                                else
                                {
                                    subItem.Status = (ChatListSubItem.UserStatus)(5);
                                }
                                //subItem.ta

                                item.SubItems.AddAccordingToStatus(subItem);
                            }

                        }
                        #endregion end 二层循环

                        item.SubItems.Sort();

                        chatListBox1.Items.Add(item);
                    }
                   
                    
                }
            }
            this.timerOfRefreshOnline.Enabled = true; ;
            this.timerOfRefreshOnline.Start();
        }

        private void RefreshUserOnling()
        {
            string mysql = "select w.KU_ID,w.KU_ONLINE from WK_T_USER w";
            IList userList = baseService.ExecuteSQL(mysql);
            if (userList != null)
            {
                foreach (object[] o in userList)
                {
                    ChatListSubItem cha = GetTheUserById(int.Parse(o[0].ToString()));
                    if (cha != null)
                    {
                        if (o[1].ToString() == "1")
                        {
                            cha.Status = (ChatListSubItem.UserStatus)(1);
                        }
                        else
                        {
                            cha.Status = (ChatListSubItem.UserStatus)(5);
                        }
                    }
                }
            }
        }



        private ChatListSubItem GetTheUserById(int theuserid)
        {
            foreach (ChatListItem group in chatListBox1.Items)
            {
                foreach (ChatListSubItem user in group.SubItems)
                {
                    if (user.userid == theuserid)
                    {
                        return user; 
                    }
                
                }
            }

            return null;
        }

        private void timerOfRefreshOnline_Tick(object sender, EventArgs e)
        {
            RefreshUserOnling();
        }

        #endregion

        #endregion //end界面的现实

        #region 
        private void AddInPanelOfLogCommentMessage()
        {
            if (commentlistfromService.Count > 0)
            {
                foreach (KjqbService.CommentInService com in commentlistfromService)
                {
                    string sql = "select u from WkTUser u where u.KuName = '" + com.CommentUserName + "'";

                    WkTUser commentuser = (WkTUser)baseService.loadEntityList(sql)[0];

                    panelOfLogCommentMessageItem item = new panelOfLogCommentMessageItem();
                    item.HeaderUserid = int.Parse(commentuser.Id.ToString());
                    item.NameContent = com.CommentUserName;
                    item.ContentClicked += item_ContentClicked;
                    item.Tag = com.LogId;
                    this.panelOfLogCommentMessage1.AddCommentItem(item);
                }
                this.panelOfLogCommentMessage1.Visible = true;
            }
            else
            {
                this.panelOfLogCommentMessage1.Visible = false;
            }

        }

        void item_ContentClicked(object sender, EventArgs e)
        {
            Label p = (Label)sender;
            long logid = long.Parse(p.Parent.Tag.ToString());
            p.Parent.Cursor = Cursors.WaitCursor;
            StaffLog sl = new StaffLog();
            baseService.loadEntity(sl, logid);
            writeLog log = new writeLog();
            log.IsComment = true;
            log.User = sl.Staff;
            log.CommentPersonName = User.KuName;
            log.LogDate = new DateTime(sl.WriteTime);
            log.ShowDialog();
            panelOfLogCommentMessageItem item = (panelOfLogCommentMessageItem)p.Parent;
            this.panelOfLogCommentMessage1.RemoveCommentItem(item);

            p.Parent.Cursor = Cursors.Hand;

        }


        public void SetMessageCount(MessageCountLabel m, int count)
        {
            if (m.MessageCount == 0)
                m.MessageCount = count == 0 ? 0 : count;
            else
                m.MessageCount = m.MessageCount + count;
        }

        #endregion


        #endregion //主界面四个按钮

        #region 底部按钮
        #region 底部按钮功能



        //我的日历
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
        //请假管理
        private void qjgl_pictureBox_Click(object sender, EventArgs e)
        {
            if (levlistfromservice.Count > 0)
            {
                ser.SetLeaveInfoIsRead((int)this.user.Id);
                this.levlistfromservice.Clear();
                this.meaaageCountLabelOFQingJia.MessageCount = 0;
            }
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
        //日志管理
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
        //日程管理
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
        //加班管理
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
        //值班管理
        private void zbgl_pictureBox_Click(object sender, EventArgs e)
        {
            if (tfmListfromservice.Count > 0)
            {
                ser.SetTimeArrangeForManagerIsRead((int)this.user.Id);
                this.meaaageCountLabelOfZhiBan.MessageCount = 0;
                this.tfmListfromservice.Clear();
            }
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
        //出差管理
        private void ccgl_pictureBox_Click(object sender, EventArgs e)
        {

            if(this.businessfromservice.Count>0)
            {
                ser.SetBusinessInfoIsRead((long)this.user.Id);
                this.businessfromservice.Clear();
                this.meaaageCountLabelCHuChai.MessageCount = 0;
            }

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
        //随笔管理
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
        //系统设置
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
        #endregion
        #region 底部图标选中效果
        #region 我的日历
        private void wdrl_pictureBox_MouseEnter(object sender, EventArgs e)
        {
            wdrl_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.底部椭圆背景选中;
        }
        private void wdrl_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            wdrl_pictureBox.BackgroundImage = null;
        }
        #endregion

        #region 日程管理
        private void rcgl_pictureBox_MouseEnter(object sender, EventArgs e)
        {
            rcgl_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.底部椭圆背景选中;
        }
        private void rcgl_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            rcgl_pictureBox.BackgroundImage = null;
        }
        #endregion

        #region 日志管理
        private void rzgl_pictureBox_MouseEnter(object sender, EventArgs e)
        {
            rzgl_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.底部椭圆背景选中;
        }
        private void rzgl_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            rzgl_pictureBox.BackgroundImage = null;
        }
        #endregion


        #region 随笔管理
        private void SuiBiGuanLi_pictureBox_MouseEnter(object sender, EventArgs e)
        {
            SuiBiGuanLi_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.底部椭圆背景选中;
        }

        private void SuiBiGuanLi_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            SuiBiGuanLi_pictureBox.BackgroundImage = null;
        }

        #endregion

        #region 请假管理

        private void qjgl_pictureBox_MouseEnter(object sender, EventArgs e)
        {
            qjgl_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.底部椭圆背景选中;
        }
        private void qjgl_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            qjgl_pictureBox.BackgroundImage = null;
        }
        #endregion

        #region 加班管理
        private void jbgl_pictureBox_MouseEnter(object sender, EventArgs e)
        {
            jbgl_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.底部椭圆背景选中;
        }
        private void jbgl_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            jbgl_pictureBox.BackgroundImage = null;
        }
        #endregion

        #region 值班管理
        private void zbgl_pictureBox_MouseEnter(object sender, EventArgs e)
        {
            zbgl_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.底部椭圆背景选中;
        }
        private void zbgl_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            zbgl_pictureBox.BackgroundImage = null;
        }
        #endregion

        #region 出差管理
        private void ccgl_pictureBox_MouseEnter(object sender, EventArgs e)
        {
            ccgl_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.底部椭圆背景选中;
        }
        private void ccgl_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            ccgl_pictureBox.BackgroundImage = null;
        }
        #endregion

        #region 综合统计
        private void spgl_pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            spgl_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.底部椭圆背景选中;
        }

        private void spgl_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            spgl_pictureBox.BackgroundImage = null;
        }
        #endregion

        #endregion
        #endregion

        #region 退出时签到相关事件及方法
        /// <summary>
        /// 退出时签到
        /// </summary>
        private void signExit()
        {
            try
            {

                if (this.backgroundWorkerOfDownPicture.IsBusy == true)
                {
                    this.backgroundWorkerOfDownPicture.CancelAsync();
                    this.backgroundWorkerOfDownPicture.Dispose();
                
                }

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
                            attendance_label.Text = CNDate.getTimeByTimeTicks(todaySignStart.SignStartTime) + "-";
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


                KjqbService.ScheduleInService[] lists2;
                lists2 = ser.SearchShareSchedule((int)this.user.Id);
                for (int i = 0; i < lists2.Length; i++)
                {
                    schedulelistfromService.Add(lists2[i]);
                }

                
                //this.labelNewMEssageCount.Text = (int.Parse(this.labelNewMEssageCount.Text) + lists2.Length).ToString();

                KjqbService.CommentInService[] lists3;
                lists3 = ser.SearchCommentlog((int)this.user.Id);
                for (int i = 0; i < lists3.Length; i++)
                {
                    commentlistfromService.Add(lists3[i]);
                }
                
                //SetMessageCount(this.meaaageCountLabelOfRicheng, lists2.Length);
                //this.labelNewMEssageCount.Text = (int.Parse(this.labelNewMEssageCount.Text) + lists3.Length).ToString();
                
                KjqbService.TimeArrangeForManagerInService[] lists4;
                lists4 = ser.SearchTimeArrangeForManager((int)this.user.Id);
                for (int i = 0; i < lists4.Length; i++)
                {
                    tfmListfromservice.Add(lists4[i]);
                }

                //this.labelNewMEssageCount.Text = (int.Parse(this.labelNewMEssageCount.Text) + lists4.Length).ToString();

                KjqbService.LeaveInService[] lists5;
                lists5 = ser.SearchLeaveInfo((int)this.user.Id);
                for (int i = 0; i < lists5.Length; i++)
                {
                    levlistfromservice.Add(lists5[i]);
                }


                //this.labelNewMEssageCount.Text = (int.Parse(this.labelNewMEssageCount.Text) + lists5.Length).ToString();

                KjqbService.BusinessService[] lists6;
                lists6 = ser.SearchBusinessInfo((int)this.user.Id);
                for (int i = 0; i < lists6.Length; i++)
                {
                    businessfromservice.Add(lists6[i]);
                }

                SetMessageCount(meaaageCountLabelOfRiZhi, lists.Length);

                SetMessageCount(this.meaaageCountLabelOfRicheng, lists2.Length);

                SetMessageCount(this.meaaageCountLabelOfZhiBan, lists4.Length);

                SetMessageCount(this.meaaageCountLabelOFQingJia, lists5.Length);

                SetMessageCount(this.meaaageCountLabelCHuChai, lists6.Length);
               
                SetMessageCount(this.messageCountLabelOfCommentLog, lists3.Length);

            }
            catch
            {
                this.timerMessageSend.Stop();
                MessageBox.Show("与服务器失去建立连接，可能是由于网络原因，程序将退出，未记录本次签退时间，请在网络正常后再次登录。");
                this.Close();
            
            }
        }

      


     
        


        #region 接受聊天信息


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

        /// <summary>
        /// 如果已经在和这个人聊天则不能再创建窗体
        /// </summary>
        List<WkTUser> chatwindowsusers;

        private void chatListBox1_DoubleClickSubItem(object sender, ChatListEventArgs e)
        {
            ChatListSubItem cha = e.SelectSubItem;
            if (cha.IsTwinkle)
            {
                cha.IsTwinkle = !cha.IsTwinkle;
                RemoveFromChaterList(cha.userid);
            }

            WkTUser w = new WkTUser();
            w = (WkTUser)baseService.loadEntity(w, cha.userid);

            if (chatwindowsusers == null)
            {
                chatwindowsusers = new List<WkTUser>();
            }
            if (!chatwindowsusers.Contains(w))
            {

                ChatWindows chat = new ChatWindows();
                chat.ReceiveUser = w;
                chat.SendUser = this.user;
                chat.Chatwindwosuser = chatwindowsusers;
                chat.Show();
            }
            else
            {

            }
        }
        private void RemoveFromChaterList(int id)
        {

            if (chattinguserlist !=null&& this.chattinguserlist.Count > 0)
            {
                foreach (WkTUser u in chattinguserlist)
                {
                    if (u.Id == id)
                    {
                        chattinguserlist.Remove(u);
                    }
                }
            }
        }
        private void timerOfReceiveChattingMessage_Tick(object sender, EventArgs e)
        {
            ReceiveChattingMessage();
        }

        private void ReceiveChattingMessage()
        {
            #region 接受聊天信息
            try
            {
                KjqbService.ChatInService[] lists7;
                lists7 = ser.SearchChatInfo((int)this.user.Id);
                for (int i = 0; i < lists7.Length; i++)
                {
                    chatinservice.Add(lists7[i]);
                    if (!IsInChatUserlist(lists7[i].SendUserId))
                    {
                        WkTUser w = new WkTUser();
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
                SetMessageCount(meaaageCountLabelOfXiaoXI, lists7.Length);
            }
            catch { }
            SetthechatingUserIsTwinkle();
            #endregion
        
        }

        private void SetthechatingUserIsTwinkle()
        {

            if (chattinguserlist.Count > 0 && chattinguserlist != null)
            {
                foreach (WkTUser user in chattinguserlist)
                {
                    ChatListSubItem cha = this.GetTheUserById(int.Parse(user.Id.ToString()));
                    if (cha != null)
                    {
                        cha.IsTwinkle = true;
                    }
                }
            }
        }


        #endregion


        #region 任务栏右下角图标右键项
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
        #endregion 


        #region 钩子事件判定鼠标未操作时间
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
            if (this.timeCount > 60*40)//timecount单位秒 40分钟
            {
                this.timerOfMouseOrKeyUnDo.Stop();
                ShowMsg showmsg = new ShowMsg();
                showmsg.ShowDialog();
                if (showmsg.DialogResult == DialogResult.OK)
                {
                    this.Close();
                }
                else
                {
                    timeCount = 0;
                    this.timerOfMouseOrKeyUnDo.Start();
                }
            }
        }

        #endregion


        #region 写日志写日程按钮
        #region 写日志写日程按钮滑动效果

        private void log_pictureBox_MouseEnter(object sender, EventArgs e)
        {
            this.log_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.WriteLog;
        }

        private void log_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            this.log_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.WritelogUnChecked;
        }
        private void schedule_pictureBox_MouseEnter(object sender, EventArgs e)
        {
            this.schedule_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.writeSch;
        }

        private void schedule_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            this.schedule_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.writeSchUnchecked;
        }


        #endregion
        #region 写日志写日程按钮功能
        //写日志
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
        //写日程
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
        #endregion

        

       

       

        #endregion
      


    }
}
