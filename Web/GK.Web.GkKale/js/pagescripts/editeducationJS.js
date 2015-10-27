/// <reference path="../jquery/jquery.min.js" />
/// <reference path="../jquery/jquery.widget.min.js" />
/// <reference path="../jquery/purl.js" />
/// <reference path="GlobalVariables.js" />
/// <reference path="indexJS.js" />
/// <reference path="../jquery/jquery.session.js" />
/// <reference path="mainJS.js" />

var EducationHelper = {
    "OnLoad": function () {
        $(parent.window).scrollTop(0);

        EducationHelper.OnClickEvents();
        EducationHelper.OnChangeEvents();

    },
    "OnClickEvents": function () {
    },
    "OnChangeEvents": function () {

    },
    "GetEducationInfo": function (callBackFunction) {

        //debugger;
        var jData = {};
        jData.token = EducationHelper.Token;
        jData.educationId = EducationHelper.ObjectId;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/GetEducationInfo",
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


                    if (data.Success == true) {
                        $("#divMain").show();
                    }
                    else {
                        $("#lblErrorMain").show();
                        $("#lblErrorMain").html("<h1 class='fg-yellow'><i class='icon-warning'></i></h1><br />" + ReturnMessage(IndexHelper.LanguageCode, data.Result));

                        return;
                    }

                    var html = "";

                    if (data.ReturnObject != null && data.ReturnObject.length == 0) {
                        $("#lblErrorMain").show();
                        $("#lblErrorMain").html("<h1 class='fg-yellow'><i class='icon-warning'></i></h1><br />" + ReturnMessage(IndexHelper.LanguageCode, "M017"));
                    }
                    else {

                        $("#btnEducation").html(data.ReturnObject.Name);

                        if (data.ReturnObject.ImagePath != null && data.ReturnObject.ImagePath != "" && data.ReturnObject.ImagePath != "no_image_available.png") {
                            html += "	<div class='row' style='margin-top: -5px;'>";
                            html += "		<div class='span7bucuk padding10' style='margin-bottom: 10px; margin-top: -20px;'>";
                            html += "			<img class='span7bucuk' src='" + attachmentUrl + data.ReturnObject.ImagePath + "'>";
                            html += "		</div>";
                            html += "	</div>";
                        }

                        //LIKE - COMMENT COUNT
                        html += "	<div class='row' style='margin-top: -20px;'>";
                        html += "		<div class='span7bucuk padding10' style='margin-top: -40px;'>";
                        html += "           <div class='span7bucuk' style='background-color: #f6f7f8;width:560px !important;'>";
                        html += "			    <p class='padding10 place-left' style='background-color: #f6f7f8;width:400px;'><span class='item-title'>" + data.ReturnObject.Name + "</span></p>";
                        html += "               <p class='padding10 place-right' style='background-color: #f6f7f8;width:160px;'>";
                        html += "                   <small class='fg-lightGray place-right' style='margin-left:5px;' id='lblCommentCount'>" + (data.ReturnObject.CommentList != null ? data.ReturnObject.CommentList.length : "0") + "</small>";
                        html += "			        <i class='icon-comments-5 fg-lightGray place-right' style='margin-left: 10px;'></i>";
                        html += "                   <small class='fg-lightGray place-right' style='margin-left:5px;' type='down'>22</small>";
                        html += "                   <i class='icon-thumbs-down fg-lightGray place-right' style='margin-left:10px;' id='btnDown'></i>";
                        html += "                   <small class='fg-red place-right' style='margin-left:5px;' type='up'>21</small>";
                        html += "			        <i class='icon-thumbs-up fg-red place-right' style='margin-left: 0px;' id='btnUp'></i>";
                        html += "               </p>";
                        html += "           </div>";
                        html += "	    </div>";
                        html += "	</div>";
                        //END LIKE - COMMENT COUNT

                        html += "	<div class='row' style='margin-top: -20px;'>";
                        html += "		<div class='span7bucuk padding20' style='margin-top: -30px;'>";
                        html += "			<p>" + data.ReturnObject.Description + "</p>";
                        html += "		</div>";
                        html += "	</div>";

                        $("#rowMainInfo").html(html);

                        $("img").load(function () {
                            parent.IndexHelper.AutoResize("ifrmContent");
                        });

                        $("#btnDown").click(function () {

                            parent.IndexHelper.DislikeEntity(EducationHelper.Token, EducationHelper.ObjectId, "new_education");
                        });

                        $("#btnUp").click(function () {

                            parent.IndexHelper.LikeEntity(EducationHelper.Token, EducationHelper.ObjectId, "new_education");
                        });

                        parent.IndexHelper.GetEntityLikeInfo(EducationHelper.Token, EducationHelper.ObjectId, "new_education");

                        //DOCUMENTS
                        html = ""
                        $("#lblErrorDocs").hide();

                        if (data.ReturnObject.AttachmentFileList != null && data.ReturnObject.AttachmentFileList.length > 0) {

                            for (var i = 0; i < data.ReturnObject.AttachmentFileList.length; i++) {
                                html += "<li>";
                                html += "<a href='" + attachmentUrl + data.ReturnObject.AttachmentFileList[i].FilePath + "' target='_blank' download='" + data.ReturnObject.AttachmentFileList[i].Name + "' id='" + data.ReturnObject.AttachmentFileList[i].AttachmentFileId + "'>";
                                html += "<p><i class='" + data.ReturnObject.AttachmentFileList[i].IconFile + "'></i>&nbsp;" + data.ReturnObject.AttachmentFileList[i].Name + "</p>";
                                html += "</a>";
                                html += "</li>";
                            }

                            $("#rowDocs").show();
                        }
                        else {
                            $("#rowDocs").hide();
                            $("#lblErrorDocs").show();
                            $("#lblErrorDocs").html(ReturnMessage(IndexHelper.LanguageCode, "M074"));
                        }

                        $("#lstDocs").show();
                        $("#lstDocs").html(html);

                        $("#lstDocs a").click(function (e) {

                            var id = $(this).attr("id");

                        });

                        // END DOCUMENTS


                        //COMMENTS
                        html = "";
                        $("#lblErrorComments").hide();

                        if (data.ReturnObject.CommentList != null && data.ReturnObject.CommentList.length > 0) {


                            for (var i = 0; i < data.ReturnObject.CommentList.length; i++) {

                                html += "		<div class='row bg-grayLighter' style='margin-top: 0px;background-color:#e8e8e8 !important;'>";
                                html += "			<div class='span1 padding10'>";
                                html += "              <div style='width: 40px; height: 40px; float: left;background-size: 100%; background-position: center center;  background-repeat: no-repeat; background-image: url(\"" + attachmentUrl + data.ReturnObject.CommentList[i].PortalUserImage + "\");'>";
                                //html += "				<img class='span1' src='" + attachmentUrl + data.ReturnObject.CommentList[i].PortalUserImage + "' />";
                                html += "			    </div>";
                                html += "			</div>";
                                html += "			<div class='span6 padding10'>";
                                html += "				<p class='tertiary-text' style='margin-left: -30px;'><a href='#' type='edituser' username='" + data.ReturnObject.CommentList[i].PortalUser.Name + "' userid='" + data.ReturnObject.CommentList[i].PortalUser.Id + "'><span class='fg-blue tertiary-text'>" + data.ReturnObject.CommentList[i].PortalUser.Name + "</span></a>&nbsp;" + data.ReturnObject.CommentList[i].Description + "</p>";
                                html += "				<p class='tertiary-text' style='margin-left: -30px; margin-top: -5px;'><small>" + data.ReturnObject.CommentList[i].CreatedOnString + "</small></p>";
                                html += "			</div>";
                                html += "		</div>";
                            }
                        }
                        else {
                            $("#lblErrorComments").show();
                            $("#lblErrorComments").html(ReturnMessage(IndexHelper.LanguageCode, "M006"));

                        }

                        html += "	<div class='row bg-grayLighter' style='margin-top: 0px;background-color:#e8e8e8 !important;' type='postComment'>";
                        html += "		<div class='span1 padding10'>";
                        html += "           <div style='width: 40px; height: 40px; float: left;background-size: 100%; background-position: center center;  background-repeat: no-repeat; background-image: url(\"" + imgUrl + "\");'>";
                        //html += "			<img class='span1' src='" + imgUrl + "' />";
                        html += "		    </div>";
                        html += "		</div>";
                        html += "		<div class='span6 padding10'>";
                        html += "			<div class='input-control text' data-role='input-control' style='margin-left: -30px; margin-top: 3px;'>";
                        html += "				<input id='" + EducationHelper.ObjectId + "' class='tertiary-text-secondary' rows='5' type='text' value='' placeholder='Yorum yap' texttype='comment' style='width: 480px;' />";
                        html += "				<a href='#'><i class='place-right icon-share-3 fg-gray' style='margin-top: -23px; margin-right: -20px; z-index: 1; position: relative;'></i></a>";
                        html += "				<img src='pics/loading.gif' class='place-right' width='20' style='margin-top: -25px; margin-right: -20px; z-index: 1; position: relative; display: none;' />";
                        html += "			</div>";
                        html += "		</div>";
                        html += "	</div>";
                        html += "</div>";

                        $("#lstComments").show();
                        $("#lstComments").html(html);


                        $("div[type='postComment']").find("input[texttype='comment']").keydown(function (e) {
                            if (e.which == 13) {
                                var graffitiId = $(this).attr("id");
                                var description = $(this).val();
                                if (description == null || description == "" || description == undefined || description == "undefined") {
                                    parent.IndexHelper.ShowAlertDialog(2, ReturnMessage(IndexHelper.LanguageCode, "M060"), ReturnMessage(IndexHelper.LanguageCode, "M087"));
                                    return;
                                }

                                EducationHelper.SendComment(description, EducationHelper.ObjectId);
                            }
                        });

                        $("div[type='postComment']").find("a").click(function (e) {
                            var graffitiId = $(this).parent().find("input").attr("id");
                            var description = $(this).parent().find("input").val();
                            if (description == null || description == "" || description == undefined || description == "undefined") {
                                parent.IndexHelper.ShowAlertDialog(2, ReturnMessage(IndexHelper.LanguageCode, "M060"), ReturnMessage(IndexHelper.LanguageCode, "M087"));
                                return;
                            }

                            EducationHelper.SendComment(description, EducationHelper.ObjectId);
                        });
                        // END COMMENTS
                    }
                }
                else {
                    $("#lblErrorMain").show();
                    $("#lblErrorMain").html("<h1 class='fg-yellow'><i class='icon-warning'></i></h1><br />" + ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />GetEducationInfo");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), c + "<br />GetEducationInfo");
                return false;
            }
        });
    },
    "SendComment": function (description, educationId, callBackFunction) {

        var jData = {};
        jData.token = EducationHelper.Token;

        jData.comment = {};

        jData.comment.PortalUser = {};
        jData.comment.PortalUser.Id = EducationHelper.UserId;
        jData.comment.PortalUser.Name = parent.IndexHelper.UserName;
        jData.comment.PortalUser.LogicalName = "new_user";

        jData.comment.Education = {};
        jData.comment.Education.Id = educationId;
        jData.comment.Education.LogicalName = "new_education";

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


function editEducationController($scope, $sce) {

    $(parent.window).scrollTop(0);

    var parentUrl = $.url(parent.document.location);
    var url = $.url(document.location);

    EducationHelper.Token = parentUrl.param("token");
    $scope.token = parentUrl.param("token");

    EducationHelper.ObjectId = url.param("objectid");
    $scope.educationId = url.param("objectid");

    EducationHelper.CategoryId = url.param("categoryid");
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

                EducationHelper.UserId = e.ReturnObject.PortalUserId;
                EducationHelper.PortalId = e.ReturnObject.PortalId;

                $scope.userId = e.ReturnObject.PortalUserId;
                $scope.portalId = e.ReturnObject.PortalId;
            }
            else {
                return;
            }
        });
    });

    EducationHelper.GetEducationInfo(function (e) {

        $scope.$apply(function () {
            var a = JSON.stringify(e);

            if (e.Success == true) {

                $scope.education = e.ReturnObject;

                $scope.educationDesc = $sce.trustAsHtml(e.ReturnObject.Description);


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

        $("#ifrmContent", parent.document).attr("src", "egitimlerDetay.html?objectid=" + $scope.categoryId + "&name=" + $scope.categoryName);
    };

    $scope.GetEntityComments = function () {

        EducationHelper.GetEntityComments(EducationHelper.Token, EducationHelper.ObjectId, "new_education", 0, 20, function (e) {

            $scope.$apply(function () {

                if (e.Success == true) {
                    $scope.education.CommentList = e.ReturnObject;

                    setTimeout(function () {
                        parent.IndexHelper.AutoResize("ifrmContent");
                    }, 500);
                }

            });
        });
    };

    $scope.sendComment = function (educationId, description) {
        if (description == null || description == "" || description == undefined || description == "undefined") {
            parent.IndexHelper.ToastrShow("warning", ReturnMessage(parent.IndexHelper.LanguageCode, "M087"), "Uyarı");
            return;
        }

        EducationHelper.SendComment(description, educationId, function (e) {
            $scope.$apply(function () {

                if (e.Success == true) {

                    $scope.noCommentRecord = false;

                    parent.IndexHelper.ToastrShow("success", ReturnMessage(parent.IndexHelper.LanguageCode, e.Result), "Başarıılı");

                    $scope.education.commentText = null;
                    $scope.GetEntityComments();
                }
                else {
                    parent.IndexHelper.ToastrShow("error", ReturnMessage(parent.IndexHelper.LanguageCode, e.Result), "Hata");
                }
            });
        });
    };
}