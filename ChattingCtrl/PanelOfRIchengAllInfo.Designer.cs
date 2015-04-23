namespace ChattingCtrl
{
    partial class PanelOfRIchengAllInfo
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ContentBg = new System.Windows.Forms.Panel();
            this.Content = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ContentBg.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Gray;
            this.label1.Location = new System.Drawing.Point(-4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "详细内容";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Gray;
            this.label2.Location = new System.Drawing.Point(329, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "X";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            this.label2.MouseEnter += new System.EventHandler(this.label2_MouseEnter);
            this.label2.MouseLeave += new System.EventHandler(this.label2_MouseLeave);
            // 
            // ContentBg
            // 
            this.ContentBg.BackgroundImage = global::ChattingCtrl.Properties.Resources.GrayRect;
            this.ContentBg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ContentBg.Controls.Add(this.Content);
            this.ContentBg.Location = new System.Drawing.Point(15, 33);
            this.ContentBg.Name = "ContentBg";
            this.ContentBg.Size = new System.Drawing.Size(315, 143);
            this.ContentBg.TabIndex = 12;
            // 
            // Content
            // 
            this.Content.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Content.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.Content.Location = new System.Drawing.Point(11, 15);
            this.Content.Name = "Content";
            this.Content.Size = new System.Drawing.Size(292, 105);
            this.Content.TabIndex = 1;
            this.Content.Text = "测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测" +
    "试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测" +
    "试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.SkyBlue;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(350, 27);
            this.panel1.TabIndex = 13;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Gainsboro;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 199);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(350, 16);
            this.panel2.TabIndex = 14;
            // 
            // PanelOfRIchengAllInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ContentBg);
            this.Controls.Add(this.panel2);
            this.DoubleBuffered = true;
            this.Name = "PanelOfRIchengAllInfo";
            this.Size = new System.Drawing.Size(350, 215);
            this.ContentBg.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel ContentBg;
        private System.Windows.Forms.Label Content;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}
