namespace WorkLogForm
{
    partial class login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(login));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timeroflabelmessage = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerOfLoad = new System.ComponentModel.BackgroundWorker();
            this.pictureBoxOfHeadIcon = new System.Windows.Forms.PictureBox();
            this.textBox1 = new ChattingCtrl.AlphaBlendTextBox();
            this.textBox2 = new ChattingCtrl.AlphaBlendTextBox();
            this.labelofMessage = new System.Windows.Forms.Label();
            this.pictureBoxofAutoLogin = new System.Windows.Forms.PictureBox();
            this.pictureBoxOfRememberPwd = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.close_pictureBox = new System.Windows.Forms.PictureBox();
            this.min_pictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOfHeadIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxofAutoLogin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOfRememberPwd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.close_pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.min_pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timeroflabelmessage
            // 
            this.timeroflabelmessage.Interval = 2000;
            this.timeroflabelmessage.Tick += new System.EventHandler(this.timeroflabelmessage_Tick);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // backgroundWorkerOfLoad
            // 
            this.backgroundWorkerOfLoad.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerOfLoad_DoWork);
            // 
            // pictureBoxOfHeadIcon
            // 
            this.pictureBoxOfHeadIcon.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxOfHeadIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBoxOfHeadIcon.Location = new System.Drawing.Point(84, 45);
            this.pictureBoxOfHeadIcon.Name = "pictureBoxOfHeadIcon";
            this.pictureBoxOfHeadIcon.Size = new System.Drawing.Size(114, 114);
            this.pictureBoxOfHeadIcon.TabIndex = 25;
            this.pictureBoxOfHeadIcon.TabStop = false;
            // 
            // textBox1
            // 
            this.textBox1.BackAlpha = 0;
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(224)))), ((int)(((byte)(200)))));
            this.textBox1.IsPasswordChar = false;
            this.textBox1.Location = new System.Drawing.Point(50, 212);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(176, 19);
            this.textBox1.TabIndex = 24;
            this.textBox1.Text = "请输入用户名";
            this.textBox1.Enter += new System.EventHandler(this.textBox1_Enter);
            this.textBox1.Leave += new System.EventHandler(this.textBox1_Leave);
            // 
            // textBox2
            // 
            this.textBox2.BackAlpha = 0;
            this.textBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(224)))), ((int)(((byte)(200)))));
            this.textBox2.IsPasswordChar = false;
            this.textBox2.Location = new System.Drawing.Point(48, 253);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(186, 19);
            this.textBox2.TabIndex = 23;
            this.textBox2.Text = "请输入密码";
            this.textBox2.WordWrap = false;
            this.textBox2.Enter += new System.EventHandler(this.textBox2_Enter);
            this.textBox2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox2_KeyDown);
            this.textBox2.Leave += new System.EventHandler(this.textBox2_Leave);
            // 
            // labelofMessage
            // 
            this.labelofMessage.AutoSize = true;
            this.labelofMessage.BackColor = System.Drawing.Color.Transparent;
            this.labelofMessage.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelofMessage.ForeColor = System.Drawing.Color.Brown;
            this.labelofMessage.Location = new System.Drawing.Point(82, 366);
            this.labelofMessage.Name = "labelofMessage";
            this.labelofMessage.Size = new System.Drawing.Size(68, 17);
            this.labelofMessage.TabIndex = 22;
            this.labelofMessage.Text = "密码不能为";
            this.labelofMessage.Visible = false;
            // 
            // pictureBoxofAutoLogin
            // 
            this.pictureBoxofAutoLogin.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxofAutoLogin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBoxofAutoLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxofAutoLogin.Location = new System.Drawing.Point(174, 297);
            this.pictureBoxofAutoLogin.Name = "pictureBoxofAutoLogin";
            this.pictureBoxofAutoLogin.Size = new System.Drawing.Size(12, 12);
            this.pictureBoxofAutoLogin.TabIndex = 20;
            this.pictureBoxofAutoLogin.TabStop = false;
            this.pictureBoxofAutoLogin.Click += new System.EventHandler(this.pictureBoxOfRememberPwd_Click);
            // 
            // pictureBoxOfRememberPwd
            // 
            this.pictureBoxOfRememberPwd.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxOfRememberPwd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBoxOfRememberPwd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxOfRememberPwd.Location = new System.Drawing.Point(46, 296);
            this.pictureBoxOfRememberPwd.Name = "pictureBoxOfRememberPwd";
            this.pictureBoxOfRememberPwd.Size = new System.Drawing.Size(12, 12);
            this.pictureBoxOfRememberPwd.TabIndex = 19;
            this.pictureBoxOfRememberPwd.TabStop = false;
            this.pictureBoxOfRememberPwd.Click += new System.EventHandler(this.pictureBoxOfRememberPwd_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.BackgroundImage = global::WorkLogForm.Properties.Resources.logo4535;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.Location = new System.Drawing.Point(4, 4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(35, 28);
            this.pictureBox2.TabIndex = 18;
            this.pictureBox2.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(187)))), ((int)(((byte)(249)))));
            this.label1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(110, 327);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 21);
            this.label1.TabIndex = 17;
            this.label1.Text = "登   录";
            this.label1.Click += new System.EventHandler(this.button1_Click);
            this.label1.MouseEnter += new System.EventHandler(this.pictureBox1_MouseEnter);
            this.label1.MouseLeave += new System.EventHandler(this.pictureBox1_MouseLeave);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = global::WorkLogForm.Properties.Resources.LoginButton;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Location = new System.Drawing.Point(40, 321);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(204, 37);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.button1_Click);
            this.pictureBox1.MouseEnter += new System.EventHandler(this.pictureBox1_MouseEnter);
            this.pictureBox1.MouseLeave += new System.EventHandler(this.pictureBox1_MouseLeave);
            // 
            // close_pictureBox
            // 
            this.close_pictureBox.BackColor = System.Drawing.Color.Transparent;
            this.close_pictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.close_pictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.close_pictureBox.Image = global::WorkLogForm.Properties.Resources.close;
            this.close_pictureBox.Location = new System.Drawing.Point(250, -1);
            this.close_pictureBox.Name = "close_pictureBox";
            this.close_pictureBox.Size = new System.Drawing.Size(31, 30);
            this.close_pictureBox.TabIndex = 6;
            this.close_pictureBox.TabStop = false;
            this.close_pictureBox.Click += new System.EventHandler(this.close_pictureBox_Click);
            this.close_pictureBox.MouseEnter += new System.EventHandler(this.close_pictureBox_MouseEnter);
            this.close_pictureBox.MouseLeave += new System.EventHandler(this.close_pictureBox_MouseLeave);
            // 
            // min_pictureBox
            // 
            this.min_pictureBox.BackColor = System.Drawing.Color.Transparent;
            this.min_pictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.min_pictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.min_pictureBox.Image = global::WorkLogForm.Properties.Resources.Min;
            this.min_pictureBox.Location = new System.Drawing.Point(219, 0);
            this.min_pictureBox.Name = "min_pictureBox";
            this.min_pictureBox.Size = new System.Drawing.Size(31, 30);
            this.min_pictureBox.TabIndex = 5;
            this.min_pictureBox.TabStop = false;
            this.min_pictureBox.Click += new System.EventHandler(this.min_pictureBox_Click);
            this.min_pictureBox.MouseEnter += new System.EventHandler(this.min_pictureBox_MouseEnter);
            this.min_pictureBox.MouseLeave += new System.EventHandler(this.min_pictureBox_MouseLeave);
            // 
            // login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(281, 394);
            this.Controls.Add(this.pictureBoxOfHeadIcon);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.labelofMessage);
            this.Controls.Add(this.pictureBoxofAutoLogin);
            this.Controls.Add(this.pictureBoxOfRememberPwd);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.close_pictureBox);
            this.Controls.Add(this.min_pictureBox);
            this.GradientTime = 1;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainPosition = new System.Drawing.Point(5, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "login";
            this.Opacity = 0D;
            this.ShowInTaskbar = true;
            this.SkinBack = global::WorkLogForm.Properties.Resources.LoginBg;
            this.SkinOpacity = 0;
            this.SkinShowInTaskbar = false;
            this.SkinSize = new System.Drawing.Size(290, 400);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "登录";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.login_Load);
            this.Shown += new System.EventHandler(this.login_Shown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.login_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.login_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOfHeadIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxofAutoLogin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOfRememberPwd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.close_pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.min_pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox close_pictureBox;
        private System.Windows.Forms.PictureBox min_pictureBox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBoxOfRememberPwd;
        private System.Windows.Forms.PictureBox pictureBoxofAutoLogin;
        private System.Windows.Forms.Label labelofMessage;
        private System.Windows.Forms.Timer timeroflabelmessage;
        private ChattingCtrl.AlphaBlendTextBox textBox2;
        private ChattingCtrl.AlphaBlendTextBox textBox1;
        private System.Windows.Forms.PictureBox pictureBoxOfHeadIcon;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.ComponentModel.BackgroundWorker backgroundWorkerOfLoad;
    }
}