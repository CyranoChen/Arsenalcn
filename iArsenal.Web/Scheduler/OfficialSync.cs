using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using Arsenalcn.Core;
using Arsenalcn.Core.Logger;
using Arsenalcn.Core.Scheduler;
using Arsenalcn.Core.Utility;
using iArsenal.Service;

namespace iArsenal.Scheduler
{
    internal class OfficialSync : ISchedule
    {
        private readonly ILog _log = new AppLog();
        private readonly IRepository _repo = new Repository();

        public void Execute(object state)
        {
            var logInfo = new LogInfo
            {
                MethodInstance = MethodBase.GetCurrentMethod(),
                ThreadInstance = Thread.CurrentThread
            };

            try
            {
                _log.Info("Scheduler Start: (OfficialSync)", logInfo);

                var rand = new Random(Guid.NewGuid().GetHashCode());
                var result = rand.Next(0, 900);
                var season = "1617";

                if (result < 80)
                {
                    var member = GetUnSyncMemberByRandom();

                    if (member != null)
                    {
                        var success = SyncOfficialMemberInfo(member);

                        // logger the result of Official Synchronization
                        ILog uLog = new UserLog();

                        var logPara = new LogInfo
                        {
                            MethodInstance = MethodBase.GetCurrentMethod(),
                            ThreadInstance = Thread.CurrentThread,
                            UserClient = new UserClientInfo
                            {
                                UserID = member.AcnID,
                                UserName = member.AcnName,
                                UserIP = "127.0.0.1",
                                UserBrowser = string.Empty,
                                UserOS = string.Empty
                            }
                        };

                        if (success)
                        {
                            member.OfficialSync = season;
                            _repo.Update(member);

                            uLog.Info($"官方会员信息同步成功 (随机码：{result}, {season}会员：{member.Name})", logPara);
                        }
                        else
                        {
                            uLog.Error($"官方会员信息同步失败 (随机码：{result}, {season}会员：{member.Name})", logPara);
                        }
                    }
                }

                _log.Info($"Scheduler End: (OfficialSync ({result}))", logInfo);
            }
            catch (Exception ex)
            {
                _log.Warn(ex, logInfo);
            }
        }

        private Member GetUnSyncMemberByRandom()
        {
            // Get the unsync member instance
            var attr = Repository.GetTableAttr<Member>();

            string sql =
                $"SELECT TOP 1 * FROM {attr.Name} WHERE OfficialSync = '0000' AND Evalution = 0 AND MemberType <> 2 AND IsActive = 1 ORDER BY NEWID()";

            var dapper = new DapperHelper();

            var reader = dapper.ExecuteReader(sql);

            return reader?.DataReaderMapTo<Member>().FirstOrDefault();
        }

