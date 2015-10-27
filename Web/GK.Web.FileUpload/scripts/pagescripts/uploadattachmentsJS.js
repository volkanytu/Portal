/// <reference path="../jquery/jquery.min.js" />
/// <reference path="../jquery/jquery.widget.min.js" />
/// <reference path="../jquery/purl.js" />
/// <reference path="GlobalVariables.js" />

var FileUploadHelper = {
    "OnLoad": function () {

        var url = $.url(document.location);
        FileUploadHelper.EntityId = url.param("id");
        FileUploadHelper.ObjectTypeName = url.param("typename");
        FileUploadHelper.PortalId = url.param("portalid");

        //var brand = parent.window.Xrm.Page.getAttribute("new_portalid").getValue();
        //FileUploadHelper.PortalId = brand[0].id.replace('{', '').replace('}', '');
        //FileUploadHelper.PortalId = '41B3018E-AB09-E411-80CC-000C2961A080';

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

        FileUploadHelper.OnClickEvents();

        $("#btn_file").click(function () {
            $("#file").click();
        });

        $("#file").change(function () {

            if (this.files.length == 0) {
                $("#lbl_message").text("Lütfen dosya seçiniz!");
                return;
            }

            $("#txt_file").val(this.files[0].name);
            $("#lbl_message").text("");

            FileUploadHelper.AddFile();
        });

    },
    "OnClickEvents": function () {
        $("#btn_import").click(function () {

            FileUploadHelper.UploadProcess();
        });
    },
    "OnChangeEvents": function () {

    },
    "UploadProcess": function () {
        var hasError = false;
        var listViewCount = $("#lstView a").length;

        if (listViewCount === 0) {

            $("#lbl_message").text("Lütfen dosya seçiniz!");
            return;
        }

        $("#lstView a[isSuccess != '1']").each(function (i) {
            var row = $(this);

            FileUploadHelper.UploadToCrm(row);
        });
    },
    "AddFile": function () {
        var filepath = $("#txt_file").val();
        if (filepath != null && filepath != "") {
            var file = $("#file")[0].files[0];

            var file = $("#file")[0].files[0];
            var size = (file.size / 1024) / 1024;
            //if (size > 4) {
            //    $("#lbl_message").html("Seçilen dosya 4 MB'dan büyük olamaz!");
            //    return false;
            //}

            var reader = new FileReader();

            reader.onloadend = function () {
                var data = reader.result;
                data = data.substr(data.indexOf('base64') + 7);

                var htmlText = "";
                htmlText += "<a href='#' style='margin-left:0px;' class='span6 list' fileData='" + data + "' mimeType='" + file.type + "' fileName='" + file.name + "'>";
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
                htmlText += "<span class='list-remark'><button class='warning place-right' onclick='$(this).parent().parent().parent().parent().remove();'>Sil</button></span>";
                htmlText += "</div>";
                htmlText += "</div>";
                htmlText += "</a>";

                $("#lstView").append(htmlText);

                $("div[name='progressBar']").progressbar();
                $("#txt_file").val("");
            }

            reader.readAsDataURL(file);
        }
        else {
            $("#lbl_message").text("Lütfen bir dosya seçiniz...");
        }
    },
    "UploadToCrm": function (row) {
        try {
            var intervalBefore;
            var intervalSuccess;
            var progress = 0;
            var hasError = false;


            var randomNumber = getRandomNumber(50, 70);
            var data = $(row).attr('fileData');
            var mimeType = $(row).attr('mimeType');
            var name = $(row).attr('fileName');

            var jData = {};
            jData.note = {};
            if (FileUploadHelper.PortalId != "") {
                jData.note.Portal = {};
                jData.note.Portal.LogicalName = "new_portal";
                jData.note.Portal.Id = FileUploadHelper.PortalId;

            }
            if (FileUploadHelper.EntityId != "") {
                jData.note.Object = {};
                jData.note.Object.LogicalName = FileUploadHelper.ObjectTypeName;
                jData.note.Object.Id = FileUploadHelper.EntityId;

            }

            jData.note.MimeType = mimeType;
            jData.note.FileName = name;
            jData.note.File = data;
            jData.relationName = FileUploadHelper.RelationName;

            var jSonData = JSON.stringify(jData);
            //debugger;
            $.ajax({
                url: "upload.ashx?operation=1",
                async: true,
                dataType: "json",
                type: "POST",
                data: {
                    jsonData: jSonData
                },
                beforeSend: function () {
                    var pb = $(row).find("div[name='progressBar']").progressbar();
                    var intervalBefore = setInterval(
                                function () {
                                    pb.progressbar('value', (++progress));
                                    $(row).find("span[name='progress']").html("%" + progress + " tamamlandı");
                                    if (progress >= randomNumber || hasError) {
                                        window.clearInterval(intervalBefore);
                                    }
                                }, 100);
                },
                complete: function () {
                    $(row).attr('isSuccess', '1');
                    window.clearInterval(intervalBefore);
                    window.clearInterval(intervalSuccess);
                },
                success: function (data) {
                    //debugger;
                    if (data != null) {
                        if (data.Success == true) {
                            hasError = false;
                            var pb = $(row).find("div[name='progressBar']").progressbar();

                            if (progress < randomNumber) {
                                window.clearInterval(intervalBefore);
                                randomNumber = progress;
                            }
                            var intervalSuccess = setInterval(
                                        function () {
                                            pb.progressbar('value', (++progress));
                                            if (progress >= 100 || hasError) {
                                                window.clearInterval(intervalSuccess);
                                                $(row).find("span[name='progress']").html("%" + progress + " tamamlandı");
                                                //var control = parent.window.Xrm.Page.ui.controls.get("attachments");
                                                //control.refresh();
                                            }
                                        }, 50);
                        }
                        else {
                            $(row).addClass('bg-red fg-white');
                            hasError = true;
                            alert(data.Result);
                            return;
                        }
                    }
                    else {
                        $(row).addClass('bg-red fg-white');
                        hasError = true;
                    }
                },
                error: function (a, b, c) {
                    $(row).addClass('bg-red fg-white');
                    hasError = true;
                }
            });
        } catch (e) {

        }
    },
    "EntityId": "",
    "PortalId": "",
    "ObjectTypeName": "",
    "RelationName": "",
};

function getRandomNumber(min, max) {
    return Math.floor(Math.random() * (max - min + 1) + min);
}