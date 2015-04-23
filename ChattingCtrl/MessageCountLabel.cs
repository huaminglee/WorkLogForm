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
    public partial class MessageCountLabel : UserControl
    {

        int messagecount = 0;
        public MessageCountLabel()
        {
            InitializeComponent();
            this.label1.Text = "";
           
        }
        public int MessageCount 
        {
            get 
            {
                return messagecount;
            }
            set 
            {
                messagecount = value;
                string t = value.ToString();
                if (t.Length == 1)
                {
                    if (value != 0)
                    {
                        this.Width = 6;
                    }
                    else
                    {
                        t = "";
                    }
                }
                else if (t.Length == 2)
                {
                    this.Width = 12;
                    this.Location = new Point(this.Location.X - 6, this.Location.Y);
                }
                else if (t.Length == 3)
                {
                    this.Width = 18;
                    this.Location = new Point(this.Location.X - 12, this.Location.Y);
                }
                this.label1.Text = t;
            }
        
        }
        public string Tooltiptext
        {
            set 
            {
                toolTip1.SetToolTip(this.label1, value);
            }
        }
      
    }
}
