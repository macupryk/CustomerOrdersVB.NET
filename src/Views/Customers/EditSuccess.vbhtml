@Section MainContent
    <h1>@ViewData("Message")</h1>
    <div align="center">
        <div class="actionPanel">
            @Html.ActionLink("Back to customer list", "List")|
        </div>
    </div>
End Section
