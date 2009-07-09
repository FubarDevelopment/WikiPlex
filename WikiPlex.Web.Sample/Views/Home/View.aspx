<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WikiPlex.Web.Sample.Models.Content>" %>
<asp:Content ID="titleContent" ContentPlaceHolderID="head" runat="server">
    <title>WikiPlex Sample - <%= Html.Encode(Model.Title.Name) %></title>
    <script type="text/javascript">
        var timeout = null;
        $(function() {
            var dlg = $('#editWikiForm');
            var cnt = $('#editWikiContent');
            var cntPos = cnt.position();
            dlg.dialog({ autoOpen: false, 
                         width: 450,
                         position: [cntPos.left - 450 + cnt.outerWidth(), cntPos.top + cnt.outerHeight()],
                         show: 'blind', 
                         hide: 'blind',
                         beforeclose: function() { $('#originalWikiContent').show(); $('#previewWikiContent').hide(); }
                      });
            cnt.click(function() {
                if (!dlg.dialog('isOpen')) {
                    $.post('<%= Url.RouteUrl("Source", new { Model.Title.Slug, Model.Version }) %>', function(data) {
                        $('#Source').val(data);
                        var original = $('#originalWikiContent');
                        original.hide();
                        $('#previewWikiContent').html(original.html()).show();
                        dlg.dialog('open');
                    });
                } else {
                    dlg.dialog('close');
                }
            });
            
            $('#Source').keyup(function(e) {
                if (timeout != null) {
                    clearTimeout(timeout);
                    timeout = null;
                }
                
                var self = $(this);
                timeout = setTimeout(function() {
                    $.post('<%= Url.RouteUrl("Act", new { action = "preview", Model.Title.Slug }) %>',
                           { source: self.val() },
                           function(data) { $('#previewWikiContent').html(data); });
                }, 250);
            });
            
            $('#cancelEdit').click(function() {
                $('#editWikiForm').dialog('close');
            });
        });
    </script>
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="editWiki">
        <a id="editWikiContent" href="#">Edit Content</a>
    </div>
    
    <div id="wikiHistory">
        <h3>Page History</h3>
        <ul>
            <% foreach (var content in Model.Title.Contents.OrderByDescending(x => x.Version)) {
                   if (Model.Version == content.Version) { %>
            <li><%= content.VersionDate.ToString() %></li>
                <% } else { %>
            <li><a href="<%= Url.RouteUrl("History", new {Model.Title.Slug, content.Version}) %>"><%= content.VersionDate.ToString() %></a></li>
            <% } } %>
        </ul>
    </div>
    
    <div id="wikiContent">
        <div id="originalWikiContent"><%= Model.RenderedSource %></div>
        <div id="previewWikiContent" style="display:none;"></div>
    </div>
    
    <div class="clear"></div>
    
    <div id="editWikiForm">
        <% using (Html.BeginRouteForm("Act", new { action = "edit", Model.Title.Slug }, FormMethod.Post)) { %>
            <%= Html.Hidden("Name", Model.Title.Name) %>
            <fieldset>
                <label for="Source">Source:</label>
                <% if (Model.Version != Model.Title.Contents.Max(x => x.Version)) { %>
                <span id="editWikiNotLatest">
                    Note: Not editing latest source
                </span>
                <% } %>
                <%= Html.TextArea("Source", string.Empty) %>
                <input type="submit" value="Save" />
                <input id="cancelEdit" type="button" value="Cancel" />
            </fieldset>
        <% } %>
    </div>
</asp:Content>
