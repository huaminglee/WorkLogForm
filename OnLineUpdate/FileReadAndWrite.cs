using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;

namespace OnLineUpdate
{
    public class FileReadAndWrite
    {

         [DllImport("kernel32")]

         private static extern long WritePrivateProfileString(string section,string key,string val,string filePath);

         [DllImport("kernel32")]

         private static extern int GetPrivateProfileString(string section,string key,string def,StringBuilder retVal,int size,string filePath);

         public static void IniWriteValue(string Section,string Key,string Value)
         {
             WritePrivateProfileString(Section, Key, Value, Application.StartupPath.ToString() + "\\setting.ini");
         }
         public static string IniReadValue(string Section, string Key)
         {
              StringBuilder temp = new StringBuilder(255);
              int i = GetPrivateProfileString(Section, Key, "", temp, 255, Application.StartupPath.ToString() + "\\setting.ini");
              return temp!=null?temp.ToString():null;
         }

         /// <summary>
         /// 写txt文件
         /// </summary>
         /// <param name="str"></param>
         /// <param name="filePath">例如：Errlog</param>
         /// <param name="fileName">例如：ErrLog.txt</param>
         public static void writeLog(string str,string filePath,string fileName)
         {
             if (!Directory.Exists(filePath))
             {
                 Directory.CreateDirectory(filePath);
             }

             using (StreamWriter sw = new StreamWriter(filePath + "\\" + fileName, true))
             {
                 sw.WriteLine(str);
                 sw.WriteLine("---------------------------------------------------------");
                 sw.Close();
             }
         }
    }
}
