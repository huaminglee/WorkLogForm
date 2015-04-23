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
    public partial class RiChengRect : UserControl
    {
        public RiChengRect()
        {
            InitializeComponent();
        }

        private string _subTitleText;
        public  string SubTitleText
        {
            get { return _subTitleText; }
            set
            {
                this._subTitleText = value;
                this.SubTitle.Text = value;
            }
        }
        private string _contentText;
        public  string ContentText
        {
            get { return _contentText; }
            set
            {
                this._contentText = value;
                this.Content.Text = value;
                int contentheight = ((value.Length / 20) + 1) * 20+10;
                Content.Height = contentheight;
                this.ContentBg.Height = Content.Location.Y + contentheight+20;
                this.Height = this.ContentBg.Location.Y + this.ContentBg.Height + 20;
                this.pictureBox1.Location = new Point(this.pictureBox1.Location.X, this.ContentBg.Location.Y + this.ContentBg.Height + 10);
            }
        }
        private string _arrangePersonNameText;
        public  string ArrangePersonNameText
        {
            get { return _arrangePersonNameText; }
            set
            {
                this._arrangePersonNameText = value;
                this.ArrangePerson.Text = "安排人："+value;
            }
        }
        private string _timeText;
        public  string TimeText
        {
            get 
            {
                return _timeText;
            }
            set
            {
                this._timeText = value;
                this.Dotime.Text = value;
            }
        
        }
        private int _headerId;
        public  int HeaderId
        {
            get
            {
                return _headerId;
            }
            set
            {
                this.headerIconPicturebox2.UserId = value;
                _headerId = value;
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
