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
            loadData();
            this.dateTimePicker1.Value = DateTime.Now;
           
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
            }

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
            if(ad.DialogResult == DialogResult.OK)
            {
                if (TfM != null)
                {
                    if(TfM.IsDone == 0)
                    {
                        TfM.IsDone = 1;
                    }

                    #region 行政班
                    if (TfM.DutyType == 0) //行政班
                    {
                        Label Duser = GetTheLabelByLocation((Panel)((LinkLabel)sender).Parent, 51, 29);
                        if (  Duser == null)
                        {
                            Duser = new Label();
                            Duser.Text = ad.Duser.KuName;
                            Duser.Location = new Point(51, 29);
                            Duser.Font = new Font("微软雅黑", 9);
                            Duser.Parent = ((LinkLabel)sender).Parent;
                            Duser.ForeColor = ad.Duser.Id == user.Id ? Color.Red : Color.Black;
                            Duser.AutoSize = true;
                        }
                        else
                        {
                            Duser.Text = ad.Duser.KuName;
                            Duser.ForeColor = ad.Duser.Id == user.Id ? Color.Red : Color.Black;

                        }

                        Label Buser = GetTheLabelByLocation((Panel)((LinkLabel)sender).Parent, 51, 49);
                        if (Buser == null)
                        {
                            Buser = new Label();
                            Buser.Text = ad.Buser.KuName;
                            Buser.Location = new Point(51, 49);
                            Buser.Font = new Font("微软雅黑", 9);
                            Buser.Parent = ((LinkLabel)sender).Parent;
                            Buser.ForeColor = ad.Buser.Id == user.Id ? Color.Red : Color.Black;
                            Buser.AutoSize = true;
                        }
                        else
                        {
                            Buser.Text = ad.Buser.KuName;
                            Buser.ForeColor = ad.Buser.Id == user.Id ? Color.Red : Color.Black;

                        }

                        Label Yuser = GetTheLabelByLocation((Panel)((LinkLabel)sender).Parent, 51, 67);
                        if (Yuser == null)
                        {
                            Yuser = new Label();
                            Yuser.Text = ad.Yuser.KuName;
                            Yuser.Location = new Point(51, 67);
                            Yuser.Font = new Font("微软雅黑", 9);
                            Yuser.Parent = ((LinkLabel)sender).Parent;
                            Yuser.ForeColor = ad.Yuser.Id == user.Id ? Color.Red : Color.Black;
                            Yuser.AutoSize = true;
                        }
                        else
                        {
                            Yuser.Text = ad.Yuser.KuName;
                            Yuser.ForeColor = ad.Yuser.Id == user.Id ? Color.Red : Color.Black;

                        }

                    }
                    #endregion

                    #region 网络班
                    else 
                    {
                         Label Duser = GetTheLabelByLocation((Panel)((LinkLabel)sender).Parent, 91, 29);
                         if (Duser == null)
                         {
                             Duser = new Label();
                             Duser.Text = ad.Duser.KuName;
                             Duser.Location = new Point(91, 29);
                             Duser.Font = new Font("微软雅黑", 9);
                             Duser.ForeColor = ad.Duser.Id == user.Id ? Color.Red : Color.Black;
                             Duser.Parent = ((LinkLabel)sender).Parent;
                             Duser.AutoSize = true;
                         }
                         else
                         {
                             Duser.Text = ad.Duser.KuName;
                             Duser.ForeColor = ad.Duser.Id == user.Id ? Color.Red : Color.Black;

                         }

                         Label Buser = GetTheLabelByLocation((Panel)((LinkLabel)sender).Parent, 91, 49);
                         if (Buser == null)
                         {
                             Buser = new Label();
                             Buser.Text = ad.Buser.KuName;
                             Buser.Location = new Point(91, 49);
                             Buser.Font = new Font("微软雅黑", 9);
                             Buser.Parent = ((LinkLabel)sender).Parent;
                             Buser.ForeColor = ad.Buser.Id == user.Id ? Color.Red : Color.Black;
                             Buser.AutoSize = true;
                         }
                         else
                         {
                             Buser.Text = ad.Buser.KuName;
                             Buser.ForeColor = ad.Buser.Id == user.Id ? Color.Red : Color.Black;

                         }

                         Label Yuser = GetTheLabelByLocation((Panel)((LinkLabel)sender).Parent, 91, 67);
                         if (Yuser == null)
                         {
                             Yuser = new Label();
                             Yuser.Text = ad.Yuser.KuName;
                             Yuser.Location = new Point(91, 67);
                             Yuser.Font = new Font("微软雅黑", 9);
                             Yuser.Parent = ((LinkLabel)sender).Parent;
                             Yuser.ForeColor = ad.Yuser.Id == user.Id ? Color.Red : Color.Black;
                             Yuser.AutoSize = true;
                         }
                         else
                         {
                             Yuser.ForeColor = ad.Yuser.Id == user.Id ? Color.Red : Color.Black;
                             Yuser.Text = ad.Yuser.KuName;
                         }
                    }
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

       

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker1.Cursor = Cursors.WaitCursor;
            if (this.dateTimePicker1.Value.Ticks > DateTime.Now.Ticks)
            {
                DateTime tt = new DateTime(this.dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, 1);
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
                }
                else
                {
                    this.initCalendar(this.dateTimePicker1.Value.Year, this.dateTimePicker1.Value.Month, false);
                }

            }
            else 
            {
                this.initCalendar(this.dateTimePicker1.Value.Year, this.dateTimePicker1.Value.Month,false);
            }
            dateTimePicker1.Cursor = Cursors.Hand;
        }
                           
    }
}
