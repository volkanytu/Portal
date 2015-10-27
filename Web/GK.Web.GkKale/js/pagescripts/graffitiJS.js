/// <reference path="../jquery/jquery.min.js" />
/// <reference path="../jquery/jquery.widget.min.js" />
/// <reference path="../jquery/purl.js" />
/// <reference path="GlobalVariables.js" />
/// <reference path="../jquery/jquery.session.js" />
/// <reference path="indexJS.js" />
/// <reference path="languageMessagesJS.js" />
/// <reference path="mainJS.js" />
var isFirstTime = true;

var GraffitiHelper = {
    "OnLoad": function () {
        $(parent.window).scrollTop(0);

        var url = $.url(document.location);
        GraffitiHelper.GetGraffities(GraffitiHelper.GraffitiCount, (GraffitiHelper.GraffitiCount + GraffitiHelper.IncreaseCount));
        GraffitiHelper.GraffitiCount = (GraffitiHelper.GraffitiCount + GraffitiHelper.IncreaseCount);

        parent.IndexHelper.AutoResize("ifrmContent");

        GraffitiHelper.OnClickEvents();
        GraffitiHelper.OnChangeEvents();
    },
    "OnClickEvents": function () {
        $("#btnShare").click(function () {
            var txtGraffiti = $("#txtGraffiti").val();

            if (txtGraffiti == null || txtGraffiti == "" || txtGraffiti == undefined || txtGraffiti == "undefined") {
                parent.IndexHelper.ShowAlertDialog(2, ReturnMessage(IndexHelper.LanguageCode, "M060"), ReturnMessage(IndexHelper.LanguageCode, "CM107"));
                return;
            }

            GraffitiHelper.CreateGraffiti();
        });

        $("#btnSelectPhoto").click(function () {
            $("#file").click();
        });

        $("#btnDelete").click(function () {
            $("#lblImage").attr("fileData", null).attr("mimeType", null).attr("fileName", null).html("");
            $(this).hide();
        });
    },
    "OnChangeEvents": function () {

        $("#file").change(function () {

            $("#txt_file").val(this.files[0].name);

            GraffitiHelper.AddFile();
        });

    },
    "GetGraffities": function (start, end, callBackFunction) {

        $("#lblErrorMain").hide();
        //debugger;
        var jData = {};
        jData.token = GraffitiHelper.Token;
        jData.start = start;
        jData.end = end;
        jData.commentCount = GraffitiHelper.CommentIncreaseCount;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/GetGraffitiList",
            async: true,
            dataType: "json",
            contentType: "application/json;",
            type: "POST",
            data: jSonData,
            beforeSend: function () {
                parent.IndexHelper.ShowLoading("Duvar yazıları çekiliyor...", null);
            },
            complete: function () {
                parent.IndexHelper.CanScrollEvent = true;

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
                    $("#lblErrorMain").html("<h1 class='fg-yellow'><i class='icon-warning'></i></h1><br />" + ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />GetGraffities");
                    return false;
                }

                isFirstTime = false;
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), c + "<br />GetGraffities");
                return false;
            }
        });
    },
    "GetGraffitiComments": function (graffitiId, start, end, callBackFunction) {
        var jData = {};
        jData.token = GraffitiHelper.Token;
        jData.entityId = graffitiId;
        jData.entityName = "new_graffiti";
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
    "SendComment": function (description, graffitiId, callBackFunction) {

        var jData = {};
        jData.token = GraffitiHelper.Token;

        jData.comment = {};

        jData.comment.PortalUser = {};
        jData.comment.PortalUser.Id = GraffitiHelper.UserId;
        jData.comment.PortalUser.Name = parent.IndexHelper.UserName;
        jData.comment.PortalUser.LogicalName = "new_user";

        jData.comment.Graffiti = {};
        jData.comment.Graffiti.Id = graffitiId;
        jData.comment.Graffiti.LogicalName = "new_graffiti";

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
    "CreateGraffiti": function (txtGraffiti, hasImage, callBackFunction) {

        var jData = {};
        jData.token = GraffitiHelper.Token;

        jData.graffiti = {};

        jData.graffiti.HasMedia = hasImage;

        jData.graffiti.Portal = {};
        jData.graffiti.Portal.Id = GraffitiHelper.PortalId;
        jData.graffiti.Portal.LogicalName = "new_portal";

        jData.graffiti.PortalUser = {};
        jData.graffiti.PortalUser.Id = GraffitiHelper.UserId;
        jData.graffiti.PortalUser.Name = parent.IndexHelper.UserName;
        jData.graffiti.PortalUser.LogicalName = "new_user";

        jData.graffiti.Description = txtGraffiti;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/SaveGraffiti",
            async: true,
            dataType: "json",
            contentType: "application/json;",
            type: "POST",
            data: jSonData,
            beforeSend: function () {
                parent.IndexHelper.ShowLoading("Duvar yazısı kaydediliyor...", null);
            },
            complete: function () {
                parent.IndexHelper.AutoResize("ifrmContent");
                parent.IndexHelper.CloseDialog();
            },
            success: function (data) {
                //debugger;
                if (data != null) {
                    if (data.Success == true) {

                        callBackFunction(data);

                        return;
                    }
                }
                else {
                    parent.IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />CreateGraffiti", "error");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ShowNotify(c + "<br />CreateGraffiti", "error");
                return false;
            }
        });
    },
    "ReadImage": function (input, callBackFunction) {

        if (input.files && input.files[0]) {

            var reader = new FileReader();

            var file = input.files[0];

            reader.readAsDataURL(input.files[0]);

            reader.onload = function (e) {

                callBackFunction(e);

                var fileSize = parseInt(file.size / 1024);
                var fileName = file.name;

                var imageFile = {};

                imageFile.blob = file;
                imageFile.fileName = file.name;

                GraffitiHelper.ImageFile = imageFile;
            }
        }

        reader.readAsDataURL(input.files[0]);
    },
    "PostFile": function (formData, graffitiId, callBackFunction) {
        $.ajax({
            url: "uploadHelper.ashx?operation=2&graffitiId=" + graffitiId,
            type: 'POST',
            complete: function () {
            },
            progress: function (evt) {
            },
            beforeSend: function (e) {
                parent.IndexHelper.ShowLoading("Dosya kaydediliyor...", null);
            },
            complete: function () {
                parent.IndexHelper.CloseDialog();
            },
            success: function (data) {
                if (data != null) {

                    callBackFunction(data);

                    return;

                    if (data.Success == true) {

                    }
                    else {
                        parent.IndexHelper.ShowNotify(data.Result, "warning");
                    }
                }
                else {
                }
            },
            error: function (a, b, c) {

                parent.IndexHelper.ShowNotify("Error:" + c, "error");
            },
            data: formData,
            cache: false,
            contentType: false,
            processData: false
        });
    },
    "ImageFile": null,
    "UserId": "",
    "PortalId": "",
    "GraffitiCount": 0,
    "IncreaseCount": 5,
    "CommentIncreaseCount": 2,
    "Token": ""
};

