using System;
using System.Collections.Generic;
using System.Linq;
using Arsenalcn.Core;
using Arsenalcn.Core.Dapper;
using Arsenalcn.Core.Extension;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Arsenal.Service.Tests.Scheduler
{
    [TestClass]
    public class AutoUpdateUserInfoTest
    {
        [TestMethod]
        public void AutoUpdateUserInfo_Test()
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
                var userList = mapper.Map<IEnumerable<UserWeChatRequestDto>>(users.DistinctBy(x => x.WeChatOpenID))
                    .ToList();

                const int pageSize = 100;
                for (var index = 0; index <= userList.Count / pageSize; index++)
                {
                    var openIds = JsonConvert.SerializeObject(new
                    {
                        user_list = userList.Skip(index*pageSize).Take(pageSize).ToList()
                    });

                    var client = new WeChatApiClient();
                    var result = client.BatchGetUserInfo(openIds);

                    var json = JToken.Parse(result);
                    if (!string.IsNullOrEmpty(json["user_info_list"].ToString()))
                    {
                        var list = JsonConvert.DeserializeObject<List<UserWeChatResponseDto>>(json["user_info_list"].ToString());

                        Assert.IsTrue(list.Count > 0);
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
            public string unionid { get; set; }
            public string remark { get; set; }
            public int groupid { get; set; }
            public List<int> tagid_list { get; set; }
        }
    }
}
