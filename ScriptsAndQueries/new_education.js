/// <reference path="../web/gk.web.gkportal/scripts/jquery/jquery.min.js" />
/// <reference path="../web/gk.web.gkportal/scripts/jquery/jquery.widget.min.js" />
/// <reference path="../web/gk.web.gkportal/scripts/jquery/purl.js" />
/// <reference path="../web/gk.web.gkportal/scripts/pagescripts/indexjs.js" />


var EducationHelper = {
    "OnLoad": function () {

        var uploadProfileImageUrl = "http://kaleanahtarcilarkulubu.com.tr/FileUpload/uploadprofileimage.html";
        var uploadFileUrl = "http://kaleanahtarcilarkulubu.com.tr/FileUpload/upload.html";

        var formType = Xrm.Page.ui.getFormType();

        if (formType == 1) //Create ise
        {
            Xrm.Page.ui.tabs.get("tab_imageupload").setVisible(false);
            Xrm.Page.ui.tabs.get("tab_attachments").setVisible(false);
        }
        else {
            var IFrame = Xrm.Page.ui.controls.get("IFRAME_objectimage");
            var Url = IFrame.getSrc();
            var params = Url.substr(Url.indexOf("?"));

            var ImageName = Xrm.Page.getAttribute("new_imageurl").getValue();
            newTarget = uploadProfileImageUrl + params + "&isimage=true&imagename=" + ImageName;
            IFrame.setSrc(newTarget);

            Xrm.Page.ui.tabs.get("tab_imageupload").setVisible(true);

            IFrame = Xrm.Page.ui.controls.get("IFRAME_fileupload");
            Url = IFrame.getSrc();
            params = Url.substr(Url.indexOf("?"));

            var portalId = Xrm.Page.getAttribute("new_portalid").getValue();
            newTarget = uploadFileUrl + params + "&portalid=" + portalId[0].id.replace('{', '').replace('}', '');
            IFrame.setSrc(newTarget);

            Xrm.Page.ui.tabs.get("tab_attachments").setVisible(true);
        }

        EducationHelper.OnClickEvents();
        EducationHelper.OnChangeEvents();

        GlobalHelper.LoadScript("/ISV/ckeditor/ckeditor.js", function () {
            CKEDITOR.replace('new_content', { height: 400 });

        });


    },
    "OnSave": function () {
        Xrm.Page.getAttribute("new_content").setValue(CKEDITOR.instances.new_content.getData());
    },
    "OnClickEvents": function () {


    },
    "OnChangeEvents": function () {

    }
};

