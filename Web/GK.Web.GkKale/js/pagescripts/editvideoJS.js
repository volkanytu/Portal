/// <reference path="../jquery/jquery.min.js" />
/// <reference path="../jquery/jquery.widget.min.js" />
/// <reference path="../jquery/purl.js" />
/// <reference path="GlobalVariables.js" />
/// <reference path="indexJS.js" />
/// <reference path="mainJS.js" />

var VideoHelper = {
    "OnLoad": function () {
        $(parent.window).scrollTop(0);

        parent.IndexHelper.AutoResize("ifrmContent");

        VideoHelper.OnClickEvents();
        VideoHelper.OnChangeEvents();
    },
    "OnClickEvents": function () {
    },
    "OnChangeEvents": function () {
    },
    "GetVideoInfo": function (callBackFunction) {
        //debugger;
        var jData = {};
        jData.token = VideoHelper.Token;
        jData.videoId = VideoHelper.ObjectId;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/GetVideoInfo",
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
                    $("#lblErrorMain").html("<h1 class='fg-yellow'><i class='icon-warning'></i></h1><br />" + ReturnMessage(IndexHelper.LanguageCode, "M002") + "-GetVideoInfo");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), "Hata:" + c + "-GetVideoInfo");
                return false;
            }
        });
    },
    "LogVideo": function (videoId) {
        var jData = {};
        jData.token = VideoHelper.Token;
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
    "SendComment": function (description, entityId, callBackFunction) {

        var jData = {};
        jData.token = VideoHelper.Token;
        jData.comment = {};

        jData.comment.PortalUser = {};
        jData.comment.PortalUser.Id = VideoHelper.UserId;
        jData.comment.PortalUser.Name = parent.IndexHelper.UserName;
        jData.comment.PortalUser.LogicalName = "new_user";

        jData.comment.Video = {};
        jData.comment.Video.Id = entityId;
        jData.comment.Video.LogicalName = "new_video";

        jData.comment.Description = description;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/SaveComment",
            async: true,
            dataType: "json",
            contentType: "application/json;",
            type: "POST",
            data: jSonData,
            beforeSend: function () {
                parent.IndexHelper.ShowLoading("Yorum kaydediliyor...", null);
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
                    parent.IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M059"), ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />SendComment", "red", "white");
                    parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />SendComment");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), c + "<br />SendComment");
                return false;
            }
        });
    },
    "GetEntityComments": function (token, entityId, entityName, start, end, callBackFunction) {
        var jData = {};
        jData.token = token;
        jData.entityId = entityId;
        jData.entityName = entityName;
        jData.start = start;
        jData.end = end;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/GetEntityComments",
            async: true,
            dataType: "json",
            contentType: "application/json;",
            type: "POST",
            data: jSonData,
            beforeSend: function () {

            },
            complete: function () {
                $("div[type='graffitiContainer'][id='" + graffitiId + "']").find("div[type='morecomments']").find("a").parent().find("img").hide();
            },
            success: function (data) {
                //debugger;
                if (data != null) {
                    data = JSON.parse(data);

                    callBackFunction(data);
                    return;
                }
                else {
                    parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />GetGraffitiComments");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), c + "<br />GetGraffitiComments");
                return false;
            }
        });
    },
    "UserId": "",
    "PortalId": "",
    "ObjectId": "",
    "CategoryId": "",
    "Token": ""
};

