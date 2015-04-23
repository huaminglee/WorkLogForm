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
    public partial class PanelOfRIchengAllInfo : UserControl
    {
        public PanelOfRIchengAllInfo()
        {
            InitializeComponent();
        }
        //public RiChengRect RechengREct
        //{
        //    get
        //    {
        //        return this.riChengRect1;
        //    }
        //    set
        //    {
        //        this.riChengRect1 = value;
        //        this.Height = value.Height;
        //        if(this.Height > 454)
        //        {
        //            this.Height = 454;
        //        }
        //    }
        //}
        public void SetRechengREct(RiChengRect rechengREct)
        {
            string str = rechengREct.ContentText;
            if(rechengREct.ContentText.Length > 300)
                str = rechengREct.ContentText.Substring(0,300)+"……";

            int contentheight = ((str.Length / 20) + 1) * 16;
            Content.Height = contentheight;
            this.ContentBg.Height = Content.Location.Y + contentheight + 20;
            this.Content.Text = str;
            this.Height = this.ContentBg.Location.Y + this.ContentBg.Height + 20;
            if (this.Height > 454)
                this.Height = 454;
        }

        private void label2_MouseEnter(object sender, EventArgs e)
        {
            this.label2.ForeColor = Color.Black;
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            this.label2.ForeColor = Color.Gray;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }
    }
}
