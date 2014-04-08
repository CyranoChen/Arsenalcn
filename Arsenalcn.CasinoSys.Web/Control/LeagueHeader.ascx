<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeagueHeader.ascx.cs"
    Inherits="Arsenalcn.CasinoSys.Web.Control.LeagueHeader" %>
<div class="CategoryHeader">
    <ul>
        <asp:Repeater ID="rptLeague" runat="server" OnItemDataBound="rptLeague_ItemDataBound">
            <ItemTemplate>
                <asp:Literal ID="ltrlLeagueInfo" runat="server"></asp:Literal>
            </ItemTemplate>
        </asp:Repeater>
    </ul>

    <script type="text/javascript">
        $("li#<%=CurrLeagueGuid.ToString() %>").addClass("Current");
    </script>

    <div class="Clear">
    </div>
</div>
