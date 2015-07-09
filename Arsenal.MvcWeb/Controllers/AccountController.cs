﻿using System;
using System.Web.Mvc;
using System.Web.Security;

using Arsenal.MvcWeb.Models;

namespace Arsenal.MvcWeb.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        //
        // GET: /Account/Index

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        //
        // POST: /Account/Login

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                object userKey;

                if (MembershipDto.ValidateUser(model.UserName, model.Password, providerUserKey: out userKey))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);

                    Session["AuthorizedUser"] = MembershipDto.GetUser(userKey);

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        TempData["DataUrl"] = string.Format("data-url={0}", returnUrl);
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        TempData["DataUrl"] = "data-url=/";
                        return RedirectToAction("Index", "Home");
                    }
                }
                else if (MembershipDto.ValidateAcnUser(model.UserName, model.Password))
                {
                    // not in SSO, but in Acn Users
                    // Sync the user info, register SSO and then log in
                    var acnUid = MembershipDto.GetAcnID(model.UserName);

                    if (acnUid > 0)
                    {
                        var user = new MembershipDto();

                        MembershipCreateStatus createStatus;
                        user.AcnSyncRegister(acnUid, providerUserKey: out userKey, status: out createStatus);

                        if (createStatus == MembershipCreateStatus.Success)
                        {
                            FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);

                            Session["AuthorizedUser"] = MembershipDto.GetUser(userKey);

                            TempData["DataUrl"] = "data-url=/";
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("Warn", ErrorCodeToString(createStatus));
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Warn", "用户名或密码不正确");
                    }
                }
                else
                {
                    ModelState.AddModelError("Warn", "用户名或密码不正确");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            Session.Abandon();

            TempData["DataUrl"] = "data-url=/";
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                var user = new MembershipDto();

                object userKey;
                MembershipCreateStatus createStatus;

                user.CreateUser(model.UserName, model.Password, model.Mobile, model.Email,
                    providerUserKey: out userKey, status: out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, createPersistentCookie: false);

                    Session["AuthorizedUser"] = MembershipDto.GetUser(userKey);

                    TempData["DataUrl"] = "data-url=/";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("Warn", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    var membership = MembershipDto.GetMembership(User.Identity.Name);

                    if (membership != null)
                    {
                        changePasswordSucceeded = membership.ChangePassword(model.OldPassword, model.NewPassword);
                    }
                    else
                    {
                        changePasswordSucceeded = false;
                    }
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    TempData["DataUrl"] = "data-url=/";
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        #region Status Codes
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
