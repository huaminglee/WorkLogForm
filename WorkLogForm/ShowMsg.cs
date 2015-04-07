using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WorkLogForm
{
    public partial class ShowMsg : Form
    {
        /// <summary>
        /// 计秒
        /// </summary>
        int SecondCount = 10 ;

        public ShowMsg()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.SecondCount--;
            this.button1.Text = "关闭"+"("+SecondCount.ToString()+")";
            if (SecondCount <= 0)
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void ShowMsg_Load(object sender, EventArgs e)
        {

        }
    }
}
