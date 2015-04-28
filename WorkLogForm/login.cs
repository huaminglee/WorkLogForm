using System;
using System.Collections;
using System.Windows.Forms;
using WorkLogForm.Service;
using ClassLibrary;
using WorkLogForm.WindowUiClass;
using WorkLogForm.CommonClass;
using System.Drawing;
using System.Threading;
using System.Text.RegularExpressions;
using CCWin;
using System.IO;

namespace WorkLogForm
{
    public partial class login : SkinMain
    {

        //声明一个更新Address的委托
    public delegate void IsLoginSucceedHandler(object sender, LoginEventArgs e);

    //声明一个更新Address的事件
    public event IsLoginSucceedHandler ShowMain;

    private delegate void CloseDele();


        private Size formSize = new Size(363, 300);
        
        private BaseService baseService = new BaseService();
        
        private WkTUser user;
        public WkTUser User
        {
            get { return user; }
            set { user = value; }
        }

        private WkTRole role;
        public WkTRole Role
        {
            get { return role; }
            set { role = value; }
        }
        public delegate void loginErrorDelegate();
        public delegate void LoadDele();
        public delegate string LogInDele();

        public login()
        {
            InitializeComponent();
            
        }
        public string loginMessage;
        private void login_Load(object sender, EventArgs e)
        {
            this.backgroundWorkerOfLoad.RunWorkerAsync();
            timer1.Start();
        }

