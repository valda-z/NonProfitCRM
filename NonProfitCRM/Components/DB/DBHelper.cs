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

﻿using NonProfitCRM.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Web;

namespace NonProfitCRM.Components.DB
{
    public static class DBHelper
    {
        public static void UpdateDB()
        {
            var _ver = getVersions();
            var _res = getScripts();
            foreach(string key in _res.Keys)
            {
                if(_ver.Count( e => e.VersionName == key) == 0)
                {
                    updateScript(key, getScriptData(_res[key]));
                }
            }
        }

        private static void updateScript(string key, string sql)
        {
            using (var scope = new TransactionScope(
                TransactionScopeOption.RequiresNew,
                new TransactionOptions()
                {
                    IsolationLevel = IsolationLevel.ReadCommitted
                }))
            {
                var cx = new Entities();

                var _ident = new C_Version();
                _ident.VersionName = key;
                cx.C_Version.Add(_ident);
                cx.SaveChanges();

                Regex regex = new Regex(@"^\s*GO\s*$", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                string[] lines = regex.Split(sql);

                foreach (string line in lines)
                {
                    if (line.Trim().Length > 0)
                    {
                        cx.Database.ExecuteSqlCommand(line);
                    }
                }
                scope.Complete();
            }
        }

        private static string getScriptData(string name)
        {
            string ret;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name))
            using (StreamReader reader = new StreamReader(stream))
            {
                ret = reader.ReadToEnd();
            }
            return ret;
        }

        private static SortedList<string, string> getScripts()
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            string folderName = string.Format("{0}.Components.DB.Scripts", executingAssembly.GetName().Name);
            var _list = executingAssembly
                .GetManifestResourceNames()
                .Where(r => r.StartsWith(folderName) && r.EndsWith(".sql"))
                .ToArray();
            SortedList<string, string> ret = new SortedList<string, string>(
                _list.ToDictionary(e => e.Remove(0, folderName.Length + 1).Replace(".sql", "")));
            return ret;
        }

        private static List<C_Version> getVersions()
        {
            try
            {
                return new Entities().C_Version.ToList();
            }
            catch
            {
                var cx = new Entities();
                cx.Database.ExecuteSqlCommand(
                    @"
CREATE TABLE [dbo].[_Version](
	[VersionName] [nvarchar](56) NOT NULL,
	PRIMARY KEY CLUSTERED 
	(
		[VersionName] ASC
	) 
) "
                    );
                return new Entities().C_Version.ToList();
            }
        }
    }
}