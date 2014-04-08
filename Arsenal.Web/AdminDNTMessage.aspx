<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
    CodeBehind="AdminDNTMessage.aspx.cs" Inherits="Arsenal.Web.AdminDNTMessage"
    Title="后台管理 短信管理" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
<%@ Register Src="Control/CustomPagerInfo.ascx" TagName="CustomPagerInfo" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var $tbInfo = $(".DivFloatLeft > .TextBox");
            $tbInfo.each(function () {
                $(this).focus(function () {
                    $(this).val("");
                });
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:AdminPanel ID="pnlAdmin" runat="server" />
    <div id="MainPanel">
        <uc2:AdminFieldToolBar ID="ctrlAdminFieldToolBar" runat="server" />
        <div class="FunctionBar">
            <div class="DivFloatLeft">
                <asp:DropDownList ID="ddlFolder" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFolder_SelectedIndexChanged">
                    <asp:ListItem Value="0" Text="收件箱" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="1" Text="发件箱"></asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="ddlNew" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlNew_SelectedIndexChanged">
                    <asp:ListItem Value="" Text="全部" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="0" Text="已读"></asp:ListItem>
                    <asp:ListItem Value="1" Text="未读"></asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="tbMsgfrom" runat="server" Text="--发送人--" CssClass="TextBox" Width="100px"></asp:TextBox>
                <asp:TextBox ID="tbMsgto" runat="server" Text="--收件人--" CssClass="TextBox" Width="100px"></asp:TextBox>
                <asp:TextBox ID="tbSubject" runat="server" Text="--标题--" CssClass="TextBox" Width="100px"></asp:TextBox>
                <asp:LinkButton ID="btnFilter" runat="server" Text="搜索短信" CssClass="LinkBtn" OnClick="btnFilter_Click"></asp:LinkButton>
            </div>
            <div class="Clear">
                <uc3:CustomPagerInfo ID="ctrlCustomPagerInfo" runat="server" />
            </div>
        </div>
        <asp:GridView ID="gvDNTMessage" runat="server" OnPageIndexChanging="gvDNTMessage_PageIndexChanging"
            PageSize="10" OnRowDataBound="gvDNTMessage_RowDataBound">
            <Columns>
                <asp:BoundField HeaderText="标识" DataField="Pmid" />
                <asp:TemplateField HeaderText="状态">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlNew" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="发送人" DataField="Msgfrom" />
                <asp:BoundField HeaderText="收件人" DataField="Msgto" />
                <asp:BoundField HeaderText="标题" DataField="Subject" ItemStyle-HorizontalAlign="Left" />
                <asp:TemplateField HeaderText="发送时间">
                    <ItemTemplate>
                        <asp:Label ID="lblPostdatetime" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlShowPM" runat="server" Text="详细" Target="_blank" CssClass="LinkBtn"></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
