using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WorkLogForm.WindowUiClass;
using ClassLibrary;
using WorkLogForm.CommonClass;
using WorkLogForm.Service;
using System.Collections;

namespace WorkLogForm
{
    public partial class ViewBusiness : Form
    {
        public  Business business=new Business();
        private BaseService baseService=new BaseService();
        public ViewBusiness()
        {
            InitializeComponent();
            creatWindow.SetFormRoundRectRgn(this, 15);
            creatWindow.SetFormShadow(this);
        }

        private void ViewBusiness_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            IList businessEmployee = getEmpByBus(business);
            listView8.Items.Clear();
            foreach (BusinessEmployee be in businessEmployee)
            {
                ListViewItem item = new ListViewItem();
                item.Text = be.EmployeeId.KuName;
                item.SubItems.Add(be.EmployeeId.Kdid.KdName.Trim());
                item.Tag = be;
                listView8.Items.Add(item);
            }            
        }

        private IList getUserByDept(WkTDept dept)//获取登陆人所在部门
        {
            string queryUser = "from WkTUser u where u.Kdid=" + dept.Id;
            return baseService.loadEntityList(queryUser);
        }

        ///<summary>获取某次出差的人员名单</summary>         
        private IList getEmpByBus(Business b)
        {
            string queryUser = "from BusinessEmployee be where be.BusinessId=" + b.Id + " and be.State=" + (int)BusinessEmployee.stateEnum.Normal;
            return baseService.loadEntityList(queryUser);
        }
    }
}
