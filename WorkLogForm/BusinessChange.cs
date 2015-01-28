using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using WorkLogForm.Service;
using ClassLibrary;

namespace WorkLogForm
{
    public partial class BusinessChange : Form
    {
        private BaseService baseService=new BaseService();
        private ArrayList beList;
        private ArrayList origin;
        private WkTDept dept;
        private WkTUser user;
        private Business business;
        private int count=0;


        public BusinessChange()
        {
            InitializeComponent();
            
            
        }

        private void BusinessChange_Load(object sender, EventArgs e)
        {
            beList = (ArrayList)this.Tag;
            origin=(ArrayList)beList.Clone();
            user = ((BusinessEmployee)beList[0]).EmployeeId;
            dept = user.Kdid;
            business = ((BusinessEmployee)beList[0]).BusinessId;
            

            foreach (BusinessEmployee be in beList)
            {
                ListViewItem item = new ListViewItem();
                item.Text = be.EmployeeId.KuName;
                item.Tag = be;
                listView1.Items.Add(item);
            }

            IList udList=getUserByDept(dept);
            foreach (WkTUser ud in udList)
            {
                ListViewItem item = new ListViewItem();
                item.Text = ud.KuName;
                item.Tag = ud;
                listView2.Items.Add(item);
            }
        }

       

        private void button1_Click(object sender, EventArgs e)//移除
        {
            if (listView1.SelectedItems != null && listView1.SelectedItems.Count != 0)
            {
                BusinessEmployee be = (BusinessEmployee)listView1.SelectedItems[0].Tag;
                listView1.Items.Remove(listView1.SelectedItems[0]);
                beList.Remove(be);
                count--;
            }
        }
        private void button2_Click(object sender, EventArgs e)//添加
        {
            if (listView2.SelectedItems != null && listView2.SelectedItems.Count != 0)
            {
                BusinessEmployee be = new BusinessEmployee();
                be.BusinessId = business;
                be.EmployeeId = ((WkTUser)listView2.SelectedItems[0].Tag);
                be.PassExam = (int)BusinessEmployee.ExamState.waiting;
                beList.Add(be);

                ListViewItem item = new ListViewItem();
                item.Text = be.EmployeeId.KuName;
                item.Tag = be;
                listView1.Items.Add(item);
                count++;
            }

        }

        private IList getUserByDept(WkTDept dept)//获取登陆人所在部门
        {
            string queryUser = "from WkTUser u where u.Kdid=" + dept.Id;
            return baseService.loadEntityList(queryUser);
        }

        private void button4_Click(object sender, EventArgs e)//取消
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)//确认
        {
            foreach (BusinessEmployee be1 in origin)
            {
                if (!beList.Contains(be1))
                {
                    //baseService.deleteEntity(be1);
                    business.BusinessEmployee.Remove(be1);
                }
            }

            foreach (BusinessEmployee be2 in beList)
            {
                if (!origin.Contains(be2))
                {
                    business.BusinessEmployee.Add(be2);
                }
            }
            
            int waitNow=business.WaitingNum+count;
            if (count!= 0)
            {
                business.WaitingNum = waitNow;
            }
            if (waitNow == 0)
            {
                business.PassExam =(int) Business.ExamState.pass;
            }
            baseService.SaveOrUpdateEntity(business);
            
            this.DialogResult=DialogResult.OK;
        }

       
    }
}
