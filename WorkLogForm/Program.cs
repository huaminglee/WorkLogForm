using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Data.SqlClient;
using WorkLogForm.CommonClass;

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
                SqlDependency.Start("UID=" + Securit.DeDES(id) + ";PWD=" + Securit.DeDES(pwd) + ";Database=" + Securit.DeDES(db) + ";server=" + Securit.DeDES(ip));
                Application.Run(mainForm);
            }
            //Application.Run(new TimeManagement());
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
