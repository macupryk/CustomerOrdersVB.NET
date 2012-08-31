/// <reference path="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.2-vsdoc.js" />
/// <reference path="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.19/jquery-ui.js" />

//*************************************************************************************************************
// AppCommon.js
// Description - Code for common object used on this site.
//                 contains useful and re-usable behaviors.
//                  TODO:  Document this....
//*************************************************************************************************************

var AppCommon = function () {          //constructor for Common library
    this.BaseUrl = "~/";

    //Properties in support of ForcePageRefresh() //////////////
    this.ReloadTimer = null;
    this.URL = unescape(window.location.pathname);
    ///////////////////////////////////////////////////////////
}

AppCommon.prototype = {
    CloseFormOK: function (strReturn) {
        if (!strReturn) strReturn = true;

        window.returnValue = strReturn;
        self.close();
    },

    CloseFormCancel: function () {
        window.returnValue = "0";
        self.close();
    },

    FormatJSONDateTime: function (jsonDate) {
        var date = eval(jsonDate.replace(/\/Date\((.*?)\)\//gi, "new Date($1)"));
        var month = "Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec".split(",");

        return date.getDate() + " " +
                month[date.getMonth()] + " " +
                date.getFullYear() + " " +
                date.getHours() + ":" +
                this.FormatMinutes(date.getMinutes());
    },

    FormatJSONDate: function (jsonDate) {
        var date = null;

        try {
            date = eval(jsonDate.replace(/\/Date\((.*?)\)\//gi, "new Date($1)"));
        } catch (err) {  //try using sugar.js here (note: make sure it's referenced!
            date = Date.create(jsonDate);
        }

        var month = "Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec".split(",");

        return date.getDate() + " " +
                month[date.getMonth()] + " " +
                date.getFullYear();
    },

    TickerSlider: function (container, delay) {
        if ($(container).length) {
            container = $(container);

            var slides = container.length, slide = 0;
            setInterval(function () {
                if (slide == slides) {
                    container.slideDown();
                    slide = 0;
                } else {
                    container.eq(slide).slideUp();
                    slide++;
                }
            }, delay)
        } else return false;
    },

    ForcePageRefresh: function (seconds) {
        if (arguments.length == 1) {
            if (this.ReloadTimer) clearTimeout(this.ReloadTimer);

            var self = this;
            this.ReloadTimer = setTimeout(function () { self.ForcePageRefresh(); }, Math.ceil(parseFloat(seconds) * 1000));
        } else {
            this.ReloadTimer = null;
            location.reload(true);
            window.location.replace(this.URL);
        }
    },

    ParseUrl: function (url) {
        var expr = /((http|ftp):\/)?\/?([^:\/\s]+)((\/\w+)*\/)([\w\-\.]+\.[^#?\s]+)(#[\w\-]+)?/;

        if (url.match(expr))
            return { url: RegExp["$&"], protocol: RegExp.$2, host: RegExp.$3, path: RegExp.$4, file: RegExp.$6, hash: RegExp.$7 };
        else
            return { url: "No match", protocol: "", host: "", path: "", file: "", hash: "" };
    },

    StringEscape: function (str) {
        var specials = ['#', '&', '~', '=', '>', "'", ':', '"', '!', ';', ','];
        var regexSpecials = ['.', '*', '+', '|', '[', ']', '(', ')', '/', '^', '$'];
        var regex = new RegExp('(' + specials.join('|') + '|\\' + regexSpecials.join('|\\') + ')', 'g');

        return str.replace(regex, '\\$1');
    },

    GetCharCount: function (str, c) {
        if (str) { return (str.length - str.replace(new RegExp(c, "g"), '').length) / c.length; }
    },

    // Define Modal windows
    ModalFadeOut: function (hash) {
        hash.o.remove(); hash.w.fadeOut(100);
    },

    ModalFadeIn: function (hash) {
        hash.w.fadeIn(100);
    },

    IsSameElement: function (elm1, elm2) {
        return elm1.get(0) == elm2.get(0);
    },

    ResolveUrl: function (url) {
        if (url.indexOf("~/") == 0)
            url = this.BaseUrl + url.substring(2);

        return url;
    },

    //why not a generic message box, eh?
    MessageBox: function (boxId, title, actionFunc, html, dialogWidth) {
        if (actionFunc == null) actionFunc = function () { return; };

        if ((dialogWidth == null) || (dialogWidth == "")) dialogWidth = 300;

        if (html.length > 50) {
            dialogWidth = dialogWidth + ((html.length - 50) * 10);

            if (dialogWidth > 860) dialogWidth = 860;
        }

        $("<div id='messageBox_" + boxId + "'>" + html + "</div>").dialog(
            {
                modal: true,
                title: title,
                show: 'slide', hide: 'slide',
                width: dialogWidth,
                buttons: {
                    "OK": function () {
                        $(this).dialog("close");
                        actionFunc();
                    }
                }
            });
    },

    JqGridLoadError: function (grid, jqXHR, textStatus, errorThrown) {
        // remove error div if exist
        $('#' + grid.id + '_err').remove();
        // insert div with the error description before the grid
        $('#' + grid.id).closest('div.ui-jqgrid').before(
                        '<div id="' + grid.id + '_err" style="max-width:' + grid.style.width +
                        ';"><div class="ui-state-error ui-corner-all" style="padding:0.7em;float:left;"><span class="ui-icon ui-icon-alert" style="float:left; margin-right: .3em;"></span><span style="clear:left">' +
		                AppCommon.prototype.DecodeErrorMessage(jqXHR, textStatus, errorThrown) + '</span></div><div style="clear:left"/></div>');
    },

    DecodeErrorMessage: function (jqXHR, errorThrown, errorMsg) {
        var err = null;
        var html = '';

        try {
            err = eval('(' + jqXHR.responseText + ')');
            html = errorThrown + '<hr />' +
                    '<b>' + errorMsg + '</b><br /><br />' +
                    'Exception Type:  ' + err.ExceptionType + '<br\>' +
                    'Message:  ' + err.Message + '<br\>' +
                    'Stack Trace:  ' + err.StackTrace;
        } catch (ex) {
            html = jqXHR.status + " - " + jqXHR.statusText
        }

        this.MessageBox('divErrorMessage', errorThrown, null, html, null);
    }
};

