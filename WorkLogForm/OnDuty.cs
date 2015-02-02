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

        IList Ondutys;
        IList<WkTRole> userrole;

        WkTRole role;
        public WkTRole Role
        {
            get { return role; }
            set { role = value; }
        }

        private BaseService baseService = new BaseService();

        private List<Label> dateLabel = new List<Label>();
        private WkTUser user;
        public WkTUser User
        {
            get { return user; }
            set { user = value; }
        }
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
        
        private void pictureBox9_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox9.BackgroundImage = WorkLogForm.Properties.Resources.最小化_副本;
        }

        private void pictureBox9_MouseLeave(object sender, EventArgs e)
        {
            pictureBox9.BackgroundImage = WorkLogForm.Properties.Resources.最小化渐变;
        }
       
        private void min_pictureBox_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
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

        private void panel_Click(object sender, EventArgs e)
        {
            //OnDutyAdd.Visible = true;
        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox46_Click(object sender, EventArgs e)
        {
            OnDutyAdd.Visible = false;
        }
       
        private void pictureBox8_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label122_Click(object sender, EventArgs e)
        {
            //OnDutyAdd.Visible = true;
        }


        /// <summary>
        /// 确定按钮安排值班人员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            //保存信息


            for (int i = 0; i < this.dataGridView1.Rows.Count; i++ )
            {
                //判断打勾的
                if( Convert.ToBoolean(this.dataGridView1.Rows[i].Cells[0].Value) )
                {
                    ClassLibrary.OnDuty theduty = new ClassLibrary.OnDuty();
                    theduty.Ku_Id = this.User;
                    theduty.Staff_Id = this.dataGridView1.Rows[i].Tag as WkTUser;
                    theduty.Time = ((DateTime)this.labelX1.Tag).Date.Ticks;
                    //theduty.Times = theduty.Times + 1;
                    if(Convert.ToBoolean(this.dataGridView1.Rows[i].Cells[4].Value))
                    {
                        theduty.Type = (int)ClassLibrary.OnDuty.DutyType.day;
                    }
                    else if(Convert.ToBoolean(this.dataGridView1.Rows[i].Cells[5].Value))
                    {
                        theduty.Type = (int)ClassLibrary.OnDuty.DutyType.night;
                    }
                    
                    opp.SaveOrUpdateEntity(theduty);


                    WkTUser thuser = this.dataGridView1.Rows[i].Tag as WkTUser;
                    thuser.DutyTimes = thuser.DutyTimes + 1;
                    opp.SaveOrUpdateEntity(thuser);


                }

            }
            
            this.Visible = false;
            MessageBox.Show("值班安排信息保存成功！");
        }



        private void pictureBox9_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        /// <summary>
        /// 窗口加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDuty_Load(object sender, EventArgs e)
        {
            
            OnDutyAdd.Visible = false;
            OndutyaddForYuanZhang.Visible = false;
            ShowDuty.Visible = false;


            loadData();

            month_comboBoxEx.SelectedIndex = DateTime.Now.Month - 1;
            year_comboBoxEx.SelectedIndex = DateTime.Now.Year - 2012;


            this.button3.Visible = false;
            this.button5.Visible = false;

            //判断用登陆角色户
             userrole = User.UserRole;

            DevComponents.Editors.ComboItem yearItem = (DevComponents.Editors.ComboItem)year_comboBoxEx.SelectedItem;
            DevComponents.Editors.ComboItem monthItem = (DevComponents.Editors.ComboItem)month_comboBoxEx.SelectedItem;
            DateTime dt = new DateTime(Convert.ToInt32(yearItem.Text), Convert.ToInt32(monthItem.Text), 1);

            foreach(WkTRole o in userrole)
            {
                if(o.KrOrder == 0 || o.KrOrder == 1) //院长副院长登陆
                {
                    Ondutys = opp.GetMonthDuty(dt.Ticks, dt.Date.AddMonths(1).Ticks);
                    //赋予不同功能
                    SetLabel();
                    this.button3.Visible = true;
                    
                }
                else if (o.KrOrder == 2) //负责人登陆
                {
                    Ondutys = opp.GetMonthDuty(dt.Ticks, dt.Date.AddMonths(1).Ticks);
                    //赋予不同功能
                    SetLabel();
                    AddeventToAlable(dt.Ticks, dt.Date.AddMonths(1).Ticks);
                }
                else if (o.KrOrder == 3) //员工登陆
                {
                    this.button5.Visible = true;
                }
            }

           
        }


        public void SetLabel()
        {

            for (int i = 0; i < dateLabel.Count; i++)
            {
                dateLabel[i].Parent.Controls[0].Text = "";
                dateLabel[i].Parent.Controls[1].Text = "";
            }



            if(Ondutys != null && Ondutys.Count >= 0)
            {
                foreach(ClassLibrary.OnDuty o in Ondutys)
                {
                   for (int i = 0; i < dateLabel.Count; i++)
                    {
                        if (o.Time == ((DateTime)dateLabel[i].Parent.Tag).Date.Ticks)
                        {
                            dateLabel[i].Parent.Controls[0].Text = o.Ku_Id.KuName;
                            dateLabel[i].Parent.Controls[1].Text = dateLabel[i].Parent.Controls[1].Text + "/" + o.Staff_Id.KuName;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 添加事件
        /// </summary>
        public void AddeventToAlable(long t1,long t2)
        {

            for (int i = 0; i < this.dateLabel.Count; i++)
            {
                dateLabel[i].Click += null;
                //dateLabel[i].ForeColor = Color.Black;
            }


            IList times = opp.SelectManagerTime(this.User, t1, t2);

            for (int i = 0; i < this.dateLabel.Count; i++)
            {
                 
                //foreach(TimeArrangeForManager o in times)
                //{
                //    DateTime dt = (DateTime)dateLabel[i].Parent.Tag;
                //    if (dateLabel[i].Parent.ForeColor != SystemColors.ControlDark && dt.Date.Ticks >= o.Startime && dt.Date.Ticks < o.Endtime && dateLabel[i].Parent.Controls[1].Text == "")
                //    {
                //        dateLabel[i].Click += OnDuty_Click;
                //        dateLabel[i].ForeColor = Color.Red;
                //    }
                //}
                
            }
        }


        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnDuty_Click(object sender, EventArgs e)
        {
        
            OnDutyAdd.Visible = true;
            System.Windows.Forms.Label thelab = (System.Windows.Forms.Label)sender;
            DateTime thetime = (DateTime) thelab.Parent.Tag;
            this.labelX1.Text = thetime.ToString("yyyy年MM月dd日");
            this.labelX1.Tag = thetime;
        
        }

        /// <summary>
        /// 初始化日历日期
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        private void initCalendar(int year, int month)
        {
            DateTime selectDay = new DateTime(year, month, 1);
            CNDate selectDateTool = new CNDate(selectDay);
            CNDate beforeDateTool = new CNDate(month == 1 ? new DateTime(year - 1, 12, 1) : new DateTime(year, month - 1, 1));
            switch (selectDay.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    initDayOfCalendar(0, selectDateTool.GetDayNumOfMonth(), beforeDateTool.GetDayNumOfMonth()); break;
                case DayOfWeek.Monday:
                    initDayOfCalendar(1, selectDateTool.GetDayNumOfMonth(), beforeDateTool.GetDayNumOfMonth()); break;
                case DayOfWeek.Tuesday:
                    initDayOfCalendar(2, selectDateTool.GetDayNumOfMonth(), beforeDateTool.GetDayNumOfMonth()); break;
                case DayOfWeek.Wednesday:
                    initDayOfCalendar(3, selectDateTool.GetDayNumOfMonth(), beforeDateTool.GetDayNumOfMonth()); break;
                case DayOfWeek.Thursday:
                    initDayOfCalendar(4, selectDateTool.GetDayNumOfMonth(), beforeDateTool.GetDayNumOfMonth()); break;
                case DayOfWeek.Friday:
                    initDayOfCalendar(5, selectDateTool.GetDayNumOfMonth(), beforeDateTool.GetDayNumOfMonth()); break;
                case DayOfWeek.Saturday:
                    initDayOfCalendar(6, selectDateTool.GetDayNumOfMonth(), beforeDateTool.GetDayNumOfMonth()); break;
            }
        }

        /// <summary>
        /// 初始化日历日期子函数
        /// </summary>
        /// <param name="startDay"></param>
        /// <param name="allDay"></param>
        /// <param name="lastMonthLastDay"></param>
        private void initDayOfCalendar(int startDay, int allDay, int lastMonthLastDay)
        {
            for (int i = 0; i < dateLabel.Count; i++)
            {
                if (i < startDay)
                {
                    dateLabel[i].Text = (lastMonthLastDay - (startDay - i) + 1).ToString();
                    dateLabel[i].Parent.ForeColor = SystemColors.ControlDark;

                    if (!((DevComponents.Editors.ComboItem)month_comboBoxEx.SelectedItem).Text.Equals("01"))
                    {
                        dateLabel[i].Parent.Tag = new DateTime(Convert.ToInt32(year_comboBoxEx.Text), Convert.ToInt32(((DevComponents.Editors.ComboItem)month_comboBoxEx.SelectedItem).Text) - 1, lastMonthLastDay - (startDay - i) + 1);
                    }
                    else
                    {
                        dateLabel[i].Parent.Tag = new DateTime(Convert.ToInt32(year_comboBoxEx.Text) - 1, 12, lastMonthLastDay - (startDay - i) + 1);
                    }
                }
                else if (i >= (startDay + allDay))
                {
                    dateLabel[i].Text = (i - (startDay + allDay) + 1).ToString();
                    dateLabel[i].Parent.ForeColor = SystemColors.ControlDark;
                    if (!((DevComponents.Editors.ComboItem)month_comboBoxEx.SelectedItem).Text.Equals("12"))
                    {
                        dateLabel[i].Parent.Tag = new DateTime(Convert.ToInt32(year_comboBoxEx.Text), Convert.ToInt32(((DevComponents.Editors.ComboItem)month_comboBoxEx.SelectedItem).Text) + 1, i - (startDay + allDay) + 1);
                    }
                    else
                    {
                        dateLabel[i].Parent.Tag = new DateTime(Convert.ToInt32(year_comboBoxEx.Text) + 1, 1, i - (startDay + allDay) + 1);
                    }

                }
                else
                {
                    dateLabel[i].Text = (i - startDay + 1).ToString();
                    dateLabel[i].Parent.ForeColor = SystemColors.ControlText;
                    dateLabel[i].Parent.Tag = new DateTime(Convert.ToInt32(year_comboBoxEx.Text), Convert.ToInt32(((DevComponents.Editors.ComboItem)month_comboBoxEx.SelectedItem).Text), i - startDay + 1);
                }
            }

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

        private void yearAndMonth_comboBoxEx_SelectedIndexChanged(object sender, EventArgs e)
        {
            DevComponents.Editors.ComboItem yearItem = (DevComponents.Editors.ComboItem)year_comboBoxEx.SelectedItem;
            DevComponents.Editors.ComboItem monthItem = (DevComponents.Editors.ComboItem)month_comboBoxEx.SelectedItem;
            if (yearItem != null && monthItem != null && yearItem.Text != "" && monthItem.Text != "")
            {

                this.initCalendar(Convert.ToInt32(yearItem.Text), Convert.ToInt32(monthItem.Text));

                DateTime dt = new DateTime(Convert.ToInt32(yearItem.Text), Convert.ToInt32(monthItem.Text), 1);

                userrole = User.UserRole;
                foreach (WkTRole o in userrole)
                {
                    if (o.KrOrder == 0 || o.KrOrder == 1) //院长副院长登陆
                    {
                        Ondutys = opp.GetMonthDuty(dt.Ticks, dt.Date.AddMonths(1).Ticks);
                        SetLabel();
                    }
                    else if (o.KrOrder == 2) //负责人登陆
                    {
                        Ondutys = opp.GetMonthDuty(dt.Ticks, dt.Date.AddMonths(1).Ticks);
                        SetLabel();
                        AddeventToAlable(dt.Ticks, dt.Date.AddMonths(1).Ticks);
                    }
                    else if (o.KrOrder == 3) //员工登陆
                    {
                        Ondutys = opp.GetMonthDuty(dt.Ticks, dt.Date.AddMonths(1).Ticks);
                        SetLabel();
                    }
                }
                //AddeventToAlable(dt.Ticks, dt.Date.AddMonths(1).Ticks);
                //Ondutys = opp.GetMonthDuty(dt.Ticks, dt.Date.AddMonths(1).Ticks);
                //赋予不同功能
                //SetLabel();
            }
        }


        /// <summary>
        /// 院长控制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            OndutyaddForYuanZhang.Visible = true;
            OndutyaddForYuanZhang.BringToFront();
            OnDutyAdd.Visible = true;
            this.dataGridView2.Rows.Clear();
        }



        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            this.dataGridView1.Rows.Clear();
            
            if(this.comboBox2.Text != "")
            {
                WkTDept select = (WkTDept)this.comboBox2.SelectedValue;
                IList users = opp.GetSelectYuanGong(this.textBox7.Text, select);
                if(users != null)
                {
                    foreach (WkTUser o in users)
                    {
                        this.dataGridView1.Rows.Add(false, o.KuName,o.Kdid.KdName, o.DutyTimes, false, false);
                        this.dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Tag = o;
                    }
                }
            }
        }


       


        /// <summary>
        /// 下拉事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox2_DropDown(object sender, EventArgs e)
        {



        }

        /// <summary>
        /// 显示框点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            #region
            if (this.dataGridView1.CurrentCell.ColumnIndex == 0)
            {

               if(Convert.ToBoolean(this.dataGridView1.CurrentRow.Cells[0].EditedFormattedValue) == true)
               {
                   this.dataGridView1.CurrentRow.Cells[4].ReadOnly = false;
                   this.dataGridView1.CurrentRow.Cells[5].ReadOnly = false;

                   this.dataGridView1.CurrentRow.Cells[4].Value = true;
               }

               if(Convert.ToBoolean(this.dataGridView1.CurrentRow.Cells[0].EditedFormattedValue) == false)
               {
                   this.dataGridView1.CurrentRow.Cells[4].Value = false;
                   this.dataGridView1.CurrentRow.Cells[5].Value = false;
               
                   this.dataGridView1.CurrentRow.Cells[4].ReadOnly = true;
                   this.dataGridView1.CurrentRow.Cells[5].ReadOnly = true;
               }
            }
            #endregion

            if (e.ColumnIndex == 4 || e.ColumnIndex == 5)
            {
                if (Convert.ToBoolean(this.dataGridView1.CurrentRow.Cells[4].EditedFormattedValue) == true)
                {
                    this.dataGridView1.CurrentRow.Cells[5].Value = false;
                }
                if (Convert.ToBoolean(this.dataGridView1.CurrentRow.Cells[5].EditedFormattedValue) == true)
                {
                    this.dataGridView1.CurrentRow.Cells[4].Value = false;
                }

            }

        }

        /// <summary>
        /// 院长显示 关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox10_Click(object sender, EventArgs e)
        {
            this.OndutyaddForYuanZhang.Visible = false;
            this.OnDutyAdd.Visible = false;
        }

        /// <summary>
        /// 员工查看关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox11_Click(object sender, EventArgs e)
        {
            this.OnDutyAdd.Visible = false;
            this.OndutyaddForYuanZhang.Visible = false;
            this.ShowDuty.Visible = false;
        }

        /// <summary>
        /// 查看自己
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            this.OnDutyAdd.Visible = true;
            this.OndutyaddForYuanZhang.Visible = true;
            this.ShowDuty.Visible = true;
        }



        /// <summary>
        /// 员工值班信息界面显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowDuty_VisibleChanged(object sender, EventArgs e)
        {
            if(this.ShowDuty.Visible == true)
            {
                DevComponents.Editors.ComboItem yearItem = (DevComponents.Editors.ComboItem)year_comboBoxEx.SelectedItem;
                DevComponents.Editors.ComboItem monthItem = (DevComponents.Editors.ComboItem)month_comboBoxEx.SelectedItem;
                DateTime dt = new DateTime(Convert.ToInt32(yearItem.Text), Convert.ToInt32(monthItem.Text), 1);
                IList myduty = opp.GetMonthDuty(dt.Ticks, dt.Date.AddMonths(1).Ticks);

                
                foreach(ClassLibrary.OnDuty o in myduty)
                {
                    if(o.Staff_Id == this.User)
                    {
                        this.ShowDutyGridView.Rows.Add(new DateTime(o.Time).ToString("yyyy-MM-dd"), o.Type == (int)ClassLibrary.OnDuty.DutyType.day ? "白班" : "夜班");
                    }
                }
            }
        }




        /// <summary>
        /// 院长控制界面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OndutyaddForYuanZhang_VisibleChanged(object sender, EventArgs e)
        {
            if (this.OndutyaddForYuanZhang.Visible == true && this.ShowDuty.Visible == false)
            {
                IList depts = opp.GetAllDept();
                if (depts != null && depts.Count > 0)
                {
                    this.comboBox1.DataSource = depts;
                    this.comboBox1.DisplayMember = "KdName";
                    this.comboBox1.ValueMember = "Itself";
                }
            }
        }

        /// <summary>
        /// 当ondutyadd显示的时候
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDutyAdd_VisibleChanged(object sender, EventArgs e)
        {
            if (OnDutyAdd.Visible == true && false == this.ShowDuty.Visible && false == this.OndutyaddForYuanZhang.Visible)
            {
                this.dataGridView1.Rows.Clear();
                IList depts = opp.GetAllDept();
                if (depts != null && depts.Count > 0)
                {
                    this.comboBox2.DataSource = depts;
                    this.comboBox2.DisplayMember = "KdName";
                    this.comboBox2.ValueMember = "Itself";
                }
            }
        }



        /// <summary>
        /// 加载负责人下拉选项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //部门已经选择
            if (this.comboBox1.SelectedValue != null)
            {

                IList deptleaders = opp.GetDeptLeader((WkTDept)this.comboBox1.SelectedValue);
                if (deptleaders != null && deptleaders.Count > 0)
                {
                    this.comboBox3.DataSource = deptleaders;
                    this.comboBox3.DisplayMember = "kuName";
                    this.comboBox3.ValueMember = "Itself";
                }

            }
            else
            {
                MessageBox.Show("您还没选择部门");

            }

        }





        /// <summary>
        /// 添加时间段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonX1_Click(object sender, EventArgs e)
        {
            
            //起始时间要小于截止时间

            if (this.dateTimePicker1.Value.Ticks > this.dateTimePicker2.Value.Ticks)
            {
                MessageBox.Show("起始时间要小于截至时间！");
            }
            else
            {
                
                //当前选择的没有安排 
                if (!opp.IsSurInSheetManager( this.dateTimePicker1.Value.Date.Ticks, this.dateTimePicker2.Value.Date.Ticks))
                {
                    //插入到grid中
                    this.dataGridView2.Rows.Add(new DateTime(this.dateTimePicker1.Value.Ticks).ToString("yyyy-MM-dd"),
                        new DateTime(this.dateTimePicker2.Value.Ticks).ToString("yyyy-MM-dd"),
                        ((WkTDept)this.comboBox1.SelectedValue).KdName,
                        ((WkTUser)this.comboBox3.SelectedValue).KuName,
                        "删除");

                    this.dataGridView2.Rows[this.dataGridView2.Rows.Count - 1].Tag = (WkTUser)this.comboBox3.SelectedValue;
                }
                else
                {
                    MessageBox.Show("这段时间已经安排过了，或者有重合！");
                }
                
            }

        }


        /// <summary>
        /// 向aarrangetimeformanager
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonX2_Click(object sender, EventArgs e)
        {
            for(int i = 0 ; i < this.dataGridView2.Rows.Count;i++)
            {
              
                
                String strst = this.dataGridView2.Rows[i].Cells[0].Value.ToString();
                string[] sst = strst.Split(new char[] { '-' });
                String stret = this.dataGridView2.Rows[i].Cells[1].Value.ToString();
                string[] set = stret.Split(new char[] { '-' });


                //TimeArrangeForManager tafm = new TimeArrangeForManager();
                //tafm.Startime = new DateTime(int.Parse(sst[0]), int.Parse(sst[1]), int.Parse(sst[2])).Ticks;
                //tafm.Endtime = new DateTime(int.Parse(set[0]), int.Parse(set[1]), int.Parse(set[2])).AddDays(-1).Ticks;
                //tafm.UserId = this.dataGridView2.Rows[i].Tag as WkTUser;
               
                //opp.SaveOrUpdateEntity(tafm);

            }

            this.OndutyaddForYuanZhang.Visible = false;
            this.OnDutyAdd.Visible = false;
        }


        /// <summary>
        /// 点击删除按钮时候
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 4 && this.dataGridView2.CurrentCell.Value.ToString() == "删除")
            {
                this.dataGridView2.Rows.Remove(this.dataGridView2.CurrentRow);
            }
        }

        
       
        
    }
}
