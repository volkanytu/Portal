/// <reference path="mainJS.js" />
/// <reference path="../jquery-base.min.js" />

var GiftListHelper = {
    "OnLoad": function () {

    },
    "GetGiftCategoryList": function (callBackFunction) {

        //debugger;
        var jData = {};
        jData.token = GiftListHelper.Token;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/GetGiftCategoryList",
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
                    $("#lblErrorMain").html("<h1 class='fg-yellow'><i class='icon-warning'></i></h1><br />" + ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />GetGiftCategoryList");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), "Hata:" + c + "<br />GetGiftCategoryList");
                return false;
            }
        });
    },

    "GetGiftList": function (categoryId, sortType, callBackFunction) {
        var jData = {};
        jData.token = GiftListHelper.Token;
        jData.categoryId = categoryId;
        jData.sortType = sortType;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/GetGiftList",
            async: true,
            dataType: "json",
            contentType: "application/json;",
            type: "POST",
            data: jSonData,
            beforeSend: function () {
                parent.IndexHelper.ShowLoading("Hediye listesi çekiliyor...");
            },
            complete: function () {
                parent.IndexHelper.AutoResize("ifrmContent");
                parent.IndexHelper.CloseDialog();
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
                    $("#lblErrorMain").html("<h1 class='fg-yellow'><i class='icon-warning'></i></h1><br />" + ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />GetGiftList");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), c + "<br / >GetGiftList");
                return false;
            }
        });
    },
    "UserId": "",
    "PortalId": "",
    "Token": "",
};

function giftListController($scope) {

    $(parent.window).scrollTop(0);

    var parentUrl = $.url(parent.document.location);
    var url = $.url(document.location);

    GiftListHelper.Token = parentUrl.param("token");
    $scope.token = parentUrl.param("token");

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

                GiftListHelper.UserId = e.ReturnObject.PortalUserId;
                GiftListHelper.PortalId = e.ReturnObject.PortalId;

                $scope.userId = e.ReturnObject.PortalUserId;
                $scope.portalId = e.ReturnObject.PortalId;
            }
            else {
                return;
            }
        });
    });

    //$scope.selectedCategory = null;
    //$scope.selectedSortType = null;

    $scope.sortTypes = [
        {
            "Value": "asc",
            "Label": "Puan Artan"
        },
        {
            "Value": "DESC",
            "Label": "Puan Azalan"
        }
    ];

    GiftListHelper.GetGiftCategoryList(function (e) {

        $scope.$apply(function () {
            var a = JSON.stringify(e);

            if (e.Success == true) {

                $scope.categories = e.ReturnObject;

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

    $scope.GetGiftList = function (categoryId, sortType) {

        GiftListHelper.GetGiftList(categoryId, sortType, function (e) {

            $scope.$apply(function () {
                var b = JSON.stringify(e);

                if (e.Success == true) {

                    $scope.gifts = e.ReturnObject;

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
    };

    $scope.CategoryChange = function () {

        var category = $scope.selectedCategory;

        var categoryId = null;
        var sortType = $scope.selectedSortType;

        if (sortType == null || sortType == undefined || category == null || category == undefined) {

            parent.IndexHelper.ShowNotify("Hediye kategorisi ve sıralama türü seçiniz");

            return;
        };

        categoryId = category.Id;

        $scope.GetGiftList(categoryId, sortType);
    };

    $scope.EditGift = function (giftId) {

        var categoryName = encodeURIComponent($scope.selectedCategory.Name);

        $("#ifrmContent", parent.document).attr("src", "hediyelerDetay.html?objectid=" + giftId + "&name=" + categoryName);

    };
}