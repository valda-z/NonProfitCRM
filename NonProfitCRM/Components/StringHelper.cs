/*
* MIT License
* 
* Copyright (c) 2015 - present valda-z
* 
* All rights reserved.
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/

﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
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

        public static string ReplaceDiacritics(this string source)
        {
            string sourceInFormD = source.Normalize(NormalizationForm.FormD);

            var output = new StringBuilder();
            foreach (char c in sourceInFormD)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != UnicodeCategory.NonSpacingMark)
                    output.Append(c);
            }

            return (output.ToString().Normalize(NormalizationForm.FormC));
        }
    }
}