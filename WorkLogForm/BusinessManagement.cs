
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WorkLogForm.WindowUiClass;
using NHibernate;
using ClassLibrary;
using WorkLogForm.Service;
using System.Collections;
using WorkLogForm.CommonClass;

namespace WorkLogForm
{
    public partial class BusinessManagement : Form
    {
        private BaseService baseService=new BaseService();
        private Business selectedBusiness;

        private WkTUser user;
        public WkTUser User
        {
            get { return user; }
            set { user = value; }
        }

        private WkTDept dept;
        public WkTDept Dept
        {
            get { return dept; }
            set { dept = value; }
        }

        public BusinessManagement()
        {
            InitializeComponent();
            initialWindow();
        }
         private void initialWindow()
        {
            creatWindow.SetFormRoundRectRgn(this, 15);
            creatWindow.SetFormShadow(this);
        }
   

        #region 最小化关闭按钮
        private void min_pictureBox_MouseEnter(object sender, EventArgs e)
        {
            min_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.最小化_副本;
        }
        private void min_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            min_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.最小化渐变;
        }
        private void min_pictureBox_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void close_pictureBox_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void close_pictureBox_MouseEnter(object sender, EventArgs e)
        {
            close_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.关闭渐变_副本; 
        }
        private void close_pictureBox_MouseLeave(object sender, EventArgs e)
        {
            close_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.关闭渐变;
        }
        #endregion


