using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace covid192020.Infrastructure
{
    public static class CustomHelper
    {
        public static string GetDateShortAndTime(DateTime dt, bool Time = false, bool Seconds = false)
         {
                string result = "";
                switch (DateTime.Now.Year - dt.Year)
                {
                    case 0:
                        result = dt.ToString("dd.MM");
                        break;
                    default:
                        result = dt.ToString("dd.MM.yyyy");
                        break;
                }
               
                if (Time && Seconds)
                    result += " " + dt.ToString("HH:mm:ss");
                else if (Time)
                    result += " " + dt.ToString("HH:mm");
            return result;
        }

        public static bool ValidStringOnNullAndSpace(string str)
        {
            if (str == null) return false;
            var str1 = str.Trim();
            return (String.IsNullOrEmpty(str1) || String.IsNullOrWhiteSpace(str1)) ? false : true;
        }
    }

}
