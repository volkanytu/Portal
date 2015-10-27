/// <reference path="../jquery/jquery.min.js" />
/// <reference path="../jquery/jquery.widget.min.js" />
/// <reference path="../jquery/purl.js" />
/// <reference path="GlobalVariables.js" />
/// <reference path="indexJS.js" />

var ForumSubjectHelper = {
    "OnLoad": function () {
        $(parent.window).scrollTop(0);

        var url = $.url(document.location);

        ForumSubjectHelper.ForumSubjectId = url.param("subjectid");
        ForumSubjectHelper.ForumId = url.param("forumid");
        $("#btnSubForum").html(url.param("forumname"));
        $("#btnMainForum").html(url.param("parentforumname"));
        $("#btnSubject").html(url.param("subjectname"));

        ForumSubjectHelper.OnClickEvents();
        ForumSubjectHelper.OnChangeEvents();

        ForumSubjectHelper.GetForumSubjectInfo();

        parent.IndexHelper.AutoResize("ifrmContent");

    },
    "OnClickEvents": function () {

        $("#btnBack").click(function () {
            //$("#ifrmContent", parent.document).attr("src", "forumsubjectlist.aspx?forumid=" + ForumSubjectHelper.ForumId);
            $("#btnSubForum").click();
        });

        $("#btnSubForum").click(function () {
            var subForumName = encodeURIComponent($("#btnSubForum").html());
            var mainForumName = encodeURIComponent($("#btnMainForum").html());
            $("#ifrmContent", parent.document).attr("src", "forumsubjectlist.aspx?forumid=" + ForumSubjectHelper.ForumId + "&forumname=" + subForumName + "&parentforumname=" + mainForumName);
        });

        $("#btnMainForum").click(function () {
            $("#ifrmContent", parent.document).attr("src", "forum.aspx");
        });

        $("#btnForum").click(function () {
            $("#ifrmContent", parent.document).attr("src", "forum.aspx");
        });

    },
    "OnChangeEvents": function () {
    },
    "GetForumSubjectInfo": function (callBakcFunction) {
        //debugger;
        var jData = {};
        jData.token = ForumSubjectHelper.Token;

        jData.forumSubjectId = ForumSubjectHelper.ForumSubjectId;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/GetForumSubjectInfo",
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

                    callBakcFunction(data);

                    return;
                }
                else {
                    $("#lblErrorMain").show();
                    $("#lblErrorMain").html("<h1 class='fg-yellow'><i class='icon-warning'></i></h1><br />" + ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />GetForumSubjects");

                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), c + "<br />GetForumSubjects");
                return false;
            }
        });
    },
    "SendComment": function (description, subjectId, callBackFunction) {

        var jData = {};
        jData.token = ForumSubjectHelper.Token;

        jData.comment = {};

        jData.comment.PortalUser = {};
        jData.comment.PortalUser.Id = ForumSubjectHelper.UserId;
        jData.comment.PortalUser.Name = parent.IndexHelper.UserName;
        jData.comment.PortalUser.LogicalName = "new_user";

        jData.comment.ForumSubject = {};
        jData.comment.ForumSubject.Id = subjectId;
        jData.comment.ForumSubject.LogicalName = "new_forumsubject";

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
    "ForumSubjectId": "",
    "ForumId": "",
    "Token": ""
};

function forumDetailController($scope, $sce) {

    $(parent.window).scrollTop(0);

    var parentUrl = $.url(parent.document.location);
    var url = $.url(document.location);

    ForumSubjectHelper.Token = parentUrl.param("token");
    $scope.token = parentUrl.param("token");

    ForumSubjectHelper.ForumSubjectId = url.param("subjectid");
    ForumSubjectHelper.ForumId = url.param("forumid");

    $scope.forumSubjectId = url.param("subjectid");
    $scope.forumId = url.param("forumid");
    $scope.forumName = url.param("forumname");
    $scope.parentForumId = url.param("parentforumid");
    $scope.parentForumName = url.param("parentforumname");
    $scope.subjectName = url.param("subjectname");

    $scope.attachmentUrl = attachmentUrl;
    $scope.ContentMessages = LanguageMessages[parent.IndexHelper.LanguageCode];

    $scope.showErrorHeader = false;
    $scope.showLoading = true;
    $scope.showMain = false;
    $scope.errorText = "";

    parent.IndexHelper.CheckSession(true, $scope.token, function (e) {
        $scope.$apply(function () {
            if (e.Success == true) {

                ForumSubjectHelper.UserId = e.ReturnObject.PortalUserId;
                ForumSubjectHelper.PortalId = e.ReturnObject.PortalId;

                $scope.userId = e.ReturnObject.PortalUserId;
                $scope.portalId = e.ReturnObject.PortalId;
            }
            else {
                return;
            }
        });
    });

    ForumSubjectHelper.GetForumSubjectInfo(function (e) {

        $scope.$apply(function () {
            var a = JSON.stringify(e);

            if (e.Success == true) {

                $scope.subject = e.ReturnObject;

                $scope.subjectDesc = $sce.trustAsHtml(e.ReturnObject.Content);

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
        $("#ifrmContent", parent.document).attr("src", "forumSubject.html?forumid=" + $scope.forumId + "&forumname=" + encodeURIComponent($scope.forumName) + "&parentforumname=" + encodeURIComponent($scope.parentForumName) + "&parentforumid=" + $scope.parentForumId);
    };

    $scope.GetEntityComments = function () {

        ForumSubjectHelper.GetEntityComments(ForumSubjectHelper.Token, ForumSubjectHelper.ForumSubjectId, "new_forumsubject", 0, 20, function (e) {

            $scope.$apply(function () {

                if (e.Success == true) {
                    $scope.subject.CommentList = e.ReturnObject;

                    setTimeout(function () {
                        parent.IndexHelper.AutoResize("ifrmContent");
                    }, 500);
                }

            });
        });
    };

    $scope.sendComment = function (subjectId, description) {
        if (description == null || description == "" || description == undefined || description == "undefined") {
            parent.IndexHelper.ToastrShow("warning", ReturnMessage(parent.IndexHelper.LanguageCode, "M087"), "Uyarı");
            return;
        }

        ForumSubjectHelper.SendComment(description, subjectId, function (e) {
            $scope.$apply(function () {

                if (e.Success == true) {

                    $scope.noCommentRecord = false;

                    parent.IndexHelper.ToastrShow("success", ReturnMessage(parent.IndexHelper.LanguageCode, e.Result), "Başarıılı");

                    $scope.subject.commentText = null;
                    $scope.GetEntityComments();
                }
                else {
                    parent.IndexHelper.ToastrShow("error", ReturnMessage(parent.IndexHelper.LanguageCode, e.Result), "Hata");
                }
            });
        });
    };

    $scope.EditUser = function (userId) {

        parent.angular.element('html').scope().EditUser(userId)
    };

    $scope.Like = function (subjectId) {

        parent.angular.element('html').scope().Like(subjectId, "new_forumsubject", function (e) {

            if (e.Success == true) {

                $scope.$apply(function () {
                    $scope.subject.LikeDetail.LikeCount = ++$scope.subject.LikeDetail.LikeCount;
                });
            }
        });
    };
}