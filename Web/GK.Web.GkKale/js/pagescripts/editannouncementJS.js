/// <reference path="../jquery/jquery.min.js" />
/// <reference path="../jquery/jquery.widget.min.js" />
/// <reference path="../jquery/purl.js" />
/// <reference path="GlobalVariables.js" />
/// <reference path="indexJS.js" />
/// <reference path="../jquery/jquery.session.js" />

var AnnouncementHelper = {
    "OnLoad": function () {

        $(parent.window).scrollTop(0);

        //var url = $.url(document.location);
        //AnnouncementHelper.ObjectId = url.param("objectid");

        //AnnouncementHelper.GetAnnouncementInfo();

        parent.IndexHelper.AutoResize("ifrmContent");

        AnnouncementHelper.OnClickEvents();
        AnnouncementHelper.OnChangeEvents();
    },
    "OnClickEvents": function () {
    },
    "OnChangeEvents": function () {
    },
    "GetAnnouncementInfo": function (callBackFunction) {
        //debugger;
        var jData = {};
        jData.token = AnnouncementHelper.Token;

        jData.announcementId = AnnouncementHelper.ObjectId;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/GetAnnouncementInfo",
            async: true,
            dataType: "json",
            contentType: "application/json;",
            type: "POST",
            data: jSonData,
            beforeSend: function () {
            },
            complete: function () {
                parent.IndexHelper.AutoResize("ifrmContent");
            },
            success: function (data) {
                //debugger;
                if (data != null) {
                    data = JSON.parse(data);

                    callBackFunction(data);

                    return;
                }
                else {
                    $("#lblErrorMain").show();
                    $("#lblErrorMain").html("<h1 class='fg-yellow'><i class='icon-warning'></i></h1><br />" + ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />GetAnnouncementInfo");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), c + "<br />GetAnnouncementInfo");
                return false;
            }
        });
    },
    "UserId": "",
    "PortalId": "",
    "ObjectId": "",
    "Token": ""
};

function announcementDetailController($scope, $sce) {

    $(parent.window).scrollTop(0);

    var parentUrl = $.url(parent.document.location);
    var url = $.url(document.location);

    AnnouncementHelper.Token = parentUrl.param("token");
    $scope.token = parentUrl.param("token");

    AnnouncementHelper.ObjectId = url.param("objectid");
    $scope.announcementId = url.param("objectid");

    parent.IndexHelper.AutoResize("ifrmContent");

    $scope.attachmentUrl = attachmentUrl;
    $scope.ContentMessages = LanguageMessages[parent.IndexHelper.LanguageCode];

    $scope.showErrorHeader = false;
    $scope.showLoading = true;
    $scope.showMain = false;
    $scope.errorText = "";

    parent.IndexHelper.CheckSession(true, $scope.token, function (e) {
        $scope.$apply(function () {
            if (e.Success == true) {

                AnnouncementHelper.UserId = e.ReturnObject.PortalUserId;
                AnnouncementHelper.PortalId = e.ReturnObject.PortalId;

                $scope.userId = e.ReturnObject.PortalUserId;
                $scope.portalId = e.ReturnObject.PortalId;
            }
            else {
                return;
            }
        });
    });

    AnnouncementHelper.GetAnnouncementInfo(function (e) {

        $scope.$apply(function () {
            var a = JSON.stringify(e);

            if (e.Success == true) {

                $scope.announcement = e.ReturnObject;

                $scope.announcementDesc = $sce.trustAsHtml(e.ReturnObject.Description);

                setTimeout(function () {
                    parent.IndexHelper.AutoResize("ifrmContent");
                }, 500);

                $scope.showErrorHeader = false;
                $scope.showLoading = false;
                $scope.showMain = true;
            }
            else {
                $scope.showErrorHeader = true;
                $scope.showLoading = false;
                $scope.showMain = false;
                $scope.errorText = ReturnMessage(parent.IndexHelper.LanguageCode, e.Result);
            }
        });

    });
}