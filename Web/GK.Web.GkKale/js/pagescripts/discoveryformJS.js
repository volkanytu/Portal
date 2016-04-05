/// <reference path="../jquery/jquery.min.js" />
/// <reference path="../jquery/jquery.widget.min.js" />
/// <reference path="../jquery/purl.js" />
/// <reference path="GlobalVariables.js" />
/// <reference path="indexJS.js" />
/// <reference path="mainJS.js" />

var DiscoveryFormHelper = {
    "OnLoad": function () {
        $(parent.window).scrollTop(0);

        DiscoveryFormHelper.OnClickEvents();
        DiscoveryFormHelper.OnChangeEvents();

        parent.IndexHelper.AutoResize("ifrmContent");

        $("#txtPhoneNumber").mask("+90-888-8888888");
    },
    "OnClickEvents": function () {
    },
    "OnChangeEvents": function () {
    },
    "SaveDiscoveryForm": function (data, callBackFunction) {
        //debugger;
        var jData = {};

        var visitDate = $('#txtDate').datepicker("getDate");

        if (visitDate != null) {
            data.VisitDate = "/Date(" + visitDate.getTime() + ")/";
        }

        data.UserId = {};
        data.UserId.Id = DiscoveryFormHelper.UserId;
        data.UserId.Name = parent.IndexHelper.UserName;
        data.UserId.LogicalName = "new_user";

        data.PortalId = {};
        data.PortalId.Id = DiscoveryFormHelper.PortalId;
        data.PortalId.LogicalName = "new_portal";

        jData.discForm = {};
        jData.discForm = data;

        jData.token = DiscoveryFormHelper.Token;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/SaveDiscoveryForm",
            async: true,
            dataType: "json",
            contentType: "application/json;",
            type: "POST",
            data: jSonData,
            beforeSend: function () {
                parent.IndexHelper.ShowLoading("Form kaydediliyor...", null);
            },
            complete: function () {
                parent.IndexHelper.AutoResize("ifrmContent");
                parent.IndexHelper.CloseDialog();
            },
            success: function (data) {
                //debugger;
                if (data != null) {

                    callBackFunction(data);

                    return;
                }
                else {
                    parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(parent.IndexHelper.LanguageCode, "M059"), ReturnMessage(parent.IndexHelper.LanguageCode, "M002") + "<br />SaveDiscoveryForm");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(parent.IndexHelper.LanguageCode, "M059"), c + "<br />SaveDiscoveryForm");
                return false;
            }
        });

    },
    "GetUserDiscoveryFormList": function (callBackFunction) {
        //debugger;
        var jData = {};

        jData.token = DiscoveryFormHelper.Token;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/GetUserDiscoveryFormList",
            async: true,
            dataType: "json",
            contentType: "application/json;",
            type: "POST",
            data: jSonData,
            beforeSend: function () {
                parent.IndexHelper.ShowLoading("Mevcut formlar çekiliyor...", null);
            },
            complete: function () {
                parent.IndexHelper.AutoResize("ifrmContent");
                parent.IndexHelper.CloseDialog();
            },
            success: function (data) {
                //debugger;
                if (data != null) {

                    callBackFunction(data);

                    return;
                }
                else {
                    parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(parent.IndexHelper.LanguageCode, "M059"), ReturnMessage(parent.IndexHelper.LanguageCode, "M002") + "<br />GetUserDiscoveryFormList");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(parent.IndexHelper.LanguageCode, "M059"), c + "<br />GetUserDiscoveryFormList");
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
                    $("#lblErrorMain").html("<h1 class='fg-yellow'><i class='icon-warning'></i></h1><br />" + ReturnMessage(parent.IndexHelper.LanguageCode, "M002") + "<br />GetProfileInfo");

                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(parent.IndexHelper.LanguageCode, "M059"), c + "<br />GetProfileInfo");
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
                    $("#lblErrorMain").html("<h1 class='fg-yellow'><i class='icon-warning'></i></h1><br />" + ReturnMessage(parent.IndexHelper.LanguageCode, "M002") + "<br />GetProfileInfo");

                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(parent.IndexHelper.LanguageCode, "M059"), c + "<br />GetProfileInfo");
                return false;
            }
        });
    },
    "UserId": "",
    "PortalId": "",
    "Token": ""
};

