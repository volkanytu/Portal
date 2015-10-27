/// <reference path="../jquery/jquery-1.11.1.js" />
/// <reference path="../jquery/purl.js" />
/// <reference path="../jquery/jquery.fileupload.js" />

var ProfileImageHelper = {
    "OnLoad": function () {

        var url = $.url(document.location);
        ProfileImageHelper.EntityId = url.param("id");
        ProfileImageHelper.ObjectTypeName = url.param("typename");
        ProfileImageHelper.PortalId = url.param("portalid");
        ProfileImageHelper.ImageName = url.param("imagename");
        ProfileImageHelper.IsImage = url.param("isimage");

        if (ProfileImageHelper.ObjectTypeName == "new_education") {
            ProfileImageHelper.RelationName = "new_new_education_new_attachment";
        }
        else if (ProfileImageHelper.ObjectTypeName == "new_article") {
            ProfileImageHelper.RelationName = "new_new_article_new_attachment";
        }
        else if (ProfileImageHelper.ObjectTypeName == "new_announcement") {
            ProfileImageHelper.RelationName = "new_new_announcement_new_attachment";
        }

        if ((ProfileImageHelper.ObjectTypeName != "new_video" || ProfileImageHelper.IsImage == "true") && ProfileImageHelper.ImageName != null && ProfileImageHelper.ImageName != undefined && ProfileImageHelper.ImageName != "" && ProfileImageHelper.ImageName != "null") {
            $("#imgProfile").show();
            $("#imgProfile").attr("src", attachmentUrl + ProfileImageHelper.ImageName);
            $("#btnDelete").show();
        }
        else {
            $("#imgProfile").hide();
            $("#btnDelete").hide();
        }

        $("#file").hide();

        $('#file').fileupload({
            url: "FileUploadHandler.ashx?upload=start" + "&operation=2&entityname=" + ProfileImageHelper.ObjectTypeName + "&entityid=" + ProfileImageHelper.EntityId + "&relationname=" + ProfileImageHelper.RelationName + "&portalid=" + ProfileImageHelper.PortalId + "&isimage=" + ProfileImageHelper.IsImage,
            add: function (e, data) {
                console.log('add', data);

                var file = data.files[0];

                var htmlText = "";
                htmlText += "<a href='#' style='margin-left:0px;' class='span6 list'>";
                htmlText += "<div class='list-content'>";
                if (file.name.toLowerCase().indexOf(".xls") >= 0 || file.name.toLowerCase().indexOf(".csv") >= 0) {
                    htmlText += "<i class='icon icon-file-excel fg-green'></i>";
                }
                else if (file.name.toLowerCase().indexOf(".doc") >= 0) {
                    htmlText += "<i class='icon icon-file-word fg-blue'></i>";
                }
                else if (file.name.toLowerCase().indexOf(".pdf") >= 0) {
                    htmlText += "<i class='icon icon-file-pdf fg-red'></i>";
                }
                else if (file.name.toLowerCase().indexOf(".zip") >= 0 || file.name.toLowerCase().indexOf(".rar") >= 0) {
                    htmlText += "<i class='icon icon-file-zip fg-red'></i>";
                }
                else if (file.name.toLowerCase().indexOf(".jpg") >= 0 || file.name.toLowerCase().indexOf(".jpeg") >= 0 || file.name.toLowerCase().indexOf(".png") >= 0 || file.name.toLowerCase().indexOf(".gif") >= 0) {
                    htmlText += "<i class='icon icon-image'></i>";
                }
                else {
                    htmlText += "<i class='icon icon-file'></i>";
                }

                htmlText += "<div name='fileInformation' class='data'>";
                htmlText += "<span class='list-title'>" + file.name + "</span>";
                htmlText += "<div name='progressBar' class='progress-bar small' data-role='progress-bar' data-value='0' data-color='bg-green'></div>";
                htmlText += "<span name='progress' class='list-remark'></span>";
                //htmlText += "<span class='list-remark'><button class='warning place-right' onclick='$(this).parent().parent().parent().parent().remove();'>Sil</button></span>";
                htmlText += "</div>";
                htmlText += "</div>";
                htmlText += "</a>";

                $("#lstView").append(htmlText);

                $("div[name='progressBar']").progressbar();

                data.submit();
            },
            progress: function (e, data) {
                var progress = parseInt(data.loaded / data.total * 100, 10);

                var pb = $("div[name='progressBar']").progressbar();

                pb.progressbar('value', progress);
                $("span[name='progress']").html("%" + progress + " tamamlandı");

            },
            success: function (response, status) {
                //$('#progressbar').hide();
                //$('#progressbar div').css('width', '0%');
                //alert(response);

                if (ProfileImageHelper.ObjectTypeName != "new_video" || ProfileImageHelper.IsImage == "true") {

                    ProfileImageHelper.ImageName = response;

                    $("#imgProfile").show();
                    $("#imgProfile").attr("src", attachmentUrl + ProfileImageHelper.ImageName);
                    $("#btnDelete").show();
                }

                console.log('success', response);
            },
            error: function (error) {
                $("#lbl_message").html(error);
                alert(error);
                console.log('error', error);
            }
        });

        ProfileImageHelper.OnChangeEvents();
        ProfileImageHelper.OnClickEvents();
    },
    "OnClickEvents": function () {

        $("#btn_file").click(function () {
            $("#file").click();
        });

        $("#btnDelete").click(function () {

            ProfileImageHelper.DeleteProfilImage();

        });

    },
    "OnChangeEvents": function () {

        $("#file").change(function () {

            //$("#lstView").html("");
            //if (this.files.length == 0) {
            //    $("#lbl_message").text("Lütfen dosya seçiniz!");
            //    return;
            //}

            //$("#txt_file").val(this.files[0].name);
            //$("#lbl_message").text("");

        });

    },
    "DeleteProfilImage": function () {
        //debugger;
        $.ajax({
            url: "FileUploadHandler.ashx?upload=start" + "&operation=3&entityname=" + ProfileImageHelper.ObjectTypeName + "&entityid=" + ProfileImageHelper.EntityId + "&relationname=" + ProfileImageHelper.RelationName + "&portalid=" + ProfileImageHelper.PortalId,
            async: true,
            type: "POST",
            beforeSend: function () {

            },
            complete: function () {

            },
            success: function (data) {
                //debugger;
                if (data != null) {
                    if (data == "success") {
                        $("#imgProfile").hide();
                        $("#btnDelete").hide();
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
    "EntityId": "",
    "PortalId": "",
    "ObjectTypeName": "",
    "RelationName": "",
    "ImageName": "",
    "IsImage": ""
}
