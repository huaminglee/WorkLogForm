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
    public partial class LabelMessageBox : Label
    {
        System.Timers.Timer t = new System.Timers.Timer(2000);
        private delegate void MessageShow(string str);
        private delegate void SettheUnvis();  

        public LabelMessageBox()
        {
            InitializeComponent();
            t.Elapsed += t_Elapsed;
            t.AutoReset = false;
            t.Enabled = false;
        }

        void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            SetUnVisible();
        }
        public void MessageageShow(string str)
        {
            SetVisible(str);
            t.Enabled = true;
        }

        private void SetVisible(string str)
        {
            if (this.InvokeRequired)
            {
                MessageShow d = new MessageShow(SetVisible);
                this.Invoke(d, new object[] { str });  
            }
            else
            {
                this.Visible = true;
                base.Text = str;
            }
        }

        private void SetUnVisible()
        {
            if (this.InvokeRequired)
            {
                SettheUnvis d = new SettheUnvis(SetUnVisible);
                this.Invoke(d);
            }
            else
            {
                this.Visible = false;
            }
        }

        
    }
}
