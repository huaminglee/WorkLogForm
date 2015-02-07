using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClassLibrary;
using WorkLogForm.Service;
using System.Collections;
using WorkLogForm.WindowUiClass;

namespace WorkLogForm
{
    public partial class BusinessRedo : Form
    {
        private Business business;
        private BaseService baseService=new BaseService();
        public BusinessRedo()
        {
            InitializeComponent();
            creatWindow.SetFormRoundRectRgn(this, 15);
            creatWindow.SetFormShadow(this);
        }

        private void BusinessRedo_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            business = (Business)this.Tag;
            IList<BusinessEmployee> beList=business.BusinessEmployee;
            foreach (BusinessEmployee be in beList)
            {
                ListViewItem item = new ListViewItem();
                item.Text = be.EmployeeId.KuName;
                item.SubItems.Add(be.EmployeeId.Kdid.KdName.Trim());
                item.Tag = be;
                listView1.Items.Add(item);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.listView1.CheckedItems.Count != 0 && this.listView1.CheckedItems != null)
            {
                BusinessEmployee be;
                foreach (ListViewItem item in this.listView1.CheckedItems)
                {
                    be = (BusinessEmployee)item.Tag;
                    string query1 = "update LOG_T_BUSINESSEMPLOYEE set PASSEXAM=" + (int)BusinessEmployee.ExamState.redo + " where Id=" + be.Id;
                    baseService.ExecuteSQL(query1);
                }
                string query2 = "update LOG_T_BUSINESS set PASSEXAM=" + (int)Business.ExamState.redo + " where Id=" + business.Id;
                baseService.ExecuteSQL(query2);
                this.DialogResult = DialogResult.OK;
                MessageBox.Show("退回成功");
            }
            else
            {
                MessageBox.Show("没有选择退回员工");
            }
        }
    }
}
