using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Arsenalcn.ClubSys.DataAccess;
using Arsenalcn.ClubSys.Entity;

namespace Arsenalcn.ClubSys.Web.Control
{
    public partial class CollectionInactiveVideo : Common.CollectionBase
    {
        private Player _playerInfo = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ProfileUserID > 0)
            {
                //load data
                _playerInfo = PlayerStrip.GetPlayerInfo(ProfileUserID);
                List<UserNumber> items = PlayerStrip.GetMyNumbers(ProfileUserID);

                items.RemoveAll(delegate(UserNumber un) { return un.ArsenalPlayerGuid.HasValue; });
                
                rptVideo.DataSource = items;
                rptVideo.DataBind();
            }
        }

        protected void rptVideo_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                UserNumber un = e.Item.DataItem as UserNumber;

                Label lblPlayerInactiveVideoID = e.Item.FindControl("lblPlayerInactiveVideoID") as Label;
                LinkButton btnActive = e.Item.FindControl("btnActive") as LinkButton;

                lblPlayerInactiveVideoID.Text = un.ID.ToString();

                if (ProfileUserID == CurrentUserID)
                {
                    //active button
                    if (_playerInfo == null)
                    {
                        btnActive.Visible = false;
                    }
                    else
                    {
                        if (_playerInfo.Shirt >= 5 && _playerInfo.Shorts >= 5 && _playerInfo.Sock >= 5)
                        {
                            btnActive.Visible = true;

                            //btnActive.CommandArgument = un.ID.ToString();

                            //postback to another url
                            //btnActive.PostBackUrl = "MyPlayerCardActive.aspx?unID=" + un.ID.ToString();
                            btnActive.OnClientClick = "GenFlashFrame('swf/ShowCardActive.swf?XMLURL=ServerXml.aspx%3FCardID=" + un.ID.ToString() + "', '160', '200'); return false";
                        }
                        else
                        {
                            btnActive.Visible = false;
                        }
                    }

                }
                else
                {
                    btnActive.Visible = false;
                }
            }
        }
    }
}