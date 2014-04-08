using System;

using Discuz.Forum;

namespace Arsenalcn.CasinoSys.Web
{
    public partial class MyGambler : Common.BasePage
    {
        protected override void OnInit(EventArgs e)
        {
            AnonymousRedirect = true;

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            #region Assign Control Property

            ctrlLeftPanel.UserID = userid;
            ctrlLeftPanel.UserName = username;

            ctrlFieldTooBar.UserID = userid;

            ctrlMenuTabBar.CurrentMenu = Arsenalcn.CasinoSys.Web.Control.CasinoMenuType.CasinoPortal;

            ctrlGamblerHeader.UserID = userid;
            ctrlGamblerHeader.UserName = username;

            #endregion

            float qsb = AdminUsers.GetUserExtCredits(userid, 2);

            ltrlUserQSB.Text = qsb.ToString("N2");
            ltrlUserCash.Text = CurrentGambler.Cash.ToString("N2");

            // Remove ExchangeFee

            //int maxCash = (int)(qsb / (1 + Entity.ConfigGlobal.ExchangeFee) * Entity.ConfigGlobal.ExchangeRate);

            int maxCash = (int)(qsb * Entity.ConfigGlobal.ExchangeRate);
            ltrlMaxCash.Text = maxCash.ToString("N0");

            if (maxCash >= 10)
                rvToCash.MaximumValue = maxCash.ToString();
            else
                rvToCash.MaximumValue = "10";

            int maxQSB = (int)(CurrentGambler.Cash * (1 - Entity.ConfigGlobal.ExchangeFee) / Entity.ConfigGlobal.ExchangeRate);
            ltrlMaxQSB.Text = maxQSB.ToString("N0");

            if (maxQSB >= 1)
                rvMaxQSB.MaximumValue = maxQSB.ToString();
            else
                rvMaxQSB.MaximumValue = "1";
        }

        protected void btnToCash_Click(object sender, EventArgs e)
        {
            try
            {
                // Remove ExchangeFee
                
                //int qsb = (int)(Convert.ToInt32(tbCash.Text) * (1 + Entity.ConfigGlobal.ExchangeFee) / Entity.ConfigGlobal.ExchangeRate);

                int qsb = (int)(Convert.ToInt32(tbCash.Text) / Entity.ConfigGlobal.ExchangeRate);

                if (qsb > AdminUsers.GetUserExtCredits(userid, 2) || qsb <= 0)
                    throw new Exception("Insufficient Founds");

                CurrentGambler.Cash += Convert.ToInt32(tbCash.Text.Trim());
                CurrentGambler.Update(null);

                Entity.Banker banker = new Entity.Banker(Entity.Banker.DefaultBankerID);
                banker.Cash += qsb * Entity.ConfigGlobal.ExchangeFee * Entity.ConfigGlobal.ExchangeRate;
                banker.Update(null);

                AdminUsers.UpdateUserExtCredits(userid, 2, -qsb);

                this.ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('充值博彩币成功');window.location.href = window.location.href;", true);
            }
            catch
            {
                this.ClientScript.RegisterClientScriptBlock(typeof(string), "failed", "alert('充值失败');", true);
            }
        }

        protected void btnToQSB_Click(object sender, EventArgs e)
        {
            try
            {
                int cash = (int)(Convert.ToInt32(tbQSB.Text) / (1 - Entity.ConfigGlobal.ExchangeFee) * Entity.ConfigGlobal.ExchangeRate);

                if (cash > CurrentGambler.Cash || cash <= 0)
                    throw new Exception("Insufficient Founds");

                CurrentGambler.Cash -= cash;
                CurrentGambler.Update(null);

                Entity.Banker banker = new Entity.Banker(Entity.Banker.DefaultBankerID);
                banker.Cash += cash * Entity.ConfigGlobal.ExchangeFee;
                banker.Update(null);

                AdminUsers.UpdateUserExtCredits(userid, 2, Convert.ToInt32(tbQSB.Text));
                this.ClientScript.RegisterClientScriptBlock(typeof(string), "succeed", "alert('套现枪手币成功');window.location.href = window.location.href;", true);
            }
            catch
            {
                this.ClientScript.RegisterClientScriptBlock(typeof(string), "failed", "alert('套现失败');", true);
            }
        }
    }
}
