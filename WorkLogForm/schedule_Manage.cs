using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using WorkLogForm.WindowUiClass;
using System.Collections;
using ClassLibrary;
using System.Text.RegularExpressions;
using WorkLogForm.Service;
using WorkLogForm.CommonClass;

namespace WorkLogForm
{
    public partial class schedule_Manage : Form
    {
        BaseService baseService = new BaseService();
        private WkTRole role;

        public WkTRole Role
        {
            get { return role; }
            set { role = value; }
        }
        private WkTUser user;

        public WkTUser User
        {
            get { return user; }
            set { user = value; }
        }
        public schedule_Manage()
        {
            InitializeComponent();
            initialWindow();
        }
        private void schedule_Manage_Load(object sender, EventArgs e)
        {
            //if (role.KrOrder.Equals(3))
            //{
            //    tabControl1.TabPages[1].Dispose();
            //}
            initData();
        }
        private void initData()
        {
            personal_year_comboBox.SelectedIndex = 0;
            personal_month_comboBox.SelectedIndex = 0;
            personal_day_comboBox.SelectedIndex = 0;
            IList deptList = baseService.loadEntityList("from WkTDept");
            if (deptList != null && deptList.Count > 0)
            {
                foreach (WkTDept dept in deptList)
                {
                    staff_comboBox.Items.Add(dept.KdName);
                    share_dept_comboBox.Items.Add(dept.KdName);
                }
            }
            if (role.KrOrder.Equals(2))
            {
                staff_comboBox.Items.Clear();
                staff_comboBox.Items.Add(user.Kdid.KdName);
            }
            else if (role.Equals(3))
            {
                staff_comboBox.Items.Clear();
            }
            staff_comboBox.SelectedIndex = 0;
            share_dept_comboBox.SelectedIndex = 0;
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

        //private void staff_LogLeader_Load(object sender, EventArgs e)
        //{
        //    this.month_comboBoxEx.SelectedIndex = 0;
        //    this.year_comboBoxEx.SelectedIndex = 0;
        //}

        //private void button3_Click(object sender, EventArgs e)
        //{
        //    y = this.year_comboBoxEx.SelectedItem.ToString();//要查看的日志的年份
        //    m = this.month_comboBoxEx.SelectedItem.ToString();//要查看的日志的月份
        //    SqlConnection connection = new SqlConnection("UID=sa;PWD=iti240;Database=kjqb;server=115.24.161.202;");
        //    string sqlstr = "select a.LAL_SIGNINTIME,a.LAL_LOG,b.LLC_COMMENT from LOG_T_ATTENCELOG a,LOG_T_LOGCOMMENT b where a.LAL_ID=b.LLC_LOGID and a.KU_ID=90021 and a.LAL_YEAR='" + int.Parse(y) + "' and a.LAL_MONTH='" + int.Parse(m) + "'";
        //    DataSet ds = new DataSet();
        //    SqlDataAdapter da = new SqlDataAdapter(sqlstr, connection);
        //    SqlCommandBuilder cb = new SqlCommandBuilder(da);
        //    da.Fill(ds);
        //    connection.Close();
        //    try
        //    {
        //        DataTable dt = ds.Tables[0];
        //        int b,i = 1;
        //        int a = dt.Rows.Count;
        //        for (b = 0; b < a; b++, i++)
        //        {
        //            //将从数据库中查询的数据显示在listview中
        //            string pubtime = dt.Rows[b][0].ToString();
        //            string content = dt.Rows[b][1].ToString();
        //            string pingjia = dt.Rows[b][2].ToString();
        //            ListViewItem lvi = new ListViewItem();
        //            lvi.Text = i.ToString();
        //            lvi.SubItems.AddRange(new string[] { pubtime, content, pingjia });
        //            this.listView3.Items.Add(lvi);
        //        }

        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }
        //}

