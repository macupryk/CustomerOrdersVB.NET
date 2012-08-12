@Section MainContent
    <p>@ViewData("Home")</p>

    @Html.ActionLink(Server.HtmlDecode("Proceed to customer listing &raquo;"), "List")
End Section