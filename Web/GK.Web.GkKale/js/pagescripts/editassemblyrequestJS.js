/// <reference path="../jquery/jquery.min.js" />
/// <reference path="../jquery/jquery.widget.min.js" />
/// <reference path="../jquery/purl.js" />
/// <reference path="GlobalVariables.js" />
/// <reference path="indexJS.js" />
/// <reference path="../jquery/jquery.session.js" />
/// <reference path="mainJS.js" />

var EditAssemblyRequestHelper = {
    "OnLoad": function () {
        $(parent.window).scrollTop(0);

        EditAssemblyRequestHelper.OnClickEvents();
        EditAssemblyRequestHelper.OnChangeEvents();

        parent.IndexHelper.AutoResize("ifrmContent");

    },
    "OnClickEvents": function () {
    },
    "OnChangeEvents": function () {
    },
    "GetRequestInfo": function (callBackFunction) {
        //debugger;
        var jData = {};
        jData.token = EditAssemblyRequestHelper.Token;
        jData.requestId = EditAssemblyRequestHelper.RequestId;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/GetRequestInfo",
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
                parent.IndexHelper.ToastrShow("error", c + "<br />GetRequestInfo", "HATA");
            }
        });
    },
    "CompleteRequest": function (callBackFunction) {
        //debugger;
        var jData = {};
        jData.token = EditAssemblyRequestHelper.Token;
        jData.requestId = EditAssemblyRequestHelper.RequestId;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/CompleteRequest",
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
                parent.IndexHelper.ToastrShow("error", c + "<br />GetRequestInfo", "HATA");
            }
        });
    },
    "UserId": "",
    "PortalId": "",
    "Token": "",
    "RequestId": ""
};

function assemblyRequestController($scope) {

    $(parent.window).scrollTop(0);

    var parentUrl = $.url(parent.document.location);
    var url = $.url(document.location);

    EditAssemblyRequestHelper.Token = parentUrl.param("token");
    $scope.token = parentUrl.param("token");

    EditAssemblyRequestHelper.RequestId = url.param("objectid");
    $scope.requestId = url.param("objectid");

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

    $scope.GetRequestInfo = function () {
        EditAssemblyRequestHelper.GetRequestInfo(function (e) {

            $scope.$apply(function () {
                var a = JSON.stringify(e);

                if (e.Success == true) {

                    $scope.request = e.ReturnObject;

                    if (e.ReturnObject.StatusCode.AttributeValue == 1) {
                        $scope.showUpdate = true;
                        $scope.status = "Açık";
                    }
                    else
                    {
                        $scope.status = "Kapalı";
                    }

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

                EditAssemblyRequestHelper.UserId = e.ReturnObject.PortalUserId;
                EditAssemblyRequestHelper.PortalId = e.ReturnObject.PortalId;

                $scope.userId = e.ReturnObject.PortalUserId;
                $scope.portalId = e.ReturnObject.PortalId;

                $scope.GetRequestInfo();
            }
            else {
                return;
            }
        });
    });

    $scope.CompleteRequest = function (id) {

        EditAssemblyRequestHelper.CompleteRequest(function (e) {

            $scope.$apply(function () {
                var a = JSON.stringify(e);

                if (e.Success == true) {
                    document.location.reload();
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
}