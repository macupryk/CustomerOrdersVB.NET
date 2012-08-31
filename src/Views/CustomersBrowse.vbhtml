@Section MainContent
    <div align="center">
        <div runat="server" ID="panel">
            <fieldset>
                <legend>Customer details</legend>
                <table>
                    <tr>
                        <td class="formLabelCell">ID:</td>
                        <td class="formValueCell">
                            @CTypeDynamic(Of String)(Model.CustomerId)
                        </td>
                    </tr>
                    <tr>
                        <td class="formLabelCell">Contact:</td>
                        <td class="formValueCell">
                            @CTypeDynamic(Of String)(Model.ContactName)
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
        <div class="actionPanel">
            @Html.ActionLink(Server.HtmlDecode("&laquo; Back to customer list"), "List", "Customers")
            @Html.ActionLink(Server.HtmlDecode("Edit Customer &raquo;"), "Edit", New With {.customerId = CTypeDynamic(Of String)(Model.CustomerId)})
        </div>
    </div>
End Section
