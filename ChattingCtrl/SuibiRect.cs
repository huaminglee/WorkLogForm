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
    public partial class SuibiRect : UserControl
    {
        public SuibiRect()
        {
            InitializeComponent();
        }

        public int HeaderId
        {
            set
            {
                this.headerIconPicturebox1.UserId = value;
            }
        }
        public new string Name
        {
            set
            {
                this.NameLabel.Text = value;
            }
        }
        public string TimeText
        {
            set
            {
                this.TimeLabel.Text = value;
            }
        }

        public string Contenttext
        {
            set
            {
                this.Content.Text = value;
                int contentheight = ((value.Length / 20) + 1) * 20 + 10;
                Content.Height = contentheight;
                this.ContentBg.Height = Content.Location.Y + contentheight + 20;
                this.Height = this.ContentBg.Location.Y + this.ContentBg.Height + 20;
                this.pictureBox1.Location = new Point(this.pictureBox1.Location.X, this.ContentBg.Location.Y + this.ContentBg.Height + 10);
            }
        }

    }
}
