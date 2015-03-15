namespace WorkLogForm
{
    partial class InstantMessenger
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
            this.close_pictureBox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.close_pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // close_pictureBox
            // 
            this.close_pictureBox.BackColor = System.Drawing.Color.Transparent;
            this.close_pictureBox.BackgroundImage = global::WorkLogForm.Properties.Resources.关闭渐变;
            this.close_pictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.close_pictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.close_pictureBox.Location = new System.Drawing.Point(0, 1);
            this.close_pictureBox.Name = "close_pictureBox";
            this.close_pictureBox.Size = new System.Drawing.Size(28, 23);
            this.close_pictureBox.TabIndex = 171;
            this.close_pictureBox.TabStop = false;
            this.close_pictureBox.Click += new System.EventHandler(this.close_pictureBox_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(156, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 22);
            this.label1.TabIndex = 172;
            this.label1.Text = "联系人列表";
            // 
            // InstantMessenger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WorkLogForm.Properties.Resources.日志管理系统背景小;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(252, 493);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.close_pictureBox);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "InstantMessenger";
            this.Text = "InstantMessenger";
            this.Load += new System.EventHandler(this.InstantMessenger_Load);
            ((System.ComponentModel.ISupportInitialize)(this.close_pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox close_pictureBox;
        private System.Windows.Forms.Label label1;
    }
}