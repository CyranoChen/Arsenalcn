using System;
using System.Data.SqlClient;
using Arsenalcn.Core;
using Newtonsoft.Json.Linq;

namespace Arsenal.Service
{
    [DbSchema("Arsenalcn_User", Key = "UserGuid", Sort = "LastActivityDate DESC")]
    public class User : Entity<Guid>
    {
        public void SyncUserByMember()
        {
            if (AcnID != null)
            {
                using (var trans = DapperHelper.MarsConnection.BeginTransaction())
                {
                    try
                    {
                        IRepository repo = new Repository();

                        var client = new RestClient();
                        const string uri = "http://www.iarsenal.com/servermembercheck.ashx";

                        var responseResult = client.ApiGet($"{uri}?acnid={AcnID.Value}");
                        var json = JToken.Parse(responseResult);

                        if (json["ID"] != null && AcnUserName == json["AcnName"].Value<string>())
                        {
                            if (MemberID != json["ID"].Value<int>() || MemberName != json["Name"].Value<string>())
                            {
                                MemberID = json["ID"].Value<int>();
                                MemberName = json["Name"].Value<string>();

                                repo.Update(this, trans);
                            }

                            var mem = repo.Single<Membership>(ID);

                            if (mem.Mobile != json["Mobile"].Value<string>() ||
                                mem.Email != json["Email"].Value<string>())
                            {
                                mem.Mobile = json["Mobile"].Value<string>();
                                mem.Email = json["Email"].Value<string>();

                                repo.Update(mem, trans);
                            }

                            trans.Commit();
                        }
                    }
                    catch
                    {
                        trans.Rollback();

                        throw;
                    }
                }
            }
        }

        #region Members and Properties

        [DbColumn("UserName")]
        public string UserName { get; set; }

        [DbColumn("IsAnonymous")]
        public bool IsAnonymous { get; set; }

        [DbColumn("LastActivityDate")]
        public DateTime LastActivityDate { get; set; }

        [DbColumn("AcnID")]
        public int? AcnID { get; set; }

        [DbColumn("AcnUserName")]
        public string AcnUserName { get; set; }

        [DbColumn("MemberID")]
        public int? MemberID { get; set; }

        [DbColumn("MemberName")]
        public string MemberName { get; set; }

        [DbColumn("WeChatOpenID")]
        public string WeChatOpenID { get; set; }

        [DbColumn("WeChatNickName")]
        public string WeChatNickName { get; set; }

        #endregion
    }
}