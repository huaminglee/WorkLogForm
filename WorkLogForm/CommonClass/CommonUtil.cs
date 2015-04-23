using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
        public static string HtmlToReguFormat(string html)
        {
            Regex r = new Regex("<[^<]*>");
            MatchCollection mc = r.Matches(html);
            String contentText = html.Replace("&nbsp;", " ");
            for (int j = 0; j < mc.Count; j++)
            {
                if (mc[j].Value.Contains("src"))
                {
                    contentText = contentText.Replace(mc[j].Value, "[图片]");
                }
                else
                {
                    contentText = contentText.Replace(mc[j].Value, " ");
                }
            }

            return contentText;
        }

    }
}
