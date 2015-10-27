/// <reference path="../jquery/jquery.min.js" />
/// <reference path="../jquery/jquery.widget.min.js" />
/// <reference path="../jquery/purl.js" />
/// <reference path="GlobalVariables.js" />
/// <reference path="mainJS.js" />

var AddForumSubjectHelper = {
    "OnLoad": function () {

        AddForumSubjectHelper.OnClickEvents();
        AddForumSubjectHelper.OnChangeEvents();

        parent.IndexHelper.AutoResize("ifrmContent");

    },
    "OnClickEvents": function () {
    },
    "OnChangeEvents": function () {
    },
    "CreateForumSubject": function (name, content, callBackFunction) {
        var html = "";

        var jData = {};

        jData.token = AddForumSubjectHelper.Token;

        jData.forumSubject = {};

        jData.forumSubject.Name = name;
        jData.forumSubject.Content = content;

        jData.forumSubject.Forum = {};
        jData.forumSubject.Forum.Id = AddForumSubjectHelper.ForumId;
        jData.forumSubject.Forum.Name = "";
        jData.forumSubject.Forum.LogicalName = "new_forum";


        jData.forumSubject.User = {};
        jData.forumSubject.User.Id = AddForumSubjectHelper.UserId;
        jData.forumSubject.User.Name = "";
        jData.forumSubject.User.LogicalName = "new_user";

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/CreateForumSubject",
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

                    if (data.Success == true) {

                        html += "<a href='#' type='subject' id='" + data.CrmId + "'><h2 style='margin-top:20px;'><i class=' icon-cc-share'></i><span>" + $("#txtTitle").val() + "</span></h2></a>";

                        parent.$.Dialog.close();

                        parent.IndexHelper.AutoResize("ifrmContent");

                        parent.IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M061"), ReturnMessage(IndexHelper.LanguageCode, data.Result), "green", "white");
                    }
                    else {
                        parent.IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M059"), ReturnMessage(IndexHelper.LanguageCode, data.Result), "red", "white");
                    }
                }
                else {
                    parent.IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M059"), ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />CreateForumSubject", "red", "white");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), c + "<br />CreateForumSubject");
                return false;
            }
        });
    },
    "UserId": "",
    "PortalId": "",
    "ForumId": "",
    "Token": ""
};

function newForumSubjectController($scope, $sce) {

    $(parent.window).scrollTop(0);

    var parentUrl = $.url(parent.document.location);
    var url = $.url(document.location);

    AddForumSubjectHelper.Token = parentUrl.param("token");
    $scope.token = parentUrl.param("token");

    AddForumSubjectHelper.ForumId = url.param("forumid");
    $scope.forumId = url.param("forumid");

    $scope.attachmentUrl = attachmentUrl;
    $scope.ContentMessages = LanguageMessages[parent.IndexHelper.LanguageCode];

    $scope.showErrorHeader = false;
    $scope.showLoading = true;
    $scope.showMain = false;
    $scope.errorText = "";

    parent.IndexHelper.CheckSession(true, $scope.token, function (e) {
        $scope.$apply(function () {
            if (e.Success == true) {

                AddForumSubjectHelper.UserId = e.ReturnObject.PortalUserId;
                AddForumSubjectHelper.PortalId = e.ReturnObject.PortalId;

                $scope.userId = e.ReturnObject.PortalUserId;
                $scope.portalId = e.ReturnObject.PortalId;
            }
            else {
                return;
            }
        });
    });

    $scope.CreateForumSubject = function () {

        var title = $scope.title;
        var content = $scope.content;

        if (title == null || title == "" || title == undefined || title == "undefined") {
            parent.IndexHelper.ToastrShow("warning", ReturnMessage(parent.IndexHelper.LanguageCode, "M062"), ReturnMessage(parent.IndexHelper.LanguageCode, "M060"));
            return;
        }

        if (content == null || content == "" || content == undefined || content == "undefined") {
            parent.IndexHelper.ToastrShow("warning", ReturnMessage(parent.IndexHelper.LanguageCode, "M063"), ReturnMessage(parent.IndexHelper.LanguageCode, "M060"));
            return;
        }

        AddForumSubjectHelper.CreateForumSubject(title, content, function (e) {

            if (e.Success) {
                parent.IndexHelper.ToastrShow("success", ReturnMessage(parent.IndexHelper.LanguageCode, e.Result), "Başarılı");

                parent.IndexHelper.CloseDialog();
            }
            else {
                parent.IndexHelper.ToastrShow("error", ReturnMessage(parent.IndexHelper.LanguageCode, e.Result), "HATA");
            }
        });
    };

}