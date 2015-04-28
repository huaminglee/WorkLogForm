namespace ChattingCtrl
{
    partial class ChattingPanel
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.flowLayoutPanelofDisChatting = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelofSeemore = new System.Windows.Forms.Label();
            this.flowLayoutPanelOfChattingNow = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanelofDisChatting
            // 
            this.flowLayoutPanelofDisChatting.FlowDirection = System.Windows.Forms.FlowDirection.BottomUp;
            this.flowLayoutPanelofDisChatting.Location = new System.Drawing.Point(0, 27);
            this.flowLayoutPanelofDisChatting.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanelofDisChatting.Name = "flowLayoutPanelofDisChatting";
            this.flowLayoutPanelofDisChatting.Size = new System.Drawing.Size(499, 16);
            this.flowLayoutPanelofDisChatting.TabIndex = 0;
            this.flowLayoutPanelofDisChatting.WrapContents = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelofSeemore);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(496, 21);
            this.panel1.TabIndex = 0;
            // 
            // labelofSeemore
            // 
            this.labelofSeemore.AutoSize = true;
            this.labelofSeemore.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelofSeemore.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelofSeemore.ForeColor = System.Drawing.Color.DarkGray;
            this.labelofSeemore.Location = new System.Drawing.Point(233, 3);
            this.labelofSeemore.Name = "labelofSeemore";
            this.labelofSeemore.Size = new System.Drawing.Size(56, 17);
            this.labelofSeemore.TabIndex = 0;
            this.labelofSeemore.Text = "查看更多";
            this.labelofSeemore.Click += new System.EventHandler(this.SeemoreLabelClicked_Click);
            this.labelofSeemore.MouseEnter += new System.EventHandler(this.labelofSeemore_MouseEnter);
            this.labelofSeemore.MouseLeave += new System.EventHandler(this.labelofSeemore_MouseLeave);
            // 
            // flowLayoutPanelOfChattingNow
            // 
            this.flowLayoutPanelOfChattingNow.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelOfChattingNow.Location = new System.Drawing.Point(0, 43);
            this.flowLayoutPanelOfChattingNow.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanelOfChattingNow.Name = "flowLayoutPanelOfChattingNow";
            this.flowLayoutPanelOfChattingNow.Size = new System.Drawing.Size(499, 17);
            this.flowLayoutPanelOfChattingNow.TabIndex = 1;
            this.flowLayoutPanelOfChattingNow.WrapContents = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.Controls.Add(this.flowLayoutPanelofDisChatting);
            this.flowLayoutPanel1.Controls.Add(this.flowLayoutPanelOfChattingNow);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(530, 280);
            this.flowLayoutPanel1.TabIndex = 2;
            this.flowLayoutPanel1.WrapContents = false;
            this.flowLayoutPanel1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.flowLayoutPanel1_Scroll);
            // 
            // ChattingPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "ChattingPanel";
            this.Size = new System.Drawing.Size(530, 280);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelofDisChatting;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelOfChattingNow;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelofSeemore;
    }
}
