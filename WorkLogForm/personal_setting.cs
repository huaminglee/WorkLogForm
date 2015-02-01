using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WorkLogForm.WindowUiClass;
using System.Collections;
using ClassLibrary;
using WorkLogForm.Service;
using WorkLogForm.CommonClass;

namespace WorkLogForm
{
    public partial class personal_setting : Form
    {
        BaseService baseService = new BaseService();
        private Hobby ri_zhi_hobby;
        private Hobby ri_cheng_hobby;
        private Hobby sui_bi_hobby;
        private WkTRole role;

   
        public WkTRole Role
        {
            get { return role; }
            set { role = value; }
        }
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
        public personal_setting()
        {
            InitializeComponent();
            initialWindow();
        }
        IList hobbysList;
        IList hobbysRiChenglist;
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

        private void personal_setting_Load(object sender, EventArgs e)
        {
            if (this.formLocation != null)
            {
                this.Location = formLocation;
            }

            hobbysList = baseService.loadEntityList("from Hobby where STATE=" + (int)IEntity.stateEnum.Normal + " and Staff=" + user.Id + " and TypeFlag = " + (int)Hobby.hobbyTypeEnum.RiZhi);
            hobbysRiChenglist = baseService.loadEntityList("from Hobby where STATE=" + (int)IEntity.stateEnum.Normal + " and Staff=" + user.Id + " and TypeFlag = " + (int)Hobby.hobbyTypeEnum.RiCheng);


            IList<WkTUser> shares = new List<WkTUser>();
            IList<WkTUser> sharesRicheng = new List<WkTUser>();


            if (hobbysList != null && hobbysList.Count != 0)
            {
                foreach (Hobby oo in hobbysList)
                {
                    
                    shares = oo.SharedStaffs;
                }
            }

            if (hobbysRiChenglist != null && hobbysRiChenglist.Count != 0)
            {
                foreach (Hobby oo in hobbysRiChenglist)
                {

                    sharesRicheng = oo.SharedStaffs;
                }
            }


            createTree(treeView1,shares);
            createTree( treeView2,sharesRicheng);



        }

        public void createTree(TreeView tv, IList<WkTUser> shares)
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
                    t1.Text = o.KdName;

                    string sql1 = "select u from WkTUser u left join u.Kdid dept where dept.Id = " + o.Id;
                    IList userlist = baseService.loadEntityList(sql1);

                    if (userlist != null && userlist.Count > 0)
                    {
                        foreach (WkTUser oo in userlist)
                        {
                            if (oo.Id != user.Id)
                            {
                                TreeNode t2 = new TreeNode();
                                t2.Text = oo.KuName;
                                t2.Tag = oo;
                                if (shares != null && shares.Count > 0)
                                {
                                    foreach (WkTUser n in shares)
                                    {
                                        if (n.Id == oo.Id)
                                        {
                                            t2.Checked = true;
                                        }
                                    }
                                }

                                t1.Nodes.Add(t2);
                            }
                        }
                    }
                    Gt.Nodes.Add(t1);
                }
            }
            #endregion
        
        }
        

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            TreeNode t = e.Node;
            SelectTree(t);
        }

        private void SelectTree(TreeNode t)
        {
            if (t.Text == "部门")
            {
                TreeNode tt = treeView1.Nodes[0];
                if (t.Checked == true)
                {

                    foreach (TreeNode t2 in tt.Nodes)
                    {
                        t2.Checked = true;
                    }

                }
                else if (t.Checked == false)
                {
                    foreach (TreeNode t2 in tt.Nodes)
                    {
                        t2.Checked = false;
                        foreach (TreeNode t3 in t2.Nodes)
                        {
                            t3.Checked = false;
                        }
                    }
                }
            }
            else if (t.Text != "部门")
            {
                if (t.Tag is WkTDept)
                {

                    if (t.Checked == true)
                    {
                        foreach (TreeNode t3 in t.Nodes)
                        {
                            t3.Checked = true;
                        }
                    }
                    else if (t.Checked == false)
                    {
                        foreach (TreeNode t3 in t.Nodes)
                        {
                            t3.Checked = false;
                        }
                    }


                }
                else if (t.Tag is WkTUser)
                {

                }

            }
        
        }


        /// <summary>
        /// 日志的保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            TreeNode t = treeView1.Nodes[0];
            //表中是否有存 如果有先删除原来的
            if(hobbysList != null)
            {
                foreach (Hobby h in hobbysList)
                {
                    h.State = (int)IEntity.stateEnum.Deleted;
                    baseService.SaveOrUpdateEntity(h);
                }
            }

            ri_zhi_hobby = new Hobby();
            ri_zhi_hobby.Staff = user;
            ri_zhi_hobby.State = (int)IEntity.stateEnum.Normal;
            ri_zhi_hobby.TimeStamp = DateTime.Now.Ticks;
            ri_zhi_hobby.TypeFlag = (int)Hobby.hobbyTypeEnum.RiZhi;
           
            if (ri_zhi_hobby.SharedStaffs == null)
            {
                ri_zhi_hobby.SharedStaffs = new List<WkTUser>();
            }
            else
            {
                ri_zhi_hobby.SharedStaffs.Clear();
            }

            foreach(TreeNode t1 in t.Nodes)
            {
              foreach(TreeNode t2 in t1.Nodes)
              {
                  if(t2.Checked == true)
                  {
                      WkTUser u = (WkTUser)t2.Tag;
                      ri_zhi_hobby.SharedStaffs.Add(u);
                  }
                 
              }
            }
           
            baseService.SaveOrUpdateEntity(ri_zhi_hobby);
            MessageBox.Show("保存成功！");
        }


        private void close_pictureBox_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        /// <summary>
        /// 日程偏好保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            TreeNode t = treeView2.Nodes[0];
            //表中是否有存 如果有先删除原来的
            if (hobbysList != null)
            {
                foreach (Hobby h in hobbysRiChenglist)
                {
                    h.State = (int)IEntity.stateEnum.Deleted;
                    baseService.SaveOrUpdateEntity(h);
                }
            }

            ri_cheng_hobby = new Hobby();
            ri_cheng_hobby.Staff = user;
            ri_cheng_hobby.State = (int)IEntity.stateEnum.Normal;
            ri_cheng_hobby.TimeStamp = DateTime.Now.Ticks;
            ri_cheng_hobby.TypeFlag = (int)Hobby.hobbyTypeEnum.RiCheng;

            if (ri_cheng_hobby.SharedStaffs == null)
            {
                ri_cheng_hobby.SharedStaffs = new List<WkTUser>();
            }
            else
            {
                ri_cheng_hobby.SharedStaffs.Clear();
            }

            foreach (TreeNode t1 in t.Nodes)
            {
                foreach (TreeNode t2 in t1.Nodes)
                {
                    if (t2.Checked == true)
                    {
                        WkTUser u = (WkTUser)t2.Tag;
                        ri_cheng_hobby.SharedStaffs.Add(u);
                    }

                }
            }

            baseService.SaveOrUpdateEntity(ri_cheng_hobby);
            MessageBox.Show("保存成功！");
        }

    }
}
