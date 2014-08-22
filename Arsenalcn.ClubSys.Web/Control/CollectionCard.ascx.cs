using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Entity;

using ArsenalPlayer = Arsenal.Entity.Player;

namespace Arsenalcn.ClubSys.Web.Control
{
    public partial class CollectionCard : Common.CollectionBase
    {
        private Player _playerInfo = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ProfileUserID > 0)
            {
                _playerInfo = PlayerStrip.GetPlayerInfo(ProfileUserID);

                List<UserNumber> list = PlayerStrip.GetMyCards(ProfileUserID).FindAll(delegate(UserNumber un)
                { return un.IsActive && un.ArsenalPlayerGuid.HasValue && un.ActiveDate.HasValue; });

                list = SortUserNumberListByOrderClause(list, OrderClause);

                rptCard.DataSource = list;
                rptCard.DataBind();
            }
        }

        private string OrderClause
        {
            get
            {
                if (!string.IsNullOrEmpty(ddlCardOrder.SelectedValue))
                    return ddlCardOrder.SelectedValue;
                else
                    return string.Empty;
            }
        }

        private List<UserNumber> SortUserNumberListByOrderClause(List<UserNumber> list, string orderClause)
        {
            if (list.Count > 0 && !string.IsNullOrEmpty(orderClause))
            {
                if (orderClause.Equals("SquadNumber"))
                {
                    list.Sort(delegate(UserNumber un1, UserNumber un2)
                    {
                        ArsenalPlayer p1 = ArsenalPlayer.Cache.Load(un1.ArsenalPlayerGuid.Value);
                        ArsenalPlayer p2 = ArsenalPlayer.Cache.Load(un2.ArsenalPlayerGuid.Value);

                        if (p1.SquadNumber.Equals(p2.SquadNumber))
                            return Comparer<string>.Default.Compare(p1.DisplayName, p2.DisplayName);
                        else
                            return p1.SquadNumber - p2.SquadNumber;
                    });
                }
                else if (orderClause.Equals("ActiveDate DESC"))
                {
                    list.Sort(delegate(UserNumber un1, UserNumber un2) { return Comparer<DateTime>.Default.Compare(un2.ActiveDate.Value, un1.ActiveDate.Value); });
                }
                else if (orderClause.Equals("Legend, SquadNumber"))
                {
                    list.Sort(delegate(UserNumber un1, UserNumber un2)
                    {
                        ArsenalPlayer p1 = ArsenalPlayer.Cache.Load(un1.ArsenalPlayerGuid.Value);
                        ArsenalPlayer p2 = ArsenalPlayer.Cache.Load(un2.ArsenalPlayerGuid.Value);

                        if (p1.IsLegend.Equals(p2.IsLegend))
                        {
                            if (p1.SquadNumber.Equals(p2.SquadNumber))
                                return Comparer<string>.Default.Compare(p1.DisplayName, p2.DisplayName);
                            else
                                return p1.SquadNumber - p2.SquadNumber;
                        }
                        else
                        {
                            return Comparer<Boolean>.Default.Compare(p1.IsLegend, p2.IsLegend);
                        }
                    });
                }
                else
                {
                    list.Sort(delegate(UserNumber un1, UserNumber un2) { return Comparer<DateTime>.Default.Compare(un2.GainDate, un1.GainDate); });
                }
            }

            return list;
        }

        protected void rptCard_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                UserNumber un = e.Item.DataItem as UserNumber;

                Label lblPlayerCardID = e.Item.FindControl("lblPlayerCardID") as Label;
                Label lblPlayerCardPath = e.Item.FindControl("lblPlayerCardPath") as Label;
                LinkButton btnSetCurrent = e.Item.FindControl("btnSetCurrent") as LinkButton;
                Label lblSetCurrent = e.Item.FindControl("lblSetCurrent") as Label;

                lblPlayerCardID.Text = un.ID.ToString();
                lblPlayerCardPath.Text = string.Format("swf/PlayerCardActive.swf?XMLURL=ServerXml.aspx%3FPlayerGuid={0}", un.ArsenalPlayerGuid.ToString());

                if (ProfileUserID == this.CurrentUserID)
                {
                    //setCurrent button
                    if (_playerInfo == null)
                    {
                        btnSetCurrent.Visible = false;
                        lblSetCurrent.Visible = false;
                    }
                    else
                    {
                        if (un.IsInUse)
                        {
                            btnSetCurrent.Visible = true;
                            btnSetCurrent.ToolTip = "取消使用";
                            btnSetCurrent.CssClass = "BtnCancelCurrent";
                            btnSetCurrent.CommandName = "CancelCurrent";
                            btnSetCurrent.CommandArgument = un.ID.ToString();

                            lblSetCurrent.Visible = true;
                        }
                        else if (!un.IsActive)
                        {
                            btnSetCurrent.Visible = false;
                            lblSetCurrent.Visible = false;
                        }
                        else
                        {
                            btnSetCurrent.Visible = true;
                            btnSetCurrent.ToolTip = "点击使用";
                            btnSetCurrent.CssClass = "BtnSetCurrent";
                            btnSetCurrent.CommandName = "SetCurrent";
                            btnSetCurrent.CommandArgument = un.ID.ToString();

                            lblSetCurrent.Visible = false;
                        }
                    }
                }
                else
                {
                    btnSetCurrent.Visible = false;

                    if (un.IsInUse)
                    {
                        lblSetCurrent.Visible = true;
                    }
                    else
                    {
                        lblSetCurrent.Visible = false;
                    }
                }
            }
        }

        protected void rptCard_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "SetCurrent")
            {
                int userNumID = int.Parse(e.CommandArgument.ToString());

                UserNumber un = PlayerStrip.GetUserNumber(userNumID);

                if (un.UserID == CurrentUserID && un.IsActive && !un.IsInUse)
                {
                    PlayerStrip.UpdatePlayerCurrentNum(CurrentUserID, userNumID);

                    string script = "alert('球衣已被换上'); window.location.href = window.location.href;";
                    this.Page.ClientScript.RegisterClientScriptBlock(typeof(string), "SetCurrentSucceed", script, true);
                }
                else
                {
                    string script = "alert('您不能换上该球衣');";
                    this.Page.ClientScript.RegisterClientScriptBlock(typeof(string), "SetCurrentFailed", script, true);
                }
            }
            else if (e.CommandName == "CancelCurrent")
            {
                int userNumID = int.Parse(e.CommandArgument.ToString());

                UserNumber un = PlayerStrip.GetUserNumber(userNumID);

                if (un.UserID == CurrentUserID && un.IsActive && un.IsInUse)
                {
                    PlayerStrip.RemovePlayerCurrentNum(CurrentUserID, userNumID);

                    string script = "alert('球衣已被换下'); window.location.href = window.location.href;";
                    this.Page.ClientScript.RegisterClientScriptBlock(typeof(string), "SetCurrentSucceed", script, true);
                }
                else
                {
                    string script = "alert('您不能换下该球衣');";
                    this.Page.ClientScript.RegisterClientScriptBlock(typeof(string), "SetCurrentFailed", script, true);
                }
            }
        }
    }
}