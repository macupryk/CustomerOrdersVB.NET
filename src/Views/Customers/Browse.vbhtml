@Section MainContent
    <div align="center">
        <div runat="server" ID="panel">
            <fieldset>
                <legend>Customer details</legend>
                <table>
                    <tr>
                        <td class="formLabelCell">ID:</td>
                        <td class="formValueCell">
                            @Model.CustomerId
                        </td>
                    </tr>
                    <tr>
                        <td class="formLabelCell">Contact:</td>
                        <td class="formValueCell">
                            @Model.ContactName
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
        <div class="actionPanel">
            @Html.ActionLink(Server.HtmlDecode("&laquo; Back to customer list"), "List")
            @Html.ActionLink(Server.HtmlDecode("Edit Customer &raquo;"), "Edit", New With {.customerId = Model.CustomerId})
        </div>
    </div>
End Section
