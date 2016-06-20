<%@ Page Language="C#" MasterPageFile="DefaultMaster.master" AutoEventWireup="true"
CodeBehind="AdminLog.aspx.cs" Inherits="iArsenal.Web.AdminLog" Title="后台管理 日志查询" Theme="Arsenalcn" %>

<%@ Register Src="Control/AdminPanel.ascx" TagName="AdminPanel" TagPrefix="uc1" %>
<%@ Register Src="Control/AdminFieldToolBar.ascx" TagName="AdminFieldToolBar" TagPrefix="uc2" %>
<%@ Register Src="Control/CustomPagerInfo.ascx" TagName="CustomPagerInfo" TagPrefix="uc3" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="cphHead" runat="server">
    <script type="text/javascript">
        $(function () {
            var $tbInfo = $(".DivFloatLeft > .TextBox");
            $tbInfo.each(function() {
                $(this).focus(function() {
                    $(this).val("");
                });
            });
        });
    </script>
</asp:Content>
<asp:Content ID="cphMain" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:AdminPanel ID="pnlAdmin" runat="server"/>
    <div id="MainPanel">
        <uc2:AdminFieldToolBar ID="ctrlAdminFieldToolBar" runat="server"/>
        <div class="FunctionBar">
            <div class="DivFloatLeft">
                <asp:DropDownList ID="ddlLogger" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLogger_SelectedIndexChanged">
                    <asp:ListItem Value="UserLog" Text="用户"></asp:ListItem>
                    <asp:ListItem Value="AppLog" Text="应用"></asp:ListItem>
                    <asp:ListItem Value="DaoLog" Text="数据"></asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="ddlLevel" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLevel_SelectedIndexChanged">
                    <asp:ListItem Value="" Text="--级别--"></asp:ListItem>
                    <asp:ListItem Value="Debug" Text="Debug"></asp:ListItem>
                    <asp:ListItem Value="Info" Text="Info"></asp:ListItem>
                    <asp:ListItem Value="Warn" Text="Warn"></asp:ListItem>
                    <asp:ListItem Value="Error" Text="Error"></asp:ListItem>
                    <asp:ListItem Value="Fatal" Text="Fatal"></asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="ddlException" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlException_SelectedIndexChanged">
                    <asp:ListItem Value="true" Text="异常" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="false" Text="正常"></asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="tbMethod" runat="server" Text="--方法名称--" CssClass="TextBox"
                             Width="100px">
                </asp:TextBox>
                <asp:TextBox ID="tbUserID" runat="server" Text="--ID--" CssClass="TextBox"
                             Width="50px">
                </asp:TextBox>
                <asp:TextBox ID="tbUserIP" runat="server" Text="--IP--" CssClass="TextBox"
                             Width="100px">
                </asp:TextBox>
                <asp:TextBox ID="tbUserBrowser" runat="server" Text="--浏览器--" CssClass="TextBox"
                             Width="100px">
                </asp:TextBox>
                <asp:TextBox ID="tbUserOS" runat="server" Text="--操作系统--" CssClass="TextBox"
                             Width="100px">
                </asp:TextBox>
                <asp:LinkButton ID="btnFilter" runat="server" Text="搜索日志" CssClass="LinkBtn" OnClick="btnFilter_Click"></asp:LinkButton>
            </div>
            <div class="Clear">
                <uc3:CustomPagerInfo ID="ctrlCustomPagerInfo" runat="server"/>
            </div>
        </div>
        <asp:GridView ID="gvLog" runat="server" DataKeyNames="ID" OnRowDataBound="gvLog_RowDataBound"
                      OnPageIndexChanging="gvLog_PageIndexChanging" PageSize="20" OnSelectedIndexChanged="gvLog_SelectedIndexChanged">
            <Columns>
                <asp:HyperLinkField HeaderText="标识" DataTextField="ID" DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="AdminLogView.aspx?LogID={0}"/>
                <asp:BoundField HeaderText="类型" DataField="Logger"/>
                <asp:BoundField HeaderText="创建时间" DataField="CreateTime" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}"/>
                <asp:BoundField HeaderText="级别" DataField="Level"/>
                <asp:BoundField HeaderText="消息" DataField="Message" DataFormatString="<em>{0}</em>"
                                HtmlEncode="false" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="Wrap"/>
                <asp:BoundField HeaderText="方法信息" DataField="Method" ItemStyle-HorizontalAlign="Left"/>
                <asp:BoundField HeaderText="用户ID" DataField="UserID"/>
                <asp:BoundField HeaderText="用户IP" DataField="UserIP"/>
                <asp:BoundField HeaderText="浏览器" DataField="UserBrowser"/>
                <asp:BoundField HeaderText="操作系统" DataField="UserOS"/>
                <asp:TemplateField HeaderText="异常出错">
                    <ItemTemplate>
                        <asp:Literal ID="ltrlException" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowEditButton="false" ShowSelectButton="true" ShowDeleteButton="false"
                                  HeaderText="操作" EditText="修改" UpdateText="保存" CancelText="取消" SelectText="详细"
                                  DeleteText="删除" ControlStyle-CssClass="LinkBtn" ItemStyle-CssClass="BtnColumn"/>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>