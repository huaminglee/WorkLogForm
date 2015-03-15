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
        }

        private void close_pictureBox_Click(object sender, EventArgs e)
        {
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
