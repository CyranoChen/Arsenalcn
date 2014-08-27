using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Arsenalcn.ClubSys.Service;
using ArsenalPlayer = Arsenalcn.ClubSys.Service.Arsenal.Player;

using Discuz.Forum;
using Discuz.Entity;

namespace Arsenalcn.ClubSys.Web
{
    public partial class AdminAward : Common.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = this.username;
            tbVideoGuid.ToolTip = Guid.Empty.ToString();

            if (!IsPostBack)
            {
                InitDropDownList();
            }
        }

        private void InitDropDownList()
        {
            List<ArsenalPlayer> list = Arsenal_Player.Cache.PlayerList.FindAll(delegate(ArsenalPlayer p) { return !string.IsNullOrEmpty(p.PhotoURL); });
            list.Sort(delegate(ArsenalPlayer p1, ArsenalPlayer p2)
            {
                if (p1.SquadNumber == p2.SquadNumber)
                    return Comparer<string>.Default.Compare(p1.DisplayName, p2.DisplayName);
                else
                    return p1.SquadNumber - p2.SquadNumber;
            });

            lstPlayer.DataSource = list;
            lstPlayer.DataValueField = "PlayerGuid";
            lstPlayer.DataBind();

            ListItem li = new ListItem("不发放球星卡", Guid.Empty.ToString());
            lstPlayer.Items.Insert(0, li);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Url.PathAndQuery);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //validate user id and user name
            int awardUserID = int.Parse(tbUserID.Text);
            ShortUserInfo sUser = AdminUsers.GetShortUserInfo(awardUserID);

            string awardUserName = tbUserName.Text;

            if (awardUserName == sUser.Username.Trim())
            {
                //process the award
                float cashIncrement = 0;
                float rp = 0;
                Guid? videoGuid = null;
                string AwardNotes = string.Empty;

                //precheck
                if (tbCash.Text.Trim() != string.Empty)
                {
                    if (!float.TryParse(tbCash.Text.Trim(), out cashIncrement))
                    {
                        ClientScript.RegisterClientScriptBlock(typeof(string), "invalidCash", "alert('枪手币格式无法转换！');", true);
                        return;
                    }
                }

                if (tbRP.Text.Trim() != string.Empty)
                {
                    if (!float.TryParse(tbRP.Text.Trim(), out rp))
                    {
                        ClientScript.RegisterClientScriptBlock(typeof(string), "invalidRP", "alert('RP格式无法转换！');", true);
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
                        ClientScript.RegisterClientScriptBlock(typeof(string), "invalidGuid", "alert('Guid格式无法转换！');", true);
                        return;
                    }
                }
                if (tbNotes.Text.Trim() != string.Empty)
                {
                    AwardNotes = tbNotes.Text.ToString();
                }

                //is actually something awarded?
                bool realAwarded = false;

                string awardMessageBody = "您获得奖励";

                //add cash
                if (cashIncrement != 0)
                {
                    AdminUsers.UpdateUserExtCredits(awardUserID, 2, cashIncrement);

                    awardMessageBody += string.Format(" 枪手币+{0}", cashIncrement);

                    realAwarded = true;
                }

                //add rp
                if (rp != 0)
                {
                    AdminUsers.UpdateUserExtCredits(awardUserID, 4, rp);

                    awardMessageBody += string.Format(" RP+{0}, ", rp);

                    realAwarded = true;
                }

                //add card
                if (!string.IsNullOrEmpty(lstPlayer.SelectedValue) && lstPlayer.SelectedValue != Guid.Empty.ToString())
                {
                    PlayerStrip.AddCard(awardUserID, awardUserName, new Guid(lstPlayer.SelectedValue), cbCardActive.Checked);

                    awardMessageBody += string.Format(" 球星卡一张({0}激活)", cbCardActive.Checked ? string.Empty : "未");

                    realAwarded = true;
                }

                //add video
                if (videoGuid != null)
                {
                    if (cbVideoActive.Checked)
                    {
                        //active
                        UserVideo.InsertActiveVideo(awardUserID, awardUserName, videoGuid.Value);
                    }
                    else
                    {
                        //inactive
                        PlayerStrip.AddCard(awardUserID, awardUserName, null, false);
                    }

                    awardMessageBody += string.Format(" 视频卡一张({0}激活)", cbVideoActive.Checked ? string.Empty : "未");

                    realAwarded = true;
                }

                if (!string.IsNullOrEmpty(AwardNotes))
                {
                    awardMessageBody += string.Format(" 奖励原因：{0}", AwardNotes.ToString());
                }

                if (realAwarded)
                {
                    PlayerLog.LogHistory(awardUserID, awardUserName, PlayerHistoryType.Award, new AwardDesc(cashIncrement, rp, (!string.IsNullOrEmpty(lstPlayer.SelectedValue) && lstPlayer.SelectedValue != Guid.Empty.ToString()), videoGuid != null).Generate());

                    PrivateMessageInfo pm = new PrivateMessageInfo();

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

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('成功颁奖！');", true);
                }
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "alert", "alert('用户ID与用户名不匹配！');", true);
            }

            InitDropDownList();
        }

        protected void BtnCheckUserID_Click(object sender, EventArgs e)
        {
            int awardUserID = int.Parse(tbUserID.Text);

            ShortUserInfo sUser = AdminUsers.GetShortUserInfo(awardUserID);
            tbUserName.Text = sUser.Username.Trim();

            InitDropDownList();
        }

        protected void lstPlayer_DataBound(object sender, EventArgs e)
        {
            foreach (ListItem li in (sender as DropDownList).Items)
            {
                ArsenalPlayer p = Arsenal_Player.Cache.Load(new Guid(li.Value));

                li.Text = string.Format("(NO.{0}) - {1} - {2}", p.SquadNumber.ToString(), p.DisplayName, !p.IsLegend ? "在队" : "离队");
            }
        }
    }
}