function graffitiController($scope) {

    $(parent.window).scrollTop(0);

    var url = $.url(parent.document.location);
    GraffitiHelper.Token = url.param("token");
    $scope.token = url.param("token");;

    $scope.attachmentUrl = attachmentUrl;
    $scope.CommentIncreaseCount = GraffitiHelper.CommentIncreaseCount;

    $scope.ContentMessages = LanguageMessages[parent.IndexHelper.LanguageCode];

    $scope.showErrorHeader = false;
    $scope.showLoading = true;
    $scope.showMain = false;
    $scope.errorText = "";

    $scope.showCommentSend = true;
    $scope.sendCommentAreaClass = "selected";
    $scope.sendImgAreaClass = "";

    $scope.GraffitiImageUrl = attachmentUrl + "no_image_available.png";

    $("#fileUpload").change(function () {

        GraffitiHelper.ReadImage(this, function (e) {

            $scope.$apply(function () {
                $scope.GraffitiImageUrl = e.target.result;
            });
        });
    });

    $scope.OpenFileDialog = function () {
        $("#fileUpload").click();
    };

    $scope.RemoveImage = function () {

        GraffitiHelper.ImageFile = null;

        //$scope.$apply(function () {

        $scope.GraffitiImageUrl = attachmentUrl + "no_image_available.png";;
        //});

    };

    parent.IndexHelper.CheckSession(false, $scope.token, function (e) {
        $scope.$apply(function () {
            if (e.Success == true) {

                GraffitiHelper.UserId = e.ReturnObject.PortalUserId;
                GraffitiHelper.PortalId = e.ReturnObject.PortalId;

                $scope.userId = e.ReturnObject.PortalUserId;
                $scope.portalId = e.ReturnObject.PortalId;
            }
            else {
                return;
            }
        });
    });

    GraffitiHelper.GetGraffities(GraffitiHelper.GraffitiCount, (GraffitiHelper.GraffitiCount + GraffitiHelper.IncreaseCount), function (e) {
        $scope.$apply(function () {
            if (e.Success == true) {

                var asd = JSON.stringify(e);

                $scope.graffities = e.ReturnObject;

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

    $scope.GetGraffities = function () {
        GraffitiHelper.GetGraffities(0, (GraffitiHelper.GraffitiCount + GraffitiHelper.IncreaseCount), function (e) {
            $scope.$apply(function () {
                if (e.Success == true) {

                    var asd = JSON.stringify(e);

                    $scope.graffities = e.ReturnObject;

                    setTimeout(function () {
                        parent.IndexHelper.AutoResize("ifrmContent");
                    }, 1000);

                    $scope.showErrorHeader = false;
                    $scope.showLoading = false;
                    $scope.showMain = true;

                }
                else {
                    $scope.showErrorHeader = true;
                    $scope.showLoading = false;
                    $scope.showMain = false;
                    $scope.errorText = ReturnMessage(LoginHelper.LanguageCode, e.Result);
                }
            });
        });
    };

    $scope.sendComment = function (graffitiId, description, commentList) {
        if (description == null || description == "" || description == undefined || description == "undefined") {
            parent.IndexHelper.ToastrShow("warning", ReturnMessage(parent.IndexHelper.LanguageCode, "M087"), "Uyarı");
            return;
        }

        GraffitiHelper.SendComment(description, graffitiId, function (e) {
            $scope.$apply(function () {
                if (e.Success == true) {
                    var elementPos = $scope.graffities.map(function (x) { return x.GraffitiId; }).indexOf(graffitiId);
                    $scope.graffities[elementPos].commentText = null;
                    //ReturnMessage(GraffitiHelper.LanguageCode, e.Result);
                    $scope.GetGraffitiComments(graffitiId, commentList, true);

                    parent.IndexHelper.ToastrShow("success", ReturnMessage(parent.IndexHelper.LanguageCode, e.Result), "Başarılı");
                }
                else {
                    parent.IndexHelper.ToastrShow("error", ReturnMessage(parent.IndexHelper.LanguageCode, e.Result), "Hata");
                }
            });
        });
    };

    $scope.GetGraffitiComments = function (graffitiId, commentList, callFromScript) {

        var elementPos = $scope.graffities.map(function (x) { return x.GraffitiId; }).indexOf(graffitiId);

        var commentCount = commentList.length;
        commentCount++;

        var end;

        if (callFromScript == true) {
            end = commentCount--;
        }
        else {
            end = ((commentCount - 1) + GraffitiHelper.CommentIncreaseCount);
        }

        GraffitiHelper.GetGraffitiComments(graffitiId, 0, end, function (e) {
            $scope.$apply(function () {
                if (e.Success == true) {
                    //alert(ReturnMessage(GraffitiHelper.LanguageCode, e.Result));
                    $scope.graffities[elementPos].CommentList = e.ReturnObject;
                }
                else {
                    alert("ERROR:" + ReturnMessage(GraffitiHelper.LanguageCode, e.Result));
                }
            });
        });
    };

    $scope.CreateGraffiti = function () {

        var hasImage = false;
        var graffitiText = $scope.txtGraffiti;

        if (GraffitiHelper.ImageFile != null) {
            hasImage = true;
        }

        if (hasImage == false && (graffitiText == null || graffitiText == undefined || graffitiText == "")) {
            parent.IndexHelper.ToastrShow("warning", "Duvar yazısı paylaşmak için içerik veya resim içeriği eklemelisiniz.", "Uyarı");

            return;
        }

        GraffitiHelper.CreateGraffiti($scope.txtGraffiti, hasImage, function (e) {

            $scope.$apply(function () {
                if (e.Success == true) {

                    if (hasImage == true) {
                        var formData = new FormData();
                        formData.append('file', GraffitiHelper.ImageFile.blob, GraffitiHelper.ImageFile.fileName);

                        GraffitiHelper.PostFile(formData, e.CrmId, function (data) {

                            if (data.Success == true) {

                                document.location.reload();
                            }
                            else {
                                parent.IndexHelper.ShowNotify(data.Result, "error");
                            }
                        });
                    }
                    else {
                        document.location.reload();
                    }

                    parent.IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M015"), "success");

                }
                else {
                    parent.IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, e.Result), "error");
                }
            });

        });
    };

    $scope.showCommentArea = function (e) {

        $scope.showCommentSend = true;
        $scope.showImgSend = false;

        $scope.sendCommentAreaClass = "selected";
        $scope.sendImgAreaClass = "";
    };

    $scope.showImgArea = function (e) {

        $scope.showImgSend = true;
        $scope.showCommentSend = false;

        $scope.sendCommentAreaClass = "";
        $scope.sendImgAreaClass = "selected";
    };

    $scope.EditUser = function (userId) {

        parent.angular.element('html').scope().EditUser(userId)
    };

    $scope.Like = function (graffitiId) {

        parent.angular.element('html').scope().Like(graffitiId, "new_graffiti", function (e) {

            if (e.Success == true) {
                var elementPos = $scope.graffities.map(function (x) { return x.GraffitiId; }).indexOf(graffitiId);

                $scope.$apply(function () {
                    $scope.graffities[elementPos].LikeCount++;
                });
            }
        });
    };
}