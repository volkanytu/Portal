/// <reference path="../web/gk.web.gkportal/scripts/jquery/jquery.min.js" />
/// <reference path="../web/gk.web.gkportal/scripts/jquery/jquery.widget.min.js" />
/// <reference path="../web/gk.web.gkportal/scripts/jquery/purl.js" />
/// <reference path="../web/gk.web.gkportal/scripts/pagescripts/indexjs.js" />


var PageContentHelper = {
    "OnLoad": function () {

        var url = $.url(document.location);
        PageContentHelper.BaseUrl = url.data.attr.base;

        //if (VideoHelper.BaseUrl.indexOf("https") != -1) {
        //    VideoHelper.BaseUrl = VideoHelper.BaseUrl.replace("https", "http");
        //}

        var formType = Xrm.Page.ui.getFormType();

        PageContentHelper.OnClickEvents();
        PageContentHelper.OnChangeEvents();

        Xrm.Page.getAttribute("new_content").setSubmitMode("always");

        GlobalHelper.LoadScript("/ISV/ckeditor/ckeditor.js", function () {
            CKEDITOR.replace('new_content', { height: 400 });

        });

    },
    "OnClickEvents": function () {

    },
    "OnSave": function () {
        Xrm.Page.getAttribute("new_content").setValue(CKEDITOR.instances.new_content.getData());
    },
    "OnChangeEvents": function () {

    },
    "BaseUrl": ""
};