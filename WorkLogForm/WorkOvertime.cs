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
    public partial class WorkOvertime : Form
    {

        IList<WkTRole> userrole;
        private BaseService baseService = new BaseService();
        private WkTUser user;
        public WkTUser User
        {
            get { return user; }
            set { user = value; }
        }
        private WkTRole role;

        public WkTRole Role
        {
            get { return role; }
            set { role = value; }
        }

        private WkTDept dept;

        public WkTDept Dept
        {
            get { return dept; }
            set { dept = value; }
        }

        private ViewOverWork viewForm;

        public WorkOvertime()
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
        private void min_pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            min_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.最小化_副本;
        }

        private void min_pictureBox_MouseLeave_1(object sender, EventArgs e)
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
        private void close_pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            close_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.关闭渐变_副本;
        }

        private void close_pictureBox_MouseLeave_1(object sender, EventArgs e)
        {
            close_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.关闭渐变;
        }

       
        #endregion


        #region 窗体移动代码
        private int x_point, y_point;
        private void WorkOvertime_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.x_point = e.X;
                this.y_point = e.Y;
            }
        }

        private void WorkOvertime_MouseMove(object sender, MouseEventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
        {
            upDateListView2(dateTimePicker4.Value.Date,dateTimePicker3.Value.Date,comboBox2.Text);//加班时间查询
        }

        private void WorkOvertime_Load(object sender, EventArgs e)
        {
            
            Dept = User.Kdid;
            userrole = User.UserRole;

            if (Role.KrOrder == 2)
            {
                textBox1.Text = User.KuName;
                textBox3.Text = Dept.KdName;
                upDateListView1(Dept);
            }
            else
            {
                tabControl1.TabPages.RemoveAt(2);
                tabControl1.TabPages.RemoveAt(0); 
            }
            
        }


        private IList getUserByDept(WkTDept dept)//获取登陆人所在部门的员工
        {
            string queryUser="from WkTUser u where u.Kdid="+dept.Id;
            return baseService.loadEntityList(queryUser);
        }

        private long  getOverTimeOfDay(WkTUser user, DateTime date)//获取当日加班时间
        {
            string query1 = "from WorkOverTime w where w.Date like '%" + date.Date.Ticks + "%'" + " and w.State=" + (int)WorkOverTime.stateEnum.Normal;
            IList result1=baseService.loadEntityList(query1);

            bool flag = false;
            long sum = 0;
            if (result1 != null && result1.Count != 0)
            {
                foreach (WorkOverTime wkot in result1)
                {
                    foreach (WkTUser u in wkot.WorkManId)
                    {
                        if (u.Id == user.Id)
                        {
                            flag = true;
                        }
                    }
                    if (flag)
                    {
                        sum += wkot.DayTime;
                        flag = false;
                    }
                }
            }
            return sum;
        }


        private long  getOverTimeOfMonth(WkTUser user, DateTime date)//获取当月加班时间
        {
            DateTime d1 = date.AddDays(-date.Day).Date;
            DateTime d2 = d1.AddMonths(1);
            string query2 = "from WorkOverTime w where w.Date>" + d1.Ticks+ "and w.Date<=" + d2.Ticks+" and w.State="+(int)WorkOverTime.stateEnum.Normal; 
            IList result2 = baseService.loadEntityList(query2);
            long sum=0;
            bool flag = false;
            if (result2 != null && result2.Count != 0)
            {
                foreach (WorkOverTime wkot in result2)
                {
                    foreach(WkTUser u in wkot.WorkManId)
                    {
                        if (u.Id == user.Id)
                        {
                            flag = true;
                        }
                    }
                    if (flag)
                    {
                        sum += wkot.DayTime;
                        flag = false;
                    }                    
                }               
            }           
            return sum;           
        }

        private string getOverTime(WkTUser user, DateTime d1,DateTime d2)//获取两段时间内加班时间 
        {
            //string query3 = "from WorkOverTime w where w.WorkManId =" + user.Id + "and w.Date>" + d1.ToString("yyyyMMdd000000") + "and w.Date<=" + d2.ToString("yyyyMMdd235959");
            string query3 = "from WorkOverTime w where w.WorkManId =" + user.Id + "and w.Date>=" + d1.Ticks + "and w.Date<=" + d2.Ticks;

            IList result3 = baseService.loadEntityList(query3);
            long sum = 0;
            if (result3 != null && result3.Count != 0)
            {
                foreach (WorkOverTime wkot in result3)
                {
                    sum += wkot.DayTime;
                }
            }
            TimeSpan tt = new TimeSpan(sum);
            return (int)tt.TotalHours + "小时 " + tt.Minutes + "分";

        }

        private void upDateListView1(WkTDept dep)
        {
            listView1.Items.Clear();
            IList subUsers = getUserByDept(dep);
            DateTime date = dateTimePicker1.Value;
            foreach (WkTUser u in subUsers)
            {
               
                TimeSpan  tOfDay =new TimeSpan ( getOverTimeOfDay(u, date));
                TimeSpan  tOfMonth =new TimeSpan( getOverTimeOfMonth(u, date));
                ListViewItem item = new ListViewItem();
                //item.Checked = true;
                item.SubItems.Add(u.KuName);
                item.SubItems.Add(tOfDay.Hours.ToString ()+"小时"+tOfDay.Minutes.ToString()+"分");
                item.SubItems.Add(tOfMonth.Hours.ToString() + "小时" + tOfMonth.Minutes.ToString() + "分");
                item.Tag = u;
                listView1.Items.Add(item);
            }
        }

        

        private void upDateListView2(DateTime t1,DateTime t2,string s)
        {
            listView2.Items.Clear();
            
            if(s.Contains("所有部门"))
                s="";
            string query="";
            if (Role.KrOrder <= 2)
            {
                query = "from WorkOverTime w where w.Date >= " + t1.Ticks + " and w.Date<= " + t2.Ticks + " and w.Dept.KdName  like '%" + s.Trim() + "%'  and w.State=" + (int)WorkOverTime.stateEnum.Normal + " order by w.Date";
            }
            else
            {
                query = "from WorkOverTime w where w.Date >= " + t1.Ticks + " and w.Date<= " + t2.Ticks + " and w.Dept.KdName  like '%" + User.Kdid.KdName + "%' w.Date and w.State=" + (int)WorkOverTime.stateEnum.Normal + " order by w.Date";
            }
            IList overTimes=baseService.loadEntityList(query);
            int i = 1;
            
            if (overTimes!=null)
            {
                foreach (WorkOverTime o in overTimes)
                {

                    TimeSpan ts = new TimeSpan (o.DayTime);
                    ListViewItem item = new ListViewItem();
                    item.UseItemStyleForSubItems = false;
                    item.Text = i.ToString();//序号
                    item.SubItems.Add(new DateTime (o.Date).Date.ToString ("yy年 MM月 dd日"));//
                    item.SubItems.Add(new DateTime(o.StartTime).ToString("HH点 mm分") + "---" + new DateTime(o.EndTime).ToString("HH点 mm分"));//时段
                    item.SubItems.Add(o.WorkContent);          //内容     
                    item.SubItems.Add(ts.Hours+"小时 "+ts.Minutes+"分");//总时长
                    item.SubItems.Add(o.Dept.KdName.Trim());
                    item.SubItems.Add(o.Ku_Id.KuName);//
                    Font font = new Font(this.Font, FontStyle.Underline);
                    item.SubItems.Add("双击查看", Color.Blue,Color.Transparent,font);
                    item.Tag = o;
                    listView2.Items.Add(item);
                    i++;
                }
            }
        }
        private void upDateListView4(DateTime t1, DateTime t2, string s)
        {
            listView4.Items.Clear();

            if (s.Contains("所有部门"))
                s = "";
            string query = "";
            if (Role.KrOrder ==2)
            {
                query = "from WorkOverTime w where w.Date >= " + t1.Ticks + " and w.Date<= " + t2.Ticks + " and w.Dept.KdName like '%" + s.Trim() + "%' and w.State=" + (int)WorkOverTime.stateEnum.Normal + " order by w.Date  ";
            }
            IList overTimes = baseService.loadEntityList(query);
            int i = 1;

            if (overTimes != null)
            {
                foreach (WorkOverTime o in overTimes)
                {
                    
                    TimeSpan ts = new TimeSpan(o.DayTime);
                    ListViewItem item = new ListViewItem();
                    item.UseItemStyleForSubItems = false;
                    item.Text = i.ToString();//序号
                    item.SubItems.Add(new DateTime(o.Date).Date.ToString("yy年 MM月 dd日"));//
                    item.SubItems.Add(new DateTime(o.StartTime).ToString("HH点 mm分") + "---" + new DateTime(o.EndTime).ToString("HH点 mm分"));//时段
                    item.SubItems.Add(o.WorkContent);          //内容     
                    item.SubItems.Add(ts.Hours + "小时 " + ts.Minutes + "分");//总时长
                    item.SubItems.Add(o.Dept.KdName.Trim());
                    item.SubItems.Add(o.Ku_Id.KuName);//
                    Font font = new Font(this.Font, FontStyle.Underline);
                    item.SubItems.Add("双击查看", Color.Blue, Color.Transparent, font);
                    item.Tag = o;
                    listView4.Items.Add(item);
                    i++;
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            TimeSpan ts=dateTimePicker5.Value.AddSeconds(1).TimeOfDay-dateTimePicker2.Value.TimeOfDay;
            if (ts.Hours >= 3)
            {
                MessageBox.Show("超过加班时长限制");
                return;
            }
            else
            {
                IList userlist = listView1.CheckedItems;
                ClassLibrary.WorkOverTime wkot = new WorkOverTime();
                wkot.WorkManId = new List<WkTUser>();
                if (userlist != null && userlist.Count != 0)
                {
                    foreach (ListViewItem item in userlist)
                    {                 
                        wkot.WorkManId.Add((WkTUser)item.Tag);
                    }
                    wkot.Ku_Id = User;

                    wkot.StartTime = dateTimePicker2.Value.Ticks;
                    wkot.EndTime = dateTimePicker5.Value.Ticks;
                    wkot.Date = dateTimePicker1.Value.Date.Ticks;
                    wkot.Dept = this.Dept;
                    wkot.DayTime = ts.Ticks;
                    wkot.WorkContent = textBox7.Text;
                    wkot.State = (int)WorkOverTime.stateEnum.Normal;
                    baseService.SaveOrUpdateEntity(wkot);
                    MessageBox.Show("提交成功");
                }
                else
                    MessageBox.Show("请选择人员");
                upDateListView1(Dept);
            }
               
        }

        private void button17_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            string query = "from WkTUser u where u.Kdid=" + Dept.Id + " and u.KuName like '%" + textBox5.Text + "%'";
            IList l=baseService.loadEntityList(query);
            DateTime date = dateTimePicker1.Value;
            foreach (WkTUser u in l)
            {
                TimeSpan tOfDay = new TimeSpan(getOverTimeOfDay(u, date));
                TimeSpan tOfMonth = new TimeSpan(getOverTimeOfMonth(u, date));
                ListViewItem item = new ListViewItem();
                item.SubItems.Add(u.KuName);
                item.SubItems.Add(tOfDay.Hours.ToString() + "小时" + tOfDay.Minutes.ToString() + "分");
                item.SubItems.Add(tOfMonth.Hours.ToString() + "小时" + tOfMonth.Minutes.ToString() + "分");
                item.Tag = u;
                listView1.Items.Add(item);
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            upDateListView1(Dept);
        }


        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Text=="安排概览")
            {
                string query = "from WkTDept";
                IList depList = baseService.loadEntityList(query);
                WkTDept dep = new WkTDept();
                dep.KdName = "所有部门";
                depList.RemoveAt(0);
                depList.Insert(0, dep);
                comboBox2.DataSource = depList;
                comboBox2.DisplayMember = "KdName";
                comboBox2.ValueMember = "Itself";
                upDateListView2(new DateTime(1900,1,1),new DateTime(2099,1,1),"");

            }
            if (tabControl1.SelectedTab.Text == "加班统计")
            {
                string query = "from WkTDept";
                IList depList = baseService.loadEntityList(query);
                //WkTDept dep = new WkTDept();
                depList.RemoveAt(0);
                comboBox1.DataSource = depList;
                comboBox1.DisplayMember = "KdName";
                comboBox1.ValueMember = "Itself";
                initPage3();
            }
            if (tabControl1.SelectedTab.Text == "加班取消")
            {
                initPage4();
            }
        }
        private void initPage3()
        {
            listView3.Items.Clear();
            comboBox1.Enabled = false;
            comboBox3.Enabled = false;
            if (Role.KrOrder < 2)
            {
                comboBox1.Enabled = true;
                comboBox3.Enabled = true;
            }
            else if (Role.KrOrder == 2)
            {
                comboBox3.Enabled = true;
            }
            comboBox1.SelectedText = Dept.KdName;
            comboBox3.Text = User.KuName;
            textBox4.Text="";
        }
        void initPage4()
        {
            upDateListView4(DateTime.Now.Date.AddDays(-1), new DateTime(2099, 1, 1), Dept.KdName);
        }

        private void listView2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (viewForm == null||viewForm.IsDisposed)
            {
                viewForm = new ViewOverWork();
                viewForm.wkot = (ClassLibrary.WorkOverTime)listView2.SelectedItems[0].Tag;
                viewForm.Show();
            }
            else
            {
                viewForm.wkot = (ClassLibrary.WorkOverTime)listView2.SelectedItems[0].Tag;
                viewForm.init();
                viewForm.Focus();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            IList uList = getUserByDept((WkTDept)comboBox1.SelectedValue);
            comboBox3.DataSource = uList;
            comboBox3.DisplayMember = "KuName";
            comboBox3.ValueMember="Itself";

        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox4.Text = "";
            listView3.Items.Clear();            
            DateTime t=dateTimePicker6.Value;
            DateTime st=t.Date.AddDays(-t.Day+1);
            DateTime ed = st.AddMonths(1);
            string query = "from WorkOverTime wkot where wkot.StartTime>" + st.Ticks + " and wkot.EndTime<" + ed.Ticks + "and wkot.State="+(int)WorkOverTime.stateEnum.Normal+" order by wkot.Date" ;
            IList oList=baseService.loadEntityList(query);
            foreach (WorkOverTime o in oList)
            {
                bool flag = false;
                foreach (WkTUser u in o.WorkManId)
                {
                   
                    if (u.Id == ((WkTUser)comboBox3.SelectedValue).Id)
                        flag = true;
                }
                if (flag)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = new DateTime(o.Date).ToString("dd日");
                    item.SubItems.Add(new DateTime(o.StartTime).ToString("hh点 MM分")+"---"+new DateTime(o.EndTime).ToString("hh点 MM分"));
                    item.SubItems.Add(o.WorkContent);
                    item.Tag=o;
                    listView3.Items.Add(item);
                    TimeSpan ts=new TimeSpan( getOverTimeOfMonth((WkTUser)comboBox3.SelectedValue, t.Date));
                    textBox4.Text = ts.Hours + "小时 " + ts.Minutes+"分钟";
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                foreach (ListViewItem item in listView4.Items)
                    item.Checked = true;
            }
            else
                foreach (ListViewItem item in listView4.Items)
                    item.Checked = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView4.CheckedItems)
            {
                WorkOverTime wkot = new WorkOverTime();
                wkot = (WorkOverTime)item.Tag;
                wkot.State = (int)WorkOverTime.stateEnum.Deleted;
                baseService.SaveOrUpdateEntity(wkot);
                listView4.Items.Remove(item);
            }
        }

        private void listView4_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (viewForm == null || viewForm.IsDisposed)
            {
                viewForm = new ViewOverWork();
                viewForm.wkot = (ClassLibrary.WorkOverTime)listView4.SelectedItems[0].Tag;
                viewForm.Show();
            }
            else
            {
                viewForm.wkot = (ClassLibrary.WorkOverTime)listView4.SelectedItems[0].Tag;
                viewForm.init();
                viewForm.Focus();
            }
        }
    }
}
