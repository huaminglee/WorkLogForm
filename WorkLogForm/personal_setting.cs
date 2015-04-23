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
using CommonClass;
using System.Net;
using System.IO;

namespace WorkLogForm
{
    public partial class personal_setting : Form
    {
        #region 头像制作
        private Point m_LastMSPoint;
        private Rectangle newCutRect;
        private Rectangle oldCutRect;

        


        #endregion


        private delegate void loadTreedle();
        BaseService baseService = new BaseService();
        private Hobby ri_zhi_hobby;
        private Hobby ri_cheng_hobby;
        //private Hobby sui_bi_hobby;
        private WkTRole role;

        private List<WkTUser> users = new List<WkTUser>();
        private List<WkTDept> deptperts = new List<WkTDept>();
        IList<WkTUser> shares = new List<WkTUser>();
        IList<WkTUser> sharesRicheng = new List<WkTUser>();

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
            this.MouseWheel += personal_setting_MouseWheel;
        }

        public main themain;

        private FileUpDown fileop;
        IList hobbysList;
        IList hobbysRiChenglist;
        #region 自定义窗体初始化方法
        /// <summary>
        /// 初始化window（界面效果）
        /// </summary>
        private void initialWindow()
        {
            creatWindow.SetFormRoundRectRgn(this, 15);
            //creatWindow.SetFormShadow(this);
        }
        #endregion

