using ClassLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WorkLogForm.Service;

namespace WorkLogForm
{
    public partial class NewMessageWindow : Form
    {
        private BaseService baseService = new BaseService();
        private List<KjqbService.LogInService> loglist;
        private List<KjqbService.ScheduleInService> schedulelist;
        private List<KjqbService.CommentInService> commentList;

        public List<KjqbService.CommentInService> CommentList
        {
            get { return commentList; }
            set { commentList = value; }
        }
        public List<KjqbService.ScheduleInService> Schedulelist
        {
            get { return schedulelist; }
            set { schedulelist = value; }
        }
        private WkTUser user;

        public WkTUser User
        {
            get { return user; }
            set { user = value; }
        }
        public List<KjqbService.LogInService> Loglist
        {
            get { return loglist; }
            set { loglist = value; }
        }
        

        private Point formLocation;

        public Point FormLocation
        {
            get { return formLocation; }
            set { formLocation = value; }
        }
        public NewMessageWindow()
        {
            InitializeComponent();
            loglist = new List<KjqbService.LogInService>();
            user = new WkTUser();

        }

        private void close_pictureBox_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        #region 窗体加载
        private void NewMessageWindow_Load(object sender, EventArgs e)
        {
            if (this.formLocation != null)
            {
                this.Location = formLocation;
            }

            if (loglist != null && loglist.Count > 0)
            {
                foreach (KjqbService.LogInService ll in Loglist)
                {
                    StaffLog ss = new StaffLog();
                    //string sql = "select u from StaffLog u where u.Id = " + ll.LogId;
                    ss = (StaffLog)baseService.loadEntity(ss, ll.LogId);
                    LinkLabel l1 = new LinkLabel();
                    l1.Text = ss.Staff.KuName + "分享给您的日志";
                    l1.Width = 150;
                    l1.Height = 30;
                    l1.Top = 10;
                    l1.Tag = ss;
                    l1.Click += l1_Click;
                    l1.Parent = flowLayoutPanel1;
                }
            }

            if (schedulelist != null && schedulelist.Count > 0)
            {
                foreach (KjqbService.ScheduleInService ll in schedulelist)
                {
                    StaffSchedule ss = new StaffSchedule();
                    //string sql = "select u from StaffLog u where u.Id = " + ll.LogId;
                    ss = (StaffSchedule)baseService.loadEntity(ss, ll.ScheduleId);
                    LinkLabel l1 = new LinkLabel();
                    if(ss.Staff.Id  == ss.ArrangeMan.Id)
                    {
                        l1.Text = ss.Staff.KuName + "分享给您的日程";
                        l1.Width = 150;
                        l1.Height = 30;
                        l1.Top = 10;
                        l1.Tag = ss;
                        l1.Click += l1_Click;
                        l1.Parent = flowLayoutPanel1;
                    }
                    else if (ss.Staff.Id != ss.ArrangeMan.Id)
                    {
                        l1.Text = ss.ArrangeMan.KuName + "给您安排的日程";
                        l1.Width = 150;
                        l1.Height = 30;
                        l1.Top = 10;
                        l1.Tag = ss;
                        l1.Click += l1_Click;
                        l1.Parent = flowLayoutPanel1;
                    }
                    
                }
            }

            if (commentList != null && commentList.Count > 0)
            {
                foreach (KjqbService.CommentInService ll in commentList)
                {
                    StaffLog ss = new StaffLog();
                    //string sql = "select u from StaffLog u where u.Id = " + ll.LogId;
                    ss = (StaffLog)baseService.loadEntity(ss, ll.LogId);
                    LinkLabel l1 = new LinkLabel();
                    l1.Text = ll.CommentUserName + "评论了您的日志";
                    l1.Width = 150;
                    l1.Height = 30;
                    l1.Top = 10;
                    l1.Tag = ss;
                    l1.Click += l1_Click;
                    l1.Parent = flowLayoutPanel1;
                }
            }


        }

        void l1_Click(object sender, EventArgs e)
        {
            LinkLabel l1 = (LinkLabel)sender;

            if (l1.Tag.GetType() == typeof(StaffLog))
            {
                StaffLog ss = (StaffLog)l1.Tag;
                writeLog wl = new writeLog();
                wl.User = ss.Staff;
                wl.LogDate = new DateTime(ss.WriteTime);
                wl.IsComment = true;
                wl.CommentPersonName = this.User.KuName;
                wl.ShowDialog();
            }
            else if (l1.Tag.GetType() == typeof(StaffSchedule))
            {
                StaffSchedule ss = (StaffSchedule)l1.Tag;
                DateTime dt = new DateTime(ss.ScheduleTime);
                MessageBox.Show(dt.ToString("yyyy-MM-dd HH:mm:ss")+" :"+ss.Content);
            }
            
        }
        #endregion








    }
}
