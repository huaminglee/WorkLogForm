using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkLogForm.CommonClass
{
    class CommonStaticParameter
    {
        public static string IS_STRING = "0";
        public static string IS_NOT_STRING = "1";
        public static string YES = "0";
        public static string NO = "1";
        public static string RoleDesc = "工作小秘书角色";

        public static string TEMP = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"temp";

    }
}
