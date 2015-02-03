using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WorkLogForm.Service;
using WorkLogForm.WindowUiClass;
using ClassLibrary;
using System.Collections;
namespace WorkLogForm
{
    public partial class ArrangeDuty : Form
    {


        BaseService baseService = new BaseService();
        List<WkTDept> depts;
        List<WkTUser> theuserDai;
        List<WkTUser> theuserBai;
        List<WkTUser> theuserYe;
        #region 三个向外传的人
        private WkTUser duser;
        public WkTUser Duser
        {
            get { return duser; }
            set { duser = value; }
        }
        private WkTUser buser;
        public WkTUser Buser
        {
            get { return buser; }
            set { buser = value; }
        }

        private WkTUser yuser;
        public WkTUser Yuser
        {
            get { return yuser; }
            set { yuser = value; }
        }
        #endregion
        public ArrangeDuty()
        {
            InitializeComponent();
            initialWindow();
            duser = new WkTUser();
            buser = new WkTUser();
            yuser = new WkTUser();
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

        #region 两个按钮
        #region 关闭按钮
        private void close_pictureBox_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void pictureBox8_MouseMove(object sender, MouseEventArgs e)
        {
            close_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.关闭渐变_副本;
        }

        private void pictureBox8_MouseLeave(object sender, EventArgs e)
        {
            close_pictureBox.BackgroundImage = WorkLogForm.Properties.Resources.关闭渐变;
        }
        #endregion

        #region 最小化
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
        #endregion
        #endregion

        private void ArrangeDuty_Load(object sender, EventArgs e)
        {
            string sql = "select u from WkTDept u";
            IList dts = baseService.loadEntityList(sql);
            if (dts != null && dts.Count > 0)
            {
                depts = new List<WkTDept>();

                foreach (WkTDept dd in dts)
                {
                    this.comboBox1.Items.Add(dd.KdName.ToString().Trim());
                    this.comboBox3.Items.Add(dd.KdName.ToString().Trim());
                    this.comboBox5.Items.Add(dd.KdName.ToString().Trim());
                    depts.Add(dd);
                }
            }
        }



        #region 带班人选择
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.Cursor = Cursors.WaitCursor;
            this.comboBox2.Items.Clear();
            if (theuserDai == null)
            {
                theuserDai = new List<WkTUser>();
            }
            this.theuserDai.Clear();
            string sql = "select u from WkTUser u left join u.Kdid dept where dept.Id = " + this.depts[this.comboBox1.SelectedIndex].Id;
            IList users = baseService.loadEntityList(sql);
            if (users != null && users.Count > 0)
            {

                if (theuserDai.Count > 0)
                {
                    this.theuserDai.Clear();
                }
                foreach (WkTUser u in users)
                {
                    this.comboBox2.Items.Add(u.KuName.ToString().Trim());
                    this.theuserDai.Add(u);
                }
            }

            comboBox1.Cursor = Cursors.Hand;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.duser = theuserDai[this.comboBox2.SelectedIndex];
        }

        #endregion



        #region 带班人选择
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.Cursor = Cursors.WaitCursor;
            this.comboBox4.Items.Clear();
            if (theuserBai == null)
            {
                theuserBai = new List<WkTUser>();
            }
            this.theuserBai.Clear();
            string sql = "select u from WkTUser u left join u.Kdid dept where dept.Id = " + this.depts[this.comboBox3.SelectedIndex].Id;
            IList users = baseService.loadEntityList(sql);
            if (users != null && users.Count > 0)
            {

                if (theuserBai.Count > 0)
                {
                    this.theuserBai.Clear();
                }
                foreach (WkTUser u in users)
                {
                    this.comboBox4.Items.Add(u.KuName.ToString().Trim());
                    this.theuserBai.Add(u);
                }
            }

            comboBox3.Cursor = Cursors.Hand;
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.buser = this.theuserBai[this.comboBox4.SelectedIndex];
        }
        #endregion


        #region
        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox5.Cursor = Cursors.WaitCursor;
            this.comboBox6.Items.Clear();
            if (theuserYe == null)
            {
                theuserYe = new List<WkTUser>();
            }
            this.theuserYe.Clear();
            string sql = "select u from WkTUser u left join u.Kdid dept where dept.Id = " + this.depts[this.comboBox5.SelectedIndex].Id;
            IList users = baseService.loadEntityList(sql);
            if (users != null && users.Count > 0)
            {

                if (theuserYe.Count > 0)
                {
                    this.theuserYe.Clear();
                }
                foreach (WkTUser u in users)
                {
                    this.comboBox6.Items.Add(u.KuName.ToString().Trim());
                    this.theuserYe.Add(u);
                }
            }

            comboBox5.Cursor = Cursors.Hand;
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.yuser = this.theuserYe[this.comboBox6.SelectedIndex];
        }
        #endregion


        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 确定按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if(this.comboBox2.Text != "" && this.comboBox4.Text != "" && this.comboBox6.Text != "")
            {
                MessageBox.Show("设置成功！");
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                MessageBox.Show("您未选择完整");
            }

        }


    }
}
