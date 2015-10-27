/// <reference path="../jquery/jquery.min.js" />
/// <reference path="../jquery/jquery.widget.min.js" />
/// <reference path="../jquery/purl.js" />
/// <reference path="GlobalVariables.js" />
/// <reference path="indexJS.js" />


var WhoisHelper = {
    "OnLoad": function () {
        $(parent.window).scrollTop(0);

        WhoisHelper.OnClickEvents();
        WhoisHelper.OnChangeEvents();
        WhoisHelper.KeyPressEvents();

        parent.IndexHelper.AutoResize("ifrmContent");
    },
    "OnClickEvents": function () {
    },
    "OnChangeEvents": function () {
    },
    "KeyPressEvents": function () {
    },
    "SearchContact": function (key, callBackFunction) {
        //debugger;
        var jData = {};
        jData.token = WhoisHelper.Token;
        jData.key = key;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/SearchContact",
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
                    $("#lblErrorMain").html("<h1 class='fg-yellow'><i class='icon-warning'></i></h1><br />" + ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />SearchContact");
                    parent.IndexHelper.AutoResize("ifrmContent");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), c + "<br />GetUserFriendList");
                return false;
            }
        });
    },
    "UserId": "",
    "PortalId": "",
    "Token": ""
};

function whoisController($scope) {

    $(parent.window).scrollTop(0);

    var url = $.url(parent.document.location);
    WhoisHelper.Token = url.param("token");
    $scope.token = url.param("token");

    parent.IndexHelper.AutoResize("ifrmContent");

    $scope.attachmentUrl = attachmentUrl;
    $scope.ContentMessages = LanguageMessages[parent.IndexHelper.LanguageCode];

    $scope.showErrorHeader = false;
    $scope.showLoading = false;
    $scope.errorText = "";
    $scope.noRecord = true;

    parent.IndexHelper.CheckSession(true, $scope.token, function (e) {
        $scope.$apply(function () {
            if (e.Success == true) {

                WhoisHelper.UserId = e.ReturnObject.PortalUserId;
                WhoisHelper.PortalId = e.ReturnObject.PortalId;

                $scope.userId = e.ReturnObject.PortalUserId;
                $scope.portalId = e.ReturnObject.PortalId;
            }
            else {
                return;
            }
        });
    });

    $scope.txtSearch = null;

    $scope.Search = function ($keyEvent) {

        if ($keyEvent == null || $keyEvent.which == 13) {

            if ($scope.txtSearch == null) {

                alert(ReturnMessage(parent.IndexHelper.LanguageCode, "M090"));
                return;
            }

            $scope.showLoading = true;
            $scope.noRecord = true;

            WhoisHelper.SearchContact($scope.txtSearch, function (e) {

                $scope.$apply(function () {
                    var a = JSON.stringify(e);

                    if (e.Success == true) {

                        $scope.contacts = e.ReturnObject;

                        setTimeout(function () {
                            parent.IndexHelper.AutoResize("ifrmContent");
                        }, 500);

                        $scope.showErrorHeader = false;
                        $scope.showLoading = false;
                        $scope.noRecord = false;
                    }
                    else {
                        $scope.showErrorHeader = true;
                        $scope.showLoading = false;
                        $scope.errorText = ReturnMessage(parent.IndexHelper.LanguageCode, e.Result);
                        $scope.noRecord = true;
                    }
                });

            });
        }
    };

    $scope.EditUser = function (userId) {

        parent.angular.element('html').scope().EditUser(userId)
    };
}