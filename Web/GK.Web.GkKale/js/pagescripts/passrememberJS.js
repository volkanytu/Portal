/// <reference path="../jquery/jquery.min.js" />
/// <reference path="../jquery/jquery.widget.min.js" />
/// <reference path="../jquery/purl.js" />
/// <reference path="GlobalVariables.js" />
/// <reference path="../jquery/jquery.session.js" />
/// <reference path="../jquery/jquery.session.js" />
var baseURL;
var portalId;
var _interval;

var PassRememberHelper = {
    "OnLoad": function () {

        PassRememberHelper.PortalId = url.param("portalid");

        PassRememberHelper.OnClickEvents();
        PassRememberHelper.OnChangeEvents();
        PassRememberHelper.KeyPressEvents();

        $('#txtMobilePhone').mask("999-9999999");

    },
    "OnClickEvents": function () {
        $("#btnSendSms").click(function () {

            $("#rowError").hide();

            var txtUserName = $("#txtUserName").val();
            var txtMobilePhone = $("#txtMobilePhone").val();

            if (txtUserName == null || txtUserName == "" || txtMobilePhone == null || txtMobilePhone == "") {
                $("#lblMessage").html(ReturnMessage(PassRememberHelper.LanguageCode, "CM120"));
                $("#rowError").show();

                return;
            }

            PassRememberHelper.CheckPhoneNumberMatch(txtUserName, txtMobilePhone.replace("-", ""));

        });

        $("#btnConfirm").click(function () {

            var txtCode = $("#txtCode").val();

            if (txtCode != null && txtCode != "") {

                PassRememberHelper.ConfirmCode(txtCode, PassRememberHelper.Token);
            }
            else {
                $("#lblMessage").html(ReturnMessage(PassRememberHelper.LanguageCode, "CM121"));
                $("#rowError").show();

                return;
            }
        });
    },
    "OnChangeEvents": function () {

    },
    "KeyPressEvents": function () {
        $("#txtUserName").keydown(function (e) {
            if (e.which == 13) {
                $("#btnSendSms").click();
            }
        });

        $("#txtMobilePhone").keydown(function (e) {
            if (e.which == 13) {
                $("#btnSendSms").click();
            }
        });

        $("#txtCode").keydown(function (e) {
            if (e.which == 13) {
                $("#btnConfirm").click();
            }
        });
    },
    "StartTimer": function (countdownSecond, callbackFunction) {

        var time = countdownSecond;

        _interval = setInterval(function () {

            time--;

            $(".errorText").html("Kalan Zaman: " + time.toString() + " sn");

            if (time == 0) {
                clearInterval(_interval);
                callbackFunction();
            }

        }, 1000);
    },
    "SendSms": function (portalUserId, phoneNumber, contactId, callBackFunction) {
        //debugger;
        var jData = {};
        jData.portalUserId = portalUserId;
        jData.portalId = PassRememberHelper.PortalId;
        jData.phoneNumber = phoneNumber;
        jData.contactId = contactId

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/SendPasswordSMS",
            async: true,
            dataType: "json",
            contentType: "application/json;",
            type: "POST",
            data: jSonData,
            beforeSend: function () {
                PassRememberHelper.ShowLoading(ReturnMessage(PassRememberHelper.LanguageCode, "CM114"));
            },
            complete: function () {
                PassRememberHelper.CloseDialog();
            },
            success: function (data) {
                //debugger;
                if (data != null) {
                    //debugger;

                    callBackFunction(data);

                    return;
                }
                else {
                    $("#lblErrorMain").show();
                    $("#lblErrorMain").html(ReturnMessage(LoginHelper.LanguageCode, "M002") + "<br />SendSms");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                $("#lblErrorMain").show();
                $("#lblErrorMain").html(c + "<br />SendSms");
                return false;
            }
        });
    },
    "CheckPhoneNumberMatch": function (userName, mobilePhone, callBackFunction) {

        //debugger;
        var jData = {};
        jData.userName = userName;
        jData.phoneNumber = mobilePhone;
        jData.portalId = PassRememberHelper.PortalId;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/CheckPhoneNumberMatch",
            async: true,
            dataType: "json",
            contentType: "application/json;",
            type: "POST",
            data: jSonData,
            beforeSend: function () {
                PassRememberHelper.ShowLoading(ReturnMessage(PassRememberHelper.LanguageCode, "CM123"));
            },
            complete: function () {
                //PassRememberHelper.CloseDialog();
            },
            success: function (data) {
                //debugger;
                if (data != null) {
                    //debugger;

                    callBackFunction(data);

                    return;
                }
                else {
                    PassRememberHelper.ToastrShow("error", ReturnMessage(LoginHelper.LanguageCode, "M002"));

                    PassRememberHelper.CloseDialog();
                }
            },
            error: function (a, b, c) {

                PassRememberHelper.ToastrShow("error", ReturnMessage(LoginHelper.LanguageCode, c));

                PassRememberHelper.CloseDialog();
            }
        });
    },
    "ConfirmCode": function (code, token, callBackFunction) {
        //debugger;
        var jData = {};
        jData.token = token;
        jData.code = code;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/ConfirmPasswordCode",
            async: true,
            dataType: "json",
            contentType: "application/json;",
            type: "POST",
            data: jSonData,
            beforeSend: function () {
                PassRememberHelper.ShowLoading(ReturnMessage(PassRememberHelper.LanguageCode, "CM124"));
            },
            complete: function () {
                PassRememberHelper.CloseDialog();
            },
            success: function (data) {
                //debugger;
                if (data != null) {

                    callBackFunction(data);

                    return;
                }
                else {
                    $("#lblErrorMain").show();
                    $("#lblErrorMain").html(ReturnMessage(LoginHelper.LanguageCode, "M002") + "<br />ConfirmCode");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                $("#lblErrorMain").show();
                $("#lblErrorMain").html(c + "<br />ConfirmCode");
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

        PassRememberHelper.ShowDialog(html, title, "modal20", false);
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
    "SmsCode": "",
    "LanguageCode": "tr",
    "Token": ""
};

function passForgotController($scope, $sce) {

    $.mask.definitions['~'] = "[+-]";
    $("#txtMobilePhone").mask("+90-888-8888888");

    $(parent.window).scrollTop(0);

    var url = $.url(document.location);

    PassRememberHelper.PortalId = url.param("portalid");
    $scope.portalId = url.param("portalid");

    $scope.ContentMessages = LanguageMessages[PassRememberHelper.LanguageCode];

    $scope.showError = false;
    $scope.errorText = "";
    $scope.showSmsCode = false;
    $scope.buttonText = "ŞİFREMİ GÖNDER";

    $scope.SendSms = function () {

        var userName = $scope.userName;
        var mobilePhone = $scope.mobilePhone;

        if (userName == null || userName == "" || mobilePhone == null || mobilePhone == "") {

            $scope.showError = true;
            $scope.errorText = ReturnMessage(PassRememberHelper.LanguageCode, "CM120");

            return;
        }

        PassRememberHelper.CheckPhoneNumberMatch(userName, mobilePhone, function (e) {

            if (e.Success == true) {

                PassRememberHelper.SendSms(e.CrmId, mobilePhone, e.Result, function (k) {
                    $scope.$apply(function () {
                        if (k.Success == true) {

                            $scope.buttonText = "DOĞRULA";
                            $scope.showSmsCode = true;

                            PassRememberHelper.Token = k.Result;

                            $scope.showError = true;

                            PassRememberHelper.StartTimer(100, function () {

                                $scope.$apply(function () {
                                    $scope.showSmsCode = false;
                                    $scope.showError = false;
                                    $scope.buttonText = "ŞİFREMİ TEKRAR GÖNDER";
                                });

                                PassRememberHelper.Token = "";
                            });
                        }
                        else {
                            $scope.$apply(function () {
                                $scope.showError = true;
                                $scope.errorText = ReturnMessage(PassRememberHelper.LanguageCode, k.Result);
                            });
                        }
                    });
                });
            }
            else {
                PassRememberHelper.CloseDialog();

                $scope.$apply(function () {
                    $scope.showError = true;
                    $scope.errorText = ReturnMessage(PassRememberHelper.LanguageCode, "CM122");
                });
            }

        });

    };

    $scope.ConfirmCode = function () {

        var smsCode = $scope.smsCode;

        if (smsCode == null || smsCode == "") {
            $scope.showError = true;
            $scope.errorText = ReturnMessage(PassRememberHelper.LanguageCode, "CM121");

            return;
        }

        PassRememberHelper.ConfirmCode(smsCode, PassRememberHelper.Token, function (e) {

            if (e.Success == true) {
                document.location.href = "passwordChange.html?token=" + PassRememberHelper.Token;
            }
            else {
                PassRememberHelper.ToastrShow("error", ReturnMessage(PassRememberHelper.LanguageCode, e.Result));
            }

        });
    };

    $scope.TriggerButton = function () {

        if ($scope.showSmsCode == false) {
            $scope.SendSms();
        }
        else if ($scope.showSmsCode == true) {
            $scope.ConfirmCode();
        }
    };
}