using ChattingCtrl;
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

        /// <summary>
        /// 获取当前服务器时间查询连天记录时候用
        /// </summary>
        private DateTime serviceTime;
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
            this.headerIconPicturebox1.UserId = int.Parse(receiveUser.Id.ToString());
            try
            {
                KjqbService.ChatInService[] chat;//= new KjqbService.ChatInService()[];
                chat = ser.SearchChatInfoUnReadContainSendId((int)sendUser.Id,(int)receiveUser.Id);
                //List<KjqbService.ChatInService> chatlist =  chat.ToList();

                //var resultList = from item in chatlist
                //                 where item.SendUserId == ReceiveUser.Id
                //                 select item;

                //var escortList = resultList.ToList();

                //chatlist = escortList;
                //chatlist.Reverse();
                System.Array.Reverse(chat);

                for (int i = 0; i < chat.Length; i++)
                {
                    createChatPanel(chat[i].ChatContent, receiveUser.Id, chat[i].TimeStamp);
                }
               
                serviceTime = new DateTime();
                serviceTime = ser.GetServiceTime();
            }
            catch { }
        }

        private void close_pictureBox_Click(object sender, EventArgs e)
        {
            this.chatwindwosuser.Remove(receiveUser);
            this.ser.SetChatInfoIsReadContainSendId((int)this.sendUser.Id, (int)this.receiveUser.Id);
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
                createChatPanel(this.textBox1.Text.Trim(), this.sendUser.Id,DateTime.Now);
               

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

                this.textBox1.Text = "";
                 
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            #region 向服务器发送数据
            try
            {
                KjqbService.ChatInService[]  chat ;//= new KjqbService.ChatInService()[];
                chat =  ser.SearchChatInfoContainSendId((int)sendUser.Id,(int)receiveUser.Id);
                System.Array.Reverse(chat);
                for(int i = 0; i<chat.Length ;i++)
                {
                    createChatPanel(chat[i].ChatContent,receiveUser.Id,chat[i].TimeStamp);
                }
               
               
            }
            catch { }

            #endregion
        }

        private void createChatPanel(string chatContent,long userId,DateTime dt)
        {
            ChattingTimePanel time = new ChattingTimePanel();
            time.TimeString = dt.ToString("yyyy-MM-dd HH:mm");
            this.chattingPanel1.AddChattingTimePanel(time);

            ChatintSubItem chatsubitem = new ChatintSubItem();
            chatsubitem.Message = chatContent;
            chatsubitem.HeaderId = int.Parse(userId.ToString());
            if (userId == sendUser.Id)
            {
                chatsubitem.IsSayIngIsMe = true;
            }
            else 
            {
                chatsubitem.IsSayIngIsMe = false;
            }
            this.chattingPanel1.AddChattingSubItem(chatsubitem);
        
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


       



     

        private void chattingPanel1_SeemoreLabelClicked(object sender, EventArgs e)
        {
            //this.linkLabel2.Cursor = Cursors.WaitCursor;
            try
            {
                KjqbService.ChatInService[] chat;

                chat = ser.SearchChatHistory((int)sendUser.Id, (int)receiveUser.Id, serviceTime);
                for (int i = 0; i < chat.Length; i++)
                {
                    if (chat[i].ReceiveUserId == sendUser.Id)
                    {
                        ChatintSubItem subitem = new ChatintSubItem();
                        subitem.HeaderId = int.Parse(receiveUser.Id.ToString());
                        subitem.Message = chat[i].ChatContent;
                        subitem.IsSayIngIsMe = false;

                        ChattingTimePanel time = new ChattingTimePanel();
                        time.TimeString = chat[i].TimeStamp.ToString("yyyy-MM-dd HH:mm");

                        this.chattingPanel1.AddChattingSubItemInDistoryPanel(subitem);

                        this.chattingPanel1.AddChattingTimePanelInDistoryPanel(time);
                        //createChatPanelOfflowlayout2(chat[i].ChatContent, receiveUser.KuName, chat[i].TimeStamp);
                    }
                    else if (chat[i].ReceiveUserId == receiveUser.Id)
                    {
                        ChatintSubItem subitem = new ChatintSubItem();
                        subitem.HeaderId = int.Parse(sendUser.Id.ToString());
                        subitem.Message = chat[i].ChatContent;
                        subitem.IsSayIngIsMe = true;

                        ChattingTimePanel time = new ChattingTimePanel();
                        time.TimeString = chat[i].TimeStamp.ToString("yyyy-MM-dd HH:mm");

                        this.chattingPanel1.AddChattingSubItemInDistoryPanel(subitem);

                        this.chattingPanel1.AddChattingTimePanelInDistoryPanel(time);

                        //createChatPanelOfflowlayout2(chat[i].ChatContent, sendUser.KuName, chat[i].TimeStamp);
                    }
                }

                serviceTime = chat[chat.Length - 1].TimeStamp;

                if (chat.Length > 0)
                {
                    //Point newPoint = new Point(0, 0);
                    //flowLayoutPanel1.AutoScrollPosition = newPoint;
                }
                else
                {
                this.chattingPanel1.SetSeeMoreLabelText("没有了…");
                }
            }
            catch { }

            //this.linkLabel2.Cursor = Cursors.Hand;
        }

       

       


    
    }
}
