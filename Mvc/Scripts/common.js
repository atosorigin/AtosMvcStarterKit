/// <reference path="_references.js" />

$(document).ready(function () {
    // Add an asterisk after each required field
    // Do not apply to checkbox field
    $("input[data-val-required][type!='hidden'][type!='checkbox'][type!='radio']").after("<span class='field-validation-error requiredIcon' title='Required'> *</span>");
    $("select[data-val-required][type!='hidden']").after("<span class='field-validation-error requiredIcon' title='Required'> *</span>");

    // For all input types that start with date, enable the datepicker
    if (!Modernizr.inputtypes.date) {
        $("input[type^=date]").datepicker();
    }

    jQuery.support.cors = true; // force cross-site scripting (as of jQuery 1.5)

    jQuery.extend({
        isEmpty: function (o, s) {
            if (o == null || o == undefined || o == "") {
                return s;
            }
            return o;
        }
    });
});

function toggleVisible(elementA, elementB) {
    if (elementA.is(':visible')) {
        elementA.hide();
        elementB.show();
    }
    else {
        elementA.show();
        elementB.hide();
    }
}

function trim(str) {
    var newstr;
    newstr = str.replace(/^\s*/, "").replace(/\s*$/, "");
    newstr = newstr.replace(/\s{2,}/, " ");
    return newstr;
}

function zeroPad(num, places) {
    var zero = places - num.toString().length + 1;
    return Array(+(zero > 0 && zero)).join("0") + num;
}

//Set defaults for the ajax calls
$.ajaxSetup({
    type: "POST",
    cache: false,
    error: function (e, xhr, settings, exception) {
        if (settings.error)
            return settings.error(e, xhr, settings, exception);
        //code to display message to user
        if (e.status != 0)
            alert('An unexpected ajax exception occured. Status: ' + e.status + '. ' + xhr.responseText);
    }
});

/* Extend jQuery with functions for PUT and DELETE requests. */

function _ajax_request(url, data, callback, type, method) {
    if (jQuery.isFunction(data)) {
        callback = data;
        data = {};
    }
    return jQuery.ajax({
        type: method,
        url: url,
        data: data,
        success: callback,
        dataType: type
    });
}

jQuery.extend({
    put: function (url, data, callback, type) {
        return _ajax_request(url, data, callback, type, 'PUT');
    },
    delete_: function (url, data, callback, type) {
        return _ajax_request(url, data, callback, type, 'DELETE');
    }
});

function loadPartialView(remoteUrl, target, data) {
    $.ajax({
        url: remoteUrl,
        type: 'GET',
        async: false,
        data: data,
        cache: false,
        success: function (result) {
            $(target).html(result);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $(target).html("<div class='error'>" + errorThrown + "</div>");
        }
    });
};