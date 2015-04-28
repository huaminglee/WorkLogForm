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
    public partial class ChattingTimePanel : UserControl
    {
        public ChattingTimePanel()
        {
            InitializeComponent();
        }
        public string TimeString
        {
            set 
            {
                this.label1.Text = value;
            }
        }
    }
}
