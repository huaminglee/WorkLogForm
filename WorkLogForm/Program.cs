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
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Reflection;

namespace WorkLogForm
{
    static class Program
    {
        
        /// <summary>
        /// 该函数设置由不同线程产生的窗口的显示状态。
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="cmdShow">指定窗口如何显示。查看允许值列表，请查阅ShowWlndow函数的说明部分。</param>
        /// <returns>如果函数原来可见，返回值为非零；如果函数原来被隐藏，返回值为零。</returns>
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        /// <summary>
        /// 该函数将创建指定窗口的线程设置到前台，并且激活该窗口。键盘输入转向该窗口，并为用户改各种可视的记号。系统给创建前台窗口的线程分配的权限稍高于其他线程。
        /// </summary>
        /// <param name="hWnd">将被激活并被调入前台的窗口句柄。</param>
        /// <returns>如果窗口设入了前台，返回值为非零；如果窗口未被设入前台，返回值为零。</returns>
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        private const int WS_SHOWNORMAL = 1;





        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Process instance = RunningInstance();
            if (instance == null)
            {


                #region 在线更新
                string thePreUpdateDate = "";
                try
                {
                    string _ip = Securit.DeDES(FileReadAndWrite.IniReadValue("ftpfile", "ip"));
                    string _id = Securit.DeDES(FileReadAndWrite.IniReadValue("ftpfile", "id"));
                    string _pwd = Securit.DeDES(FileReadAndWrite.IniReadValue("ftpfile", "pwd"));
                    FileUpDown fileUpDown = new FileUpDown(_ip, _id, _pwd);

                    string theLastsUpdateVersionNumber = GetTheUpdateVersionNum(System.Windows.Forms.Application.StartupPath);//上次的版本号



                    fileUpDown.Download(CommonStaticParameter.TEMP, "UpdateConfig.xml", "WorkLog");

                    thePreUpdateDate = GetTheUpdateVersionNum(CommonStaticParameter.TEMP);//这次的版本号
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show("程序出错 原因：" + ex.Message.ToString());
                }
                #endregion


                //Application.EnableVisualStyles();
                //Application.SetCompatibleTextRenderingDefault(false);
                SetSelfStarting();
                login login = new login();
                if (login.ShowDialog() == DialogResult.OK)
                {
                    string ip = IniReadAndWrite.IniReadValue("connect", "ip");
                    string id = IniReadAndWrite.IniReadValue("connect", "id");
                    string pwd = IniReadAndWrite.IniReadValue("connect", "pwd");
                    string db = IniReadAndWrite.IniReadValue("connect", "db");
                    main mainForm = new main(thePreUpdateDate);
                    mainForm.User = login.User;
                    mainForm.Role = login.Role;
                    SqlDependency.Start("UID=" + WorkLogForm.CommonClass.Securit.DeDES(id) + ";PWD=" + WorkLogForm.CommonClass.Securit.DeDES(pwd) + ";Database=" + WorkLogForm.CommonClass.Securit.DeDES(db) + ";server=" + WorkLogForm.CommonClass.Securit.DeDES(ip));
                    Application.Run(mainForm);
                }
                //Application.Run(new TimeManagement());




            }
            else
            {
                HandleRunningInstance(instance);
            }


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

        /// <summary>
        /// 获取正在运行的实例，没有运行的实例返回null;
        /// </summary>
        public static Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            foreach (Process process in processes)
            {
                if (process.Id != current.Id)
                {
                    if (Assembly.GetExecutingAssembly().Location.Replace("/", "//") == current.MainModule.FileName)
                    {
                        return process;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 显示已运行的程序。
        /// </summary>
        public static void HandleRunningInstance(Process instance)
        {
            ShowWindowAsync(instance.MainWindowHandle, WS_SHOWNORMAL); //显示，可以注释掉
            SetForegroundWindow(instance.MainWindowHandle);            //放到前端
        }
    }
}

