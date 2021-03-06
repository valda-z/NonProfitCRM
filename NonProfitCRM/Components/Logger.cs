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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace NonProfitCRM.Components
{
    /// <summary>
    /// Magic for logging and data history arround
    /// </summary>
    public enum LogType
    {
        INFO,
        WARNING,
        ERROR,
        CHANGE_DATA,
        LOGIN,
        LOGIN_ERROR
    }

    public static class Logger
    {
        public static void Log(
            string userName,
            string description,
            LogType logType = LogType.INFO,
            string data=null,
            string entity = null,
            int? entityid = null)
        {
            var cx = new Entities();
            var log = new NonProfitCRM.Models.Log();
            log.Created = DateTime.UtcNow;
            log.LogType = logType.ToString();
            log.UserName = userName;
            log.Description = description;
            log.Data = data;
            log.Entity = entity;
            log.EntityID = entityid;
            cx.Log.Add(log);
            cx.SaveChanges();
        }

        public static void Log(
            string userName,
            string description,
            object oldObject,
            object newObject,
            string entity = null,
            int? entityid = null,
            LogType logType = LogType.CHANGE_DATA
            )
        {
            string oldD = Serialize(oldObject);
            string newD = Serialize(newObject);
            Log(userName, description, oldD, newD, entity, entityid, logType);
        }

        public static void Log(
            string userName,
            string description,
            string oldData,
            string newData,
            string entity = null,
            int? entityid = null,
            LogType logType = LogType.CHANGE_DATA
            )
        {
            string data = HtmlDiff.Diff(oldData, newData);
            Log(userName, description, logType, data, entity, entityid);
        }

        public static string Serialize(object obj, string prefix = "")
        {
            if (obj == null)
            {
                return "";
            }
            StringBuilder ret = new StringBuilder();
            foreach (var prop in obj.GetType().GetProperties())
            {
                if (prop.PropertyType.IsPublic 
                    && prop.CanRead
                    && prop.GetIndexParameters().Length == 0)
                {
                    var x = prop.GetValue(obj, null);
                    if (x == null || !x.GetType().Namespace.StartsWith("System.Data.Entity.DynamicProxies"))
                    {
                        var enumerable = x as System.Collections.IEnumerable;
                        if (enumerable != null && enumerable.GetType().Name != "String")
                        {
                            // because recursive enumaration is too slow ...
                            continue;
                        }
                        ret.AppendFormat("{0}{1}: {2}\n",
                            prefix, prop.Name, (x == null ? "NULL" : x));
                        /* commented out  because recursive enumaration is too slow ...
                        if (enumerable != null)
                        {
                            foreach (var ix in enumerable)
                            {
                                ret.Append(Serialize(ix, prefix + "     "));
                            }
                        }
                         * */
                    }
                }
            }
            return ret.ToString();
        }
    }
}