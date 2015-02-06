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
    public partial class TimeManagement : Form
    {
        private List<DateTimePicker> bu_ban_picker_list = new List<DateTimePicker>();
        private List<Button> bu_ban_button_list = new List<Button>();
        private BaseService baseService = new BaseService();


        private WkTUser user;
        public WkTUser User
        {
            get { return user; }
            set { user = value; }
        }


        public TimeManagement()
        {
            InitializeComponent();
        }

        private void TimeManagement_Load(object sender, EventArgs e)
        {
            initialWindow();
            initWorkDayData();
            initHolidayData();
        }


        private List<WkTDept> thedepts;
        private List<WkTUser> theusers;

        /// <summary>
        /// 部门管理中一个变量
        /// </summary>
        private WkTUser userNow;

        #region 自定义窗体初始化方法
        /// <summary>
        /// 初始化window（界面效果）
        /// </summary>
        private void initialWindow()
        {
            creatWindow.SetFormRoundRectRgn(this, 15);
            creatWindow.SetFormShadow(this);

            #region 部门管理页加载

            //查询出副院长的姓名添加到列表中
            string sql = "select u from WkTUser u right join u.UserRole role where role.KrDESC='工作小秘书角色' and role.KrOrder <> 3";
            IList Namelist = baseService.loadEntityList(sql);
            
            if(Namelist != null && Namelist.Count > 0)
            {
                foreach(WkTUser o in Namelist)
                {
                    this.dataGridView1.Rows.Add(o.KuName, "查看");
                    this.dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Tag = o;
                }
            }
            #endregion
        }
        private void initHolidayData()
        {
            holiday_year_comboBox.Items.Clear();
            holiday_listView.Items.Clear();
            IList alreadyYearList = baseService.loadEntityList("select distinct HolidayYear from Holiday where State=" + (int)IEntity.stateEnum.Normal);
            if (alreadyYearList != null)
            {
                for (int i = 0; i < alreadyYearList.Count; i++)
                {
                    holiday_year_comboBox.Items.Add(alreadyYearList[i]);
                }
            }
            if (holiday_year_comboBox.Items.Contains(DateTime.Now.Year))
            {
                holiday_year_comboBox.SelectedIndex = holiday_year_comboBox.Items.IndexOf(DateTime.Now.Year);
            }
        }
        private void initWorkDayData()
        {
            work_name_comboBox.Text = "";
            work_name_comboBox.Items.Clear();
            work_listView.Items.Clear();
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox7.Checked = false;
            work_start_dateTimePicker.Value = DateTime.Now;
            work_end_dateTimePicker.Value = DateTime.Now;
            work_startDate_dateTimePicker.Value = DateTime.Now;
            IList usuallyDayList = baseService.loadEntityList("from UsuallyDay where State=" + (int)IEntity.stateEnum.Normal);
            if (usuallyDayList != null)
            {
                for (int i = 0; i < usuallyDayList.Count; i++)
                {
                    UsuallyDay usd = (UsuallyDay)usuallyDayList[i];
                    work_name_comboBox.Items.Add(usd.Name);
                }
                initWorkDayListView(usuallyDayList);
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
        private void TimeManagement_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.x_point = e.X;
                this.y_point = e.Y;
            }
        }

        private void TimeManagement_MouseMove(object sender, MouseEventArgs e)
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

        #region 节假日管理
        #region 补班日期设置相关事件
        private void button7_Click(object sender, EventArgs e)
        {
            if (bu_ban_picker_list.Count < 7)
            {
                if (bu_ban_picker_list.Count != 0)
                {
                    submit_button.Location = new Point(submit_button.Location.X, submit_button.Location.Y + 30);
                    delete_button.Location = new Point(delete_button.Location.X, delete_button.Location.Y + 30);
                }
                Button delete = new Button();
                delete.Text = "-";
                delete.Size = button7.Size;
                delete.Font = button7.Font;
                delete.TabIndex = bu_ban_picker_list.Count;
                delete.Parent = groupBox2;
                delete.Click += new EventHandler(bu_ban_time_delete);
                delete.Location = new Point(286, 196 + 30 * bu_ban_picker_list.Count);
                button7.Location = new Point(315, 196 + 30 * bu_ban_picker_list.Count);
                DateTimePicker bu_ban_time = new DateTimePicker();
                bu_ban_time.Font = start_dateTimePicker.Font;
                bu_ban_time.TabIndex = bu_ban_picker_list.Count;
                bu_ban_time.Height = 21;
                bu_ban_time.Width = 128;
                bu_ban_time.Parent = groupBox2;
                bu_ban_time.Location = new Point(152, 199 + 30 * bu_ban_picker_list.Count);
                bu_ban_picker_list.Add(bu_ban_time);
                bu_ban_button_list.Add(delete);
            }
            else
            {
                MessageBox.Show("最多只能填写七个");
            }
        }
        private void bu_ban_time_delete(object sender, EventArgs e)
        {
             Button button = (Button)sender;
             DateTimePicker dtp = bu_ban_picker_list[button.TabIndex];
            if (holiday_comboBox.Tag != null&&dtp.Tag!=null)
            {
                Holiday holiday = (Holiday)holiday_comboBox.Tag;
                holiday.WorkDays.Remove((WorkDay)dtp.Tag);
            }
            if (bu_ban_picker_list.Count > 1)
            {
                button7.Top -= 30;
                for (int i = bu_ban_picker_list.Count - 1; i > button.TabIndex; i--)
                {
                    bu_ban_picker_list[i].TabIndex--;
                    bu_ban_picker_list[i].Top -= 30;
                    bu_ban_button_list[i].TabIndex--;
                    bu_ban_button_list[i].Top -= 30;
                }
                //submit_button.Location = new Point(submit_button.Location.X, submit_button.Location.Y - 30);
                //delete_button.Location = new Point(delete_button.Location.X, delete_button.Location.Y - 30);
            }
            else
            {
                button7.Location = new Point(147, button7.Location.Y);
            }
            bu_ban_picker_list.RemoveAt(button.TabIndex);
            bu_ban_button_list.RemoveAt(button.TabIndex);
            dtp.Dispose();
            button.Dispose();
        }
        #endregion

        private void holiday_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Button delete in bu_ban_button_list)
            {
                delete.Dispose();
            }
            bu_ban_button_list.Clear();
            foreach (DateTimePicker dateTime in bu_ban_picker_list)
            {
                dateTime.Dispose();
            }
            bu_ban_picker_list.Clear();
            button7.Location = new Point(147, 196);
            //submit_button.Location = new Point(53, 238);
            //delete_button.Location = new Point(198, 238);
            IList holidayList = baseService.loadEntityList("from Holiday where HolidayYear=" + Convert.ToInt32(year_textBox.Text) + " and name='" + holiday_comboBox.Text + "' and State=" + (int)IEntity.stateEnum.Normal);
            if (holidayList != null && holidayList.Count > 0)
            {
                Holiday h = (Holiday)holidayList[0];
                holiday_comboBox.Tag = holidayList[0];
                start_dateTimePicker.Value = new DateTime(h.StartTime);
                end_dateTimePicker.Value = new DateTime(h.EndTime);
                if (h.WorkDays != null)
                {
                    foreach (WorkDay d in h.WorkDays)
                    {
                        //if (bu_ban_picker_list.Count != 0)
                        //{
                            //submit_button.Location = new Point(submit_button.Location.X, submit_button.Location.Y + 30);
                            //delete_button.Location = new Point(delete_button.Location.X, delete_button.Location.Y + 30);
                        //}
                        Button delete = new Button();
                        delete.Text = "-";
                        delete.Size = button7.Size;
                        delete.Font = button7.Font;
                        delete.TabIndex = bu_ban_picker_list.Count;
                        delete.Parent = groupBox2;
                        delete.Click += new EventHandler(bu_ban_time_delete);
                        delete.Location = new Point(286, 196 + 30 * bu_ban_picker_list.Count);
                        button7.Location = new Point(315, 196 + 30 * bu_ban_picker_list.Count);
                        DateTimePicker bu_ban_time = new DateTimePicker();
                        bu_ban_time.Font = start_dateTimePicker.Font;
                        bu_ban_time.TabIndex = bu_ban_picker_list.Count;
                        bu_ban_time.Height = 21;
                        bu_ban_time.Width = 128;
                        bu_ban_time.Parent = groupBox2;
                        bu_ban_time.Location = new Point(152, 199 + 30 * bu_ban_picker_list.Count);
                        bu_ban_time.Tag = d;
                        bu_ban_time.Value = new DateTime(d.WorkDateTime);
                        bu_ban_picker_list.Add(bu_ban_time);
                        bu_ban_button_list.Add(delete);
                    }
                }
            }
        }
        private void year_textBox_Leave(object sender, EventArgs e)
        {
            holiday_comboBox.Items.Clear();
            holiday_comboBox.Text = "";
            initVoid();
            queryByYear();
        }
        private void holiday_comboBox_TextChanged(object sender, EventArgs e)
        {
            if (holiday_comboBox.Items.Contains(holiday_comboBox.Text))
            {
                holiday_comboBox.SelectedIndex = holiday_comboBox.Items.IndexOf(holiday_comboBox.Text);
            }
            else
            {
                initVoid();
            }
        }
        private void submit_button_Click(object sender, EventArgs e)
        {
            if (year_textBox.Text == null || year_textBox.Text == "")
            {
                MessageBox.Show("请填写要设定何年的节日！");
                return;
            }
            if (holiday_comboBox.Text == null || holiday_comboBox.Text == "")
            {
                MessageBox.Show("请填写节日名称！");
                return;
            }
            if (start_dateTimePicker.Value.Date.Ticks > end_dateTimePicker.Value.Date.Ticks)
            {
                MessageBox.Show("节日开始日期必须早于结束时间！");
                return;
            }
            if (end_dateTimePicker.Value.Date.Ticks <= DateTime.Now.Date.Ticks)
            {
                MessageBox.Show("不可设置今日之前（包括今日的）的假期！");
                return;
            }
            for (int i = 0; i < bu_ban_picker_list.Count; i++)
            {
                DateTimePicker dtp = bu_ban_picker_list[i];
                if (dtp.Value.Date.Ticks <= end_dateTimePicker.Value.Date.Ticks && dtp.Value.Date.Ticks >= start_dateTimePicker.Value.Date.Ticks)
                {
                    MessageBox.Show("补班日期不可在节日期间！");
                    return;
                }
                if (dtp.Value.Date.Ticks <= DateTime.Now.Date.Ticks)
                {
                    MessageBox.Show("不可设置今日之前（包括今日的）的假期！");
                    return;
                }
            }
            if (holiday_comboBox.Tag == null)
            {
                if (checkIfExsit(false, 0))
                {
                    return;
                }
                Holiday newHoliday = new Holiday();
                newHoliday.Name = holiday_comboBox.Text;
                newHoliday.StartTime = start_dateTimePicker.Value.Date.Ticks;
                newHoliday.EndTime = end_dateTimePicker.Value.Date.Ticks;
                //newHoliday.State = (int)IEntity.stateEnum.Normal;
                newHoliday.TimeStamp = DateTime.Now.Ticks;
                newHoliday.HolidayYear = Convert.ToInt32(year_textBox.Text.Trim());
                IList<WorkDay> workList = new List<WorkDay>();
                newHoliday.WorkDays = workList;
                for (int i = 0; i < bu_ban_picker_list.Count; i++)
                {
                    DateTimePicker dtp = bu_ban_picker_list[i];
                    WorkDay w = new WorkDay();
                    w.WorkDateTime = dtp.Value.Date.Ticks;
                    w.State = (int)IEntity.stateEnum.Normal;
                    w.TimeStamp = DateTime.Now.Ticks;
                    w.HolidayId = newHoliday;
                    workList.Add(w);
                }
                baseService.saveEntity(newHoliday);
            }
            else
            {
                Holiday newHoliday = (Holiday)holiday_comboBox.Tag;
                if (checkIfExsit(true, newHoliday.Id))
                {
                    return;
                }
                newHoliday.Name = holiday_comboBox.Text;
                newHoliday.StartTime = start_dateTimePicker.Value.Date.Ticks;
                newHoliday.EndTime = end_dateTimePicker.Value.Date.Ticks;
                newHoliday.State = (int)IEntity.stateEnum.Normal;
                newHoliday.TimeStamp = DateTime.Now.Ticks;
                newHoliday.HolidayYear = Convert.ToInt32(year_textBox.Text.Trim());
                if (newHoliday.WorkDays == null)
                {
                    newHoliday.WorkDays = new List<WorkDay>();
                }
                for (int i = 0; i < bu_ban_picker_list.Count; i++)
                {
                    DateTimePicker dtp = bu_ban_picker_list[i];
                    if (dtp.Tag != null)
                    {
                        WorkDay d = (WorkDay)dtp.Tag;
                        d.WorkDateTime = dtp.Value.Date.Ticks;
                        d.TimeStamp = DateTime.Now.Ticks;
                    }
                    else
                    {
                        WorkDay w = new WorkDay();
                        w.WorkDateTime = dtp.Value.Date.Ticks;
                        w.State = (int)IEntity.stateEnum.Normal;
                        w.TimeStamp = DateTime.Now.Ticks;
                        w.HolidayId = newHoliday;
                        newHoliday.WorkDays.Add(w);
                    }
                }
                try
                {
                    baseService.SaveOrUpdateEntity(newHoliday);
                }
                catch
                {
                    MessageBox.Show("设置失败！");
                    return;
                }
                MessageBox.Show("设置成功！");
            }
            initVoid();
            holiday_comboBox.Items.Clear();
            holiday_comboBox.Text = "";
            queryByYear();
            initHolidayData();
        }
        private void delete_button_Click(object sender, EventArgs e)
        {
            if (holiday_comboBox.Tag != null)
            {
                Holiday holiday = (Holiday)holiday_comboBox.Tag;
                if (holiday.StartTime <= DateTime.Now.Date.Ticks)
                {
                    MessageBox.Show("不可删除今日之前（包括今日）的假期！");
                    return;
                }
                baseService.deleteEntity(holiday);
            }
            else
            {
                MessageBox.Show("请选择要删除的假期");
            }
            holiday_comboBox.Items.Clear();
            holiday_comboBox.Text = "";
            initVoid();
            queryByYear();
            initHolidayData();
        }
        private void holiday_year_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            holiday_listView.Items.Clear();
            IList holidayList = baseService.loadEntityList("from Holiday where State =" + (int)IEntity.stateEnum.Normal + " and HolidayYear=" + Convert.ToInt32(holiday_year_comboBox.Text.Trim()));
            if (holidayList != null && holidayList.Count > 0)
            {
                initHolidayListView(holidayList);
            }
        }

        #region 自定义方法
        /// <summary>
        /// 初始化为最初状态
        /// </summary>
        private void initVoid()
        {
            foreach (Button delete in bu_ban_button_list)
            {
                delete.Dispose();
            }
            bu_ban_button_list.Clear();
            foreach (DateTimePicker dateTime in bu_ban_picker_list)
            {
                dateTime.Dispose();
            }
            bu_ban_picker_list.Clear();
            button7.Location = new Point(147, 196);
            //submit_button.Location = new Point(86, 244);
            //delete_button.Location = new Point(175, 244);
            start_dateTimePicker.Value = DateTime.Now;
            end_dateTimePicker.Value = DateTime.Now;
            holiday_comboBox.SelectedIndex = -1;
            holiday_comboBox.Tag = null;
        }
        /// <summary>
        /// 根据yearTextbox初始化节日
        /// </summary>
        private void queryByYear()
        {
            if (year_textBox.Text != null && year_textBox.Text != "")
            {
                IList holidayList = baseService.loadEntityList("from Holiday where HolidayYear=" + Convert.ToInt32(year_textBox.Text) + " and State=" + (int)IEntity.stateEnum.Normal);
                for (int i = 0; i < holidayList.Count; i++)
                {
                    Holiday h = (Holiday)holidayList[i];
                    holiday_comboBox.Items.Add(h.Name);
                }
            }
        }
        /// <summary>
        /// 初始化节假日ListView
        /// </summary>
        /// <param name="holidayList"></param>
        private void initHolidayListView(IList holidayList)
        {
            int i = 1;
            Font subFont = new Font(holiday_listView.Font.FontFamily, 9, FontStyle.Regular);
            foreach(Holiday h in holidayList)
            {
                ListViewItem item = new ListViewItem();
                item.Font = subFont;
                System.Windows.Forms.ListViewItem.ListViewSubItem nameSub = new System.Windows.Forms.ListViewItem.ListViewSubItem();
                System.Windows.Forms.ListViewItem.ListViewSubItem timeSub = new System.Windows.Forms.ListViewItem.ListViewSubItem();
                System.Windows.Forms.ListViewItem.ListViewSubItem bbTimeSub = new System.Windows.Forms.ListViewItem.ListViewSubItem();
                nameSub.Text = h.Name;
                DateTime startDate = new DateTime(h.StartTime);
                DateTime endDate = new DateTime(h.EndTime);
                timeSub.Text = startDate.ToShortDateString() + "~" + endDate.ToShortDateString();
                if(h.WorkDays!=null&&h.WorkDays.Count>0)
                {
                    foreach(WorkDay w in h.WorkDays)
                    {
                        DateTime workDay= new DateTime(w.WorkDateTime);
                        bbTimeSub.Text += workDay.ToShortDateString() + ";";
                    }
                }
                item.Text = i.ToString();
                item.SubItems.Add(nameSub);
                item.SubItems.Add(timeSub);
                item.SubItems.Add(bbTimeSub);
                holiday_listView.Items.Add(item);
                i++;
            }
        }
        /// <summary>
        /// 检查是否有已经被设置过的日期
        /// </summary>
        /// <param name="isUpdate"></param>
        /// <param name="holidayId"></param>
        /// <returns></returns>
        private bool checkIfExsit(bool isUpdate,long holidayId)
        {
            String checkSql = "from Holiday h left join h.WorkDays w where ((h.StartTime<="
                + start_dateTimePicker.Value.Date.Ticks + " and h.EndTime>="
                + start_dateTimePicker.Value.Date.Ticks + ") or (h.StartTime<="
                + end_dateTimePicker.Value.Date.Ticks + " and h.EndTime>="
                + end_dateTimePicker.Value.Date.Ticks + ") or (h.StartTime>="
                + start_dateTimePicker.Value.Date.Ticks + " and h.EndTime<="
                + end_dateTimePicker.Value.Date.Ticks + ") or w.WorkDateTime="
                + start_dateTimePicker.Value.Date.Ticks + " or w.WorkDateTime=" 
                + end_dateTimePicker.Value.Date.Ticks;
            for (int i = 0; i < bu_ban_picker_list.Count; i++)
            {
                DateTimePicker dtp = bu_ban_picker_list[i];
                checkSql += " or (h.StartTime<=" + dtp.Value.Date.Ticks + " and h.EndTime>="
                    + dtp.Value.Date.Ticks + ")";
                checkSql += " or w.WorkDateTime=" + dtp.Value.Date.Ticks;
            }
            checkSql += ") and h.State=" + (int)IEntity.stateEnum.Normal + " and w.State=" + (int)IEntity.stateEnum.Normal;
            if (isUpdate)
            {
                checkSql += " and h.Id!=" + holidayId;
            }
            ArrayList checkList = (ArrayList)baseService.loadEntityList(checkSql);
            if (checkList != null && checkList.Count > 0)
            {
                String message = "所选日期中有已被假期";
                for (int i = 0; i < checkList.Count; i++)
                {
                    object[] checkResult = (object[])checkList[i];
                    Holiday alreadyExsitHoliday = (Holiday)checkResult[0];
                    message += "\"" + alreadyExsitHoliday.Name + "\"";
                }
                message += "设置过的日期！";
                MessageBox.Show(message);
                return true;
            }
            return false;
        }
        #endregion      
        #endregion

        #region 日常上班管理
        private void work_submit_button_Click(object sender, EventArgs e)
        {
            if (work_startDate_dateTimePicker.Value.Date.Ticks <= DateTime.Now.Ticks)
            {
                MessageBox.Show("不可设置今日之前（包括今日）的工作时间！");
                return;
            }
            UsuallyDay usuallDay = new UsuallyDay();
            if (work_name_comboBox.Tag != null)
            {
                usuallDay = (UsuallyDay)work_name_comboBox.Tag;
            }
            String workDay = "";
            workDay += checkBox1.Checked ? ((char)UsuallyDay.workDayEnum.WorkDay).ToString() : ((char)UsuallyDay.workDayEnum.Holiday).ToString();
            workDay += checkBox2.Checked ? ((char)UsuallyDay.workDayEnum.WorkDay).ToString() : ((char)UsuallyDay.workDayEnum.Holiday).ToString();
            workDay += checkBox3.Checked ? ((char)UsuallyDay.workDayEnum.WorkDay).ToString() : ((char)UsuallyDay.workDayEnum.Holiday).ToString();
            workDay += checkBox4.Checked ? ((char)UsuallyDay.workDayEnum.WorkDay).ToString() : ((char)UsuallyDay.workDayEnum.Holiday).ToString();
            workDay += checkBox5.Checked ? ((char)UsuallyDay.workDayEnum.WorkDay).ToString() : ((char)UsuallyDay.workDayEnum.Holiday).ToString();
            workDay += checkBox6.Checked ? ((char)UsuallyDay.workDayEnum.WorkDay).ToString() : ((char)UsuallyDay.workDayEnum.Holiday).ToString();
            workDay += checkBox7.Checked ? ((char)UsuallyDay.workDayEnum.WorkDay).ToString() : ((char)UsuallyDay.workDayEnum.Holiday).ToString();
            usuallDay.Name = work_name_comboBox.Text.Trim();
            usuallDay.StartTime = work_startDate_dateTimePicker.Value.Date.Ticks;
            usuallDay.WorkTimeStart = work_start_dateTimePicker.Value.TimeOfDay.Ticks;
            usuallDay.WorkTimeEnd = work_end_dateTimePicker.Value.TimeOfDay.Ticks;
            usuallDay.WorkDay = workDay;
            usuallDay.State = (int)IEntity.stateEnum.Normal;
            usuallDay.TimeStamp = DateTime.Now.Ticks;
            try
            {
                baseService.SaveOrUpdateEntity(usuallDay);
            }
            catch
            {
                MessageBox.Show("设置失败！");
                return;
            }
            MessageBox.Show("设置成功！");
            initWorkDayData();
        }
        private void work_delete_button_Click(object sender, EventArgs e)
        {
            if (work_name_comboBox.Tag != null)
            {
                UsuallyDay u = (UsuallyDay)work_name_comboBox.Tag;
                if (u.StartTime <= DateTime.Now.Ticks)
                {
                    MessageBox.Show("不可删除今日之前（包括今日）的设置！");
                    return;
                }
                baseService.deleteEntity(u);
                initWorkDayData();
            }
            else
            {
                MessageBox.Show("请选择要删除的内容");
            }
        }
        private void work_name_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            IList usuallyDayList = baseService.loadEntityList("from UsuallyDay where State=" + (int)IEntity.stateEnum.Normal + " and Name='" + work_name_comboBox.Text.Trim() + "'");
            if (usuallyDayList != null && usuallyDayList.Count > 0)
            {
                UsuallyDay usuallyDay = (UsuallyDay)usuallyDayList[0];
                work_name_comboBox.Tag = usuallyDay;
                char[] workDays = usuallyDay.WorkDay.ToCharArray();
                checkBox1.Checked = workDays[0].Equals((char)UsuallyDay.workDayEnum.WorkDay);
                checkBox2.Checked = workDays[1].Equals((char)UsuallyDay.workDayEnum.WorkDay);
                checkBox3.Checked = workDays[2].Equals((char)UsuallyDay.workDayEnum.WorkDay);
                checkBox4.Checked = workDays[3].Equals((char)UsuallyDay.workDayEnum.WorkDay);
                checkBox5.Checked = workDays[4].Equals((char)UsuallyDay.workDayEnum.WorkDay);
                checkBox6.Checked = workDays[5].Equals((char)UsuallyDay.workDayEnum.WorkDay);
                checkBox7.Checked = workDays[6].Equals((char)UsuallyDay.workDayEnum.WorkDay);
                work_start_dateTimePicker.Value = new DateTime(DateTime.Now.Date.Ticks + usuallyDay.WorkTimeStart);
                work_end_dateTimePicker.Value = new DateTime(DateTime.Now.Date.Ticks + usuallyDay.WorkTimeEnd);
                work_startDate_dateTimePicker.Value = new DateTime(usuallyDay.StartTime);
            }
        }
        private void work_name_comboBox_TextChanged(object sender, EventArgs e)
        {
            if (work_name_comboBox.Items.Contains(work_name_comboBox.Text))
            {
                work_name_comboBox.SelectedIndex = work_name_comboBox.Items.IndexOf(work_name_comboBox.Text);
            }
            else
            {
                work_name_comboBox.Tag = null;
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox6.Checked = false;
                checkBox7.Checked = false;
                work_start_dateTimePicker.Value = DateTime.Now;
                work_end_dateTimePicker.Value = DateTime.Now;
                work_startDate_dateTimePicker.Value = DateTime.Now;
            }
        }

        #region 自定义方法
        private void initWorkDayListView(IList workDayList)
        {
            int i = 1;
            Font subFont = new Font(work_listView.Font.FontFamily, 9, FontStyle.Regular);
            foreach (UsuallyDay u in workDayList)
            {
                ListViewItem item = new ListViewItem();
                item.Font = subFont;
                System.Windows.Forms.ListViewItem.ListViewSubItem nameSub = new System.Windows.Forms.ListViewItem.ListViewSubItem();
                System.Windows.Forms.ListViewItem.ListViewSubItem starTimeSub = new System.Windows.Forms.ListViewItem.ListViewSubItem();
                System.Windows.Forms.ListViewItem.ListViewSubItem endTimeSub = new System.Windows.Forms.ListViewItem.ListViewSubItem();
                System.Windows.Forms.ListViewItem.ListViewSubItem StartDateSub = new System.Windows.Forms.ListViewItem.ListViewSubItem();
                System.Windows.Forms.ListViewItem.ListViewSubItem workDaysSub = new System.Windows.Forms.ListViewItem.ListViewSubItem();
                nameSub.Text = u.Name;
                starTimeSub.Text = CNDate.getTimeByTimeTicks(u.WorkTimeStart);
                endTimeSub.Text = CNDate.getTimeByTimeTicks(u.WorkTimeEnd);
                DateTime startDate = new DateTime(u.StartTime);
                StartDateSub.Text = startDate.ToShortDateString();
                if (u.WorkDay != null && u.WorkDay != "" && u.WorkDay.Length == 7)
                {
                    char[] workDays = u.WorkDay.ToCharArray();
                    for (int j = 0; j < workDays.Length; j++)
                    {
                        workDaysSub.Text += workDays[j].Equals((char)UsuallyDay.workDayEnum.WorkDay) ? (j + 1).ToString() + "," : "";
                    }
                    if (workDaysSub.Text.Length > 0)
                    {
                        workDaysSub.Text = workDaysSub.Text.Substring(0, workDaysSub.Text.Length - 1);
                    }
                }
                item.Text = i.ToString();
                item.SubItems.Add(nameSub);
                item.SubItems.Add(starTimeSub);
                item.SubItems.Add(endTimeSub);
                item.SubItems.Add(StartDateSub);
                item.SubItems.Add(workDaysSub);
                work_listView.Items.Add(item);
                i++;
            }
        }
        #endregion

       
        #endregion

        #region  部门管理

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 1)
            {
                userNow = (WkTUser)this.dataGridView1.Rows[e.RowIndex].Tag;
                string sql = "select u.DeptId from Wktuser_M_Dept u where u.WktuserId = " + ((WkTUser)this.dataGridView1.Rows[e.RowIndex].Tag).Id + " and u.State =  " + (int)IEntity.stateEnum.Normal; 
                IList theone = baseService.loadEntityList(sql);
                
                //加载数据到表2
                this.dataGridView2.Rows.Clear();
                if (theone != null && theone.Count > 0)
                {
                    foreach (WkTDept o in theone)
                    {
                        this.dataGridView2.Rows.Add(o.KdName.ToString().Trim());
                    }
                }
                else 
                {
                    MessageBox.Show("您还未设置！");
                }
                
            }
        }


        /// <summary>
        /// 设置按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.panel3.Visible = true;

            string sql = "select u from WkTDept u"; 
            IList depts = baseService.loadEntityList(sql);
            this.dataGridView3.Rows.Clear();
            if(depts != null && depts.Count > 0)
            {
                foreach(WkTDept o in depts)
                {
                    this.dataGridView3.Rows.Add(0, o.KdName.ToString().Trim());
                    this.dataGridView3.Rows[this.dataGridView3.Rows.Count - 1].Tag = o;
                }
            }

        }

        /// <summary>
        ///  确定按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定修改？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                //删除原来的
                string sql = "select u from Wktuser_M_Dept u where u.WktuserId = " + userNow.Id + " and u.State = " + (int)IEntity.stateEnum.Normal;
                IList theone = baseService.loadEntityList(sql);
                if (theone != null && theone.Count > 0)
                {
                    foreach (Wktuser_M_Dept o in theone)
                    {
                        Wktuser_M_Dept oldone = o;
                        oldone.State = (int)IEntity.stateEnum.Deleted;
                        baseService.SaveOrUpdateEntity(o);
                    }
                }


                int number = this.dataGridView3.Rows.Count;
                for (int i = 0; i < number; i++)
                {
                    if ((bool)dataGridView3.Rows[i].Cells[0].EditedFormattedValue == true)
                    {
                        Wktuser_M_Dept thenew = new Wktuser_M_Dept();
                        thenew.WktuserId = userNow;
                        thenew.DeptId = (WkTDept)dataGridView3.Rows[i].Tag;
                        thenew.State = (int)IEntity.stateEnum.Normal;
                        thenew.TimeStamp = DateTime.Now.Ticks;
                        baseService.SaveOrUpdateEntity(thenew);
                    }

                }
                MessageBox.Show("设置成功！");
                string sql1 = "select u.DeptId from Wktuser_M_Dept u where u.WktuserId = " + userNow.Id + " and u.State = " + (int)IEntity.stateEnum.Normal; 
                IList theone1 = baseService.loadEntityList(sql1);

                //加载数据到表2
                this.dataGridView2.Rows.Clear();
                if (theone1 != null && theone1.Count > 0)
                {
                    foreach (WkTDept o in theone1)
                    {
                        this.dataGridView2.Rows.Add(o.KdName.ToString().Trim());
                    }
                }
                this.panel3.Visible = false;
            }
            
        }


        #endregion


        #region 值班管理

        /// <summary>
        /// 选择tab页的时候加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

            this.comboBox3.SelectedIndex = 0;
            if(this.tabControl1.SelectedIndex == 3)
            {
                //加载以往加载的安排事件
                if(this.dataGridView4.Rows.Count == 0)
                {
                    //查库
                    string sql = "select u from  TimeArrangeForManager u where u.State = " + (int)IEntity.stateEnum.Normal + " order by u.TimeMonth desc";
                    IList i = baseService.loadEntityList(sql);
                    if(i!= null && i.Count > 0)
                    {
                        foreach (TimeArrangeForManager tgm in i)
                        {
                            DataGridView4RowsAdd(tgm);
                        }
                    }
                
                }


                //加载部门下拉列表
                AddDeptsInCombox(this.comboBox1);
               
            }

            if (tabControl1.SelectedIndex == 4)
            {
                listView2.Items.Clear();
            }
            if (tabControl1.SelectedIndex == 5)
            {
                initPage8();
            }
        }

        public void AddDeptsInCombox(ComboBox cb)
        {
            if (cb.Items.Count == 0) //防止重复加载
            {
                //下拉菜单中加载部门信息
                string sql = "select u from WkTDept u";
                IList depts = baseService.loadEntityList(sql);
                if (depts != null && depts.Count > 0)
                {
                    if (thedepts == null)
                    {
                        thedepts = new List<WkTDept>();
                    }
                    else
                    {
                        thedepts.Clear();
                    }
                    foreach (WkTDept dd in depts)
                    {
                        cb.Items.Add(dd.KdName.ToString().Trim());
                        thedepts.Add(dd);
                    }
                }
            }
        
        }


        /// <summary>
        /// 当部门选择改变的时候需要显示相应部门的人员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.Cursor = Cursors.WaitCursor;
            this.comboBox2.Items.Clear();
            if (theusers == null)
            {
                theusers = new List<WkTUser>();
            }
            this.theusers.Clear();
            string sql = "select u from WkTUser u left join u.Kdid dept where dept.Id = " + this.thedepts[this.comboBox1.SelectedIndex].Id;
            IList users = baseService.loadEntityList(sql);
            if (users != null && users.Count > 0)
            {
               
                if (theusers.Count > 0)
                {
                    this.theusers.Clear();
                }
                foreach (WkTUser u in users)
                {
                    this.comboBox2.Items.Add(u.KuName.ToString().Trim());
                    this.theusers.Add(u);
                }
            }

            comboBox1.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// 确定按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            this.button3.Cursor = Cursors.WaitCursor;
            DateTime dt = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, 1);
            //判断这个月是否安排
            string sql = "select u from  TimeArrangeForManager u where u.TimeMonth = " + dt.Ticks + 
                " and u.DutyType = "+this.comboBox3.SelectedIndex+
                " and u.State = "+ (int)IEntity.stateEnum.Normal;
            IList i = baseService.loadEntityList(sql);
            if(i!= null &&i.Count>0)
            {
                MessageBox.Show("该月份已经安排过了");
                this.button3.Cursor = Cursors.Hand;
                return;
            }
            
            
            if(this.comboBox1.Text != "" && this.comboBox2.Text != "")
            {
                TimeArrangeForManager tgm = new TimeArrangeForManager();
                tgm.ArrangeUserId = User;
                tgm.UserId = theusers[this.comboBox2.SelectedIndex];
                tgm.State = (int)IEntity.stateEnum.Normal;
                tgm.TimeStamp = DateTime.Now.Ticks;
               
                tgm.TimeMonth = dt.Ticks;
                tgm.IsDone = 0;
                tgm.DutyType = this.comboBox3.SelectedIndex;
                tgm.ExamineState = 0;
                baseService.SaveOrUpdateEntity(tgm);
                DataGridView4RowsAdd(tgm);
                MessageBox.Show("添加成功！");
            }
            this.button3.Cursor = Cursors.Hand;
        }

        public void DataGridView4RowsAdd(TimeArrangeForManager tgm)
        {
            if(tgm != null)
            {
                DateTime dt = new DateTime (tgm.TimeMonth);
                if(tgm.ExamineState == 0)
                {
                    this.dataGridView4.Rows.Add(dt.ToString("yyyy年MM月"), tgm.UserId.KuName, tgm.ArrangeUserId.KuName, tgm.DutyType == 0 ? "行政班" : "网络班","未审核", tgm.IsDone == 1 ? "" : "删除");
                }
                else if(tgm.ExamineState == 1)
                {
                    this.dataGridView4.Rows.Add(dt.ToString("yyyy年MM月"), tgm.UserId.KuName, tgm.ArrangeUserId.KuName, tgm.DutyType == 0 ? "行政班" : "网络班","审核通过", tgm.IsDone == 1 ? "" : "删除");
                }
                else if(tgm.ExamineState == 2)
                {
                    this.dataGridView4.Rows.Add(dt.ToString("yyyy年MM月"), tgm.UserId.KuName, tgm.ArrangeUserId.KuName, tgm.DutyType == 0 ? "行政班" : "网络班","审核为通过", tgm.IsDone == 1 ? "" : "删除");
                }

                this.dataGridView4.Rows[this.dataGridView4.Rows.Count - 1].Tag = tgm;
            }
        }


        /// <summary>
        /// 选择值班更换界面时候发生的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControl2.SelectedIndex == 1)
            {
                this.comboBox11.SelectedIndex = 0;
            }
        }

        OnDutyTable onDutyTable;
        /// <summary>
        /// 值班调整查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button11_Click(object sender, EventArgs e)
        {
            this.button11.Cursor = Cursors.WaitCursor;

            DateTime dt = new DateTime(this.dateTimePicker9.Value.Year,this.dateTimePicker9.Value.Month,this.dateTimePicker9.Value.Day);
            string sql = "select u from OnDutyTable u where u.Time = " + dt.Ticks + " and u.State = "+(int)IEntity.stateEnum.Normal;
            IList thedayInfo = baseService.loadEntityList(sql);
            if (thedayInfo != null && thedayInfo.Count > 0)
            {
                onDutyTable = (OnDutyTable)thedayInfo[0];
                this.labelOfDaiBan.Text = onDutyTable.DaiBanID.KuName;
                this.labelOfBaiBan.Text = onDutyTable.BaiBanID.KuName;
                this.labelOfYeBan.Text = onDutyTable.YeBanID.KuName;
                this.panelOfThreeButtonChange.Visible = true;
            }
            else
            {
                MessageBox.Show("没有值班信息");
            }


            this.button11.Cursor = Cursors.Hand;
        }

        int whichOne;
        private void buttonDaiban_Click(object sender, EventArgs e)
        {
            this.panelOfGetNew.Visible = true;
            whichOne = 0;
            AddDeptsInCombox(this.comboBox10);
        }

        private void buttonOfBaiban_Click(object sender, EventArgs e)
        {
            this.panelOfGetNew.Visible = true;
            whichOne = 1;
            AddDeptsInCombox(this.comboBox10);
        }

        private void buttonOfYeBan_Click(object sender, EventArgs e)
        {
            this.panelOfGetNew.Visible = true;
            whichOne = 2;
            AddDeptsInCombox(this.comboBox10);
        }


        private void comboBox10_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox10.Cursor = Cursors.WaitCursor;
            this.comboBox9.Items.Clear();
            if (theusers == null)
            {
                theusers = new List<WkTUser>();
            }
            this.theusers.Clear();
            string sql = "select u from WkTUser u left join u.Kdid dept where dept.Id = " + this.thedepts[this.comboBox10.SelectedIndex].Id;
            IList users = baseService.loadEntityList(sql);
            if (users != null && users.Count > 0)
            {

                if (theusers.Count > 0)
                {
                    this.theusers.Clear();
                }
                foreach (WkTUser u in users)
                {
                    this.comboBox9.Items.Add(u.KuName.ToString().Trim());
                    this.theusers.Add(u);
                }
            }

            comboBox10.Cursor = Cursors.Hand;
        }


        /// <summary>
        /// 确定更改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button10_Click(object sender, EventArgs e)
        {
            if (this.comboBox9.Text == "")
            {
                MessageBox.Show("您还为选择！");
            }
            else
            {
                if(MessageBox.Show("确定要更改吗?", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    if (onDutyTable != null)
                    {
                        WkTUser newuser = this.theusers[this.comboBox9.SelectedIndex];
                        switch (whichOne)
                        {
                            case 0:
                                onDutyTable.DaiBanID = newuser;
                                this.labelOfDaiBan.Text = newuser.KuName;
                                break;
                            case 1:
                                onDutyTable.BaiBanID = newuser;
                                this.labelOfBaiBan.Text = newuser.KuName;
                                break;
                            case 2:
                        onDutyTable.YeBanID = newuser;
                                this.labelOfYeBan.Text = newuser.KuName;
                                break;
                        }

                        baseService.SaveOrUpdateEntity(onDutyTable);
                        MessageBox.Show("更改成功！");
                        this.panelOfGetNew.Visible = false;
                    
                    }
                
                }
            }

        }


        #endregion

        
        #region 出差管理
        private void button4_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            string query = "from Business b where b.StartTime>= " + dateTimePicker3.Value.Date.Ticks + " and  b.EndTime<=" + dateTimePicker4.Value.Date.Ticks + " and b.Ku_Id.KuName like '%" + textBox2.Text + "%' and b.State=" + (int)Business.stateEnum.Normal +" and b.PassExam="+(int)Business.ExamState.done+ " order by b.StartTime";
            IList depList = baseService.loadEntityList(query);
            int i = 1;
            foreach (Business b in depList)
            {
                ListViewItem item = new ListViewItem();
                item.UseItemStyleForSubItems = false;
                item.Text = i.ToString();
                item.SubItems.Add(new DateTime(b.StartTime).ToShortDateString());
                item.SubItems.Add(new DateTime(b.EndTime).ToShortDateString());
                //item.SubItems.Add(b.BusinessDestination);
                //item.SubItems.Add(b.BusinessReason);
                item.SubItems.Add(b.Ku_Id.KuName);
                item.SubItems.Add("通过审批");
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

        private void listView2_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {
            ViewBusiness view = new ViewBusiness();
            view.business = (Business)listView2.SelectedItems[0].Tag;
            view.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Business b = new Business();
            if (listView2.SelectedItems != null)
            {
                b = (Business)listView2.SelectedItems[0].Tag;
                string query1 = "update LOG_T_BUSINESS set State="+(int)Business.stateEnum.Deleted+"where Id="+b.Id;
                string query2 = "update LOG_T_BUSINESSEMPLOYEE set State=" + (int)BusinessEmployee.stateEnum.Deleted + "where BUSINESSID=" + b.Id;
                baseService.ExecuteSQL(query2);
                baseService.ExecuteSQL(query1);
                listView2.Items.Remove(listView2.SelectedItems[0]);
            }
        }
        #endregion
        #region 请假销假管理
        private void button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            String sql = "select h from LeaveManage h left join h.Ku_Id u left join u.Kdid d where u.KuName like '%" + textBox1.Text.Trim() + "%'and d.KdName like '%" + comboBox1.Text.Trim() + "%' and h.StartTime>=" + dateTimePicker5.Value.Date.Ticks + " and h.EndTime<=" + dateTimePicker2.Value.Date.Ticks + "and h.LeaveResult=2 and h.State="+(int)LeaveManage.stateEnum.Normal;

            IList searchList = baseService.loadEntityList(sql);
            if (searchList != null && searchList.Count > 0)
            {
                initlistviewdata(searchList, listView1);
            }
        }
        private void initlistviewdata(IList lista, ListView list_a)//将list1中的数据加载到list_a中显示
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
                    subchargeman.Text = subchargeman.Text + h.KuName + " ";
                }

                subresult.Text = "审批通过";
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
        private void initPage8() 
        {
            listView1.Items.Clear();
            button8.Enabled = false;
            button9.Enabled = false;
        }
        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {

            if (this.listView1.SelectedItems == null) return;
            ListViewItem item = this.listView1.SelectedItems[0];//请假查看-listview2中的数据被选中
            if (item == null) return;

            LeaveManage u = (LeaveManage)item.Tag;
   
            comboBox5.Text = item.SubItems[3].Text;//请假类别
            textBox3.Text = item.SubItems[8].Text;//请假原因

            dateTimePicker7.Value = new DateTime(u.StartTime);//开始时间
            dateTimePicker6.Value = new DateTime(u.EndTime);//结束时间
            groupBox3.Enabled = true;
            button8.Enabled = true;
            button9.Enabled = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems == null) return;
            ListViewItem item = this.listView1.SelectedItems[0];//请假查看-listview2中的数据被选中
            if (item == null) return;

            LeaveManage u = (LeaveManage)item.Tag;
            u.StartTime = dateTimePicker7.Value.Ticks;
            u.EndTime = dateTimePicker6.Value.Ticks;
            baseService.SaveOrUpdateEntity(u);
            listView1.Items.Remove(item);
            button8.Enabled = false;
            button9.Enabled = false;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems == null) return;
            ListViewItem item = this.listView1.SelectedItems[0];//请假查看-listview2中的数据被选中
            if (item == null) return;

            LeaveManage u = (LeaveManage)item.Tag;
            u.State = (int)LeaveManage.stateEnum.Deleted;
            baseService.SaveOrUpdateEntity(u);
            listView1.Items.Remove(item);
            button8.Enabled = false;
            button9.Enabled = false;
        }
        #endregion

      

       

        

       

      

    }
}
