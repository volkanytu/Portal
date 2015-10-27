/// <reference path="../jquery/jquery.min.js" />
/// <reference path="../jquery/jquery.widget.min.js" />
/// <reference path="../jquery/purl.js" />
/// <reference path="GlobalVariables.js" />
/// <reference path="indexJS.js" />

var FriendshipHelper = {
    "OnLoad": function () {
        $(parent.window).scrollTop(0);

        FriendshipHelper.OnClickEvents();
        FriendshipHelper.OnChangeEvents();

        parent.IndexHelper.AutoResize("ifrmContent");
    },
    "OnClickEvents": function () {
    },
    "OnChangeEvents": function () {
    },
    "GetUserFriendList": function (callBackFunction) {
        //debugger;
        var jData = {};
        jData.token = FriendshipHelper.Token;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/GetUserFriendList",
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
                    $("#lblErrorMain").html("<h1 class='fg-yellow'><i class='icon-warning'></i></h1><br />" + ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />GetUserFriendList");

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

function friendshipController($scope) {

    $(parent.window).scrollTop(0);

    var url = $.url(parent.document.location);
    FriendshipHelper.Token = url.param("token");
    $scope.token = url.param("token");

    parent.IndexHelper.AutoResize("ifrmContent");

    $scope.attachmentUrl = attachmentUrl;
    $scope.ContentMessages = LanguageMessages[parent.IndexHelper.LanguageCode];

    $scope.showErrorHeader = false;
    $scope.showLoading = true;
    $scope.showMain = false;
    $scope.errorText = "";

    $scope.showAllFriends = true;
    $scope.showMyRequests = false;
    $scope.showuserRequests = false;

    parent.IndexHelper.CheckSession(true, $scope.token, function (e) {
        $scope.$apply(function () {
            if (e.Success == true) {

                FriendshipHelper.UserId = e.ReturnObject.PortalUserId;
                FriendshipHelper.PortalId = e.ReturnObject.PortalId;

                $scope.userId = e.ReturnObject.PortalUserId;
                $scope.portalId = e.ReturnObject.PortalId;
            }
            else {
                return;
            }
        });
    });

    FriendshipHelper.GetUserFriendList(function (e) {

        $scope.$apply(function () {
            var a = JSON.stringify(e);

            if (e.Success == true) {

                $scope.allFriends = e.ReturnObject.filter(function (el) {
                    return (el.UserType === 1);
                });

                $scope.myRequests = e.ReturnObject.filter(function (el) {
                    return (el.UserType === 2);
                });


                $scope.userRequests = e.ReturnObject.filter(function (el) {
                    return (el.UserType === 3);
                });

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

    $scope.AllFriends = function () {
        $scope.showAllFriends = true;
        $scope.showMyRequests = false;
        $scope.showuserRequests = false;

        $scope.noRecordUserRequests = false;
        $scope.noRecordMyRequests = false;

        if ($scope.allFriends.length > 0) {
            $scope.noRecordAllFriends = false;
        }
        else {
            $scope.noRecordAllFriends = true;
        }
    };

    $scope.MyRequests = function () {
        $scope.showAllFriends = false;
        $scope.showMyRequests = true;
        $scope.showuserRequests = false;

        $scope.noRecordUserRequests = false;
        $scope.noRecordAllFriends = false;

        if ($scope.myRequests.length > 0) {
            $scope.noRecordMyRequests = false;
        }
        else {
            $scope.noRecordMyRequests = true;
        }

    };

    $scope.UserRequests = function () {
        $scope.showAllFriends = false;
        $scope.showMyRequests = false;
        $scope.showuserRequests = true;

        $scope.noRecordMyRequests = false;
        $scope.noRecordAllFriends = false;

        if ($scope.userRequests.length > 0) {
            $scope.noRecordUserRequests = false;
        }
        else {
            $scope.noRecordUserRequests = true;
        }
    };

    $scope.EditUser = function (userId) {

        parent.angular.element('html').scope().EditUser(userId)
    };
}