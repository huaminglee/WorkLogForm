using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WorkLogForm
{
    public partial class NewMessageWindow : Form
    {

        private Point formLocation;

        public Point FormLocation
        {
            get { return formLocation; }
            set { formLocation = value; }
        }
        public NewMessageWindow()
        {
            InitializeComponent();

        }

        private void close_pictureBox_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void NewMessageWindow_Load(object sender, EventArgs e)
        {
            if (this.formLocation != null)
            {
                this.Location = formLocation;
            }
        }
    }
}
