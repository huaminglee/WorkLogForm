using OnLineUpdate;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace ChattingCtrl
{
    public class LoadPicture
    {
        public static Image GetHeadIcon(int userid)
        {

           
            string address = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"icons" + @"\" + userid.ToString() + ".png";
            if (File.Exists(address))//本地是否存在 存在
            {
                string[] files = Directory.GetFiles(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"icons", userid.ToString()+"__"+"*.png", System.IO.SearchOption.AllDirectories);
                if (files.Length > 0)
                {
                    //File.Copy(files[0], System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"icons" + @"\" + userid.ToString() + ".png", true);
                    FileInfo oldfi = new FileInfo(address);
                    oldfi.Delete();
                    File.Move(files[0], address);
                }
                
                Image a = Image.FromFile(address);
                a.Tag = userid;
                return a;
            }
            else
            {
                Image res = ChattingCtrl.Properties.Resources.AutoIconBigWhite;
                res.Tag = userid;
                return res;
            }
        }

    }
}
