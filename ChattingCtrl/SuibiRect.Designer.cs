namespace ChattingCtrl
{
    partial class SuibiRect
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
            this.ContentBg = new System.Windows.Forms.Panel();
            this.Content = new System.Windows.Forms.Label();
            this.TimeLabel = new System.Windows.Forms.Label();
            this.NameLabel = new System.Windows.Forms.Label();
            this.headerIconPicturebox1 = new ChattingCtrl.HeaderIconPicturebox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ContentBg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.headerIconPicturebox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // ContentBg
            // 
            this.ContentBg.BackColor = System.Drawing.Color.Transparent;
            this.ContentBg.BackgroundImage = global::ChattingCtrl.Properties.Resources.GrayRect;
            this.ContentBg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ContentBg.Controls.Add(this.Content);
            this.ContentBg.Location = new System.Drawing.Point(5, 80);
            this.ContentBg.Name = "ContentBg";
            this.ContentBg.Size = new System.Drawing.Size(315, 100);
            this.ContentBg.TabIndex = 8;
            // 
            // Content
            // 
            this.Content.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.Content.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.Content.Location = new System.Drawing.Point(12, 12);
            this.Content.Name = "Content";
            this.Content.Size = new System.Drawing.Size(292, 64);
            this.Content.TabIndex = 3;
            this.Content.Text = "一二三四五六七八九十一二三四五六七八九十一二三四五六七八九十一二三四五六七八九十一二三四五六七八九十一二三四五六七八九十";
            // 
            // TimeLabel
            // 
            this.TimeLabel.AutoSize = true;
            this.TimeLabel.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.TimeLabel.ForeColor = System.Drawing.Color.DarkBlue;
            this.TimeLabel.Location = new System.Drawing.Point(76, 49);
            this.TimeLabel.Name = "TimeLabel";
            this.TimeLabel.Size = new System.Drawing.Size(111, 17);
            this.TimeLabel.TabIndex = 7;
            this.TimeLabel.Text = "2015-4-19 12：12";
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.NameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(79)))), ((int)(((byte)(101)))));
            this.NameLabel.Location = new System.Drawing.Point(75, 16);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(50, 25);
            this.NameLabel.TabIndex = 6;
            this.NameLabel.Text = "程倩";
            // 
            // headerIconPicturebox1
            // 
            this.headerIconPicturebox1.BackColor = System.Drawing.Color.Transparent;
            this.headerIconPicturebox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.headerIconPicturebox1.Location = new System.Drawing.Point(7, 13);
            this.headerIconPicturebox1.Name = "headerIconPicturebox1";
            this.headerIconPicturebox1.Size = new System.Drawing.Size(60, 60);
            this.headerIconPicturebox1.TabIndex = 5;
            this.headerIconPicturebox1.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::ChattingCtrl.Properties.Resources.GrayLine;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(2, 194);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(320, 3);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // SuibiRect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.ContentBg);
            this.Controls.Add(this.TimeLabel);
            this.Controls.Add(this.NameLabel);
            this.Controls.Add(this.headerIconPicturebox1);
            this.Name = "SuibiRect";
            this.Size = new System.Drawing.Size(325, 218);
            this.ContentBg.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.headerIconPicturebox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel ContentBg;
        private System.Windows.Forms.Label Content;
        private System.Windows.Forms.Label TimeLabel;
        private System.Windows.Forms.Label NameLabel;
        private HeaderIconPicturebox headerIconPicturebox1;
        private System.Windows.Forms.PictureBox pictureBox1;

    }
}
