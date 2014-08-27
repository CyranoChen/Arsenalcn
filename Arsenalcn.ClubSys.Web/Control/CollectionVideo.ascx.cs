using System;
using System.Data;
using System.Web.UI.WebControls;

using Arsenalcn.ClubSys.Service;
using ArsenalVideo = Arsenalcn.ClubSys.Service.Arsenal.Video;

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
                int haveVideoCount = 0;
                int totalVideoCount = Arsenal_Video.Cache.GetAvailableVideosByRank(GRank, TRank).Count;

                //load data
                DataTable dt = UserVideo.GetUserVideo(ProfileUserID);

                if (dt != null)
                {
                    string expression = string.Empty;

                    if (GRank > 0 && TRank <= 0)
                        expression = string.Format("GoalRank = {0}", GRank.ToString());
                    else if (GRank > 0 && TRank > 0)
                        expression = string.Format("GoalRank = {0} AND TeamworkRank = {1}", GRank.ToString(), TRank.ToString());
                    else if (GRank <= 0 && TRank > 0)
                        expression = string.Format("TeamworkRank = {0}", TRank.ToString());

                    DataRow[] dr = dt.Select(expression, "ActiveDate DESC");

                    haveVideoCount = dr.Length;

                    rptVideo.DataSource = dr;
                    rptVideo.DataBind();
                }

                ltlVideoCount.Text = string.Format("<span title=\"GRank:{0} | TRank:{1}\">已获得(总共)视频:<em>{2}/{3}</em></span>", GRank.ToString(), TRank.ToString(), haveVideoCount.ToString(), totalVideoCount.ToString());
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
                DataRow dr = e.Item.DataItem as DataRow;

                Label lblPlayerVideoID = e.Item.FindControl("lblPlayerVideoID") as Label;
                Label lblPlayerVideoPath = e.Item.FindControl("lblPlayerVideoPath") as Label;

                lblPlayerVideoID.Text = dr["ID"].ToString();
                lblPlayerVideoPath.Text = string.Format("swf/PlayerVideoActive.swf?XMLURL=ServerXml.aspx%3FUserVideoID={0}", dr["ID"].ToString());

                LinkButton btnSwfView = e.Item.FindControl("btnSwfView") as LinkButton;

                btnSwfView.OnClientClick = string.Format("ShowVideoPreview('{0}'); return false", dr["VideoGuid"].ToString());

                LinkButton btnSetCurrent = e.Item.FindControl("btnSetCurrent") as LinkButton;
                Label lblCurrent = e.Item.FindControl("lblSetCurrent") as Label;

                if (!this.ProfileUserID.Equals(this.CurrentUserID))
                {
                    btnSetCurrent.Visible = false;
                }

                if (btnSetCurrent != null)
                {
                    btnSetCurrent.CommandArgument = dr["ID"].ToString();

                    if (dr["IsPublic"] != DBNull.Value && (bool)dr["IsPublic"] == true)
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
            int id = int.Parse(e.CommandArgument.ToString());

            if (e.CommandName == "SetCurrent")
            {
                if (UserVideo.GetUserPublicVideo(this.ProfileUserID).Count >= 3)
                {
                    this.Page.ClientScript.RegisterClientScriptBlock(typeof(string), "cannotsetcurrent", "alert('集锦使用数量上限为3个，请取消其他已使用的集锦。');", true);
                }
                else
                {
                    UserVideo.SetUserVideoPublic(id, true);
                    this.Page.ClientScript.RegisterClientScriptBlock(typeof(string), "cannotsetcurrent", "alert('该集锦已使用。');window.location.href = window.location.href;", true);
                }
            }
            else if (e.CommandName == "CancelCurrent")
            {
                UserVideo.SetUserVideoPublic(id, false);
                this.Page.ClientScript.RegisterClientScriptBlock(typeof(string), "cannotsetcurrent", "alert('该集锦取消使用。');window.location.href = window.location.href;", true);
            }
        }

    }
}