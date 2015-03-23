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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.close_pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.min_pictureBox)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // close_pictureBox
            // 
            this.close_pictureBox.BackColor = System.Drawing.Color.Transparent;
            this.close_pictureBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("close_pictureBox.BackgroundImage")));
            this.close_pictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.close_pictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.close_pictureBox.Location = new System.Drawing.Point(3, 2);
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
            this.min_pictureBox.Location = new System.Drawing.Point(32, 2);
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
            this.labelOfReceiveUser.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelOfReceiveUser.Location = new System.Drawing.Point(84, 3);
            this.labelOfReceiveUser.Name = "labelOfReceiveUser";
            this.labelOfReceiveUser.Size = new System.Drawing.Size(59, 22);
            this.labelOfReceiveUser.TabIndex = 6;
            this.labelOfReceiveUser.Text = "label1";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(11, 44);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(569, 259);
            this.flowLayoutPanel1.TabIndex = 7;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(11, 321);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(570, 83);
            this.textBox1.TabIndex = 8;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(422, 410);
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
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel1.Location = new System.Drawing.Point(475, 306);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(101, 12);
            this.linkLabel1.TabIndex = 11;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "点击查看聊天记录";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.linkLabel2);
            this.panel1.Controls.Add(this.flowLayoutPanel2);
            this.panel1.Location = new System.Drawing.Point(592, 38);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(334, 393);
            this.panel1.TabIndex = 12;
            this.panel1.Visible = false;
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Location = new System.Drawing.Point(140, 374);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(41, 12);
            this.linkLabel2.TabIndex = 10;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "往上翻";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoScroll = true;
            this.flowLayoutPanel2.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.BottomUp;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 4);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(324, 360);
            this.flowLayoutPanel2.TabIndex = 9;
            this.flowLayoutPanel2.WrapContents = false;
            // 
            // ChatWindows
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WorkLogForm.Properties.Resources.日志管理系统背景小;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(592, 449);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.flowLayoutPanel1);
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
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox close_pictureBox;
        private System.Windows.Forms.PictureBox min_pictureBox;
        private System.Windows.Forms.Label labelOfReceiveUser;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.LinkLabel linkLabel2;
    }
}