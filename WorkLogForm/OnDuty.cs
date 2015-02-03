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

      

       
       



     


     

        /// <summary>
        /// 窗口加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDuty_Load(object sender, EventArgs e)
        {
            
          
         

            loadData();

            month_comboBoxEx.SelectedIndex = DateTime.Now.Month - 1;
            year_comboBoxEx.SelectedIndex = DateTime.Now.Year - 2012;


          
           

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
                    //this.button3.Visible = true;
                    
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
                        //if (o.Time == ((DateTime)dateLabel[i].Parent.Tag).Date.Ticks)
                        //{
                           // dateLabel[i].Parent.Controls[0].Text = o.Ku_Id.KuName;
                            //dateLabel[i].Parent.Controls[1].Text = dateLabel[i].Parent.Controls[1].Text + "/" + o.Staff_Id.KuName;
                       // }
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

       


      




       


   

     






       // DevComponents.Editors.ComboItem yearItem = (DevComponents.Editors.ComboItem)year_comboBoxEx.SelectedItem;
        //DevComponents.Editors.ComboItem monthItem = (DevComponents.Editors.ComboItem)month_comboBoxEx.SelectedItem;
       // DateTime dt = new DateTime(Convert.ToInt32(yearItem.Text), Convert.ToInt32(monthItem.Text), 1);


       
        
    }
}
