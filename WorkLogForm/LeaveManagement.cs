using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WorkLogForm.WindowUiClass;//包含creatWindow窗口处理函数
using ClassLibrary;//leave实体所在的命名空间
using WorkLogForm.Service;
using NHibernate;
using NHibernate.Cfg;
using System.Collections;
using WorkLogForm.CommonClass;

namespace WorkLogForm
{
    public partial class LeaveManagement : Form
    {
        private BaseService baseService=new BaseService();
        private WkTUser user;

        public WkTUser User
        {
            get { return user; }
            set { user = value; }
        }
        public LeaveManagement()
        {
            InitializeComponent();
            initialWindow();
        }

        //设置界面圆角及阴影效果
        private void initialWindow()
        {
            creatWindow.SetFormRoundRectRgn(this, 20);
            creatWindow.SetFormShadow(this);
        }
        //设置无边框界面跟随鼠标移动

        private int x, y;


        private void Leave_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && this.Location.Y > 0)
            {
                Top = MousePosition.Y - y;
                Left = MousePosition.X - x;
            }
            else if (e.Button == MouseButtons.Left && e.Y > this.y)
            {
                Top = MousePosition.Y - y;
                Left = MousePosition.X - x;
            }
        }

        private void Leave_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.x = e.X;
                this.y = e.Y;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox3_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox3.BackgroundImage = WorkLogForm.Properties.Resources.最小化渐变;
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            pictureBox3.BackgroundImage = WorkLogForm.Properties.Resources.最小化_2_;
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox2.BackgroundImage = WorkLogForm.Properties.Resources.关闭渐变;
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.BackgroundImage = WorkLogForm.Properties.Resources.关闭1;
        }

        //private void InitData()
        //{   //加载该用户的请假信息*************************************************************
        //    listView2.GridLines = true;//显示各个记录的分割线
        //    listView2.Scrollable = true;//需要时候显示滚动条；
        //    List<string> leaveEntity = new List<string>();
        //    leaveEntity.Add("Leave");
        //    ArrayList leaveList = baseService.loadEntityList(leaveEntity, new string[,] { { "Leave", "ku_Id", CommonStaticParameter.IS_NOT_STRING, user.Id.ToString() } });
          
        //   // leaveList是获取的一个与请假表leave相对应的，该用户的请假信息，我们需要提取出该表中我们需要的字段；
        //    for (int i = 0; i < leaveList.Count; i++)
        //    {
        //        ClassLibrary.Leave leave = (ClassLibrary.Leave)leaveList[i];
        //        ListViewItem li = new ListViewItem();
        //        li.SubItems.Clear();
        //        li.SubItems[0].Text = leave.LIL_STARTTIME.ToString()+leave.LIL_STARTSTAGE.ToString();//开始时间=开始的日期与上下午字段的结合
        //        li.SubItems[1].Text = leave.LIL_ENDTIME.ToString()+leave.LIL_ENDSTAGE.ToString();//结束时间=结束的日期+上下午字段
        //        li.SubItems[2].Text = leave.LIL_LEAVETYPE.ToString();//请假类别
        //        li.SubItems[3].Text = leave.LIL_LEAVEREASON.ToString();// 请假原因
        //        li.SubItems[4].Text = leave.LIL_APPROVALSTATUS.ToString();//审核状态
        //        listView2.Items.Add(li);
        //    }
        //}//end**InitData()
        //    private void LeaveAplication() //将提交的请假申请数据存储到数据库中；
        //    {
        //        IEntity leaveapply = new IEntity();


        //        ClassLibrary.Leave a = new Leave();
        //        //a.LIL_STARTTIME =trunc(dateTimePicker1.Value);
        //    }
            
      }

        

    

       
    }

