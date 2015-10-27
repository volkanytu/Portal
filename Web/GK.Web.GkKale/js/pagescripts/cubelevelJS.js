/// <reference path="../jquery/jquery.min.js" />
/// <reference path="../jquery/jquery.widget.min.js" />
/// <reference path="../jquery/purl.js" />
/// <reference path="GlobalVariables.js" />
/// <reference path="indexJS.js" />
/// <reference path="../jquery/jquery.session.js" />

var CubeLevelHelper = {
    "OnLoad": function () {
        $(parent.window).scrollTop(0);

        CubeLevelHelper.OnClickEvents();
        CubeLevelHelper.OnChangeEvents();

        parent.IndexHelper.AutoResize("ifrmContent");

    },
    "OnClickEvents": function () {
    },
    "OnChangeEvents": function () {
    },
    "GetQuestionLevels": function (callBackFunction) {
        //debugger;
        var jData = {};
        jData.token = CubeLevelHelper.Token;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/GetQuestionLevels",
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
                    $("#lblErrorMain").html("<h1 class='fg-yellow'><i class='icon-warning'></i></h1><br />" + ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />GetQuestionLevels");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), c + "<br />GetQuestionLevels");
                return false;
            }
        });
    },
    "CheckUserHasLimit": function (callBackFunction) {
        //debugger;
        var jData = {};
        jData.token = CubeLevelHelper.Token;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/HasUserQuestionLimit",
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

                    callBackFunction(data);

                    return;
                }
                else {
                    $("#lblErrorMain").show();
                    $("#lblErrorMain").html("<h1 class='fg-yellow'><i class='icon-warning'></i></h1><br />" + ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />CheckUserHasLimit");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), "Error:" + c + "<br />CheckUserHasLimit");
                return false;
            }
        });
    },
    "UserId": "",
    "PortalId": "",
    "Token": ""
};

function cubeLevelController($scope) {

    $(parent.window).scrollTop(0);

    var url = $.url(parent.document.location);
    CubeLevelHelper.Token = url.param("token");
    $scope.token = url.param("token");

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

                CubeLevelHelper.UserId = e.ReturnObject.PortalUserId;
                CubeLevelHelper.PortalId = e.ReturnObject.PortalId;

                $scope.userId = e.ReturnObject.PortalUserId;
                $scope.portalId = e.ReturnObject.PortalId;
            }
            else {
                return;
            }
        });
    });

    $scope.GetQuestionLevels = function () {

        CubeLevelHelper.GetQuestionLevels(function (e) {

            $scope.$apply(function () {
                var a = JSON.stringify(e);

                if (e.Success == true) {
                    $scope.cubeLevels = e.ReturnObject;

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
    };

    CubeLevelHelper.CheckUserHasLimit(function (e) {

        $scope.$apply(function () {
            var a = JSON.stringify(e);

            if (e.Success == true) {

                setTimeout(function () {
                    parent.IndexHelper.AutoResize("ifrmContent");
                }, 500);

                $scope.showErrorHeader = false;
                $scope.showLoading = false;
                $scope.showMain = true;

                $scope.GetQuestionLevels();

            }
            else {
                $scope.showErrorHeader = true;
                $scope.showLoading = false;
                $scope.showMain = false;
                $scope.errorText = ReturnMessage(parent.IndexHelper.LanguageCode, e.Result);
            }
        });

    });

    $scope.SelectLevel = function (id, name) {

        var isTrust = $scope.trustMyself;

        $("#ifrmContent", parent.document).attr("src", "bilgiKupuSoru.html?objectid=" + id + "&istrust=" + isTrust);
    };
}