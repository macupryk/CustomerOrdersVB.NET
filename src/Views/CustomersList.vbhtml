@Section StylesContent
    <link  rel="stylesheet" type="text/css" href="@Url.Content("~/Content/Styles/jqGrid/ui.jqgrid.css")" />
End Section

@Section MainContent
    <h1>Customers in database</h1>

    <p>
        Below you can see all customers in the database paged to grid.
        By clicking customer name you enter the details and edit information.
        By clicking orders you can see all orders that customer has. Orders can
        be in shipped state (they have shipped date) or they can be waiting for 
        shipment. You can ship orders that aren't shipped yet on the orders screen.
    </p>
    <p>
        The customer entity is serialized to a JSON object and encapsulated in a view model
        which includes basic calculation algorithm that we can use to calculate customer's value to us.
    </p>
    <br />
    <div align="center">
        <table id="customerList" class="datagrid" cellpadding="0" cellspacing="0"></table>
        <div id="pager" class="datagrid" style="text-align:center;"></div>
    </div>
End Section

@Section ScriptsContent
    @Code
        Dim jqGridLocale = "~/Content/Scripts/jqGrid/i18n/grid.locale-" + ViewData("Locale") + ".js"
    End Code

    <script type="text/javascript" src="@Url.Content(jqGridLocale)"></script>
    <script type="text/javascript" src="@Url.Content("~/Content/Scripts/jqGrid/jquery.jqGrid.min.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/Content/Scripts/customers_list.js")"></script>
  
    <script type="text/javascript">
        var _customersListJS = null;
        $(function () {
            _customersListJS = new CustomersListJS(_appCommon, '@Request.ApplicationPath');
            $("#customerList").jqGrid({
                width: "100%",
                url: '@Request.ApplicationPath/Customers/FillCustomersGrid',
                datatype: 'json',
                jsonReader: { repeatitems: false },
                mtype: 'GET',
                colNames: ['Id', 'Contact', 'Company', 'Class', ''],
                colModel: [
                  { name: 'CustomerId', index: 'CustomerId', width: 50, align: 'left' },
                  { name: 'ContactName', index: 'ContactName', width: 250, align: 'left', sortable: true, formatter: _customersListJS.customerLink },
                  { name: 'CompanyName', index: 'CompanyName', width: 250, align: 'left', sortable: true },
                  { name: 'Classification', index: 'Classification', width: 50, align: 'left', sortable: false, formatter: _customersListJS.getClassification },
                  { name: 'Orders', width: 50, align: 'center', sortable: false, formatter: _customersListJS.ordersLink }
                  ],
                pager: $('#pager'),
                rowNum: 10,
                rowList: [5, 10, 20, 50],
                sortname: 'CustomerId',
                sortorder: "asc",
                viewrecords: true,
                caption: 'Customers',
                loadError: function (jqXHR, textStatus, errorThrown) {
                    _appCommon.jqGridLoadError(this, jqXHR, textStatus, errorThrown);
                },
                loadComplete: function () {
                    // remove error div if exist
                    $('#' + this.id + '_err').remove();
                }
            });
        });
    </script>
end section
