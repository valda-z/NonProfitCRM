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
using System.Linq;
using System.Text;
using System.Web;

namespace NonProfitCRM.Components.Security
{
    public static class HexUtil
    {
        public static void XorCopy(byte[] source, int indexSource, byte[] dest, int indexDest, int len)
        {
            for (int i = 0; i < len; i++)
            {
                dest[i] = (byte)(dest[i + indexDest] ^ source[i + indexSource]);
            }
        }


        public static string BytesToHexString(byte[] bytes)
        {
            char[] c = new char[bytes.Length * 2];
            int b;
            for (int i = 0; i < bytes.Length; i++)
            {
                b = bytes[i] >> 4;
                c[i * 2] = (char)(55 + b + (((b - 10) >> 31) & -7));
                b = bytes[i] & 0xF;
                c[i * 2 + 1] = (char)(55 + b + (((b - 10) >> 31) & -7));
            }
            return new string(c);
        }

        public static byte[] HexStringToBytes(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                     .Where(e => e % 2 == 0)
                     .Select(e => Convert.ToByte(hex.Substring(e, 2), 16))
                     .ToArray();
        }

        public static byte[] ColonHexStringToBytes(string hex)
        {
            return HexStringToBytes(hex.Replace(":", ""));
        }

        public static string BytesToColonHexString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", ":");
        }

    }
}