        #region 自定义窗体初始化方法
        /// <summary>
        /// 初始化window（界面效果）
        /// </summary>
        private void initialWindow()
        {
            //creatWindow.SetFormRoundRectRgn(this, 15);//圆角
            //creatWindow.SetFormShadow(this);//阴影
        }


        
        /// <summary>
        /// 记住密码 自动填充效果 加载头像
        /// </summary>
        private void initialData() 
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new LoadDele(initialData));
            }
            else
            {
                this.pictureBoxOfHeadIcon.BackgroundImage = WorkLogForm.Properties.Resources.AutoIconBigWhite;

                if (IniReadAndWrite.IniReadValue("temp", "rem").Equals(CommonStaticParameter.YES))
                {
                    string un = Securit.DeDES(IniReadAndWrite.IniReadValue("temp", "un"));
                    string pwd = Securit.DeDES(IniReadAndWrite.IniReadValue("temp", "pw"));
                    string myid = IniReadAndWrite.IniReadValue("temp", "myid");
                    this.pictureBoxOfRememberPwd.BackgroundImage = WorkLogForm.Properties.Resources.Checked;
                    #region 加载自己的头像
                    string address = CommonStaticParameter.ICONS + @"\" + myid + ".png";
                    if (File.Exists(address))
                    {

                        string filename = address;//如果不是png类型，须转换
                        System.Drawing.Bitmap ybitmap = new System.Drawing.Bitmap(filename);
                        this.pictureBoxOfHeadIcon.BackgroundImage = ybitmap;
                    }

                    #endregion
                    //rem_checkBox.Checked = true;
                    textBox1.Text = un != null && un != "" ? un : "请输入用户名";
                    if (pwd != null && pwd != "")
                    {
                        this.textBox2.UseSystemPasswordChar = true;
                        this.textBox2.Font = new Font(new FontFamily("微软雅黑"), 12);
                        this.textBox2.Font = new Font(new FontFamily("宋体"), 12);
                        this.textBox2.Text = pwd;
                    }
                    else
                    {
                        this.textBox2.IsPasswordChar = false;
                        this.textBox2.Font = new Font(new FontFamily("微软雅黑"), 12);
                        this.textBox2.Font = new Font(new FontFamily("宋体"), 12);
                        this.textBox2.Text = "请输入密码";
                    }
                }
                if (IniReadAndWrite.IniReadValue("temp", "auto").Equals(CommonStaticParameter.YES))
                {
                    pictureBoxofAutoLogin.BackgroundImage = WorkLogForm.Properties.Resources.Checked;
                    button1_Click(pictureBox1, new EventArgs());
                }
            }
            //
        }


        #endregion


        #region 最小化 与 关闭按钮
        private void min_pictureBox_MouseEnter(object sender, EventArgs e)
        {
            min_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.Minenter;
        }
        private void min_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            min_pictureBox.BackgroundImage = null;
        }
        private void min_pictureBox_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void close_pictureBox_Click(object sender, EventArgs e)
        {
            if(this.backgroundWorkerOfLoginSucceed.IsBusy)
            {
                this.backgroundWorkerOfLoginSucceed.CancelAsync();
                this.backgroundWorkerOfLoginSucceed.Dispose();
            }
            this.Close();
        }
        private void close_pictureBox_MouseEnter(object sender, EventArgs e)
        {
            close_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.Closeenter;
        }
        private void close_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            close_pictureBox.BackgroundImage = null;
        }
        #endregion
       

        /// <summary>
        /// 登陆按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            LoginLoading();
            backgroundWorker1.RunWorkerAsync();
        }




        private string Tologin()
        {

            //if (this.InvokeRequired)
            //{
            //    this.Invoke(new LogInDele(Tologin));
            //}
            //else
            //{
                string IpAdress;
                IpAdress = GetIP();
                if (IpAdress == "未成功获取IP地址")
                {
                    //ShowLabelMessage("未成功获取IP地址");
                    return "未成功获取IP地址";
                }
                else
                {
                    //http://www.txt2re.com/index-csharp.php3?s=10.1.15.100&-6&-21&-11&-22&-18&20&-23&5 用的是这个网站生成的正则表达式
                    string re1 = "(10)";	// Integer Number 1
                    string re2 = "(\\.)";	// Any Single Character 1
                    string re3 = "(1)";	// Integer Number 2
                    string re4 = "(\\.)";	// Any Single Character 2
                    string re5 = "(1)";	// Any Single Digit 1
                    string re6 = "(\\d)";	// Any Single Digit 2
                    string re7 = "(\\.)";	// Any Single Character 3
                    string re8 = "(\\d+)";	// Integer Number 3

                    Regex r = new Regex(re1 + re2 + re3 + re4 + re5 + re6 + re7 + re8, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    Match m = r.Match(IpAdress);
                    //if (m.Success)
                    {
                        #region 登陆效果
                        if (textBox1.Text.Trim() == "" || textBox1.Text.Trim() == "请输入用户名" || textBox2.Text.Trim() == "" || textBox2.Text.Trim() == "请输入密码")
                        {
                            return "用户名和密码不能为空!"; //不在执行下面的函数
                        }


                        string str = loginMethod();
                        return str;

                        #endregion
                    }
                    //else
                    {
                        //MessageBox.Show("您未在正确地点登录！");
                        //return;
                    }

                    //}

                }
        }

        protected string GetIP()   //获取本地IP
        {
            System.Net.IPHostEntry IpEntry = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            for (int i = 0; i != IpEntry.AddressList.Length; i++)
            {
                if (IpEntry.AddressList[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return IpEntry.AddressList[i].ToString();
                }
            }
            return "未成功获取IP地址";
        }






        /// <summary>
        /// 登陆框收起的效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {

            #region 页面渐显
            if (this.Opacity != 1)
            {
                this.Opacity = this.Opacity + 0.1;  //((double)(255 - this.SkinOpacity) / (double)255);
                //this.SkinOpacity = this.SkinOpacity - 1;
            }
            else
            {
                this.timer1.Stop();
            }
            #endregion

        }


        /// <summary>
        /// 登录函数 其中包括查库操作
        /// </summary>
        private string loginMethod()
        {
            try
            {
                    #region 将用户信息存入临时文件
                    if (pictureBoxOfRememberPwd.BackgroundImage != null)
                    {
                        IniReadAndWrite.IniWriteValue("temp", "rem", CommonStaticParameter.YES);
                    }
                    else
                    {
                        IniReadAndWrite.IniWriteValue("temp", "rem", CommonStaticParameter.NO);
                    }
                    if (pictureBoxofAutoLogin.BackgroundImage != null)
                    {
                        IniReadAndWrite.IniWriteValue("temp", "auto", CommonStaticParameter.YES);
                    }
                    else
                    {
                        IniReadAndWrite.IniWriteValue("temp", "auto", CommonStaticParameter.NO);
                    }
                    IniReadAndWrite.IniWriteValue("temp", "un", Securit.DES(textBox1.Text.Trim()));
                    IniReadAndWrite.IniWriteValue("temp", "pw", Securit.DES(textBox2.Text.Trim()));
                    #endregion

                    //判断是否加密成功
                    IList pwd = baseService.ExecuteSQL("select right(sys.fn_VarBinToHexStr(hashbytes('MD5', '" + textBox2.Text.Trim() + "')),32)"); // 数据库属性，跟具体表无关
                    if (pwd == null || pwd.Count <= 0)
                    {
                        return "登录异常！";
                    }
                    object[] pwdArray = (object[])pwd[0];
                    //因为是共用表 选择是工作小秘书相关的角色
                    IList userList = baseService.loadEntityList("select u from WkTUser u right join u.UserRole role where role.KrDESC='工作小秘书角色' and u.KuLid='" + textBox1.Text.Trim() + "' and u.KuPassWD='" + pwdArray[0] + "'");
                    if (userList == null || userList.Count <= 0)
                    {
                        return "用户名或密码错误！";
                    }
                    else if (userList.Count > 1)
                    {
                        return "用户异常，请联系管理员！";
                    }
                    else
                    {
                        WkTUser u = (WkTUser)userList[0];
                        foreach (WkTRole r in u.UserRole)
                        {
                            if (r.KrDESC.Trim().Equals("工作小秘书角色"))//是本系统的用户角色
                            {
                                role = r;
                            }
                        }
                        this.User = (WkTUser)userList[0];
                        IniReadAndWrite.IniWriteValue("temp", "myid", User.Id.ToString());
                        
                        return "登录成功！";
                    }
                   
               // }
            }
            catch
            {
                return "未能与服务器建立连接……";
            }

        }


        /// <summary>
        ///  登录按钮的普通状态
        /// </summary>
        private void loginError() 
        {
            this.label1.Cursor = Cursors.Hand;
            this.pictureBox1.Cursor = Cursors.Hand;
            SetLabel1location("登   录");
            this.LoginButtonNoOn();
            this.pictureBox1.Cursor = Cursors.Hand;
            this.label1.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// 登录按钮登录中的状态
        /// </summary>
        private void LoginLoading()
        {
            this.pictureBox1.Cursor = Cursors.WaitCursor;
            this.label1.Cursor = Cursors.WaitCursor;
            SetLabel1location("正在登录…");
            this.LoginButtonOn();
            this.label1.ForeColor = Color.White;
        }

        private void SetLabel1location(string str)
        {
            this.label1.Text = str;
            int x = (this.Width / 2) - (this.label1.Width / 2);
            this.label1.Location = new Point(x, this.label1.Location.Y);
        }



        #region 针对自定义窗体的  窗体移动代码
        private int x_point, y_point;
        private void login_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.x_point = e.X;
                this.y_point = e.Y;
            }
        }

        private void login_MouseMove(object sender, MouseEventArgs e)
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

        #region 登录按钮的选中效果
        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
           this.LoginButtonOn();
        }

        /// <summary>
        /// 登录按钮的选中效果
        /// </summary>
        private void LoginButtonOn()
        {
            pictureBox1.BackgroundImage = WorkLogForm.Properties.Resources.LoginbuttonEnter;
            this.label1.BackColor = Color.FromArgb(59, 199, 241);
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            LoginButtonNoOn();
        }

        /// <summary>
        ///登录按钮的未选中效果 
        /// </summary>
        private void LoginButtonNoOn()
        {
            pictureBox1.BackgroundImage = WorkLogForm.Properties.Resources.LoginButton;
            this.label1.BackColor = Color.FromArgb(19, 187, 249);
        }

        #endregion

        #region 初始焦点
        private void login_Shown(object sender, EventArgs e)
        {
            pictureBox1.Focus();
        }
        #endregion

        #region 选中框的事件
        private void pictureBoxOfRememberPwd_Click(object sender, EventArgs e)
        {
            PictureBox picbox = (PictureBox)sender;
            if (picbox.BackgroundImage != null)
            {
                picbox.BackgroundImage = null;
            }
            else
            {
                picbox.BackgroundImage = WorkLogForm.Properties.Resources.Checked;
            }
        }
        #endregion

        #region 两个输入框的事件

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "" && this.textBox1.Focused == false)
            {
                textBox1.Text = "请输入用户名";
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "" && this.textBox2.Focused == false)
            {
                this.textBox2.IsPasswordChar = false;
                this.textBox2.Font = new Font(new FontFamily("微软雅黑"), 12);
                this.textBox2.Font = new Font(new FontFamily("宋体"), 12);
                this.textBox2.Text = "请输入密码";

            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "请输入密码")
            {
                textBox2.Text = "";
                if (textBox2 != null)
                {
                    this.textBox2.IsPasswordChar = true;
                    this.textBox2.Font = new Font(new FontFamily("微软雅黑"), 12);
                    this.textBox2.Font = new Font(new FontFamily("宋体"), 12);
                }
               
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "请输入用户名")
            {
                textBox1.Text = "";
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            string str = this.textBox2.Text;
            if (e.KeyCode == Keys.Enter)
            {
                LoginLoading();
                backgroundWorker1.RunWorkerAsync();
            }
        }
        #endregion

        #region label消息弹出框
        private void timeroflabelmessage_Tick(object sender, EventArgs e)
        {
            this.labelofMessage.Visible = false;
            this.timeroflabelmessage.Stop();
        }

        private void ShowLabelMessage(string str)
        {
            this.labelofMessage.Visible = true;
            this.labelofMessage.Text = str;
            int x = (this.Width / 2) - (this.labelofMessage.Width / 2);
            this.labelofMessage.Location = new Point(x, this.labelofMessage.Location.Y);
            this.timeroflabelmessage.Start();
        }

        #endregion



        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            loginMessage = Tologin();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            this.ShowLabelMessage(loginMessage);
            loginError();
            if (loginMessage == "登录成功！")
            {
                SetLabel1location("正在绘制主界面…");
                this.ShowLabelMessage("登录成功！");
                this.pictureBox1.Enabled = false;
                this.label1.Enabled = false;
                this.close_pictureBox.Enabled = false;
                this.pictureBox1.Cursor = Cursors.WaitCursor;
                this.backgroundWorkerOfLoginSucceed.RunWorkerAsync();
            }
        }


        private void backgroundWorkerOfLoad_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            initialData();
        }


        public void CloseLogin()
        {
            if (this.InvokeRequired)
            {
                CloseDele d = new CloseDele(CloseLogin);
                this.Invoke(d);
            }
            else
            {
                this.Close();
                //SetLabel1location("出现主界面");
            }
        }

        private void backgroundWorkerOfLoginSucceed_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var args = new LoginEventArgs(loginMessage);
            ShowMain(this, args);
        }
    }

    public class LoginEventArgs : System.EventArgs
    {
        private string _loginMess;
        public LoginEventArgs(string mloginMessage)
        {
            this._loginMess = mloginMessage;
        }
        public string LoginMess { get { return _loginMess; } }
    }

}
