/// <reference path="../jquery/jquery.min.js" />
/// <reference path="../jquery/jquery.widget.min.js" />
/// <reference path="../jquery/purl.js" />
/// <reference path="GlobalVariables.js" />
/// <reference path="../jquery/jquery.session.js" />
/// <reference path="indexJS - DO.js" />
/// <reference path="../jquery/jquery.session.js" />

var objectId;

var NpsHelper = {
    "OnLoad": function () {
        var url = $.url(document.location);
        objectId = url.param("objectid");

        NpsHelper.OnClickEvents();
        NpsHelper.OnChangeEvents();
        NpsHelper.KeyPressEvents();
    },
    "OnClickEvents": function () {

        $("#btnSave").click(function (e) {

            var suggest = $("#suggest option:selected").val();
            var suggestPoint = $("#suggestPoint option:selected").val();

            NpsHelper.SaveAnswer(suggest, suggestPoint, function (e) {

                if (e.Success == true) {
                    NpsHelper.ShowDialog("Katılımınız için teşekkür ederiz.", "Teşekkürler", null, true, function (e) {
                        document.location.href = "http://www.kalekilit.com.tr";
                    });
                }
                else {
                    NpsHelper.CloseDialog();
                    NpsHelper.ToastrShow("warning", e.Result, "Uyarı");
                }
            });

        });
    },
    "OnChangeEvents": function () {

    },
    "KeyPressEvents": function () {

    },
    "SaveAnswer": function (suggest, suggestPoint, callBackFunction) {

        //debugger;
        var jData = {};

        jData.npsSurveyId = objectId;
        jData.suggest = parseInt(suggest);
        jData.suggestPoint = parseInt(suggestPoint);

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/AnswerNpsSurvey",
            async: true,
            dataType: "json",
            contentType: "application/json;",
            type: "POST",
            data: jSonData,
            beforeSend: function () {
                NpsHelper.ShowLoading("Cevabınız kaydediliyor...");
            },
            complete: function () {

            },
            success: function (data) {
                //debugger;
                if (data != null) {

                    callBackFunction(data);
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
    "ShowLoading": function (message, title) {

        var html = "";
        html += "<div id='divLoadingMain' ng-show='showLoading' style='text-align:center;'>";
        html += "	<img src='images/loading.gif' width='48' />";
        html += "	<br />";
        html += "	<br />";
        html += "	<h4 style='text-align:center;'>" + message + "</h4>";
        html += "</div>";

        NpsHelper.ShowDialog(html, title, "modal20", false);
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

                NpsHelper.ShowLoading("Sayfa yükleniyor...");
            },
            complete: function () {
                NpsHelper.CloseDialog();
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
    }
};

function npsController($scope) {
    var url = $.url(document.location);
    baseURL = url.data.attr["base"];

    $scope.showUserInteraction = true;
    $scope.showErrorHeader = false;
    $scope.showError = false;
    $scope.showLoading = true;
    $scope.showMain = false;
    $scope.errorText = "";

    NpsHelper.GetPortalInfo(function (e) {
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
                $scope.errorText = ReturnMessage("tr", e.Result);
            }
        });
    });
}