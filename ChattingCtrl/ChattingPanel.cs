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
    public partial class ChattingPanel : UserControl
    {
        public ChattingPanel()
        {
            InitializeComponent();
            //this.flowLayoutPanelOfChattingNow.Height = 20;
        }
        public void AddChattingSubItem(ChatintSubItem item)
        {
            this.flowLayoutPanelOfChattingNow.Controls.Add(item);
            this.flowLayoutPanelOfChattingNow.Height = this.flowLayoutPanelOfChattingNow.Height + item.Height+20;
            Point newPoint = new Point(0, this.flowLayoutPanel1.Height - flowLayoutPanel1.AutoScrollPosition.Y);
            flowLayoutPanel1.AutoScrollPosition = newPoint;
        }
        public void AddChattingTimePanel(ChattingTimePanel time)
        {
            this.flowLayoutPanelOfChattingNow.Controls.Add(time);
            this.flowLayoutPanelOfChattingNow.Height = this.flowLayoutPanelOfChattingNow.Height + time.Height;
            Point newPoint = new Point(0, this.flowLayoutPanel1.Height - flowLayoutPanel1.AutoScrollPosition.Y);
            flowLayoutPanel1.AutoScrollPosition = newPoint;
        }

        private void labelofSeemore_MouseEnter(object sender, EventArgs e)
        {
            this.labelofSeemore.ForeColor = Color.Black;
        }
        private void labelofSeemore_MouseLeave(object sender, EventArgs e)
        {
            this.labelofSeemore.ForeColor = Color.DarkGray;
        }
        public void AddChattingSubItemInDistoryPanel(ChatintSubItem item)
        {
            this.flowLayoutPanelofDisChatting.Controls.Add(item);
            this.flowLayoutPanelofDisChatting.Height = this.flowLayoutPanelofDisChatting.Height + item.Height + 20;
            //Point newPoint = new Point(0, this.flowLayoutPanel1.Height - flowLayoutPanel1.AutoScrollPosition.Y);
            //flowLayoutPanelofDisChattingflowLayoutPanel1.AutoScrollPosition = newPoint;
        }
        public void AddChattingTimePanelInDistoryPanel(ChattingTimePanel time)
        {
            this.flowLayoutPanelofDisChatting.Controls.Add(time);
            this.flowLayoutPanelofDisChatting.Height = this.flowLayoutPanelofDisChatting.Height + time.Height;
            //Point newPoint = new Point(0, this.flowLayoutPanel1.Height - flowLayoutPanel1.AutoScrollPosition.Y);
            //flowLayoutPanel1.AutoScrollPosition = newPoint;
        }

        //定义委托
        public delegate void SeeMorelabelClickHandle(object sender, EventArgs e);
        //定义事件
        public event SeeMorelabelClickHandle SeemoreLabelClicked;

        private void SeemoreLabelClicked_Click(object sender, EventArgs e)
        {
            if (SeemoreLabelClicked != null)
                SeemoreLabelClicked(sender, new EventArgs());//把按钮自身作为参数传递
        }

        private void flowLayoutPanel1_Scroll(object sender, ScrollEventArgs e)
        {
            if (flowLayoutPanel1.AutoScrollPosition.Y == 0)
            {
                this.labelofSeemore.Visible = true;
            }
            else
            {
                this.labelofSeemore.Visible = false;   
            }
             
        }
        public void SetSeeMoreLabelText(string str)
        {
            this.labelofSeemore.Text = str;
            this.labelofSeemore.Enabled = false;
        }





    }
}
