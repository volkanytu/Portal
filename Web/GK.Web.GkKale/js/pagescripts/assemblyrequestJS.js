/// <reference path="../jquery/jquery.min.js" />
/// <reference path="../jquery/jquery.widget.min.js" />
/// <reference path="../jquery/purl.js" />
/// <reference path="GlobalVariables.js" />
/// <reference path="indexJS.js" />
/// <reference path="../jquery/jquery.session.js" />
/// <reference path="mainJS.js" />

var AssemblyRequestHelper = {
    "OnLoad": function () {
        $(parent.window).scrollTop(0);

        AssemblyRequestHelper.OnClickEvents();
        AssemblyRequestHelper.OnChangeEvents();

        parent.IndexHelper.AutoResize("ifrmContent");

    },
    "OnClickEvents": function () {
    },
    "OnChangeEvents": function () {
    },
    "GetRequests": function (callBackFunction) {
        //debugger;
        var jData = {};
        jData.token = AssemblyRequestHelper.Token;
        jData.userId = AssemblyRequestHelper.UserId;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/GetRequests",
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
                    $("#lblErrorMain").html("<h1 class='fg-yellow'><i class='icon-warning'></i></h1><br />" + ReturnMessage(parent.IndexHelper.LanguageCode, "M002") + "<br />GetRequests");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ToastrShow("error",c + "<br />GetRequests","HATA");
            }
        });
    },
    "UserId": "",
    "PortalId": "",
    "Token": "",
    "PageRecord": 5
};

function assemblyRequestController($scope) {

    $(parent.window).scrollTop(0);

    var url = $.url(parent.document.location);
    AssemblyRequestHelper.Token = url.param("token");
    $scope.token = url.param("token");

    parent.IndexHelper.AutoResize("ifrmContent");

    $scope.attachmentUrl = attachmentUrl;
    $scope.ContentMessages = LanguageMessages[parent.IndexHelper.LanguageCode];

    $scope.showErrorHeader = false;
    $scope.showLoading = true;
    $scope.showMain = false;
    $scope.errorText = "";

    $scope.noRecord = true;

    //TEST İÇİN
    //$scope.showErrorHeader = false;
    //$scope.showLoading = false;
    //$scope.showMain = true;
    //$scope.noRecord = false;
    ////////////////////////////////////

    $scope.GetRequests = function () {
        AssemblyRequestHelper.GetRequests(function (e) {

            $scope.$apply(function () {
                var a = JSON.stringify(e);

                if (e.Success == true) {

                    if (e.ReturnObject != null && e.ReturnObject.length > 0) {
                        $scope.noRecord = false;
                    }

                    $scope.requests = e.ReturnObject;

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

    parent.IndexHelper.CheckSession(true, $scope.token, function (e) {
        $scope.$apply(function () {
            if (e.Success == true) {

                AssemblyRequestHelper.UserId = e.ReturnObject.PortalUserId;
                AssemblyRequestHelper.PortalId = e.ReturnObject.PortalId;

                $scope.userId = e.ReturnObject.PortalUserId;
                $scope.portalId = e.ReturnObject.PortalId;

                $scope.GetRequests();
            }
            else {
                return;
            }
        });
    });

    $scope.EditRequest = function (id) {

        $("#ifrmContent", parent.document).attr("src", "isEmriGoruntule.html?objectid=" + id);
    };
}