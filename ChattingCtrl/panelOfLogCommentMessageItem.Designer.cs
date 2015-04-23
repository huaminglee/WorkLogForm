namespace ChattingCtrl
{
    partial class panelOfLogCommentMessageItem
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
            this.headerIconPicturebox1 = new ChattingCtrl.HeaderIconPicturebox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.headerIconPicturebox1)).BeginInit();
            this.SuspendLayout();
            // 
            // headerIconPicturebox1
            // 
            this.headerIconPicturebox1.BackColor = System.Drawing.Color.Transparent;
            this.headerIconPicturebox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.headerIconPicturebox1.Location = new System.Drawing.Point(28, 1);
            this.headerIconPicturebox1.Name = "headerIconPicturebox1";
            this.headerIconPicturebox1.Size = new System.Drawing.Size(40, 40);
            this.headerIconPicturebox1.TabIndex = 0;
            this.headerIconPicturebox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label1.ForeColor = System.Drawing.Color.Gray;
            this.label1.Location = new System.Drawing.Point(81, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(170, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "程小倩评论了你的日志";
            this.label1.Click += new System.EventHandler(this.ContentBg_Click);
            this.label1.MouseEnter += new System.EventHandler(this.label1_MouseEnter);
            this.label1.MouseLeave += new System.EventHandler(this.label1_MouseLeave);
            // 
            // panelOfLogCommentMessageItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.headerIconPicturebox1);
            this.Name = "panelOfLogCommentMessageItem";
            this.Size = new System.Drawing.Size(281, 45);
            ((System.ComponentModel.ISupportInitialize)(this.headerIconPicturebox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HeaderIconPicturebox headerIconPicturebox1;
        private System.Windows.Forms.Label label1;
    }
}