        //private void button4_Click(object sender, EventArgs e)
        //{
        //    System.Windows.Forms.SaveFileDialog sfd = new SaveFileDialog();
        //    sfd.DefaultExt = "doc";
        //    sfd.Filter = "Word文件(*.doc)|*.doc";
        //    sfd.FileName = "员工日志";
        //    if (sfd.ShowDialog() == DialogResult.OK)
        //    {
        //        DoWord(listView3, sfd.FileName);
        //    }
        //}
        ////将listview中的数据导出到word中
        //private void DoWord(ListView listView, string strFileName) 
        //{
        //    int rowNum = listView.Items.Count;//表格的行数
        //    int columnNum = listView.Items[0].SubItems.Count;//表格的列数
        //    int rowIndex = 1;
        //    int columnIndex = 0;
        //    if (rowNum == 0 || string.IsNullOrEmpty(strFileName))
        //    {
        //        return;
        //    }
        //    if (rowNum > 0)
        //    {
        //        object oMissing = System.Reflection.Missing.Value;
        //        Microsoft.Office.Interop.Word.Application WordApp;
        //        Microsoft.Office.Interop.Word.Document WordDoc;
        //        WordApp = new Microsoft.Office.Interop.Word.ApplicationClass();
        //        WordDoc = WordApp.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
        //        WordApp.ActiveDocument.PageSetup.PageWidth = WordApp.CentimetersToPoints(float.Parse("29.71"));//纸张宽度(A3)             
        //        WordApp.ActiveDocument.PageSetup.PageHeight = WordApp.CentimetersToPoints( float.Parse( "42.01" ) );//纸张高度(A3)
        //        string strContent = "员工日志\n ";//标题
        //        WordDoc.Paragraphs.First.Range.Text = strContent;
        //        WordDoc.Paragraphs.First.Range.Font.Size = 18;//设置标题字体大小
        //        WordDoc.Paragraphs.First.Range.Font.Bold = 2;//设置标题加粗
        //        WordDoc.Paragraphs.First.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;//设置标题为居中
        //        //WordApp.Selection.TypeParagraph();//插入段落
        //        Microsoft.Office.Interop.Word.Range tableLocation = WordDoc.Paragraphs.Last.Range;//插入表格的位置
        //        Microsoft.Office.Interop.Word.Table newTable = WordDoc.Tables.Add(tableLocation, rowNum+1, columnNum, ref oMissing, ref oMissing);//插入表格
        //        newTable.Borders.Enable = 1;
        //        newTable.Select();//选中表格
        //        WordApp.Selection.Tables[1].Rows.Alignment = Microsoft.Office.Interop.Word.WdRowAlignment.wdAlignRowCenter;//表格居中
        //        //表格的第一行存listview各列的列名
        //        foreach (ColumnHeader dc in listView.Columns)
        //        {
        //            columnIndex++;
        //            newTable.Cell(rowIndex, columnIndex).Range.Text = dc.Text;
        //            newTable.Cell(rowIndex, columnIndex).Range.Bold = 2;//设置单元格中表头字体为粗体
        //            //设置表头字体居中
        //            newTable.Cell(rowIndex, columnIndex).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
        //        }
        //        newTable.Columns[1].Width = 40f;//设置第一列"序号"的宽度
        //        //将listview中的日志保存到表格中,从表格的第二行开始存
        //        for (int i =0; i < rowNum; i++)
        //        {
        //            rowIndex = 2;
        //            columnIndex = 0;
        //            for (int j = 0; j < columnNum; j++)
        //            {
        //                columnIndex++;
        //                newTable.Cell(rowIndex, columnIndex).Range.Text = Convert.ToString(listView.Items[i].SubItems[j].Text) + "\t";
        //            }
        //        }
        //            WordDoc.SaveAs(strFileName, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing);
        //        //关闭WordDoc文档对象     
        //        WordDoc.Close(ref oMissing, ref oMissing, ref oMissing);
        //        MessageBox.Show("日志导出成功");
        //        //关闭WordApp组件对象   
        //        WordApp.Quit(ref oMissing, ref oMissing, ref oMissing);   
        //    }
            
        //}

        ////将listview中的数据导出到excel中
        //private void DoExport(ListView listView, string strFileName)
        //{
        //    int rowNum = listView.Items.Count;//行数
        //    int columnNum = listView.Items[0].SubItems.Count;//列数
        //    int rowIndex = 1;
        //    int columnIndex = 0;
        //    if (rowNum == 0 || string.IsNullOrEmpty(strFileName))
        //    {
        //        return;
        //    }
        //    if (rowNum > 0)
        //    {
        //        Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.ApplicationClass();
        //        object missing = System.Reflection.Missing.Value;//作为缺省值参数传给word或excel对象的某个函数
        //        Microsoft.Office.Interop.Excel.Workbooks xlBooks = xlApp.Workbooks;
        //        Microsoft.Office.Interop.Excel.Workbook xlBook = xlBooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
        //        Microsoft.Office.Interop.Excel.Worksheet xlSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlBook.Worksheets[1];//取得sheet1
        //        Microsoft.Office.Interop.Excel.Range range = null;
        //        if (xlApp == null)
        //        {
        //            MessageBox.Show("无法创建excel对象，可能您的系统没有安装excel");
        //            return;
        //        }
        //        xlApp.DefaultFilePath = "";
        //        xlApp.DisplayAlerts = true;
        //        //将ListView的列名导入Excel表第一行
        //        foreach (ColumnHeader dc in listView.Columns)
        //        {
        //            columnIndex++;
        //            xlSheet.Cells[rowIndex, columnIndex] = dc.Text;
                    
