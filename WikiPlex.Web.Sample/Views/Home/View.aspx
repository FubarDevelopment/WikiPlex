﻿<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WikiPlex.Web.Sample.Views.Home.ViewContent>" %>
<asp:Content ID="titleContent" ContentPlaceHolderID="head" runat="server">
    <title>WikiPlex Sample - <%= Html.Encode(Model.Content.Title.Name) %></title>
    <% if (Model.Editable)
       { %>
    <script type="text/javascript">
        var timeout = null;
        $(function () {
            var dlg = $('#editWikiForm');
            var cnt = $('#editWikiContent');
            var cntPos = cnt.position();
            dlg.dialog({ autoOpen: false,
                width: 450,
                position: [cntPos.left - 450 + cnt.outerWidth(), cntPos.top + cnt.outerHeight()],
                show: 'blind',
                hide: 'blind',
                beforeclose: function () { $('#originalWikiContent').show(); $('#previewWikiContent').hide(); }
            });
            cnt.click(function () {
                if (!dlg.dialog('isOpen')) {
                    $.post('<%= Url.RouteUrl("Source", new { Model.Content.Title.Id, Model.Content.Title.Slug, Model.Content.Version }) %>', function (data) {
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

            $('#Source').keyup(function (e) {
                if (timeout != null) {
                    clearTimeout(timeout);
                    timeout = null;
                }

                var self = $(this);
                timeout = setTimeout(function () {
                    $.post('<%= Url.RouteUrl("Act", new { action = "GetWikiPreview", Model.Content.Title.Id, Model.Content.Title.Slug }) %>',
                           { source: self.val() },
                           function (data) { $('#previewWikiContent').html(data); });
                }, 250);
            });

            $('#cancelEdit').click(function () {
                $('#editWikiForm').dialog('close');
            });
        });
    </script>
    <% } %>
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <% if (Model.Editable) { %>
    <div id="editWiki" class="editWiki">
        <a id="editWikiContent" href="#">Edit Content</a>
    </div>
    <% } %>
    
    <div id="wikiHistory">
        <h3>Page History</h3>
        <ul>
            <% foreach (var content in Model.History) {
                   if (Model.Content.Version == content.Version) { %>
            <li><%= content.VersionDate.ToString() %></li>
                <% } else { %>
            <li><a href="<%= Url.RouteUrl("History", new {Model.Content.Title.Id, Model.Content.Title.Slug, content.Version}) %>"><%= content.VersionDate.ToString() %></a></li>
            <% } } %>
        </ul>
    </div>
    
    <div id="wikiContent">
        <div id="originalWikiContent"><%= Model.Content.RenderedSource%></div>
        <div id="previewWikiContent" style="display:none;"></div>
    </div>
    
    <div class="clear"></div>
    
    <% if (Model.Editable) { %>
    <div id="editWikiForm" class="editWikiForm">
        <% using (Html.BeginRouteForm("Act", new { action = "EditWiki", Model.Content.Title.Slug }, FormMethod.Post)) { %>
            <%= Html.Hidden("Name", Model.Content.Title.Name)%>
            <fieldset>
                <label for="Source">Source:</label>
                <% if (Model.Content.Version != Model.Content.Title.MaxVersion) { %>
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
    <% } %>
</asp:Content>
