/// <reference path="../jquery/jquery.min.js" />
/// <reference path="../jquery/jquery.widget.min.js" />
/// <reference path="../jquery/purl.js" />
/// <reference path="GlobalVariables.js" />
/// <reference path="../jquery/jquery.session.js" />
/// <reference path="indexJS - DO.js" />
/// <reference path="../jquery/jquery.session.js" />
var baseURL;
var portalId;

//var app = angular.module('loginApp', ['ngRoute']);
//app.controller('loginController', ['$scope', function ($scope) {
//    $scope.ContentMessages = LanguageMessages[LoginHelper.LanguageCode];
//}]);


var LoginHelper = {
    "OnLoad": function () {
        var url = $.url(document.location);
        baseURL = url.data.attr["base"];

        LoginHelper.OnClickEvents();
        LoginHelper.OnChangeEvents();
        LoginHelper.KeyPressEvents();

        $("#txtUserName").focus();


    },
    "OnClickEvents": function () {
        $("#btnLogIn").click(function () {

            LoginHelper.CheckLogin();

        });

        $("#btnChangePass").click(function () {

            document.location = "passremember.html?portalid=" + LoginHelper.PortalInfo.ReturnObject.Id;

        });

    },
    "OnChangeEvents": function () {

    },
    "KeyPressEvents": function () {
        $("#txtUserName").keydown(function (e) {
            if (e.which == 13) {
                $("#btnLogIn").click();
            }
        });

        $("#txtPassword").keydown(function (e) {
            if (e.which == 13) {
                $("#btnLogIn").click();
            }
        });
    },
    "GetPortalInfo": function (callBackFunction) {
        var jData = {};
        jData.url = baseURL;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/GetPortal",
            async: true,
            dataType: "json",
            contentType: "application/json;",
            type: "POST",
            data: jSonData,
            beforeSend: function () {

            },
            complete: function () {

            },
            success: function (data) {
                //debugger;
                if (data != null) {
                    //debugger;
                    data = JSON.parse(data);

                    callBackFunction(data);
                }
                else {
                    $("#lblErrorMain").show();
                    $("#lblErrorMain").html(ReturnMessage(LoginHelper.LanguageCode, "M002") + "<br />GetPortalId");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                $("#lblErrorMain").show();
                $("#lblErrorMain").html(c + "<br />GetPortalId");
                return false;
            }
        });
    },
    "CheckLogin": function (loginObject, callBackFunction) {

        //debugger;
        //var jData = {};

        //jData.portalId = loginObject.portalId;
        //jData.userName = loginObject.userName;
        //jData.password = loginObject.password;

        var jSonData = JSON.stringify(loginObject);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/CheckLogin",
            async: true,
            dataType: "json",
            contentType: "application/json;",
            type: "POST",
            data: jSonData,
            beforeSend: function () {

            },
            complete: function () {

            },
            success: function (data) {
                //debugger;
                if (data != null) {

                    callBackFunction(data);
                    //debugger;
                    //if (data.Success) {
                    //    //$.session.set("userId", data.CrmId).set("portalId", portalId).set("login", "OK").set("loginLogId", data.Result);
                    //    $.session.set("token", data.Result).set("loginLogId", data.CrmId);
                    //    document.location.href = "main.html?token=" + data.Result;
                    //}
                    //else {
                    //    $("#txtPassword").focus();
                    //    IndexHelper.ShowNotify(ReturnMessage(LoginHelper.LanguageCode, "M059"), ReturnMessage(LoginHelper.LanguageCode, data.Result), "orange", "white");
                    //}
                }
                else {
                    IndexHelper.ShowNotify(ReturnMessage(LoginHelper.LanguageCode, "M059"), ReturnMessage(LoginHelper.LanguageCode, "M002"), "red", "white");
                    $("#txtUserName").focus();
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                IndexHelper.ShowNotify(ReturnMessage(LoginHelper.LanguageCode, "M059"), c + "<br />CheckLogin", "red", "white");
                $("#txtUserName").focus();
                return false;
            }
        });
    },
    "ShowDialog": function ShowDialog(html, title, className, showCloseButton, closeCallBackFunction) {
        bootbox.dialog({
            message: html,
            title: title,
            className: className,
            closeButton: showCloseButton == null ? true : showCloseButton,
            onEscape: closeCallBackFunction
        });
    },
    "CloseDialog": function () {

        bootbox.hideAll();
    },
    "ShowNotify": function (message, type) {
        //Types:success,info,warn,error

        $.notify(message, type);
    },
    "PortalInfo": {},
    "LanguageCode": "tr"
};

