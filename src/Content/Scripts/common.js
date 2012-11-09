/*jslint debug: true, es5: false, evil: true, plusplus: true, regexp: true, todo: true, vars: true, white: true, browser: true, indent: 4 */
/*global window */                  //this is a global variable set elsewhere (browser object)
/*global self */                    //this is a global variable set elsewhere (browser object)
/*global $ */                       //this is a global variable set elsewhere (jQuery object)

//*************************************************************************************************************
// AppCommon.js
// Description - Code for common object used on this site.  Contains useful and re-usable behaviors.
//                  Please follow common ECMAScript code conventions when appending the AppCommon object.
//                  Please read http://javascript.crockford.com/code.html for more details regarding code
//                  conventions.
//*************************************************************************************************************

var AppCommon = new function () {
    'use strict';                       //See http://wiki.ecmascript.org/doku.php?id=proposals:strict_and_standard_modes&s=use+strict

    ///// public attributes /////////////////////////////////////////
    this.BaseUrl = "~/";
    this.ReloadTimer = null; 
    this.URL = decodeURI(window.location.pathname);
    ///////////////////////////////////////////////////////////////////

    ///// private functions //////////////////////////////////////////
    var getFormatFromJSONDateTime = function (jsonDate) {
        var date = null;
        try {
            date = Date.create(jsonDate);       //Using sugar.js here.  If not referenced, try the alternate method
        } catch (err) {
            date = eval(jsonDate.replace(/\/Date\((.*?)\)\//gi, 'new Date($1)'));       //TODO:  Re-write this to be more secure
        }
        return date;
    };
    //////////////////////////////////////////////////////////////////////

    ///// public functions ////////////////////////////////////////////////
    this.closeFormOK = function (strReturn) {
        if (!strReturn) { strReturn = true; }

        window.returnValue = strReturn;
        self.close();
    };

    this.closeFormCancel = function () {
        window.returnValue = '0';
        self.close();
    };

    this.formatJSONDateTime = function (jsonDate) {
        var date = getFormatFromJSONDateTime(jsonDate);
        var month = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

        return date.getDate() + " " +
                month[date.getMonth()] + " " +
                date.getFullYear() + " " +
                date.getHours() + ":" +
                this.FormatMinutes(date.getMinutes());
    };

    this.formatJSONDate = function (jsonDate) {
        var date = getFormatFromJSONDateTime(jsonDate);
        var month = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

        return date.getDate() + ' ' + month[date.getMonth()] + ' ' + date.getFullYear();
    };

    this.tickerSlider = function (container, delay) {
        if ($(container).length) {
            container = $(container);

            var slides = container.length, slide = 0;
            setInterval(function () {
                if (slide === slides) {
                    container.slideDown();
                    slide = 0;
                } else {
                    container.eq(slide).slideUp();
                    slide++;
                }
            }, delay);
        } else {
            return false;
        }
    };

    this.forcePageRefresh = function (seconds) {
        if (arguments.length === 1) {
            if (this.ReloadTimer) { clearTimeout(this.ReloadTimer); }

            var self = this;
            this.ReloadTimer = setTimeout(function () { self.ForcePageRefresh(); }, Math.ceil(parseFloat(seconds) * 1000));
        } else {
            this.ReloadTimer = null;
            location.reload(true);
            window.location.replace(this.URL);
        }
    };

    this.parseUrl = function (url) {
        var returnVal = null;
        var expr = /((http|ftp):\/)?\/?([^:\/\s]+)((\/\w+)*\/)([\w\-\.]+\.[^#?\s]+)(#[\w\-]+)?/;            //TODO:  Re-write this to be more secure

        if (url.match(expr)) {
            returnVal = { url: RegExp['$&'], protocol: RegExp.$2, host: RegExp.$3, path: RegExp.$4, file: RegExp.$6, hash: RegExp.$7 };
        } else {
            returnVal = { url: 'No match', protocol: '', host: '', path: '', file: '', hash: '' };
        }

        return returnVal;
    };

    this.stringEscape = function (str) {
        var specials = ['#', '&', '~', '=', '>', "'", ':', '"', '!', ';', ','];
        var regexSpecials = ['.', '*', '+', '|', '[', ']', '(', ')', '/', '^', '$'];
        var regex = new RegExp('(' + specials.join('|') + '|\\' + regexSpecials.join('|\\') + ')', 'g');

        return str.replace(regex, '\\$1');
    };

    this.getCharCount = function (str, c) {
        return (str.length - str.replace(new RegExp(c, "g"), '').length) / c.length;
    };

    // Define Modal windows
    this.modalFadeOut = function (hash) {
        hash.o.remove(); hash.w.fadeOut(100);
    };

    this.modalFadeIn = function (hash) {
        hash.w.fadeIn(100);
    };

    this.isSameElement = function (elm1, elm2) {
        return elm1.get(0) === elm2.get(0);
    };

    this.resolveUrl = function (url) {
        if (url.indexOf('~/') === 0) {
            url = this.BaseUrl + url.substring(2);
        }
        return url;
    };

    //why not a generic message box, eh?
    this.messageBox = function (boxId, title, actionFunc, html, dialogWidth) {
        if (actionFunc === null) { actionFunc = function () { return; }; }

        if ((dialogWidth === null) || (dialogWidth === "")) { dialogWidth = 300; }

        if (html.length > 50) {
            dialogWidth = dialogWidth + ((html.length - 50) * 10);

            if (dialogWidth > 860) { dialogWidth = 860; }
        }

        $('<div id=\'messageBox_' + boxId + '\'>' + html + '</div>').dialog(
            {
                modal: true,
                title: title,
                show: 'slide', hide: 'slide',
                width: dialogWidth,
                buttons: {
                    'OK': function () {
                        $(this).dialog('close');
                        actionFunc();
                    }
                }
            });
    };

    this.jqGridLoadError = function (grid, jqXHR, textStatus, errorThrown) {
        debugger;
        // remove error div if exist
        $('#' + grid.id + '_err').remove();
        // insert div with the error description before the grid
        $('#' + grid.id).closest('div.ui-jqgrid').before(
                        '<div id="' + grid.id + '_err" style="max-width:' + grid.style.width +
                        ';"><div class="ui-state-error ui-corner-all" style="padding:0.7em;float:left;"><span class="ui-icon ui-icon-alert" style="float:left; margin-right: .3em;"></span><span style="clear:left">' +
		                $(this)[0].decodeErrorMessage(jqXHR, textStatus, errorThrown) + '</span></div><div style="clear:left"/></div>');
    };

    this.decodeErrorMessage = function (jqXHR, errorThrown, errorMsg) {
        var err = null;
        var html = '';

        try {
            err = eval('(' + jqXHR.responseText + ')');
            html = errorThrown + '<hr />' +
                    '<b>' + errorMsg + '</b><br /><br />' +
                    'Exception Type:  ' + err.ExceptionType + '<br\\>' +
                    'Message:  ' + err.Message + '<br\\>' +
                    'Stack Trace:  ' + err.StackTrace;
        } catch (ex) {
            html = jqXHR.status + " - " + jqXHR.statusText;
        }

        $(this)[0].messageBox('divErrorMessage', errorThrown, null, html, null);
    };
};
