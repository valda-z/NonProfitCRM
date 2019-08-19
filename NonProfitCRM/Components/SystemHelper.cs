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
using System.Web;

namespace NonProfitCRM.Components
{
    public class SystemHelper
    {
        public static string GetAssemblyName()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public static string GetUserName
        {
            get
            {
                var userClaims = HttpContext.Current.User.Identity as System.Security.Claims.ClaimsIdentity;
                return userClaims?.FindFirst("preferred_username")?.Value;
            }
        }

        public enum Roles
        {
            FRD,
            FRD_SYSTEM_ADMINISTRATOR
        }

        public static bool IsInRole(Roles role)
        {
            var userClaims = HttpContext.Current.User.Identity as System.Security.Claims.ClaimsIdentity;
            if (!userClaims.IsAuthenticated)
            {
                return false;
            }
            return userClaims.HasClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", role.ToString());
        }
    }
}