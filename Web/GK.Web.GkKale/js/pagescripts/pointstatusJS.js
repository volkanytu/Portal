/// <reference path="../jquery/jquery.min.js" />
/// <reference path="../jquery/jquery.widget.min.js" />
/// <reference path="../jquery/purl.js" />
/// <reference path="GlobalVariables.js" />
/// <reference path="indexJS.js" />
/// <reference path="../jquery/jquery.session.js" />
/// <reference path="mainJS.js" />

var PointStatusHelper = {
    "OnLoad": function () {
        $(parent.window).scrollTop(0);

        PointStatusHelper.OnClickEvents();
        PointStatusHelper.OnChangeEvents();

        parent.IndexHelper.AutoResize("ifrmContent");
    },
    "OnClickEvents": function () {
    },
    "OnChangeEvents": function () {
    },
    "GetPointStatus": function (callBackFunction) {
        //debugger;
        var jData = {};
        jData.token = PointStatusHelper.Token;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/GetCubeStatusList",
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
                    $("#lblErrorMain").html("<h1 class='fg-yellow'><i class='icon-warning'></i></h1><br />" + ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />GetPointStatusHelper");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), c + "<br />GetPointStatusHelper");
                return false;
            }
        });
    },
    "UserId": "",
    "PortalId": "",
    "Token": "",
    "PointTableData": [
        { Name: "164 GMC", Point: 25 },
        { Name: "164 GNC", Point: 25 },
        { Name: "164 GR", Point: 25 },
        { Name: "164 M", Point: 25 },
        { Name: "164 BNE", Point: 75 },
        { Name: "164 BME", Point: 75 },
        { Name: "164 SNC", Point: 45 },
        { Name: "164 SM", Point: 45 },
        { Name: "164 OBS BE", Point: 45 },
        { Name: "164 KTB S", Point: 100 },
        { Name: "264 SN+SM", Point: 100 },
        { Name: "164 KTB SM", Point: 100 },
        { Name: "164 F", Point: 10 },
        { Name: "164 UY", Point: 15 },
        { Name: "264 KTBS+KTBSM", Point: 200 },
        { Name: "364", Point: 200 },
        { Name: "164 ASM", Point: 700 },
        { Name: "164 DBNE", Point: 125 },
        { Name: "164 CEC", Point: 900 },
        { Name: "164 CEC M", Point: 900 },
        { Name: "164 YGS SE", Point: 350 },
        { Name: "164 KİS", Point: 1350 },
    ]
};

function pointStatusController($scope) {

    $(parent.window).scrollTop(0);

    var url = $.url(parent.document.location);
    PointStatusHelper.Token = url.param("token");
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

                PointStatusHelper.UserId = e.ReturnObject.PortalUserId;
                PointStatusHelper.PortalId = e.ReturnObject.PortalId;

                $scope.userId = e.ReturnObject.PortalUserId;
                $scope.portalId = e.ReturnObject.PortalId;
            }
            else {
                return;
            }
        });
    });

    $scope.pointTableData = PointStatusHelper.PointTableData;

    PointStatusHelper.GetPointStatus(function (e) {

        $scope.$apply(function () {
            var a = JSON.stringify(e);

            if (e.Success == true) {

                $scope.pointStatus = e.ReturnObject;

                var indexOfCurrentUserStatus = $scope.pointStatus.map(function (x) { return x.UserId.Id; }).indexOf(PointStatusHelper.UserId);

                $scope.currentUserStatus = $scope.pointStatus[indexOfCurrentUserStatus];

                $scope.pointStatus[indexOfCurrentUserStatus].className = 'warning';

                for (var i = 0; i < ($scope.pointStatus.length > 10 ? 10 : $scope.pointStatus.length) ; i++) {
                    $scope.pointStatus[i].showRow = true;
                }

                //for (var i = indexOfCurrentUserStatus; i < (indexOfCurrentUserStatus + 1) ; i++) {
                //    $scope.pointStatus[i].showRow = true;
                //}

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

    $scope.ShowAll = function () {
        for (var i = 0; i < $scope.pointStatus.length; i++) {

            $scope.pointStatus[i].showRow = true;
        }

        setTimeout(function () {
            parent.IndexHelper.AutoResize("ifrmContent");
        }, 500);
    };
}

