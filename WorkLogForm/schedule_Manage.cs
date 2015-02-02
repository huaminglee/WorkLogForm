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
            initData();
        }

        private List<WkTDept> theDepts = new List<WkTDept>();
        private void schedule_Manage_Load(object sender, EventArgs e)
        {
          if(Role.KrOrder.Equals(3))
          {
              this.panel3_pictureBox.Visible = false;
          }
          if (this.panel3_pictureBox.Visible != false)
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
                         " from WK_T_USER u left join (select ll.WkTUserId,ll.Id from LOG_T_STAFFSCHEDULE ll where ll.STATE = " + (int)IEntity.stateEnum.Normal
                         + ") l on u.KU_ID = l.WkTUserId " +
                          " where u.KD_ID = " + dept.Id + " group by u.KU_ID,u.KU_NAME,u.KD_ID) t where dept.KD_ID = t.did ;";
                      IList thenames = baseService.ExecuteSQL(sql1);
                      //[0]人的Id [1]部门id [2] 姓名 [3]文章数量 [4] 部门名称
                      foreach (object[] oo in thenames)
                      {
                          this.dataGridView1.Rows.Add(oo[2], oo[4], oo[3], "查看");
                          this.dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Tag = oo[0];
                      }

                  }
                  comboBox1.SelectedIndex = 0;
              }
              //向表一中添加数据

          }

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
      
        #region 个人日程
        private void personal_search_button_Click(object sender, EventArgs e)
        {
           
            long starttime = dateTimePicker1.Value.Ticks;
            long endtime = dateTimePicker2.Value.Ticks;
            string sql = "from StaffSchedule where State="
                + (int)IEntity.stateEnum.Normal + " and ScheduleTime>=" + starttime + " and ScheduleTime<= " + endtime + " and Staff = " + user.Id +
                " and ArrangeMan.KuName like '%" + this.textBox2.Text.Trim() + "%' and Content like '%"
                + this.textBox1.Text.Trim() + "%' order by ScheduleTime desc";
            IList logList = baseService.loadEntityList(sql);
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
                    row.Cells[1].Value = writeTime.ToString("yyyy-MM-dd HH:mm"); //CNDate.getTimeByTimeTicks(writeTime.TimeOfDay.Ticks);
                    row.Cells[2].Value = sl.Content;
                    row.Cells[2].ToolTipText = CommonUtil.toolTipFormat(sl.Content);
                    row.Cells[3].Value = sl.ArrangeMan.KuName;
                    if(sl.ArrangeMan.Id == user.Id)
                    {
                        row.Cells[4].Value = "删除";
                    }

                    personal_dataGridView.Rows.Add(row);
                    i++;
                }
            }
        }
        private void personal_dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4 && this.personal_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "删除")
            {
                if (MessageBox.Show("您确定要删除？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    StaffSchedule ss = (StaffSchedule)this.personal_dataGridView.Rows[e.RowIndex].Tag;
                    ss.State = (int)IEntity.stateEnum.Deleted;
                    baseService.SaveOrUpdateEntity(ss);
                    this.personal_dataGridView.Rows.RemoveAt(e.RowIndex);
                    MessageBox.Show("删除成功！");
                }


            }
        }
        #endregion


        #region 分享日程

        /// <summary>
        /// 分享日程查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void share_search_button_Click(object sender, EventArgs e)
        {

            long starttime = share_dateTimePicker.Value.Date.Ticks;
            long endtime = share_dateTimePicker_end.Value.Date.Ticks + new DateTime(1, 1, 2).Date.Ticks;

            String sql = "select tt.*,dept.KD_NAME from WK_T_DEPT dept , " +
" (select t.*,ww.KU_NAME name ,ww.KD_ID depid from WK_T_USER ww,  " +
" (select l.Id id ,l.Contents con ,l.ScheduleTime thetime ," +
" l.WkTUserId fuser   from StaffSchedule_M_WkTUser m, LOG_T_STAFFSCHEDULE l,WK_T_USER w " +
" where m.StaffScheduleId = l.Id and m.KU_ID = w.KU_ID and w.KU_ID = " + this.user.Id + " and " +
" l.Contents like '%" + this.textBox5.Text.Trim() + "%' and l.ScheduleTime >= " + this.share_dateTimePicker.Value.Ticks + " and l.ScheduleTime<= " + this.share_dateTimePicker_end.Value.Ticks + ") t " +
" where t.fuser = ww.KU_ID and ww.KU_NAME like '%" + this.share_name_textBox.Text + "%') tt where tt.depid = dept.KD_ID " +
" and dept.KD_NAME like '%" + this.share_dept_comboBox.Text + "%'  order by dept.KD_ID; ";

            string sql1 = "select tt.*,dept.KD_NAME from WK_T_DEPT dept , " +
" (select t.*,ww.KU_NAME name ,ww.KD_ID depid from WK_T_USER ww,  " +
" (select l.Id id ,l.Contents con ,l.ScheduleTime thetime ," +
" l.WkTUserId fuser   from StaffSchedule_M_WkTUser m, LOG_T_STAFFSCHEDULE l,WK_T_USER w " +
" where m.StaffScheduleId = l.Id and m.KU_ID = w.KU_ID and w.KU_ID = " + this.user.Id + " and " +
" l.Contents like '%" + this.textBox5.Text.Trim() + "%' and l.ScheduleTime > " + this.share_dateTimePicker.Value.Ticks + " and l.ScheduleTime<= " + this.share_dateTimePicker_end.Value.Ticks + ") t " +
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
                    DateTime writeTime = new DateTime(Convert.ToInt64(sl[2].ToString()));
                    row.Cells[3].Value = writeTime.ToString("yyyy-MM-dd HH:mm");
                   
                    row.Cells[4].Value = sl[1].ToString();
                    row.Cells[4].ToolTipText = CommonUtil.toolTipFormat(sl[1].ToString());

                    share_dataGridView.Rows.Add(row);
                    i++;
                }
            }
        }

        #endregion

        #region 员工日程

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                dataGridView2.Rows.Clear();
                if (Convert.ToInt32(this.dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString()) != 0)
                {
                    int id = Convert.ToInt32(this.dataGridView1.Rows[e.RowIndex].Tag.ToString());
                    IList logList = baseService.loadEntityList("from StaffSchedule where State=" + (int)IEntity.stateEnum.Normal +
                        " and Staff=" + id + "  order by ScheduleTime desc");

                    foreach (StaffSchedule sl in logList)
                    {

                        DataGridViewRow row = new DataGridViewRow();
                        row.Tag = sl;

                        //这一段代码很重要 要不然赋不进值去
                        foreach (DataGridViewColumn c in this.dataGridView2.Columns)
                        {
                            row.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);
                        }
                        //end

                        row.Cells[0].Value = sl.Content;
                        //row.Cells[0].ToolTipText = CommonUtil.toolTipFormat(contentText);
                        row.Cells[1].Value = new DateTime(sl.ScheduleTime).ToString("yyyy-MM-dd HH:mm");
                        row.Cells[2].Value = sl.ArrangeMan.KuName;
                        if (sl.ArrangeMan.Id == user.Id)
                        {
                            row.Cells[3].Value = "删除";
                        }

                        dataGridView2.Rows.Add(row);
                    }
                }
            }
        }


        /// <summary>
        /// 表二的删除操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3 && this.dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "删除")
            {
                if (MessageBox.Show("您确定要删除？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    StaffSchedule ss = (StaffSchedule)this.dataGridView2.Rows[e.RowIndex].Tag;
                    ss.State = (int)IEntity.stateEnum.Deleted;
                    baseService.SaveOrUpdateEntity(ss);
                    this.dataGridView2.Rows.RemoveAt(e.RowIndex);
                    MessageBox.Show("删除成功！");
                }
            }

        }

        /// <summary>
        /// 日程查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            this.button1.Cursor = Cursors.WaitCursor;
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
                            string sql = "select log from StaffSchedule log  left join log.Staff u where u.Kdid = "
                                + o.Id + " and u.KuName like '%"
                                + this.textBox3.Text.Trim()
                                + "%' and  log.Content like '%"
                                + this.textBox4.Text.Trim() + "%' and " +
                                " log.ScheduleTime >=  " + this.dateTimePicker3.Value.Ticks +
                                " log.ScheduleTime <=  " + this.dateTimePicker4.Value.Ticks
                                 + " log.State = " + (int)IEntity.stateEnum.Normal;
                            IList thelist = baseService.loadEntityList(sql);

                            foreach (StaffSchedule sl in thelist)
                            {
                                DataGridViewRow row = new DataGridViewRow();
                                row.Tag = sl;

                                //这一段代码很重要 要不然赋不进值去
                                foreach (DataGridViewColumn c in this.dataGridView3.Columns)
                                {
                                    row.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);
                                }
                                //end

                                
                                row.Cells[0].Value = sl.Staff.KuName;
                                row.Cells[1].Value = sl.Staff.Kdid.KdName;
                                row.Cells[2].Value = sl.Content;
                                row.Cells[2].ToolTipText = CommonUtil.toolTipFormat(sl.Content);

                                row.Cells[3].Value = new DateTime(sl.ScheduleTime).ToString("yyyy-MM-dd HH:mm");
                                row.Cells[4].Value = sl.ArrangeMan.KuName;
                                if (sl.ArrangeMan.Id == user.Id)
                                {
                                    row.Cells[5].Value = "删除";
                                }

                                dataGridView3.Rows.Add(row);
                            }

                        }

                    }

                }
                else //选择某一个
                {
                    string sql;
                    if (theDepts.Count > 1)
                    {
                        sql = "select log from StaffSchedule log  left join log.Staff u where u.Kdid = "
                                   + ((WkTDept)theDepts[this.comboBox1.SelectedIndex - 1]).Id + " and u.KuName like '%"
                                   + this.textBox3.Text.Trim()
                                   + "%' and  log.Content like '%"
                                   + this.textBox4.Text.Trim() + "%' and " +
                                   " log.ScheduleTime > " + this.dateTimePicker3.Value.Ticks +
                                   " log.ScheduleTime < " + this.dateTimePicker4.Value.Ticks
                                    + " log.State = " + (int)IEntity.stateEnum.Normal;

                    }
                    else
                    {
                        sql = "select log from StaffSchedule log  left join log.Staff u where u.Kdid = "
                                     + ((WkTDept)theDepts[this.comboBox1.SelectedIndex]).Id + " and u.KuName like '%"
                                     + this.textBox3.Text.Trim()
                                     + "%' and  log.Content like '%"
                                     + this.textBox4.Text.Trim() + "%' and " +
                                     " log.ScheduleTime > " + this.dateTimePicker3.Value.Ticks +
                                     " log.ScheduleTime < " + this.dateTimePicker4.Value.Ticks
                                     + " log.State = " + (int)IEntity.stateEnum.Normal;
                    }
                    IList thelist = baseService.loadEntityList(sql);
                    this.dataGridView3.Rows.Clear();
                    foreach (StaffSchedule sl in thelist)
                    {

                        DataGridViewRow row = new DataGridViewRow();
                        row.Tag = sl;

                        //这一段代码很重要 要不然赋不进值去
                        foreach (DataGridViewColumn c in this.dataGridView3.Columns)
                        {
                            row.Cells.Add(c.CellTemplate.Clone() as DataGridViewCell);
                        }
                        //end


                        row.Cells[0].Value = sl.Staff.KuName;
                        row.Cells[1].Value = sl.Staff.Kdid.KdName;
                        row.Cells[2].Value = sl.Content;
                        row.Cells[2].ToolTipText = CommonUtil.toolTipFormat(sl.Content);

                        row.Cells[3].Value = new DateTime(sl.ScheduleTime).ToString("yyyy-MM-dd HH:mm");
                        row.Cells[4].Value = sl.ArrangeMan.KuName;
                        if (sl.ArrangeMan.Id == user.Id)
                        {
                            row.Cells[5].Value = "删除";
                        }

                        dataGridView3.Rows.Add(row);

                       
                    }

                }
                this.button1.Cursor = Cursors.Hand;
            }
            else
            {
                MessageBox.Show("没有选择部门，请联系管理员。");

            }
        }

        /// <summary>
        /// 日程查询的删除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5 && this.dataGridView3.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "删除")
            {
                if (MessageBox.Show("您确定要删除？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    StaffSchedule ss = (StaffSchedule)this.dataGridView3.Rows[e.RowIndex].Tag;
                    ss.State = (int)IEntity.stateEnum.Deleted;
                    baseService.SaveOrUpdateEntity(ss);
                    this.dataGridView3.Rows.RemoveAt(e.RowIndex);
                    MessageBox.Show("删除成功！");
                }
            
            }
        }
        #endregion

        #region 三个按钮
        private void panel1_pictureBox_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel2.Visible = false;
            panel3.Visible = false;
            this.label1.Text = "个人日程";

        }

        private void panel2_pictureBox_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = true;
            panel3.Visible = false;
            this.label1.Text = "分享日程";
        }

        private void panel3_pictureBox_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = true;
            this.label1.Text = "员工日程";
        }


        #endregion

       

       


        
      

     

    }
}
