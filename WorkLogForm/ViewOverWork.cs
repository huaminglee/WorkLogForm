using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClassLibrary;

namespace WorkLogForm
{
    public partial class ViewOverWork : Form
    {
        public ClassLibrary.WorkOverTime wkot;
        public ViewOverWork()
        {
            InitializeComponent();
        }

        private void ViewOverWork_Load(object sender, EventArgs e)
        {
            init();
            
        }
        public void init()
        {
            listView1.Items.Clear();
            foreach (WkTUser u in wkot.WorkManId)
            {
                ListViewItem item = new ListViewItem();
                item.Text = u.KuName;
                item.Tag = u;
                listView1.Items.Add(item);
            }
        }
    }
}
