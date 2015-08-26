using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Arsenalcn.ClubSys.Service;


namespace Arsenalcn.ClubSys.Web.Control
{
    public partial class CollectionVideo : Common.CollectionBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (GRank > 0)
                ddlGoalRank.SelectedValue = GRank.ToString();

            if (TRank > 0)
                ddlTeamRank.SelectedValue = TRank.ToString();

            if (ProfileUserID > 0)
            {
                var haveVideoCount = 0;
                var totalVideoCount = Video.Cache.GetAvailableVideosByRank(GRank, TRank).Count;

                //load data
                //DataTable dt = UserVideo.GetUserVideo(ProfileUserID);
                var list = Entity.UserVideo.GetUserVideosByUserID(ProfileUserID);

                if (list != null && list.Count > 0)
                {
                    //string expression = string.Empty;

                    if (GRank > 0 && TRank <= 0)
                    {
                        //expression = string.Format("GoalRank = {0}", GRank.ToString());
                        list = list.FindAll(delegate(Entity.UserVideo uv)
                        { return Video.Cache.Load(uv.VideoGuid).GoalRank.Equals(GRank.ToString()); });
                    }
                    else if (GRank > 0 && TRank > 0)
                    {
                        //expression = string.Format("GoalRank = {0} AND TeamworkRank = {1}", GRank.ToString(), TRank.ToString());
                        list = list.FindAll(delegate(Entity.UserVideo uv)
                        {
                            var v = Video.Cache.Load(uv.VideoGuid);
                            return v.GoalRank.Equals(GRank.ToString()) && v.TeamworkRank.Equals(TRank.ToString());
                        });
                    }
                    else if (GRank <= 0 && TRank > 0)
                    {
                        //expression = string.Format("TeamworkRank = {0}", TRank.ToString());
                        list = list.FindAll(delegate(Entity.UserVideo uv)
                        { return Video.Cache.Load(uv.VideoGuid).TeamworkRank.Equals(TRank.ToString()); });
                    }

                    //DataRow[] dr = dt.Select(expression, "ActiveDate DESC");
                    list.Sort(delegate(Entity.UserVideo uv1, Entity.UserVideo uv2)
                        { return uv2.ActiveDate.CompareTo(uv1.ActiveDate); });

                    haveVideoCount = list.Count;

                    rptVideo.DataSource = list;
                    rptVideo.DataBind();
                }

                ltlVideoCount.Text =
                    $"<span title=\"GRank:{GRank.ToString()} | TRank:{TRank.ToString()}\">已获得(总共)视频:<em>{haveVideoCount.ToString()}/{totalVideoCount.ToString()}</em></span>";
            }
        }

        public int GRank
        {
            get
            {
                int tmp;
                if (int.TryParse(ddlGoalRank.SelectedValue, out tmp))
                    return tmp;
                else if (int.TryParse(Request.QueryString["GRank"], out tmp))
                    return tmp;
                else
                    return 0;
            }
        }

        public int TRank
        {
            get
            {
                int tmp;
                if (int.TryParse(ddlTeamRank.SelectedValue, out tmp))
                    return tmp;
                else if (int.TryParse(Request.QueryString["TRank"], out tmp))
                    return tmp;
                else
                    return 0;
            }
        }

        protected void rptVideo_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                //DataRow dr = e.Item.DataItem as DataRow;
                var uv = e.Item.DataItem as Entity.UserVideo;

                var lblPlayerVideoID = e.Item.FindControl("lblPlayerVideoID") as Label;
                var lblPlayerVideoPath = e.Item.FindControl("lblPlayerVideoPath") as Label;

                lblPlayerVideoID.Text = uv.UserVideoID.ToString();
                lblPlayerVideoPath.Text =
                    $"swf/PlayerVideoActive.swf?XMLURL=ServerXml.aspx%3FUserVideoID={uv.UserVideoID.ToString()}";

                var btnSwfView = e.Item.FindControl("btnSwfView") as LinkButton;

                btnSwfView.OnClientClick = $"ShowVideoPreview('{uv.VideoGuid.ToString()}'); return false";

                var btnSetCurrent = e.Item.FindControl("btnSetCurrent") as LinkButton;
                var lblCurrent = e.Item.FindControl("lblSetCurrent") as Label;

                if (!this.ProfileUserID.Equals(this.CurrentUserID))
                {
                    btnSetCurrent.Visible = false;
                }

                if (btnSetCurrent != null)
                {
                    btnSetCurrent.CommandArgument = uv.UserVideoID.ToString();

                    if (uv.IsPublic)
                    {
                        //cancel button
                        lblCurrent.Visible = true;

                        btnSetCurrent.ToolTip = "取消使用";
                        btnSetCurrent.CssClass = "BtnCancelCurrent";
                        btnSetCurrent.CommandName = "CancelCurrent";
                    }
                    else
                    {
                        //set current button
                        lblCurrent.Visible = false;

                        btnSetCurrent.ToolTip = "点击使用";
                        btnSetCurrent.CssClass = "BtnSetCurrent";
                        btnSetCurrent.CommandName = "SetCurrent";
                    }
                }
            }
        }

        protected void rptVideo_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            var id = int.Parse(e.CommandArgument.ToString());

            if (e.CommandName == "SetCurrent")
            {
                if (Entity.UserVideo.GetUserVideosByUserID(this.ProfileUserID).FindAll(
                    delegate(Entity.UserVideo uv) { return uv.IsPublic; }).Count >= 3)
                {
                    this.Page.ClientScript.RegisterClientScriptBlock(typeof(string), "cannotsetcurrent", "alert('集锦使用数量上限为3个，请取消其他已使用的集锦。');", true);
                }
                else
                {
                    var uv = new Entity.UserVideo();
                    uv.UserVideoID = id;
                    uv.Select();

                    uv.IsPublic = true;
                    uv.Update();

                    this.Page.ClientScript.RegisterClientScriptBlock(typeof(string), "cannotsetcurrent", "alert('该集锦已使用。');window.location.href = window.location.href;", true);
                }
            }
            else if (e.CommandName == "CancelCurrent")
            {
                var uv = new Entity.UserVideo();
                uv.UserVideoID = id;
                uv.Select();

                uv.IsPublic = false;
                uv.Update();

                this.Page.ClientScript.RegisterClientScriptBlock(typeof(string), "cannotsetcurrent", "alert('该集锦取消使用。');window.location.href = window.location.href;", true);
            }
        }

    }
}