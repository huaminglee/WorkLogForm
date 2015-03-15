using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WorkLogForm.WindowUiClass;

namespace WorkLogForm
{
    public partial class InstantMessenger : Form
    {
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
        }

        private void close_pictureBox_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
