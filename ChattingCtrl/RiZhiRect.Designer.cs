namespace ChattingCtrl
{
    partial class RiZhiRect
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
            this.SharePersonName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.timeLabel = new System.Windows.Forms.Label();
            this.headerIconPicturebox2 = new ChattingCtrl.HeaderIconPicturebox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ContentBg = new System.Windows.Forms.Panel();
            this.Content = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.headerIconPicturebox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.ContentBg.SuspendLayout();
            this.SuspendLayout();
            // 
            // SharePersonName
            // 
            this.SharePersonName.AutoSize = true;
            this.SharePersonName.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SharePersonName.Location = new System.Drawing.Point(88, 21);
            this.SharePersonName.Name = "SharePersonName";
            this.SharePersonName.Size = new System.Drawing.Size(72, 27);
            this.SharePersonName.TabIndex = 15;
            this.SharePersonName.Text = "程小倩";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.label1.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label1.Location = new System.Drawing.Point(159, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 21);
            this.label1.TabIndex = 14;
            this.label1.Text = "分享给您的日志";
            // 
            // timeLabel
            // 
            this.timeLabel.AutoSize = true;
            this.timeLabel.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.timeLabel.ForeColor = System.Drawing.Color.DarkGray;
            this.timeLabel.Location = new System.Drawing.Point(92, 54);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(118, 17);
            this.timeLabel.TabIndex = 13;
            this.timeLabel.Text = "2015-12-31 12：12";
            // 
            // headerIconPicturebox2
            // 
            this.headerIconPicturebox2.BackColor = System.Drawing.Color.Transparent;
            this.headerIconPicturebox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.headerIconPicturebox2.Location = new System.Drawing.Point(11, 17);
            this.headerIconPicturebox2.Name = "headerIconPicturebox2";
            this.headerIconPicturebox2.Size = new System.Drawing.Size(60, 60);
            this.headerIconPicturebox2.TabIndex = 16;
            this.headerIconPicturebox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::ChattingCtrl.Properties.Resources.GrayLine;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(13, 242);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(300, 3);
            this.pictureBox1.TabIndex = 18;
            this.pictureBox1.TabStop = false;
            // 
            // ContentBg
            // 
            this.ContentBg.BackgroundImage = global::ChattingCtrl.Properties.Resources.GrayRect;
            this.ContentBg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ContentBg.Controls.Add(this.Content);
            this.ContentBg.Location = new System.Drawing.Point(7, 80);
            this.ContentBg.Name = "ContentBg";
            this.ContentBg.Size = new System.Drawing.Size(315, 143);
            this.ContentBg.TabIndex = 17;
            this.ContentBg.MouseEnter += new System.EventHandler(this.ContentBg_MouseEnter);
            this.ContentBg.MouseLeave += new System.EventHandler(this.ContentBg_MouseLeave);
            // 
            // Content
            // 
            this.Content.BackColor = System.Drawing.Color.Transparent;
            this.Content.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Content.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.Content.Location = new System.Drawing.Point(12, 21);
            this.Content.Name = "Content";
            this.Content.Size = new System.Drawing.Size(292, 105);
            this.Content.TabIndex = 1;
            this.Content.Text = "测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测" +
    "试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测" +
    "试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字";
            this.Content.Click += new System.EventHandler(this.ContentBg_Click);
            this.Content.MouseEnter += new System.EventHandler(this.ContentBg_MouseEnter);
            this.Content.MouseLeave += new System.EventHandler(this.ContentBg_MouseLeave);
            // 
            // RiZhiRect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.headerIconPicturebox2);
            this.Controls.Add(this.SharePersonName);
            this.Controls.Add(this.timeLabel);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.ContentBg);
            this.Name = "RiZhiRect";
            this.Size = new System.Drawing.Size(325, 272);
            ((System.ComponentModel.ISupportInitialize)(this.headerIconPicturebox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ContentBg.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HeaderIconPicturebox headerIconPicturebox2;
        private System.Windows.Forms.Label SharePersonName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.Label Content;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel ContentBg;
    }
}
