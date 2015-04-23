using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ChattingCtrl
{
    public partial class panelOfLogCommentMessageItem : UserControl
    {
        public panelOfLogCommentMessageItem()
        {
            InitializeComponent();
            this.Margin = new Padding(0);
        }




        private void label1_MouseEnter(object sender, EventArgs e)
        {
            this.label1.ForeColor = Color.DodgerBlue;
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            this.label1.ForeColor = Color.Gray;
        }

        //定义委托
        public delegate void ContentClickHandle(object sender, EventArgs e);
        //定义事件
        public event ContentClickHandle ContentClicked;

        private void ContentBg_Click(object sender, EventArgs e)
        {
            if (ContentClicked != null)
                ContentClicked(sender, new EventArgs());//把按钮自身作为参数传递
        }

        public string NameContent
        {
            set 
            {
                this.label1.Text = value + "评论了你的日志";
            }
        }
        public int HeaderUserid
        {
            set 
            {
                this.headerIconPicturebox1.UserId = value;
            }
        }
       
    }
}
