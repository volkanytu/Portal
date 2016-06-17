/// <reference path="../jquery/jquery.widget.min.js" />
/// <reference path="../jquery/purl.js" />
/// <reference path="GlobalVariables.js" />
/// <reference path="../jquery/jquery.session.js" />
/// <reference path="languageMessagesJS.js" />

var bodyHolderHeight;
var isOnload = true;

var IndexHelper = {
    "OnLoad": function () {

        $(window).scroll(function () {

            var isFocused = $("#txtCode").is(":focus");

            if (isFocused == true) {
                return;
            }

            if ($(window).scrollTop() + 3 >= $(document).height() - $(window).height()) {

                if (IndexHelper.CanScrollEvent == true) {
                    IndexHelper.CanScrollEvent = false;

                    var ifmUrl = $.url($('#ifrmContent')[0].contentWindow.location);
                    var page = ifmUrl.data.attr.file;

                    if (page == "duvarYazisi.html") {
                        var graffitiCount = $('#ifrmContent')[0].contentWindow.GraffitiHelper.GraffitiCount;
                        var increaseCount = $('#ifrmContent')[0].contentWindow.GraffitiHelper.IncreaseCount;

                        $('#ifrmContent')[0].contentWindow.GraffitiHelper.GraffitiCount = graffitiCount + increaseCount;

                        $("#loadGraffiti", $("#ifrmContent", parent.document)[0].contentWindow.document).click();
                    }
                }
            }
        });

        bodyHolderHeight = document.getElementById("bodyHolder").scrollHeight;

        //IndexHelper.GetRecordCounts();

        IndexHelper.OnClickEvents();
        IndexHelper.OnChangeEvents();
        IndexHelper.OnPageUnLoad();

    },
    "CheckSession": function (isParent, token, callbackFunction) {
        //debugger;
        var jData = {};
        jData.token = token;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/CheckSession",
            async: true,
            dataType: "json",
            contentType: "application/json;",
            type: "POST",
            data: jSonData,
            beforeSend: function () {
                //IndexHelper.ShowLoading("Oturum bilgisi kontrol ediliyor...", null);
            },
            complete: function () {
                //IndexHelper.CloseDialog();
            },
            success: function (data) {
                //debugger;
                if (data != null) {
                    data = JSON.parse(data);

                    if (data.Success) {

                        callbackFunction(data);
                    }
                    else {
                        if (isParent == false) {
                            //IndexHelper.ShowTimeoutAlert(function () {
                            document.location.href = "login.html";
                            //});

                        }
                        else {

                            //parent.IndexHelper.ShowTimeoutAlert(function () {
                            document.location.href = "login.html";
                            //});
                        }
                        return;
                    }
                }
                else {
                    if (isParent == false) {
                        IndexHelper.ShowAlertDialog(3, "HATA", ReturnMessage(IndexHelper.LanguageCode, "M002") + "-CheckSession", function () {
                            document.location.href = "index.html";
                        });
                    }
                    else {
                        parent.IndexHelper.ShowAlertDialog(3, "HATA", ReturnMessage(IndexHelper.LanguageCode, "M002") + "-CheckSession", function () {
                            document.location.href = "index.html";
                        });
                    }
                    return;
                }
            },
            error: function (a, b, c) {
                debugger;
                if (isParent == false) {
                    IndexHelper.ShowAlertDialog(3, "HATA", "Hata:" + c + "-CheckSession", function () {
                        document.location.href = "index.html";
                    });
                }
                else {
                    parent.IndexHelper.ShowAlertDialog(3, "HATA", "Hata:" + c + "-CheckSession", function () {
                        document.location.href = "index.html";
                    });
                }
                return;
            }
        });
    },
    "OnPageUnLoad": function () {

        window.onbeforeunload = function () {

            IndexHelper.UpdateLogoutDate();

        };
    },
    "OnClickEvents": function () {

    },
    "OnChangeEvents": function () {

    },
    "AutoResize": function (id) {
        try {
            var windowHeight = $(window).height();

            if (isOnload == true) {
                bodyHolderHeight = document.getElementById("bodyHolder").scrollHeight;
                isOnload = false;
            }
            var newheight;
            var newwidth;
            //debugger;
            if (document.getElementById) {
                newwidth = document.getElementById(id).contentWindow.document.body.scrollWidth;
                newheight = document.getElementById(id).contentWindow.document.getElementById("divMain").scrollHeight
            }

            if (newheight < windowHeight) {
                newheight = windowHeight - 60;
            }

            document.getElementById(id).height = (newheight + 30) + "px";

        } catch (e) {

        }
    },
    "GetUserInfo": function (token, callBackFunction) {

        //debugger;
        var jData = {};
        jData.token = token;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/GetUserInfo",
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
                    IndexHelper.ToastrShow("error", ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />GetUserInfo", "Hata");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                $("#lblErrorMain").show();

                IndexHelper.ToastrShow("error", c + "<br />GetUserInfo", "Hata");
                return false;
            }
        });
    },
    "ShowNotify": function (message, type) {
        //Types:success,info,warn,error

        $.notify(message, type);
    },
    "ToastrShow": function (type, message, title) {

        //Types:success,info,warning,error

        toastr.options = {
            "closeButton": true,
            "debug": false,
            "positionClass": "toast-top-right",
            "onclick": null,
            "showDuration": "1000",
            "hideDuration": "5000",
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "progressBar": true,
            "hideMethod": "fadeOut"
        }

        toastr[type](message, title);

    },
    "ShowDialog": function ShowDialog(html, title, className, showCloseButton, closeCallBackFunction) {
        bootbox.dialog({
            message: html,
            title: title,
            className: className,
            closeButton: showCloseButton == null ? true : showCloseButton,
            onEscape: closeCallBackFunction
        });
    },
    "ShowLoading": function (message, title) {

        var html = "";
        html += "<div id='divLoadingMain' ng-show='showLoading' style='text-align:center;'>";
        html += "	<img src='images/loading.gif' width='48' />";
        html += "	<br />";
        html += "	<br />";
        html += "	<h4 style='text-align:center;'>" + message + "</h4>";
        html += "</div>";

        IndexHelper.ShowDialog(html, title, "modal20", false);
    },
    "CloseDialog": function () {

        bootbox.hideAll();
    },
    "GetTopAnnouncements": function (start, end) {
        $("#lblErrorAnnounce").hide();
        $("#carouselAnnounce").html("");

        //debugger;
        var jData = {};
        jData.token = IndexHelper.Token;
        jData.start = start;
        jData.end = end;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/GetAnnouncementList",
            async: true,
            dataType: "json",
            contentType: "application/json;",
            type: "POST",
            data: jSonData,
            beforeSend: function () {
                $("#divLoadingAnnounce").show();
                $("#carouselAnnounce").hide();
            },
            complete: function () {
                $("#divLoadingAnnounce").hide();
            },
            success: function (data) {
                //debugger;
                if (data != null) {
                    data = JSON.parse(data);

                    if (data.Success) {
                        $("#carouselAnnounce").show();
                    }
                    else {
                        $("#lblErrorAnnounce").show();
                        $("#lblErrorAnnounce").html(ReturnMessage(IndexHelper.LanguageCode, data.Result));

                        return;
                    }

                    var html = "";

                    if (data.ReturnObject != null && data.ReturnObject.length == 0) {
                        $("#lblErrorAnnounce").show();
                        $("#lblErrorAnnounce").html("Duyuru yok");
                    }
                    else {
                        for (var i = 0; i < data.ReturnObject.length; i++) {
                            var strLimit = 300;

                            html += "<div class='slide'>";
                            html += "<div class='panel-content' id='" + data.ReturnObject[i].AnnouncementId + "'>";

                            if (data.ReturnObject[i].ImagePath != null && data.ReturnObject[i].ImagePath != "" && data.ReturnObject[i].ImagePath != "no_image_available.png") {
                                strLimit = 75;
                                html += "<img src='" + attachmentUrl + data.ReturnObject[i].ImagePath + "' class='span3' style='height: 175px !important; margin-bottom: 10px;' />";
                            }
                            html += "<p class='tertiary-text' style='margin-top: 10px !important; margin-bottom: 2px;' title='" + data.ReturnObject[i].Caption + "'><strong><small>" + data.ReturnObject[i].Caption.substring(0, 45) + (data.ReturnObject[i].Caption.length < 45 ? "" : "...") + "</small></strong></p>";
                            html += "<p class='tertiary-text-secondary place icon-calendar' style='margin-top: -5px !important;'> &nbsp;<small>" + data.ReturnObject[i].CreatedOnString + "</small></p>";
                            //html += "<p class='tertiary-text-secondary' title='" + data.ReturnObject[i].Description + "'>" + data.ReturnObject[i].Description.substring(0, strLimit) + (data.ReturnObject[i].Description.length < strLimit ? "" : "...") + "</p>";
                            html += "</div>";
                            html += "</div>";
                        }

                        $("#carouselAnnounce").html(html);

                        $("#carouselAnnounce").carousel({
                            height: 270,
                            duration: 500,
                            period: 4000,
                            effect: 'slide',
                            markers: {
                                show: true,
                                type: 'default',
                                position: 'bottom-right'
                            }
                        });

                        $("#carouselAnnounce .panel-content").click(function () {
                            //$("a[type='menu']").addClass("fg-white").removeClass("bg-white");
                            //$("#btnAnnounce").removeClass("fg-white").addClass("fg-black").addClass("bg-white");
                            $("a[type='menu']").removeClass("bg-white");

                            var id = $(this).attr("id");
                            $("#ifrmContent", parent.document).attr("src", "editannouncement.aspx?objectid=" + id + "&portalid=" + IndexHelper.PortalId + "&userid=" + IndexHelper.UserId);
                        });
                    }
                }
                else {
                    $("#lblErrorAnnounce").show();
                    $("#lblErrorAnnounce").html(ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />GetTopAnnouncements");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                IndexHelper.ShowAlertDialog(3, "Hata", "Hata:" + c + "<br />GetTopAnnouncements");
            }
        });
    },
    "GetTopVideos": function (start, end) {
        $("#lblErrorVideo").hide();
        $("#carouselVideo").html("");

        //debugger;
        var jData = {};
        jData.token = IndexHelper.Token;
        jData.start = start;
        jData.end = end;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/GetVideoList",
            async: true,
            dataType: "json",
            contentType: "application/json;",
            type: "POST",
            data: jSonData,
            beforeSend: function () {
                $("#divLoadingVideo").show();
                $("#carouselVideo").hide();
            },
            complete: function () {
                $("#divLoadingVideo").hide();
            },
            success: function (data) {
                //debugger;
                if (data != null) {
                    data = JSON.parse(data);

                    if (data.Success) {
                        $("#carouselVideo").show();
                    }
                    else {
                        $("#lblErrorVideo").show();
                        $("#lblErrorVideo").html(ReturnMessage(IndexHelper.LanguageCode, data.Result));

                        return;
                    }

                    var html = "";

                    if (data.ReturnObject != null && data.ReturnObject.length == 0) {
                        $("#lblErrorVideo").show();
                        $("#lblErrorVideo").html(ReturnMessage(IndexHelper.LanguageCode, "M022"));
                    }
                    else {
                        for (var i = 0; i < data.ReturnObject.length; i++) {
                            html += " <div class='slide'>";
                            html += "<div class='tile bg-steel double double-vertical' id='" + data.ReturnObject[i].Id + "' videopath='" + data.ReturnObject[i].VideoPath + "' style='width:220px;'>";
                            html += "	<div class='tile-content icon' " + (data.ReturnObject[i].ImagePath != "" ? "style='background: url(" + attachmentUrl + data.ReturnObject[i].ImagePath + ") no-repeat; background-size: 220px 250px;'" : "") + " >";
                            html += "		<div class='text-right ntp bg-black' style='padding-right:10px;'>";
                            html += "			<p class='fg-white'><small>" + data.ReturnObject[i].CreatedOnString + "</small></p>";
                            html += "		</div>";
                            html += "		<span class='icon-play-alt'></span>";
                            html += "	</div>";
                            html += "	<div class='tile-status bg-black'>";
                            html += "		<span class='name'><strong type='name' class='fg-white'>" + data.ReturnObject[i].Name + "</strong> </span>";
                            html += "	</div>";
                            html += "</div>";
                            html += "</div>";
                        }

                        $("#carouselVideo").html(html);

                        $("#carouselVideo").carousel({
                            height: 275,
                            duration: 500,
                            period: 4000,
                            effect: 'slide',
                            markers: {
                                show: true,
                                type: 'default',
                                position: 'bottom-right'
                            }
                        });

                        $("#carouselVideo .tile").click(function () {
                            var id = $(this).attr("id");
                            var videoPath = $(this).attr("videopath");
                            var name = $("strong[type='name']").html();


                            $("a[type='menu']").addClass("fg-white").removeClass("bg-white");
                            $("#btnVideo").removeClass("fg-white").addClass("fg-black").addClass("bg-white");

                            //IndexHelper.LogVideo(id);

                            //parent.IndexHelper.ShowDialog(name, "<video class='span7' height='300' controls><source src='attachments/" + videoPath + "' type='video/mp4;' />Tarayıcınız video tagini desteklemiyor.</video>");
                            $("#ifrmContent", parent.document).attr("src", "editvideo.aspx?objectid=" + id + "&portalid=" + IndexHelper.PortalId + "&userid=" + IndexHelper.UserId);

                        });

                    }
                }
                else {
                    $("#lblErrorVideo").show();
                    $("#lblErrorVideo").html(ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />GetTopVideos");
                    return false;
                }
            },
            error: function (a, b, c) {
                //debugger;
                IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), c + "<br />GetTopVideos");
            }
        });
    },
    "GetUserCubeStatus": function () {
        var jData = {};
        jData.token = IndexHelper.Token;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/GetUserCubeStatus",
            async: true,
            dataType: "json",
            contentType: "application/json;",
            type: "POST",
            data: jSonData,
            beforeSend: function () {
                $("#divLoadingCube").show();
                $("#cubeInfo").hide();
            },
            complete: function () {
                $("#divLoadingCube").hide();
                $("#cubeInfo").show();
            },
            success: function (data) {
                //debugger;
                if (data != null) {
                    data = JSON.parse(data);
                    //debugger;
                    if (data.Success) {

                        $("#lblPoint").html(data.ReturnObject.Point);
                        $("#lblRanking").html(data.ReturnObject.Rank + ".");
                    }
                    else {
                        IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M059"), ReturnMessage(IndexHelper.LanguageCode, data.Result), "red", "white");
                        return false;
                    }
                }
                else {
                    IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M059"), ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />GetUserCubeStatus", "red", "white");
                    return false;
                }
            },
            error: function (a, b, c) {
                IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M059"), c + "<br />GetUserCubeStatus", "red", "white");
                return false;
            }
        });
    },
    "UpdateLogoutDate": function () {
        var jData = {};
        jData.loginLogId = IndexHelper.LoginLogId;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/UpdateLogoutDate",
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
                    if (data.Success) {

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
    "LogVideo": function (videoId) {
        var jData = {};
        jData.token = IndexHelper.Token;
        jData.videoId = videoId;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/LogVideo",
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
                    if (data.Success) {

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
    "OpenEditUserPage": function OpenPriceListPage(title, iframeSrcUrl) {
        $.Dialog({
            overlay: true,
            shadow: true,
            flat: true,
            width: 600,
            icon: '<i class="icon-user"></i>',
            title: "<strong>" + title + "</strong>",
            content: "<iframe src='" + iframeSrcUrl + "' width='600' height='500' style='border:none;margin:10px;' scrolling='no'></iframe>",

            onShow: function (_dialog) {
            }

        });
    },
    "OpenAnswerForum": function OpenPriceListPage(title, iframeSrcUrl) {
        $.Dialog({
            overlay: true,
            shadow: true,
            flat: true,
            width: 600,
            icon: '<i class="icon-pencil"></i>',
            title: "<strong>" + title + "</strong>",
            content: "<iframe src='" + iframeSrcUrl + "' width='600' height='300' style='border:none;margin:10px;' scrolling='no'></iframe>",

            onShow: function (_dialog) {
            }

        });
    },
    "OpenAddForumSubject": function (title, iframeSrcUrl) {
        $.Dialog({
            overlay: true,
            shadow: true,
            flat: true,
            width: 600,
            icon: '<i class="icon-pencil"></i>',
            title: "<strong>" + title + "</strong>",
            content: "<iframe src='" + iframeSrcUrl + "' width='600' height='400' style='border:none;margin:10px;' scrolling='no'></iframe>",

            onShow: function (_dialog) {
            }

        });
    },
    "OpenWelcomePage": function (closefunction) {
        $.Dialog({
            overlay: true,
            shadow: true,
            flat: true,
            width: 600,
            overlayClickClose: false,
            sysBtnCloseClick: closefunction,
            icon: '<i class="icon-checkmark"></i>',
            title: "<strong>Hoş geldin</strong>",
            content: "<iframe src='welcome.aspx' width='600' height='400' style='border:none;margin:10px;' scrolling='yes'></iframe>",

            onShow: function (_dialog) {
            }

        });
    },
    "OpenContractPage": function (closefunction) {
        //$.Dialog({
        //    overlay: true,
        //    shadow: true,
        //    flat: true,
        //    width: 600,
        //    overlayClickClose: false,
        //    sysBtnCloseClick: closefunction,
        //    icon: '<i class="icon-copy"></i>',
        //    title: "<strong>Gizlilik Sözleşmesi</strong>",
        //    content: "<iframe src='approvecontract.aspx' width='600' height='350' style='border:none;margin:10px;' scrolling='yes'></iframe>",

        //    onShow: function (_dialog) {
        //    }

        //});

        $.DialogDogus({
            overlay: true,
            shadow: true,
            flat: true,
            width: 600,
            overlayClickClose: false,
            sysBtnCloseClick: closefunction,
            //icon: '<i class="icon-copy"></i>',
            title: "<strong>Gizlilik Sözleşmesi</strong>",
            content: "<iframe class='bg-magenta' src='contract.html?token=" + IndexHelper.Token + "' width='520'  style='background-color:#d80073 !important;border:none;margin:10px;' scrolling='no'></iframe>",

            onShow: function (_dialog) {
            }

        });
    },
    "UpdateWelcomePageGenerated": function () {
        var jData = {};
        jData.token = IndexHelper.Token;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/UpdateWelcomePageGenerated",
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
                    if (data.Success) {

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
    "UpdateContractApprove": function () {
        var jData = {};
        jData.token = IndexHelper.Token;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/UpdateContractApprove",
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
                    if (data.Success) {
                        document.location.reload();
                        $.Dialog.close();
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
    "SelectSurvey": function () {
        $("#lblErrorSurvey").hide();
        $("#divSurvey").html("");

        //debugger;
        var jData = {};
        jData.token = IndexHelper.Token;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/SelectSurvey",
            async: true,
            dataType: "json",
            contentType: "application/json;",
            type: "POST",
            data: jSonData,
            beforeSend: function () {
                $("#rowSurvey").hide();
            },
            complete: function () {
            },
            success: function (data) {
                //debugger;
                if (data != null) {
                    data = JSON.parse(data);

                    if (data.Success) {
                        $("#rowSurvey").show();
                    }
                    else {
                        //parent.IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M060"), ReturnMessage(IndexHelper.LanguageCode, data.Result), "red", "white");
                        return;
                    }

                    var html = "";

                    if (data.ReturnObject != null && data.ReturnObject.length == 0) {

                    }
                    else {
                        html += "<p class='tertiary-text'>" + data.ReturnObject.Name + "</p>";

                        html += "<div class='grid'>";
                        if (data.ReturnObject.SurveyChoices != null && data.ReturnObject.SurveyChoices.length > 0) {
                            for (var i = 0; i < data.ReturnObject.SurveyChoices.length; i++) {
                                html += "<div class='row' style='margin-top:0px;'>";
                                html += "<div class='span3'>";
                                html += "<div class='input-control radio'>";
                                html += "	<label>";
                                html += "		<input type='radio' value='" + data.ReturnObject.SurveyChoices[i].Id + "' text='" + data.ReturnObject.SurveyChoices[i].Name + "' name='survey' />";
                                html += "		<span class='check'></span>";
                                html += data.ReturnObject.SurveyChoices[i].Name;
                                html += "	</label>";
                                html += "</div>";
                                html += "</div>";
                                html += "</div>";
                            }

                            html += "<div class='row' style='margin-top:0px;'>";
                            html += "<div class='span3'>";
                            html += "<button type='button' class='bg-pink fg-white large place-left' id='btnSaveSurvey' surveyid='" + data.ReturnObject.Id + "'>" + ReturnMessage(IndexHelper.LanguageCode, "M091") + "</button>"
                            html += "</div>";
                            html += "</div>";
                        }
                        html += "</div>";

                        $("#divSurvey").html(html);

                        $("#btnSaveSurvey").click(function () {
                            var surveyId = $(this).attr("surveyid");
                            var choiceId = $("input[type=radio][name='survey']:checked").val();

                            IndexHelper.AnswerSurvey(surveyId, choiceId);
                        });
                    }
                }
                else {
                    //parent.IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M059"), ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />SelectSurvey", "red", "white");

                    return false;
                }
            },
            error: function (a, b, c) {
                //debugger;
                parent.IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M059"), c + "<br />SelectSurvey", "red", "white");
            }
        });
    },
    "AnswerSurvey": function (surveyId, choiceId) {
        $("#lblErrorSurvey").hide();
        //debugger;
        var jData = {};
        jData.token = IndexHelper.Token;

        jData.answer = {};

        jData.answer.Portal = {};
        jData.answer.Portal.Id = IndexHelper.PortalId;
        jData.answer.Portal.LogicalName = "new_portal";

        jData.answer.PortalUser = {};
        jData.answer.PortalUser.Id = IndexHelper.UserId;
        jData.answer.PortalUser.Name = IndexHelper.FullName;
        jData.answer.PortalUser.LogicalName = "new_user";

        jData.answer.Survey = {};
        jData.answer.Survey.Id = surveyId;
        jData.answer.Survey.LogicalName = "new_survey";

        jData.answer.SurveyChoice = {};
        jData.answer.SurveyChoice.Id = choiceId;
        jData.answer.SurveyChoice.LogicalName = "new_surveychoice";

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/AnswerSurvey",
            async: true,
            dataType: "json",
            contentType: "application/json;",
            type: "POST",
            data: jSonData,
            beforeSend: function () {
                $("#divLoadingSurvey").show();
                $("#divSurvey").hide();
            },
            complete: function () {
                $("#divLoadingSurvey").hide();
                $("#divSurvey").show();
            },
            success: function (data) {
                //debugger;
                if (data != null) {

                    if (data.Success) {
                        parent.IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M061"), ReturnMessage(IndexHelper.LanguageCode, data.Result), "green", "white");

                        $("#rowSurvey").hide();
                        $("#rowSurvey").html("");
                    }
                    else {
                        parent.IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M060"), ReturnMessage(IndexHelper.LanguageCode, data.Result), "red", "white");
                        return;
                    }
                }
                else {
                    parent.IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M059"), ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />AnswerSurvey", "red", "white");

                    return false;
                }
            },
            error: function (a, b, c) {
                //debugger;
                parent.IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M059"), c + "<br />AnswerSurvey", "red", "white");
            }
        });
    },
    "CloseSession": function () {
        var jData = {};
        jData.token = IndexHelper.Token;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/CloseSession",
            async: false,
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
                    if (data.Success) {

                        var html = "";

                        IndexHelper.ToastrShow("success", ReturnMessage(IndexHelper.LanguageCode, data.Result), "Başarılı");

                        html += "<div class='text-center' style='margin-top:20px;'>";
                        html += "	<p>" + ReturnMessage(IndexHelper.LanguageCode, data.Result) + "</p>";
                        html += "</div>";

                        IndexHelper.ShowDialog(html, "Çıkış", null, true, function () {
                            $.session.clear();
                            document.location.href = "login.html";
                        });

                        return;
                    }
                    else {
                        IndexHelper.ToastrShow("error", ReturnMessage(IndexHelper.LanguageCode, data.Result), "Hata");
                        return false;
                    }
                }
                else {
                    IndexHelper.ToastrShow("error", ReturnMessage(IndexHelper.LanguageCode, "M002"), "Hata");
                    return false;
                }
            },
            error: function (a, b, c) {
                IndexHelper.ToastrShow("error", c, "Hata");
                return false;
            }
        });
    },
    "ScrollToBottom": function () {
        $("html, body").animate({ scrollTop: $(document).height() - $(window).height() }, 500);
    },
    "LikeEntity": function (token, entityId, entityName, callBakcFunction) {

        var jData = {};
        jData.token = token;
        jData.entityId = entityId;
        jData.entityName = entityName;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/LikeEntity",
            async: true,
            dataType: "json",
            contentType: "application/json;",
            type: "POST",
            data: jSonData,
            beforeSend: function () {
                IndexHelper.ShowLoading("Beğeni kaydı işleniyor...", null);
            },
            complete: function () {
                IndexHelper.CloseDialog();
            },
            success: function (data) {
                //debugger;
                if (data != null) {

                    callBakcFunction(data);

                    return;

                    if (data.Success == true) {

                        IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M061"), ReturnMessage(IndexHelper.LanguageCode, data.Result), "green", "white");
                        IndexHelper.GetEntityLikeInfo(token, entityId, entityName);
                    }
                    else {
                        IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M059"), ReturnMessage(IndexHelper.LanguageCode, data.Result), "red", "white");
                        // parent.IndexHelper.ShowAlertDialog(3, "Hata", data.Result);
                    }
                }
                else {
                    IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M059"), ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />LikeEntity", "red", "white");
                    //parent.IndexHelper.ShowAlertDialog(3, "Hata", "Servisten boş data döndü.<br />LikeEntity");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), c + "<br />LikeEntity");
                return false;
            }
        });
    },
    "DislikeEntity": function (token, entityId, entityName) {

        var jData = {};
        jData.token = token;
        jData.entityId = entityId;
        jData.entityName = entityName;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/DislikeEntity",
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
                    if (data.Success == true) {

                        IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M061"), ReturnMessage(IndexHelper.LanguageCode, data.Result), "green", "white");
                        IndexHelper.GetEntityLikeInfo(token, entityId, entityName);
                    }
                    else {
                        IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M059"), ReturnMessage(IndexHelper.LanguageCode, data.Result), "red", "white");
                        //parent.IndexHelper.ShowAlertDialog(3, "Hata", data.Result);
                    }
                }
                else {
                    IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M059"), ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />LikeEntity", "red", "white");
                    //parent.IndexHelper.ShowAlertDialog(3, "Hata", "Servisten boş data döndü.<br />DislikeEntity");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), c + "<br />DislikeEntity");
                return false;
            }
        });
    },
    "GetEntityLikeInfo": function (token, entityId, entityName, callBackFunction) {
        //debugger;
        var jData = {};
        jData.token = token;
        jData.entityId = entityId;
        jData.entityName = entityName;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/GetEntityLikeInfo",
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

                if (data != null) {
                    data = JSON.parse(data);

                    callBackFunction(data);

                    return;

                    if (data.Success) {
                        $("#ifrmContent").contents().find("small[type='up']").html(data.ReturnObject.LikeCount);
                        $("#ifrmContent").contents().find("small[type='down']").html(data.ReturnObject.DislikeCount);
                    }
                    else {
                        IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M059"), ReturnMessage(IndexHelper.LanguageCode, data.Result), "red", "white");
                        return;
                    }

                }
                else {
                    IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M059"), ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />GetEntityLikeInfo", "red", "white");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), c + "-GetEntityLikeInfo");
                return false;
            }
        });
    },
    "ReportImproperContent": function (token, entityId, entityName) {

        var jData = {};
        jData.token = token;
        jData.entityId = entityId;
        jData.entityName = entityName;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/ReportImproperContent",
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
                    if (data.Success == true) {

                        IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M061"), ReturnMessage(IndexHelper.LanguageCode, data.Result), "green", "white");
                        IndexHelper.GetEntityLikeInfo(token, entityId, entityName);
                    }
                    else {
                        IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M059"), ReturnMessage(IndexHelper.LanguageCode, data.Result), "red", "white");
                        //parent.IndexHelper.ShowAlertDialog(3, "Hata", data.Result);
                    }
                }
                else {
                    IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M059"), ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />ReportImproperContent", "red", "white");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), c + "<br />ReportImproperContent");
                return false;
            }
        });
    },
    "ShowTimeoutAlert": function (callbackFunction) {

        var html = "";

        //IndexHelper.ToastrShow("error", "Oturum bilgisi zaman aşımına uğradı.</br>Lütfen tekrardan sisteme giriş yapmayı deneyiniz.");

        html = "";
        html += "<div class='text-center' style='margin-top:20px;'>";
        html += "	<p>Oturum bilgisi zaman aşımına uğradı. </p>";
        html += "	<p>Lütfen tekrardan sisteme giriş yapmayı deneyiniz.</p>";
        html += "</div>";

        IndexHelper.ShowDialog(html, "Oturum Zaman Aşımı", "", true, callbackFunction);

        return;
    },
    "ShowColoredAlert": function (html, width, paddingLeft, paddingTop, bgColor, callbackFunction) {

        $.DialogDogus2({
            overlay: true,
            shadow: true,
            flat: true,
            width: width,
            overlayClickClose: false,
            paddingLeft: paddingLeft,
            paddingTop: paddingTop,
            bgColor: bgColor,
            content: html,
            sysBtnCloseClick: callbackFunction,
            onShow: function (_dialog) {
            }

        });
    },
    "GetRecordCounts": function () {
        //debugger;
        var jData = {};
        jData.token = IndexHelper.Token;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/GetModuleRecordCounts",
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

                    if (data.Success) {

                        if (data.ReturnObject != null && data.ReturnObject.length > 0) {

                            for (var i = 0; i < data.ReturnObject.length; i++) {

                                $("span[type='count'][typeof='" + data.ReturnObject[i].RecType + "']").show().html(data.ReturnObject[i].RecCount);
                            }
                        }
                    }
                }
                else {
                    parent.IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M059"), ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />GetRecordCounts", "red", "white");

                    return false;
                }
            },
            error: function (a, b, c) {
                //debugger;
                parent.IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M059"), c + "<br />GetRecordCounts", "red", "white");
            }
        });
    },
    "UsePointCode": function (code, callBackFunction) {
        //debugger;
        var jData = {};
        jData.token = IndexHelper.Token;
        jData.pointCode = code;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/UsePointCode",
            async: true,
            dataType: "json",
            contentType: "application/json;",
            type: "POST",
            data: jSonData,
            beforeSend: function () {
                IndexHelper.ShowLoading("Kod kontrol ediliyor...", null);
            },
            complete: function () {
                IndexHelper.CloseDialog();
            },
            success: function (data) {
                //debugger;
                if (data != null) {

                    callBackFunction(data);

                    return;
                }
                else {
                    parent.IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />GetRecordCounts", "error");

                    return false;
                }
            },
            error: function (a, b, c) {
                //debugger;
                parent.IndexHelper.ShowNotify(c + "<br />GetRecordCounts", "error");
            }
        });
    },
    "ConstructSocketIo": function () {
        try {
            IndexHelper.Socket = io.connect("http://kaleanahtarcilarkulubu.com.tr:5555/");


        } catch (e) {
            alert(e);
            return;
        }

        if (IndexHelper.Socket != undefined) {
            console.log("OK!");
        }
        else {
            alert("Soket bilgisi boş");
            return;
        }
    },
    "ListenSocketMessages": function (callBackFunction) {

        IndexHelper.Socket.on("message", function (data) {

            var comingData = data;
            data = data.message;

            if (data.ToId.Id == IndexHelper.UserId) {

                if (comingData.CreatedOn != null && comingData.CreatedOn != undefined) {
                    var date = new Date(comingData.CreatedOn);
                    var dateStr = date.getDate() + "." + (date.getMonth() + 1) + "." + date.getFullYear() + " " + date.getHours() + ":" + date.getMinutes();

                    comingData.CreatedOnString = dateStr;
                }
                else {
                    comingData.CreatedOnString = "---";
                }

                callBackFunction(comingData);

            }
            else {
                callBackFunction(null);
            }

            return;
        });
    },
    "GetUnReadMessages": function (token, callBackFunction) {

        //debugger;
        var jData = {};
        jData.token = token;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/GetUnReadMessages",
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
                }
                else {
                    IndexHelper.ToastrShow("error", ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />GetUnReadMessages", "Hata");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                IndexHelper.ToastrShow("error", c + "<br />GetUnReadMessages", "Hata");
                return false;
            }
        });
    },
    "DocumentBaseUrl": "",
    "ProfileImagePath": "",
    "UserName": "",
    "UserId": "",
    "FullName": "",
    "PortalId": "",
    "PageName": "",
    "PageTypeCode": "",
    "ObjectId": "",
    "LoginLogId": "",
    "LogoImageData": "",
    "LogoImageMimeType": "",
    "Token": "",
    "CanScrollEvent": true,
    "LanguageCode": "tr",
    "Socket": {}
};

