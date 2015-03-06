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




        }

        void l1_Click(object sender, EventArgs e)
        {
            LinkLabel l1 = (LinkLabel)sender;
            StaffLog ss = (StaffLog)l1.Tag;
            writeLog wl = new writeLog();
            wl.User = ss.Staff;
            wl.LogDate = new DateTime(ss.WriteTime);
            wl.IsComment = true;
            wl.CommentPersonName = this.User.KuName;
            wl.ShowDialog();
        }
        #endregion








    }
}
