/// <reference path="../web/gk.web.gkportal/scripts/jquery/jquery.min.js" />
/// <reference path="../web/gk.web.gkportal/scripts/jquery/jquery.widget.min.js" />
/// <reference path="../web/gk.web.gkportal/scripts/jquery/purl.js" />
/// <reference path="../web/gk.web.gkportal/scripts/pagescripts/indexjs.js" />


var EducationCategoryHelper = {
    "OnLoad": function () {

        var uploadProfileImageUrl = "http://kaleanahtarcilarkulubu.com.tr/FileUpload/uploadprofileimage.html";
        var uploadFileUrl = "http://kaleanahtarcilarkulubu.com.tr/FileUpload/upload.html";

        var formType = Xrm.Page.ui.getFormType();

        if (formType == 1) //Create ise
        {
            Xrm.Page.ui.tabs.get("tab_imageupload").setVisible(false);
        }
        else {
            var IFrame = Xrm.Page.ui.controls.get("IFRAME_objectimage");
            var Url = IFrame.getSrc();
            var params = Url.substr(Url.indexOf("?"));

            var ImageName = Xrm.Page.getAttribute("new_imageurl").getValue();
            newTarget = uploadProfileImageUrl + params + "&isimage=true&imagename=" + ImageName;
            IFrame.setSrc(newTarget);

            Xrm.Page.ui.tabs.get("tab_imageupload").setVisible(true);
        }

        EducationCategoryHelper.OnClickEvents();
        EducationCategoryHelper.OnChangeEvents();

    },
    "OnSave": function () {
        
    },
    "OnClickEvents": function () {


    },
    "OnChangeEvents": function () {

    }
};

