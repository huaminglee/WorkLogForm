using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ChattingCtrl
{
    public partial class HeaderIconPicturebox : PictureBox
    {
        public HeaderIconPicturebox()
        {
            InitializeComponent();
            base.BackColor = Color.Transparent;
            base.BackgroundImageLayout = ImageLayout.Stretch;
        }
        public  int UserId
        {
            set
            {
                base.BackgroundImage = LoadPicture.GetHeadIcon(value);
            }
        }
        public  int WidthAndHeight
        {
            set
            {
                base.Width = value;
                base.Height = value;
            }
        }
    }
}
