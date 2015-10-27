/// <reference path="../jquery/jquery.min.js" />
/// <reference path="../jquery/jquery.widget.min.js" />
/// <reference path="../jquery/purl.js" />
/// <reference path="languageMessagesJS.js" />

var CustomServiceUrl = "http://localhost:64831/";
//var CustomServiceUrl = "../../WebServices/REST/GK.WebServices.REST.CrmService/";

var attachmentUrl = "https://platform.4bizcrm.com/Web/GK.Web.GkPortal/attachments/";
//var attachmentUrl = "http://localhost:64831/attachments/";

var pageTypeCodes = [];
pageTypeCodes[1] = "graffiti.aspx";
pageTypeCodes[2] = "education.aspx";
pageTypeCodes[3] = "article.aspx";

var PageName = {
    "Welcome": 100000000,
    "Contract": 100000001,
    "PlatfromNedir": 100000002,
    "Manifesto": 100000003
};

function ReturnMessage(languageCode, messageCode) {

    var message = LanguageMessages[languageCode][messageCode];
    if (message == null || message == undefined || message == "" || message == "undefined") {
        return messageCode;
    }
    else {
        return message;
    }

}