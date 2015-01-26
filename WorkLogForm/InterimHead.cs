using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WorkLogForm.WindowUiClass;
using ClassLibrary;
using System.Collections;
using WorkLogForm.Service;

namespace WorkLogForm
{
    public partial class InterimHead : Form
    {
        private BaseService baseService = new BaseService();
        private List<WkTUser> shareList;
        private WkTUser user;
        public WkTUser User
        {
            get { return user; }
            set { user = value; }
        }
        private Form parentForm;
        public Form ParentForm1
        {
            get { return parentForm; }
            set { parentForm = value; }
        }
        public InterimHead()
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
        private void initialData()
        {
            if (typeof(writeLog) == ParentForm1.GetType())
            {
                writeLog wl = (writeLog)ParentForm1;
                shareList = wl.SharedUser;
            }
            else if (typeof(writeSchedule) == ParentForm1.GetType())
            {
                writeSchedule ws = (writeSchedule)ParentForm1;
                shareList = ws.SharedUser;
            }
        }
        #endregion

        #region 窗体移动代码
        private int x_point, y_point;
        private void InterimHead_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.x_point = e.X;
                this.y_point = e.Y;
            }
        }

        private void InterimHead_MouseMove(object sender, MouseEventArgs e)
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

        private void close_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InterimHead_Load(object sender, EventArgs e)
        {
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - this.Width / 2, Screen.PrimaryScreen.WorkingArea.Height / 2 - this.Height / 2);
            initialData();
            initListView(listView2, shareList);
        }

        private void initListView(ListView listView, IList userList)
        {
            if (userList != null && userList.Count > 0)
            {
                foreach (WkTUser u in userList)
                {
                    ListViewItem item = new ListViewItem();
                    item.Tag = u;
                    ListViewItem.ListViewSubItem name = new ListViewItem.ListViewSubItem();
                    ListViewItem.ListViewSubItem dept = new ListViewItem.ListViewSubItem();
                    ListViewItem.ListViewSubItem role = new ListViewItem.ListViewSubItem();
                    name.Text = u.KuName.Trim();
                    dept.Text = u.Kdid.KdName.Trim();
                    foreach (WkTRole r in u.UserRole)
                    {
                        if (r.KrDESC.Trim().Equals("工作小秘书角色"))
                        {
                            role.Text = r.KrName;
                        }
                    }
                    item.SubItems.Add(name);
                    item.SubItems.Add(dept);
                    item.SubItems.Add(role);
                    listView.Items.Add(item);
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            String sql = "select u from WkTUser u left join u.Kdid where u.KuName like '%" + textBox1.Text.Trim() + "%' and u.Kdid.KdName like '%" + comboBox1.Text.Trim() + "%'";
            sql += " and u.Id!=" + this.user.Id;
            if (shareList != null && shareList.Count > 0)
            {
                foreach (WkTUser u in shareList)
                {
                    sql += " and u.Id!=" + u.Id;
                }
            }
            IList shareUserList = baseService.loadEntityList(sql);
            initListView(listView1, shareUserList);
        }

        private void add_pictureBox_Click(object sender, EventArgs e)
        {
            while (listView1.CheckedItems.Count > 0)
            {
                if (shareList == null)
                {
                    shareList = new List<WkTUser>();
                }
                ListViewItem item = listView1.CheckedItems[0];
                listView1.Items.Remove(item);
                listView2.Items.Add(item);
                shareList.Add((WkTUser)item.Tag);
                item.Checked = false;
            }
        }

        private void remove_pictureBox_Click(object sender, EventArgs e)
        {
            while (listView2.CheckedItems.Count > 0)
            {
                ListViewItem item = listView2.CheckedItems[0];
                listView2.Items.Remove(item);
                listView1.Items.Add(item);
                shareList.Remove((WkTUser)item.Tag);
                item.Checked = false;
            }
        }

        private void save_button_Click(object sender, EventArgs e)
        {
            if (typeof(writeLog) == ParentForm1.GetType())
            {
                writeLog wl = (writeLog)ParentForm1;
                wl.SharedUser = shareList;
            }
            else if (typeof(writeSchedule) == ParentForm1.GetType())
            {
                writeSchedule ws = (writeSchedule)ParentForm1;
                ws.SharedUser = shareList;
            }
            MessageBox.Show("保存成功！");
            this.Close();
        }
    }
}