function mainController($scope) {

    var url = $.url(document.location);
    IndexHelper.DocumentBaseUrl = url.data.attr.base + url.data.attr.path + "?" + url.data.attr.query;

    IndexHelper.Token = url.param("token");
    $scope.token = url.param("token");;

    IndexHelper.LoginLogId = $.session.get("loginLogId");

    $scope.ContentMessages = LanguageMessages[IndexHelper.LanguageCode];

    $scope.showErrorHeader = false;
    $scope.showLoading = true;
    $scope.showMain = false;
    $scope.errorText = "";
    $scope.showMessageNotification = false;
    $scope.notificationCount = 0;


    IndexHelper.CheckSession(false, $scope.token, function (e) {
        $scope.$apply(function () {
            if (e.Success == true) {

                IndexHelper.UserId = e.ReturnObject.PortalUserId;
                IndexHelper.PortalId = e.ReturnObject.PortalId;

                $scope.userId = e.ReturnObject.PortalUserId;
                $scope.portalId = e.ReturnObject.PortalId;

                IndexHelper.ConstructSocketIo();
                //IndexHelper.Socket.emit("user_login", IndexHelper.UserId);
                //$scope.ListenSocketMessages();
            }
            else {
                return;
            }
        });
    });

    $scope.iframeSrc = "duvarYazisi.html";

    IndexHelper.GetUserInfo($scope.token, function (e) {
        $scope.$apply(function () {
            if (e.Success == true) {

                if (e.ReturnObject.IsDisclaimerApproved == false) {
                    //IndexHelper.OpenContractPage(function () {
                    //    $("#btnLogOut").click();
                    //});
                    alert("CONFIRM CONTRACT");
                    return;
                }

                if (e.ReturnObject.IsWelcomeMessageGenerate == false) {
                    //IndexHelper.OpenWelcomePage(function () {
                    //    IndexHelper.UpdateWelcomePageGenerated();
                    //});

                    alert("SHOW WELCOME MESSAGE");
                }

                $scope.contact = e.ReturnObject.ContactInfo;

                var imagePath = e.ReturnObject.Image;

                if (imagePath != null || imagePath == undefined || imagePath == "nouserprofile.jpg") {
                    $scope.profileImagePath = attachmentUrl + imagePath;
                }
                else {
                    $scope.profileImagePath = "./images/no-user.png";
                }

                IndexHelper.ProfileImagePath = $scope.profileImagePath;
                IndexHelper.UserName = e.ReturnObject.UserName;

                //    IndexHelper.GetTopAnnouncements(0, 3);
                //    IndexHelper.GetTopVideos(0, 3);

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

    $scope.openPage = function (e, page) {
        $scope.iframeSrc = page;

        $("a[type='menu']").removeClass("selected");
        var element = angular.element(e.srcElement);
        element.addClass("selected");

        $(window).scrollTop(0);
    };

    $scope.OpenDynamicPage = function (pageNo) {
        $scope.iframeSrc = "pagecontent.html?pageno=" + pageNo;
    };

    $scope.CloseSession = function () {
        IndexHelper.CloseSession();
    };

    $scope.showCodeMessage = false;

    $scope.codeMessage = null;

    $scope.UseCode = function () {
            var code = $scope.pointCode;

            $scope.showCodeMessage = false;
            $scope.codeMessage = null;

            if (code != null && code != "" && code != undefined) {

                IndexHelper.UsePointCode(code, function (e) {
                    $scope.$apply(function () {
                        if (e.Success == true) {

                            IndexHelper.ToastrShow("success", e.Result, "Başarılı");


                            $scope.pointCode = null;

                            $scope.showCodeMessage = true;
                            $scope.codeMessage = e.Result;
                            //document.location.reload();

                        }
                        else {
                            $scope.showCodeMessage = true;
                            $scope.codeMessage = e.Result;

                            IndexHelper.ToastrShow("error", e.Result, "Hata");
                        }
                    });
                });
            }
            else {
                $scope.showCodeMessage = true;
                $scope.codeMessage = "Lütfen bir kod giriniz.";
                IndexHelper.ToastrShow("warning", "Lütfen bir kod giriniz.", "Eksik Kod");
            }
    };

    $scope.EditUser = function (userId) {

        if (userId.toLowerCase() != IndexHelper.UserId.toLowerCase()) {
            IndexHelper.ShowDialog("<iframe src='popupArkadaslarim.html?userid=" + userId + "' style='width:100%;height:570px;' />", null, null, true);
        }
    };

    IndexHelper.GetUnReadMessages($scope.token, function (e) {
        $scope.$apply(function () {
            if (e.Success == true) {

                $scope.Unreads = e.ReturnObject;

                $scope.notificationCount = e.ReturnObject.length;
            }
        });
    });

    $scope.OpenUserChatPage = function (toId, toName) {

        var userId = IndexHelper.UserId;
        var userIdName = IndexHelper.userIdName;

        $("#ifrmContent").attr("src", "http://kaleanahtarcilarkulubu.com.tr:5555/chat?targetuserid=" + toId + "&targetuseridname=" + toName + "&userid=" + userId + "&useridname=" + userIdName);
        //$("#ifrmContent").attr("src", "http://localhost:3000/chat?targetuserid=" + toId + "&targetuseridname=" + toName + "&userid=" + userId + "&useridname=" + userIdName);

        $("li[fromid='" + toId + "']").remove();

        $scope.notificationCount = $(".ntfHolder ul li").length;
    };

    $scope.Like = function (entityId, entityName, callBack) {

        IndexHelper.LikeEntity(IndexHelper.Token, entityId, entityName, function (e) {

            if (e.Success == true) {
                IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, e.Result), "success");
            }
            else {
                IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, e.Result), "error");
            }

            callBack(e);
        });
    };

    $scope.ListenSocketMessages = function () {

        IndexHelper.Socket.on("has_message", function (data) {

            $scope.$apply(function () {

                $scope.showMessageNotification = true;
            });

        });

    };
}