<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PortalPaymentInfo.ascx.cs" Inherits="iArsenal.Web.Control.PortalPaymentInfo" %>
<div class="InfoPanel">
    <h3 class="Col" onclick="$(this).toggleClass('Col'); $(this).toggleClass('Exp'); $(this).next('div').toggle('normal');">
        <a>付款方式信息</a></h3>
    <div class="Block" style="background: #fff48d">
        <p>
            (1). 支付宝转账账户：<br /><em>vicky@arsenalcn.com</em>，户名：凌薇。
        </p>
        <p>
            (2). 银行转账账户：<br />
            （招商银行上海分行闵行支行）<br />
            <em>6226 0902 1624 4489</em>，户名：凌薇。
        </p>
    </div>
</div>
