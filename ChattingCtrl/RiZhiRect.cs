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
    public partial class RiZhiRect : UserControl
    {
        public RiZhiRect()
        {
            InitializeComponent();
        }
        public int HeaderId
        {
            set
            {
                this.headerIconPicturebox2.UserId = value;
            }
        }
        public new string Name
        {
            set
            {
                this.SharePersonName.Text = value;
                this.label1.Location = new Point(this.SharePersonName.Location.X + this.SharePersonName.Width +1,this.label1.Location.Y);
            }
        }
        public string TimeText
        {
            set
            {
                this.timeLabel.Text = value;
            }
        }

        public string Contenttext
        {
            set
            {
                this.Content.Text = value;
                int contentheight = ((value.Length / 20) + 1) * 20+10;
                Content.Height = contentheight;
                this.ContentBg.Height = Content.Location.Y + contentheight + 20;
                this.Height = this.ContentBg.Location.Y + this.ContentBg.Height + 20;
                this.pictureBox1.Location = new Point(this.pictureBox1.Location.X, this.ContentBg.Location.Y + this.ContentBg.Height + 10);
            }
        }

        private void ContentBg_MouseEnter(object sender, EventArgs e)
        {
            this.ContentBg.BackgroundImage = ChattingCtrl.Properties.Resources.GrayRectBgOn;
        }

        private void ContentBg_MouseLeave(object sender, EventArgs e)
        {
            this.ContentBg.BackgroundImage = ChattingCtrl.Properties.Resources.GrayRect;
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




    }
}
