/// <reference path="../jquery/jquery-1.11.1.js" />
/// <reference path="../jquery/purl.js" />
/// <reference path="../jquery/jquery.fileupload.js" />

var FileUploadHelper = {
    "OnLoad": function () {

        var url = $.url(document.location);
        FileUploadHelper.EntityId = url.param("id");
        FileUploadHelper.ObjectTypeName = url.param("typename");
        FileUploadHelper.PortalId = url.param("portalid");

        if (FileUploadHelper.ObjectTypeName == "new_education") {
            FileUploadHelper.RelationName = "new_new_education_new_attachment";
        }
        else if (FileUploadHelper.ObjectTypeName == "new_article") {
            FileUploadHelper.RelationName = "new_new_article_new_attachment";
        }
        else if (FileUploadHelper.ObjectTypeName == "new_announcement") {
            FileUploadHelper.RelationName = "new_new_announcement_new_attachment";
        }


        $("#file").hide();

        $('#file').fileupload({
            url: "FileUploadHandler.ashx?upload=start" + "&operation=1&entityname=" + FileUploadHelper.ObjectTypeName + "&entityid=" + FileUploadHelper.EntityId + "&relationname=" + FileUploadHelper.RelationName + "&portalid=" + FileUploadHelper.PortalId,
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
                console.log('success', response);
            },
            error: function (error) {
                $("#lbl_message").html(error);
                alert(error);
                console.log('error', error);
            }
        });

        FileUploadHelper.OnChangeEvents();
        FileUploadHelper.OnClickEvents();
    },
    "OnClickEvents": function () {

        $("#btn_file").click(function () {
            $("#file").click();
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
    "EntityId": "",
    "PortalId": "",
    "ObjectTypeName": "",
    "RelationName": ""

}
