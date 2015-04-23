using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace OnLineUpdate
{
    public partial class OnLineUpdateWindow : Form
    {
        public OnLineUpdateWindow()
        {
            InitializeComponent();
        }
        FileUpDown fileUpDown;
        private string GetThetUpdateVersionNum(string Dir)
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
                    //获取文件的版本号    
                    LastUpdateTime = xml.GetAttribute("Num"); break;
                }
            }
            xml.Close();
            myFile.Close();
            return LastUpdateTime;
        }
        private void OnLineUpdateWindow_Load(object sender, EventArgs e)
        {
            
        }


        private void BatchDownload()
        {
            try
            {
                if (!File.Exists(System.Windows.Forms.Application.StartupPath + "\\UpdateConfig.xml"))
                    return;//打开xml文件     
                FileStream myFile = new FileStream(System.Windows.Forms.Application.StartupPath + "\\UpdateConfig.xml", FileMode.Open);//xml文件阅读器     
                XmlTextReader xml = new XmlTextReader(myFile);
                IList<string[]> fileList = new List<string[]>();
                while (xml.Read())
                {
                    if (xml.Name == "UpdateFile")
                    {
                        string[] s = { xml.GetAttribute("FileName"), xml.GetAttribute("DestPath") };
                        fileList.Add(s);
                    }
                }
                xml.Close();
                myFile.Close();

                progressBar.Maximum = fileList.Count;
                int filesum = 0;
                foreach (string[] SourceFile in fileList)  //循环取服务器更新路径文件
                {
                    string FileName = SourceFile[0].Substring(SourceFile[0].LastIndexOf("/") + 1);//取更新文件名
                    string FilePath = SourceFile[0].Substring(0, SourceFile[0].LastIndexOf("/"));//取更新文件名
                    fileUpDown.Download(CommonStaticParameter.TEMP + FilePath, SourceFile[0], "WorkLog");
                    string source = CommonStaticParameter.TEMP + FilePath.Replace('/', '\\') + "\\" + FileName;
                    string destFile = System.Windows.Forms.Application.StartupPath + SourceFile[1];
                    while (File.Exists(source) != true)
                    {
                        fileUpDown.Download(CommonStaticParameter.TEMP + FilePath, SourceFile[0], "WorkLog");
                    }
                    this.labDownFile.Text = "正在下载文件：" + FileName + "，文件总数量：" + fileList.Count.ToString();
                    this.labDownFile.Refresh();


                    ++filesum;
                    this.labDownFile.Text = "正在复制文件：" + FileName + "，文件总数量：" + fileList.Count.ToString();
                    this.labDownFile.Refresh();
                    File.Copy(source, destFile + FileName, true);

                    progressBar.Value = filesum;
                }
                progressBar.Value = fileList.Count;
            }
            catch (Exception ex)
            {
                MessageBox.Show("更新失败！" + ex.Message);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.timer1.Interval = 1000000;
            string _ip = Securit.DeDES(FileReadAndWrite.IniReadValue("ftpfile", "ip"));
            string _id = Securit.DeDES(FileReadAndWrite.IniReadValue("ftpfile", "id"));
            string _pwd = Securit.DeDES(FileReadAndWrite.IniReadValue("ftpfile", "pwd"));
            fileUpDown = new FileUpDown(_ip, _id, _pwd);
            string thePreUpdateDate = GetThetUpdateVersionNum(CommonStaticParameter.TEMP);

            File.Copy(CommonStaticParameter.TEMP + "\\UpdateConfig.xml", System.Windows.Forms.Application.StartupPath + "\\UpdateConfig.xml", true);

            //关闭原有的应用程序     
            this.labDownFile.Text = "正在关闭程序....";
            System.Diagnostics.Process[] proc = System.Diagnostics.Process.GetProcessesByName("WorkLogForm");
            //关闭原有应用程序的所有进程     
            foreach (System.Diagnostics.Process pro in proc)
            {
                pro.Kill();
            }


            this.labDownFile.Text = "下载更新文件";
            this.progressBar.Value = 0;
            BatchDownload();
            MessageBox.Show("更新成功！");

            this.labDownFile.Text = "正在启动程序....";
            System.Diagnostics.Process.Start(Application.StartupPath + "\\" + "WorkLogForm.exe");
            this.timer1.Stop();
            this.Close();
        }

    }
}
