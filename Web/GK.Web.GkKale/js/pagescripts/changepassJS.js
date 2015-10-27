/// <reference path="../jquery/jquery.min.js" />
/// <reference path="../jquery/jquery.widget.min.js" />
/// <reference path="../jquery/purl.js" />
/// <reference path="GlobalVariables.js" />
/// <reference path="../jquery/jquery.session.js" />
/// <reference path="indexJS.js" />
/// <reference path="../jquery/jquery.session.js" />
var baseURL;
var portalId;
var _interval;

var app = angular.module('changePassApp', ['ngRoute']);
app.controller('changePassController', ['$scope', function ($scope) {
    $scope.ContentMessages = LanguageMessages[ChangePasswordHelper.LanguageCode];
}]);


var ChangePasswordHelper = {
    "OnLoad": function () {
        var url = $.url(document.location);
        baseURL = url.data.attr["base"];

        ChangePasswordHelper.Token = url.param("token");

        ChangePasswordHelper.OnClickEvents();
        ChangePasswordHelper.OnChangeEvents();
        ChangePasswordHelper.KeyPressEvents();

    },
    "OnClickEvents": function () {
        $("#btnChangePassword").click(function () {

            $("#rowError").hide();

            var txtNewPass = $("#txtNewPassword").val();
            var txtRePass = $("#txtReType").val();

            if (txtNewPass == null || txtNewPass == "" || txtRePass == null || txtRePass == "") {
                $("#lblMessage").html(ReturnMessage(ChangePasswordHelper.LanguageCode, "M081"));
                $("#rowError").show();

                return;
            }

            if (txtNewPass != txtRePass) {
                $("#lblMessage").html(ReturnMessage(ChangePasswordHelper.LanguageCode, "M080"));
                $("#rowError").show();

                return;
            }


            ChangePasswordHelper.UpdatePassword(ChangePasswordHelper.Token, txtNewPass);

        });

    },
    "OnChangeEvents": function () {

    },
    "KeyPressEvents": function () {
        $("#txtNewPassword").keydown(function (e) {
            if (e.which == 13) {
                $("#btnChangePassword").click();
            }
        });

        $("#txtReType").keydown(function (e) {
            if (e.which == 13) {
                $("#btnChangePassword").click();
            }
        });

    },
    "UpdatePassword": function (newPassword, callBakcFunction) {
        //debugger;
        var jData = {};
        jData.token = ChangePasswordHelper.Token;
        jData.newPassword = newPassword;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/UpdateUserPassword",
            async: true,
            dataType: "json",
            contentType: "application/json;",
            type: "POST",
            data: jSonData,
            beforeSend: function () {
                ChangePasswordHelper.ShowLoading("Şifreniz güncelleniyor...");
            },
            complete: function () {
                //ChangePasswordHelper.CloseDialog();
            },
            success: function (data) {
                //debugger;
                if (data != null) {
                    //debugger;

                    callBakcFunction(data);

                    return;
                }
                else {
                    ChangePasswordHelper.CloseDialog();
                    $("#rowError").show();
                    $("#lblMessage").html(ReturnMessage(LoginHelper.LanguageCode, "M002") + "<br />UpdatePassword");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                ChangePasswordHelper.CloseDialog();
                $("#rowError").show();
                $("#lblMessage").html(c + "<br />UpdatePassword");
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
    "ShowLoading": function (message, title) {

        var html = "";
        html += "<div id='divLoadingMain' ng-show='showLoading' style='text-align:center;'>";
        html += "	<img src='images/loading.gif' width='48' />";
        html += "	<br />";
        html += "	<br />";
        html += "	<h4 style='text-align:center;'>" + message + "</h4>";
        html += "</div>";

        ChangePasswordHelper.ShowDialog(html, title, "modal20", false);
    },
    "ToastrShow": function (type, message, title) {
        //Types:success,info,warning,error

        toastr.options = {
            "closeButton": true,
            "debug": false,
            "positionClass": "toast-top-right",
            "onclick": null,
            "showDuration": "1000",
            "hideDuration": "5000",
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "progressBar": true,
            "hideMethod": "fadeOut"
        }

        toastr[type](message, title);

    },
    "CloseDialog": function () {

        bootbox.hideAll();
    },
    "PortalId": "",
    "LanguageCode": "tr",
    "Token": ""
};

function passChangeController($scope, $sce) {

    $(parent.window).scrollTop(0);

    var url = $.url(document.location);

    ChangePasswordHelper.Token = url.param("token");
    $scope.token = url.param("token");

    $scope.ContentMessages = LanguageMessages[ChangePasswordHelper.LanguageCode];

    $scope.showError = false;
    $scope.errorText = "";

    $scope.UpdatePassword = function () {

        var newPass = $scope.newPass;
        var reType = $scope.reType;

        if (newPass == null || newPass == "" || reType == null || reType == "") {

            ChangePasswordHelper.ToastrShow("warning", ReturnMessage(ChangePasswordHelper.LanguageCode, "M081"));

            $scope.showError = true;
            $scope.errorText = ReturnMessage(ChangePasswordHelper.LanguageCode, "M081");

            return;
        }

        if (newPass != reType) {
            ChangePasswordHelper.ToastrShow("warning", ReturnMessage(ChangePasswordHelper.LanguageCode, "M080"));

            $scope.showError = true;
            $scope.errorText = ReturnMessage(ChangePasswordHelper.LanguageCode, "M080");

            return;
        }

        ChangePasswordHelper.UpdatePassword(newPass, function (e) {

            if (e.Success == true) {

                ChangePasswordHelper.ShowDialog(ReturnMessage(ChangePasswordHelper.LanguageCode, "M011"), "Başarılı", null, true, function () {

                    document.location = "login.html";
                });
            }
            else {
                ChangePasswordHelper.ToastrShow("warning", ReturnMessage(ChangePasswordHelper.LanguageCode, e.Result));

                $scope.$apply(function () {
                    $scope.showError = true;
                    $scope.errorText = ReturnMessage(ChangePasswordHelper.LanguageCode, e.Result);
                });
            }
        });

    };
}