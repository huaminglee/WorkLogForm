using ClassLibrary;
using System;
using System.Collections;
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
    public partial class InstantMessenger : Form
    {
        BaseService baseService = new BaseService();
        KjqbService.Service1Client ser = new KjqbService.Service1Client();
        List<KjqbService.ChatInService> chattinguserlist;
        private WkTUser user;

        public WkTUser User
        {
            get { return user; }
            set { user = value; }
        }
        
        private Point formLocation;
        public Point FormLocation
        {
            get { return formLocation; }
            set { formLocation = value; }
        }


        public List<KjqbService.ChatInService> Chattinguserlist
        {
            get { return chattinguserlist; }
            set { chattinguserlist = value; }
        }


        Timer mainReceiveMessage;
        /// <summary>
        /// 在子界面控制主界面的timer开关
        /// </summary>
        public Timer MainReceiveMessage
        {
            get { return mainReceiveMessage; }
            set { mainReceiveMessage = value; }
        }

        /// <summary>
        /// 如果已经在和这个人聊天则不能再创建窗体
        /// </summary>
        List<WkTUser> chatwindowsusers;
        public InstantMessenger()
        {
            InitializeComponent();
            initialWindow();
            chatwindowsusers = new List<WkTUser>();
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

        private void InstantMessenger_Load(object sender, EventArgs e)
        {
            if (this.formLocation != null)
            {
                this.Location = formLocation;
            }
            createTree(treeView1);

            if (Chattinguserlist != null && Chattinguserlist.Count >0)
            {
                foreach (KjqbService.ChatInService chat in chattinguserlist)
                {
                    WkTUser ww = new WkTUser();
                    ww = (WkTUser)baseService.loadEntity(ww, chat.SendUserId);

                    if (!chatwindowsusers.Contains(ww))
                    {
                        Panel pp = IsInFlowPanel2(ww);
                        if (pp == null)
                        {
                            CreateWktuserInPanel(ww);
                            Panel pp1 = IsInFlowPanel2(ww);
                            Label ll = (Label)pp1.Controls[1];
                            ll.Text = "1";
                        }
                        else
                        {
                            Label ll = (Label)pp.Controls[1];
                            ll.Text = (int.Parse(ll.Text) + 1).ToString();
                        }
                    }
                }
            }
        }

        private void close_pictureBox_Click(object sender, EventArgs e)
        {
            this.mainReceiveMessage.Enabled = true;
            if (chatwindowsusers.Count == 0)
            {
                Chattinguserlist.Clear();
                this.Visible = false;
            }
        }

        #region 加载树结构
        public void createTree(TreeView tv)
        {
            #region 加载树结构
            TreeNode Gt = new TreeNode();
            Gt.Text = "部门";
            Gt.Tag = 1;
            tv.Nodes.Add(Gt);
            string sql = "select u from WkTDept u";
            IList depts = baseService.loadEntityList(sql);
            if (depts != null && depts.Count > 0)
            {
                foreach (WkTDept o in depts)
                {
                    TreeNode t1 = new TreeNode();
                    t1.Tag = o;
                    t1.NodeFont = new Font("微软雅黑", 10, FontStyle.Regular);
                    t1.Text = o.KdName.Trim();

                    string sql1 = "select u from WkTUser u left join u.Kdid dept where dept.Id = " + o.Id + " order by u.KuOnline desc";
                    IList userlist = baseService.loadEntityList(sql1);

                    if (userlist != null && userlist.Count > 0)
                    {
                        foreach (WkTUser oo in userlist)
                        {
                            if (oo.Id != user.Id)
                            {
                                TreeNode t2 = new TreeNode();
                                if (oo.KuOnline == 1)
                                {
                                    t2.NodeFont = new Font("微软雅黑", 12, FontStyle.Bold);
                                    t2.Text = oo.KuName + "  在线";
                                }
                                else if (oo.KuOnline == 0)
                                {
                                    t2.NodeFont = new Font("微软雅黑", 12, FontStyle.Regular);
                                    t2.Text = oo.KuName;
                                }

                                t2.Tag = oo;
                                
                                t1.Nodes.Add(t2);
                            }
                        }
                    }
                    Gt.Nodes.Add(t1);
                }
            }
            #endregion

        }
        #endregion


        #region 双击树节点
        /// <summary>
        /// 弹出聊天窗口 创建临时的聊天快捷label
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode node = e.Node;
            if(node.Tag.GetType() == typeof(WkTUser))
            {
                WkTUser receuser = (WkTUser)node.Tag;
                
                if (!chatwindowsusers.Contains(receuser))
                {
                    Panel pp = IsInFlowPanel2(receuser);
                    if (pp == null)
                    {
                        CreateWktuserInPanel(receuser);
                    }

                    this.chatwindowsusers.Add(receuser);
                    ChatWindows chat = new ChatWindows();
                    chat.ReceiveUser = receuser;
                    chat.SendUser = this.user;
                    chat.Chatwindwosuser = chatwindowsusers;
                    chat.Show();
                }
                else
                {
                   
                }
               
            }

        }

        private Panel IsInFlowPanel2(WkTUser w)
        {
            if (flowLayoutPanel1.Controls != null&&flowLayoutPanel1.Controls.Count >0)
            {
                foreach (Control c in flowLayoutPanel1.Controls)
                {
                    Panel p = (Panel)c;
                    WkTUser t = (WkTUser)c.Tag;
                    if (t.Id == w.Id)
                    {
                        return p;
                    }
                }
                return null;
            }
            else
            {
                return null;
            }
        
        }
        #endregion

        /// <summary>
        /// 轮训数据库的消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerOfreceiveMessageInThisWin_Tick(object sender, EventArgs e)
        {
            #region 接受聊天信息
            try
            {
                KjqbService.ChatInService[] lists7;
                lists7 = ser.SearchChatInfo((int)this.user.Id);
                for (int i = 0; i < lists7.Length; i++)
                {
                    WkTUser ww = new WkTUser();
                    ww = (WkTUser)baseService.loadEntity(ww, lists7[i].SendUserId);

                    if (!chatwindowsusers.Contains(ww))
                    {
                        Panel pp = IsInFlowPanel2(ww);
                        if (pp == null)
                        {
                            CreateWktuserInPanel(ww);
                            Panel pp1 = IsInFlowPanel2(ww);
                            Label ll = (Label)pp1.Controls[1];
                            ll.Text = "1";
                        }
                        else
                        {
                            Label ll = (Label)pp.Controls[1];
                            ll.Text = (int.Parse(ll.Text) + 1).ToString();
                        }
                    }
                   

                }
              
            }
            catch { }
            #endregion



        }

        private void CreateWktuserInPanel(WkTUser w)
        {
            Panel p1 = new Panel();
            p1.Width = 123;
            p1.Height = 42;
            p1.Parent = flowLayoutPanel1;
            p1.Tag = w;
            p1.Cursor = Cursors.Hand;



            Label l1 = new Label();
            l1.Font = new Font("微软雅黑", 13, FontStyle.Bold);
            l1.Text = w.KuName;
            l1.AutoSize = true;
            
            l1.Location = new Point(1, 11);
            l1.Parent = p1;
            

            Label l2 = new Label();
            l2.Font = new Font("微软雅黑", 13, FontStyle.Bold);
            l2.Text = "";
            l2.ForeColor = Color.Red;
            l2.AutoSize = true ;
            l2.Location = new Point(l1.Width+5, 11);
            l2.Parent = p1;

            l1.DoubleClick += l1_DoubleClick;
            
        }

        void l1_DoubleClick(object sender, EventArgs e)
        {

            Label ll1 = (Label)sender;

            Panel node = (Panel)ll1.Parent;

            if (node.Tag.GetType() == typeof(WkTUser))
            {
                WkTUser receuser = (WkTUser)node.Tag;

                if (!chatwindowsusers.Contains(receuser))
                {
                    Label ll = (Label)node.Controls[1];

                    ll.Text = "0";

                    this.chatwindowsusers.Add(receuser);
                    ChatWindows chat = new ChatWindows();
                    chat.ReceiveUser = receuser;
                    chat.SendUser = this.user;
                    chat.Chatwindwosuser = chatwindowsusers;
                    chat.Show();
                }
                else
                {

                }

            }
        }

        /// <summary>
        /// 不显示的时候则关闭线程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InstantMessenger_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible == true)
            {
                if (this.timerOfreceiveMessageInThisWin.Enabled == false)
                {
                    if (Chattinguserlist != null && Chattinguserlist.Count > 0)
                    {
                        foreach (KjqbService.ChatInService chat in chattinguserlist)
                        {
                            WkTUser ww = new WkTUser();
                            ww = (WkTUser)baseService.loadEntity(ww, chat.SendUserId);

                            if (!chatwindowsusers.Contains(ww))
                            {
                                Panel pp = IsInFlowPanel2(ww);
                                if (pp == null)
                                {
                                    CreateWktuserInPanel(ww);
                                    Panel pp1 = IsInFlowPanel2(ww);
                                    Label ll = (Label)pp1.Controls[1];
                                    ll.Text = "1";
                                }
                                else
                                {
                                    Label ll = (Label)pp.Controls[1];
                                    ll.Text = (int.Parse(ll.Text) + 1).ToString();
                                }
                            }
                        }
                    }
                    timerOfreceiveMessageInThisWin.Enabled = true;
                    timerofrefreshOnlineTree.Enabled = true;
                }
            }
            else
            {
                if (this.timerOfreceiveMessageInThisWin.Enabled == true)
                {
                    timerOfreceiveMessageInThisWin.Enabled = false;
                    timerofrefreshOnlineTree.Enabled = false;
                }
            }
        }


        //更新onlinetree
        private void timerofrefreshOnlineTree_Tick(object sender, EventArgs e)
        {
            this.treeView1.Nodes.Clear();
            createTree(treeView1);
        }
    }
}
