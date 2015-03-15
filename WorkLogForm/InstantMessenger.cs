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

        public InstantMessenger()
        {
            InitializeComponent();
            initialWindow();
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
        }

        private void close_pictureBox_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void createTree(TreeView tv)
        {
            #region 加载树结构
            TreeNode Gt = new TreeNode();
            Gt.Text = "部门";
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

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode node = e.Node;
            if(node.Tag.GetType() == typeof(WkTUser))
            {
                WkTUser receuser = (WkTUser)node.Tag;
                ChatWindows chat = new ChatWindows();
                chat.ReceiveUser = receuser;
                chat.SendUser = this.user;
                chat.Show();
            }
        }
    }
}
