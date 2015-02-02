
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
    public partial class Leave : Form

    {
        private BaseService baseService = new BaseService();
        private List<WkTUser> chargeman=new List<WkTUser>();//用来保存负责人信息;
        private WkTUser leaveman;//用来保存请假者的信息
        private LeaveManage leaveobject = new LeaveManage();//用来保存审批的请假信息


        public WkTUser Leaveman
        {
            get { return leaveman; }
            set { leaveman = value; }
        }

        private WkTRole role;//
        public WkTRole Role
        {
            get { return role; }
            set { role = value; }
        }
       
        public Leave()
        {
            InitializeComponent();
            initialWindow();
            
            
        }

        #region 自定义窗体初始化方法
        /// <summary>
        /// 初始化window（界面效果）
        /// </summary>
        private void initialWindow()
        {
            creatWindow.SetFormRoundRectRgn(this, 15);
            creatWindow.SetFormShadow(this);
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
        private void Leave_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.x_point = e.X;
                this.y_point = e.Y;
            }
        }

        private void Leave_MouseMove(object sender, MouseEventArgs e)
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

       

        #region 初始化负责人listview函数

        private void initLeaveListView(IList leaveList,ListView listviewB)
        {
            int i = 1;
            Font subFont = new Font(listviewB.Font.FontFamily, 9, FontStyle.Regular);
            foreach (WkTUser h in leaveList)
            {
                ListViewItem item = new ListViewItem();
                item.Tag = h;
                item.Font = subFont;
                System.Windows.Forms.ListViewItem.ListViewSubItem nameSub = new System.Windows.Forms.ListViewItem.ListViewSubItem();
                System.Windows.Forms.ListViewItem.ListViewSubItem departmentSub = new System.Windows.Forms.ListViewItem.ListViewSubItem();
                System.Windows.Forms.ListViewItem.ListViewSubItem positionSub = new System.Windows.Forms.ListViewItem.ListViewSubItem();
                
                nameSub.Text = h.KuName.Trim();//负责人名称
                departmentSub.Text = h.Kdid.KdName.Trim();//部门名称
                foreach (WkTRole r in h.UserRole)
                {
                    if (r.KrDESC.Trim().Equals("工作小秘书角色"))
                    {
                        positionSub.Text = r.KrName;
                        
                    }
                }
                
                item.SubItems.Add(nameSub);
                item.SubItems.Add(departmentSub);
                item.SubItems.Add(positionSub);
                listviewB.Items.Add(item);
                i++;
            }

        }
        #endregion
        

        
        private void button4_Click(object sender, EventArgs e)
        {
            //保存指定的负责人到对应的请假表，先暂存在一个list中，最后界面信息提交的时候再保存信息；

            if (listView1.CheckedItems.Count == 0)
            {
                MessageBox.Show("请勾选您要指定的负责人！");
            }
            else
            {

                while (listView1.CheckedItems.Count > 0)
                {
                    if (chargeman == null)
                    {
                        chargeman = new List<WkTUser>();
                    }

                    ListViewItem item = listView1.CheckedItems[0];
                    chargeman.Add((WkTUser)item.Tag);
                    item.Checked = false;
                }
                MessageBox.Show("负责人列表已保存");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //提交请假信息
            LeaveManage lev = new LeaveManage();

            //对请假信息的填写状况进行判断
            if (dateTimePicker1.Value.Date.Ticks > dateTimePicker2.Value.Date.Ticks)
            {
                MessageBox.Show("请假开始时间必须早于或等于结束时间！");
                return;
            }
            if (comboBox2.Text == null || comboBox2.Text == "")
            {
                MessageBox.Show("请选择请假类型！");
                return;
            }

            if (textBox7.Text == null || textBox7.Text == "")
            {
                MessageBox.Show("请填写请假原因！");
                return;
            }

            lev.StartTime = dateTimePicker1.Value.Date.Ticks;
            lev.EndTime = dateTimePicker2.Value.Date.Ticks;
            lev.LeaveType = comboBox2.Text.Trim();//请假类型
            lev.LeaveReason = textBox7.Text.Trim();//请假原因
           // role.KrOrder含义，0：院长，1：副院长，2：负责人，3：员工
            if (role.KrOrder == 3)//员工提交请假信息
            {
                lev.LeaveResult = "3";//审核结果,3表示未审核的
                lev.LeaveStage = "0";//审批阶段，属于未审批
            }
            else if (role.KrOrder == 2)//负责人提交请假
            {
                
                if (comboBox2.Text.Trim() == "病假" || comboBox2.Text.Trim() == "事假")
                {
                    lev.LeaveResult = "1";
                    lev.LeaveStage = "1";
                }
                else
                {   //婚假、产假、年休假、探亲假；负责人请假后，由院长直接审批就可以
                    lev.LeaveResult = "1";
                    lev.LeaveStage = "2";
                }
            }
            else if (role.KrOrder == 1 || role.KrOrder == 0)//副院长和院长提交请假,都由院长审批
            {
                //默认负责人审批通过，待副院长审批
                lev.LeaveResult = "1";
                lev.LeaveStage = "2";
            }

            lev.Ku_Id = leaveman;//请假人信息
            lev.LeaveChargeId = chargeman;//指定的负责人列表
            lev.State = (int)IEntity.stateEnum.Normal;
            lev.TimeStamp = DateTime.Now.Ticks;

            try
            {
                baseService.SaveOrUpdateEntity(lev);
            }
            catch
            {
                MessageBox.Show("保存失败！");
                return;
            }

            MessageBox.Show("保存成功！");

            comboBox2.Text = "";
            textBox7.Clear();
            chargeman.Clear();//每次用完之后清空全局变量chargeman中的数据
            
                
        }
        #region//系统按钮效果
        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            pictureBox2.BackgroundImage = WorkLogForm.Properties.Resources.请假管理_请假申请_副本;
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            if (panel1.Visible == true)
            { return; }
            pictureBox2.BackgroundImage = WorkLogForm.Properties.Resources.请假管理_请假申请;
        }

        private void pictureBox3_MouseEnter(object sender, EventArgs e)
        {
            pictureBox3.BackgroundImage = WorkLogForm.Properties.Resources.请假管理_请假查看_副本;
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            if (panel2.Visible == true)
            { return; }
            pictureBox3.BackgroundImage = WorkLogForm.Properties.Resources.请假管理_请假查看;
        }

        private void pictureBox4_MouseEnter(object sender, EventArgs e)
        {
            pictureBox4.BackgroundImage = WorkLogForm.Properties.Resources.请假管理_请假审批__副本;
        }

        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            
            pictureBox4.BackgroundImage = WorkLogForm.Properties.Resources.请假管理_请假审批_;
        }

       
        #endregion
        private void Leave_Load(object sender, EventArgs e)
        {
            if(role.KrOrder==2)
            {
                //如果当前用户是负责人，那么请假审批的-部门以及部门选择框都不显示，该用户只能查看本部门的请假申请
                comboBox6.Visible=false;
                label23.Visible = false;
            }
            else if (role.KrOrder == 3)
            { 
                //当前用户为员工，没有请假审批和审批修改等功能
                pictureBox4.Visible = false;
                pictureBox5.Visible = false;
            }
            if (!Leaveman.Kdid.KdName.Contains("综合办公室"))
            {
                pictureBox5.Visible = false;
            }

            try
            {
                initdata1();
                initdata2();
            }
            catch
            {
                MessageBox.Show("数据加载失败！");
            }
            
            
           
        }
        #region

        private void initdata1()
        {

            //加载负责人数据
            listView1.Items.Clear();
            IList leaveList = baseService.loadEntityList("from WkTUser where Kdid = " + leaveman.Kdid.Id);
            if (leaveList != null && leaveList.Count > 0)
            {
                initLeaveListView(leaveList,listView1);
            }
        
        }
        private void initdata2() //请假申请-加载listview1中的数据，只显示该用户的请假申请信息
        {
            listView2.Items.Clear();
            IList list2 = baseService.loadEntityList("from LeaveManage where STATE=" + (int)IEntity.stateEnum.Normal + "and Ku_Id=" + leaveman.Id);
            if (role.KrOrder == 3)
            {
                comboBox1.Enabled = false;
                textBox1.Enabled = false;
            }
            if (list2 != null)
            { initlistviewdata(list2, listView2); }

        }
           
        private void initlistviewdata(IList lista,ListView list_a)//将list1中的数据加载到list_a中显示
        {
            int i = 1;
            Font subfont = new Font(list_a.Font.FontFamily, 9, FontStyle.Regular);
             
            foreach (LeaveManage u in lista)
              {
                ListViewItem item = new ListViewItem();
                item.Tag = u;
                item.Font = subfont;
                item.Text = i.ToString();
                ListViewItem.ListViewSubItem subname = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem subdepartment = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem subtype = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem subdays = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem substart_end = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem subresult = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem subchargeman = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem subreason= new ListViewItem.ListViewSubItem();
                subname.Text = u.Ku_Id.KuName.Trim();
                subdepartment.Text = u.Ku_Id.Kdid.KdName.Trim();
                subtype.Text = u.LeaveType;
                subdays.Text = ((u.EndTime - u.StartTime) / 864000000000+1).ToString();//请假天数，将数据库中保存的纳秒数装换为天
                DateTime starttime = new DateTime(u.StartTime);
                DateTime endtime = new DateTime(u.EndTime);

                substart_end.Text = starttime.ToShortDateString() + "-" + endtime.ToShortDateString(); //将显示精确到天就行
                
                foreach(WkTUser h in u.LeaveChargeId)
                {
                    subchargeman.Text = subchargeman.Text+h.KuName+" ";
                }
               
                //审批状态，审批阶段组合含义。LeaveResult=3未审批；LeaveResult：LeaveStage=0:1负责人审批未通过，1：1负责人审批通过；0：2副院长审批未通过，1：2副院长审批通过；0：3院长审批未通过，1:3院长审批通过；LeaveResult=2,审批通过
                
                if (u.LeaveResult.Trim() == "3")
                { subresult.Text = "待审批"; }
                else if (u.LeaveResult.Trim() == "2")
                { subresult.Text = "审批通过"; }
                else if (u.LeaveResult.Trim() == "0")
                {
                    if (u.LeaveStage.Trim() == "1")
                    { subresult.Text = "负责人审批未通过"; }
                    else if (u.LeaveStage.Trim() == "2")
                    { subresult.Text = "副院长审批未通过"; }
                    else if (u.LeaveStage.Trim() == "3")
                    { subresult.Text = "院长审批未通过" ;}
                }
                else if (u.LeaveResult.Trim() == "1")
                {
                    if (u.LeaveStage.Trim() == "1")
                    { subresult.Text = "待副院长审批"; }
                    else if (u.LeaveStage.Trim() == "2")
                    { subresult.Text = "待院长审批"; }        
                }
                subreason.Text = u.LeaveReason.Trim();


                item.SubItems.Add(subname);
                item.SubItems.Add(subdepartment);
                item.SubItems.Add(subtype);
                item.SubItems.Add(subdays);
                item.SubItems.Add(substart_end);
                item.SubItems.Add(subchargeman);
                item.SubItems.Add(subresult);
                item.SubItems.Add(subreason);
               
                list_a.Items.Add(item);
                
                i++;
            
            }
        
        }
        #endregion


      
       
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            pictureBox2.BackgroundImage = WorkLogForm.Properties.Resources.请假管理_请假申请_副本;
            pictureBox3.BackgroundImage = WorkLogForm.Properties.Resources.请假管理_请假查看;
            pictureBox4.BackgroundImage = WorkLogForm.Properties.Resources.请假管理_请假审批_;
            pictureBox5.BackgroundImage = WorkLogForm.Properties.Resources.请假管理_审批修改_;

            panel1.Visible = true;//请假申请
            panel2.Visible = false;//请假查看
            panel4.Visible = false;
            panel5.Visible = false;

           


        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            pictureBox2.BackgroundImage = WorkLogForm.Properties.Resources.请假管理_请假申请;
            pictureBox3.BackgroundImage = WorkLogForm.Properties.Resources.请假管理_请假查看_副本;
            pictureBox4.BackgroundImage = WorkLogForm.Properties.Resources.请假管理_请假审批_;
            pictureBox5.BackgroundImage = WorkLogForm.Properties.Resources.请假管理_审批修改_;
            
            panel1.Visible = false;
            panel2.Visible = true;
            panel4.Visible = false;
            panel5.Visible = false;
            initdata2();
            
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            pictureBox2.BackgroundImage = WorkLogForm.Properties.Resources.请假管理_请假申请;
            pictureBox3.BackgroundImage = WorkLogForm.Properties.Resources.请假管理_请假查看;
            pictureBox4.BackgroundImage = WorkLogForm.Properties.Resources.请假管理_请假审批__副本;
            pictureBox5.BackgroundImage = WorkLogForm.Properties.Resources.请假管理_审批修改_;

            panel1.Visible = false;
            panel2.Visible = false;
            panel4.Visible = true;
            panel5.Visible = false;
            initdata4();


        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            
            pictureBox2.BackgroundImage = WorkLogForm.Properties.Resources.请假管理_请假申请;
            pictureBox3.BackgroundImage = WorkLogForm.Properties.Resources.请假管理_请假查看;
            pictureBox4.BackgroundImage = WorkLogForm.Properties.Resources.请假管理_请假审批_;
            pictureBox5.BackgroundImage = WorkLogForm.Properties.Resources.请假管理_审批修改__副本;

            panel1.Visible = false;
            panel2.Visible = false;
            panel4.Visible =false;
            panel5.Visible = true;
            initdata5();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //请假查看-查询按钮
            listView2.Items.Clear();
            String sql = "";
            if (role.KrOrder<2)
            {
                sql = "select h from LeaveManage h left join h.Ku_Id u left join u.Kdid d where u.KuName like '%" + textBox1.Text.Trim() + "%'and d.KdName like '%" + comboBox1.Text.Trim() + "%' and h.StartTime>=" + dateTimePicker3.Value.Date.Ticks + " and h.EndTime<=" + dateTimePicker4.Value.Date.Ticks;
            }
            else if (role.KrOrder == 2)
            {
                sql = "select h from LeaveManage h left join h.Ku_Id u left join u.Kdid d where u.KuName like '%" + textBox1.Text.Trim() + "%'and d.KdName like '%" + leaveman.Kdid.KdName.Trim() + "%' and h.StartTime>=" + dateTimePicker3.Value.Date.Ticks + " and h.EndTime<=" + dateTimePicker4.Value.Date.Ticks;
               
            }
            else
            {
                sql = "select h from LeaveManage h left join h.Ku_Id u left join u.Kdid d where u.KuName like '%" + this.Leaveman.KuName + "%'and d.KdName like '%" + leaveman.Kdid.KdName.Trim() + "%' and h.StartTime>=" + dateTimePicker3.Value.Date.Ticks + " and h.EndTime<=" + dateTimePicker4.Value.Date.Ticks;
            }

            IList searchList = baseService.loadEntityList(sql);
            if (searchList != null && searchList.Count > 0)
            {
                initlistviewdata(searchList, listView2);
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();

            String sql = "select u from WkTUser u left join u.Kdid where u.KuName like '%" + textBox3.Text.Trim() + "%'and u.Kdid.KdName like '%" + comboBox4.Text.Trim() + "%'";

            IList leaveList = baseService.loadEntityList(sql);
            if (leaveList != null && leaveList.Count > 0)
            {
                initLeaveListView(leaveList, listView1);
                comboBox4.Text = "";
                textBox3.Text = "";
            }
            else
            {
                MessageBox.Show("没有符合条件的查询结果！");
                comboBox4.Text="";
                textBox3.Text = "";
            
            }
        }

        private void listView2_MouseDoubleClick(object sender, MouseEventArgs e)//双击listview,更改被选中数据的信息，弹出修改框，完成修改保存到数据库
        {
            //if (this.listView2.SelectedItems == null) return;
            //ListViewItem item = this.listView2.SelectedItems[0];//请假查看-listview2中的数据被选中
            //if (item == null) return;
            
            //LeaveManage u = (LeaveManage)item.Tag;
            //if (u.LeaveResult.Trim() == "3")
            //{
            //    //取出item中的数据，在请假修改groupbox3中显示

            //    groupBox3.Visible = true;


            //    label21.Text = item.SubItems[6].Text;//负责人
            //    comboBox3.Text = item.SubItems[3].Text;//请假类别
            //    textBox2.Text = item.SubItems[8].Text;//请假原因

            //    dateTimePicker5.Value = new DateTime(u.StartTime);//开始时间
            //    dateTimePicker6.Value = new DateTime(u.EndTime);//结束时间

            //}
            //else
            //    groupBox3.Visible = false;


;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            groupBox3.Visible = false;//隐藏请假修改
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //请假修改提交；首先修改界面上listview2中选中数据显示信息，然后再将更改的数据保存到数据库中
            if (this.listView2.SelectedItems == null) return;
            ListViewItem item = this.listView2.SelectedItems[0];//请假查看-listview2中的数据被选中
            if (item == null) return;
            LeaveManage levchange = new LeaveManage();
            levchange = (LeaveManage)item.Tag;

            item.SubItems[6].Text = label21.Text;//负责人
            item.SubItems[3].Text=comboBox3.Text  ;//请假类别
            item.SubItems[8].Text=textBox2.Text;//请假原因


            if (dateTimePicker5.Value.Date.Ticks > dateTimePicker6.Value.Date.Ticks)
            {
                MessageBox.Show("请假开始时间必须早于或等于结束时间！");
                return;
            }
            if (comboBox3.Text == null || comboBox3.Text == "")
            {
                MessageBox.Show("请选择请假类型！");
                return;
            }
            levchange.StartTime = dateTimePicker1.Value.Date.Ticks;
            levchange.EndTime = dateTimePicker2.Value.Date.Ticks;
            levchange.LeaveType = comboBox2.Text.Trim();//请假类型
            levchange.LeaveReason = textBox7.Text.Trim();//请假原因
           

            levchange.Ku_Id = leaveman;//请假人信息
            levchange.LeaveChargeId = chargeman;//指定的负责人列表
            levchange.State = (int)IEntity.stateEnum.Normal;
            levchange.TimeStamp = DateTime.Now.Ticks;

            try
            {
                baseService.SaveOrUpdateEntity(levchange);
            }
            catch
            {
                MessageBox.Show("保存失败！");
                return;
            }

            MessageBox.Show("保存成功！");
        }

       

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //重新指定负责人
            panel3.Visible = true;
            initdata3();


        }
        private void initdata3()
        {

            //加载负责人数据
            listView3.Items.Clear();
            IList leaveList = baseService.loadEntityList("from WkTUser where Kdid = " + leaveman.Kdid.Id);
            if (leaveList != null && leaveList.Count > 0)
            {
                initLeaveListView(leaveList,listView3);
            }

        }

        private void button10_Click(object sender, EventArgs e)
        {
            //重新指定负责人确定
            if (listView3.CheckedItems.Count == 0)
            {
                MessageBox.Show("请勾选您要指定的负责人！");
            }
            else
            {

                while (listView3.CheckedItems.Count > 0)
                {
                    if (chargeman == null)
                    {
                        chargeman = new List<WkTUser>();
                    }

                    ListViewItem item = listView3.CheckedItems[0];
                    chargeman.Add((WkTUser)item.Tag);
                    item.Checked = false;
                }
                MessageBox.Show("负责人列表已保存");
            }
            panel3.Visible = false;

        }

        private void button11_Click(object sender, EventArgs e)
        {
            listView3.Items.Clear();
            String sql = "select u from WkTUser u left join u.Kdid where u.KuName like '%" + textBox4.Text.Trim() + "%'and u.Kdid.KdName like '%" + comboBox5.Text.Trim() + "%'";

            IList leaveList = baseService.loadEntityList(sql);
            if (leaveList != null && leaveList.Count > 0)
            {
                initLeaveListView(leaveList, listView3);
                textBox4.Text = "";
                comboBox5.Text = "";
            }
            else
            {
                MessageBox.Show("不存在符合条件的查询结果！");
                textBox4.Text="";
                comboBox5.Text = "";

            }

        }

        #region//请假修改

        private void initdata4() //请假审批-加载listview4中的数据，只显示未审批的请假申请
        {
            //根据当前用户角色的不同，加载不同的审批数据；
            listView4.Items.Clear();
            //给负责人加载所有提交的未审批请假数据；副院长-加载负责人已经通过的的请假；院长-只审批副院长已经审批通过的请假
            String sql = "";
            if (role.KrOrder == 0)
            { 
              //院长,只加载副院长通过审核的申请（多个部门）
                sql = "from LeaveManage where STATE=" + (int)IEntity.stateEnum.Normal + "and LeaveStage=" + 2 + "and LeaveResult=" + 1;
            
            }
            else if (role.KrOrder == 1)
            { //副院长，只是加载负责人审批通过的请假申请（多个部门）
                           
                //sql = "from LeaveManage where STATE=" + (int)IEntity.stateEnum.Normal + "and LeaveStage=" + 1 + "and LeaveResult=" + 1;
                sql = "from LeaveManage leave where leave.Ku_Id.Kdid in (select w.DeptId from Wktuser_M_Dept w where w.WktuserId=" + Leaveman.Id + " and w.State=" + (int)IEntity.stateEnum.Normal + " ) and STATE=" + (int)IEntity.stateEnum.Normal + "and LeaveStage=" + 1 + "and LeaveResult=" + 1;
            }
            else if (role.KrOrder == 2)
            { 
            //负责人，加载员工提交的请假申请，但是只是加载本部门的（单个部门）
                sql = "from LeaveManage where STATE=" + (int)IEntity.stateEnum.Normal + "and LeaveResult=" + 3 + "and Ku_Id.Kdid.KdName like '%" + leaveman.Kdid.KdName.Trim()+"%'";
            }
            IList list4 = baseService.loadEntityList(sql);
            if (list4 != null)
            { initexaminedata(list4, listView4); }

        }




        private void initexaminedata(IList lista, ListView list_a)//将list4中的数据加载到listView4中显示
        {
            
            int i = 1;
            Font subfont = new Font(list_a.Font.FontFamily, 9, FontStyle.Regular);

            foreach (LeaveManage u in lista)
            {
                ListViewItem item = new ListViewItem();
                item.Tag = u;
                item.Font = subfont;
                item.Text = i.ToString();
                ListViewItem.ListViewSubItem subname = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem subdepartment = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem subtype = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem subdays = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem substart_end = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem subresult = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem subchargeman = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem subreason = new ListViewItem.ListViewSubItem();

                subname.Text = u.Ku_Id.KuName.Trim();
                subdepartment.Text = u.Ku_Id.Kdid.KdName.Trim();
                subtype.Text = u.LeaveType;
                subdays.Text = ((u.EndTime - u.StartTime) / 864000000000+1).ToString();//请假天数，将数据库中保存的纳秒数装换为天
                DateTime starttime = new DateTime(u.StartTime);
                DateTime endtime = new DateTime(u.EndTime);

                substart_end.Text = starttime.ToShortDateString() + "-" + endtime.ToShortDateString(); //将显示精确到天就行

                foreach (WkTUser h in u.LeaveChargeId)
                {
                    subchargeman.Text = subchargeman.Text + h.KuName + " ";//负责人可能有多个
                }

               //


              
                subresult.Text = "未审批"; 
                subreason.Text = u.LeaveReason.Trim();


                item.SubItems.Add(subname);
                item.SubItems.Add(subdepartment);
                item.SubItems.Add(subtype);
                item.SubItems.Add(subdays);
                item.SubItems.Add(substart_end);
                item.SubItems.Add(subchargeman);
                item.SubItems.Add(subresult);
                item.SubItems.Add(subreason);

                list_a.Items.Add(item);

                i++;

            }

        }

        private void button12_Click(object sender, EventArgs e)
        {
            //请假审批查询按钮

            listView4.Items.Clear();
            String sql="";
            if (role.KrOrder == 0)
            {
                //院长,只加载副院长通过审核的申请（多个部门）同时添加限制条件
                sql = "select h from LeaveManage h left join h.Ku_Id u left join u.Kdid d where u.KuName like '%" + textBox5.Text.Trim() + "%'and d.KdName like '%" + comboBox6.Text.Trim() + "%' and h.StartTime>=" + dateTimePicker7.Value.Date.Ticks + " and h.EndTime<=" + dateTimePicker8.Value.Date.Ticks +"and h.LeaveStage=" + 2 + "and h.LeaveResult=" + 1;
              
            }
            else if (role.KrOrder == 1)
            { //副院长，只是加载负责人审批通过的请假申请（多个部门）
                
                //sql = "select h from LeaveManage h left join h.Ku_Id u left join u.Kdid d where u.KuName like '%" + textBox5.Text.Trim() + "%'and d.KdName like '%" + comboBox6.Text.Trim() + "%' and h.StartTime>=" + dateTimePicker7.Value.Date.Ticks + " and h.EndTime<=" + dateTimePicker8.Value.Date.Ticks + "and h.LeaveStage=" + 1 + "and h.LeaveResult=" + 1;
                sql = "select h from LeaveManage h left join h.Ku_Id u left join u.Kdid d where u.KuName like '%" + textBox5.Text.Trim() + "%'and d.KdName like '%" + comboBox6.Text.Trim() + "%' and h.StartTime>=" + dateTimePicker7.Value.Date.Ticks + " and h.EndTime<=" + dateTimePicker8.Value.Date.Ticks + " and  h.Ku_Id.Kdid in (select w.DeptId from Wktuser_M_Dept w where w.WktuserId=" + Leaveman.Id + " and w.State=" + (int)IEntity.stateEnum.Normal + " )  and h.LeaveStage=" + 1 + "and h.LeaveResult=" + 1;

            }
            else if (role.KrOrder == 2)
            {
                //负责人,只加载本部门
                
                  sql = "select h from LeaveManage h left join h.Ku_Id u left join u.Kdid d where u.KuName like '%" + textBox5.Text.Trim() + "%'and d.KdName like '%" + leaveman.Kdid.KdName + "%' and h.StartTime>=" + dateTimePicker7.Value.Date.Ticks + " and h.EndTime<=" + dateTimePicker8.Value.Date.Ticks + "and h.LeaveStage=" + 1 + "and h.LeaveResult=" + 1;    
            }

            IList searchList = baseService.loadEntityList(sql);
            if (searchList != null && searchList.Count > 0)
            {
                initlistviewdata(searchList, listView4);
            }

        }

        private void button13_Click(object sender, EventArgs e)
        {
            //请假审批-通过按钮

            while (listView4.CheckedItems.Count > 0)
            {
                if (leaveobject == null)
                {
                    leaveobject = new LeaveManage();
                }

                ListViewItem item = listView4.CheckedItems[0];
                leaveobject = (LeaveManage)item.Tag;
                if(role.KrOrder==0)
                {
                    //院长审批阶段
                    leaveobject.LeaveStage = "3";
                    leaveobject.LeaveResult ="2";
                }
                else if (role.KrOrder == 1)
                {
                    //副院长审批
                    if (leaveobject.LeaveType == "病假" || leaveobject.LeaveType == "事假")
                    {
                       //判断请假天数，如果小于10的话，可以直接审批通过，无需院长继续审批
                        if (((leaveobject.EndTime - leaveobject.StartTime) / 864000000000 + 1) <= 10)
                        {
                            leaveobject.LeaveStage = "2";
                            leaveobject.LeaveResult = "2";//审批完全通过
                        }
                    }
                    else
                    {
                        leaveobject.LeaveStage = "2";
                        leaveobject.LeaveResult = "1";
                    }
                
                }
                else if (role.KrOrder == 2)
                { 
                //负责人审批
                    if (leaveobject.LeaveType == "病假" || leaveobject.LeaveType == "事假")
                    {
                        //判断请假天数，如果小于3的话，可以直接审批通过，无需副院长继续审批
                        if (((leaveobject.EndTime - leaveobject.StartTime) / 864000000000 + 1) <= 3)
                        {
                            leaveobject.LeaveStage = "1";
                            leaveobject.LeaveResult = "2";//审批完全通过
                        }
                    }
                    else
                    {
                        leaveobject.LeaveStage = "1";
                        leaveobject.LeaveResult = "1";
                    }
                
                }
                try
                {
                    baseService.SaveOrUpdateEntity(leaveobject);
                }
                catch
                {
                    MessageBox.Show("审批失败！");
                    return;
                }
               
                item.Checked = false;
                listView4.Items.Remove(item);
            }
            
            MessageBox.Show("审批成功！");
            
        }

        private void button14_Click(object sender, EventArgs e)
        {
            //请假审批-通过按钮

            while (listView4.CheckedItems.Count > 0)
            {
                if (leaveobject == null)
                {
                    leaveobject = new LeaveManage();
                }

                ListViewItem item = listView4.CheckedItems[0];
                leaveobject = (LeaveManage)item.Tag;
                if (role.KrOrder == 0)
                {
                    //院长审批未通过
                    leaveobject.LeaveStage = "3";
                    leaveobject.LeaveResult = "0";
                }
                else if (role.KrOrder == 1)
                {
                    //副院长审批未通过
                   
                        leaveobject.LeaveStage = "2";
                        leaveobject.LeaveResult = "0";   

                }
                else if (role.KrOrder == 2)
                {
                    
                    
                        leaveobject.LeaveStage = "1";
                        leaveobject.LeaveResult = "0";
                    
                }
                try
                {
                    baseService.SaveOrUpdateEntity(leaveobject);
                }
                catch
                {
                    MessageBox.Show("审批失败！");
                    return;
                }

                item.Checked = false;
                //item.SubItems.Clear();
                listView4.Items.Remove(item);
            }

            MessageBox.Show("审批成功！");
        }
       
