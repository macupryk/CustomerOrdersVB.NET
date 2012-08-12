@Section MainContent
    <h1>@ViewData("Welcome")</h1>

    <p>@ViewData("Home")</p>

    @Html.ActionLink(Server.HtmlDecode("Proceed to customer listing &raquo;"), "List")
End Section