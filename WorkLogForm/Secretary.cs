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
    public partial class Secretary : Form
    {
        public Secretary()
        {
            InitializeComponent();
        }

        private string logToolTip(string l)
        {
            int s = (int)Math.Sqrt(3*l.Length/4);
            String result = "";
            while (l.Length > s)
            {
                result += l.Substring(0, s) + System.Environment.NewLine;
                l = l.Substring(s);
            }
            if (l.Length > 0)
            {
                result += l;
            }
            return result;

        }

        private void Secretary_Load(object sender, EventArgs e)
        {
            base.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - base.Width, Screen.PrimaryScreen.WorkingArea.Height - base.Height);
            toolTip1.Active =false;
            toolTip1.SetToolTip(label1, logToolTip("参加院内大会"));
            toolTip1.Active = true; toolTip1.OwnerDraw = false;
            toolTip1.Show("参加院内大会", label1, -100, -20, 100000);
            //toolTip1.Container
        }

        private void Secretary_MouseHover(object sender, EventArgs e)
        {
           // toolTip1.SetToolTip(this, logToolTip("参加院内大会"));
        }
    }
}
