using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Web;

using Arsenalcn.ClubSys.Entity;
using Arsenalcn.ClubSys.Service;



namespace Arsenalcn.ClubSys.Web
{
    public partial class ServerXml : Common.BasePage
    {
        private int ClubID
        {
            get
            {
                int tmp;
                if (int.TryParse(Request.QueryString["ClubID"], out tmp))
                    return tmp;
                else
                {
                    return -1;
                }
            }
        }

        private int UserID
        {
            get
            {
                int tmp;
                if (int.TryParse(Request.QueryString["UserID"], out tmp))
                    return tmp;
                else
                {
                    return -1;
                }
            }
        }

        private string PlayerGuid
        {
            get
            {
                return Request.QueryString["PlayerGuid"];
            }
        }

        private string VideoGuid
        {
            get
            {
                return Request.QueryString["VideoGuid"];
            }
        }

        private int CardID
        {
            get
            {
                int tmp;
                if (int.TryParse(Request.QueryString["CardID"], out tmp))
                    return tmp;
                else
                {
                    return -1;
                }
            }
        }

        private int UserVideoID
        {
            get
            {
                int tmp;
                if (int.TryParse(Request.QueryString["UserVideoID"], out tmp))
                    return tmp;
                else
                {
                    return -1;
                }
            }
        }

