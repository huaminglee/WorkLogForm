using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkLogForm.CommonClass
{
    class CommonUtil
    {
        public static String toolTipFormat(string l)
        {
            if (l.Length < 20)
            {
                return l;
            }
            int s = (int)Math.Sqrt(3*l.Length);
            String result = "";
            while (l.Length > s)
            {
                result += l.Substring(0, s) + System.Environment.NewLine;
                l = l.Substring(s);
            }
            if (l.Length > 0)
            {
                result += l;
            }
            return result;

        }
    }
}
