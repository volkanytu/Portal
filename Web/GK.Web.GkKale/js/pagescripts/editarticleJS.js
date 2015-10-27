/// <reference path="../jquery/jquery.min.js" />
/// <reference path="../jquery/jquery.widget.min.js" />
/// <reference path="../jquery/purl.js" />
/// <reference path="GlobalVariables.js" />
/// <reference path="indexJS.js" />
/// <reference path="mainJS.js" />
/// <reference path="../jquery/jquery.session.js" />

var ArticleHelper = {
    "OnLoad": function () {
        $(parent.window).scrollTop(0);

        ArticleHelper.OnClickEvents();
        ArticleHelper.OnChangeEvents();
    },
    "OnClickEvents": function () {
    },
    "OnChangeEvents": function () {
    },
    "GetArticleInfo": function (callBackFunction) {
        //debugger;
        var jData = {};
        jData.token = ArticleHelper.Token;
        jData.articleId = ArticleHelper.ObjectId;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/GetArticleInfo",
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
                    $("#lblErrorMain").html("<h1 class='fg-yellow'><i class='icon-warning'></i></h1><br />" + ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br/>GetArticleInfo");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ShowAlertDialog(3, "Error", "Error:" + c + "<br />GetArticleInfo");
                return false;
            }
        });
    },
    "SendComment": function (description, entityId, callBackFunction) {

        var jData = {};
        jData.token = ArticleHelper.Token;
        jData.comment = {};

        jData.comment.PortalUser = {};
        jData.comment.PortalUser.Id = ArticleHelper.UserId;
        jData.comment.PortalUser.Name = parent.IndexHelper.UserName;
        jData.comment.PortalUser.LogicalName = "new_user";

        jData.comment.Article = {};
        jData.comment.Article.Id = entityId;
        jData.comment.Article.LogicalName = "new_article";

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

function editArticleController($scope, $sce) {

    $(parent.window).scrollTop(0);

    var parentUrl = $.url(parent.document.location);
    var url = $.url(document.location);

    ArticleHelper.Token = parentUrl.param("token");
    $scope.token = parentUrl.param("token");

    ArticleHelper.ObjectId = url.param("objectid");
    $scope.articleId = url.param("objectid");

    ArticleHelper.CategoryId = url.param("categoryid");
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

                ArticleHelper.UserId = e.ReturnObject.PortalUserId;
                ArticleHelper.PortalId = e.ReturnObject.PortalId;

                $scope.userId = e.ReturnObject.PortalUserId;
                $scope.portalId = e.ReturnObject.PortalId;
            }
            else {
                return;
            }
        });
    });

    ArticleHelper.GetArticleInfo(function (e) {

        $scope.$apply(function () {
            var a = JSON.stringify(e);

            if (e.Success == true) {

                $scope.article = e.ReturnObject;

                $scope.articleDesc = $sce.trustAsHtml(e.ReturnObject.Description);


                if (e.ReturnObject.AttachmentFileList == null || e.ReturnObject.AttachmentFileList.length == 0) {
                    $scope.noDocRecord = true;
                }
                else {
                    $scope.noDocRecord = false;
                }

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

        $("#ifrmContent", parent.document).attr("src", "makalelerDetay.html?objectid=" + $scope.categoryId + "&name=" + $scope.categoryName);
    };

    $scope.GetEntityComments = function () {

        ArticleHelper.GetEntityComments(ArticleHelper.Token, ArticleHelper.ObjectId, "new_article", 0, 20, function (e) {

            $scope.$apply(function () {

                if (e.Success == true) {
                    $scope.article.CommentList = e.ReturnObject;

                    setTimeout(function () {
                        parent.IndexHelper.AutoResize("ifrmContent");
                    }, 500);
                }

            });
        });
    };

    $scope.sendComment = function (articleId, description) {
        if (description == null || description == "" || description == undefined || description == "undefined") {
            parent.IndexHelper.ToastrShow("warning", ReturnMessage(parent.IndexHelper.LanguageCode, "M087"), "Uyarı");
            return;
        }

        ArticleHelper.SendComment(description, articleId, function (e) {
            $scope.$apply(function () {

                if (e.Success == true) {

                    $scope.noCommentRecord = false;

                    parent.IndexHelper.ToastrShow("success", ReturnMessage(parent.IndexHelper.LanguageCode, e.Result), "Başarıılı");

                    $scope.article.commentText = null;
                    $scope.GetEntityComments();
                }
                else {
                    parent.IndexHelper.ToastrShow("error", ReturnMessage(parent.IndexHelper.LanguageCode, e.Result), "Hata");
                }
            });
        });
    };
}