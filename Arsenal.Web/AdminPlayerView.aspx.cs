using System;
using Arsenal.Service;
using Arsenalcn.Core;
using Arsenalcn.Core.Dapper;

namespace Arsenal.Web
{
    public partial class AdminPlayerView : AdminPageBase
    {
        private readonly IRepository _repo = new Repository();

        private Guid PlayerGuid
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["PlayerGuid"]))
                {
                    try
                    {
                        return new Guid(Request.QueryString["PlayerGuid"]);
                    }
                    catch
                    {
                        return Guid.Empty;
                    }
                }
                return Guid.Empty;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlAdminFieldToolBar.AdminUserName = Username;

            if (!IsPostBack)
            {
                InitForm();
            }
        }

        private void InitForm()
        {
            if (PlayerGuid != Guid.Empty)
            {
                var p = _repo.Single<Player>(PlayerGuid);

                lblPlayerBasicInfo.InnerHtml = $"<em>{p.DisplayName}</em> 基本信息";
                lblPlayerDetailInfo.InnerHtml = $"<em>{p.DisplayName}</em> 详细信息";

                tbPlayerGuid.Text = PlayerGuid.ToString();
                tbFirstName.Text = !string.IsNullOrEmpty(p.FirstName) ? p.FirstName : string.Empty;
                tbLastName.Text = !string.IsNullOrEmpty(p.LastName) ? p.LastName : string.Empty;
                tbPrintingName.Text = !string.IsNullOrEmpty(p.PrintingName) ? p.PrintingName : string.Empty;
                ddlPosition.SelectedValue = !p.PlayerPosition.Equals(PlayerPositionType.None)
                    ? p.PlayerPosition.ToString()
                    : string.Empty;
                tbSquadNumber.Text = p.SquadNumber.ToString();
                tbFaceURL.Text = p.FaceURL;
                tbPhotoURL.Text = p.PhotoURL;
                tbOffset.Text = p.Offset.ToString();
                cbLegend.Checked = p.IsLegend;
                cbLoan.Checked = p.IsLoan;

                DateTime birthday;
                if (p.Birthday.HasValue && DateTime.TryParse(p.Birthday.ToString(), out birthday))
                    tbBirthday.Text = birthday.ToString("yyyy-MM-dd");
                else
                    tbBirthday.Text = string.Empty;

                tbBorn.Text = p.Born;
                tbStarts.Text = p.Starts.ToString();
                tbSubs.Text = p.Subs.ToString();
                tbGoals.Text = p.Goals.ToString();

                DateTime joinDate;
                if (p.JoinDate.HasValue && DateTime.TryParse(p.JoinDate.ToString(), out joinDate))
                    tbJoinDate.Text = joinDate.ToString("yyyy-MM-dd");
                else
                    tbJoinDate.Text = string.Empty;

                tbJoined.Text = p.Joined;
                tbLeft.Text = p.LeftYear;
                tbDebut.Text = p.Debut;
                tbFirstGoal.Text = p.FirstGoal;
                tbPreviousClubs.Text = p.PreviousClubs;
                tbProfile.Text = p.Profile;
            }
            else
            {
                lblPlayerBasicInfo.InnerHtml = "球员基本信息";
                lblPlayerDetailInfo.InnerHtml = "球员详细信息";

                tbPlayerGuid.Text = Guid.NewGuid().ToString();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                var p = new Player();

                if (!PlayerGuid.Equals(Guid.Empty))
                {
                    p = _repo.Single<Player>(PlayerGuid);
                }

                p.FirstName = !string.IsNullOrEmpty(tbFirstName.Text.Trim()) ? tbFirstName.Text.Trim() : null;
                p.LastName = !string.IsNullOrEmpty(tbLastName.Text.Trim()) ? tbLastName.Text.Trim() : null;

                var pName = $"{tbFirstName.Text.Trim()} {tbLastName.Text.Trim()}".Trim();
                if (!string.IsNullOrEmpty(pName))
                    p.DisplayName = pName.Trim();
                else
                    throw new Exception("请输入球员姓名");

                p.PrintingName = !string.IsNullOrEmpty(tbPrintingName.Text.Trim()) ? tbPrintingName.Text.Trim() : null;

                if (!string.IsNullOrEmpty(ddlPosition.SelectedValue))
                {
                    p.PlayerPosition = (PlayerPositionType)Enum.Parse(typeof(PlayerPositionType), ddlPosition.SelectedValue);
                }
                else
                {
                    p.PlayerPosition = PlayerPositionType.None;
                }

                p.SquadNumber = Convert.ToInt16(tbSquadNumber.Text.Trim());
                p.FaceURL = tbFaceURL.Text.Trim();
                p.PhotoURL = tbPhotoURL.Text.Trim();
                p.Offset = Convert.ToInt16(tbOffset.Text.Trim());
                p.IsLegend = cbLegend.Checked;
                p.IsLoan = cbLoan.Checked;

                DateTime birthday;
                if (!string.IsNullOrEmpty(tbBirthday.Text.Trim()) &&
                    DateTime.TryParse(tbBirthday.Text.Trim(), out birthday))
                    p.Birthday = birthday;
                else
                    p.Birthday = null;

                p.Born = tbBorn.Text.Trim();
                p.Starts = Convert.ToInt16(tbStarts.Text.Trim());
                p.Subs = Convert.ToInt16(tbSubs.Text.Trim());

                if (p.Starts >= 0 && p.Subs >= 0)
                    p.Apps = p.Starts + p.Subs;
                else
                    p.Apps = -1;

                p.Goals = Convert.ToInt16(tbGoals.Text.Trim());

                DateTime joinDate;
                if (!string.IsNullOrEmpty(tbJoinDate.Text.Trim()) &&
                    DateTime.TryParse(tbJoinDate.Text.Trim(), out joinDate))
                    p.JoinDate = joinDate;
                else
                    p.JoinDate = null;

                p.Joined = tbJoined.Text.Trim();
                p.LeftYear = tbLeft.Text.Trim();
                p.Debut = tbDebut.Text.Trim();
                p.FirstGoal = tbFirstGoal.Text.Trim();
                p.PreviousClubs = tbPreviousClubs.Text.Trim();
                p.Profile = tbProfile.Text.Trim();

                if (PlayerGuid != Guid.Empty)
                {
                    _repo.Update(p);
                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed",
                        "alert('更新成功');window.location.href = window.location.href", true);
                }
                else
                {
                    _repo.Insert(p);
                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed",
                        "alert('添加成功');window.location.href = 'AdminPlayer.aspx'", true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(typeof(string), "failed", $"alert('{ex.Message}')", true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (PlayerGuid != Guid.Empty)
            {
                Response.Redirect("AdminPlayer.aspx?PlayerGuid=" + PlayerGuid);
            }
            else
            {
                Response.Redirect("AdminPlayer.aspx");
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (PlayerGuid != Guid.Empty)
                {
                    _repo.Delete<Player>(PlayerGuid);

                    ClientScript.RegisterClientScriptBlock(typeof(string), "succeed",
                        "alert('删除成功');window.location.href='AdminPlayer.aspx'", true);
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
    }
}