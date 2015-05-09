using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Arsenal.Service;
using Arsenalcn.Core;

namespace Arsenal.Web
{
    public partial class AdminVideoView : AdminPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = this.Username;

            if (!IsPostBack)
            {
                #region Bind ddlLeague, ddlMatch
                List<League> leagueList = League.Cache.LeagueList.FindAll(delegate(League l)
                {
                    return Match.Cache.MatchList.FindAll(delegate(Match m) { return m.LeagueGuid.Equals(l.LeagueGuid); }).Count > 0;
                });

                ddlLeague.DataSource = leagueList;
                ddlLeague.DataTextField = "LeagueNameInfo";
                ddlLeague.DataValueField = "LeagueGuid";
                ddlLeague.DataBind();

                ddlLeague.Items.Insert(0, new ListItem("--请选择比赛分类--", string.Empty));

                #endregion

                #region Bind ddlGoalPlayer, ddlAssistPlayer
                List<Player> list = Player.Cache.PlayerList_HasSquadNumber;

                if (list != null && list.Count > 0)
                {
                    foreach (Player p in list)
                    {
                        ddlGoalPlayer.Items.Add(new ListItem(string.Format("NO.{0} - {1}", p.SquadNumber.ToString(), p.DisplayName), p.PlayerGuid.ToString()));
                        ddlAssistPlayer.Items.Add(new ListItem(string.Format("NO.{0} - {1}", p.SquadNumber.ToString(), p.DisplayName), p.PlayerGuid.ToString()));
                    }
                }

                ddlGoalPlayer.Items.Insert(0, new ListItem("--请选择进球队员--", string.Empty));
                ddlAssistPlayer.Items.Insert(0, new ListItem("--请选择助攻队员--", string.Empty));

                #endregion

                InitForm();
            }
        }

        private Guid VideoGuid
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["VideoGuid"]))
                {
                    try { return new Guid(Request.QueryString["VideoGuid"]); }
                    catch { return Guid.Empty; }
                }
                else
                    return Guid.Empty;
            }
        }

        private void InitForm()
        {
            if (VideoGuid != Guid.Empty)
            {
                Video v = new Video().Single<Video>(VideoGuid);

                tbVideoGuid.Text = VideoGuid.ToString();
                tbFileName.Text = v.FileName.ToString();

                #region Set Video ArsenalMatchGuid
                if (v.ArsenalMatchGuid.HasValue)
                {
                    Match m = Match.Cache.Load(v.ArsenalMatchGuid.Value);

                    if (m != null)
                    {
                        if (m.LeagueGuid.HasValue)
                        {
                            ddlLeague.SelectedValue = m.LeagueGuid.Value.ToString();
                            BindMatchData(m.LeagueGuid.Value);
                            ddlMatch.SelectedValue = v.ArsenalMatchGuid.Value.ToString();
                        }
                    }
                }
                else
                {
                    ddlLeague.SelectedValue = string.Empty;
                    ddlMatch.Items.Clear();
                }
                #endregion

                if (v.GoalPlayerGuid.HasValue)
                    ddlGoalPlayer.SelectedValue = v.GoalPlayerGuid.Value.ToString();
                else
                    ddlGoalPlayer.SelectedValue = string.Empty;

                if (v.AssistPlayerGuid.HasValue)
                    ddlAssistPlayer.SelectedValue = v.AssistPlayerGuid.Value.ToString();
                else
                    ddlAssistPlayer.SelectedValue = string.Empty;

                tbGoalRank.Text = v.GoalRank;
                tbTeamworkRank.Text = v.TeamworkRank;
                tbGoalYear.Text = v.GoalYear;
                tbOpponent.Text = v.Opponent;
                ddlVideoType.SelectedValue = v.VideoType.ToString();
                tbVideoLength.Text = v.VideoLength.ToString();
                tbVideoWidth.Text = v.VideoWidth.ToString();
                tbVideoHeight.Text = v.VideoHeight.ToString();
            }
            else
            {
                tbVideoGuid.Text = Guid.NewGuid().ToString();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Video v = new Video();

                v.FileName = tbFileName.Text.Trim();

                if (!string.IsNullOrEmpty(ddlLeague.SelectedValue) && !string.IsNullOrEmpty(ddlMatch.SelectedValue))
                {
                    v.ArsenalMatchGuid = new Guid(ddlMatch.SelectedValue.ToString());
                }
                else
                {
                    v.ArsenalMatchGuid = null;
                }

                if (!string.IsNullOrEmpty(ddlGoalPlayer.SelectedValue))
                {
                    v.GoalPlayerGuid = new Guid(ddlGoalPlayer.SelectedValue);
                    v.GoalPlayerName = Player.Cache.Load(v.GoalPlayerGuid.Value).DisplayName;
                }
                else
                {
                    throw new Exception("请选择进球队员");
                }

                if (!string.IsNullOrEmpty(ddlAssistPlayer.SelectedValue))
                {
                    v.AssistPlayerGuid = new Guid(ddlAssistPlayer.SelectedValue);
                    v.AssistPlayerName = Player.Cache.Load(v.AssistPlayerGuid.Value).DisplayName;
                }
                else
                {
                    v.AssistPlayerGuid = null;
                    v.AssistPlayerName = null;
                }

                v.GoalRank = tbGoalRank.Text.Trim();
                v.TeamworkRank = tbTeamworkRank.Text.Trim();
                v.GoalYear = tbGoalYear.Text.Trim();
                v.Opponent = tbOpponent.Text.Trim();
                //v.VideoType = ddlVideoType.SelectedValue;
                v.VideoType = (VideoFileType)Enum.Parse(typeof(VideoFileType), ddlVideoType.SelectedValue);
                v.VideoLength = Convert.ToInt16(tbVideoLength.Text.Trim());
                v.VideoWidth = Convert.ToInt16(tbVideoWidth.Text.Trim());
                v.VideoHeight = Convert.ToInt16(tbVideoHeight.Text.Trim());

                if (VideoGuid != Guid.Empty)
                {
                    v.VideoGuid = VideoGuid;
                    //v.FileName = string.Format("{0}/{1}{2}", v.GoalPlayerName.Replace(" ", "_"), v.VideoGuid.ToString().ToUpper(), ddlVideoType.SelectedValue.ToUpper());
                    v.Update<Video>(v);

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('更新成功');window.location.href = window.location.href", true);
                }
                else
                {
                    v.VideoGuid = new Guid(tbVideoGuid.Text.Trim());
                    //v.FileName = string.Format("{0}/{1}{2}", v.GoalPlayerName.Replace(" ", "_"), v.VideoGuid.ToString().ToUpper(), ddlVideoType.SelectedValue.ToUpper());
                    v.Create<Video>(v);

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('添加成功');window.location.href = 'AdminVideo.aspx'", true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", string.Format("alert('{0}')", ex.Message.ToString()), true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (VideoGuid != Guid.Empty)
            {
                Response.Redirect("AdminVideo.aspx?VideoGuid=" + VideoGuid.ToString());
            }
            else
            {
                Response.Redirect("AdminVideo.aspx");
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (VideoGuid != Guid.Empty)
                {
                    Video v = new Video();
                    v.VideoGuid = VideoGuid;
                    v.Delete<Video>(v);

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('删除成功');window.location.href='AdminVideo.aspx'", true);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", "alert('删除失败')", true);
            }
        }

        private void BindMatchData(Guid LeagueGuid)
        {
            string _strRound = string.Empty;

            ddlMatch.Items.Clear();

            List<Match> list = Match.Cache.MatchList.FindAll(delegate(Match m) { return m.IsActive && m.LeagueGuid.Equals(LeagueGuid); });

            if (list != null && list.Count > 0)
            {
                foreach (Match m in list)
                {
                    if (m.Round.HasValue)
                        _strRound = string.Format("【{0}】", m.Round.Value.ToString());
                    else
                        _strRound = string.Empty;

                    ddlMatch.Items.Add(new ListItem(string.Format("【{0}】-{1}- {2}", m.IsHome ? "主" : "客", _strRound, m.TeamName), m.MatchGuid.ToString()));
                }
            }
            else
            {
                ddlMatch.Items.Clear();
            }

            ddlMatch.Items.Insert(0, new ListItem("--请选择比赛对阵--", string.Empty));
        }

        private void BindOponnentData(Guid MatchGuid)
        {
            Match m = Match.Cache.Load(MatchGuid);

            if (m != null)
            {
                tbOpponent.Text = m.TeamName;
                tbGoalYear.Text = m.PlayTime.Year.ToString();
            }
        }

        protected void ddlLeague_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlLeague.SelectedValue))
                BindMatchData(new Guid(ddlLeague.SelectedValue));
        }

        protected void ddlMatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlMatch.SelectedValue))
                BindOponnentData(new Guid(ddlMatch.SelectedValue));
        }
    }
}