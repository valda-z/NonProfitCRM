using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using NonProfitCRM.Filters;
using NonProfitCRM.Models;
using System.Linq.Expressions;
using NonProfitCRM.Components;
using System.Text;

namespace NonProfitCRM.Controllers
{
    [Authorize]
    //[InitializeSimpleMembership]
    public class AccountController : Controller
    {
        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl, FormCollection m)
        {
            model.RememberMe = (m["RememberMe"] != null && m["RememberMe"] == "true");
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                UsersContext uc = new UsersContext();
                UserProfile up = uc.UserProfiles.Single(e => e.UserName == model.UserName);
                if (up.Deleted)
                {
                    // if user is deleted, lets log off!!
                    WebSecurity.Logout();
                    // If we got this far, something failed, redisplay form
                    ModelState.AddModelError("", "This user name is disabled.");
                    Logger.Log(model.UserName, "This user name is disabled.", LogType.LOGIN_ERROR);
                    return View(model);
                }

                //save timezone
                if (m["TimeZone"] != null && m["TimeZone"].Length > 0)
                {
                    string timZ = m["TimeZone"];
                    if (timZ.Length > 128)
                    {
                        timZ = timZ.Substring(0, 128);
                    }
                    up.TimeZone = timZ;
                    uc.SaveChanges();
                }
                Logger.Log(model.UserName, "Login successful.", LogType.LOGIN);
                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        //
        // POST: /Account/LogOff

        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "USER_ADMINISTRATOR")]
        public ActionResult Users(string search)
        {
            if (search == null)
            {
                search = "";
            }

            var model = new Entities().ViewUserProfileList.
                Where(e => search == "" || e.UserName.Contains(search) || 
                    e.FirstName.Contains(search) || e.LastName.Contains(search) || e.Email.Contains(search)).
                OrderBy(e => e.UserName).Take(NonProfitCRM.Properties.Settings.Default.MAXRECORDS);

            return View(model);
        }

        [Authorize(Roles = "USER_ADMINISTRATOR")]
        public ActionResult Groups(string search)
        {
            if (search == null)
            {
                search = "";
            }

            var model = new Entities().Group.
                Where(e => search == "" || e.Name.Contains(search)).
                OrderBy(e => e.Name);

            return View(model);
        }

        [HttpGet]
        public ActionResult ChangePassword(string returnUrl)
        {
            var model = new NonProfitCRM.Models.LocalPasswordModel();
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpGet]
        public ActionResult MyAccount()
        {
            return View(AccountHelper.GetUserDetail(WebSecurity.GetUserId(User.Identity.Name)));
        }

        [HttpPost]
        public ActionResult ChangePassword(NonProfitCRM.Models.LocalPasswordModel model, string returnUrl)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            if (hasLocalAccount)
            {
                //if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        // redirect
                        if (returnUrl != null && returnUrl.Length > 0)
                        {
                            return RedirectToLocal(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "USER_ADMINISTRATOR")]
        public ActionResult UserDetailChangePassword(int id, string returnUrl)
        {
            var model = new NonProfitCRM.Models.LocalPasswordModel();
            ViewBag.UserName = new NonProfitCRM.Models.UsersContext().UserProfiles.Single(e => e.UserId == id).UserName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "USER_ADMINISTRATOR")]
        public ActionResult UserDetailChangePassword(int id, NonProfitCRM.Models.LocalPasswordModel model, string returnUrl)
        {
            string username = new NonProfitCRM.Models.UsersContext().UserProfiles.Single(e => e.UserId == id).UserName;

            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(id);
            ViewBag.HasLocalPassword = hasLocalAccount;
            if (hasLocalAccount)
            {
                //if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        var token = WebSecurity.GeneratePasswordResetToken(username);
                        // create a link with this token and send email
                        // link directed to an action with form to capture password
                        changePasswordSucceeded = WebSecurity.ResetPassword(token, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        // redirect
                        if (returnUrl != null && returnUrl.Length > 0)
                        {
                            return RedirectToLocal(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "The new password is invalid.");
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "USER_ADMINISTRATOR")]
        public ActionResult UserDetail(int id, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(AccountHelper.GetUserDetail(id));
        }

        
        [HttpPost]
        [Authorize(Roles = "USER_ADMINISTRATOR")]
        public ActionResult UserDetail(int id, UserDetail model, string returnUrl, FormCollection m)
        {
            UsersContext uc = new UsersContext();
            if (model.User.Description == null)
            {
                model.User.Description = "";
            }
            //if we are creating user ....
            if (model.User.UserId == 0)
            {
                model.NewPassword = m["NewPassword"];
                model.NewPassword2 = m["NewPassword2"];
                if (model.NewPassword != model.NewPassword2)
                {
                    ModelState.AddModelError("", "Password and Confirm password are different!");
                    return View(model);
                }
                if (!WebSecurity.UserExists(model.User.UserName))
                {
                    WebSecurity.CreateUserAndAccount(
                        model.User.UserName,
                        model.NewPassword,
                        new { FirstName = model.User.FirstName, 
                                LastName = model.User.LastName, 
                                Email = model.User.Email, 
                                Description = model.User.Description, 
                                Deleted = model.User.Deleted });
                    id = uc.UserProfiles.Single(e => e.UserName == model.User.UserName).UserId;
                }
                else
                {
                    //error... user already exists
                    ModelState.AddModelError("", "Username ["+model.User.UserName+"] already exists, please use different username.");
                    return View(model);
                }
            }

            //save user info
            UserProfile up = uc.UserProfiles.Single(e => e.UserId == id);
            up.FirstName = model.User.FirstName;
            up.LastName = model.User.LastName;
            up.Deleted = model.User.Deleted;
            up.Email = model.User.Email;
            up.GroupId = model.User.GroupId;
            up.Description = model.User.Description;
            uc.SaveChanges();
            //save roles
            saveUserRoles(up.UserName, model.User.GroupId);
            // redirect
            if (returnUrl != null && returnUrl.Length > 0)
            {
                return RedirectToLocal(returnUrl);
            }
            else
            {
                return RedirectToAction("Users", "Account");
            }
        }

        [HttpGet]
        [Authorize(Roles = "USER_ADMINISTRATOR")]
        public ActionResult GroupDetail(int id, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            NonProfitCRM.Models.Group g;
            if (id == 0)
            {
                g = new Group();
            }
            else
            {
                g = new NonProfitCRM.Models.Entities().Group.Where(e => e.Id == id).Single();
            }
            return View(g);
        }

        [HttpPost]
        [Authorize(Roles = "USER_ADMINISTRATOR")]
        public ActionResult GroupDetail(int id, Group model, string returnUrl, FormCollection m)
        {
            NonProfitCRM.Models.Group p;
            Entities cx = new Entities();
            if (model.Id == 0)
            {
                p = model;
                p.Roles = getGroupRoles(m);
                cx.Group.Add(p);
                cx.SaveChanges();
            }
            else
            {
                p = cx.Group.Single(e => e.Id == id);
                p.Name = model.Name;
                p.Roles = getGroupRoles(m);
                cx.SaveChanges();
            }

            //save user roles
            saveGroupRolesForUsers(p.Id, p.Roles);

            // redirect
            if (returnUrl != null && returnUrl.Length > 0)
            {
                return RedirectToLocal(returnUrl);
            }
            else
            {
                return RedirectToAction("Groups", "Account");
            }
        }

        private string getGroupRoles(FormCollection m)
        {
            StringBuilder s = new StringBuilder();
            foreach (string rol in m.AllKeys)
            {
                if (rol.StartsWith("role_") && m[rol].Trim() != "")
                {
                    s.Append(m[rol].Trim());
                    s.Append(";");
                }
            }
            return s.ToString();
        }

        private string[] convertRolesString(string roles)
        {
            string[] r = new string[0];
            if (roles != null)
            {
                List<string> s = new List<string>();
                foreach(var _tmp in roles.Split(';'))
                {
                    if (_tmp.Length > 0)
                    {
                        s.Add(_tmp);
                    }
                }
                r = s.ToArray<string>();
            }
            return r;
        }

        private void saveGroupRolesForUsers(int id, string roles)
        {
            // get usernames for group
            string[] rolesArr = convertRolesString(roles);
            foreach (var u in new UsersContext().UserProfiles.Where(e => e.GroupId == id))
            {
                saveUserRoles(u.UserName, rolesArr);
            }
        }

        private void saveUserRoles(string username, string[] roles)
        {
            //Save role changes, first step = remove and then add to selected roles
            if (Roles.GetRolesForUser(username).Length > 0)
            {
                Roles.RemoveUserFromRoles(username, Roles.GetRolesForUser(username));
            }
            if (roles.Length > 0)
            {
                Roles.AddUserToRoles(username, roles);
            }
        }

        private void saveUserRoles(string username, int group_id)
        {
            saveUserRoles(username, 
                convertRolesString(new Entities().Group.Single(e => e.Id == group_id).Roles));
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed." 
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
