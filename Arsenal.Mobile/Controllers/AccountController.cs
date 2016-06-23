using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Arsenal.Mobile.Models;
using Arsenal.Service;
using Arsenal.Service.Rest;
using Arsenalcn.Core;
using Arsenalcn.Core.Utility;
using AutoMapper;
using Newtonsoft.Json.Linq;
using Membership = Arsenal.Service.Membership;

namespace Arsenal.Mobile.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IRepository _repo = new Repository();

        // 用户中心
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
        // GET: /Account/WeChatLogin

        public ActionResult WeChatLogin(ScopeType scope)
        {
            var user = UserDto.GetSession();

            var client = new WeChatSnsClient();
            var openUri = client.GetOpenUrl("http://mobile.arsenal.cn/Account/WeChatAuth", scope, user.ID.ToString());

            if (!string.IsNullOrEmpty(openUri))
            {
                TempData["DataUrl"] = $"data-url={openUri}";
                return Redirect(openUri);
            }

            TempData["DataUrl"] = "data-url=/Account";
            return RedirectToAction("Index", "Account");
        }

        // 
        // GET: /Account/WeChatAuth

        public ActionResult WeChatAuth(string code, string state)
        {
            // 获取微信授权access_token
            var client = new WeChatSnsClient();
            var result = client.GetAccessToken(code);

            if (!string.IsNullOrEmpty(result))
            {
                var json = JToken.Parse(result);

                if (json["access_token"] != null && json["expires_in"] != null
                    && json["refresh_token"] != null && json["openid"] != null && json["scope"] != null)
                {
                    var scope = (ScopeType)Enum.Parse(typeof(ScopeType), json["scope"].Value<string>());
                    var accessToken = json["access_token"].Value<string>();
                    var openId = json["openid"].Value<string>();

                    UserWeChatDto.Authorize(new Guid(state), accessToken, json["expires_in"].Value<double>(),
                        json["refresh_token"].Value<string>(), openId, scope);
                }
            }

            TempData["DataUrl"] = "data-url=/";
            return RedirectToAction("Index", "Home");
        }

        // 统一用户认证登录
        // POST: /Account/Login

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                Membership mem;
                int acnUid;
                var loginSuccess = false;

                if (MembershipDto.ValidateUser(model.UserName, out mem))
                {
                    if (mem.Password.Equals(Encrypt.GetMd5Hash(model.Password)))
                    {
                        // Sign in
                        FormsAuthentication.SetAuthCookie(mem.UserName, model.RememberMe);
                        UserDto.SetSession(mem.SignIn());

                        loginSuccess = true;
                    }
                    else
                    {
                        ModelState.AddModelError("Warn", "用户名或密码不正确");
                    }
                }
                else if (MembershipDto.ValidateAcnUser(model.UserName, model.Password, out acnUid))
                {
                    // not in SSO, but in Acn Users
                    // Sync the user info, register SSO and then log in

                    if (acnUid > 0)
                    {
                        var membership = new MembershipDto();

                        MembershipCreateStatus createStatus;
                        membership.CreateAcnUser(acnUid, out createStatus);

                        if (createStatus.Equals(MembershipCreateStatus.Success))
                        {
                            FormsAuthentication.SetAuthCookie(membership.UserName, model.RememberMe);
                            UserDto.SetSession(membership.SignIn());

                            loginSuccess = true;
                        }
                        else
                        {
                            ModelState.AddModelError("Warn", ErrorCodeToString(createStatus));
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Warn", ErrorCodeToString(MembershipCreateStatus.InvalidUserName));
                    }
                }
                else
                {
                    ModelState.AddModelError("Warn", "用户名不存在或密码不正确");
                }

                // 处理登录跳转，如果开启微信授权则跳转微信OpenUrl, 否则跳转返回路径, 最后跳转首页
                if (loginSuccess)
                {
                    if (BrowserInfo.IsWeChatClient() && ConfigGlobal_Arsenal.WeChatActive)
                    {
                        TempData["DataUrl"] = $"data-url=/Account/WeChatLogin/?scope={ScopeType.snsapi_userinfo}";
                        return RedirectToAction("WeChatLogin", "Account", new { scope = ScopeType.snsapi_userinfo });
                    }

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        TempData["DataUrl"] = $"data-url={returnUrl}";
                        return Redirect(returnUrl);
                    }

                    TempData["DataUrl"] = "data-url=/";
                    return RedirectToAction("Index", "Home");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            // update user lastActivityDate
            var user = UserDto.GetSession();
            user.LastActivityDate = DateTime.Now;
            _repo.Update(user);

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

                user.CreateUser(model.UserName, model.Email, model.Password, out userKey, out createStatus);

                if (createStatus.Equals(MembershipCreateStatus.Success))
                {
                    // 注册成功后，直接将表单的用户名，存入cookie
                    FormsAuthentication.SetAuthCookie(model.UserName, false);
                    UserDto.SetSession(userKey);

                    // 如果开启微信授权则跳转微信OpenUrl, 跳转首页
                    if (BrowserInfo.IsWeChatClient() && ConfigGlobal_Arsenal.WeChatActive)
                    {
                        TempData["DataUrl"] = $"data-url=/Account/WeChatLogin/?scope={ScopeType.snsapi_base}";
                        return RedirectToAction("WeChatLogin", "Account", new { scope = ScopeType.snsapi_base });
                    }

                    TempData["DataUrl"] = "data-url=/";
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("Warn", ErrorCodeToString(createStatus));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        // 我的用户切换
        // GET: /Account/MyAvatar

        public ActionResult MyAvatar()
        {
            var model = new MyAvatarDto();

            // 判断当前用户是否有马甲
            var user = UserDto.GetSession();

            // 如果超过一个相同的微信OpenID账号，则跳转MyAvatar
            if (!string.IsNullOrEmpty(user.WeChatOpenID))
            {
                var query = _repo.Query<User>(x => x.WeChatOpenID == user.WeChatOpenID);

                if (query.Count > 1)
                {
                    var mapper = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDto>()).CreateMapper();

                    var list = mapper.Map<IEnumerable<UserDto>>(query.AsEnumerable());

                    model.Avatars = list;

                    return View(model);
                }
            }

            TempData["DataUrl"] = "data-url=/Account";
            return RedirectToAction("Index", "Account");
        }

        // 切换用户账号
        // GET: /Account/AvatarLogin

        public ActionResult AvatarLogin(Guid userGuid)
        {
            var user = UserDto.GetSession();
            var newUser = UserDto.Single(userGuid);

            if (user.WeChatOpenID != null && newUser.WeChatOpenID != null &&
                user.WeChatOpenID == newUser.WeChatOpenID)
            {
                #region 注销当前用户

                user.LastActivityDate = DateTime.Now;
                _repo.Update(user);

                FormsAuthentication.SignOut();
                Session.Abandon();

                #endregion

                #region 登录新用户

                var mem = MembershipDto.Single(userGuid);

                FormsAuthentication.SetAuthCookie(mem.UserName, createPersistentCookie: true);
                UserDto.SetSession(mem.SignIn());

                newUser.IsAnonymous = true;
                _repo.Update(newUser);

                #endregion
            }

            TempData["DataUrl"] = "data-url=/";
            return RedirectToAction("Index", "Home");
        }


        // 用户信息
        // GET: /Account/UserProfile

        public ActionResult UserProfile()
        {
            var model = new UserProfileDto();

            var membership = MembershipDto.Single(User.Identity.Name);
            var user = UserDto.GetSession();

            model.RealName = user.MemberName;
            model.Mobile = membership.Mobile;
            model.Email = membership.Email;

            return View(model);
        }

        //
        // POST: /Account/UserProfile

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserProfile(UserProfileDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var membership = MembershipDto.Single(User.Identity.Name);
                    var user = UserDto.GetSession();

                    if (membership != null && user != null)
                    {
                        user.MemberName = model.RealName;
                        membership.Email = model.Email;
                        membership.Mobile = model.Mobile;

                        _repo.Update(user);
                        _repo.Update(membership);

                        TempData["DataUrl"] = "data-url=/Account";
                        return RedirectToAction("Index", "Account");
                    }
                    ModelState.AddModelError("Warn", "当前用户不存在");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Warn", ex.Message);
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
                try
                {
                    var membership = MembershipDto.Single(User.Identity.Name);

                    if (membership != null)
                    {
                        // ChangePassword will throw an exception rather
                        // than return false in certain failure scenarios.
                        if (MembershipDto.ChangePassword(membership, model.OldPassword, model.NewPassword))
                        {
                            TempData["DataUrl"] = "data-url=/Account/ChangePasswordSuccess";
                            return RedirectToAction("ChangePasswordSuccess", "Account");
                        }
                    }
                    else
                    {
                        //changePasswordSucceeded = false;

                        ModelState.AddModelError("Warn", "当前用户不存在");
                    }
                }
                catch (Exception ex)
                {
                    //changePasswordSucceeded = false;

                    ModelState.AddModelError("Warn", ex.Message);
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
                    return "此用户名已存在";

                case MembershipCreateStatus.DuplicateEmail:
                    return "此邮箱已被其他用户注册使用";

                case MembershipCreateStatus.InvalidPassword:
                    return "此密码不符合密码规则";

                case MembershipCreateStatus.InvalidEmail:
                    return "此邮箱地址无效";

                case MembershipCreateStatus.InvalidAnswer:
                    return "此密码问题答案无效";

                case MembershipCreateStatus.InvalidQuestion:
                    return "此密码问题无效";

                case MembershipCreateStatus.InvalidUserName:
                    return "此用户名无效";

                case MembershipCreateStatus.ProviderError:
                    return "注册过程出错，请联系管理员";

                case MembershipCreateStatus.UserRejected:
                    return "此申请被取消";

                default:
                    return "发生未知错误，请联系管理员";
            }
        }

        #endregion
    }
}