        #region 窗体移动代码
        private int x_point, y_point;
        private void BusinessManagement_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.x_point = e.X;
                this.y_point = e.Y;
            }
        }

        private void BusinessManagement_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && this.Location.Y > 0)
            {
                Top = MousePosition.Y - y_point;
                Left = MousePosition.X - x_point;
            }
            else if (e.Button == MouseButtons.Left && e.Y > this.y_point)
            {
                Top = MousePosition.Y - y_point;
                Left = MousePosition.X - x_point;
            }
        }
        
       
        #endregion
      
        private void button17_Click(object sender, EventArgs e)
        {
            Business buss = new Business();
            
            
            buss.Ku_Id = User;
            //buss.BusinessId = (WkTUser)listView1.SelectedItems[0].Tag;
            buss.StartTime = dateTimePicker1.Value.Date.Ticks;
            buss.EndTime = dateTimePicker2.Value.Date.Ticks;
            buss.BusinessDestination = textBox4.Text;
            buss.BusinessNote = textBox3.Text;
            buss.BusinessReason = textBox5.Text;
            buss.PassExam = (int)Business.ExamState.waiting;
            buss.WaitingNum = listView1.Items.Count;
            baseService.SaveOrUpdateEntity(buss);
            Business buss1 =(Business) (getBussItself(buss)[0]);

            foreach (ListViewItem row in listView1.Items)
            {
                BusinessEmployee be = new BusinessEmployee();
                WkTUser employee = new WkTUser();
                employee = (WkTUser)row.Tag;
                be.BusinessId = buss1;
                be.EmployeeId = employee;
                be.PassExam =(int) BusinessEmployee.ExamState.waiting;
                baseService.SaveOrUpdateEntity(be);
            }
            

            MessageBox.Show("添加成功！");
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            listView4.Items.Clear();
            listView1.Items.Clear();
           
        }

        private void BusinessManagement_Load(object sender, EventArgs e)
        {
            Dept = User.Kdid;
            initTabPage1();
        }

        //private IList getBossDept()//获取院领导部门
        //{
        //    string queryBoss = "from WkTDept  where Id=" + 51;
        //    return baseService.loadEntityList(queryBoss);
        //}


        private IList getBussItself(Business b)
        {
            string query = "from Business bu where bu.BusinessDestination='" + b.BusinessDestination + "' and bu.BusinessReason='" + b.BusinessReason + "' and bu.StartTime=" + b.StartTime + " and bu.EndTime=" + b.EndTime + " and bu.Ku_Id=" + b.Ku_Id.Id + " and bu.State=" +(int) Business.stateEnum.Normal;
            return baseService.loadEntityList(query);
        }

        private IList getUserByDept(WkTDept dept)//获取登陆人所在部门
        {
            string queryUser = "from WkTUser u where u.Kdid=" + dept.Id ;
            return baseService.loadEntityList(queryUser);
        }

        ///<summary>获取某次出差的人员名单</summary>         
        private IList getEmpByBus(Business b)
        {
            string queryUser = "from BusinessEmployee be where be.BusinessId=" + b.Id+" and be.State="+(int)BusinessEmployee.stateEnum.Normal;
            return baseService.loadEntityList(queryUser);
        }

        

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
           
            if (tabControl1.SelectedIndex == 1)
            {
                initTabPage4();
            }
            if (tabControl1.SelectedIndex == 2)
            {
                initTabPage3();
            }
            if (tabControl1.SelectedIndex == 3)
            {
                 initTabPage2();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //upDateListView2(comboBox2.Text.Trim(), dateTimePicker4.Value.Date);
           
        }

        private bool roleInUser(WkTUser u,string roleName)
        {
            IList<WkTRole> roleList = u.UserRole;
            foreach (WkTRole rr in roleList)
            {
                if (rr.KrName.Trim() ==roleName)
                    return true;
            }
            return false;
        }

        private void initTabPage1()
        {
            textBox1.Text = User.KuName;
            string query = "from WkTDept";
            IList depList = baseService.loadEntityList(query);
            WkTDept dep = new WkTDept();
            depList.Remove(depList[0]);
            comboBox1.DataSource = depList;
            comboBox1.DisplayMember = "KdName";
            comboBox1.ValueMember = "Itself";
        }

        private void initTabPage2()
        {
            listView2.Items.Clear();
            string query = "from Business b";
            IList depList = baseService.loadEntityList(query);
            int i = 1;
            foreach (Business b in depList)
            {
                ListViewItem item = new ListViewItem();
                item.UseItemStyleForSubItems = false;
                item.Text = i.ToString();
                item.SubItems.Add(new DateTime(b.StartTime).ToShortDateString());
                item.SubItems.Add(new DateTime(b.EndTime).ToShortDateString());
                item.SubItems.Add(b.BusinessDestination);
                item.SubItems.Add(b.BusinessReason);
                item.SubItems.Add(b.Ku_Id.KuName);
                switch (b.PassExam)
                { 
                    case (int)Business.ExamState.waiting:
                        item.SubItems.Add("待审核");
                        break;
                    case (int)Business.ExamState.pass:
                        item.SubItems.Add("通过");
                        break;
                    case (int)Business.ExamState.npass:
                        item.SubItems.Add("退回");
                        break;
                }
                Font font=new Font(this.Font,FontStyle.Underline);
                
                item.SubItems.Add("双击查看",Color.Blue,Color.Transparent,font);
                item.Tag = b;
                listView2.Items.Add(item);
                i++;
            }
        }

        private void initTabPage4()
        {
            listView5.Items.Clear();
            listView7.Items.Clear();
            listView6.Items.Clear();
            if (true)//(roleInUser(this.User, "部门负责人"))
            {
                string query = "from Business b where b.PassExam=" + (int)Business.ExamState.waiting;
                IList depList = baseService.loadEntityList(query);
                int i = 1;
                foreach (Business b in depList)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = i.ToString();
                    item.SubItems.Add(new DateTime(b.StartTime).ToShortDateString());
                    item.SubItems.Add(new DateTime(b.EndTime).ToShortDateString());
                    item.SubItems.Add(b.BusinessDestination);
                    item.SubItems.Add(b.BusinessReason);
                    item.SubItems.Add(b.Ku_Id.KuName);
                    item.Tag = b;
                    listView5.Items.Add(item);
                    i++;
                }


                IList uList = getUserByDept(User.Kdid);
                foreach (WkTUser u in uList)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = u.KuName;
                    item.Tag = u;
                    listView7.Items.Add(item);
                }
            }

        }


        private int flagOfRole=0;
        private void initTabPage3()//初始化审批界面
        {
            listView3.Items.Clear();

            string query = "from Business b where b.PassExam=" + (int)Business.ExamState.waiting;
            IList depList = baseService.loadEntityList(query);
            int flagOfShow = 0;


            if (roleInUser(this.User, "院长"))
            {
                flagOfRole = 1;
            }
            if (roleInUser(this.User, "副院长"))
            {
                flagOfRole = 2;
            }


            if (flagOfRole == 1)//登陆用户为院长
            {

                foreach (Business b in depList)
                {
                    flagOfShow = 0;
                    int i = 1;
                    IList beList = getEmpByBus(b);
                    foreach (BusinessEmployee be in beList)//判断是否显示本条出差记录
                    {
                        if (roleInUser(be.EmployeeId, "副院长") || roleInUser(be.EmployeeId, "部门主任"))
                        {
                            flagOfShow = 1;
                        }
                    }

                    if (flagOfShow == 1)
                    {

                        ListViewItem item = new ListViewItem();
                        item.Text = i.ToString();
                        item.SubItems.Add(new DateTime(b.StartTime).ToShortDateString());
                        item.SubItems.Add(new DateTime(b.EndTime).ToShortDateString());
                        item.SubItems.Add(b.BusinessDestination);
                        item.SubItems.Add(b.BusinessReason);
                        item.SubItems.Add(b.Ku_Id.KuName);
                        item.Tag = b;
                        listView3.Items.Add(item);
                        i++;
                    }
                }
            }

            if (flagOfRole == 2)
            {

                foreach (Business b in depList)
                {
                    flagOfShow = 0;
                    int i = 1;
                    IList beList = getEmpByBus(b);
                    foreach (BusinessEmployee be in beList)
                    {
                        if (be.PassExam==(int)BusinessEmployee.ExamState.waiting&&be.EmployeeId.Kdid.KdName == this.User.Kdid.KdName && !roleInUser(be.EmployeeId, "副院长") && !roleInUser(be.EmployeeId, "部门主任"))
                        {
                            flagOfShow = 1;
                        }
                        if (be.PassExam == (int)BusinessEmployee.ExamState.waiting&&roleInUser(be.EmployeeId, "院长"))
                        {
                            flagOfShow = 1;
                        }
                    }
                    if (flagOfShow == 1)
                    {

                        ListViewItem item = new ListViewItem();
                        item.Text = i.ToString();
                        item.SubItems.Add(new DateTime(b.StartTime).ToShortDateString());
                        item.SubItems.Add(new DateTime(b.EndTime).ToShortDateString());
                        item.SubItems.Add(b.BusinessDestination);
                        item.SubItems.Add(b.BusinessReason);
                        item.SubItems.Add(b.Ku_Id.KuName);
                        item.Tag = b;
                        listView3.Items.Add(item);
                        i++;
                    }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string queryName = "from WkTUser user where user.Kdid.KdName ='"+((WkTDept)comboBox1.SelectedValue).KdName.Trim()+"'";
            IList Employee = baseService.loadEntityList(queryName);
            if (Employee.Count == 0)
                listView4.Items.Clear();
            else 
            {
                listView4.Items.Clear();
                foreach (WkTUser u in Employee)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = u.KuName;
                    item.Tag = u;
                    listView4.Items.Add(item);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listView4.SelectedItems.Count != 0)
            {
                WkTUser u = (WkTUser)listView4.SelectedItems[0].Tag;
                ListViewItem item = new ListViewItem();
                item.Text = u.KuName.Trim();
                item.SubItems.Add(u.Kdid.KdName.Trim());
                item.Tag = u;
                listView1.Items.Add(item);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 0)
            {
                listView1.Items.Remove(listView1.SelectedItems[0]);
            }
        }



        private void listView5_MouseClick(object sender, MouseEventArgs e)
        {
            Business b = (Business)listView5.SelectedItems[0].Tag;
            IList businessEmployee = getEmpByBus(b);
            listView6.Items.Clear();
            foreach (BusinessEmployee be in businessEmployee)
            {
               
                if (be.EmployeeId.Kdid.KdName == this.User.Kdid.KdName&&be.PassExam==(int)BusinessEmployee.ExamState.waiting)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = be.EmployeeId.KuName;
                    item.Tag = be;
                    listView6.Items.Add(item);
                }
            }

            selectedBusiness = b;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listView6.SelectedItems.Count != 0)
            {
                BusinessEmployee be = (BusinessEmployee)listView6.SelectedItems[0].Tag;
                baseService.deleteEntity(be);
                listView6.Items.Remove(listView6.SelectedItems[0]);

                int i = selectedBusiness.WaitingNum;
                i--;
                string update = "update LOG_T_BUSINESS set WAITINGNUM ="+ i +" where id = "+selectedBusiness.Id;
                baseService.ExecuteSQL(update);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listView7.SelectedItems.Count != 0&&listView5.SelectedItems.Count!=0)
            {
                BusinessEmployee be = new BusinessEmployee();
                be.BusinessId = selectedBusiness;
                be.EmployeeId = (WkTUser)listView7.SelectedItems[0].Tag;
                ListViewItem item = new ListViewItem();
                item.Text = be.EmployeeId.KuName;
                item.Tag = be;
                listView6.Items.Add(item);
                be.PassExam = (int)BusinessEmployee.ExamState.waiting;
                baseService.SaveOrUpdateEntity(be);

                int i = selectedBusiness.WaitingNum;
                i++;
                string update = "update LOG_T_BUSINESS set WAITINGNUM =" + i + " where id = " + selectedBusiness.Id;
                baseService.ExecuteSQL(update);

            }
        }

        private void listView3_MouseClick(object sender, MouseEventArgs e)
        {
            if (listView3.SelectedItems != null&&listView3.SelectedItems.Count!=0)
            {
                Business b = (Business)listView3.SelectedItems[0].Tag;
                IList businessEmployee = getEmpByBus(b);
                //listView8.Items.Clear();
                foreach (BusinessEmployee be in businessEmployee)
                {
                    if (be.PassExam == (int)BusinessEmployee.ExamState.waiting)//还未审核
                    {
                        if (flagOfRole == 1)//院长审核
                        {
                            if (roleInUser(be.EmployeeId, "副院长") || roleInUser(be.EmployeeId, "部门主任"))
                            {
                                ListViewItem item = new ListViewItem();
                                item.Text = be.EmployeeId.KuName;
                                item.Tag = be;
                                //listView8.Items.Add(item);
                            }
                        }
                        if (flagOfRole == 2)//副院长审核
                        {
                            if (be.EmployeeId.Kdid.KdName == this.User.Kdid.KdName && !roleInUser(be.EmployeeId, "副院长") && !roleInUser(be.EmployeeId, "部门主任"))
                            {
                                ListViewItem item = new ListViewItem();
                                item.Text = be.EmployeeId.KuName;
                                item.Tag = be;
                                //listView8.Items.Add(item);
                            }
                            if (roleInUser(be.EmployeeId, "院长"))
                            {
                                ListViewItem item = new ListViewItem();
                                item.Text = be.EmployeeId.KuName;
                                item.Tag = be;
                                //listView8.Items.Add(item);
                            }
                        }
                    }
                }
            }
        }

      /*  private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                foreach (ListViewItem item in listView3.Items)
                {
                    item.Checked = true;
                }
            }
            if (checkBox1.Checked == false )
            {
                foreach (ListViewItem item in listView3.Items)
                {
                    item.Checked = false ;
                }
            }
        }*/

        private void button6_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView3.Items)
            {
                if (item.Checked == true)
                {
                    Business b = (Business)item.Tag;
                    IList businessEmployee = getEmpByBus(b);
                    int count = b.WaitingNum;
                    #region 根据审核角色修改BusinessEmployee表
                    foreach (BusinessEmployee be in businessEmployee)
                    {
                        if (be.PassExam == (int)BusinessEmployee.ExamState.waiting)//还未审核
                        {
                            if (flagOfRole == 1)//院长审核
                            {
                                if (roleInUser(be.EmployeeId, "副院长") || roleInUser(be.EmployeeId, "部门主任"))
                                {
                                    string update = "update LOG_T_BUSINESSEMPLOYEE set PASSEXAM=" + (int)BusinessEmployee.ExamState.pass + " where Id=" + be.Id;
                                    baseService.ExecuteSQL(update);
                                    count--;
                                }
                            }
                            if (flagOfRole == 2)//副院长审核
                            {
                                if (be.EmployeeId.Kdid.KdName == this.User.Kdid.KdName && !roleInUser(be.EmployeeId, "副院长") && !roleInUser(be.EmployeeId, "部门主任"))
                                {
                                    string update = "update LOG_T_BUSINESSEMPLOYEE set PASSEXAM=" +(int) BusinessEmployee.ExamState.pass + " where Id=" + be.Id;
                                    baseService.ExecuteSQL(update);
                                    count--;
                                }
                                if (roleInUser(be.EmployeeId, "院长"))
                                {
                                    string update = "update LOG_T_BUSINESSEMPLOYEE set PASSEXAM=" + (int)BusinessEmployee.ExamState.pass + " where Id=" + be.Id;
                                    baseService.ExecuteSQL(update);
                                    count--;
                                }
                            }
                        }
                    }
                    #endregion

                    string update1 = "update LOG_T_BUSINESS set WAITINGNUM =" + count + " where id = " + b.Id;
                    baseService.ExecuteSQL(update1);
                    if (count == 0)//所有人员通过审核
                    {
                        string update2 = "update LOG_T_BUSINESS set PASSEXAM =" + (int)Business.ExamState.pass + " where id = " + b.Id;
                        baseService.ExecuteSQL(update2);
                    }
                    listView3.Items.Remove(item);
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            RefuseReason refuse = new RefuseReason();
            if (refuse.ShowDialog() == DialogResult.OK)
            {
                foreach (ListViewItem item in listView3.Items)
                {
                    if (item.Checked == true)
                    {
                        Business b = (Business)item.Tag;
                        IList businessEmployee = getEmpByBus(b);

                        #region 修改BusinessEmployee表对应的项
                        foreach (BusinessEmployee be in businessEmployee)
                        {
                            string update = "update LOG_T_BUSINESSEMPLOYEE set PASSEXAM=" + (int)BusinessEmployee.ExamState.npass + " where Id=" + be.Id;
                            baseService.ExecuteSQL(update);
                        }
                        #endregion

                        string update2 = "update LOG_T_BUSINESS set PASSEXAM =" + (int)Business.ExamState.npass + " ,REFUSEREASON='" + refuse.reason + "' ,WAITINGNUM=-1 where id = " + b.Id;
                        baseService.ExecuteSQL(update2);

                        listView3.Items.Remove(item);
                    }
                }
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            string query = "from Business b where b.StartTime>= " + dateTimePicker3.Value.Date.Ticks + " and  b.EndTime<=" + dateTimePicker4.Value.Date.Ticks + " and b.Ku_Id.KuName like '%" + textBox2.Text + "%'";
            IList depList = baseService.loadEntityList(query);
            int i = 1;
            foreach (Business b in depList)
            {
                ListViewItem item = new ListViewItem();
                item.UseItemStyleForSubItems = false;
                item.Text = i.ToString();
                item.SubItems.Add(new DateTime(b.StartTime).ToShortDateString());
                item.SubItems.Add(new DateTime(b.EndTime).ToShortDateString());
                item.SubItems.Add(b.BusinessDestination);
                item.SubItems.Add(b.BusinessReason);
                item.SubItems.Add(b.Ku_Id.KuName);
                switch (b.PassExam)
                {
                    case (int)Business.ExamState.waiting:
                        item.SubItems.Add("待审核");
                        break;
                    case (int)Business.ExamState.pass:
                        item.SubItems.Add("通过");
                        break;
                    case (int)Business.ExamState.npass:
                        item.SubItems.Add("退回");
                        break;
                }
                Font font = new Font(this.Font, FontStyle.Underline);

                item.SubItems.Add("双击查看", Color.Blue, Color.Transparent, font);
                item.Tag = b;
                listView2.Items.Add(item);
                i++;
            }
        }

        private void listView2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ViewBusiness view = new ViewBusiness();
            view.business = (Business)listView2.SelectedItems[0].Tag;
            view.ShowDialog();
        }


    }
}


    