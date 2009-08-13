<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="WikiPlex.Web.Sample.WebForms.View" MasterPageFile="~\Views\Shared\Site.master" %>
<asp:Content ID="titleContent" ContentPlaceHolderID="head" runat="server">
    <title><asp:Literal ID="title" runat="server" /></title>
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="editWiki">
        <a id="editWikiContent" href="#">Edit Content</a>
    </div>
    
    <div id="wikiHistory">
        <h3>Page History</h3>
        <ul>
            <asp:Repeater ID="pageHistory" runat="server" OnItemDataBound="BindPageHistoryItem">
                <ItemTemplate>
                    <li>
                        <asp:Literal ID="date" runat="server" />
                        <asp:HyperLink ID="versionLink" runat="server" />
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
    </div>
    
    <div id="wikiContent">
        <div id="originalWikiContent"><asp:Literal ID="renderedSource" runat="server" /></div>
        <div id="previewWikiContent" style="display:none;"></div>
    </div>
    
    <div class="clear"></div>
</asp:Content>