        private void personal_setting_Load(object sender, EventArgs e)
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
            newCutRect = new Rectangle(86, 70, 114, 114);
            oldCutRect = newCutRect;
            if (this.formLocation != null)
            {
                this.Location = formLocation;
            }
            this.backgroundWorkerOfLoadTheTree.RunWorkerAsync();
        }

        public void createTree(TreeView tv, IList<WkTUser> shares)
        {
            #region 加载树结构
            TreeNode Gt = new TreeNode();
            Gt.Text = "部门";
            tv.Nodes.Add(Gt);
           
            if (this.deptperts.Count > 0)
            {
                foreach (WkTDept o in deptperts)
                {
                    TreeNode t1 = new TreeNode();
                    t1.Tag = o;
                    t1.Text = o.KdName;
                   
                    if (this.users.Count > 0)
                    {
                        foreach (WkTUser oo in users)
                        {
                            if (oo.Id != user.Id)
                            {
                                if(oo.Kdid.Id == o.Id)
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
                    }
                    Gt.Nodes.Add(t1);
                }
            }
            #endregion
        
        }
        

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            TreeNode t = e.Node;
            TreeView tree = (TreeView)sender;
            SelectTree(t,tree.Nodes[0]);
        }

        private void SelectTree(TreeNode t,TreeNode tt)
        {
            if (t.Text == "部门")
            {
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


        #region 取消自动登录
        private void button2_Click(object sender, EventArgs e)
        {
            IniReadAndWrite.IniWriteValue("temp", "auto", CommonStaticParameter.YES);
            MessageBox.Show("设置自动登录成功！");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            IniReadAndWrite.IniWriteValue("temp", "auto", CommonStaticParameter.NO);
            MessageBox.Show("取消自动登录成功！");

        }
        #endregion

        #region 更改密码
        
        private void button5_Click(object sender, EventArgs e)
        {

            IList pwd = baseService.ExecuteSQL("select right(sys.fn_VarBinToHexStr(hashbytes('MD5', '" + textBox1.Text.Trim() + "')),32)"); // 数据库属性，跟具体表无关
            if (pwd == null || pwd.Count <= 0)
            {
                MessageBox.Show("登录异常！");
                return;
            }
            object[] pwdArray = (object[])pwd[0];
            //因为是共用表 选择是工作小秘书相关的角色
            IList userList = baseService.loadEntityList("select u from WkTUser u right join u.UserRole role where role.KrDESC='工作小秘书角色' and u.KuLid='" + user.KuLid + "' and u.KuPassWD='" + pwdArray[0] + "'");
            if (userList == null || userList.Count <= 0)
            {
                MessageBox.Show("密码错误！");
                return;
            }
            else if (userList.Count > 1)
            {
                MessageBox.Show("用户异常，请联系管理员！");
                return;
            }
            else
            {
                if (this.textBox2.Text.Trim() == this.textBox3.Text.Trim() && this.textBox2.Text.Trim() != "" && this.textBox3.Text.Trim() != "")
                {
                    
                    IList newpwd = baseService.ExecuteSQL("select right(sys.fn_VarBinToHexStr(hashbytes('MD5', '" + textBox2.Text.Trim() + "')),32)");
                    object[] newpwdArray = (object[])newpwd[0];
                    if (newpwd == null || newpwd.Count <= 0)
                    {
                        MessageBox.Show("异常！");
                        return;
                    }
                    user.KuPassWD = newpwdArray[0].ToString();
                    baseService.SaveOrUpdateEntity(user);
                    this.textBox1.Text = "";
                    this.textBox2.Text = "";
                    this.textBox3.Text = "";
                    MessageBox.Show("修改成功！");
                }
                else
                {
                    MessageBox.Show("两次输入的密码不一致！");
                    this.textBox2.Text = "";
                    this.textBox3.Text = "";
                    return;
                }
            }
        }

        #endregion


       


        #region 头像制作

        void personal_setting_MouseWheel(object sender, MouseEventArgs e)
        {
            if (this.tabControl1.SelectedIndex == 0)
            {
                var t = newCutRect.Size;
                t.Width += (e.Delta / 120) * 3;
                t.Height += (e.Delta / 120) * 3;
                newCutRect.Size = t;
                if (newCutRect.Width < 114)
                {
                    newCutRect.Width = 114;
                    newCutRect.Height = 114;
                }
                if (newCutRect.Width > 300)
                {
                    newCutRect.Width = 300;
                    newCutRect.Height = 300;
                }

                oldCutRect.Width = newCutRect.Width;
                oldCutRect.Height = newCutRect.Height;
                pb_photo_original.Refresh();
            }
        }

        private void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedIndex == 0)
            {
              
            }
            if (this.tabControl1.SelectedIndex == 1 )//|| this.tabControl1.SelectedIndex == 2)
            {
                createTree(treeView1,shares);
            }
            if (this.tabControl1.SelectedIndex == 2)
            {
                createTree(treeView2,sharesRicheng);
            }

        }

        private void btn_photo_make_Click(object sender, EventArgs e)
        {
            OpenFileDialog opdialog = new OpenFileDialog();
            opdialog.InitialDirectory = @"C:\";
            opdialog.FilterIndex = 1;
            opdialog.Filter = "Image   Files(*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG";
            if (opdialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.pb_photo_original.Image = Image.FromFile(opdialog.FileName);
        }


      

        private void btn_photo_done_Click(object sender, EventArgs e)
        {
            if (this.pb_photo_original.Image != null)
            {
                this.btn_photo_done.Cursor = Cursors.WaitCursor;
                this.pb_photo_cut.BackgroundImage = null;
                Bitmap bitmap = new Bitmap(newCutRect.Width, newCutRect.Height);
                //创建作图区域   
                Graphics graphic = Graphics.FromImage(bitmap);
                Point p = this.pb_photo_original.PointToScreen(this.oldCutRect.Location);
                //截取原图相应区域写入作图区   
                //graphic.DrawImage(this.pb_photo_original.Image, new Rectangle(0, 0, 128, 160), oldCutRect, GraphicsUnit.Pixel);
                graphic.CopyFromScreen(p.X, p.Y, 0, 0, newCutRect.Size);



                Bitmap zoomBitmap = new Bitmap(bitmap, newCutRect.Size);

                string tmpname = CommonStaticParameter.ICONS + @"\tmp" + DateTime.Now.Ticks.ToString() + ".png";
                CommonClass.OperateImage.CutForSquare(zoomBitmap, tmpname, 114, 100);


                this.pb_photo_cut.BackgroundImage = ToRoundPic(tmpname);
                
                zoomBitmap.Dispose();
                bitmap.Dispose();
                graphic.Dispose();
            }
            else
            {
                MessageBox.Show("您还未选择头像！");
            }
            this.btn_photo_done.Cursor = Cursors.Hand;
        }

        private Bitmap ToRoundPic(string addd)
        {
            //保存图象   
            string filename = addd;//如果不是png类型，须转换
            System.Drawing.Bitmap ybitmap = new System.Drawing.Bitmap(filename);
            for (int y = 0; y < ybitmap.Width; y++)
            {
                for (int x = 0; x < ybitmap.Height; x++)
                {
                    if ((x - ybitmap.Width / 2) * (x - ybitmap.Width / 2) + (y - ybitmap.Width / 2) * (y - ybitmap.Width / 2) > ybitmap.Width / 2 * ybitmap.Width / 2)
                    {
                        ybitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(0, 255, 255, 255));
                    }
                }
            }

            Graphics g = Graphics.FromImage(ybitmap);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.DrawImage(ybitmap, new Point(0, 0));
            g.DrawEllipse(new Pen(Color.White), 0, 0, ybitmap.Width, ybitmap.Width);
            g.Dispose();

           

            return ybitmap;
        }

       

        private void SaveAndUploadHeadPic()
        {
            if (this.pb_photo_cut.BackgroundImage != null)
            {
                Bitmap savpic = new Bitmap(this.pb_photo_cut.BackgroundImage);
                string addressonlyid = CommonStaticParameter.ICONS + @"\" + this.user.Id.ToString() + ".png";
                string address = CommonStaticParameter.ICONS + @"\" + this.user.Id.ToString() + "__" + DateTime.Now.Ticks.ToString() + ".png";

                if (!Directory.Exists(CommonStaticParameter.ICONS))
                    Directory.CreateDirectory(CommonStaticParameter.ICONS);

                //删除原来的临时文件
                string[] files = Directory.GetFiles(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"icons", this.user.Id.ToString() + "__" + "*.png", System.IO.SearchOption.AllDirectories);
                if (files.Length > 0)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        FileInfo oldfi = new FileInfo(files[i]);
                        oldfi.Delete();
                    }
                }
                savpic.Save(address); //保存临时文件
                //ToRoundPic(address).Save(address);
                if (fileop == null)
                {
                    string _ip = Securit.DeDES(FileReadAndWrite.IniReadValue("ftpfile", "ip"));
                    string _id = Securit.DeDES(FileReadAndWrite.IniReadValue("ftpfile", "id"));
                    string _pwd = Securit.DeDES(FileReadAndWrite.IniReadValue("ftpfile", "pwd"));
                    fileop = new FileUpDown(_ip, _id, _pwd);
                }
                try
                {
                    if (fileop.DirectoryExist(this.user.Id.ToString() + ".png", "Iconpics"))
                    {
                        fileop.DeleteFileName(this.user.Id.ToString() + ".png", "Iconpics");
                    }
                    fileop.Upload(address, "Iconpics");
                    fileop.Rename(address, this.user.Id.ToString() + ".png", "Iconpics");
                    themain.RefreshHeaderPic();
                    MessageBox.Show("上传成功！");
                }
                catch (Exception exp)
                {
                    MessageBox.Show("上传失败"+exp.ToString());
                    return;
                }
            
            }
        }


        private void panel_shade_MouseDown(object sender, MouseEventArgs e)
        {
            if (oldCutRect.Contains(e.Location))
                this.m_LastMSPoint = e.Location;
        }

        private void panel_shade_MouseMove(object sender, MouseEventArgs e)
        {

            if (e.Button != MouseButtons.Left)
                return;



            if (oldCutRect.Contains(e.Location))
            {
                this.newCutRect.Offset(e.Location.X - this.m_LastMSPoint.X, e.Location.Y - this.m_LastMSPoint.Y);
                if (newCutRect.Location.X < 0)
                    newCutRect.X = 0;
                if (newCutRect.Location.Y < 0)
                    newCutRect.Y = 0;
                if (newCutRect.Location.X + 114 > 300)
                    newCutRect.X = 300 - 114;
                if (newCutRect.Location.Y + 114 > 300)
                    newCutRect.Y = 300 - 114;

                this.panel_shade.Invalidate(oldCutRect, false);
                this.panel_shade.Invalidate(newCutRect, false);
                this.m_LastMSPoint = e.Location;
                oldCutRect = newCutRect;
            }

        }
        private void panel_shade_Paint(object sender, PaintEventArgs e)
        {
            if (this.pb_photo_original.Image == null)
                return;

            // Set clipping region to exclude rectangle.
            e.Graphics.ExcludeClip(newCutRect);

            // Fill large rectangle to show clipping region.
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(168, Color.Black)), this.panel_shade.ClientRectangle);
       
        }


        /// <summary>
        /// 上传头像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            this.button6.Cursor = Cursors.WaitCursor;
            this.SaveAndUploadHeadPic();
            this.button6.Cursor = Cursors.Hand;

        }

        #endregion
        private void loadThetree()
        {
            
                hobbysList = baseService.loadEntityList("from Hobby where STATE=" + (int)IEntity.stateEnum.Normal + " and Staff=" + user.Id + " and TypeFlag = " + (int)Hobby.hobbyTypeEnum.RiZhi);
                hobbysRiChenglist = baseService.loadEntityList("from Hobby where STATE=" + (int)IEntity.stateEnum.Normal + " and Staff=" + user.Id + " and TypeFlag = " + (int)Hobby.hobbyTypeEnum.RiCheng);


                if (hobbysList != null && hobbysList.Count != 0)
                {
                    foreach (Hobby oo in hobbysList)
                    {
                        if(oo.SharedStaffs != null && oo.SharedStaffs.Count>0)
                        {
                            foreach (WkTUser w in oo.SharedStaffs)
                            {
                                shares.Add(w);
                            }
                        }
                    }
                }

                if (hobbysRiChenglist != null && hobbysRiChenglist.Count != 0)
                {
                    foreach (Hobby oo in hobbysRiChenglist)
                    {
                        if (oo.SharedStaffs != null && oo.SharedStaffs.Count > 0)
                        {
                            foreach (WkTUser w in oo.SharedStaffs)
                            {
                                sharesRicheng.Add(w);
                            }
                        }
                    }
                }

                string sql = "select u from WkTDept u";
                IList depts = baseService.loadEntityList(sql);
                if (depts !=null&& depts.Count > 0)
                {
                    foreach (WkTDept de in depts)
                    {
                        this.deptperts.Add(de);
                    }
                }

                string sql1 = "select u from WkTUser u";
                IList userlist = baseService.loadEntityList(sql1);
                if (userlist != null && userlist.Count > 0)
                {
                    foreach (WkTUser u in userlist)
                    {
                        this.users.Add(u);
                    }
                }

                //createTree(treeView1, shares);
                //createTree(treeView2, sharesRicheng);
            //}

        }

        private void backgroundWorkerOfLoadTheTree_DoWork(object sender, DoWorkEventArgs e)
        {
            loadThetree();
        }
    }
}
