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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.close_pictureBox = new System.Windows.Forms.PictureBox();
            this.min_pictureBox = new System.Windows.Forms.PictureBox();
            this.rem_checkBox = new System.Windows.Forms.CheckBox();
            this.auto_checkBox = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.close_pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.min_pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(238)))), ((int)(((byte)(243)))));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.textBox1.Location = new System.Drawing.Point(112, 105);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(128, 14);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "输入用户名";
            this.textBox1.Enter += new System.EventHandler(this.textBox1_Enter);
            this.textBox1.Leave += new System.EventHandler(this.textBox1_Leave);
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(238)))), ((int)(((byte)(243)))));
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.textBox2.Location = new System.Drawing.Point(112, 135);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(128, 14);
            this.textBox2.TabIndex = 3;
            this.textBox2.Text = "输入密码";
            this.textBox2.Enter += new System.EventHandler(this.textBox2_Enter);
            this.textBox2.Leave += new System.EventHandler(this.textBox2_Leave);
            // 
            // close_pictureBox
            // 
            this.close_pictureBox.BackColor = System.Drawing.Color.Transparent;
            this.close_pictureBox.BackgroundImage = global::WorkLogForm.Properties.Resources.关闭1;
            this.close_pictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.close_pictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.close_pictureBox.Location = new System.Drawing.Point(335, 0);
            this.close_pictureBox.Name = "close_pictureBox";
            this.close_pictureBox.Size = new System.Drawing.Size(28, 23);
            this.close_pictureBox.TabIndex = 6;
            this.close_pictureBox.TabStop = false;
            this.close_pictureBox.Click += new System.EventHandler(this.close_pictureBox_Click);
            this.close_pictureBox.MouseEnter += new System.EventHandler(this.close_pictureBox_MouseEnter);
            this.close_pictureBox.MouseLeave += new System.EventHandler(this.close_pictureBox_MouseLeave);
            // 
            // min_pictureBox
            // 
            this.min_pictureBox.BackColor = System.Drawing.Color.Transparent;
            this.min_pictureBox.BackgroundImage = global::WorkLogForm.Properties.Resources.最小化2;
            this.min_pictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.min_pictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.min_pictureBox.Location = new System.Drawing.Point(307, 0);
            this.min_pictureBox.Name = "min_pictureBox";
            this.min_pictureBox.Size = new System.Drawing.Size(28, 23);
            this.min_pictureBox.TabIndex = 5;
            this.min_pictureBox.TabStop = false;
            this.min_pictureBox.Click += new System.EventHandler(this.min_pictureBox_Click);
            this.min_pictureBox.MouseEnter += new System.EventHandler(this.min_pictureBox_MouseEnter);
            this.min_pictureBox.MouseLeave += new System.EventHandler(this.min_pictureBox_MouseLeave);
            // 
            // rem_checkBox
            // 
            this.rem_checkBox.AutoSize = true;
            this.rem_checkBox.BackColor = System.Drawing.Color.Transparent;
            this.rem_checkBox.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.rem_checkBox.Location = new System.Drawing.Point(112, 170);
            this.rem_checkBox.Name = "rem_checkBox";
            this.rem_checkBox.Size = new System.Drawing.Size(72, 16);
            this.rem_checkBox.TabIndex = 7;
            this.rem_checkBox.Text = "记住密码";
            this.rem_checkBox.UseVisualStyleBackColor = false;
            // 
            // auto_checkBox
            // 
            this.auto_checkBox.AutoSize = true;
            this.auto_checkBox.BackColor = System.Drawing.Color.Transparent;
            this.auto_checkBox.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.auto_checkBox.Location = new System.Drawing.Point(183, 170);
            this.auto_checkBox.Name = "auto_checkBox";
            this.auto_checkBox.Size = new System.Drawing.Size(72, 16);
            this.auto_checkBox.TabIndex = 8;
            this.auto_checkBox.Text = "自动登录";
            this.auto_checkBox.UseVisualStyleBackColor = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::WorkLogForm.Properties.Resources.loginButton1;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Location = new System.Drawing.Point(112, 203);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(136, 30);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.button1_Click);
            this.pictureBox1.MouseEnter += new System.EventHandler(this.pictureBox1_MouseEnter);
            this.pictureBox1.MouseLeave += new System.EventHandler(this.pictureBox1_MouseLeave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("幼圆", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(140, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 14);
            this.label2.TabIndex = 13;
            this.label2.Text = "工作小秘书";
            // 
            // timer1
            // 
            this.timer1.Interval = 50;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::WorkLogForm.Properties.Resources.login14;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(363, 300);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.auto_checkBox);
            this.Controls.Add(this.rem_checkBox);
            this.Controls.Add(this.close_pictureBox);
            this.Controls.Add(this.min_pictureBox);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "登录";
            this.Load += new System.EventHandler(this.login_Load);
            this.Shown += new System.EventHandler(this.login_Shown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.login_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.login_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.close_pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.min_pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.PictureBox close_pictureBox;
        private System.Windows.Forms.PictureBox min_pictureBox;
        private System.Windows.Forms.CheckBox rem_checkBox;
        private System.Windows.Forms.CheckBox auto_checkBox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer timer1;
    }
}