namespace WorkLogForm
{
    partial class ChatWindows
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChatWindows));
            this.close_pictureBox = new System.Windows.Forms.PictureBox();
            this.min_pictureBox = new System.Windows.Forms.PictureBox();
            this.labelOfReceiveUser = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.chattingPanel1 = new ChattingCtrl.ChattingPanel();
            this.headerIconPicturebox1 = new ChattingCtrl.HeaderIconPicturebox();
            ((System.ComponentModel.ISupportInitialize)(this.close_pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.min_pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.headerIconPicturebox1)).BeginInit();
            this.SuspendLayout();
            // 
            // close_pictureBox
            // 
            this.close_pictureBox.BackColor = System.Drawing.Color.Transparent;
            this.close_pictureBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("close_pictureBox.BackgroundImage")));
            this.close_pictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.close_pictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.close_pictureBox.Location = new System.Drawing.Point(501, 2);
            this.close_pictureBox.Name = "close_pictureBox";
            this.close_pictureBox.Size = new System.Drawing.Size(28, 23);
            this.close_pictureBox.TabIndex = 5;
            this.close_pictureBox.TabStop = false;
            this.close_pictureBox.Click += new System.EventHandler(this.close_pictureBox_Click);
            // 
            // min_pictureBox
            // 
            this.min_pictureBox.BackColor = System.Drawing.Color.Transparent;
            this.min_pictureBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("min_pictureBox.BackgroundImage")));
            this.min_pictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.min_pictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.min_pictureBox.Location = new System.Drawing.Point(473, 3);
            this.min_pictureBox.Name = "min_pictureBox";
            this.min_pictureBox.Size = new System.Drawing.Size(28, 23);
            this.min_pictureBox.TabIndex = 4;
            this.min_pictureBox.TabStop = false;
            this.min_pictureBox.Click += new System.EventHandler(this.min_pictureBox_Click);
            // 
            // labelOfReceiveUser
            // 
            this.labelOfReceiveUser.AutoSize = true;
            this.labelOfReceiveUser.BackColor = System.Drawing.Color.Transparent;
            this.labelOfReceiveUser.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelOfReceiveUser.Location = new System.Drawing.Point(90, 15);
            this.labelOfReceiveUser.Name = "labelOfReceiveUser";
            this.labelOfReceiveUser.Size = new System.Drawing.Size(55, 21);
            this.labelOfReceiveUser.TabIndex = 6;
            this.labelOfReceiveUser.Text = "label1";
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(3, 400);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(522, 72);
            this.textBox1.TabIndex = 8;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(365, 473);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(160, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "发送";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 200;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // chattingPanel1
            // 
            this.chattingPanel1.BackColor = System.Drawing.Color.Transparent;
            this.chattingPanel1.Location = new System.Drawing.Point(-1, 81);
            this.chattingPanel1.Name = "chattingPanel1";
            this.chattingPanel1.Size = new System.Drawing.Size(530, 280);
            this.chattingPanel1.TabIndex = 14;
            this.chattingPanel1.SeemoreLabelClicked += new ChattingCtrl.ChattingPanel.SeeMorelabelClickHandle(this.chattingPanel1_SeemoreLabelClicked);
            // 
            // headerIconPicturebox1
            // 
            this.headerIconPicturebox1.BackColor = System.Drawing.Color.Transparent;
            this.headerIconPicturebox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.headerIconPicturebox1.Location = new System.Drawing.Point(23, 10);
            this.headerIconPicturebox1.Name = "headerIconPicturebox1";
            this.headerIconPicturebox1.Size = new System.Drawing.Size(60, 60);
            this.headerIconPicturebox1.TabIndex = 13;
            this.headerIconPicturebox1.TabStop = false;
            // 
            // ChatWindows
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WorkLogForm.Properties.Resources.ChattingBg;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(530, 500);
            this.ControlBox = false;
            this.Controls.Add(this.chattingPanel1);
            this.Controls.Add(this.headerIconPicturebox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.labelOfReceiveUser);
            this.Controls.Add(this.close_pictureBox);
            this.Controls.Add(this.min_pictureBox);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ChatWindows";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "聊天窗口";
            this.Load += new System.EventHandler(this.ChatWindows_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Leave_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Leave_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.close_pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.min_pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.headerIconPicturebox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox close_pictureBox;
        private System.Windows.Forms.PictureBox min_pictureBox;
        private System.Windows.Forms.Label labelOfReceiveUser;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer timer1;
        private ChattingCtrl.HeaderIconPicturebox headerIconPicturebox1;
        private ChattingCtrl.ChattingPanel chattingPanel1;
       
    }
}