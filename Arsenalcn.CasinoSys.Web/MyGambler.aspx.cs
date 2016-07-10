using System;
using Arsenalcn.CasinoSys.Entity;
using Arsenalcn.CasinoSys.Web.Common;
using Arsenalcn.CasinoSys.Web.Control;
using Discuz.Forum;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class MyGambler : BasePage
    {
        protected override void OnInit(EventArgs e)
        {
            AnonymousRedirect = true;

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            #region Assign Control Property

            ctrlLeftPanel.UserId = userid;
            ctrlLeftPanel.UserName = username;

            ctrlFieldTooBar.UserId = userid;

            ctrlMenuTabBar.CurrentMenu = CasinoMenuType.CasinoPortal;

            ctrlGamblerHeader.UserId = userid;
            ctrlGamblerHeader.UserName = username;

            #endregion

            var qsb = Users.GetUserExtCredits(userid, 2);

            ltrlUserQSB.Text = qsb.ToString("N2");
            ltrlUserCash.Text = CurrentGambler.Cash.ToString("N2");

            // Remove ExchangeFee

            //int maxCash = (int)(qsb / (1 + Entity.ConfigGlobal.ExchangeFee) * Entity.ConfigGlobal.ExchangeRate);

            var maxCash = (int) (qsb*ConfigGlobal.ExchangeRate);
            ltrlMaxCash.Text = maxCash.ToString("N0");

            rvToCash.MaximumValue = maxCash >= 10 ? maxCash.ToString() : "10";

            var maxQsb = (int) (CurrentGambler.Cash*(1 - ConfigGlobal.ExchangeFee)/ConfigGlobal.ExchangeRate);
            ltrlMaxQSB.Text = maxQsb.ToString("N0");

            rvMaxQSB.MaximumValue = maxQsb >= 1 ? maxQsb.ToString() : "1";
        }

        protected void btnToCash_Click(object sender, EventArgs e)
        {
            try
            {
                // Remove ExchangeFee

                //int qsb = (int)(Convert.ToInt32(tbCash.Text) * (1 + Entity.ConfigGlobal.ExchangeFee) / Entity.ConfigGlobal.ExchangeRate);

                var qsb = Convert.ToInt32(tbCash.Text)/ConfigGlobal.ExchangeRate;

                if (qsb > Users.GetUserExtCredits(userid, 2) || qsb <= 0)
                    throw new Exception("Insufficient Founds");

                CurrentGambler.Cash += Convert.ToInt32(tbCash.Text.Trim());
                CurrentGambler.Update();

                var banker = new Banker(Banker.DefaultBankerID);
                banker.Cash += qsb*ConfigGlobal.ExchangeFee*ConfigGlobal.ExchangeRate;
                banker.Update(null);

                Users.UpdateUserExtCredits(userid, 2, -qsb);

                ClientScript.RegisterClientScriptBlock(typeof (string), "succeed",
                    "alert('充值博彩币成功');window.location.href = window.location.href;", true);
            }
            catch
            {
                ClientScript.RegisterClientScriptBlock(typeof (string), "failed", "alert('充值失败');", true);
            }
        }

        protected void btnToQSB_Click(object sender, EventArgs e)
        {
            try
            {
                var cash = (int) (Convert.ToInt32(tbQSB.Text)/(1 - ConfigGlobal.ExchangeFee)*ConfigGlobal.ExchangeRate);

                if (cash > CurrentGambler.Cash || cash <= 0)
                    throw new Exception("Insufficient Founds");

                CurrentGambler.Cash -= cash;
                CurrentGambler.Update();

                var banker = new Banker(Banker.DefaultBankerID);
                banker.Cash += cash*ConfigGlobal.ExchangeFee;
                banker.Update(null);

                Users.UpdateUserExtCredits(userid, 2, Convert.ToInt32(tbQSB.Text));
                ClientScript.RegisterClientScriptBlock(typeof (string), "succeed",
                    "alert('套现枪手币成功');window.location.href = window.location.href;", true);
            }
            catch
            {
                ClientScript.RegisterClientScriptBlock(typeof (string), "failed", "alert('套现失败');", true);
            }
        }
    }
}