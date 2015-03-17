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
using WorkLogForm.WindowUiClass;

namespace WorkLogForm
{
    public partial class ChatWindows : Form
    {
        BaseService baseService = new BaseService();
        KjqbService.Service1Client ser = new KjqbService.Service1Client();

        List<WkTUser> chatwindwosuser;

        public List<WkTUser> Chatwindwosuser
        {
            get { return chatwindwosuser; }
            set { chatwindwosuser = value; }
        }
        private WkTUser sendUser;
        /// <summary>
        /// 由谁发送的也就是当前系统登录人
        /// </summary>
        public WkTUser SendUser
        {
            get { return sendUser; }
            set { sendUser = value; }
        }

        private WkTUser receiveUser;
        /// <summary>
        /// 在发送给谁 也就是接受人
        /// </summary>
        public WkTUser ReceiveUser
        {
            get { return receiveUser; }
            set { receiveUser = value; }
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

        public ChatWindows()
        {
            InitializeComponent();
            initialWindow();
            receiveUser = new WkTUser();
            sendUser = new WkTUser();
        }

        private void ChatWindows_Load(object sender, EventArgs e)
        {
            this.Text = ReceiveUser.KuName;
            this.labelOfReceiveUser.Text = ReceiveUser.Kdid.KdName.Trim() +"  "+ReceiveUser.KuName;

            try
            {
                KjqbService.ChatInService[] chat;//= new KjqbService.ChatInService()[];
                chat = ser.SearchChatInfoUnRead((int)sendUser.Id);
                System.Array.Reverse(chat);
                for (int i = 0; i < chat.Length; i++)
                {
                    createChatPanel(chat[i].ChatContent, receiveUser.KuName, chat[i].TimeStamp);
                }
                Point newPoint = new Point(0, this.flowLayoutPanel1.Height - flowLayoutPanel1.AutoScrollPosition.Y);
                flowLayoutPanel1.AutoScrollPosition = newPoint;
            }
            catch { }
        }

        private void close_pictureBox_Click(object sender, EventArgs e)
        {
            this.chatwindwosuser.Remove(receiveUser);
            this.ser.SetChatInfoIsRead((int)this.sendUser.Id);
            this.Close();
        }

        private void min_pictureBox_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        /// <summary>
        /// 发送按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text.Trim() != "")
            {
                createChatPanel(this.textBox1.Text.Trim(), sendUser.KuName,DateTime.Now);
               

                #region 向服务器发送数据
                try
                {
                    KjqbService.ChatInService chat = new KjqbService.ChatInService();
                    chat.ChatContent = this.textBox1.Text.Trim();
                    chat.SendUserId = sendUser.Id;
                    chat.ReceiveUserId = receiveUser.Id;
                    chat.TimeStamp = DateTime.Now;
                    ser.SaveInChatInfoInService(chat);

                }
                catch { }

                #endregion



                //滚动条显示最后
                Point newPoint = new Point(0, this.flowLayoutPanel1.Height - flowLayoutPanel1.AutoScrollPosition.Y);
                flowLayoutPanel1.AutoScrollPosition = newPoint;

                this.textBox1.Text = "";
                 
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            #region 向服务器发送数据
            try
            {
                KjqbService.ChatInService[]  chat ;//= new KjqbService.ChatInService()[];
                chat =  ser.SearchChatInfo((int)sendUser.Id);
                System.Array.Reverse(chat);
                for(int i = 0; i<chat.Length ;i++)
                {
                    createChatPanel(chat[i].ChatContent,receiveUser.KuName,chat[i].TimeStamp);
                }
                if (chat.Length > 0)
                {
                    Point newPoint = new Point(0, this.flowLayoutPanel1.Height - flowLayoutPanel1.AutoScrollPosition.Y);
                    flowLayoutPanel1.AutoScrollPosition = newPoint;
                }
               
            }
            catch { }

            #endregion
        }

        private void createChatPanel(string chatContent,string userName,DateTime dt)
        {
            Panel p1 = new Panel();
            p1.Width = 534;

            Label l1 = new Label();
            l1.AutoSize = true;
            l1.Text = userName + "     " + dt.ToString("HH:mm") ;
            l1.Font = new Font("微软雅黑", 12, FontStyle.Bold);
            l1.Location = new Point(10,8);
            l1.Parent = p1;

            Label l2 = new Label();
            l2.AutoSize = false;
            l2.Width = 519;
            l2.Text = chatContent;
            l2.Height = ((chatContent.Length / 36) + 1) * 23;
            l2.Font = new Font("微软雅黑", 12, FontStyle.Regular);
            l2.Location = new Point(12, 34);
            l2.Parent = p1;

            p1.Height = l2.Location.Y + l2.Height + 15;

            p1.Parent = flowLayoutPanel1;
        
        }

        #region 窗体移动代码
        private int x_point, y_point;
        private void Leave_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.x_point = e.X;
                this.y_point = e.Y;
            }
        }

        private void Leave_MouseMove(object sender, MouseEventArgs e)
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

       


    
    }
}
