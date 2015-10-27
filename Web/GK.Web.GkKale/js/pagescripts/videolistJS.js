/// <reference path="../jquery/jquery.min.js" />
/// <reference path="../jquery/jquery.widget.min.js" />
/// <reference path="../jquery/purl.js" />
/// <reference path="GlobalVariables.js" />
/// <reference path="indexJS.js" />

var VideoListHelper = {
    "OnLoad": function () {
        $(parent.window).scrollTop(0);

        VideoListHelper.OnClickEvents();
        VideoListHelper.OnChangeEvents();

        parent.IndexHelper.AutoResize("ifrmContent");

    },
    "OnClickEvents": function () {
    },
    "OnChangeEvents": function () {
    },
    "GetVideoCategoryInfo": function (callBackFunction) {
        //debugger;
        var jData = {};
        jData.token = VideoListHelper.Token;
        jData.categoryId = VideoListHelper.CategoryId;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/GetVideoCategoryInfo",
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
                    $("#lblErrorMain").html("<h1 class='fg-yellow'><i class='icon-warning'></i></h1><br />" + ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />GetVideoCategoryInfo");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), c + "<br />GetVideoCategoryInfo");

                return false;
            }
        });
    },
    "GetVideos": function (start, end, isFirst, callBackFunction) {
        //debugger;
        var jData = {};
        jData.token = VideoListHelper.Token;
        jData.categoryId = VideoListHelper.CategoryId;
        jData.start = start;
        jData.end = end;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/GetVideoList",
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
                    $("#lblErrorMain").html("<h1 class='fg-yellow'><i class='icon-warning'></i></h1><br />" + ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />GetVideos");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), c + "<br />GetVideos");
                return false;
            }
        });
    },
    "LogVideo": function (videoId) {
        var jData = {};
        jData.token = VideoListHelper.Token;
        jData.videoId = videoId;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/LogVideo",
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
    "PageRecord": 10,
    "CategoryId": ""
};

function videoListController($scope) {

    $(parent.window).scrollTop(0);

    var parentUrl = $.url(parent.document.location);
    var url = $.url(document.location);

    VideoListHelper.Token = parentUrl.param("token");
    $scope.token = parentUrl.param("token");

    VideoListHelper.CategoryId = url.param("objectid");
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

                VideoListHelper.UserId = e.ReturnObject.PortalUserId;
                VideoListHelper.PortalId = e.ReturnObject.PortalId;

                $scope.userId = e.ReturnObject.PortalUserId;
                $scope.portalId = e.ReturnObject.PortalId;
            }
            else {
                return;
            }
        });
    });

    $scope.GetVideos = function () {
        VideoListHelper.GetVideos(0, 20, true, function (e) {

            $scope.$apply(function () {
                var a = JSON.stringify(e);

                if (e.Success == true) {

                    $scope.videos = e.ReturnObject;

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

    VideoListHelper.GetVideoCategoryInfo(function (e) {

        $scope.$apply(function () {
            var a = JSON.stringify(e);

            if (e.Success == true) {

                $scope.categories = e.ReturnObject;
                $scope.categoryImageUrl = attachmentUrl + e.ReturnObject.LogicalName;

                $scope.GetVideos();
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

    $scope.EditVideo = function (id, name) {

        $("#ifrmContent", parent.document).attr("src", "videoPlayer.html?objectid=" + id + "&portalid=" + VideoListHelper.PortalId + "&userid=" + VideoListHelper.UserId + "&categoryid=" + VideoListHelper.CategoryId + "&categoryname=" + encodeURIComponent($scope.categoryName));

    };
}