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
    public partial class OnDuty : Form
    {

     
        WkTRole role;
        public WkTRole Role
        {
            get { return role; }
            set { role = value; }
        }



        TimeArrangeForManager TfM;

        private BaseService baseService = new BaseService();

        private List<Label> dateLabel = new List<Label>();
        private WkTUser user;
        public WkTUser User
        {
            get { return user; }
            set { user = value; }
        }

        IList dutyDayLis;

        /// <summary>
        /// 判定角色0是普通人 1是综合办主任 2是科研所主任
        /// </summary>
        private int Therole;

        OpOndutyDateBase opp = new OpOndutyDateBase();
        public OnDuty()
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


        /// <summary>
        /// 窗体关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox8_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 窗体最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox9_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void pictureBox9_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox9.BackgroundImage = WorkLogForm.Properties.Resources.最小化_副本;
        }

        private void pictureBox9_MouseLeave(object sender, EventArgs e)
        {
            pictureBox9.BackgroundImage = WorkLogForm.Properties.Resources.最小化渐变;
        }
       
        
        private void pictureBox8_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox8.BackgroundImage = WorkLogForm.Properties.Resources.关闭渐变_副本;
        }

        private void pictureBox8_MouseLeave(object sender, EventArgs e)
        {
            pictureBox8.BackgroundImage = WorkLogForm.Properties.Resources.关闭渐变;
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

      

        /// <summary>
        /// 窗口加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDuty_Load(object sender, EventArgs e)
        {
            if (role.KrOrder == 3)
            {
                this.button4.Visible = false;
            }
            this.comboBox1.SelectedIndex = 1;


            //查询综合办与网络中心的主任
            Therole = 0;
            string sql = "select u from WkTUser u  left join u.UserRole role where role.KrDESC='工作小秘书角色' and role.KrOrder = 2  and (u.Kdid.KdName like '%科技信息资源研究所%' or u.Kdid.KdName like '%综合办公室%') ";
            IList i = baseService.loadEntityList(sql);
            foreach (WkTUser o in i)
            {
                if (o.Kdid.KdName.Trim() == "科技信息资源研究所")
                {
                    if(user.Id == o.Id)
                    {
                        Therole = 2;
                    }
                }
                else if (o.Kdid.KdName.Trim() == "综合办公室")
                {
                    if (user.Id == o.Id)
                    {
                        Therole = 1;
                    }
                }

            }

            

            loadData();
            this.dateTimePicker1.Value = DateTime.Now;
           
        }
        
        public void PrintDutyPersonName(Panel p, WkTUser DaiBan, WkTUser Baiban, WkTUser Yeban, int Type)
        {
            #region
            if (Type == 0) //行政班
            {
                Label Duser = GetTheLabelByLocation(p, 51, 29);
                if (Duser == null)
                {
                    Duser = new Label();
                    Duser.Text = DaiBan.KuName;
                    Duser.Location = new Point(51, 29);
                    Duser.Font = new Font("微软雅黑", 9);
                    Duser.Parent = p;
                    Duser.ForeColor = DaiBan.Id == user.Id ? Color.Red : Color.Black;
                    Duser.AutoSize = true;
                }
                else
                {
                    Duser.Text = DaiBan.KuName;
                    Duser.ForeColor = DaiBan.Id == user.Id ? Color.Red : Color.Black;

                }

                Label Buser = GetTheLabelByLocation(p, 51, 49);
                if (Buser == null)
                {
                    Buser = new Label();
                    Buser.Text = Baiban.KuName;
                    Buser.Location = new Point(51, 49);
                    Buser.Font = new Font("微软雅黑", 9);
                    Buser.Parent = p;
                    Buser.ForeColor = Baiban.Id == user.Id ? Color.Red : Color.Black;
                    Buser.AutoSize = true;
                }
                else
                {
                    Buser.Text = Baiban.KuName;
                    Buser.ForeColor = Baiban.Id == user.Id ? Color.Red : Color.Black;

                }

                Label Yuser = GetTheLabelByLocation(p, 51, 67);
                if (Yuser == null)
                {
                    Yuser = new Label();
                    Yuser.Text = Yeban.KuName;
                    Yuser.Location = new Point(51, 67);
                    Yuser.Font = new Font("微软雅黑", 9);
                    Yuser.Parent = p;
                    Yuser.ForeColor = Yeban.Id == user.Id ? Color.Red : Color.Black;
                    Yuser.AutoSize = true;
                }
                else
                {
                    Yuser.Text = Yeban.KuName;
                    Yuser.ForeColor = Yeban.Id == user.Id ? Color.Red : Color.Black;
                }

            }
            #endregion

            #region 网络班
            else
            {
                Label Duser = GetTheLabelByLocation(p, 91, 29);
                if (Duser == null)
                {
                    Duser = new Label();
                    Duser.Text = DaiBan.KuName;
                    Duser.Location = new Point(91, 29);
                    Duser.Font = new Font("微软雅黑", 9);
                    Duser.ForeColor = DaiBan.Id == user.Id ? Color.Red : Color.Black;
                    Duser.Parent = p;
                    Duser.AutoSize = true;
                }
                else
                {
                    Duser.Text = DaiBan.KuName;
                    Duser.ForeColor = DaiBan.Id == user.Id ? Color.Red : Color.Black;

                }

                Label Buser = GetTheLabelByLocation(p, 91, 49);
                if (Buser == null)
                {
                    Buser = new Label();
                    Buser.Text = Baiban.KuName;
                    Buser.Location = new Point(91, 49);
                    Buser.Font = new Font("微软雅黑", 9);
                    Buser.Parent = p;
                    Buser.ForeColor = Baiban.Id == user.Id ? Color.Red : Color.Black;
                    Buser.AutoSize = true;
                }
                else
                {
                    Buser.Text = Baiban.KuName;
                    Buser.ForeColor = Baiban.Id == user.Id ? Color.Red : Color.Black;

                }

                Label Yuser = GetTheLabelByLocation(p, 91, 67);
                if (Yuser == null)
                {
                    Yuser = new Label();
                    Yuser.Text = Yeban.KuName;
                    Yuser.Location = new Point(91, 67);
                    Yuser.Font = new Font("微软雅黑", 9);
                    Yuser.Parent = p;
                    Yuser.ForeColor = Yeban.Id == user.Id ? Color.Red : Color.Black;
                    Yuser.AutoSize = true;
                }
                else
                {
                    Yuser.ForeColor = Yeban.Id == user.Id ? Color.Red : Color.Black;
                    Yuser.Text = Yeban.KuName;
                }
            }
            #endregion
        }
      

        /// <summary>
        /// 初始化日历日期
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        private void initCalendar(int year, int month, bool isManager)
        {
            DateTime selectDay = new DateTime(year, month, 1);
            CNDate selectDateTool = new CNDate(selectDay);
            CNDate beforeDateTool = new CNDate(month == 1 ? new DateTime(year - 1, 12, 1) : new DateTime(year, month - 1, 1));
            switch (selectDay.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    initDayOfCalendar(0, selectDateTool.GetDayNumOfMonth(), beforeDateTool.GetDayNumOfMonth(),isManager); break;
                case DayOfWeek.Monday:
                    initDayOfCalendar(1, selectDateTool.GetDayNumOfMonth(), beforeDateTool.GetDayNumOfMonth(),isManager); break;
                case DayOfWeek.Tuesday:
                    initDayOfCalendar(2, selectDateTool.GetDayNumOfMonth(), beforeDateTool.GetDayNumOfMonth(),isManager); break;
                case DayOfWeek.Wednesday:
                    initDayOfCalendar(3, selectDateTool.GetDayNumOfMonth(), beforeDateTool.GetDayNumOfMonth(),isManager); break;
                case DayOfWeek.Thursday:
                    initDayOfCalendar(4, selectDateTool.GetDayNumOfMonth(), beforeDateTool.GetDayNumOfMonth(),isManager); break;
                case DayOfWeek.Friday:
                    initDayOfCalendar(5, selectDateTool.GetDayNumOfMonth(), beforeDateTool.GetDayNumOfMonth(),isManager); break;
                case DayOfWeek.Saturday:
                    initDayOfCalendar(6, selectDateTool.GetDayNumOfMonth(), beforeDateTool.GetDayNumOfMonth(),isManager); break;
            }
        }

        /// <summary>
        /// 初始化日历日期子函数
        /// </summary>
        /// <param name="startDay"></param>
        /// <param name="allDay"></param>
        /// <param name="lastMonthLastDay"></param>
        private void initDayOfCalendar(int startDay, int allDay, int lastMonthLastDay,bool isMannager)
        {
            for (int i = 0; i < dateLabel.Count; i++)
            {
                Panel par = (Panel)dateLabel[i].Parent;
                //par.Controls.
                foreach (Control c in par.Controls)
                {
                    if (c.GetType() == typeof(LinkLabel))
                    {
                        par.Controls.Remove(c);
                        break;
                    }
                }

                if (i < startDay)
                {
                    dateLabel[i].Text = (lastMonthLastDay - (startDay - i) + 1).ToString();
                    dateLabel[i].Parent.ForeColor = SystemColors.ControlDark;
                    
                    if (this.dateTimePicker1.Value.Month != 1)
                    {
                        
                        dateLabel[i].Parent.Tag = new DateTime(this.dateTimePicker1.Value.Year, this.dateTimePicker1.Value.Month - 1, lastMonthLastDay - (startDay - i) + 1);
                    }

                    else
                    {
                        dateLabel[i].Parent.Tag = new DateTime(this.dateTimePicker1.Value.Year - 1, 12, lastMonthLastDay - (startDay - i) + 1);
                    }
                }
                else if (i >= (startDay + allDay))
                {
                    dateLabel[i].Text = (i - (startDay + allDay) + 1).ToString();
                    dateLabel[i].Parent.ForeColor = SystemColors.ControlDark;
                    if (this.dateTimePicker1.Value.Month != 12)
                    {
                        dateLabel[i].Parent.Tag = new DateTime(this.dateTimePicker1.Value.Year, this.dateTimePicker1.Value.Month + 1, i - (startDay + allDay) + 1);
                    }
                    else
                    {
                        dateLabel[i].Parent.Tag = new DateTime(this.dateTimePicker1.Value.Year + 1, 1, i - (startDay + allDay) + 1);
                    }

                }
                else
                {
                    dateLabel[i].Text = (i - startDay + 1).ToString();
                    dateLabel[i].Parent.ForeColor = SystemColors.ControlText;
                    dateLabel[i].Parent.Tag = new DateTime(this.dateTimePicker1.Value.Year,this.dateTimePicker1.Value.Month, i - startDay + 1);
                    if (isMannager)
                    {
                        LinkLabel panban = new LinkLabel();
                        panban.ActiveLinkColor = Color.Yellow;
                        panban.Font = new Font("微软雅黑", 9);
                        panban.Location = new Point(5, 67);
                        panban.Parent = dateLabel[i].Parent;
                        panban.Text = "排班";
                        panban.AutoSize = true;
                        panban.Click += panban_Click;
                    }
                }


                //加载人员
                DateTime dt = (DateTime)dateLabel[i].Parent.Tag;
                List<OnDutyTable> dayInfo = IsTheTimeIndutyDayLis(dt);
                if (dayInfo !=null)
                {
                    foreach (OnDutyTable oo in dayInfo)
                    {
                        if(oo.Type == 0)
                        {
                            PrintDutyPersonName((Panel)dateLabel[i].Parent, oo.DaiBanID, oo.BaiBanID, oo.YeBanID, 0);
                        }
                        else if (oo.Type == 1)
                        {
                            PrintDutyPersonName((Panel)dateLabel[i].Parent, oo.DaiBanID, oo.BaiBanID, oo.YeBanID, 1);
                        }
                    
                    }
                
                }
            }

        }

        public List<OnDutyTable> IsTheTimeIndutyDayLis(DateTime dt)
        {
            List<OnDutyTable> dayInfo = null;
            if (dutyDayLis != null && dutyDayLis.Count>0)
            {
                foreach (OnDutyTable o in dutyDayLis)
                {
                    if(o.Time == dt.Ticks)
                    {
                        dayInfo = new List<OnDutyTable>();
                        dayInfo.Add(o);
                    }
                }
            }

            return dayInfo;
        }


        /// <summary>
        /// 点击排班的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void panban_Click(object sender, EventArgs e)
        {
            ArrangeDuty ad = new ArrangeDuty();
            ad.ShowDialog();
            if (ad.DialogResult == DialogResult.OK)
            {
                if (TfM != null)
                {
                    OnDutyTable ot;
                    DateTime dt = new DateTime(((DateTime)((((LinkLabel)sender).Parent).Tag)).Ticks);
                    string sql = "select u from OnDutyTable u where u.Time = " + dt.Ticks +
                        " and u.State = " + (int)IEntity.stateEnum.Normal;
                    IList i = baseService.loadEntityList(sql);
                    if (i != null && i.Count > 0)
                    {
                        ot = (OnDutyTable)i[0];
                    }
                    else
                    {
                        ot = new OnDutyTable();

                    }
                    ot.TFMId = TfM;
                    ot.Time = dt.Ticks;
                    ot.DaiBanID = ad.Duser;
                    ot.BaiBanID = ad.Buser;
                    ot.YeBanID = ad.Yuser;
                    ot.State = (int)IEntity.stateEnum.Normal;
                    ot.TimeStamp = DateTime.Now.Ticks;
                    TfM.IsDone = 1;

                    TfM.ExamineState = 0;
                    this.CheckState.Text = "审核状态：未审核";

                    baseService.SaveOrUpdateEntity(TfM);

                    #region 行政班
                    if (TfM.DutyType == 0) //行政班
                    {
                        PrintDutyPersonName((Panel)((LinkLabel)sender).Parent, ad.Duser, ad.Buser, ad.Yuser, 0);
                        ot.Type = 0;
                    }
                    #endregion

                    #region 网络班
                    else
                    {

                        PrintDutyPersonName((Panel)((LinkLabel)sender).Parent, ad.Duser, ad.Buser, ad.Yuser, 1);
                        ot.Type = 1;
                    }

                    baseService.SaveOrUpdateEntity(ot);

                    #endregion

                }

            }

        }

        public Label GetTheLabelByLocation(Panel p , int x ,int y)
        {
            Label l = null;
            foreach (Control c in p.Controls)
            {
                if(c.Location.X == x && c.Location.Y == y)
                {
                    l = (Label)c;
                    return l;
                }
            }
            return l;
        }

        private void loadData()
        {
            dateLabel.Add(label111); dateLabel.Add(label122); dateLabel.Add(label133); dateLabel.Add(label144);
            dateLabel.Add(label155); dateLabel.Add(label166); dateLabel.Add(label177);
            
            dateLabel.Add(label211); dateLabel.Add(label222); dateLabel.Add(label233); dateLabel.Add(label244); 
            dateLabel.Add(label255); dateLabel.Add(label266); dateLabel.Add(label277); 
            
            dateLabel.Add(label311); dateLabel.Add(label322); dateLabel.Add(label333); dateLabel.Add(label344); 
            dateLabel.Add(label355); dateLabel.Add(label366); dateLabel.Add(label377); 
            
            dateLabel.Add(label411); dateLabel.Add(label422); dateLabel.Add(label433); dateLabel.Add(label444);
            dateLabel.Add(label455); dateLabel.Add(label466); dateLabel.Add(label477);
            
            dateLabel.Add(label511); dateLabel.Add(label522); dateLabel.Add(label533); dateLabel.Add(label544);
            dateLabel.Add(label555); dateLabel.Add(label566); dateLabel.Add(label577); 
            
            dateLabel.Add(label611); dateLabel.Add(label622); dateLabel.Add(label633); dateLabel.Add(label644); 
            dateLabel.Add(label655); dateLabel.Add(label666); dateLabel.Add(label677);

        }


        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

            DateTime tt = new DateTime(this.dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, 1);

            string sql1 = "Select u from OnDutyTable u where u.State = "+(int)IEntity.stateEnum.Normal+
                " and u.Time > " + tt.AddDays(-16).Ticks+
                " and u.Time < " + tt.AddDays(16).Ticks;
            dutyDayLis = baseService.loadEntityList(sql1);

            dateTimePicker1.Cursor = Cursors.WaitCursor;
            if (this.dateTimePicker1.Value.Ticks > DateTime.Now.Ticks)
            {
               
                //查询这个月是否自己排班

                string sql = "select u from TimeArrangeForManager u where u.UserId = " + user.Id +
                    " and u.TimeMonth = " + tt.Ticks +
                    " and u.State = " + (int)IEntity.stateEnum.Normal;

                IList timemananer = baseService.loadEntityList(sql);

                if (timemananer != null && timemananer.Count > 0)
                {
                    TfM = new TimeArrangeForManager();
                    TfM = (TimeArrangeForManager)timemananer[0];

                    this.initCalendar(this.dateTimePicker1.Value.Year, this.dateTimePicker1.Value.Month,true);
                    switch (TfM.ExamineState)
                    {
                        case 0:
                            this.CheckState.Text = "审核状态：未审核"; break;
                        case 1:
                            this.CheckState.Text = "审核状态：审核通过"; break;
                        case 2:
                            this.CheckState.Text = "审核状态：审核未通过"; break;
                    }
                }
                else
                {
                    this.initCalendar(this.dateTimePicker1.Value.Year, this.dateTimePicker1.Value.Month, false);
                    this.CheckState.Text = "";
                }

                if (Therole != 0)
                {
                    this.PanelOfTwoButtons.Visible = true;
                }

            }
            else 
            {
                this.initCalendar(this.dateTimePicker1.Value.Year, this.dateTimePicker1.Value.Month,false);
                this.CheckState.Text = "";
                this.PanelOfTwoButtons.Visible = false;
            }

            dateTimePicker1.Cursor = Cursors.Hand;
        }


        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            DateTime tt = new DateTime(this.dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, 1);
            string sql = "select u from TimeArrangeForManager u where " +
             "  u.TimeMonth = " + tt.Ticks +
             " and u.State = " + (int)IEntity.stateEnum.Normal + 
             " and u.IsDone = 1 ";

            IList timemananer = baseService.loadEntityList(sql);
             if(timemananer != null && timemananer.Count > 0)
             {
                 TimeArrangeForManager timeman = (TimeArrangeForManager)timemananer[0];
                 if (timeman.ExamineState == 0)
                 {
                     if (MessageBox.Show("确定要通过吗？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                     {

                         timeman.ExamineState = 1;
                         baseService.SaveOrUpdateEntity(timeman);

                         this.CheckState.Text = "审核状态：审核通过";
                     }
                 }
                 else
                 {
                     MessageBox.Show("您已经审核过了");
                 }
                
             }
               
            

        }

        /// <summary>
        /// 审核不通过
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            DateTime tt = new DateTime(this.dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, 1);
            string sql = "select u from TimeArrangeForManager u where " +
             "  u.TimeMonth = " + tt.Ticks +
             " and u.State = " + (int)IEntity.stateEnum.Normal +
             " and u.IsDone = 1 ";

            IList timemananer = baseService.loadEntityList(sql);
            if (timemananer != null && timemananer.Count > 0)
            {
                TimeArrangeForManager timeman = (TimeArrangeForManager)timemananer[0];
                if (timeman.ExamineState == 0)
                {
                    if (MessageBox.Show("确定要不通过吗？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {

                        timeman.ExamineState = 2;
                        baseService.SaveOrUpdateEntity(timeman);

                        this.CheckState.Text = "审核状态：审核不通过";
                    }
                }
                else
                {
                    MessageBox.Show("您已经审核过了");
                }

            }
        }
        /*-----------------值班统计---------------------------*/

        #region 统计自己
        /// <summary>
        /// 统计自己查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            this.button5.Cursor = Cursors.WaitCursor;

            string sql0 = "select u from OnDutyTable u left join u.DaiBanID user where user.Id = " + user.Id +
                " and u.Time >= "+this.dateTimePicker2.Value.Ticks + " and u.Time <= " +dateTimePicker3.Value.Ticks+ 
                " u.State = "+(int)IEntity.stateEnum.Normal;
            string sql1 = "select u from OnDutyTable u left join u.BaiBanID user where user.Id = " + user.Id +
                " and u.Time >= " + this.dateTimePicker2.Value.Ticks + " and u.Time <= " + dateTimePicker3.Value.Ticks +
                " u.State = " + (int)IEntity.stateEnum.Normal;
            string sql2 = "select u from OnDutyTable u left join u.YeBanID user where user.Id = " + user.Id +
                " and u.Time >= " + this.dateTimePicker2.Value.Ticks + " and u.Time <= " + dateTimePicker3.Value.Ticks +
                " u.State = " + (int)IEntity.stateEnum.Normal;
            string sql = "";
            
            switch (this.comboBox1.SelectedIndex)
            {
                case 0:
                    sql = sql0;break;
                case 1:
                    sql = sql1;break;
                case 2:
                    sql = sql2;break;
            }
            if (sql != "")
            {
                IList times = baseService.loadEntityList(sql);
                if(times!= null && times.Count > 0)
                {
                    this.dataGridView1.Rows.Clear();
                    int i = 1;
                    foreach (OnDutyTable o in times)
                    {
                        DateTime dt = new DateTime(o.Time);
                        this.dataGridView1.Rows.Add(i, dt.ToString("yyyy年MM月dd日") +"  "+EnglishToChinese(dt), 
                            this.comboBox1.Text,
                            user.KuName);
                        i++;
                    }
                
                }
            }

            this.button5.Cursor = Cursors.Hand;
        }
        //Monday Tuesday Wednesday Thursday  Friday Saturday  Sunday
        public string EnglishToChinese(DateTime dt)
        {
            string weekday = dt.DayOfWeek.ToString();
           
            if (dt.DayOfWeek.ToString() == "Monday")
            {
                weekday = "星期一";
            }
            else if (dt.DayOfWeek.ToString() == "Tuesday")
            {
                weekday = "星期二";
            }
            else if (dt.DayOfWeek.ToString() == "Wednesday")
            {
                weekday = "星期三";
            }
            else if (dt.DayOfWeek.ToString() == "Thursday")
            {
                weekday = "星期四";
            }
            else if (dt.DayOfWeek.ToString() == "Friday")
            {
                weekday = "星期五";
            }
            else if (dt.DayOfWeek.ToString() == "Saturday")
            {
                weekday = "星期六";
            }
            else if (dt.DayOfWeek.ToString() == "Sunday")
            {
                weekday = "星期日";
            }


            return weekday;
        }
        #endregion
        /// <summary>
        /// 统计自己
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            this.panel1.Visible = true;
            this.panel2.Visible = false;
            
        }

        /// <summary>
        /// 统计员工
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            this.button4.Cursor = Cursors.WaitCursor;

            this.panel2.Visible = true;
            this.panel1.Visible = false;




            this.button4.Cursor = Cursors.Hand;
        }


    }
}
