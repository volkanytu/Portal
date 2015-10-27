/// <reference path="../jquery/jquery.min.js" />
/// <reference path="../jquery/jquery.widget.min.js" />
/// <reference path="../jquery/purl.js" />
/// <reference path="GlobalVariables.js" />
/// <reference path="indexJS.js" />
/// <reference path="../jquery/jquery.session.js" />

var EducationListHelper = {
    "OnLoad": function () {
        $(parent.window).scrollTop(0);

        EducationListHelper.OnClickEvents();
        EducationListHelper.OnChangeEvents();

        parent.IndexHelper.AutoResize("ifrmContent");

    },
    "OnClickEvents": function () {
    },
    "OnChangeEvents": function () {
    },
    "GetEducationCategoryInfo": function (callBackFunction) {
        //debugger;
        var jData = {};
        jData.token = EducationListHelper.Token;
        jData.categoryId = EducationListHelper.CategoryId;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/GetEducationCategoryInfo",
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
                    data = JSON.parse(data);

                    callBackFunction(data);

                    return;
                }
                else {
                    $("#lblErrorMain").show();
                    $("#lblErrorMain").html("<h1 class='fg-yellow'><i class='icon-warning'></i></h1><br />" + ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />GetEducations");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), c + "<br />GetEducations");

                return false;
            }
        });
    },
    "GetEducations": function (start, end, isFirst, callBackFunction) {
        //debugger;
        var jData = {};
        jData.token = EducationListHelper.Token;
        jData.categoryId = EducationListHelper.CategoryId;
        jData.start = start;
        jData.end = end;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/GetEducationList",
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
                    $("#lblErrorMain").html("<h1 class='fg-yellow'><i class='icon-warning'></i></h1><br />" + ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />GetEducations");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), c + "<br />GetEducations");

                return false;
            }
        });
    },
    "LogEducation": function (educationId) {
        var jData = {};
        jData.token = EducationListHelper.Token;
        jData.educationId = educationId;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/LogEducation",
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
                    if (data.Success) {

                    }
                    else {
                        return false;
                    }
                }
                else {
                    return false;
                }
            },
            error: function (a, b, c) {
                return false;
            }
        });
    },
    "UserId": "",
    "PortalId": "",
    "Token": "",
    "PageRecord": 5,
    "CategoryId": ""
};

function educationListController($scope) {

    $(parent.window).scrollTop(0);

    var parentUrl = $.url(parent.document.location);
    var url = $.url(document.location);

    EducationListHelper.Token = parentUrl.param("token");
    $scope.token = parentUrl.param("token");

    EducationListHelper.CategoryId = url.param("objectid");
    $scope.categoryId = url.param("objectid");
    $scope.categoryName = url.param("name");

    parent.IndexHelper.AutoResize("ifrmContent");

    $scope.attachmentUrl = attachmentUrl;
    $scope.ContentMessages = LanguageMessages[parent.IndexHelper.LanguageCode];

    $scope.showErrorHeader = false;
    $scope.showLoading = true;
    $scope.showMain = false;
    $scope.errorText = "";

    $scope.noRecord = true;

    parent.IndexHelper.CheckSession(true, $scope.token, function (e) {
        $scope.$apply(function () {
            if (e.Success == true) {

                EducationListHelper.UserId = e.ReturnObject.PortalUserId;
                EducationListHelper.PortalId = e.ReturnObject.PortalId;

                $scope.userId = e.ReturnObject.PortalUserId;
                $scope.portalId = e.ReturnObject.PortalId;
            }
            else {
                return;
            }
        });
    });

    $scope.GetEducations = function () {
        EducationListHelper.GetEducations(0, 20, true, function (e) {

            $scope.$apply(function () {
                var a = JSON.stringify(e);

                if (e.Success == true) {

                    $scope.educations = e.ReturnObject;

                    $scope.showErrorHeader = false;
                    $scope.showLoading = false;
                    $scope.showMain = true;

                    //if (data.RecordCount != null && data.RecordCount > EducationListHelper.PageRecord) {
                    //    $("#divPaging").show();

                    //    var pageCount = Math.ceil(data.RecordCount / EducationListHelper.PageRecord);

                    //    if (isFirst == true) {
                    //        $("#divPaging").pagination({
                    //            items: data.RecordCount,
                    //            itemsOnPage: EducationListHelper.PageRecord,
                    //            onPageClick: function (pageNumber, event) {

                    //                var start = ((pageNumber - 1) * EducationListHelper.PageRecord + 1);
                    //                var end = (pageNumber * EducationListHelper.PageRecord);

                    //                EducationListHelper.GetEducations(start, end);
                    //            },
                    //        });
                    //    }
                    //}
                }
                else {
                    $scope.showErrorHeader = true;
                    $scope.showLoading = false;
                    $scope.showMain = false;
                    $scope.errorText = ReturnMessage(parent.IndexHelper.LanguageCode, e.Result);
                }

                setTimeout(function () {
                    parent.IndexHelper.AutoResize("ifrmContent");
                }, 500);
            });

        });

    };

    EducationListHelper.GetEducationCategoryInfo(function (e) {

        $scope.$apply(function () {
            var a = JSON.stringify(e);

            if (e.Success == true) {

                $scope.categories = e.ReturnObject;
                $scope.categoryImageUrl = attachmentUrl + e.ReturnObject.LogicalName;

                $scope.GetEducations();
            }
            else {
                $scope.showErrorHeader = true;
                $scope.showLoading = false;
                $scope.showMain = false;
                $scope.errorText = ReturnMessage(parent.IndexHelper.LanguageCode, e.Result);
            }

            setTimeout(function () {
                parent.IndexHelper.AutoResize("ifrmContent");
            }, 500);
        });

    });

    $scope.EditEducation = function (id, name) {

        $("#ifrmContent", parent.document).attr("src", "egitimlerIcerikDetay.html?objectid=" + id + "&portalid=" + EducationListHelper.PortalId + "&userid=" + EducationListHelper.UserId + "&categoryid=" + EducationListHelper.CategoryId + "&categoryname=" + encodeURIComponent($scope.categoryName));

    };
}