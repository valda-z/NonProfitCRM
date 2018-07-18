using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NonProfitCRM.Components
{
    public static class StringHelper
    {
        /// <summary>
        /// Receives string and returns the string with its letters reversed.
        /// </summary>
        public static string ReverseString(string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        public static string FormatBytes(Int64 bytes)
        {
            string[] sizes = { "Bytes", "KB", "MB", "GB", "TB" };
            if (bytes == 0)
            {
                return "0 Byte";
            }
            var i = (int)(Math.Floor(Math.Log(bytes) / Math.Log(1024)));
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, 
                "{0} {1}", Math.Round(bytes / Math.Pow(1024, i), 2), sizes[i]);
        }
    }
}