using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomExtensions
{
    public static class StringExtension
    {
        public static string ReplaceLineBreakToBr(this string str)
        {
            string newStr = "";
            if (!string.IsNullOrEmpty(str))
            {
                newStr = str.Replace("\n", "<br/>");
            }
            return newStr;
        }
    }
}