        private bool CurrArsenalPlayer
        {
            get
            {
                bool tmp;
                if (bool.TryParse(Request.QueryString["CurrArsenalPlayer"], out tmp))
                    return tmp;
                else
                {
                    return false;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ClubID > 0)
            {
                //output club info

                Club club = ClubLogic.GetClubInfo(ClubID);

                if (club != null)
                {
                    RankAlgorithm ra = new RankAlgorithm(club);

                    StringBuilder xmlContent = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    xmlContent.AppendFormat("<RankChart ClubID=\"{0}\" RankPoint=\"{1}\">", club.ID, club.RankScore);
                    xmlContent.AppendFormat("<RankItem name=\"会员数\" value=\"{0}\" />", ra.MemberCountRank);
                    xmlContent.AppendFormat("<RankItem name=\"总财富\" value=\"{0}\" />", ra.ClubFortuneRank);
                    xmlContent.AppendFormat("<RankItem name=\"总积分\" value=\"{0}\" />", ra.MemberCreditRank);
                    xmlContent.AppendFormat("<RankItem name=\"总RP值\" value=\"{0}\" />", ra.MemberRPRank);
                    xmlContent.AppendFormat("<RankItem name=\"装备数\" value=\"{0}\" /></RankChart>", ra.MemberEquipmentRank);
                    Response.Write(xmlContent.ToString());
                }
            }
            else if (UserID > 0)
            {
                //output player info and public video info

                Gamer player = PlayerStrip.GetPlayerInfo(UserID);

                if (player != null)
                {
                    StringBuilder xmlContent = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    int playerLv = player.Shirt;

                    if (player.Shorts < playerLv)
                        playerLv = player.Shorts;

                    if (player.Sock < playerLv)
                        playerLv = player.Sock;

                    xmlContent.AppendFormat("<UserItems username=\"{0}\" userid=\"{1}\" userlv=\"{2}\" ", player.UserName, player.UserID, ((playerLv > ConfigGlobal.PlayerMaxLv) ? ConfigGlobal.PlayerMaxLv.ToString() + "+" : playerLv.ToString()));
                    int CardCount = PlayerStrip.GetMyNumbers(UserID).Count;
                    int VideoCount = Entity.UserVideo.GetUserVideosByUserID(UserID).Count;
                    int InactiveCount = PlayerStrip.GetMyNumbers(UserID).FindAll(delegate (Card c) { return !c.ArsenalPlayerGuid.HasValue; }).Count;

                    xmlContent.AppendFormat("ShirtCount=\"{0}\" ShortsCount=\"{1}\" SockCount=\"{2}\" CardCount=\"{3}\" VideoCount=\"{4}\">", player.Shirt, player.Shorts, player.Sock, CardCount - InactiveCount, VideoCount + InactiveCount);

                    xmlContent.AppendFormat("<UserVideo>");

                    //DataView dv = Service.UserVideo.GetUserPublicVideo(UserID);
                    List<Entity.UserVideo> list = Entity.UserVideo.GetUserVideosByUserID(UserID)
                        .FindAll(delegate (Entity.UserVideo uv) { return uv.IsPublic; });

                    foreach (Entity.UserVideo uv in list)
                    {
                        object _value;

                        var v = Video.Cache.Load(uv.VideoGuid);

                        if (v != null)
                        {
                            var p = Player.Cache.Load(v.GoalPlayerGuid.Value);

                            if (p != null)
                            {
                                xmlContent.Append("<VideoItem ");

                                // build UserVideo xml info
                                foreach (var properInfo in uv.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                                {
                                    // remove the property VideoGuid of UserVideo
                                    if (!properInfo.Name.Equals("VideoGuid"))
                                    {
                                        _value = properInfo.GetValue(uv, null);
                                        if (_value == null) { _value = string.Empty; }

                                        xmlContent.AppendFormat("{0}=\"{1}\" ", properInfo.Name, HttpUtility.HtmlAttributeEncode(_value.ToString()));
                                    }
                                }

                                // build ArsenalVideo xml info
                                foreach (var properInfo in v.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                                {
                                    _value = properInfo.GetValue(v, null);
                                    if (_value == null) { _value = string.Empty; }

                                    xmlContent.AppendFormat("{0}=\"{1}\" ", properInfo.Name, HttpUtility.HtmlAttributeEncode(_value.ToString()));
                                }

                                // build ArsenalPlayer xml info
                                foreach (var properInfo in p.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                                {
                                    _value = properInfo.GetValue(p, null);
                                    if (_value == null) { _value = string.Empty; }

                                    xmlContent.AppendFormat("{0}=\"{1}\" ", properInfo.Name, HttpUtility.HtmlAttributeEncode(_value.ToString()));
                                }

                                xmlContent.Append("></VideoItem>");
                            }
                        }
                    }

                    xmlContent.Append("</UserVideo>");

                    xmlContent.Append("<UserCard>");
                    List<Card> cards = PlayerStrip.GetMyNumbers(UserID);
                    cards.RemoveAll(delegate (Card un) { return !un.ArsenalPlayerGuid.HasValue; });

                    foreach (Card c in cards)
                    {
                        xmlContent.Append("<CardItem ");
                        xmlContent.AppendFormat("UserNumberID=\"{0}\" IsActive=\"{1}\" ", c.ID, c.IsActive);

                        var p = Player.Cache.Load(c.ArsenalPlayerGuid.Value);
                        Object _value;

                        foreach (var properInfo in p.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                        {
                            _value = properInfo.GetValue(p, null);
                            if (_value == null) { _value = string.Empty; }

                            xmlContent.AppendFormat("{0}=\"{1}\" ", properInfo.Name, HttpUtility.HtmlAttributeEncode(_value.ToString()));
                        }

                        xmlContent.Append("></CardItem>");
                    }
                    xmlContent.Append("</UserCard>");

                    xmlContent.Append("</UserItems>");

                    Response.Write(xmlContent.ToString());
                }
            }
            else if (PlayerGuid != null)
            {
                //output arsenal player info

                //DataRow rowInfo = Arsenal_Player.Cache.GetInfo(new Guid(PlayerGuid));
                var p = Player.Cache.Load(new Guid(PlayerGuid));
                Object _value;

                if (p != null)
                {
                    StringBuilder xmlContent = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    xmlContent.Append("<PlayerInfo ");

                    foreach (var properInfo in p.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        _value = properInfo.GetValue(p, null);
                        if (_value == null) { _value = string.Empty; }

                        xmlContent.AppendFormat("{0}=\"{1}\" ", properInfo.Name, HttpUtility.HtmlAttributeEncode(_value.ToString()));
                    }

                    xmlContent.Append("></PlayerInfo>");

                    Response.Write(xmlContent.ToString());
                }
            }
            else if (CardID > 0)
            {
                Card c = PlayerStrip.GetUserNumber(CardID);

                if (c != null)
                {
                    //output arsenal player info
                    if (c.ArsenalPlayerGuid.HasValue)
                    {
                        //DataRow rowInfo = Arsenal_Player.Cache.GetInfo(c.ArsenalPlayerGuid.Value);
                        var p = Player.Cache.Load(c.ArsenalPlayerGuid.Value);
                        Object _value;

                        if (p != null)
                        {
                            StringBuilder xmlContent = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                            xmlContent.AppendFormat("<CardInfo CardID=\"{0}\" ", CardID);

                            foreach (var properInfo in p.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                            {
                                _value = properInfo.GetValue(p, null);
                                if (_value == null) { _value = string.Empty; }

                                xmlContent.AppendFormat("{0}=\"{1}\" ", properInfo.Name, HttpUtility.HtmlAttributeEncode(_value.ToString()));
                            }

                            xmlContent.Append("></CardInfo>");

                            Response.Write(xmlContent.ToString());
                        }
                    }
                    else
                    {
                        StringBuilder xmlContent = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                        xmlContent.AppendFormat("<CardInfo CardID=\"{0}\" Legend=\"True\" />", CardID);
                        Response.Write(xmlContent.ToString());
                    }
                }
            }
            else if (UserVideoID > 0)
            {
                //output video info
                //DataRow rowInfo = Service.UserVideo.GetVideoInfoByUserVideoID(UserVideoID);
                try
                {
                    Entity.UserVideo uv = new Entity.UserVideo();
                    uv.UserVideoID = UserVideoID;
                    uv.Select();

                    object _value;

                    var v = Video.Cache.Load(uv.VideoGuid);

                    if (v != null)
                    {
                        var p = Player.Cache.Load(v.GoalPlayerGuid.Value);

                        if (p != null)
                        {
                            StringBuilder xmlContent = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                            xmlContent.Append("<VideoInfo ");

                            // build UserVideo xml info
                            foreach (var properInfo in uv.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                            {
                                // remove the property VideoGuid of UserVideo
                                if (!properInfo.Name.Equals("VideoGuid"))
                                {
                                    _value = properInfo.GetValue(uv, null);
                                    if (_value == null) { _value = string.Empty; }

                                    xmlContent.AppendFormat("{0}=\"{1}\" ", properInfo.Name, HttpUtility.HtmlAttributeEncode(_value.ToString()));
                                }
                            }

                            // build ArsenalVideo xml info
                            foreach (var properInfo in v.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                            {
                                _value = properInfo.GetValue(v, null);
                                if (_value == null) { _value = string.Empty; }

                                xmlContent.AppendFormat("{0}=\"{1}\" ", properInfo.Name, HttpUtility.HtmlAttributeEncode(_value.ToString()));
                            }

                            // build ArsenalPlayer xml info
                            foreach (var properInfo in p.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                            {
                                _value = properInfo.GetValue(p, null);
                                if (_value == null) { _value = string.Empty; }

                                xmlContent.AppendFormat("{0}=\"{1}\" ", properInfo.Name, HttpUtility.HtmlAttributeEncode(_value.ToString()));
                            }

                            xmlContent.Append("></VideoInfo>");

                            Response.Write(xmlContent.ToString());
                        }
                    }
                }
                catch { }
            }
            else if (VideoGuid != null)
            {
                //DataRow rowInfo = Service.UserVideo.GetVideoInfoByVideoGuid(new Guid(VideoGuid));
                Object _value;

                var v = Video.Cache.Load(new Guid(VideoGuid));

                if (v != null)
                {
                    var p = Player.Cache.Load(v.GoalPlayerGuid.Value);

                    if (p != null)
                    {
                        StringBuilder xmlContent = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                        xmlContent.Append("<VideoInfo ");

                        // build ArsenalVideo xml info
                        foreach (var properInfo in v.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                        {
                            _value = properInfo.GetValue(v, null);
                            if (_value == null) { _value = string.Empty; }

                            xmlContent.AppendFormat("{0}=\"{1}\" ", properInfo.Name, HttpUtility.HtmlAttributeEncode(_value.ToString()));
                        }

                        // build ArsenalPlayer xml info
                        foreach (var properInfo in p.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                        {
                            _value = properInfo.GetValue(p, null);
                            if (_value == null) { _value = string.Empty; }

                            xmlContent.AppendFormat("{0}=\"{1}\" ", properInfo.Name, HttpUtility.HtmlAttributeEncode(_value.ToString()));
                        }

                        xmlContent.Append("></VideoInfo>");

                        Response.Write(xmlContent.ToString());
                    }
                }
            }
            else if (CurrArsenalPlayer == true)
            {
                var list = Player.Cache.PlayerList.FindAll(p => !p.IsLegend && !p.IsLoan && p.SquadNumber >= 0);

                list.Sort((p1, p2) => p1.SquadNumber - p2.SquadNumber);

                StringBuilder xmlContent = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                xmlContent.Append("<CurrArsenalPlayer>");

                foreach (var p in list)
                {
                    xmlContent.AppendFormat("<PlayerInfo Guid=\"{0}\" DisplayName=\"{1}\" SquadNumber=\"{2}\" FaceURL=\"{3}\" />", p.ID.ToString(), p.DisplayName, p.SquadNumber.ToString(), p.FaceURL);
                }
                xmlContent.Append("</CurrArsenalPlayer>");

                Response.Write(xmlContent.ToString());
            }
        }
    }
}
