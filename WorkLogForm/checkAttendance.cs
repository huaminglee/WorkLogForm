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

namespace WorkLogForm
{
    public partial class checkAttendance : Form
    {
        public checkAttendance()
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
        //private void button2_Click(object sender, EventArgs e)
        //{
        //    DateTime dtFirst = DateTime.Parse(dateTimePicker1.Value.ToShortDateString());//获取查询的开始时间并转换成年月日的形式
        //    DateTime dtLast = DateTime.Parse(dateTimePicker2.Value.ToShortDateString());//获取查询的结束时间并转换成年月日的形式
        //    TimeSpan d =  dtLast - dtFirst;
        //    int days = d.Days + 1;//转换成查询时间段相差的天数
        //    DateTime now = DateTime.Now;
        //    SqlConnection connection = new SqlConnection("UID=sa;PWD=iti240;Database=kjqb;server=115.24.161.202;");
        //    if(dtLast>DateTime.Parse(now.ToShortDateString()))
        //        MessageBox.Show("查询结束时间大于系统当前时间");
        //    else
        //    {
        //        int s1 = 0, s2 = 0,s3 = 0,s4,s5 = 0;//s1:实际出勤 s2:迟到 s3:早退 s4:缺卡 s5:请假
        //        //查询实际出勤天数的SQL语句
        //        string sqlstr1 = "select count(*) from LOG_T_ATTENCELOG where KU_ID=90021 and LAL_SIGNINTIME>='" + dtFirst + "'and LAL_SIGNINTIME<='" + dtLast + "'";
        //        //查询迟到天数的SQL语句
        //        string sqlstr2 = "select count(*) from LOG_T_ATTENCELOG where KU_ID=90021 and LAL_SIGNINTIME>='" + dtFirst + "'and LAL_SIGNINTIME<='" + dtLast + "'and LAL_ISLATE=1";
        //        //查询早退天数的SQL语句
        //        string sqlstr3 = "select count(*) from LOG_T_ATTENCELOG where KU_ID=90021 and LAL_SIGNINTIME>='" + dtFirst + "'and LAL_SIGNINTIME<='" + dtLast + "'and LAL_ISLEAVEEARLY=1";
        //        //查询请假天数的SQL语句(还没有写)************************************************************
        //        string sqlstr5 = "";
        //        SqlDataReader objDataReader1, objDataReader2, objDataReader3, objDataReader4;
        //        SqlCommand objCommand1, objCommand2, objCommand3, objCommand4;            
        //        ListViewItem lvi = new ListViewItem();
        //        //执行数据库查询s1
        //        try 
        //        {
        //            objCommand1 = new SqlCommand(sqlstr1, connection);
        //            connection.Open();
        //            objDataReader1 = objCommand1.ExecuteReader(CommandBehavior.CloseConnection);                 
        //            if (objDataReader1.Read())
        //            {
        //                s1 = int.Parse(objDataReader1[0].ToString());
        //            }
        //        }
        //        finally 
        //        {
        //            connection.Close();
        //        }
        //        //执行数据库查询s2
        //        try 
        //        {
        //            objCommand2 = new SqlCommand(sqlstr2, connection);
        //            connection.Open();
        //            objDataReader2 = objCommand2.ExecuteReader(CommandBehavior.CloseConnection);
        //            if (objDataReader2.Read())
        //            {
        //                s2 = int.Parse(objDataReader2[0].ToString());
        //            }
        //        }
        //        finally 
        //        {
        //            connection.Close();
        //        }
        //        //执行数据库查询s3
        //        try
        //        {
        //            objCommand3 = new SqlCommand(sqlstr3, connection);
        //            connection.Open();
        //            objDataReader3 = objCommand3.ExecuteReader(CommandBehavior.CloseConnection);
        //            if (objDataReader3.Read())
        //            {
        //                s3 = int.Parse(objDataReader3[0].ToString());
        //            }
        //        }
        //        finally
        //        {
        //            connection.Close();
        //        }
        //        //执行数据库查询s5**********************************************
        //        //查询缺卡天数s4(缺卡=查询时间段的天数-实际出勤天数)
        //        s4 = days - s1;
        //        lvi.Text = s1.ToString();
        //        lvi.SubItems.AddRange(new string[] { s2.ToString(), s3.ToString(), s4.ToString(), "0" });
        //        this.listView1.Items.Add(lvi);
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
        private void checkAttendance_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.x_point = e.X;
                this.y_point = e.Y;
            }
        }

        private void checkAttendance_MouseMove(object sender, MouseEventArgs e)
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

    }
}
