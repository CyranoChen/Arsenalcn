using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Arsenalcn.ClubSys.Service;
using Arsenalcn.ClubSys.Web.Common;
using Discuz.Entity;
using Discuz.Forum;
using UserVideo = Arsenalcn.ClubSys.Entity.UserVideo;

namespace Arsenalcn.ClubSys.Web
{
    public partial class AdminAward : AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = username;
            tbVideoGuid.ToolTip = Guid.Empty.ToString();

            if (!IsPostBack)
            {
                InitDropDownList();
            }
        }

        private void InitDropDownList()
        {
            var list = Player.Cache.PlayerList.FindAll(p => !string.IsNullOrEmpty(p.PhotoURL));
            list.Sort((p1, p2) =>
            {
                if (p1.SquadNumber == p2.SquadNumber)
                    return Comparer<string>.Default.Compare(p1.DisplayName, p2.DisplayName);
                return p1.SquadNumber - p2.SquadNumber;
            });

            lstPlayer.DataSource = list;
            lstPlayer.DataValueField = "ID";
            lstPlayer.DataBind();

            var li = new ListItem("不发放球星卡", Guid.Empty.ToString());
            lstPlayer.Items.Insert(0, li);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Url.PathAndQuery);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //validate user id and user name
            var awardUserID = int.Parse(tbUserID.Text);
            var sUser = Users.GetShortUserInfo(awardUserID);

            var awardUserName = tbUserName.Text;

            if (awardUserName == sUser.Username.Trim())
            {
                //process the award
                float cashIncrement = 0;
                float rp = 0;
                Guid? videoGuid = null;
                var AwardNotes = string.Empty;

                //precheck
                if (tbCash.Text.Trim() != string.Empty)
                {
                    if (!float.TryParse(tbCash.Text.Trim(), out cashIncrement))
                    {
                        ClientScript.RegisterClientScriptBlock(typeof (string), "invalidCash", "alert('枪手币格式无法转换！');",
                            true);
                        return;
                    }
                }

                if (tbRP.Text.Trim() != string.Empty)
                {
                    if (!float.TryParse(tbRP.Text.Trim(), out rp))
                    {
                        ClientScript.RegisterClientScriptBlock(typeof (string), "invalidRP", "alert('RP格式无法转换！');", true);
                        return;
                    }
                }

                if (tbVideoGuid.Text.Trim() != string.Empty)
                {
                    try
                    {
                        videoGuid = new Guid(tbVideoGuid.Text);
                    }
                    catch
                    {
                        ClientScript.RegisterClientScriptBlock(typeof (string), "invalidGuid", "alert('Guid格式无法转换！');",
                            true);
                        return;
                    }
                }
                if (tbNotes.Text.Trim() != string.Empty)
                {
                    AwardNotes = tbNotes.Text;
                }

                //is actually something awarded?
                var realAwarded = false;

                var awardMessageBody = "您获得奖励";

                //add cash
                if (cashIncrement != 0)
                {
                    Users.UpdateUserExtCredits(awardUserID, 2, cashIncrement);

                    awardMessageBody += $" 枪手币+{cashIncrement}";

                    realAwarded = true;
                }

                //add rp
                if (rp != 0)
                {
                    Users.UpdateUserExtCredits(awardUserID, 4, rp);

                    awardMessageBody += $" RP+{rp}, ";

                    realAwarded = true;
                }

                //add card
                if (!string.IsNullOrEmpty(lstPlayer.SelectedValue) && lstPlayer.SelectedValue != Guid.Empty.ToString())
                {
                    PlayerStrip.AddCard(awardUserID, awardUserName, new Guid(lstPlayer.SelectedValue),
                        cbCardActive.Checked);

                    awardMessageBody += $" 球星卡一张({(cbCardActive.Checked ? string.Empty : "未")}激活)";

                    realAwarded = true;
                }

                //add video
                if (videoGuid != null)
                {
                    if (cbVideoActive.Checked)
                    {
                        //active
                        //UserVideo.InsertActiveVideo(awardUserID, awardUserName, videoGuid.Value);

                        var uv = new UserVideo();
                        uv.UserID = awardUserID;
                        uv.UserName = awardUserName;
                        uv.VideoGuid = videoGuid.Value;
                        uv.ActiveDate = DateTime.Now;
                        uv.UserDesc = string.Empty;
                        uv.IsPublic = false;

                        uv.Insert();
                    }
                    else
                    {
                        //inactive
                        PlayerStrip.AddCard(awardUserID, awardUserName, null, false);
                    }

                    awardMessageBody += $" 视频卡一张({(cbVideoActive.Checked ? string.Empty : "未")}激活)";

                    realAwarded = true;
                }

                if (!string.IsNullOrEmpty(AwardNotes))
                {
                    awardMessageBody += $" 奖励原因：{AwardNotes}";
                }

                if (realAwarded)
                {
                    PlayerLog.LogHistory(awardUserID, awardUserName, PlayerHistoryType.Award,
                        new AwardDesc(cashIncrement, rp,
                            (!string.IsNullOrEmpty(lstPlayer.SelectedValue) &&
                             lstPlayer.SelectedValue != Guid.Empty.ToString()), videoGuid != null).Generate());

                    var pm = new PrivateMessageInfo();

                    pm.Msgfrom = ClubSysPrivateMessage.ClubSysAdminName;
                    pm.Msgfromid = 0;

                    pm.Folder = 0;
                    pm.Message = awardMessageBody;
                    pm.Msgto = awardUserName;
                    pm.Msgtoid = awardUserID;
                    pm.New = 1;
                    pm.Postdatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    pm.Subject = "ACN球会系统奖励（请勿回复此系统信息）";

                    PrivateMessages.CreatePrivateMessage(pm, 0);

                    ClientScript.RegisterClientScriptBlock(typeof (string), "succeed", "alert('成功颁奖！');", true);
                }
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(typeof (string), "alert", "alert('用户ID与用户名不匹配！');", true);
            }

            InitDropDownList();
        }

        protected void BtnCheckUserID_Click(object sender, EventArgs e)
        {
            var awardUserID = int.Parse(tbUserID.Text);

            var sUser = Users.GetShortUserInfo(awardUserID);
            tbUserName.Text = sUser.Username.Trim();

            InitDropDownList();
        }

        protected void lstPlayer_DataBound(object sender, EventArgs e)
        {
            foreach (ListItem li in (sender as DropDownList).Items)
            {
                var p = Player.Cache.Load(new Guid(li.Value));

                li.Text = $"(NO.{p.SquadNumber}) - {p.DisplayName} - {(!p.IsLegend ? "在队" : "离队")}";
            }
        }
    }
}