
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
        private IList EmpInBusDept;

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


        #region tabpage1 出差发起

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
            listView9.Items.Clear();
            IList Boss = getBoss();
            foreach (WkTUser b in Boss)
            {
                ListViewItem item = new ListViewItem();
                item.Text = b.KuName;
                item.Tag = b;
                listView9.Items.Add(item);
            }
            listView9.SelectedItems.Clear();
        }


        private void button17_Click(object sender, EventArgs e)//出差发起提交
        {
            if (listView9.SelectedItems.Count==0)
                MessageBox.Show("请指定审批院领导");
            else if (textBox4.Text == "" || textBox5.Text == "" || listView1.Items.Count == 0)
                MessageBox.Show("请完成出差表单");
            else if (dateTimePicker1.Value < DateTime.Now.Date || dateTimePicker2.Value < dateTimePicker1.Value)
                MessageBox.Show("请正确选择时间");
            else
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
                buss.Boss = (WkTUser)listView9.SelectedItems[0].Tag;
                
//                Business buss1 = (Business)(getBussItself(buss)[0]);

                buss.BusinessEmployee = new List<BusinessEmployee>();
                foreach (ListViewItem row in listView1.Items)
                {
                    BusinessEmployee be = new BusinessEmployee();
                    be.EmployeeId = (WkTUser)row.Tag;
                    be.PassExam = (int)BusinessEmployee.ExamState.waiting;

                    buss.BusinessEmployee.Add(be);
                }
                baseService.SaveOrUpdateEntity(buss);

                MessageBox.Show("添加成功！");
                listView9.SelectedItems.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                listView4.Items.Clear();
                listView1.Items.Clear();
            }
           
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)//选择部门
        {
            string queryName = "from WkTUser user where user.Kdid.KdName ='" + ((WkTDept)comboBox1.SelectedValue).KdName.Trim() + "'";
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

        private void button1_Click(object sender, EventArgs e)//添加人员
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

        private void button3_Click(object sender, EventArgs e)//移除人员
        {
            if (listView1.SelectedItems.Count != 0)
            {
                listView1.Items.Remove(listView1.SelectedItems[0]);
            }
        }

        #endregion
        #region tabpage4 部门审核
        private void initTabPage4()
        {
            listView5.Items.Clear();
            listView6.Items.Clear();
            if (roleInUser(this.User, "部门主任"))
            {
                string query = "from Business b where b.PassExam=" + (int)Business.ExamState.waiting;
                IList depList = baseService.loadEntityList(query);
                int i = 1;
                foreach (Business b in depList)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = i.ToString();
                    item.SubItems.Add(b.Ku_Id.KuName);
                    item.Tag = b;
                    listView5.Items.Add(item);
                    i++;
                }

                //IList uList = getUserByDept(User.Kdid);
                //foreach (WkTUser u in uList)
                //{
                //    ListViewItem item = new ListViewItem();
                //    item.Text = u.KuName;
                //    item.Tag = u;
                //    listView7.Items.Add(item);
                //}
            }

        }


        private void listView5_MouseClick(object sender, MouseEventArgs e)
        {
            Business b = (Business)listView5.SelectedItems[0].Tag;
            IList businessEmployee = getEmpByBus(b);
            listView6.Items.Clear();
            foreach (BusinessEmployee be in businessEmployee)
            {

                ListViewItem item = new ListViewItem();
                item.Text = be.EmployeeId.KuName;
                item.SubItems.Add(be.EmployeeId.Kdid.KdName.Trim());
                switch (be.PassExam)
                {
                    case (int)BusinessEmployee.ExamState.waiting:
                        item.SubItems.Add("未审核");
                        break;
                    case (int)BusinessEmployee.ExamState.pass:
                        item.SubItems.Add("已审核");
                        break;
                }

                item.Tag = be;
                listView6.Items.Add(item);
            }
            DateTime st=new DateTime(b.StartTime), et=new DateTime(b.EndTime);
            textBox6.Text = b.BusinessReason;
            textBox7.Text = st.ToString("yyyy年 MM月 dd日") + "----" + et.ToString("yyyy年 MM月 dd日");
            textBox8.Text = b.BusinessDestination;
            textBox12.Text = b.BusinessNote;


            string queryEmp = "from BusinessEmployee be where be.BusinessId=" + b.Id + "and be.EmployeeId.Kdid=" + User.Kdid.Id + "and  be.PassExam=" + (int)BusinessEmployee.ExamState.waiting + " and be.State=" + (int)BusinessEmployee.stateEnum.Normal;
            EmpInBusDept = baseService.loadEntityList(queryEmp);
            if (EmpInBusDept == null||EmpInBusDept.Count==0)
            {
                button4.Enabled = false;
                button5.Enabled = false;
                button8.Enabled = false;
            }
            else 
            {
                button4.Enabled = true;
                button5.Enabled = true;
                button8.Enabled = true;
            }

            selectedBusiness = b;
        }

        private void button5_Click(object sender, EventArgs e)// 审核通过
        {
            if (EmpInBusDept != null&&EmpInBusDept.Count!=0)
            {
                foreach (BusinessEmployee be in EmpInBusDept)
                {
                    
                    string query1 = "update LOG_T_BUSINESSEMPLOYEE set PASSEXAM=" + (int)BusinessEmployee.ExamState.pass + " where Id=" + be.Id;//修改员工审核状态为通过
                    baseService.ExecuteSQL(query1);
                }

                int waitNum = selectedBusiness.WaitingNum - EmpInBusDept.Count;
                string query2 = "update LOG_T_BUSINESS set WAITINGNUM=" +waitNum + " where Id=" + selectedBusiness.Id;//修改待审核人数
                baseService.ExecuteSQL(query2);
                if (waitNum == 0)
                {
                    string query3 = "update LOG_T_BUSINESS set PASSEXAM=" + (int)Business.ExamState.pass + " where Id=" + selectedBusiness.Id;//修改出差审核状态为通过
                    baseService.ExecuteSQL(query3);
                }
                MessageBox.Show("审核通过！");
                initTabPage4();
            }
        }
        #endregion
        #region tabpage3 院领导审批
        private int flagOfRole = 0;
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
                        if (be.PassExam == (int)BusinessEmployee.ExamState.waiting && be.EmployeeId.Kdid.KdName == this.User.Kdid.KdName && !roleInUser(be.EmployeeId, "副院长") && !roleInUser(be.EmployeeId, "部门主任"))
                        {
                            flagOfShow = 1;
                        }
                        if (be.PassExam == (int)BusinessEmployee.ExamState.waiting && roleInUser(be.EmployeeId, "院长"))
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

        #endregion
        #region tabpage2 出差查看
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
                Font font = new Font(this.Font, FontStyle.Underline);

                item.SubItems.Add("双击查看", Color.Blue, Color.Transparent, font);
                item.Tag = b;
                listView2.Items.Add(item);
                i++;
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

        #endregion



        
        private void BusinessManagement_Load(object sender, EventArgs e)
        {
            Dept = User.Kdid;
            initTabPage1();
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



        /*
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
        }*/

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
        }

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

        */



        #region 数据库操作
        //private IList getBossDept()//获取院领导部门
        //{
        //    string queryBoss = "from WkTDept  where Id=" + 51;
        //    return baseService.loadEntityList(queryBoss);
        //}

        private IList getBoss()//获取院领导
        {

            string qureyBoss = "from WkTUser b where b.Kdid=51";
            return baseService.loadEntityList(qureyBoss);
        }

        private IList getBussItself(Business b)
        {
            string query = "from Business bu where bu.BusinessDestination='" + b.BusinessDestination + "' and bu.BusinessReason='" + b.BusinessReason + "' and bu.StartTime=" + b.StartTime + " and bu.EndTime=" + b.EndTime + " and bu.Ku_Id=" + b.Ku_Id.Id + " and bu.State=" + (int)Business.stateEnum.Normal;
            return baseService.loadEntityList(query);
        }

        private IList getUserByDept(WkTDept dept)//获取登陆人所在部门的员工
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

        private bool roleInUser(WkTUser u, string roleName)
        {
            IList<WkTRole> roleList = u.UserRole;
            foreach (WkTRole rr in roleList)
            {
                if (rr.KrName.Trim() == roleName && rr.KrDESC == "工作小秘书角色")
                    return true;
            }
            return false;
        }
        #endregion

        private void button4_Click(object sender, EventArgs e)
        {
            BusinessChange bcForm = new BusinessChange();
            bcForm.Tag = EmpInBusDept;
            bcForm.ShowDialog();
            initTabPage4();
        }



    }
}


    

