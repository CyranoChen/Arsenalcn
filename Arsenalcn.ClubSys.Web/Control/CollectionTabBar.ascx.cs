using System;
using System.Web.UI;
using Arsenalcn.ClubSys.Entity;
using Arsenalcn.ClubSys.Service;
using UserVideo = Arsenalcn.ClubSys.Entity.UserVideo;

namespace Arsenalcn.ClubSys.Web.Control
{
    public partial class CollectionTabBar : UserControl
    {
        private CollectionTab _current = CollectionTab.Video;

        public CollectionTab Current
        {
            get
            {
                if (string.IsNullOrEmpty(Request.QueryString["type"]))
                {
                    switch (Request.QueryString["type"])
                    {
                        case "Video":
                            _current = CollectionTab.Video;
                            break;
                        case "Card":
                            _current = CollectionTab.Card;
                            break;
                        case "InactiveCard":
                            _current = CollectionTab.InactiveCard;
                            break;
                        case "InactiveVideo":
                            _current = CollectionTab.InactiveVideo;
                            break;
                    }
                }

                return _current;
            }
        }

        public int ProfileUserID { get; set; } = -1;

        public string QueryStringUserID
        {
            get
            {
                if (ProfileUserID > 0)
                    return $"&userid={ProfileUserID}";
                return string.Empty;
            }
        }

        public string ActiveCardCount
        {
            get
            {
                var items = PlayerStrip.GetMyNumbers(ProfileUserID);
                items.RemoveAll(delegate(Card un) { return !un.IsActive; });

                return items.Count.ToString();
            }
        }

        public string InactiveCardCount
        {
            get
            {
                var items = PlayerStrip.GetMyNumbers(ProfileUserID);
                items.RemoveAll(delegate(Card un) { return un.IsActive || !un.ArsenalPlayerGuid.HasValue; });

                return items.Count.ToString();
            }
        }

        public string ActiveVideoCount
        {
            get
            {
                //DataTable dtVideo = Service.UserVideo.GetUserVideo(ProfileUserID);

                return UserVideo.GetUserVideosByUserID(ProfileUserID).Count.ToString();
                ;
            }
        }

        public string InactiveVideoCount
        {
            get
            {
                var items = PlayerStrip.GetMyNumbers(ProfileUserID);
                items.RemoveAll(delegate(Card un) { return un.ArsenalPlayerGuid.HasValue; });

                return items.Count.ToString();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            try
            {
                _current = (CollectionTab) Enum.Parse(typeof (CollectionTab), Request.QueryString["type"], true);
            }
            catch
            {
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            switch (_current)
            {
                case CollectionTab.Card:
                    liCard.Attributes.Add("class", "Current");
                    break;
                case CollectionTab.Video:
                    liVideo.Attributes.Add("class", "Current");
                    break;
                case CollectionTab.InactiveCard:
                    liInactiveCard.Attributes.Add("class", "Current");
                    break;
                case CollectionTab.InactiveVideo:
                    liInactiveVideo.Attributes.Add("class", "Current");
                    break;
            }
        }
    }

    public enum CollectionTab
    {
        Card,
        Video,
        InactiveCard,
        InactiveVideo
    }
}