function editVideoController($scope, $sce) {
    $(parent.window).scrollTop(0);

    var parentUrl = $.url(parent.document.location);
    var url = $.url(document.location);

    VideoHelper.Token = parentUrl.param("token");
    $scope.token = parentUrl.param("token");

    VideoHelper.ObjectId = url.param("objectid");
    $scope.videoId = url.param("objectid");

    VideoHelper.CategoryId = url.param("categoryid");
    $scope.categoryId = url.param("categoryid");
    $scope.categoryName = url.param("categoryname");

    parent.IndexHelper.AutoResize("ifrmContent");

    $scope.attachmentUrl = attachmentUrl;
    $scope.ContentMessages = LanguageMessages[parent.IndexHelper.LanguageCode];

    $scope.showErrorHeader = false;
    $scope.showLoading = true;
    $scope.showMain = false;
    $scope.errorText = "";

    $scope.noDocRecord = false;
    $scope.noCommentRecord = false;

    parent.IndexHelper.CheckSession(true, $scope.token, function (e) {
        $scope.$apply(function () {
            if (e.Success == true) {

                VideoHelper.UserId = e.ReturnObject.PortalUserId;
                VideoHelper.PortalId = e.ReturnObject.PortalId;

                $scope.userId = e.ReturnObject.PortalUserId;
                $scope.portalId = e.ReturnObject.PortalId;
            }
            else {
                return;
            }
        });
    });

    VideoHelper.GetVideoInfo(function (e) {

        $scope.$apply(function () {
            var a = JSON.stringify(e);

            if (e.Success == true) {

                $scope.video = e.ReturnObject;

                if (e.ReturnObject.CommentList == null || e.ReturnObject.CommentList.length == 0) {
                    $scope.noCommentRecord = true;
                }
                else {
                    $scope.noCommentRecord = false;
                }

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

    $scope.BackToCategory = function () {

        $("#ifrmContent", parent.document).attr("src", "videolarDetay.html?objectid=" + $scope.categoryId + "&name=" + $scope.categoryName);
    };

    $scope.GetEntityComments = function () {

        VideoHelper.GetEntityComments(VideoHelper.Token, VideoHelper.ObjectId, "new_video", 0, 20, function (e) {

            $scope.$apply(function () {

                if (e.Success == true) {
                    $scope.video.CommentList = e.ReturnObject;

                    setTimeout(function () {
                        parent.IndexHelper.AutoResize("ifrmContent");
                    }, 500);
                }

            });
        });
    };

    $scope.sendComment = function (videoId, description) {
        if (description == null || description == "" || description == undefined || description == "undefined") {
            parent.IndexHelper.ToastrShow("warning", ReturnMessage(parent.IndexHelper.LanguageCode, "M087"), "Uyarı");
            return;
        }

        VideoHelper.SendComment(description, videoId, function (e) {
            $scope.$apply(function () {

                if (e.Success == true) {

                    $scope.noCommentRecord = false;

                    parent.IndexHelper.ToastrShow("success", ReturnMessage(parent.IndexHelper.LanguageCode, e.Result), "Başarıılı");

                    $scope.video.commentText = null;
                    $scope.GetEntityComments();
                }
                else {
                    parent.IndexHelper.ToastrShow("error", ReturnMessage(parent.IndexHelper.LanguageCode, e.Result), "Hata");
                }
            });
        });
    };

    $scope.OpenVideo = function () {

        var id = $scope.video.Id;
        var videoPath = $scope.video.VideoPath;
        var name = $scope.video.Name;
        var youtubeUrl = $scope.video.YouTubeUrl;

        VideoHelper.LogVideo(id);

        //parent.IndexHelper.ShowDialog("<iframe src='popupVideolar.html' style='width:100%;height:370px;' />", null, null, true);

        if (youtubeUrl == null || youtubeUrl == "" || youtubeUrl == undefined) {
            parent.IndexHelper.ShowDialog("<video height='300' style='width:100%;' controls><source src='" + attachmentUrl + videoPath + "' type='video/mp4;' />Tarayıcınız video tagini desteklemiyor.</video>", name, null, true);
        }
        else {
            parent.IndexHelper.ShowDialog("<iframe width='100%' height='300' style='height:300px; width:100%;' src='https://www.youtube.com/embed/" + youtubeUrl + "' frameborder='0' allowfullscreen></iframe>", name, null, true);
        }
    };

    $scope.Like = function (videoId) {

        parent.angular.element('html').scope().Like(videoId, "new_video", function (e) {

            if (e.Success == true) {

                $scope.$apply(function () {
                    $scope.video.LikeDetail.LikeCount = ++$scope.video.LikeDetail.LikeCount;
                });
            }
        });
    };
}