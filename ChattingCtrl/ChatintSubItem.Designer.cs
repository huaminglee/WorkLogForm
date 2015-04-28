namespace ChattingCtrl
{
    partial class ChatintSubItem
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
            this.panelOfMessage = new System.Windows.Forms.Panel();
            this.labelOfMessage = new System.Windows.Forms.Label();
            this.headerIconPicturebox1 = new ChattingCtrl.HeaderIconPicturebox();
            this.panelOfMessage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.headerIconPicturebox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelOfMessage
            // 
            this.panelOfMessage.BackgroundImage = global::ChattingCtrl.Properties.Resources.ChattingMessageBg;
            this.panelOfMessage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelOfMessage.Controls.Add(this.labelOfMessage);
            this.panelOfMessage.Location = new System.Drawing.Point(160, 7);
            this.panelOfMessage.Name = "panelOfMessage";
            this.panelOfMessage.Size = new System.Drawing.Size(280, 44);
            this.panelOfMessage.TabIndex = 1;
            // 
            // labelOfMessage
            // 
            this.labelOfMessage.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.labelOfMessage.Location = new System.Drawing.Point(8, 5);
            this.labelOfMessage.Name = "labelOfMessage";
            this.labelOfMessage.Size = new System.Drawing.Size(265, 36);
            this.labelOfMessage.TabIndex = 0;
            this.labelOfMessage.Text = "测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字测试的文字";
            // 
            // headerIconPicturebox1
            // 
            this.headerIconPicturebox1.BackColor = System.Drawing.Color.Transparent;
            this.headerIconPicturebox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.headerIconPicturebox1.Location = new System.Drawing.Point(448, 6);
            this.headerIconPicturebox1.Name = "headerIconPicturebox1";
            this.headerIconPicturebox1.Size = new System.Drawing.Size(45, 45);
            this.headerIconPicturebox1.TabIndex = 0;
            this.headerIconPicturebox1.TabStop = false;
            // 
            // ChatintSubItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panelOfMessage);
            this.Controls.Add(this.headerIconPicturebox1);
            this.Name = "ChatintSubItem";
            this.Size = new System.Drawing.Size(500, 59);
            this.panelOfMessage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.headerIconPicturebox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private HeaderIconPicturebox headerIconPicturebox1;
        private System.Windows.Forms.Panel panelOfMessage;
        private System.Windows.Forms.Label labelOfMessage;
    }
}
