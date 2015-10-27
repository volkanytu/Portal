/// <reference path="../jquery/jquery.min.js" />
/// <reference path="../jquery/jquery.widget.min.js" />
/// <reference path="../jquery/purl.js" />
/// <reference path="GlobalVariablesJS.js" />
/// <reference path="GlobalVariables.js" />

var ObjectProfileHelper = {
    "OnLoad": function () {

        var url = $.url(document.location);
        ObjectProfileHelper.EntityId = url.param("id");
        ObjectProfileHelper.EntityId.replace("{", "").replace("}", "");
        ObjectProfileHelper.ObjectTypeName = url.param("typename");
        ObjectProfileHelper.IsImage = url.param("isimage");
        ObjectProfileHelper.ImageName = url.param("imagename");

        if (ObjectProfileHelper.ObjectTypeName == "new_video" && ObjectProfileHelper.IsImage != "true") {
            ObjectProfileHelper.FieldName = "new_videourl";
        }
        else {
            ObjectProfileHelper.FieldName = "new_imageurl";
        }


        //ObjectProfileHelper.ImageName = parent.window.Xrm.Page.getAttribute(ObjectProfileHelper.FieldName).getValue();

        if (ObjectProfileHelper.ImageName != null && ObjectProfileHelper.ImageName != "" && ObjectProfileHelper.ImageName != "null" && ObjectProfileHelper.FieldName == "new_imageurl") {
            $("#imgProfile").show();
            $("#imgProfile").attr("src", attachmentUrl + ObjectProfileHelper.ImageName);
            $("#btnDelete").show();
        }
        else {
            $("#imgProfile").hide();
            $("#btnDelete").hide();
        }

        $("#file").hide();

        ObjectProfileHelper.OnClickEvents();
        ObjectProfileHelper.OnChangeEvents();

    },
    "OnClickEvents": function () {

        $("#btnDelete").click(function () {
            //parent.window.Xrm.Page.getAttribute(ObjectProfileHelper.FieldName).setValue(null);

            ObjectProfileHelper.DeleteProfilImage(true);

        });

        $("#btn_import").click(function () {

            ObjectProfileHelper.UploadProcess();
        });

        $("#btn_file").click(function () {
            var listViewCount = $("#lstView a").length;

            if (listViewCount >= 1) {
                $("#lbl_message").text((ObjectProfileHelper.FieldName != "new_imageurl" ? "Video" : "Profil resmi") + "için sadece bir dosyaya izin verilir!");
                return;
            }

            $("#file").click();
        });
    },
    "OnChangeEvents": function () {
        $("#file").change(function () {

            if (this.files.length == 0) {
                $("#lbl_message").text("Lütfen dosya seçiniz!");
                return;
            }

            $("#txt_file").val(this.files[0].name);
            $("#lbl_message").text("");

            ObjectProfileHelper.AddFile();
        });
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

            ObjectProfileHelper.UploadToCrm(row);
        });
    },
    "AddFile": function () {
        var filepath = $("#txt_file").val();
        if (filepath != null && filepath != "") {
            var file = $("#file")[0].files[0];

            var file = $("#file")[0].files[0];
            var size = (file.size / 1024) / 1024;
            if (size > 4 && ObjectProfileHelper.FieldName == "new_imageurl") {
                $("#lbl_message").html("Maksimum dosya boyutu 4MB!");
                return false;
            }

            var reader = new FileReader();

            reader.onloadend = function () {
                var data = reader.result;
                data = data.substr(data.indexOf('base64') + 7);

                if (file.name.toLowerCase().indexOf(".mp4") < 0 && ObjectProfileHelper.FieldName != "new_imageurl") {
                    $("#lbl_message").text("Lütfen MP4 video formatında dosya seçiniz!");
                    return;
                }

                var htmlText = "";
                htmlText += "<a href='#' style='margin-left:0px;' class='span6 list' fileData='" + data + "' mimeType='" + file.type + "' fileName='" + file.name + "'>";
                htmlText += "<div class='list-content'>";
                if (file.name.toLowerCase().indexOf(".xls") >= 0 || file.name.toLowerCase().indexOf(".csv") >= 0) {
                    htmlText += "<i class='icon icon-file-excel fg-green'></i>";

                    if (ObjectProfileHelper.FieldName == "new_imageurl") {
                        $("#lbl_message").text("Lütfen resim dosyası seçiniz!");
                        return;
                    }
                }
                else if (file.name.toLowerCase().indexOf(".doc") >= 0) {
                    htmlText += "<i class='icon icon-file-word fg-blue'></i>";

                    if (ObjectProfileHelper.FieldName == "new_imageurl") {
                        $("#lbl_message").text("Lütfen resim dosyası seçiniz!");
                        return;
                    }
                }
                else if (file.name.toLowerCase().indexOf(".pdf") >= 0) {
                    htmlText += "<i class='icon icon-file-pdf fg-red'></i>";

                    if (ObjectProfileHelper.FieldName == "new_imageurl") {
                        $("#lbl_message").text("Lütfen resim dosyası seçiniz!");
                        return;
                    }

                }
                else if (file.name.toLowerCase().indexOf(".zip") >= 0 || file.name.toLowerCase().indexOf(".rar") >= 0) {
                    htmlText += "<i class='icon icon-file-zip fg-red'></i>";

                    if (ObjectProfileHelper.FieldName == "new_imageurl") {
                        $("#lbl_message").text("Lütfen resim dosyası seçiniz!");
                        return;
                    }
                }
                else if (file.name.toLowerCase().indexOf(".jpg") >= 0 || file.name.toLowerCase().indexOf(".jpeg") >= 0 || file.name.toLowerCase().indexOf(".png") >= 0 || file.name.toLowerCase().indexOf(".gif") >= 0) {
                    htmlText += "<i class='icon icon-image'></i>";
                }
                else {
                    htmlText += "<i class='icon icon-file'></i>";

                    if (ObjectProfileHelper.FieldName == "new_imageurl") {
                        $("#lbl_message").text("Lütfen resim dosyası seçiniz!");
                        return;
                    }
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
            $("#lbl_message").text("Lütfen dosya seçiniz!");
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
            if (ObjectProfileHelper.EntityId != "") {
                jData.note.Object = {};
                jData.note.Object.LogicalName = ObjectProfileHelper.ObjectTypeName;
                jData.note.Object.Id = ObjectProfileHelper.EntityId;

            }

            jData.note.MimeType = mimeType;
            jData.note.FileName = name;
            jData.note.File = data;

            jData.relationName = ObjectProfileHelper.FieldName;

            var jSonData = JSON.stringify(jData);
            //debugger;
            $.ajax({
                url: "upload.ashx?operation=2",
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
                                    $(row).find("span[name='progress']").html("%" + progress + " tamanlandı");
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
                                                //parent.window.Xrm.Page.getAttribute(ObjectProfileHelper.FieldName).setValue(data.Result);

                                                ObjectProfileHelper.ImageName = data.Result;

                                                $("#imgProfile").show();
                                                $("#imgProfile").attr("src", attachmentUrl + data.Result);
                                                $("#btnDelete").show();

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
    "DeleteProfilImage": function (directDelete) {
        //debugger;
        $.ajax({
            url: "upload.ashx?operation=3",
            async: true,
            dataType: "json",
            type: "POST",
            data: {
                entityId: ObjectProfileHelper.EntityId,
                entityName: ObjectProfileHelper.ObjectTypeName,
                fieldName: ObjectProfileHelper.FieldName,
                fileName: ObjectProfileHelper.ImageName,
                directDelete: directDelete
            },
            beforeSend: function () {

            },
            complete: function () {

            },
            success: function (data) {
                //debugger;
                if (data != null) {
                    if (data.Success == true) {
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
    "ObjectTypeName": "",
    "FieldName": "",
    "ImageName": "",
    "IsImage": ""
};

function getRandomNumber(min, max) {
    return Math.floor(Math.random() * (max - min + 1) + min);
}