using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ChattingCtrl
{
    public partial class ChatintSubItem : UserControl
    {
        public ChatintSubItem()
        {
            InitializeComponent();
        }

        bool _isSayIsMe;
        public bool IsSayIngIsMe
        {
            get
            {
                return _isSayIsMe;
            }
            set
            {
                _isSayIsMe = value;
                if (value)//右方
                {
                    this.headerIconPicturebox1.Location = new Point(448, 6);
                    this.panelOfMessage.Location = new Point(this.Width - this.panelOfMessage.Width-this.headerIconPicturebox1.Width - 20,9);
                }
                else//左方
                {
                    this.headerIconPicturebox1.Location = new Point(7,6);
                    this.panelOfMessage.Location = new Point(60,6);
                }
            }
        }

        private string _Message;
        public string Message
        {
            get 
            {
                return _Message;
            }
            set 
            {
                _Message = value;
                if (value.Length < 21)
                {
                    this.labelOfMessage.Text = value;
                    this.labelOfMessage.Width = value.Length * 15;
                    this.panelOfMessage.Height = 45;
                    this.labelOfMessage.Location = new Point(this.labelOfMessage.Location.X, this.labelOfMessage.Location.Y + 10);
                    this.panelOfMessage.Width = this.labelOfMessage.Width + 15;
                }
                else
                {
                    if (value.Length < 42)
                        this.labelOfMessage.Text = value;
                    else
                    {
                        this.labelOfMessage.Text = value;
                        int labelmessageHeight = (value.Length / 21 + 1) * 16;
                        this.labelOfMessage.Height = labelmessageHeight;
                        this.panelOfMessage.Height = labelmessageHeight + 15;
                        this.Height = labelmessageHeight + 30;
                    }
                }
            }   
        }
        
        int _headerIconId;
        public int HeaderId 
        {
            get {
                return _headerIconId;
            }
            set 
            {
                this._headerIconId = value;
                this.headerIconPicturebox1.UserId = value;
            }
        }


    }
}