#endregion//


        private void initdata5() //请假审批-加载listview5中的数据，只显示已经审批的请假申请
        {
            //根据当前用户角色的不同，加载不同的审批数据；
            listView5.Items.Clear();
            
            String sql = "";
            if (role.KrOrder == 0)
            {
                //院长,只加载院长审核过的申请（多个部门）
                sql = "from LeaveManage where STATE=" + (int)IEntity.stateEnum.Normal + "and LeaveStage=" + 3;

            }
            else if (role.KrOrder == 1)
            { //副院长，只是加载负责人审批通过的请假申请（多个部门）
                sql = "from LeaveManage leave where leave.STATE=" + (int)IEntity.stateEnum.Normal + "and leave.Ku_Id.Kdid in (select w.DeptId from Wktuser_M_Dept w where w.WktuserId=" + Leaveman.Id + " and w.State=" + (int)IEntity.stateEnum.Normal + " ) and LeaveStage=" + 2;
            }
            else if (role.KrOrder == 2)
            {
                //负责人，加载员工提交的请假申请，但是只是加载本部门的（单个部门）
                sql = "from LeaveManage where STATE=" + (int)IEntity.stateEnum.Normal + "and LeaveResult=" + 1 + "and Ku_Id.Kdid.KdName like '%" + leaveman.Kdid.KdName+"%'";
            }
            IList list5 = baseService.loadEntityList(sql);
            if (list5 != null)
            { initexaminedata1(list5, listView5); }

        }

        private void initexaminedata1(IList lista, ListView list_a)//将list4中的数据加载到listView4中显示
        {

            int i = 1;
            Font subfont = new Font(list_a.Font.FontFamily, 9, FontStyle.Regular);

            foreach (LeaveManage u in lista)
            {
                ListViewItem item = new ListViewItem();
                item.Tag = u;
                item.Font = subfont;
                item.Text = i.ToString();
                ListViewItem.ListViewSubItem subname = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem subdepartment = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem subtype = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem subdays = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem substart_end = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem subresult = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem subchargeman = new ListViewItem.ListViewSubItem();
                ListViewItem.ListViewSubItem subreason = new ListViewItem.ListViewSubItem();

                subname.Text = u.Ku_Id.KuName.Trim();
                subdepartment.Text = u.Ku_Id.Kdid.KdName.Trim();
                subtype.Text = u.LeaveType;
                subdays.Text = ((u.EndTime - u.StartTime) / 864000000000 + 1).ToString();//请假天数，将数据库中保存的纳秒数装换为天
                DateTime starttime = new DateTime(u.StartTime);
                DateTime endtime = new DateTime(u.EndTime);

                substart_end.Text = starttime.ToShortDateString() + "-" + endtime.ToShortDateString(); //将显示精确到天就行

                foreach (WkTUser h in u.LeaveChargeId)
                {
                    subchargeman.Text = subchargeman.Text + h.KuName + " ";//负责人可能有多个
                }

                //

                if (u.LeaveResult.Trim() == "0")
                {
                    subresult.Text = "已审批-未通过";
                }
                else if (u.LeaveResult.Trim() == "1" || u.LeaveResult.Trim() == "2")
                {
                    subresult.Text = "已审批-通过";
                }
                
                subreason.Text = u.LeaveReason.Trim();


                item.SubItems.Add(subname);
                item.SubItems.Add(subdepartment);
                item.SubItems.Add(subtype);
                item.SubItems.Add(subdays);
                item.SubItems.Add(substart_end);
                item.SubItems.Add(subchargeman);
                item.SubItems.Add(subresult);
                item.SubItems.Add(subreason);

                list_a.Items.Add(item);

                i++;

            }

        }

        private void listView2_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.listView2.SelectedItems == null) return;
            ListViewItem item = this.listView2.SelectedItems[0];//请假查看-listview2中的数据被选中
            if (item == null) return;

            LeaveManage u = (LeaveManage)item.Tag;
          

            groupBox3.Visible = true;

            button8.Visible = false;
            button9.Visible = false;
            linkLabel1.Visible = false;
            label21.Text = item.SubItems[6].Text;//负责人
            comboBox3.Text = item.SubItems[3].Text;//请假类别
            textBox2.Text = item.SubItems[8].Text;//请假原因

            dateTimePicker5.Value = new DateTime(u.StartTime);//开始时间
            dateTimePicker6.Value = new DateTime(u.EndTime);//结束时间
        }

      
      
        
       

    }
}
