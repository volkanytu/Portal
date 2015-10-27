/// <reference path="../jquery/jquery.min.js" />
/// <reference path="../jquery/jquery.widget.min.js" />
/// <reference path="../jquery/purl.js" />
/// <reference path="GlobalVariables.js" />
/// <reference path="indexJS.js" />
/// <reference path="../jquery/jquery.session.js" />

var AnnouncementListHelper = {
    "OnLoad": function () {
        $(parent.window).scrollTop(0);

        AnnouncementListHelper.OnClickEvents();
        AnnouncementListHelper.OnChangeEvents();

        parent.IndexHelper.AutoResize("ifrmContent");

    },
    "OnClickEvents": function () {
    },
    "OnChangeEvents": function () {
    },
    "GetAnnouncements": function (start, end, isFirst, callBackFunction) {
        //debugger;
        var jData = {};
        jData.token = AnnouncementListHelper.Token;
        jData.start = start;
        jData.end = end;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/GetAnnouncementList",
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
                    $("#lblErrorMain").html("<h1 class='fg-yellow'><i class='icon-warning'></i></h1><br />" + ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />GetAnnouncements");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), c + "<br />GetAnnouncements");
            }
        });
    },
    "UserId": "",
    "PortalId": "",
    "Token": "",
    "PageRecord": 5
};

function announcementController($scope) {

    $(parent.window).scrollTop(0);

    var url = $.url(parent.document.location);
    AnnouncementListHelper.Token = url.param("token");
    $scope.token = url.param("token");

    parent.IndexHelper.AutoResize("ifrmContent");

    $scope.attachmentUrl = attachmentUrl;
    $scope.ContentMessages = LanguageMessages[parent.IndexHelper.LanguageCode];

    $scope.showErrorHeader = false;
    $scope.showLoading = true;
    $scope.showMain = false;
    $scope.errorText = "";

    $scope.noRecord = true;

    parent.IndexHelper.CheckSession(true, $scope.token, function (e) {
        $scope.$apply(function () {
            if (e.Success == true) {

                AnnouncementListHelper.UserId = e.ReturnObject.PortalUserId;
                AnnouncementListHelper.PortalId = e.ReturnObject.PortalId;

                $scope.userId = e.ReturnObject.PortalUserId;
                $scope.portalId = e.ReturnObject.PortalId;
            }
            else {
                return;
            }
        });
    });

    AnnouncementListHelper.GetAnnouncements(0, 20, true, function (e) {

        $scope.$apply(function () {
            var a = JSON.stringify(e);

            if (e.Success == true) {

                if (e.ReturnObject != null && e.ReturnObject.length > 0) {
                    $scope.noRecord = false;
                }

                $scope.announcements = e.ReturnObject;

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

    $scope.EditAnnouncement = function (id, name) {

        $("#ifrmContent", parent.document).attr("src", "duyurularDetay.html?objectid=" + id);
    };
}