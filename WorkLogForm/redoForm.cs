using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using WorkLogForm.WindowUiClass;
using ClassLibrary;
using WorkLogForm.CommonClass;
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
            creatWindow.SetFormRoundRectRgn(this, 15);
            creatWindow.SetFormShadow(this);

        }

        private void redoForm_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
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
                item.SubItems.Add(be.EmployeeId.Kdid.KdName.Trim());
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
                listView3.Items.Add(item);
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
            item.SubItems.Add(u.Kdid.KdName.Trim());
            item.Tag = u;
            listView2.Items.Add(item);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WkTUser u = (WkTUser)this.listView2.SelectedItems[0].Tag;
            empList.Remove(u);
            listView2.Items.Remove(this.listView2.SelectedItems[0]);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listView2.Items.Count == 0 && listView2.Items == null)
            {
                MessageBox.Show("未作人员修改");
            }
            else
            {
                WkTUser user = new WkTUser();
                foreach (ListViewItem item in listView2.Items)
                {
                    user = (WkTUser)item.Tag;
                    BusinessEmployee be = new BusinessEmployee();
                    be.EmployeeId=user;
                    be.BusinessId = business;
                    be.PassExam = (int)BusinessEmployee.ExamState.pass;
                    business.BusinessEmployee.Add(be);
                }

                foreach (ListViewItem item in listView3.Items)
                {

                    BusinessEmployee be = (BusinessEmployee)item.Tag; ;
                    if (be.PassExam == (int)BusinessEmployee.ExamState.redo)
                    {
                        business.BusinessEmployee.Remove(be);
                    }
                }
                business.PassExam = (int)Business.ExamState.pass;
                baseService.SaveOrUpdateEntity(business);

                #region 服务器通信
                KjqbService.Service1Client ser = new KjqbService.Service1Client();

                KjqbService.BusinessService bs = new KjqbService.BusinessService();
                bs.BusinessID = business.Id;
                bs.ReceiveID = business.Boss.Id;
                bs.Type = 0;
                bs.TimeStamp = DateTime.Now.Ticks;
                ser.SaveInBusinessListInService(bs);

                #endregion

                MessageBox.Show("成功提交");
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
