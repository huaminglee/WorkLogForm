using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnLineUpdate
{
    public class CommonStaticParameter
    {
        public static string IS_STRING = "0";
        public static string IS_NOT_STRING = "1";
        public static string YES = "0";
        public static string NO = "1";

        public static string BASE_PATH = System.Windows.Forms.Application.StartupPath + @"\lib\";

        public static string TITLE_RSOURCE = "titleResource";
        public static string NEWS_RSOURCE = "newsResource";
        public static string LETTER_RSOURCE = "letterResource";

        public static string TEMP = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"temp";

        public static string Weather = System.Windows.Forms.Application.StartupPath + @"\Weather\";
    }
}
