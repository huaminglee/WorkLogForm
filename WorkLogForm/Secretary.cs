using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CCWin;
namespace WorkLogForm
{
    public partial class Secretary : SkinMain
    {

        #region 透明窗体属性注释
        /************************透明窗体的属性**************************/
        //GradientTime：控件层渐变特效时长(越小越快)。
        //MainPosition：窗口在绘图层位置。
        //SkinBack：设置或获取绘图层窗口背景。
        //SkinMobile：窗体是否可以移动。
        //SkinOpacity：设置或获取绘图层窗口透明度(0-255)。
        //SkinShowInTaskbar：绘图层是否出现在Windows任务栏中。
        //SkinSize：设置或获取绘图层窗口大小。
        //SkinTrankColor：绘图层需要透明的颜色。
        //SkinWhetherTank：绘图层是否开启位图仿透明。注意(SkinOpacity < 255时，此属性为False可达到背景透明，控件不透明的效果。)。
        /************************透明窗体的属性**************************/
        #endregion

        public Secretary(main main)
        {
            InitializeComponent();
            Main = main;
        }
        
        private main main;
        public main Main
        {
            get { return main; }
            set { main = value; }
        }


        writeLog write_log;
        /// <summary>
        /// 接受写日志的窗口
        /// </summary>
        public writeLog Write_log
        {
            get { return write_log; }
            set { write_log = value; }
        }
        

        private void Secretary_Load(object sender, EventArgs e)
        {
            WindowsStartPosition();
            this.flowLayoutPanel1.Controls.Clear();
        }

        #region 向flow1中添加message消息
        /// <summary>
        /// 向flow1中添加message消息
        /// </summary>
        /// <param name="content"></param>
        public void AddMessageLabelInFlowPanel1(string content)
        {
            Label label = new Label();
            label.Font = new Font(new FontFamily("微软雅黑"), 10, FontStyle.Regular);
            label.ForeColor = Color.DarkRed;
            label.AutoSize = true;
            label.Text = content;
            label.Margin = new System.Windows.Forms.Padding(0);
            label.Parent = flowLayoutPanel1;
            if (this.flowLayoutPanel1.Controls.Count > 4)
            {
                this.linkLabel1.Visible = true;
                this.flowLayoutPanel1.Controls.RemoveAt(0);
            }
        }
        #endregion

        public void ClearMessagePanel()
        {
            this.flowLayoutPanel1.Controls.Clear();
            this.linkLabel1.Visible = false;
        }

        /// <summary>
        /// 显示您该写日志了的字体
        /// </summary>
        public void SetWriteIsVis()
        {
            this.linkLabel1.Visible = true;
        }

        #region 设定窗体的初始化位置
        /// <summary>
        /// 设定窗体的初始化位置
        /// </summary>
        public void WindowsStartPosition()
        {
            int x = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Width - this.SkinSize.Width+this.MainPosition.X;
            int y = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Height - this.SkinSize.Height + this.MainPosition.Y; ;
            this.SetDesktopLocation(x, y);
            this.SkinOpacity = 255;
            this.Opacity = 1;
        }
        #endregion

        #region 窗体关闭
        private void contextMenuStrip1_Click(object sender, EventArgs e)
        {
            //this.SkinOpacity = 0;
            //this.Visible = false;
            this.Close();
        }
        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
            //this.SkinOpacity = 0;
            //this.Visible = false;


        }
        #endregion
        
        #region 点击弹出写日志的页面
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (write_log == null || write_log.IsDisposed)
            {
                write_log = new writeLog();
                write_log.User = main.User;
            }
            if (!write_log.Created)
            {
                write_log.Show();
            }
            else
            {
                write_log.WindowState = FormWindowState.Normal;
                write_log.Focus();
            }
            if (write_log.DialogResult == DialogResult.OK)
            {
                if (main.IsWriteLog == 0)
                {
                    main.IsWriteLog = 1;
                }
            }
        }
        #endregion

        #region
        public void AddRiChengInFlow2(string time, string title)
        {
            Panel panel = new Panel();
            panel.Width = 100;
            panel.Height = 43;
            panel.Margin = new Padding(0);
            panel.Parent = flowLayoutPanel2;

            Label labeltime = new Label();
            labeltime.Font = new Font(new FontFamily("微软雅黑"), 9, FontStyle.Regular);
            labeltime.ForeColor = Color.Black;
            labeltime.AutoSize = true;
            labeltime.Text = time;
            labeltime.Location = new Point(3, 6);
            labeltime.Parent = panel;

            Label labeltitle = new Label();
            labeltitle.Font = new Font(new FontFamily("微软雅黑"), 9, FontStyle.Regular);
            labeltitle.ForeColor = Color.Black;
            labeltitle.AutoSize = true;
            if(title.Length >7)
            {
                title = title.Substring(0, 6)+"…";
            }
            labeltitle.Text = title;
            labeltitle.Location = new Point(3, 23);
            labeltitle.Parent = panel;
            if (this.flowLayoutPanel2.Controls.Count > 2)
            {
                this.flowLayoutPanel2.Controls.RemoveAt(0);
            }
        }
        #endregion

        /// <summary>
        /// 更多聊天
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            main.Visible = true;
            main.TopMost = true;
        }

    }
}
