using System;
using System.Collections;
using System.Windows.Forms;
using WorkLogForm.Service;
using ClassLibrary;
using WorkLogForm.WindowUiClass;
using WorkLogForm.CommonClass;
using System.Drawing;
using System.Threading;

namespace WorkLogForm
{
    public partial class login : Form
    {
        private Size formSize = new Size(363, 300);
        private Thread loginThread;
        
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
        
        public login()
        {
            InitializeComponent();
        }
        
        private void login_Load(object sender, EventArgs e)
        {
            initialWindow();
            initialData();
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

        /// <summary>
        /// 记住密码 自动填充效果
        /// </summary>
        private void initialData()
        {
            if (IniReadAndWrite.IniReadValue("temp", "rem").Equals(CommonStaticParameter.YES))
            {
                string un = Securit.DeDES(IniReadAndWrite.IniReadValue("temp", "un"));
                string pwd = Securit.DeDES(IniReadAndWrite.IniReadValue("temp", "pw"));
                rem_checkBox.Checked = true;
                textBox1.Text = un!=null&&un!=""?un:"输入用户名";
                if (pwd != null && pwd != "")
                {
                    textBox2.UseSystemPasswordChar = true;
                    textBox2.Text = pwd;
                }
                else
                {
                    textBox2.UseSystemPasswordChar = false;
                    textBox2.Text = "输入密码";
                }
            }
            if (IniReadAndWrite.IniReadValue("temp", "auto").Equals(CommonStaticParameter.YES))
            {
                auto_checkBox.Checked = true;
                button1_Click(pictureBox1, new EventArgs());
            }
        }
        #endregion


        #region 最小化 与 关闭按钮
        private void min_pictureBox_MouseEnter(object sender, EventArgs e)
        {
            min_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.最小化渐变;
        }
        private void min_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            min_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.最小化2;
        }
        private void min_pictureBox_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void close_pictureBox_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void close_pictureBox_MouseEnter(object sender, EventArgs e)
        {
            close_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.关闭渐变;
        }
        private void close_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            close_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.关闭1;
        }
        #endregion
       

        /// <summary>
        /// 登陆按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "" || textBox1.Text.Trim() == "输入用户名" || textBox2.Text.Trim() == "" || textBox2.Text.Trim() == "输入密码")
            {
                MessageBox.Show("用户名和密码不能为空!");
                return; //不在执行下面的函数
            }
            timer1.Start(); //登录窗收起效果
        }

        /// <summary>
        /// 登陆框收起的效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.Height > 154)//窗口渐变效果
            {
                this.Height -= 50;
            }
            else
            {
                this.BackgroundImage = WorkLogForm.Properties.Resources.logined3;
                min_pictureBox.Visible = false;
                close_pictureBox.Visible = false;
                this.Height = 104;
                label2.Text = "loading...";
                loginThread = new Thread(new ThreadStart(loginMethod));
                loginThread.Start();
                timer1.Stop();
            }
        }


        /// <summary>
        /// 登录函数 其中包括查库操作
        /// </summary>
        private void loginMethod()
        {
            loginErrorDelegate led = new loginErrorDelegate(loginMethod);
            if (this.InvokeRequired == true)
            {
                this.BeginInvoke(led);
            }
            else
            {
                #region 将用户信息存入临时文件
                if (rem_checkBox.Checked)
                {
                    IniReadAndWrite.IniWriteValue("temp", "rem", CommonStaticParameter.YES);
                }
                else
                {
                    IniReadAndWrite.IniWriteValue("temp", "rem", CommonStaticParameter.NO);
                }
                if (auto_checkBox.Checked)
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
                    MessageBox.Show("登录异常！");
                    loginError();
                    return;
                }
                object[] pwdArray = (object[])pwd[0];
                //因为是共用表 选择是工作小秘书相关的角色
                IList userList = baseService.loadEntityList("select u from WkTUser u right join u.UserRole role where role.KrDESC='工作小秘书角色' and u.KuLid='" + textBox1.Text.Trim() + "' and u.KuPassWD='" + pwdArray[0] + "'");
                if (userList == null || userList.Count <= 0)
                {
                    MessageBox.Show("用户名或密码错误！");
                    loginError();
                    return;
                }
                else if (userList.Count > 1)
                {
                    MessageBox.Show("用户异常，请联系管理员！");
                    loginError();
                    return;
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
                    this.DialogResult = DialogResult.OK;
                    loginThread.Abort();
                }
            }
        }



        private void loginError() 
        {
            this.BackgroundImage = WorkLogForm.Properties.Resources.login14;
            min_pictureBox.Visible = true;
            close_pictureBox.Visible = true;
            this.Height = 300;
            label2.Text = "工作小秘书";
            loginThread.Abort();
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


        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.BackgroundImage = WorkLogForm.Properties.Resources.loginButton11;
        }
        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.BackgroundImage = WorkLogForm.Properties.Resources.loginButton1;
        }
        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "输入密码")
            {
                textBox2.Text = "";
                textBox2.UseSystemPasswordChar = true;
            }
        }
        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "输入用户名")
            {
                textBox1.Text = "";
            }
        }
        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "输入用户名";
            }
        }
        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = "输入密码";
                textBox2.UseSystemPasswordChar = false;
            }
        }
        private void login_Shown(object sender, EventArgs e)
        {
            pictureBox1.Focus();
        }



    }
}