        //        }
        //        //将ListView中的数据导入Excel中
        //        for (int i = 0; i < rowNum; i++)
        //        {
        //            rowIndex++;
        //            columnIndex = 0;
        //            for (int j = 0; j < columnNum; j++)
        //            {
        //                columnIndex++;
        //                //注意这个在导出的时候加了“\t” 的目的就是避免导出的数据显示为科学计数法。可以放在每行的首尾。
        //                xlSheet.Cells[rowIndex, columnIndex] = Convert.ToString(listView.Items[i].SubItems[j].Text) + "\t";
        //                range = xlSheet.get_Range(xlSheet.Cells[rowIndex, columnIndex], xlSheet.Cells[rowIndex, columnIndex]);//表示获取要设置的单元格或单元格范围
        //                range.Columns.AutoFit(); // 设置列宽为自动适应
        //            }
        //        }
        //        xlSheet.SaveAs(strFileName, missing, missing, missing, missing, missing, missing, missing, missing, missing);
        //        MessageBox.Show("日志导出成功");
        //        xlApp.Quit();
        //    }
        //}

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
        private void staff_LogLeader_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.x_point = e.X;
                this.y_point = e.Y;
            }
        }

        private void staff_LogLeader_MouseMove(object sender, MouseEventArgs e)
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
        private void initDayCombobox(object sender, EventArgs e)
        {
            if (personal_month_comboBox.Text.Trim() != "请选择" && personal_year_comboBox.Text.Trim() != "请选择")
            {
                personal_day_comboBox.Items.Clear();
                CNDate cnDate = new CNDate(new DateTime(Convert.ToInt32(personal_year_comboBox.Text), Convert.ToInt32(personal_month_comboBox.Text), 1));
                personal_day_comboBox.Items.Add("请选择");
                for (int i = 1; i <= cnDate.GetDayNumOfMonth(); i++)
                {
                    personal_day_comboBox.Items.Add(i.ToString());
                }
            }
            personal_day_comboBox.SelectedIndex = 0;
        }
        #region 个人日程
        private void personal_search_button_Click(object sender, EventArgs e)
        {
            int year = personal_year_comboBox.Text != "请选择" ? Convert.ToInt32(personal_year_comboBox.Text.Trim()) : 0;
            int month = personal_month_comboBox.Text != "请选择" ? Convert.ToInt32(personal_month_comboBox.Text.Trim()) : 0;
            int day = personal_day_comboBox.Text != "请选择" ? Convert.ToInt32(personal_day_comboBox.Text.Trim()) : 0;
            long starttime = 0;
            long endtime = 0;
            if (year == 0)
            {
                starttime = new DateTime(1, 1, 1).Date.Ticks;
                endtime = new DateTime(3000, 1, 1).Date.Ticks;
                personal_month_comboBox.Text = "请选择";
                personal_day_comboBox.Text = "请选择";
            }
            else if (month == 0)
            {
                starttime = new DateTime(year, 1, 1).Date.Ticks;
                endtime = new DateTime(year, 12, 31, 23, 59, 59).Date.Ticks;
                personal_day_comboBox.Text = "请选择";
            }
            else if (day == 0)
            {
                if (month == 12)
                {
                    starttime = new DateTime(year, month, 1).Date.Ticks;
                    endtime = new DateTime(year + 1, 1, 1).Date.Ticks;
                }
                else
                {
                    starttime = new DateTime(year, month, 1).Date.Ticks;
                    endtime = new DateTime(year, month + 1, 1).Date.Ticks;
                }
            }
            else
            {
                starttime = new DateTime(year, month, 1).Date.Ticks;
                endtime = new DateTime(year, month, 1, 23, 59, 59).Date.Ticks;
            }
            IList logList = baseService.loadEntityList("from StaffSchedule where State=" + (int)IEntity.stateEnum.Normal + " and ScheduleTime>=" + starttime + " and ScheduleTime<" + endtime + " and Staff=" + user.Id + " order by ScheduleTime desc");
            personal_dataGridView.Rows.Clear();
            initPersonalDataGridView(logList);
        }
        private void initPersonalDataGridView(IList logList)
        {
            personal_dataGridView.Rows.Clear();
            int i = 1;
            if (logList != null && logList.Count > 0)
            {
                foreach (StaffSchedule sl in logList)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.Tag = sl;
                    foreach (DataGridViewColumn c in this.personal_dataGridView.Columns)
                    {
                        row.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);
                    }
                    row.Cells[0].Value = i;
                    DateTime writeTime = new DateTime(sl.ScheduleTime);
                    row.Cells[1].Value = CNDate.getTimeByTimeTicks(writeTime.TimeOfDay.Ticks);
                    row.Cells[2].Value = sl.Content;
                    row.Cells[2].ToolTipText = CommonUtil.toolTipFormat(sl.Content);
                    personal_dataGridView.Rows.Add(row);
                    i++;
                }
            }
        }
        #endregion
        #region 分享日程
        private void share_search_button_Click(object sender, EventArgs e)
        {
            long starttime = share_dateTimePicker.Value.Date.Ticks;
            long endtime = starttime + new DateTime(1, 1, 2).Date.Ticks;
            String sql = "select sf.id from LOG_T_STAFFSCHEDULE sf,StaffSchedule_M_WkTUser smu,WK_T_USER u,WK_T_DEPT dept where sf.WkTUserId=u.KU_ID and sf.Id=smu.StaffScheduleId and u.KD_ID=dept.KD_ID and smu.KU_ID=" + user.Id + " and sf.State=" + (int)IEntity.stateEnum.Normal + " and sf.ScheduleTime>=" + starttime + " and sf.ScheduleTime<" + endtime + " and u.KU_NAME like '%" + share_name_textBox.Text.Trim() + "%'";
            if (share_dept_comboBox.Text.Trim() != "请选择")
            {
                sql += " and dept.KD_NAME like '" + share_dept_comboBox.Text.Trim() + "%'";
            }
            sql += " order by ScheduleTime desc";
            IList logList = baseService.ExecuteSQL(sql);
            initShareDataGridView(logList);
        }
        private void initShareDataGridView(IList logList)
        {
            share_dataGridView.Rows.Clear();
            int i = 1;
            if (logList != null && logList.Count > 0)
            {
                foreach (object[] slId in logList)
                {
                    StaffSchedule sl = new StaffSchedule();
                    String id = slId[0].ToString();
                    baseService.loadEntity(sl, Convert.ToInt64(id));
                    DataGridViewRow row = new DataGridViewRow();
                    row.Tag = sl;
                    foreach (DataGridViewColumn c in this.share_dataGridView.Columns)
                    {
                        row.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);
                    }
                    row.Cells[0].Value = i;
                    row.Cells[1].Value = sl.Staff.KuName.Trim();
                    row.Cells[2].Value = sl.Staff.Kdid.KdName.Trim();
                    DateTime writeTime = new DateTime(sl.ScheduleTime);
                    row.Cells[3].Value = CNDate.getTimeByTimeTicks(writeTime.TimeOfDay.Ticks);
                    row.Cells[4].Value = sl.Content;
                    row.Cells[4].ToolTipText = CommonUtil.toolTipFormat(sl.Content);
                    share_dataGridView.Rows.Add(row);
                    i++;
                }
            }
        }
        #endregion
        #region 员工日程
        private void staff_button_Click(object sender, EventArgs e)
        {
            long starttime = staff_dateTimePicker_start.Value.Date.Ticks;
            long endtime = starttime + new DateTime(1, 1, 2).Date.Ticks;
            String sql = "select sf from StaffSchedule sf left join sf.Staff left join sf.Staff.Kdid where State=" + (int)IEntity.stateEnum.Normal + " and sf.ScheduleTime>=" + starttime + " and sf.ScheduleTime<" + endtime + " and sf.Staff.KuName like '%" + staff_name_textBox.Text.Trim() + "%' and sf.Staff.KuName !='" + user.KuName + "'";
            if (staff_comboBox.Text.Trim() != "请选择")
            {
                sql += " and sf.Staff.Kdid.KdName like '" + share_dept_comboBox.Text.Trim() + "%'";
            }
            sql += " order by ScheduleTime desc";
            IList logList = baseService.loadEntityList(sql);
            initStaffDataGridView(logList);
        }
        private void initStaffDataGridView(IList logList)
        {
            staff_dataGridView.Rows.Clear();
            int i = 1;
            if (logList != null && logList.Count > 0)
            {
                foreach (StaffSchedule sl in logList)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.Tag = sl;
                    foreach (DataGridViewColumn c in this.staff_dataGridView.Columns)
                    {
                        row.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);
                    }
                    row.Cells[0].Value = i;
                    row.Cells[1].Value = sl.Staff.KuName.Trim();
                    row.Cells[2].Value = sl.Staff.Kdid.KdName.Trim();
                    DateTime writeTime = new DateTime(sl.ScheduleTime);
                    row.Cells[3].Value = CNDate.getTimeByTimeTicks(writeTime.TimeOfDay.Ticks);
                    row.Cells[4].Value = sl.Content;
                    row.Cells[4].ToolTipText = CommonUtil.toolTipFormat(sl.Content);
                    staff_dataGridView.Rows.Add(row);
                    i++;
                }
            }
        }
        #endregion

        private void panel1_pictureBox_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel2.Visible = false;
            panel3.Visible = false;
        }

        private void panel2_pictureBox_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = true;
            panel3.Visible = false;
        }

        private void panel3_pictureBox_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = true;
        }

        
    }
}
