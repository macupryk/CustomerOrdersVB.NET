var OrdersProcessJS = function (commonJS, appPath) {
    var me = this;

    this.AppPath = appPath;
    this.CommonJS = commonJS

    this.FormatDate = function(cellValue, options, rowdata, action) {
        return me.CommonJS.formatJSONDate(rowdata.OrderDate);
    }
};