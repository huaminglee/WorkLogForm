
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

         #region 窗口操作
         private void BusinessManagement_Load(object sender, EventArgs e)
         {
             Dept = User.Kdid;
             initTabPage1();
         }
         private void tabControl1_Selected(object sender, TabControlEventArgs e)
         {
             if (tabControl1.SelectedIndex == 0)
             {
                 initTabPage1();
             }

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
                 initTabPage5();
             }
             if (tabControl1.SelectedIndex == 4)
             {
                 initTabPage2();
             }

         }
         #endregion
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
            if (Employee.Count == 0&&Employee==null)
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
                if (depList != null)
                {
                    foreach (Business b in depList)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = i.ToString();
                        item.SubItems.Add(b.Ku_Id.KuName);
                        item.Tag = b;
                        listView5.Items.Add(item);
                        i++;
                    }
                }
            }
            selectedBusiness = null;
            EmpInBusDept = null;
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox12.Text = "";
            button4.Enabled = false;
            button5.Enabled = false;
            button8.Enabled = false;
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

        private void button8_Click(object sender, EventArgs e)//撤销出差
        {
            Business b = selectedBusiness;
            b.PassExam = (int)Business.ExamState.npass;
            //IList empList = getEmpByBus(b);
            foreach (BusinessEmployee be in b.BusinessEmployee)
            {
                be.PassExam = (int)BusinessEmployee.ExamState.npass;
            }
            b.WaitingNum = -1;
            baseService.SaveOrUpdateEntity(b);
            initTabPage4();
        }
        private void button4_Click(object sender, EventArgs e)//人员修改
        {
            BusinessChange bcForm = new BusinessChange();
            bcForm.Tag = EmpInBusDept;
            bcForm.ShowDialog();
            initTabPage4();
        }
        #endregion
        #region tabpage3 院领导审批
        private void initTabPage3()//初始化审批界面
        {
            listView3.Items.Clear();
            listView7.Items.Clear();
            string query = "from Business b where b.Boss="+User.Id+"and b.PassExam=" + (int)Business.ExamState.pass;
            IList busList = baseService.loadEntityList(query);
            int i = 1;
            if (busList != null)
            {
                foreach (Business b in busList)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = i.ToString();
                    item.SubItems.Add(b.Ku_Id.KuName);
                    item.Tag = b;
                    i++;
                    listView7.Items.Add(item);

                }
            }
            textBox9.Text = "";
            textBox10.Text = "";
            textBox11.Text = "";
            textBox13.Text = "";
            selectedBusiness = null;
        }
        private void listView7_MouseClick(object sender, MouseEventArgs e)
        {
            listView3.Items.Clear();
            Business b = (Business)listView7.SelectedItems[0].Tag;
            foreach (BusinessEmployee be in b.BusinessEmployee)
            {
                ListViewItem item = new ListViewItem();
                item.Text = be.EmployeeId.KuName;
                item.SubItems.Add(be.EmployeeId.Kdid.KdName.Trim());
                item.Tag = be;
                listView3.Items.Add(item);
            }

            DateTime st = new DateTime(b.StartTime), et = new DateTime(b.EndTime);
            textBox9.Text = b.BusinessNote;
            textBox10.Text = b.BusinessDestination;
            textBox11.Text = st.ToString("yyyy年 MM月 dd日") + "----" + et.ToString("yyyy年 MM月 dd日");
            textBox13.Text = b.BusinessReason;
            selectedBusiness = b;

        }
        private void button7_Click(object sender, EventArgs e)
        {
            Business b = selectedBusiness;
            foreach (BusinessEmployee be in b.BusinessEmployee)
            {
                be.PassExam = (int)BusinessEmployee.ExamState.done;
            }
            b.PassExam = (int)Business.ExamState.done;
            baseService.SaveOrUpdateEntity(b);
            initTabPage3();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            Business b = selectedBusiness;
            foreach (BusinessEmployee be in b.BusinessEmployee)
            {
                be.PassExam = (int)BusinessEmployee.ExamState.npass;
            }
            b.PassExam = (int)Business.ExamState.npass;
            baseService.SaveOrUpdateEntity(b);
            initTabPage3();
        }
        private void button9_Click(object sender, EventArgs e)
        {
            BusinessRedo burdo = new BusinessRedo();
            burdo.Tag = selectedBusiness;
            burdo.ShowDialog();
            MessageBox.Show("退回成功");
            initTabPage3();
        }


        #endregion
        #region 退回修改
        private void initTabPage5()
        {
            listView8.Items.Clear();
            listView10.Items.Clear();
            string query = "from Business b where b.Ku_Id=" + this.User.Id + "and b.PassExam=" + (int)Business.ExamState.redo;
            IList busList = baseService.loadEntityList(query);
            int i=1;
            if (busList != null)
            {
                foreach (Business b in busList)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = i.ToString();
                    item.SubItems.Add(b.Boss.KuName);
                    item.Tag = b;
                    listView10.Items.Add(item);
                }
            }

            textBox14.Text = "";
            textBox15.Text = "";
            textBox16.Text = "";
            textBox17.Text = "";
            selectedBusiness = null;
        }

        private void listView10_Click(object sender, EventArgs e)
        {
            listView8.Items.Clear();
            Business b = (Business)this.listView10.SelectedItems[0].Tag;
            foreach (BusinessEmployee be in b.BusinessEmployee)
            {
                ListViewItem item = new ListViewItem();
                item.Text = be.EmployeeId.KuName;
                item.SubItems.Add(be.EmployeeId.Kdid.KdName.Trim());
                switch (be.PassExam)
                {
                    case (int)BusinessEmployee.ExamState.redo:
                        item.SubItems.Add("退回");
                        break;
                    case (int)BusinessEmployee.ExamState.pass:
                        item.SubItems.Add("通过审批");
                        break;
                }
                item.Tag = be;
                listView8.Items.Add(item);
            }
            selectedBusiness = b;
            DateTime st = new DateTime(b.StartTime), et = new DateTime(b.EndTime);
            textBox17.Text = b.BusinessReason;
            textBox15.Text = b.BusinessDestination;
            textBox14.Text = b.BusinessNote;
            textBox16.Text = st.ToString("yyyy年 MM月 dd日") + "----" + et.ToString("yyyy年 MM月 dd日");
        }
        private void button11_Click(object sender, EventArgs e)//一键通过
        {
            Business b = selectedBusiness;
            foreach (BusinessEmployee be in b.BusinessEmployee)
            {
                if (be.PassExam == (int)BusinessEmployee.ExamState.redo)
                {
                    b.BusinessEmployee.Remove(be);
                }
                else if (be.PassExam == (int)BusinessEmployee.ExamState.pass)
                {
                    be.PassExam = (int)BusinessEmployee.ExamState.done;
                }
            }
            b.PassExam = (int)Business.ExamState.done;
            baseService.SaveOrUpdateEntity(b);
        }

        #endregion
        #region tabpage2 出差查看
        private void initTabPage2()
        {
            listView2.Items.Clear();
            string query = "from Business b";
            IList depList = baseService.loadEntityList(query);
            int i = 1;

            if (depList != null)
            {
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


      


    }
}


    

