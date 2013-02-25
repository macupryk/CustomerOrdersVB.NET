var CustomersListJS = function (commonJS, appPath) {
    var me = this;

    this.AppPath = appPath;
    this.CommonJS = commonJS;

    this.getClassification = function (cellValue, options, rowdata, action) {
        //just a simple classification for the example calculate based on customer's A alphabet count
        var aCount = me.CommonJS.getCharCount(rowdata.ContactName, 'a');
        if (aCount > 4) { return 'A'; }   //great customer!
        if (aCount > 1) { return 'B'; }   //ok...

        return 'C';   //a really lousy customer
    };

    this.customerLink = function (cellValue, options, rowdata, action) {
        return "<a href='" + me.AppPath + "/Customers/Browse/" + rowdata.CustomerId +
           "' >" + rowdata.ContactName + "</a>";
    };

    this.ordersLink = function (cellValue, options, rowdata, action) {
        return "<a href='" + me.AppPath + "/Orders/Process/" + rowdata.CustomerId +
                "/" + rowdata.ContactName + "'>Orders</a>";
    }
};