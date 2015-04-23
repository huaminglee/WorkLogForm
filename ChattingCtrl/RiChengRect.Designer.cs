namespace ChattingCtrl
{
    partial class RiChengRect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RiChengRect));
            this.SubTitle = new System.Windows.Forms.Label();
            this.ArrangePerson = new System.Windows.Forms.Label();
            this.Dotime = new System.Windows.Forms.Label();
            this.ContentBg = new System.Windows.Forms.Panel();
            this.Content = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.headerIconPicturebox2 = new ChattingCtrl.HeaderIconPicturebox();
            this.ContentBg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.headerIconPicturebox2)).BeginInit();
            this.SuspendLayout();
            // 
            // SubTitle
            // 
            this.SubTitle.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SubTitle.Location = new System.Drawing.Point(86, 23);
            this.SubTitle.Name = "SubTitle";
            this.SubTitle.Size = new System.Drawing.Size(200, 23);
            this.SubTitle.TabIndex = 9;
            this.SubTitle.Text = "开会开会开会";
            // 
            // ArrangePerson
            // 
            this.ArrangePerson.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.ArrangePerson.Location = new System.Drawing.Point(89, 58);
            this.ArrangePerson.Name = "ArrangePerson";
            this.ArrangePerson.Size = new System.Drawing.Size(94, 21);
            this.ArrangePerson.TabIndex = 8;
            this.ArrangePerson.Text = "安排人：张彦忠";
            // 
            // Dotime
            // 
            this.Dotime.AutoSize = true;
            this.Dotime.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Dotime.ForeColor = System.Drawing.Color.Red;
            this.Dotime.Location = new System.Drawing.Point(181, 58);
            this.Dotime.Name = "Dotime";
            this.Dotime.Size = new System.Drawing.Size(118, 17);
            this.Dotime.TabIndex = 7;
            this.Dotime.Text = "2015-12-31 12：12";
            // 
            // ContentBg
            // 
            this.ContentBg.BackgroundImage = global::ChattingCtrl.Properties.Resources.GrayRect;
            this.ContentBg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ContentBg.Controls.Add(this.Content);
            this.ContentBg.Location = new System.Drawing.Point(8, 82);
            this.ContentBg.Name = "ContentBg";
            this.ContentBg.Size = new System.Drawing.Size(315, 143);
            this.ContentBg.TabIndex = 11;
            this.ContentBg.Click += new System.EventHandler(this.ContentBg_Click);
            this.ContentBg.MouseEnter += new System.EventHandler(this.ContentBg_MouseEnter);
            this.ContentBg.MouseLeave += new System.EventHandler(this.ContentBg_MouseLeave);
            // 
            // Content
            // 
            this.Content.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Content.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.Content.Location = new System.Drawing.Point(11, 15);
            this.Content.Name = "Content";
            this.Content.Size = new System.Drawing.Size(292, 105);
            this.Content.TabIndex = 1;
            this.Content.Text = resources.GetString("Content.Text");
            this.Content.Click += new System.EventHandler(this.ContentBg_Click);
            this.Content.MouseEnter += new System.EventHandler(this.ContentBg_MouseEnter);
            this.Content.MouseLeave += new System.EventHandler(this.ContentBg_MouseLeave);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::ChattingCtrl.Properties.Resources.GrayLine;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(12, 240);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(300, 3);
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // headerIconPicturebox2
            // 
            this.headerIconPicturebox2.BackColor = System.Drawing.Color.Transparent;
            this.headerIconPicturebox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.headerIconPicturebox2.Location = new System.Drawing.Point(9, 19);
            this.headerIconPicturebox2.Name = "headerIconPicturebox2";
            this.headerIconPicturebox2.Size = new System.Drawing.Size(60, 60);
            this.headerIconPicturebox2.TabIndex = 10;
            this.headerIconPicturebox2.TabStop = false;
            // 
            // RiChengRect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.headerIconPicturebox2);
            this.Controls.Add(this.SubTitle);
            this.Controls.Add(this.ArrangePerson);
            this.Controls.Add(this.Dotime);
            this.Controls.Add(this.ContentBg);
            this.Name = "RiChengRect";
            this.Size = new System.Drawing.Size(325, 274);
            this.ContentBg.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.headerIconPicturebox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HeaderIconPicturebox headerIconPicturebox2;
        private System.Windows.Forms.Label SubTitle;
        private System.Windows.Forms.Label ArrangePerson;
        private System.Windows.Forms.Label Dotime;
        private System.Windows.Forms.Label Content;
        private System.Windows.Forms.Panel ContentBg;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
