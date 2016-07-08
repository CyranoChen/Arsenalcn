using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Arsenal.Service;
using Arsenalcn.Core;
using Arsenalcn.Core.Logger;
using Arsenalcn.Core.Scheduler;
using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Arsenal.Scheduler
{
    internal class AutoUpdateUserInfo : ISchedule
    {
        private readonly ILog _log = new AppLog();

        public void Execute(object state)
        {
            var logInfo = new LogInfo
            {
                MethodInstance = MethodBase.GetCurrentMethod(),
                ThreadInstance = Thread.CurrentThread
            };

            try
            {
                _log.Info("Scheduler Start: (AutoUpdateUserInfo)", logInfo);

                // 同步所有未取得实名信息的用户资料
                SyncUserMemberName();

                // 同步所有微信用户信息
                SyncUserWeChatInfo();


                _log.Info("Scheduler End: (AutoUpdateUserInfo)", logInfo);
            }
            catch (Exception ex)
            {
                _log.Warn(ex, logInfo);
            }
        }

        private static void SyncUserMemberName()
        {
            IRepository repo = new Repository();

            var users = repo.Query<User>(x => x.AcnID.HasValue).FindAll(x => !x.MemberID.HasValue);

            if (users.Count > 0)
            {
                foreach (var u in users)
                {
                    try
                    {
                        u.SyncUserByMember();
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
        }

        private static void SyncUserWeChatInfo()
        {
            IRepository repo = new Repository();

            // 7天来未更新的微信会员 或 微信昵称为空的会员
            var usersWeChat = repo.Query<UserWeChat>(x => x.LastAuthorizeDate < DateTime.Now.AddDays(-7));

            var users = repo.Query<User>(x => x.WeChatOpenID != "")
                            .FindAll(x => string.IsNullOrEmpty(x.WeChatNickName) || usersWeChat.Exists(y => y.ID == x.ID));

            if (users.Count > 0 && ConfigGlobal_Arsenal.WeChatActive)
            {
                // 根据users，生成输入参数 
                //{"user_list": [
                //        { "openid": "otvxTs4dckWG7imySrJd6jSi0CWE", "lang": "zh-CN"}, 
                //        { "openid": "otvxTs_JZ6SEiP0imdhpi50fuSZg",  "lang": "zh-CN" }
                //    ]
                //}

                var mapper = UserWeChatRequestDto.ConfigMapper().CreateMapper();

                var userList = new { user_list = mapper.Map<IEnumerable<UserWeChatRequestDto>>(users.DistinctBy(x => x.WeChatOpenID)).ToList() };
                var openIds = JsonConvert.SerializeObject(userList);

                var client = new WeChatApiClient();
                var result = client.BatchGetUserInfo(openIds);

                var json = JToken.Parse(result);
                if (!string.IsNullOrEmpty(json["user_info_list"].ToString()))
                {
                    var list = JsonConvert.DeserializeObject<List<UserWeChatResponseDto>>(json["user_info_list"].ToString());

                    if (list.Count > 0)
                    {
                        foreach (var u in users)
                        {
                            var uResp = list.Find(x => x.openid == u.WeChatOpenID);
                            var uWeChat = usersWeChat.Find(x => x.ID == u.ID);

                            if (uResp != null)
                            {
                                u.WeChatOpenID = uResp.openid;
                                u.WeChatNickName = uResp.nickname;

                                repo.Update(u);

                                if (uWeChat != null)
                                {
                                    uWeChat.Gender = uResp.sex;
                                    uWeChat.LastAuthorizeDate = DateTime.Now;
                                    uWeChat.City = uResp.city;
                                    uWeChat.Province = uResp.province;
                                    uWeChat.Country = uResp.country;
                                    uWeChat.HeadImgUrl = uResp.headimgurl;
                                    uWeChat.UnionID = uResp.unionid;

                                    repo.Update(uWeChat);
                                }
                                else
                                {
                                    var instance = new UserWeChat
                                    {
                                        ID = u.ID,
                                        UserName = u.UserName,
                                        LastAuthorizeDate = DateTime.Now,
                                        AccessToken = string.Empty,
                                        AccessTokenExpiredDate = DateTime.MinValue,
                                        RefreshToken = string.Empty,
                                        RefreshTokenExpiredDate = DateTime.MinValue,
                                        Gender = uResp.sex,
                                        Province = uResp.province,
                                        City = uResp.city,
                                        Country = uResp.country,
                                        HeadImgUrl = uResp.headimgurl,
                                        Privilege = string.Empty,
                                        UnionID = uResp.unionid
                                    };

                                    repo.Insert(instance);
                                }
                            }
                        }
                    }
                }
            }
        }

        private class UserWeChatRequestDto
        {
            public static MapperConfiguration ConfigMapper()
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserWeChatRequestDto>()
                    .ForMember(d => d.openid, opt => opt.MapFrom(s => s.WeChatOpenID)));

                return config;
            }

            public string openid { get; set; }

            public string lang => "zh_CN";
        }

        private class UserWeChatResponseDto
        {
            public bool subscribe { get; set; }
            public string openid { get; set; }
            public string nickname { get; set; }
            public short sex { get; set; }
            public string language { get; set; }
            public string city { get; set; }
            public string province { get; set; }
            public string country { get; set; }
            public string headimgurl { get; set; }
            public string subscribe_time { get; set; }
            public string remark { get; set; }
            public string unionid { get; set; }
            public int groupid { get; set; }
            public List<int> tagid_list { get; set; }
        }

    }
}