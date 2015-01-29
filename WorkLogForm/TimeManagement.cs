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
            string sql = "select u from WkTUser u right join u.UserRole role where role.KrDESC='工作小秘书角色' and role.KrOrder = 1";
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
            IList alreadyYearList = baseService.loadEntityList("select distinct HolidayYear from Holiday where STATE=" + (int)IEntity.stateEnum.Normal);
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
            IList usuallyDayList = baseService.loadEntityList("from UsuallyDay where STATE=" + (int)IEntity.stateEnum.Normal);
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
                submit_button.Location = new Point(submit_button.Location.X, submit_button.Location.Y - 30);
                delete_button.Location = new Point(delete_button.Location.X, delete_button.Location.Y - 30);
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
            submit_button.Location = new Point(86, 244);
            delete_button.Location = new Point(175, 244);
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
                newHoliday.State = (int)IEntity.stateEnum.Normal;
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
            IList holidayList = baseService.loadEntityList("from Holiday where STATE=" + (int)IEntity.stateEnum.Normal + " and HolidayYear=" + Convert.ToInt32(holiday_year_comboBox.Text.Trim()));
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
            submit_button.Location = new Point(86, 244);
            delete_button.Location = new Point(175, 244);
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
            checkSql += ") and h.STATE=" + (int)IEntity.stateEnum.Normal + " and w.STATE=" + (int)IEntity.stateEnum.Normal;
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
            IList usuallyDayList = baseService.loadEntityList("from UsuallyDay where STATE=" + (int)IEntity.stateEnum.Normal + " and Name='" + work_name_comboBox.Text.Trim() + "'");
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

        #region

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 1)
            {
                string sql = "select u from Wktuser_M_Dept u where u.WktuserId = " + ((WkTUser)this.dataGridView1.Rows[e.RowIndex].Tag).Id ;
                IList theone = baseService.loadEntityList(sql);
                
            
            }
        }

        #endregion


    }
}
