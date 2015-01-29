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
using WorkLogForm.CommonClass;
using WorkLogForm.Service;

namespace WorkLogForm
{
    public partial class staff_LogLeader : Form
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
        public staff_LogLeader()
        {
            InitializeComponent();
            initialWindow();
        }

        private List<WkTDept> theDepts = new List<WkTDept> () ;
        private void staff_LogLeader_Load(object sender, EventArgs e)
        {
            if (role.KrOrder.Equals(3))
            {
                panel3_pictureBox.Visible = false;
            }
            else //加载领导部门 
            {
                string sql = "select u.DeptId from Wktuser_M_Dept u where u.WktuserId = " + user.Id + " and u.State = " + (int)IEntity.stateEnum.Normal;
                IList theone = baseService.loadEntityList(sql);

                if (theone != null && theone.Count > 0)
                {
                    if (theone.Count > 1)
                    {
                        comboBox1.Items.Add("选择全部…");

                    }
                    foreach (WkTDept dept in theone)
                    {
                        comboBox1.Items.Add(dept.KdName.Trim());
                        theDepts.Add(dept);

                        string sql1 = "select t.*,dept.KD_NAME from kjqbtest.dbo.WK_T_DEPT dept, " +
                           " (select u.KU_ID id ,u.KD_ID did ,u.KU_NAME name ,COUNT(l.Id) num  " +
                           " from WK_T_USER u left join (select ll.WkTUserId,ll.Id from LOG_T_STAFFLOG ll where ll.STATE = "+(int)IEntity.stateEnum.Normal
                           +") l on u.KU_ID = l.WkTUserId " +
                            " where u.KD_ID = "+ dept.Id +" group by u.KU_ID,u.KU_NAME,u.KD_ID) t where dept.KD_ID = t.did ;";
                        IList thenames = baseService.ExecuteSQL(sql1);
                        //[0]人的Id [1]部门id [2] 姓名 [3]文章数量 [4] 部门名称
                        foreach(object[] oo in thenames)
                        {
                            this.dataGridView1.Rows.Add(oo[2], oo[4], oo[3], "查看");
                            this.dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Tag = oo[0];
                        }

                    }
                    comboBox1.SelectedIndex = 0;
                }
                //向表一中添加数据
               
               



            }
            initData();
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
        private void initData()
        {
            IList deptList = baseService.loadEntityList("from WkTDept");
            share_dept_comboBox.Items.Add("选择全部…"); 
            if (deptList != null && deptList.Count > 0)
            {
                foreach (WkTDept dept in deptList)
                {
                   share_dept_comboBox.Items.Add(dept.KdName.Trim());
                }
            }
           
            
            share_dept_comboBox.SelectedIndex = 0;
        }

        #region 注释的内容
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

      
        #region 个人日志 已处理
        private void personal_search_button_Click(object sender, EventArgs e)
        {
            IList logList = baseService.loadEntityList("from StaffLog where State=" + (int)IEntity.stateEnum.Normal + " and WriteTime>=" + dateTimePicker1.Value.Ticks + " and WriteTime<" + dateTimePicker2.Value.Ticks + " and Staff=" + user.Id + " and Content like '%"+this.textBox1.Text.Trim()+"%' order by WriteTime desc");
            personal_dataGridView.Rows.Clear();
            initPersonalDataGridView(logList);
        }
        private void initPersonalDataGridView(IList logList)
        {
            personal_dataGridView.Rows.Clear();
            int i = 1;
            if (logList != null && logList.Count > 0)
            {
                foreach (StaffLog sl in logList)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.Tag = sl;
                    foreach (DataGridViewColumn c in this.personal_dataGridView.Columns)
                    {
                        row.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);
                    }
                    row.Cells[0].Value = i;
                    DateTime writeTime = new DateTime(sl.WriteTime);
                    row.Cells[1].Value = writeTime.Year + "年" + writeTime.Month + "月" + writeTime.Day + "日";
                    Regex r = new Regex("<[^<]*>");
                    MatchCollection mc = r.Matches(sl.Content.ToString());
                    String contentText = sl.Content.ToString().Replace("&nbsp;", " ");
                    for (int j = 0; j < mc.Count; j++)
                    {
                        contentText = contentText.Replace(mc[j].Value, "");
                    }
                    row.Cells[2].Value = contentText;
                    row.Cells[2].ToolTipText = CommonUtil.toolTipFormat(contentText);
                   

                    row.Cells[3].Value = "查看";
                    row.Cells[4].Value = "删除";
                    row.Cells[3].Tag = sl;
                    personal_dataGridView.Rows.Add(row);
                    i++;
                }
            }
        }
        private void personal_dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                StaffLog sf = (StaffLog)personal_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag;
                writeLog wl = new writeLog();
                wl.User = sf.Staff;
                wl.LogDate = new DateTime(sf.WriteTime);
                wl.IsComment = true;
                wl.CommentPersonName = this.User.KuName;
                wl.ShowDialog();
            }
            else if (e.ColumnIndex == 4)
            {
                if (MessageBox.Show("确定要删除吗？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    StaffLog sf = (StaffLog)personal_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Tag;
                    sf.State = (int)IEntity.stateEnum.Deleted;
                    baseService.SaveOrUpdateEntity(sf);
                    this.personal_dataGridView.Rows.RemoveAt(e.RowIndex);
                }
                
            
            }
        }
        #endregion


        #region 分享日志  已处理
        private void share_search_button_Click(object sender, EventArgs e)
        {
            long starttime = share_dateTimePicker.Value.Date.Ticks;
            long endtime = share_dateTimePicker_end.Value.Date.Ticks + new DateTime(1, 1, 2).Date.Ticks;

            String sql = "select tt.*,dept.KD_NAME from WK_T_DEPT dept , " +
" (select t.*,ww.KU_NAME name ,ww.KD_ID depid from WK_T_USER ww,  " +
" (select l.Id id ,l.Contents con ,l.WriteTime thetime ," +
" l.WkTUserId fuser   from StaffLog_M_WkTUser m, LOG_T_STAFFLOG l,WK_T_USER w " +
" where m.StaffLogId = l.Id and m.KU_ID = w.KU_ID and w.KU_ID = "+this.user.Id+" and " +
" l.Contents like '%"+this.textBox3.Text.Trim()+"%' and l.WriteTime > "+ this.share_dateTimePicker.Value.Ticks+" and l.WriteTime<= "+this.share_dateTimePicker_end.Value.Ticks+") t " +
" where t.fuser = ww.KU_ID and ww.KU_NAME like '%"+this.share_name_textBox.Text+"%') tt where tt.depid = dept.KD_ID " +
" and dept.KD_NAME like '%" + this.share_dept_comboBox.Text + "%'  order by dept.KD_ID; ";

            string sql1 = "select tt.*,dept.KD_NAME from WK_T_DEPT dept , " +
" (select t.*,ww.KU_NAME name ,ww.KD_ID depid from WK_T_USER ww,  " +
" (select l.Id id ,l.Contents con ,l.WriteTime thetime ," +
" l.WkTUserId fuser   from StaffLog_M_WkTUser m, LOG_T_STAFFLOG l,WK_T_USER w " +
" where m.StaffLogId = l.Id and m.KU_ID = w.KU_ID and w.KU_ID = " + this.user.Id + " and " +
" l.Contents like '%" + this.textBox3.Text.Trim() + "%' and l.WriteTime > " + this.share_dateTimePicker.Value.Ticks + " and l.WriteTime<= " + this.share_dateTimePicker_end.Value.Ticks + ") t " +
" where t.fuser = ww.KU_ID and ww.KU_NAME like '%" + this.share_name_textBox.Text + "%') tt where tt.depid = dept.KD_ID  order by dept.KD_ID;";

            IList logList;
            if (this.share_dept_comboBox.Text == this.share_dept_comboBox.Items[0].ToString())
            {
                //部门选择全部
                logList = baseService.ExecuteSQL(sql1);
            }
            else
            {
                 logList = baseService.ExecuteSQL(sql);
            }
            //[0] 日志的ID [1] 日志内容 [2] 日志写的时间 [3]分享日志的人的ID [4]分享日志人的姓名 [5]分享人所在的部门ID [6] 分享人所在的部门名称
            
            
           
            initShareDataGridView(logList);
        }
        private void initShareDataGridView(IList logList)
        {
            share_dataGridView.Rows.Clear();
            int i = 1;
            if (logList != null && logList.Count > 0)
            {
                foreach (object[] sl in logList)
                {
            
                    DataGridViewRow row = new DataGridViewRow();
                    row.Tag = sl;

                    //这一段代码很重要 要不然赋不进值去
                    foreach (DataGridViewColumn c in this.share_dataGridView.Columns)
                    {
                        row.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);
                    }
                    //end
                    row.Cells[0].Value = i.ToString();
                    row.Cells[1].Value = sl[4].ToString();
                    row.Cells[2].Value = sl[6].ToString().Trim();
                    DateTime writeTime = new DateTime( Convert.ToInt64(sl[2].ToString()));
                    row.Cells[3].Value = writeTime.Year + "年" + writeTime.Month + "月" + writeTime.Day + "日";
                    Regex r = new Regex("<[^<]*>");
                    MatchCollection mc = r.Matches(sl[1].ToString());
                    String contentText = sl[1].ToString().Replace("&nbsp;", " ");
                    for (int j = 0; j < mc.Count; j++)
                    {
                        contentText = contentText.Replace(mc[j].Value, "");
                    }
                    row.Cells[4].Value = contentText;
                    row.Cells[4].ToolTipText = CommonUtil.toolTipFormat(contentText);
                   
                    row.Cells[5].Value = "查看";
                    row.Cells[5].Tag = sl;
                    share_dataGridView.Rows.Add(row);
                    i++;
                }
            }
        }
        private void share_dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5)
            {
                StaffLog thelog = new StaffLog();
                object[] sf = (object[])share_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag;
                thelog = (StaffLog)baseService.loadEntity(thelog, Convert.ToInt64(sf[0].ToString()));

                writeLog wl = new writeLog();
                wl.User = thelog.Staff;
                wl.LogDate = new DateTime(thelog.WriteTime);
                wl.IsComment = true;
                wl.CommentPersonName = this.user.KuName;
                wl.ShowDialog();
            }
        }
        #endregion

        #region 员工日志 已处理

        /// <summary>
        /// 员工日志查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Items.Count != 0)
            {
                //选择全部
                if (this.comboBox1.Text == this.comboBox1.Items[0].ToString())
                {
                    this.dataGridView3.Rows.Clear();
                    if (theDepts.Count > 0)
                    {
                        foreach (WkTDept o in theDepts)
                        {
                            string sql = "select log from StaffLog log  left join log.Staff u where u.Kdid = "
                                + o.Id + " and u.KuName like '%"
                                + this.textBox2.Text.Trim()
                                + "%' and  log.Content like '%"
                                + this.textBox4.Text.Trim() + "%' and " +
                                " log.WriteTime > " + this.dateTimePicker3.Value.Ticks +
                                " log.WriteTime < " + this.dateTimePicker4.Value.Ticks
                                 + " log.State = " + (int)IEntity.stateEnum.Normal;
                            IList thelist = baseService.loadEntityList(sql);

                            foreach (StaffLog oo in thelist)
                            {
                                this.dataGridView3.Rows.Add(oo.Staff.KuName,oo.Staff.Kdid.KdName,oo.Content,oo.WriteTime, "查看");
                                this.dataGridView3.Rows[this.dataGridView3.Rows.Count - 1].Tag = oo;
                            }
                            
                        }
                        
                    }
                    
                }
                else //选择某一个
                {
                    string sql;
                    if (theDepts.Count > 1)
                    {
                        sql = "select log from StaffLog log  left join log.Staff u where u.Kdid = "
                                   + ((WkTDept)theDepts[this.comboBox1.SelectedIndex - 1]).Id + " and u.KuName like '%"
                                   + this.textBox2.Text.Trim()
                                   + "%' and  log.Content like '%"
                                   + this.textBox4.Text.Trim() + "%' and " +
                                   " log.WriteTime > " + this.dateTimePicker3.Value.Ticks +
                                   " log.WriteTime < " + this.dateTimePicker4.Value.Ticks
                                    + " log.State = " + (int)IEntity.stateEnum.Normal;

                    }
                    else
                    {
                         sql = "select log from StaffLog log  left join log.Staff u where u.Kdid = "
                                      + ((WkTDept)theDepts[this.comboBox1.SelectedIndex]).Id + " and u.KuName like '%"
                                      + this.textBox2.Text.Trim()
                                      + "%' and  log.Content like '%"
                                      + this.textBox4.Text.Trim() + "%' and " +
                                      " log.WriteTime > " + this.dateTimePicker3.Value.Ticks +
                                      " log.WriteTime < " + this.dateTimePicker4.Value.Ticks
                                      + " log.State = " + (int)IEntity.stateEnum.Normal;
                    }
                    IList thelist = baseService.loadEntityList(sql);
                    this.dataGridView3.Rows.Clear();
                    foreach (StaffLog oo in thelist)
                    {
                        this.dataGridView3.Rows.Add(oo.Staff.KuName, oo.Staff.Kdid.KdName, oo.Content, new DateTime(oo.WriteTime).ToString("yyyy-MM-dd"), "查看");
                        this.dataGridView3.Rows[this.dataGridView3.Rows.Count - 1].Tag = oo;
                    }

                }
            }
            else
            {
                MessageBox.Show("没有选择部门，请联系管理员。");
            
            }
        }

        /// <summary>
        /// 查看
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex  == 4)
            {
                StaffLog sf = (StaffLog)dataGridView3.Rows[e.RowIndex].Tag;
                writeLog wl = new writeLog();
                wl.User = sf.Staff;
                wl.LogDate = new DateTime(sf.WriteTime);
                wl.IsComment = true;
                wl.CommentPersonName = this.User.KuName;
                wl.ShowDialog();
            }
        }

        /// <summary>
        /// 表一中查看
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 3)
            {
                if(Convert.ToInt32(this.dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString()) != 0)
                {
                    int id = Convert.ToInt32(this.dataGridView1.Rows[e.RowIndex].Tag.ToString());
                    IList logList = baseService.loadEntityList("from StaffLog where State=" + (int)IEntity.stateEnum.Normal + 
                        " and Staff=" + id + "  order by WriteTime desc");
                    
                    foreach(StaffLog sl in logList)
                    {
                        
                        this.dataGridView2.Rows.Add(sl.Content, new DateTime(sl.WriteTime).ToString("yyyy-MM-dd"), "查看");
                        this.dataGridView2.Rows[this.dataGridView2.Rows.Count - 1].Tag = sl;
                    }
                }
            }
        }

        /// <summary>
        /// 表二中查看
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 2)
            {
                StaffLog sf = (StaffLog)dataGridView2.Rows[e.RowIndex].Tag;
                writeLog wl = new writeLog();
                wl.User = sf.Staff;
                wl.LogDate = new DateTime(sf.WriteTime);
                wl.IsComment = true;
                wl.CommentPersonName = this.User.KuName;
                wl.ShowDialog();
            }


        }


        #endregion

        #region 三个按钮
        /// <summary>
        /// 个人日志按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel1_pictureBox_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel2.Visible = false;
            panel3.Visible = false;
            this.label14.Text = "个人日志";
        }

        /// <summary>
        /// 分享日志按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel2_pictureBox_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = true;
            this.label14.Text = "分享日志";
        }
        /// <summary>
        /// 员工日志按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel3_pictureBox_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = true;
            panel3.Visible = false;
            this.label14.Text = "员工日志";
        }

        #endregion

       

       

     


    }
}
