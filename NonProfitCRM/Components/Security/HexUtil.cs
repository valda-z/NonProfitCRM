using System;
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