function loginController($scope) {
    var a = FindAndReplaceAll("Ã¶Ã§ÅiÄÃ¼");
    var url = $.url(document.location);
    baseURL = url.data.attr["base"];

    $scope.ContentMessages = LanguageMessages[LoginHelper.LanguageCode];

    $scope.showUserInteraction = true;
    $scope.showErrorHeader = false;
    $scope.showError = false;
    $scope.showLoading = true;
    $scope.showMain = false;
    $scope.errorText = "";

    LoginHelper.GetPortalInfo(function (e) {
        $scope.$apply(function () {
            if (e.Success == true) {

                $scope.showUserInteraction = false;
                $scope.showErrorHeader = false;
                $scope.showLoading = false;
                $scope.showMain = true;

                $scope.portal = e.ReturnObject;
            }
            else {
                $scope.showErrorHeader = true;
                $scope.showLoading = false;
                $scope.showMain = false;
                $scope.errorText = ReturnMessage(LoginHelper.LanguageCode, e.Result);
            }
        });
    });

    $scope.CheckLogin = function ($keyEvent) {

        $scope.showError = false;

        if ($scope.loginObject == null || $scope.loginObject == undefined) {
            $scope.errorText = "Kullanıcı Adı veya Şifre boş olamaz.";
            $scope.showError = true;

            return;
        }

        if ($keyEvent == null || $keyEvent.which == 13) {

            $scope.showCheckLoading = true;

            $scope.loginObject.portalId = $scope.portal.Id;

            LoginHelper.CheckLogin($scope.loginObject, function (e) {

                $scope.showCheckLoading = false;

                $scope.$apply(function () {
                    if (e.Success == true) {

                        $.session.set("token", e.Result).set("loginLogId", e.CrmId);
                        document.location.href = "main.html?token=" + e.Result;
                    }
                    else {
                        $scope.errorText = ReturnMessage(LoginHelper.LanguageCode, e.Result)
                        $scope.showError = true;
                    }
                });
            });
        }
    };

    $scope.ForgotPassword = function () {
        document.location.href = "passwordForgot.html?portalid=" + $scope.portal.Id;
    };

    $scope.OpenRegisterPage = function () {

        LoginHelper.ShowDialog("<iframe src='popupRegister.html' style='width:100%;height:570px;' />", null, null, true);
    };
}

function FindAndReplaceAll(text) {
    var normal = new Array("Ä±", "Å?", "Ã¼", "Ã§", "Ã¶", "Ä?", "ÅŸ", "Ã‡", "Ä°", "ÄŸ", "Åž", "Ã–", "Ãœ", "Ä±", "Å?", "Ã§", "Ã¶", "Ä?", "ÅŸ", "Ã‡", "Ä°", "ÄŸ", "Åž", "Ã–", "Ãœ", "Ã¼", "ÄŸ", "Ä", "Å");
    var turkish = new Array("ı", "ş", "ü", "ç", "ö", "ğ", "ş", "Ç", "i", "ğ", "Ş", "Ö", "Ü", "ı", "ş", "ç", "ö", "ğ", "ş", "Ç", "i", "ğ", "Ş", "Ö", "Ü", "ü", "ğ","ğ","ü");

    for (var i = 0; i < normal.length; i++) {

        if (text.indexOf(normal[i]) != -1) {

            text = text.replace(normal[i], turkish[i]);
        }
    }

    return text;
}