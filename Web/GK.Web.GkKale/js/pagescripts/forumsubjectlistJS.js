/// <reference path="../jquery/jquery.min.js" />
/// <reference path="../jquery/jquery.widget.min.js" />
/// <reference path="../jquery/purl.js" />
/// <reference path="GlobalVariables.js" />
/// <reference path="indexJS.js" />
/// <reference path="mainJS.js" />

var ForumSubjectListHelper = {
    "OnLoad": function () {
        $(parent.window).scrollTop(0);

        ForumSubjectListHelper.OnClickEvents();
        ForumSubjectListHelper.OnChangeEvents();

        parent.IndexHelper.AutoResize("ifrmContent");

    },
    "OnClickEvents": function () {
    },
    "OnChangeEvents": function () {
    },
    "GetForumSubjects": function (callBakcFunction) {
        //debugger;
        var jData = {};
        jData.token = ForumSubjectListHelper.Token;

        jData.forumId = ForumSubjectListHelper.ForumId;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/GetForumSubjects",
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

                    //debugger;
                    if (data.Success == true) {

                        var html = "";

                        if (data.ReturnObject != null && data.ReturnObject.length > 0) {

                            for (var i = 0; i < data.ReturnObject.length; i++) {
                                html += "<a href='#' type='subject' id='" + data.ReturnObject[i].Id + "'>";
                                html += "	<p class='fg-darkViolet item-title'>" + data.ReturnObject[i].Name + "</p>";
                                html += "</a><legend></legend>";
                            }

                            if (html != "") {
                                $("#lstForumSubjects").html(html);
                            }

                            $("a[type='subject']").click(function () {

                                var id = $(this).attr("id");

                                var subForumName = encodeURIComponent($("#btnSubForum").html());
                                var mainForumName = encodeURIComponent($("#btnMainForum").html());
                                var subjectName = encodeURIComponent($(this).find("p").html());

                                $("#ifrmContent", parent.document).attr("src", "editforumsubject.aspx?subjectid=" + id + "&forumid=" + ForumSubjectListHelper.ForumId + "&parentforumname=" + mainForumName + "&forumname=" + subForumName + "&subjectname=" + subjectName);
                            });

                            $("#divMain").show();

                            parent.IndexHelper.AutoResize("ifrmContent");
                        }

                    }
                    else {
                        $("#lblErrorMain").show();
                        $("#lblErrorMain").html("<h1 class='fg-yellow'><i class='icon-warning'></i></h1><br />" + ReturnMessage(IndexHelper.LanguageCode, data.Result));

                        return false;
                    }
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
    "UserId": "",
    "PortalId": "",
    "ForumId": "",
    "ForumName": "",
    "ParentForumId": "",
    "ParentForumName": "",
    "Token": ""
};

function forumSubjectController($scope) {

    $(parent.window).scrollTop(0);

    var parentUrl = $.url(parent.document.location);
    var url = $.url(document.location);

    ForumSubjectListHelper.Token = parentUrl.param("token");
    $scope.token = parentUrl.param("token");

    ForumSubjectListHelper.ForumId = url.param("forumid");
    $scope.forumId = url.param("forumid");
    ForumSubjectListHelper.ForumName = url.param("forumname");
    $scope.forumName = url.param("forumname");

    ForumSubjectListHelper.ParentForumId = url.param("parentforumid");
    $scope.parentForumId = url.param("parentforumid");
    ForumSubjectListHelper.ParentForumName = url.param("parentforumname");
    $scope.parentForumName = url.param("parentforumname");

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

                ForumSubjectListHelper.UserId = e.ReturnObject.PortalUserId;
                ForumSubjectListHelper.PortalId = e.ReturnObject.PortalId;

                $scope.userId = e.ReturnObject.PortalUserId;
                $scope.portalId = e.ReturnObject.PortalId;
            }
            else {
                return;
            }
        });
    });

    ForumSubjectListHelper.GetForumSubjects(function (e) {

        $scope.$apply(function () {
            var a = JSON.stringify(e);

            if (e.Success == true) {

                $scope.forumSubjects = e.ReturnObject;

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

    $scope.NewSubject = function () {
        parent.IndexHelper.ShowDialog("<iframe src='popupYeniKonuBasligi.html?forumid=" + ForumSubjectListHelper.ForumId + "' style='width:100%;height:420px;' />", null, null, true);
    };

    $scope.EditForumSubject = function (id, subjectName) {

        $("#ifrmContent", parent.document).attr("src", "forumDetail.html?subjectid=" + id + "&forumid=" + ForumSubjectListHelper.ForumId + "&parentforumname=" + encodeURIComponent(ForumSubjectListHelper.ParentForumName) + "&parentforumid=" + ForumSubjectListHelper.ParentForumId + "&forumname=" + encodeURIComponent(ForumSubjectListHelper.ForumName) + "&subjectname=" + subjectName);
    };

}