        private bool SyncOfficialMemberInfo(Member m)
        {
            var serviceUrl = "http://campaign.arsenal.com/webApp/afcSupportersClubV1";
            var req = (HttpWebRequest)WebRequest.Create(serviceUrl);

            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";

            #region Member Property getting and setting

            // birthDate
            var birthDate = DateTime.Parse("1970-01-01").AddDays((new Random()).Next((DateTime.Parse("2000-12-31") - DateTime.Parse("1970-01-01")).Days));

            if (m.Birthday.HasValue)
            {
                birthDate = m.Birthday.Value;
            }

            // address2
            var address2 = "无";

            if (!string.IsNullOrEmpty(m.IP))
            {
                var ipInfo = IPLocation.GetIPInfo(m.IP);

                if (!ipInfo.Contains("【未知】"))
                {
                    address2 = ipInfo.Contains(" ") ? ipInfo.Substring(0, ipInfo.IndexOf(" ", StringComparison.Ordinal)) : ipInfo;
                }
            }

            #endregion

            #region Set PostParas

            var postData = new Dictionary<string, string>
            {
                {"recipient__birthDate", birthDate.ToString("dd/MM/yyyy") },
                {"recipient__email", !string.IsNullOrEmpty(m.Email)? m.Email : "webmaster@arsenalcn.com"},
                {"recipient__firstName", m.Name.Substring(1, m.Name.Length-1)},
                {"recipient__lastName", m.Name.Substring(0, 1)},
                {"recipient__mobilePhone", !string.IsNullOrEmpty(m.Mobile)? m.Mobile : m.Telephone},
                {"recipient__salutation", m.Gender? "Mr" : "Ms"},
                {"recipient_Country", "{43AE96A7-1EF1-E111-BFCA-005056A73E99}"},
                {"recipient_location__address1", !string.IsNullOrEmpty(m.Address)? m.Address : "无"},
                {"recipient_location__address2", address2},
                {"recipient_location__address3", ""},
                {"recipient_location__city",  address2},
                {"recipient_location__zipCode", "20000X"},
                {"transition", "next"},
                {"userAction", "next"},
                {"vars_clubOfferSubAFC", "0"},
                {"vars_clubOfferSubAFCPartner", "0"},
                {"vars_membershipNum", ""},
                {"vars_membershipTypeId", ""},
                {"vars_supportersClub", "China (Shanghai)"},
                {"vars_supportersClubOther", ""},
                {"vars_supportersMembershipNum", m.ID.ToString()}
            };

            #endregion

            #region Set ctx

            var ctx =
                $"<ctx lang=\"en\" date=\"{DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ")}\" _target=\"web\" webApp-id=\"733191580\" _folderModel=\"nmsRecipient\">" +
                    $"<userInfo xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ns=\"urn:xtk:session\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" datakitInDatabase=\"true\" homeDir=\"\" instanceLocale=\"en-US\" locale=\"en-US\" loginCS=\"Web applications agent (webapp)\" loginId=\"3311\" noConsoleCnx=\"true\" orgUnitId=\"0\" theme=\"\" timezone=\"Europe/London\"><login-right right=\"admin\"/></userInfo>" +
                    $"<timezone current=\"Europe/London\" changed=\"false\"/>" +
                    $"<vars>* = Required Field" +
                        $"<clubOfferSubscription>false</clubOfferSubscription>" +
                        $"<clubOfferSubscription_init>false</clubOfferSubscription_init>" +
                        $"<supportersClub>{postData["vars_supportersClub"]}</supportersClub>" +
                    $"</vars>" +
                    $"<activityHistory><activity name=\"page\"/><activity name=\"script3\"/><activity name=\"query2\"/><activity name=\"query\"/><activity name=\"prefill\"/><activity name=\"start\"/></activityHistory>" +
                    $"<query><membershipType membershipTypeId=\"{{BB81D421-A7EB-E111-8167-005056A73E99}}\" name=\"Away Ticket Scheme\"/><membershipType membershipTypeId=\"{{BC81D421-A7EB-E111-8167-005056A73E99}}\" name=\"Bondholder Only\"/><membershipType membershipTypeId=\"{{C5146428 - A7EB - E111 - 8167 - 005056A73E99}}\" name=\"Cannon Club\"/><membershipType membershipTypeId=\"{{C6146428 - A7EB - E111 - 8167 - 005056A73E99}}\" name=\"Diamond Club\"/><membershipType membershipTypeId=\"{{959B2230-A7EB-E111-8167-005056A73E99}}\" name=\"Executive Box Level\"/><membershipType membershipTypeId=\"{{969B2230-A7EB-E111-8167-005056A73E99}}\" name=\"Gold\"/><membershipType membershipTypeId=\"{{C9C82136-A7EB-E111-8167-005056A73E99}}\" name=\"Junior Gunners\"/><membershipType membershipTypeId=\"{{CAC82136-A7EB-E111-8167-005056A73E99}}\" name=\"Platinum\"/><membershipType membershipTypeId=\"{{6D0DC93C-A7EB-E111-8167-005056A73E99}}\" name=\"Purple\"/><membershipType membershipTypeId=\"{{6E0DC93C-A7EB-E111-8167-005056A73E99}}\" name=\"Red\"/><membershipType membershipTypeId=\"{{6F0DC93C-A7EB-E111-8167-005056A73E99}}\" name=\"Silver\"/><membershipType membershipTypeId=\"{{5DE1B444-A7EB-E111-8167-005056A73E99}}\" name=\"Travel Club\"/><membershipType membershipTypeId=\"{{5EE1B444-A7EB-E111-8167-005056A73E99}}\" name=\"Waiting List\"/><membershipType name=\"Please select\"/><membershipType name=\"None\" membershipTypeId=\"None\"/><membershipType name=\"Digital\" membershipTypeId=\"Digital\"/></query>" +
                    $"<supportersClubEnum><enumValue label=\"025\" name=\"025\"/><enumValue label=\"2013 0400 0001 0030\" name=\"2013 0400 0001 0030\"/><enumValue label=\"367\" name=\"367\"/><enumValue label=\"ABC123\" name=\"ABC123\"/><enumValue label=\"Algeria\" name=\"Algeria\"/><enumValue label=\"America\" name=\"America\"/><enumValue label=\"Antrim\" name=\"Antrim\"/><enumValue label=\"arsenal\" name=\"Arsenal\"/><enumValue label=\"Arsenal Algeria\" name=\"Arsenal Algeria\"/><enumValue label=\"Arsenal Azerbaijan\" name=\"Arsenal Azerbaijan\"/><enumValue label=\"Arsenal Azerbaijan Supporters Club\" name=\"Arsenal Azerbaijan Supporters Club\"/><enumValue label=\"Arsenal Azerbaijani Supporters Club\" name=\"Arsenal Azerbaijani Supporters Club\"/><enumValue label=\"Arsenal Chennai Supporters Club\" name=\"Arsenal Chennai Supporters Club\"/><enumValue label=\"Arsenal Mumbai\" name=\"Arsenal Mumbai\"/><enumValue label=\"Arsenal Northern Suburbs\" name=\"Arsenal Northern Suburbs\"/><enumValue label=\"arsenal oman\" name=\"Arsenal oman\"/><enumValue label=\"Arsenal Supporters Club, Ghana \" name=\"Arsenal Supporters Club, Ghana \"/><enumValue label=\"Arsenal_Oman\" name=\"Arsenal_Oman\"/><enumValue label=\"arsensl\" name=\"arsensl\"/><enumValue label=\"Athlone\" name=\"Athlone\"/><enumValue label=\"Australia\" name=\"Australia\"/><enumValue label=\"Austria Tirol\" name=\"Austria Tirol\"/><enumValue label=\"Austria Vienna \" name=\"Austria Vienna \"/><enumValue label=\"Bahrain\" name=\"Bahrain\"/><enumValue label=\"Banbridge\" name=\"Banbridge\"/><enumValue label=\"Bangladesh\" name=\"Bangladesh\"/><enumValue label=\"Barbados\" name=\"Barbados\"/><enumValue label=\"barcelona\" name=\"barcelona\"/><enumValue label=\"BC123\" name=\"BC123\"/><enumValue label=\"Belarus\" name=\"Belarus\"/><enumValue label=\"Belfast\" name=\"Belfast\"/><enumValue label=\"Belgium\" name=\"Belgium\"/><enumValue label=\"Bosnia\" name=\"Bosnia\"/><enumValue label=\"Brazil\" name=\"Brazil\"/><enumValue label=\"Bulgaria\" name=\"Bulgaria\"/><enumValue label=\"Canada\" name=\"Canada\"/><enumValue label=\"Cape Town\" name=\"Cape Town\"/><enumValue label=\"Carrickfergus\" name=\"Carrickfergus\"/><enumValue label=\"Chennai\" name=\"Chennai\"/><enumValue label=\"Chennai India\" name=\"Chennai India\"/><enumValue label=\"China (Shanghai)\" name=\"China (Shanghai)\"/><enumValue label=\"Co. Armagh\" name=\"Co. Armagh\"/><enumValue label=\"Coleraine\" name=\"Coleraine\"/><enumValue label=\"Congolese Refugees\" name=\"Congolese Refugees\"/><enumValue label=\"Cork City\" name=\"Cork City\"/><enumValue label=\"Cornwall &amp; Devon\" name=\"Cornwall &amp; Devon\"/><enumValue label=\"Costa Blanca\" name=\"Costa Blanca\"/><enumValue label=\"County Wicklow\" name=\"County Wicklow\"/><enumValue label=\"Croatia\" name=\"Croatia\"/><enumValue label=\"Cyprus\" name=\"Cyprus\"/><enumValue label=\"Czech &amp; Slovak\" name=\"Czech &amp; Slovak\"/><enumValue label=\"Daventry\" name=\"Daventry\"/><enumValue label=\"Delhi\" name=\"Delhi\"/><enumValue label=\"Denmark\" name=\"Denmark\"/><enumValue label=\"Derry City\" name=\"Derry City\"/><enumValue label=\"Dingle\" name=\"Dingle\"/><enumValue label=\"Donegal\" name=\"Donegal\"/><enumValue label=\"Dover\" name=\"Dover\"/><enumValue label=\"Dover Gooners\" name=\"Dover Gooners\"/><enumValue label=\"Drogheda \" name=\"Drogheda \"/><enumValue label=\"Dublin\" name=\"Dublin\"/><enumValue label=\"Dundalk\" name=\"Dundalk\"/><enumValue label=\"East Cork\" name=\"East Cork\"/><enumValue label=\"East Sussex\" name=\"East Sussex\"/><enumValue label=\"Egypt\" name=\"Egypt\"/><enumValue label=\"Essex\" name=\"Essex\"/><enumValue label=\"Ethiopia\" name=\"Ethiopia\"/><enumValue label=\"Faroe Islands\" name=\"Faroe Islands\"/><enumValue label=\"Fermanagh\" name=\"Fermanagh\"/><enumValue label=\"Finland\" name=\"Finland\"/><enumValue label=\"France\" name=\"France\"/><enumValue label=\"France (Alsace)\" name=\"France (Alsace)\"/><enumValue label=\"France (Britanny)\" name=\"France (Britanny)\"/><enumValue label=\"Germany\" name=\"Germany\"/><enumValue label=\"Ghana\" name=\"Ghana\"/><enumValue label=\"Gibraltar\" name=\"Gibraltar\"/><enumValue label=\"Gloucester\" name=\"Gloucester\"/><enumValue label=\"Gorey\" name=\"Gorey\"/><enumValue label=\"Guangdong\" name=\"Guangdong\"/><enumValue label=\"Gunnerz\" name=\"Gunnerz\"/><enumValue label=\"Hampshire\" name=\"Hampshire\"/><enumValue label=\"Hellas\" name=\"Hellas\"/><enumValue label=\"Herts and Beds\" name=\"Herts and Beds\"/><enumValue label=\"Hong Kong\" name=\"Hong Kong\"/><enumValue label=\"Hungary (Pannonia)\" name=\"Hungary (Pannonia)\"/><enumValue label=\"Iceland\" name=\"Iceland\"/><enumValue label=\"India - Chennai\" name=\"India - Chennai\"/><enumValue label=\"India (Bangalore)\" name=\"India (Bangalore)\"/><enumValue label=\"India Chennai\" name=\"India Chennai\"/><enumValue label=\"India-Chennai\" name=\"India-Chennai\"/><enumValue label=\"Indonesia\" name=\"Indonesia\"/><enumValue label=\"Iran\" name=\"Iran\"/><enumValue label=\"Isle of Man\" name=\"Isle of Man\"/><enumValue label=\"Israel\" name=\"Israel\"/><enumValue label=\"Italy\" name=\"Italy\"/><enumValue label=\"Japan\" name=\"Japan\"/><enumValue label=\"Jersey\" name=\"Jersey\"/><enumValue label=\"Kenya\" name=\"Kenya\"/><enumValue label=\"Kilkenny \" name=\"Kilkenny \"/><enumValue label=\"Korea\" name=\"Korea\"/><enumValue label=\"Kosovo\" name=\"Kosovo\"/><enumValue label=\"Kovai gooners\" name=\"Kovai Gooners\"/><enumValue label=\"Kuwait\" name=\"Kuwait\"/><enumValue label=\"Laos\" name=\"Laos\"/><enumValue label=\"Lebanon\" name=\"Lebanon\"/><enumValue label=\"Liberia\" name=\"Liberia\"/><enumValue label=\"Limavady\" name=\"Limavady\"/><enumValue label=\"Limerick\" name=\"Limerick\"/><enumValue label=\"London\" name=\"London\"/><enumValue label=\"Macedonia\" name=\"Macedonia\"/><enumValue label=\"Maidstone\" name=\"Maidstone\"/><enumValue label=\"Malawi\" name=\"Malawi\"/><enumValue label=\"Malaysia\" name=\"Malaysia\"/><enumValue label=\"Mallorca Supporters Club\" name=\"Mallorca Supporters Club\"/><enumValue label=\"Malta\" name=\"Malta\"/><enumValue label=\"Manchester\" name=\"Manchester\"/><enumValue label=\"Mauritius\" name=\"Mauritius\"/><enumValue label=\"Milton Keynes\" name=\"Milton Keynes\"/><enumValue label=\"Mumbai\" name=\"Mumbai\"/><enumValue label=\"Muscat\" name=\"Muscat\"/><enumValue label=\"Myanmar\" name=\"Myanmar\"/><enumValue label=\"N/a\" name=\"N/a\"/><enumValue label=\"Nepal\" name=\"Nepal\"/><enumValue label=\"New York City\" name=\"New York City\"/><enumValue label=\"New Zealand\" name=\"New Zealand\"/><enumValue label=\"Newry &amp; Mourne\" name=\"Newry &amp; Mourne\"/><enumValue label=\"Nigeria\" name=\"Nigeria\"/><enumValue label=\"No\" name=\"No\"/><enumValue label=\"non\" name=\"non\"/><enumValue label=\"none\" name=\"NONE\"/><enumValue label=\"Norfolk\" name=\"Norfolk\"/><enumValue label=\"North Down\" name=\"North Down\"/><enumValue label=\"North Kerry\" name=\"North Kerry\"/><enumValue label=\"North Wales\" name=\"North Wales\"/><enumValue label=\"North West\" name=\"North West\"/><enumValue label=\"North Wiltshire\" name=\"North Wiltshire\"/><enumValue label=\"North-Westmeath\" name=\"North-Westmeath\"/><enumValue label=\"Norway\" name=\"Norway\"/><enumValue label=\"Ormond\" name=\"Ormond\"/><enumValue label=\"Other\" name=\"other\"/><enumValue label=\"Pakistan\" name=\"Pakistan\"/><enumValue label=\"Philadelphia\" name=\"Philadelphia\"/><enumValue label=\"Philippines\" name=\"Philippines\"/><enumValue label=\"Playa Flamenca\" name=\"Playa Flamenca\"/><enumValue label=\"Poland\" name=\"Poland\"/><enumValue label=\"Poland\" name=\"Poland\"/><enumValue label=\"Portugal\" name=\"Portugal\"/><enumValue label=\"Real madrid\" name=\"Real madrid\"/><enumValue label=\"Romania\" name=\"Romania\"/><enumValue label=\"Russia\" name=\"Russia\"/><enumValue label=\"Saudi Arabia\" name=\"Saudi Arabia\"/><enumValue label=\"Scotland\" name=\"Scotland\"/><enumValue label=\"Serbia\" name=\"Serbia\"/><enumValue label=\"Seven Towers ASC Ballymena\" name=\"Seven Towers ASC Ballymena\"/><enumValue label=\"Seychelles\" name=\"Seychelles\"/><enumValue label=\"Shannonside\" name=\"Shannonside\"/><enumValue label=\"Sichuan\" name=\"Sichuan\"/><enumValue label=\"Sierra Leone\" name=\"Sierra Leone\"/><enumValue label=\"Singapore\" name=\"Singapore\"/><enumValue label=\"Sligo\" name=\"Sligo\"/><enumValue label=\"Slovenia\" name=\"Slovenia\"/><enumValue label=\"South Africa\" name=\"South Africa\"/><enumValue label=\"South Donegal\" name=\"South Donegal\"/><enumValue label=\"South East Ireland\" name=\"South East Ireland\"/><enumValue label=\"South Italy\" name=\"South Italy\"/><enumValue label=\"South Sudan\" name=\"South Sudan\"/><enumValue label=\"South Wales\" name=\"South Wales\"/><enumValue label=\"Sudan\" name=\"Sudan\"/><enumValue label=\"Sweden\" name=\"Sweden\"/><enumValue label=\"Switzerland\" name=\"Switzerland\"/><enumValue label=\"Tanzania\" name=\"Tanzania\"/><enumValue label=\"Test\" name=\"tEST\"/><enumValue label=\"The Gambia\" name=\"The Gambia\"/><enumValue label=\"Thessaloniki\" name=\"Thessaloniki\"/><enumValue label=\"Tipperary Town &amp; District\" name=\"Tipperary Town &amp; District\"/><enumValue label=\"Togo\" name=\"Togo\"/><enumValue label=\"Tralee\" name=\"Tralee\"/><enumValue label=\"Trinidad &amp; Tobago\" name=\"Trinidad &amp; Tobago\"/><enumValue label=\"Tunisia\" name=\"Tunisia\"/><enumValue label=\"U.A.E\" name=\"U.A.E\"/><enumValue label=\"Uganda\" name=\"Uganda\"/><enumValue label=\"usmah\" name=\"usmah\"/><enumValue label=\"usmh\" name=\"usmh\"/><enumValue label=\"Vietnam\" name=\"Vietnam\"/><enumValue label=\"West Cork\" name=\"West Cork\"/><enumValue label=\"West Midlands\" name=\"West Midlands\"/><enumValue label=\"West Sussex\" name=\"West Sussex\"/><enumValue label=\"Westport\" name=\"Westport\"/><enumValue label=\"Wexford\" name=\"Wexford\"/><enumValue label=\"Yorkshire\" name=\"Yorkshire\"/><enumValue label=\"Zambia\" name=\"Zambia\"/><enumValue label=\"Zimbabwe\" name=\"Zimbabwe\"/><enumValue label=\"Please select\"/></supportersClubEnum>" +
                    $"<defaultValues page=\"true\"/>" +
                    $"<recipient " +
                        $"salutation=\"{postData["recipient__salutation"]}\" " +
                        $"firstName=\"{postData["recipient__firstName"]}\" " +
                        $"lastName=\"{postData["recipient__lastName"]}\" " +
                        $"birthDate=\"{postData["recipient__birthDate"]}\" " +
                        $"email=\"{postData["recipient__email"]}\" " +
                        $"mobilePhone=\"{postData["recipient__mobilePhone"]}\">" +
                        $"<Country>{{43AE96A7-1EF1-E111-BFCA-005056A73E99}}</Country>" +
                        $"<location address1=\"{postData["recipient_location__address1"]}\" " +
                            $"address2=\"{postData["recipient_location__address2"]}\" " +
                            $"city=\"{postData["recipient_location__city"]}\" " +
                            $"zipCode=\"{postData["recipient_location__zipCode"]}\"/>" +
                    $"</recipient>" +
                $"</ctx>";

            #endregion

            // UrlEncode
            var strParameter = new StringBuilder();

            foreach (var para in postData)
            {
                strParameter.Append(
                    $"{para.Key}={HttpUtility.UrlEncode(para.Value, Encoding.UTF8)?.Replace("%20", "+")}&");
            }

            strParameter.Append($"ctx={HttpUtility.UrlEncode(ctx, Encoding.UTF8)?.Replace("%20", "+")}");

            // resultPara
            var resultPara = strParameter.ToString().Replace("(", "%28").Replace(")", "%29");

            var encodedBytes = Encoding.UTF8.GetBytes(resultPara);
            req.ContentLength = encodedBytes.Length;

            // Write encoded data into request stream
            var requestStream = req.GetRequestStream();
            requestStream.Write(encodedBytes, 0, encodedBytes.Length);
            requestStream.Close();

            using (var response = req.GetResponse())
            {
                var receiveStream = response.GetResponseStream();

                if (receiveStream != null)
                {
                    var readStream = new StreamReader(receiveStream, Encoding.UTF8);

                    var result = readStream.ReadToEnd();

                    return result.ToLower().Contains("thank");
                }
            }

            return false;
        }
    }
}