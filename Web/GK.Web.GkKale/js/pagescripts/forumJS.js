/// <reference path="../jquery/jquery.min.js" />
/// <reference path="../jquery/jquery.widget.min.js" />
/// <reference path="../jquery/purl.js" />
/// <reference path="GlobalVariables.js" />
/// <reference path="indexJS.js" />

var ForumHelper = {
    "OnLoad": function () {
        $(parent.window).scrollTop(0);

        ForumHelper.OnClickEvents();
        ForumHelper.OnChangeEvents();

        parent.IndexHelper.AutoResize("ifrmContent");

    },
    "OnClickEvents": function () {
    },
    "OnChangeEvents": function () {
    },
    "GetUserForums": function (callBackFunction) {
        //debugger;
        var jData = {};
        jData.token = ForumHelper.Token;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/GetUserForums",
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

                    //debugger;
                    if (data.Success == true) {

                        var html = "";

                        if (data.ReturnObject != null && data.ReturnObject.length > 0) {

                            for (var i = 0; i < data.ReturnObject.length; i++) {

                                if (data.ReturnObject[i].SubForums != null && data.ReturnObject[i].SubForums.length > 0) {

                                    if (ForumHelper.ShownRecCount < data.ReturnObject[i].SubForums.length) {
                                        html += "<h3 class='padding5 fg-white' style='padding-left:10px;background-color:#56237d !important;'>" + data.ReturnObject[i].Name;
                                        html += "<a href='#' ishide='0' forumid='" + data.ReturnObject[i].Id + "' type='showall' class='place-right'><p class='place-right tertiary-text fg-white' style='font-size:large; margin-right:10px;margin-left:5px; margin-top:4px;'>Tamamını Gör</p></a>"
                                        html += "<p class='place-right tertiary-text fg-white' style='font-size:large;margin-right:5px;margin-left:5px; margin-top:4px;'>" + data.ReturnObject[i].SubForums.length + "</p><i class='icon-clipboard-2 place-right'></i>";
                                        html += "</h3>";
                                    }
                                    else {
                                        html += "<h3 class='padding5 fg-white' style='padding-left:10px;background-color:#56237d !important;'>" + data.ReturnObject[i].Name + "</h3>";
                                    }


                                    for (var j = 0; j < data.ReturnObject[i].SubForums.length; j++) {

                                        if ((j + 1) <= ForumHelper.ShownRecCount) {
                                            html += "<div type='row' forumid='" + data.ReturnObject[i].Id + "'>";
                                        }
                                        else {
                                            html += "<div type='row' forumid='" + data.ReturnObject[i].Id + "' style='display:none;'>";
                                        }

                                        html += "<a href='#' id='" + data.ReturnObject[i].SubForums[j].Id + "' type='forum' forumid='" + data.ReturnObject[i].Id + "' parentname='" + data.ReturnObject[i].Name + "'>";

                                        //html += "<div type='row' forumid='" + data.ReturnObject[i].Id + "'>";
                                        //if ((j + 1) <= ForumHelper.ShownRecCount) {

                                        //}
                                        //else {
                                        //    html += "<a href='#' id='" + data.ReturnObject[i].SubForums[j].Id + "' type='forum' forumid='" + data.ReturnObject[i].Id + "' parentname='" + data.ReturnObject[i].Name + "' style='display:none;'>";
                                        //}

                                        html += "	<p class='fg-darkViolet' style='padding-left:10px;'>" + data.ReturnObject[i].SubForums[j].Name + "</p>";
                                        html += "</a>";
                                        html += "</div>";
                                    }
                                }
                                else {
                                    html += "<h3 class='padding5 fg-white' style='padding-left:10px;background-color:#56237d !important;'>" + data.ReturnObject[i].Name + "</h3>";
                                }

                                //html += "	</div>";
                                //html += "</div>";
                            }

                            if (html != "") {
                                $("#lstForums").html(html);
                            }

                            $("a[type='forum']").click(function () {
                                var id = $(this).attr("id");
                                var name = $(this).find("p").html();
                                var parentName = $(this).attr("parentname");
                                $("#ifrmContent", parent.document).attr("src", "forumsubjectlist.aspx?forumid=" + id + "&forumname=" + encodeURIComponent(name) + "&parentforumname=" + encodeURIComponent(parentName));
                            });

                            $("a[type='showall']").click(function () {
                                var forumId = $(this).attr("forumid");
                                var isHide = $(this).attr("ishide");

                                //if (isHide != "1") {

                                $("div[type='row'][forumid='" + forumId + "']").show("fast", function () {
                                    parent.IndexHelper.AutoResize("ifrmContent");
                                });

                                //$(this).attr("ishide", "1");
                                //$(this).find("p").html("Gizle");
                                //}
                                //else {
                                //    $("div[type='row'][forumid='" + forumId + "'] :gt(" + (ForumHelper.ShownRecCount) + ")").hide("fast", function () {
                                //        parent.IndexHelper.AutoResize("ifrmContent");
                                //    });

                                //    $(this).attr("ishide", "0");
                                //    $(this).find("p").html("Tamamını Gör");
                                //}



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
                    $("#lblErrorMain").html("<h1 class='fg-yellow'><i class='icon-warning'></i></h1><br />" + ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />GetUserFrGetUserForumsiendList");

                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), c + "<br />GetUserForums");
                return false;
            }
        });
    },
    "UserId": "",
    "PortalId": "",
    "ShownRecCount": 2,
    "Token": ""
};

function forumController($scope) {

    $(parent.window).scrollTop(0);

    var url = $.url(parent.document.location);
    ForumHelper.Token = url.param("token");
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

                ForumHelper.UserId = e.ReturnObject.PortalUserId;
                ForumHelper.PortalId = e.ReturnObject.PortalId;

                $scope.userId = e.ReturnObject.PortalUserId;
                $scope.portalId = e.ReturnObject.PortalId;
            }
            else {
                return;
            }
        });
    });

    ForumHelper.GetUserForums(function (e) {

        $scope.$apply(function () {
            var a = JSON.stringify(e);

            if (e.Success == true) {

                $scope.forumList = e.ReturnObject;

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

    $scope.OpenSubForum = function (parentId, parentName, id, name) {

        $("#ifrmContent", parent.document).attr("src", "forumSubject.html?forumid=" + id + "&forumname=" + encodeURIComponent(name) + "&parentforumname=" + encodeURIComponent(parentName) + "&parentforumid="+parentId);
    };
}