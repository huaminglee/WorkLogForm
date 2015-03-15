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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChatWindows));
            this.close_pictureBox = new System.Windows.Forms.PictureBox();
            this.min_pictureBox = new System.Windows.Forms.PictureBox();
            this.labelOfReceiveUser = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.close_pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.min_pictureBox)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // close_pictureBox
            // 
            this.close_pictureBox.BackColor = System.Drawing.Color.Transparent;
            this.close_pictureBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("close_pictureBox.BackgroundImage")));
            this.close_pictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.close_pictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.close_pictureBox.Location = new System.Drawing.Point(562, 3);
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
            this.min_pictureBox.Location = new System.Drawing.Point(534, 3);
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
            this.labelOfReceiveUser.Location = new System.Drawing.Point(9, 4);
            this.labelOfReceiveUser.Name = "labelOfReceiveUser";
            this.labelOfReceiveUser.Size = new System.Drawing.Size(59, 22);
            this.labelOfReceiveUser.TabIndex = 6;
            this.labelOfReceiveUser.Text = "label1";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.flowLayoutPanel1.Controls.Add(this.panel2);
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(13, 42);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(569, 263);
            this.flowLayoutPanel1.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(3, 106);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(534, 97);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(534, 97);
            this.panel2.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(10, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 22);
            this.label2.TabIndex = 0;
            this.label2.Text = "马仕：";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(12, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(496, 46);
            this.label3.TabIndex = 1;
            this.label3.Text = "测试文字测试文字测试文字测试文字测试文字测试文字测试文字测试文字测试文字测试文字测试文字测试文字测试文字测试文字测试文字";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 321);
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
            // ChatWindows
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WorkLogForm.Properties.Resources.日志管理系统背景小;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(594, 449);
            this.ControlBox = false;
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
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox close_pictureBox;
        private System.Windows.Forms.PictureBox min_pictureBox;
        private System.Windows.Forms.Label labelOfReceiveUser;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
    }
}