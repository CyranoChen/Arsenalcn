<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" CodeBehind="MyCreateClub.aspx.cs"
    Inherits="Arsenalcn.ClubSys.Web.MyCreateClub" Title="创建新球会" EnableViewState="false" %>

<%@ Register Src="Control/LeftPanel.ascx" TagName="LeftPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/FieldToolBar.ascx" TagName="FieldToolBar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:LeftPanel ID="ctrlLeftPanel" runat="server" />
    <div id="MainPanel">
        <uc2:FieldToolBar ID="ctrlFieldToolBar" runat="server" />
        <!-- 如果已经加入了球会，或者已拥有了球会，就不可申请新球会了。就显示下面那个Div即可 -->
        <asp:Panel ID="pnlInaccessible" runat="server" CssClass="ClubSys_Tip" Visible="false">
            <label>
                您已加入了球会，或者拥有了球会，或者发帖数没有达到<asp:Literal ID="ltrlMinPosts1" runat="server"></asp:Literal>，不可申请新球会。</label>
        </asp:Panel>
        <asp:PlaceHolder ID="phContent" runat="server">
            <table class="DataView" cellspacing="0" cellpadding="5">
                <thead>
                    <tr class="Header">
                        <th colspan="2">
                            只有未加入球会的会员，才可申请球会。快来申请属于自己的球会吧。
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <!-- 这个Row是TIPS，是自定义的样式，在DETAILVIEW里没有的 -->
                    <!-- 如果不能申请家族则显示，"您的发帖数<1000，不得申请新家族。"，1000应在全局设置中可配 -->
                    <tr class="ClubSys_Tip">
                        <td colspan="2">
                            <label>
                                您的发帖数&gt;<asp:Literal ID="ltrlMinPosts" runat="server"></asp:Literal>，可以申请新家族，申请提交后，请耐心等待管理员审核。</label>
                        </td>
                    </tr>
                    <!-- 页面里的text要保留，validation要有，如果必填的话，请显示"*必填" -->
                    <tr class="Row">
                        <td class="FieldHeader">
                            球会名称:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tbFullName" runat="server" CssClass="TextBoxRead" Text="球会的全称，比如:阿森纳中国球会"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">
                            球会简称:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tbShortName" runat="server" CssClass="TextBoxRead" Text="球会的简称，比如:Arsenalcn"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <!-- 默认为当前用户，即创建者，可以修改 -->
                        <td class="FieldHeader">
                            球会会长:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tbManagerName" runat="server" CssClass="TextBoxRead" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">
                            球会口号:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tbSlogan" runat="server" CssClass="TextBoxRead" Text="宗旨或口号，比如:One Life, One Team!"
                                Width="400px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="FieldHeader">
                            球会简介:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tbDesc" runat="server" CssClass="TextBox" Text="球会简介，也可用作公告" Width="400px"
                                TextMode="MultiLine" Rows="3"></asp:TextBox>
                        </td>
                    </tr>
                </tbody>
            </table>
            <!-- 是否可以考虑已经申请的会员，打开这个页面，就是当初他申请的信息，他可以在没审核通过前编辑重新提交或者取消申请 -->
            <div class="FooterBtnBar">
                <asp:LinkButton ID="btnSave" runat="server" CssClass="LinkBtn SubmitBtn" OnClick="btnSave_Click">提交申请</asp:LinkButton>
                <asp:LinkButton ID="btnCancel" runat="server" CssClass="LinkBtn CancelBtn" OnClick="btnCancel_Click">取消申请</asp:LinkButton>
            </div>
        </asp:PlaceHolder>
    </div>
</asp:Content>
