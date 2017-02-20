/// <reference path="../jquery/jquery.min.js" />
/// <reference path="../jquery/jquery.widget.min.js" />
/// <reference path="../jquery/purl.js" />
/// <reference path="GlobalVariables.js" />
/// <reference path="indexJS.js" />
/// <reference path="mainJS.js" />

var RegisterHelper = {
    "OnLoad": function () {
        $(parent.window).scrollTop(0);

        $('#txtMobilePhone').mask("999-9999999");
        //$('#txtWorkPhone').mask("999-9999999");
    },
    "OnClickEvents": function () {
    },
    "OnChangeEvents": function () {
    },
    "Register": function (contactInfo, callBackFunction) {

        var jData = {};

        jData.contact = {};
        jData.contact = contactInfo;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/RegisterUser",
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

                    return;
                }
            },
            error: function (a, b, c) {
                debugger;
                alert("Error:" + c);
                return false;
            }
        });

    },
    "GetCitites": function (callBackFunction) {
        //debugger;
        var jData = {};

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/GetCities",
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

                    return;
                }
                else {
                    $("#lblErrorMain").show();
                    $("#lblErrorMain").html("<h1 class='fg-yellow'><i class='icon-warning'></i></h1><br />" + ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />GetProfileInfo");

                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), c + "<br />GetProfileInfo");
                return false;
            }
        });
    },
    "GetTowns": function (cityId, callBackFunction) {
        //debugger;
        var jData = {};

        jData.cityId = cityId;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/GetTowns",
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

                    return;
                }
                else {
                    $("#lblErrorMain").show();
                    $("#lblErrorMain").html("<h1 class='fg-yellow'><i class='icon-warning'></i></h1><br />" + ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />GetProfileInfo");

                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), c + "<br />GetProfileInfo");
                return false;
            }
        });
    },
    "GetDateString": function (controlDate) {
        var adate = new Date(parseInt(controlDate.replace("/Date(", "").replace(")/", ""), 10));
        var day = adate.getDate().toString().length == 1 ? "0" + adate.getDate().toString() : adate.getDate().toString();
        var month = (adate.getMonth() + 1).toString().length == 1 ? "0" + (adate.getMonth() + 1).toString() : (adate.getMonth() + 1).toString();
        var accessTime = day + "." + month + "." + adate.getFullYear();
        return accessTime;
    },
    "ToDate": function (strDate) {
        var dateParts = strDate.split(".");

        var date = new Date(dateParts[2], (dateParts[1] - 1), dateParts[0]);

        return "/Date(" + date.getTime() + ")/";
    },
    "PortalId": "",
    "Token": ""
};

function registerController($scope, $sce) {

    $.mask.definitions['~'] = "[+-]";
    $("#txtMobilePhone").mask("+90-888-8888888");
    //$("#txtWorkPhone").mask("+90-888-8888888");
    //$("#txtBirthDate").mask("88.88.8888");

    $(window).scrollTop(0);

    $scope.loadingMessage = "Form hazırlanıyor...";
    $scope.showErrorHeader = false;
    $scope.showLoading = true;
    $scope.showMain = false;
    $scope.errorText = "";

    $scope.userInfo = {};
    $scope.userInfo.ContactInfo = {};

    $scope.GetTowns = function (cityId) {

        RegisterHelper.GetTowns(cityId, function (e) {

            $scope.$apply(function () {

                $scope.towns = e;
            });
        });
    };

    RegisterHelper.GetCitites(function (e) {

        $scope.$apply(function () {

            $scope.cities = e;

            $scope.showLoading = false;
            $scope.showMain = true;
        });

    });

    $scope.Register = function () {

        if ($scope.userInfo.ContactInfo.BirthDateStr != null && $scope.userInfo.ContactInfo.BirthDateStr != undefined
            && $scope.userInfo.ContactInfo.BirthDateStr != "") {
            $scope.userInfo.ContactInfo.BirthDate = RegisterHelper.ToDate($scope.userInfo.ContactInfo.BirthDateStr);
        }
        else {
            $scope.userInfo.ContactInfo.BirthDate = null;
        }

        $scope.showErrorHeader = false;
        $scope.showLoading = true;
        $scope.loadingMessage = "Form kaydediliyor...";
        $scope.showMain = false;
        $scope.errorText = "";

        RegisterHelper.Register($scope.userInfo.ContactInfo, function (e) {

            if (e.Success == true) {

                parent.LoginHelper.ShowNotify("Kayıt işlemi başarılı bir şekilde yapılmıştır.", "success");

                parent.LoginHelper.CloseDialog();
            }
            else {
                $scope.$apply(function () {
                    $scope.showErrorHeader = true;
                    $scope.showLoading = false;
                    $scope.showMain = true;
                    $scope.errorText = e.Result;
                    parent.LoginHelper.ShowNotify(e.Result, "error");
                });

                $(window).scrollTop(0);
            }
        });
    };

    $scope.CityChange = function () {

        var city = $scope.userInfo.ContactInfo.CityId

        $scope.GetTowns(city.Id);
    };
}