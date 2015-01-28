using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using ClassLibrary;
using WorkLogForm.Service;
namespace WorkLogForm
{
    public partial class redoForm : Form
    {
        private BaseService baseService = new BaseService();
        private IList empList;
        public Business business;
        public redoForm()
        {
            InitializeComponent();
        }

        private void redoForm_Load(object sender, EventArgs e)
        {
            string query = "from WkTDept";
            IList depList = baseService.loadEntityList(query);
            WkTDept dep = new WkTDept();
            depList.Remove(depList[0]);
            comboBox1.DataSource = depList;
            comboBox1.DisplayMember = "KdName";
            comboBox1.ValueMember = "Itself";
            empList = new List<WkTUser>();

            int i=1;
            foreach (BusinessEmployee be in business.BusinessEmployee)
            {
                ListViewItem item = new ListViewItem();
                item.Text =i.ToString();
                item.SubItems.Add(be.EmployeeId.KuName);
                item.SubItems.Add(be.EmployeeId.Kdid.KdName);
                switch (be.PassExam)
                { 
                    case(int) BusinessEmployee.ExamState.redo:
                        item.SubItems.Add("退回");
                        break;
                    case (int)BusinessEmployee.ExamState.pass:
                        item.SubItems.Add("通过审核");
                        break;
                }

                i++;
                item.Tag = be;
                listView1.Items.Add(item);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string queryName = "from WkTUser user where user.Kdid.KdName ='" + ((WkTDept)comboBox1.SelectedValue).KdName.Trim() + "'";
            IList Employee = baseService.loadEntityList(queryName);
            if (Employee.Count == 0 && Employee == null)
                listView1.Items.Clear();
            else
            {
                listView1.Items.Clear();
                foreach (WkTUser u in Employee)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = u.KuName;
                    item.Tag = u;
                    listView1.Items.Add(item);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WkTUser u=(WkTUser) this.listView1.SelectedItems[0].Tag;
            empList.Add(u);
            ListViewItem item = new ListViewItem();
            item.Text = u.KuName;
            item.SubItems.Add(u.Kdid.KdName);
            item.Tag = u;
            listView2.Items.Add(item);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WkTUser u = (WkTUser)this.listView2.SelectedItems[0].Tag;
            empList.Remove(u);
            listView2.Items.Remove(this.listView2.SelectedItems[0]);
        }
    }
}
