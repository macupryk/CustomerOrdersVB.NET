@Section MainContent
    <h1>Order processing results</h1>
    
    <p>Below you can see the results from order fullfilment.</p>
    
    <div align="center">
        <div class="actionResultWindow">
            @Html.Raw(ViewData("ProcessMessages"))
        </div>
        <div class="actionPanel">
            @Html.ActionLink("Back to customer listing", "List", "Customers")
        </div>
    </div>
End Section  