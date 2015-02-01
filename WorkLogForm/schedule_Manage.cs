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
                + (int)IEntity.stateEnum.Normal + " and ScheduleTime>=" + starttime + " and ScheduleTime< " + endtime + " and Staff = " + user.Id +
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
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

    }
}
