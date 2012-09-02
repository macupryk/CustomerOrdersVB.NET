@Section MainContent
    <form id="EditForm" action="edit" method="post" runat="server">
        <h1>Update customer information</h1>
        <div align="center">
            <div>
                <fieldset>
                    <legend>Customer details</legend>
                    <table>
                        <tr>
                            <td class="formLabelCell">ID:</td>
                            <td class="formValueCell">
                                <input type="text" id="CustomerId" name="CustomerId" 
                                    readonly value="@CTypeDynamic(Of String)(Model.CustomerId)" />
                            </td>
                        </tr>
                        <tr>
                            <td class="formLabelCell">Contact:</td>
                            <td class="formValueCell">
                                <input type="text" id="ContactName" name="ContactName" 
                                    value="@CTypeDynamic(Of String)(Model.ContactName)"
                                    class="{required:true, messages:{required:'Contact Name is required'}}" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: right; padding-top: 10px">
                                <input type="submit" value="Save" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>

            <input type="hidden" id="HID_OriginalName" value="@CTypeDynamic(Of String)(Model.ContactName)" />

            <div class="actionPanel">
                @Html.ActionLink(Server.HtmlDecode("&laquo; Back to customer list"), "List", "Customers")
                <a href="#" onclick="cancelEdit();">Reset Edit &raquo;</a>
            </div>
        </div>
    </form>
End section

@Section ScriptsContent
    <script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jquery.validate/1.9/jquery.validate.min.js"></script>
    <script type="text/javascript" src="@Url.Content("~/Content/Scripts/jquery.metadata.min.js")"></script>
    <script type="text/javascript" >
        function cancelEdit() {
            var origVal = $("#HID_OriginalName").val();
            $("#ContactName").val(origVal);
        }

        $(function () {
            $("#EditForm").validate();
        })
    </script>
End Section