function discoveryController($scope, $sce) {

    $.mask.definitions['~'] = "[+-]";
    $("#txtPhoneNumber").mask("+90-888-8888888");

    $('#txtDate').datepicker({
        language: 'tr',
        autoclose: true
    });

    $(parent.window).scrollTop(0);

    var parentUrl = $.url(parent.document.location);
    var url = $.url(document.location);

    DiscoveryFormHelper.Token = parentUrl.param("token");
    $scope.token = parentUrl.param("token");

    parent.IndexHelper.AutoResize("ifrmContent");

    $scope.attachmentUrl = attachmentUrl;
    $scope.ContentMessages = LanguageMessages[parent.IndexHelper.LanguageCode];

    $scope.showErrorHeader = false;
    $scope.showLoading = true;
    $scope.showMain = false;
    $scope.errorText = "";

    $scope.showOldForms = false;

    $scope.discoveryForm = null;


    parent.IndexHelper.CheckSession(true, $scope.token, function (e) {
        $scope.$apply(function () {
            if (e.Success == true) {

                DiscoveryFormHelper.UserId = e.ReturnObject.PortalUserId;
                DiscoveryFormHelper.PortalId = e.ReturnObject.PortalId;

                $scope.userId = e.ReturnObject.PortalUserId;
                $scope.portalId = e.ReturnObject.PortalId;
            }
            else {
                return;
            }
        });
    });

    $scope.GetTowns = function (cityId) {

        DiscoveryFormHelper.GetTowns(cityId, function (e) {

            $scope.$apply(function () {

                $scope.towns = e;
            });
        });
    };

    DiscoveryFormHelper.GetCitites(function (e) {

        $scope.$apply(function () {

            $scope.cities = e;
        });

    });

    DiscoveryFormHelper.GetUserDiscoveryFormList(function (e) {

        if (e.Success == true) {

            $scope.$apply(function () {

                $scope.oldForms = e.ReturnObject;
                $scope.showOldForms = true;
            });
        }

    });

    $scope.CityChange = function () {

        var city = $scope.discoveryForm.CityId

        $scope.GetTowns(city.Id);
    };

    $scope.Send = function () {

        if ($scope.discoveryForm == null) {
            parent.IndexHelper.ToastrShow("warning", "Lütfen alanları doldurunuz.", "Uyarı");

            return;
        }

        DiscoveryFormHelper.SaveDiscoveryForm($scope.discoveryForm, function (e) {

            if (e.Success == true) {
                parent.IndexHelper.ToastrShow("success", "Form kaydedildi.", "Başarılı.");
                document.location = "duvarYazisi.html";
            }
            else {
                parent.IndexHelper.ToastrShow("error", e.Result, "Hata");
            }
        });
    };

    $scope.Edit = function (id) {

        var elementPos = $scope.oldForms.map(function (x) { return x.Id; }).indexOf(id);
        var formInfo = $scope.oldForms[elementPos];

        var html = "";
        html += "<div class='popupContainer'>";
        html += "<div class='popup popupFriend'>";
        html += " <div class='popupBody'>";
        html += "<div class='popupInfo clearfix' style='width:100%;height:390px;'>";
        html += "	<div class='popupTitle'><h2>Keşif Form Bilgisi</h2></div>";
        html += "	<ul>";
        html += "		<li><div class='clmn txt'><span>Ad</span></div><div class='clmn dtl'><span>" + formInfo.FirstName + "</span></div></li>";
        html += "		<li><div class='clmn txt'><span>Soyad Telefonu</span></div><div class='clmn dtl'><span>" + formInfo.LastName + "</span></div></li>";
        html += "		<li><div class='clmn txt'><span>İl</span></div><div class='clmn dtl'><span>" + formInfo.CityId.Name + "</span></div></li>";
        html += "		<li><div class='clmn txt'><span>İlçe</span></div><div class='clmn dtl'><span>" + formInfo.TownId.Name + "</span></div></li>";
        html += "		<li><div class='clmn txt'><span>Telefon</span></div><div class='clmn dtl'><span>" + formInfo.PhoneNumber + "</span></div></li>";
        html += "		<li><div class='clmn txt'><span>Email</span></div><div class='clmn dtl'><span>" + formInfo.Email + "</span></div></li>";
        html += "		<li><div class='clmn txt'><span>Oluşturma Tarihi</span></div><div class='clmn dtl'><span>" + formInfo.CreatedOnString + "</span></div></li>";
        html += "		<li><div class='clmn txt'><span>Durum</span></div><div class='clmn dtl'><span>" + formInfo.Status.Value + "</span></div></li>";
        html += "	</ul>";
        html += "</div>";
        html += "</div>";
        html += "</div>";
        html += "</div>";
        parent.IndexHelper.ShowDialog(html, null, null, true);        

    }
}