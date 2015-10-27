/// <reference path="../jquery/jquery.min.js" />
/// <reference path="../jquery/jquery.widget.min.js" />
/// <reference path="../jquery/purl.js" />
/// <reference path="GlobalVariables.js" />
/// <reference path="indexJS.js" />

var PageContentHelper = {
    "OnLoad": function () {
        $(parent.window).scrollTop(0);

        if (PageContentHelper.PageNo == 0 || PageContentHelper.PageNo == "" || PageContentHelper.PageNo == undefined) {
            $("#lblErrorMain").show();
            $("#lblErrorMain").html("<h1 class='fg-yellow'><i class='icon-warning'></i></h1><br />" + "İlgili sayfa numarası bulunamadı.");

            return;
        }

        PageContentHelper.OnClickEvents();
        PageContentHelper.OnChangeEvents();
    },
    "OnClickEvents": function () {
    },
    "OnChangeEvents": function () {
    },
    "GetPageContent": function (pageNo, callBakcFunction) {
        //debugger;
        var jData = {};
        jData.token = PageContentHelper.Token;
        jData.pageNo = pageNo;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/GetPageContent",
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

                    callBakcFunction(data);

                    return;
                }
                else {
                    $("#lblErrorMain").show();
                    $("#lblErrorMain").html("<h1 class='fg-yellow'><i class='icon-warning'></i></h1><br />" + ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />GetPageContent");

                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), c + "<br />GetPageContent");
                return false;
            }
        });
    },
    "UserId": "",
    "PortalId": "",
    "PageNo": 0,
    "Token": ""
};

function pageContentController($scope, $sce) {

    $(parent.window).scrollTop(0);

    var parentUrl = $.url(parent.document.location);
    var url = $.url(document.location);

    PageContentHelper.Token = parentUrl.param("token");
    $scope.token = parentUrl.param("token");

    PageContentHelper.PageNo = url.param("pageno");
    $scope.pageNo = url.param("pageno");

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

                PageContentHelper.UserId = e.ReturnObject.PortalUserId;
                PageContentHelper.PortalId = e.ReturnObject.PortalId;

                $scope.userId = e.ReturnObject.PortalUserId;
                $scope.portalId = e.ReturnObject.PortalId;
            }
            else {
                return;
            }
        });
    });

    PageContentHelper.GetPageContent($scope.pageNo, function (e) {

        $scope.$apply(function () {
            var a = JSON.stringify(e);

            if (e.Success == true) {

                $scope.pageContent = $sce.trustAsHtml(e.Result);

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