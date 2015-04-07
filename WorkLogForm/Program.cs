using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Data.SqlClient;
using WorkLogForm.CommonClass;
using CommonClass;
using System.IO;
using System.Xml;

namespace WorkLogForm
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {

            #region 在线更新

            string _ip = Securit.DeDES(FileReadAndWrite.IniReadValue("ftpfile", "ip"));
            string _id = Securit.DeDES(FileReadAndWrite.IniReadValue("ftpfile", "id"));
            string _pwd = Securit.DeDES(FileReadAndWrite.IniReadValue("ftpfile", "pwd"));
            FileUpDown fileUpDown = new FileUpDown(_ip, _id, _pwd);
         
            string theLastsUpdateVersionNumber = GetTheUpdateVersionNum(System.Environment.CurrentDirectory);//上次的版本号


            try
            {
                fileUpDown.Download(CommonStaticParameter.TEMP, "UpdateConfig.xml", "WorkLog");
            }
            catch(Exception ex)
            {
                throw ex;
            
            }
            string thePreUpdateDate = GetTheUpdateVersionNum(CommonStaticParameter.TEMP);//这次的版本号
            if (thePreUpdateDate != "")
            {
                //如果客户端将升级的应用程序的更新版本号与服务器上的不一致则进行更新    
                if (thePreUpdateDate != theLastsUpdateVersionNumber)
                {
                    MessageBox.Show("当前软件不是最新的，请更新后登陆！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    {
                        System.Diagnostics.Process.Start(Application.StartupPath + "\\" + "OnLineUpdate.exe");
                        return;
                    }
                }
            }

            #endregion


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SetSelfStarting();
            login login = new login();
            if (login.ShowDialog() == DialogResult.OK)
            {
                string ip = IniReadAndWrite.IniReadValue("connect", "ip");
                string id = IniReadAndWrite.IniReadValue("connect", "id");
                string pwd = IniReadAndWrite.IniReadValue("connect", "pwd");
                string db = IniReadAndWrite.IniReadValue("connect", "db");
                main mainForm = new main();
                mainForm.User = login.User;
                mainForm.Role = login.Role;
                SqlDependency.Start("UID=" + WorkLogForm.CommonClass.Securit.DeDES(id) + ";PWD=" + WorkLogForm.CommonClass.Securit.DeDES(pwd) + ";Database=" + WorkLogForm.CommonClass.Securit.DeDES(db) + ";server=" + WorkLogForm.CommonClass.Securit.DeDES(ip));
                Application.Run(mainForm);
            }
            //Application.Run(new TimeManagement());
        }


        /// <summary>
        /// 获取上一次的版本号
        /// </summary>
        /// <param name="Dir"></param>
        /// <returns></returns>
        private static string GetTheUpdateVersionNum(string Dir)
        {
            string LastUpdateTime = "";
            string AutoUpdaterFileName = Dir + @"\UpdateConfig.xml";
            if (!File.Exists(AutoUpdaterFileName))
                return LastUpdateTime;//打开xml文件     
            FileStream myFile = new FileStream(AutoUpdaterFileName, FileMode.Open);//xml文件阅读器     
            XmlTextReader xml = new XmlTextReader(myFile);
            while (xml.Read())
            {
                if (xml.Name == "Version")
                {
                    //获取版本号   
                    LastUpdateTime = xml.GetAttribute("Num"); break;
                }
            }
            xml.Close();
            myFile.Close();
            return LastUpdateTime;
        }


        /// <summary>
        /// 设置开机启动
        /// </summary>
        /// <returns></returns>
        public static bool SetSelfStarting()
        {
            try
            {
                string exeDir = Application.ExecutablePath;//要启动的程序绝对路径
                RegistryKey rk = Registry.LocalMachine;
                RegistryKey softWare = rk.OpenSubKey("SOFTWARE");
                RegistryKey microsoft = softWare.OpenSubKey("Microsoft");
                RegistryKey windows = microsoft.OpenSubKey("Windows");
                RegistryKey current = windows.OpenSubKey("CurrentVersion");
                RegistryKey run = current.OpenSubKey(@"Run", true);//这里必须加true就是得到写入权限
                if (run.GetValue("工作小秘书") == null || !run.GetValue("工作小秘书").Equals(exeDir))
                {
                    run.SetValue("工作小秘书", exeDir);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

       
    }
}
