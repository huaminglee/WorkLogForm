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
        private WkTDept dept;

        public WkTDept Dept
        {
            get { return dept; }
            set { dept = value; }
        }



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
            int flag=0;
            foreach (WkTRole r in userrole)
            {
                if (r.KrName == "部门主任")
                {
                    flag=1;
                }
                
            }

            if (flag==1)
            {
                textBox1.Text = User.KuName;
                textBox3.Text = Dept.KdName;
                upDateListView1(Dept);
            }
            else
                tabControl1.Enabled = false;
            
        }


        private IList getUserByDept(WkTDept dept)//获取登陆人所在部门
        {
            string queryUser="from WkTUser u where u.Kdid="+dept.Id;
            return baseService.loadEntityList(queryUser);
        }

        private string  getOverTimeOfDay(WkTUser user, DateTime date)//获取当日加班时间
        {
            string query1 = "from WorkOverTime w where w.WorkManId =" + user.Id+" and w.Date like '%" + date.Date.Ticks + "%'";
            IList result1=baseService.loadEntityList(query1);

            long sum = 0;
            if (result1 != null && result1.Count != 0)
            {
                foreach (WorkOverTime wkot in result1)
                {
                    sum += wkot.DayTime;
                }
            }
            DateTime daytime = new DateTime(sum);
            return daytime.ToString("HH小时 mm分");
        }


        private string  getOverTimeOfMonth(WkTUser user, DateTime date)//获取当月加班时间
        {
            DateTime d1 = date.AddDays(-date.Day).Date;
            DateTime d2 = d1.AddMonths(1);
            string query2 = "from WorkOverTime w where w.WorkManId =" + user.Id + "and w.Date>" + d1.Ticks+ "and w.Date<=" + d2.Ticks;
            IList result2 = baseService.loadEntityList(query2);
            long sum=0;
            if (result2 != null && result2.Count != 0)
            {
                foreach (WorkOverTime wkot in result2)
                {
                    sum += wkot.DayTime;
                }
            }
            TimeSpan tt = new TimeSpan(sum);
            return (int)tt.TotalHours+"小时 "+tt.Minutes+"分";
           
        }

        private string getOverTime(WkTUser user, DateTime d1,DateTime d2)//获取两段时间内加班时间  时间在数据库里的格式到底是怎样的？
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
                string tOfDay, tOfMonth;
                tOfDay = getOverTimeOfDay(u, date);
                tOfMonth = getOverTimeOfMonth(u, date);
                ListViewItem item = new ListViewItem();
                //item.Checked = true;
                item.SubItems.Add(u.KuName);
                item.SubItems.Add(tOfDay);
                item.SubItems.Add(tOfMonth);
                item.SubItems.Add(Dept.KdName.Trim());
                item.Tag = u;
                listView1.Items.Add(item);
            }
        }

        

        private void upDateListView2(DateTime t1,DateTime t2,string s)
        {
            listView2.Items.Clear();
            string query = "from WorkOverTime w where w.Date >= " + t1.Ticks + " and w.Date<= " + t2.Ticks + " and w.WorkManId.Kdid.KdName like '%" + s + "%' order by w.Date";
            IList overTimes=baseService.loadEntityList(query);
            int i = 1;
            if(s.Contains("所有部门"))
                s="";
            if (overTimes!=null)
            {
                foreach (WorkOverTime o in overTimes)
                {
                    string overTimeTotal = getOverTime(o.WorkManId, t1, t2);
                    ListViewItem item = new ListViewItem();
                    item.Text = i.ToString();//序号
                    item.SubItems.Add(o.WorkManId.KuName);//姓名
                    item.SubItems.Add((new DateTime(o.Date).ToShortDateString())+"  "+(new DateTime(o.StartTime).ToShortTimeString())+"-"+(new DateTime(o.EndTime).ToShortTimeString()));//时段
                    item.SubItems.Add(o.WorkContent);          //内容     
                    item.SubItems.Add(overTimeTotal.ToString());//总时长
                    item.SubItems.Add(o.WorkManId.Kdid.KdName.Trim());
                    item.SubItems.Add(o.Ku_Id.KuName);//
                    item.Tag = o;
                    listView2.Items.Add(item);
                    i++;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TimeSpan ts=dateTimePicker5.Value.AddSeconds(1).TimeOfDay-dateTimePicker2.Value.TimeOfDay;
            if (ts.Hours > 3)
            {
                MessageBox.Show("超过加班时长限制");
            }
            else
            {
                int count = 0;
                IList userlist = listView1.Items;
                ClassLibrary.WorkOverTime wkot = new WorkOverTime();
                foreach (ListViewItem item in userlist)
                {
                    if (item.Checked == true)
                    {
                        count++;
                        string totaltime=item.SubItems[2].Text;
                        string totalhour = totaltime.Substring(0, totaltime.IndexOf("小时"));
                        string totalmin = totaltime.Substring(totaltime.LastIndexOf("小时")+3, totaltime.IndexOf("分") - totaltime.LastIndexOf("小时")-3);
                        TimeSpan t1=new TimeSpan( Convert.ToInt32(totalhour),Convert.ToInt32(totalmin),0);
                        TimeSpan t2 = dateTimePicker5.Value.AddSeconds(1) - dateTimePicker2.Value;
                        if ((t1 + t2).Hours >= 36)
                        {
                            MessageBox.Show(wkot.Ku_Id.KuName + "本月加班时间不能超过36小时！");
                        }
                        else
                        {
                            wkot.Ku_Id = User;
                            wkot.WorkManId = (WkTUser)item.Tag;
                            wkot.StartTime = dateTimePicker2.Value.Ticks;
                            wkot.EndTime = dateTimePicker5.Value.Ticks;
                            wkot.Date = dateTimePicker1.Value.Date.Ticks;
                            wkot.DayTime = ts.Ticks;
                            wkot.WorkContent = textBox7.Text;
                            wkot.State = (int)WorkOverTime.stateEnum.Normal;
                            baseService.SaveOrUpdateEntity(wkot);
                            MessageBox.Show("提交成功");
                        }
                    }
                    if(count  == 0)
                    {
                        MessageBox.Show("请指定人员！");
                    }
                }
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
                string tOfDay, tOfMonth;
                tOfDay = getOverTimeOfDay(u, date);
                tOfMonth = getOverTimeOfMonth(u, date);
                ListViewItem item = new ListViewItem();
                item.SubItems.Add(u.KuName);
                item.SubItems.Add(tOfDay);
                item.SubItems.Add(tOfMonth);
                item.SubItems.Add(Dept.KdName.Trim());
                item.Tag = u;
                listView1.Items.Add(item);
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            upDateListView1(Dept);
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
            {
                string query = "from WkTDept";
                IList depList = baseService.loadEntityList(query);
                WkTDept dep = new WkTDept();
                dep.KdName = "所有部门";
                depList.Insert(0, dep);
                comboBox2.DataSource = depList;
                comboBox2.DisplayMember = "KdName";
                comboBox2.ValueMember = "Itself";
                upDateListView2(new DateTime(1900,1,1),new DateTime(2099,1,1),"");

            }
        }

       
      

       
        
        
    }
}
