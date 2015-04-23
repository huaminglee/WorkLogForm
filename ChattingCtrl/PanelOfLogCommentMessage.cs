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
    public partial class PanelOfLogCommentMessage : UserControl
    {
        public PanelOfLogCommentMessage()
        {
            InitializeComponent();
           
        }

        private void Close_MouseEnter(object sender, EventArgs e)
        {
            this.Close.ForeColor = Color.Black;
        }

        private void Close_MouseLeave(object sender, EventArgs e)
        {
            this.Close.ForeColor = Color.Gray;
        }

        private void Close_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        public void AddCommentItem(panelOfLogCommentMessageItem iteml)
        {
            this.flowLayoutPanel1.Controls.Add(iteml);
            this.flowLayoutPanel1.Height = iteml.Height * this.flowLayoutPanel1.Controls.Count;
            this.Height = this.flowLayoutPanel1.Location.Y + this.flowLayoutPanel1.Height + 20;
            if (this.Height > 450)
            {
                this.flowLayoutPanel1.Height = 400;
                this.Height = 450;
            }
        }
        public void RemoveCommentItem(panelOfLogCommentMessageItem iteml)
        {
            this.flowLayoutPanel1.Controls.Remove(iteml);
            this.flowLayoutPanel1.Height = iteml.Height * this.flowLayoutPanel1.Controls.Count;
            this.Height = this.flowLayoutPanel1.Location.Y + this.flowLayoutPanel1.Height + 20;
            if (this.flowLayoutPanel1.Controls.Count == 0)
            {
                this.Visible = true;
            }
        }
        public void clearAllItem()
        {
            this.flowLayoutPanel1.Controls.Clear();
            this.flowLayoutPanel1.Height = 100;
            this.Height = this.flowLayoutPanel1.Location.Y + this.flowLayoutPanel1.Height + 20;
        }
        public int ItemsCount
        {
            get
            {
                return this.flowLayoutPanel1.Controls.Count;
            }
        }
    }
}
