@Section MainContent
    <h1>Update customer information</h1>
    <div align="center">
        <div>
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
                            @Html.TextBoxFor(Function(m) m.ContactName, New With {.class = "{required:true, messages:{required:'User name is required'}}"})
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: right; padding-top: 10px">
                            <button id="btnSave">Save</button>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>

        <input type="hidden" id="HID_OriginalName" value="@Model.ContactName" />

        <div class="actionPanel">
            @Html.ActionLink(Server.HtmlDecode("&laquo; Back to customer list"), "List", "Customers")
            <a href="#" onclick="cancelEdit();">Reset Edit &raquo;</a>
        </div>
    </div>
End section

@Section ScriptsContent
    <script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jquery.validate/1.9/jquery.validate.min.js"></script>
    <script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jquery.validate/1.9/additional-methods.min.js"></script>
    <script type="text/javascript" src="@Url.Content("~/Content/Scripts/jquery.metadata.min.js")"></script>
    <script type="text/javascript" >
        function cancelEdit() {
            var origVal = $("#HID_OriginalName").val();
            $("#ContactName").val(origVal);
        }

        $("#btnSave").click(function () {
            var contactName = $("#ContactName").val();
            var url = '@Url.Action("Edit")?CustomerId=@Model.CustomerId&contactName=' + contactName;
            $.post(url, null,
                    function (result) {
                        window.location = '@Url.Action("EditSuccess")?customerName=' + contactName;
                    });

            return false;
        });

        $(function () {
            $("#form1").validate();
        })
    </script>
End Section
