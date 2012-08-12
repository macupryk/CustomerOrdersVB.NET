﻿@Section StylesContent
    <link  rel="stylesheet" type="text/css" href="@Url.Content("~/Content/Styles/jqGrid/ui.jqgrid.css")" />
End Section

@Section MainContent
    <h1>@ViewData("ProcessOrdersFor") <strong>@ViewData("CustomerName")</strong></h1>

    <p>@ViewData("Process")</p>

    <div align="center">
        <table id="ordersList" class="scroll" cellpadding="0" cellspacing="0"></table>
        <div id="pager" class="scroll" style="text-align:center;"></div>    
        
        <button id="btnProcess">Process Orders</button>

        <div class="actionPanel">
            @Html.ActionLink("Back to customer listing", "List", "Customers")
        </div>
    </div>
End Section

@Section ScriptsContent
    @Code
        Dim jqGridLocale = "~/Content/Scripts/jqGrid/i18n/grid.locale-" + ViewData("Locale") + ".js"
    End Code

    <script type="text/javascript" src="@Url.Content(jqGridLocale)"></script>
    <script type="text/javascript" src="@Url.Content("~/Content/Scripts/jqGrid/jquery.jqGrid.min.js")"></script>
  
    <script type="text/javascript">
        $('#btnProcess').click(function () {
            var url = '@Url.Action("Process")'
            $.post(url, null,
                    function (result) {
                        window.location = '@Url.Action("ProcessResult")';
                    });
            return false;
        });


        $(function () {
            $('#ordersList').jqGrid({
                width: '100%',
                url: '/Orders/FillOrdersGrid/@ViewData("CustomerId")',
                datatype: 'json',
                jsonReader: { repeatitems: false },
                mtype: 'GET',
                colNames: ['Id', 'Ordered', 'Shipped'],
                colModel: [
                  { name: 'OrderId', index: 'OrderId', width: 50, align: 'left' },
                  { name: 'OrderDate', index: 'OrderDate', width: 250, align: 'left', sortable: true, formatter: FormatDate },
                  { name: 'ShippedDate', index: 'ShippedDate', width: 250, align: 'left', sortable: true, formatter: FormatDate}],
                pager: $('#pager'),
                rowNum: 10,
                rowList: [5, 10, 20, 50],
                sortname: 'OrderId',
                sortorder: "asc",
                viewrecords: true,
                caption: 'Orders',
                height: 'auto',
                loadError: function (jqXHR, textStatus, errorThrown) {
                    _appCommon.JqGridLoadError(this, jqXHR, textStatus, errorThrown);
                },
                loadComplete: function () {
                    _appCommon.JqGridLoadComplete(this);
                }

            });
        });

        function FormatDate(cellValue, options, rowdata, action) {
            return _appCommon.FormatJSONDate(rowdata.OrderDate);
        }
    </script>  
End Section

