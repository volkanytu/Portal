/// <reference path="mainJS.js" />
/// <reference path="../jquery/jquery.tokenize.js" />
/// <reference path="../jquery-base.min.js" />

var NewMessagesHelper = {
    "OnLoad": function () {

        $("#divMess").niceScroll();

        $('#divTo').tokenize({
            maxElements: 1,
            datas: CustomServiceUrl + "CrmService.svc/?token=" + NewMessagesHelper.Token,
            newElements: true
        });

        var url = $.url(document.location);

        try {
            NewMessagesHelper.Socket = io.connect("https://platform.4bizcrm.com:8080", { query: "from=" + NewMessagesHelper.UserId + "&portalid=" + NewMessagesHelper.PortalId });

        } catch (e) {
            alert(e);
            return;
        }

        if (NewMessagesHelper.Socket != undefined) {
            console.log("OK!");
        }
        else {
            alert("Soket bilgisi boş");
            return;
        }

        NewMessagesHelper.OnClickEvents();
        NewMessagesHelper.OnChangeEvents();
        NewMessagesHelper.KeyPressEvents();

        NewMessagesHelper.GetUserRecentContacts();

        parent.IndexHelper.AutoResize("ifrmContent");
    },
    "OnClickEvents": function () {

        $("#btnSend").click(function () {

            NewMessagesHelper.SendNewMessage();
        });
    },
    "OnChangeEvents": function () {

    },
    "KeyPressEvents": function () {
        $("#txtKey").keydown(function (e) {
            if (e.which == 13) {
                $("#btnSearch").click();
            }
        });
    },
    "GetUserRecentContacts": function (callBackFunction) {
        var jData = {};
        jData.token = NewMessagesHelper.Token;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/GetUserRecentContacts",
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
                }
                else {
                    $("#lblErrorMain").show();
                    $("#lblErrorMain").html("<h1 class='fg-yellow'><i class='icon-warning'></i></h1><br />" + ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />GetArticles");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), c + "<br / >GetArticles");
                return false;
            }
        });
    },
    "SendNewMessage": function (message, callBackFunction) {

        var toId = $("#divTo option:selected").val();
        var toName = $("#divTo option:selected").text();

        var jData = {
            token: NewMessagesHelper.Token,
            message: {
                Portal: {
                    Id: NewMessagesHelper.PortalId,
                    LogicalName: "new_portal"
                },
                FromId: {
                    Id: NewMessagesHelper.UserId,
                    Name: parent.IndexHelper.UserName,
                    LogicalName: "new_user"
                },
                ToId: {
                    Id: toId,
                    Name: toName,
                    LogicalName: "new_user"
                },
                Content: message
            }
        };

        //NewMessagesHelper.Socket.emit("input", jData);

        // document.location = "sendmessage.html?to=" + toId + "&from=" + NewMessagesHelper.UserId + "&fullname=" + toName;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/CreateMessage",
            async: true,
            dataType: "json",
            contentType: "application/json;",
            type: "POST",
            data: jSonData,
            beforeSend: function () {
                parent.IndexHelper.ShowLoading("Mesajınız gönderiliyor...");
            },
            complete: function () {

                parent.IndexHelper.CloseDialog();
            },
            success: function (data) {
                //debugger;
                if (data != null) {

                    callBackFunction(data);

                    return;

                    //debugger;
                    parent.IndexHelper.GetRecordCounts();
                    document.location = "sendmessage.html?to=" + toId + "&from=" + NewMessagesHelper.UserId + "&fullname=" + toName;
                }
                else {
                    $("#lblErrorMain").show();
                    $("#lblUserNotFound").show().html(ReturnMessage(parent.IndexHelper.LanguageCode, "M002") + "<br />SendNewMessage");

                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), c + "<br />SendNewMessage");
                return false;
            }
        });

    },
    "ConstructSocketIo": function () {
        try {
            NewMessagesHelper.Socket = io.connect("http://kaleanahtarcilarkulubu.com.tr:5555/"
                , { query: "from=" + NewMessagesHelper.UserId + "&portalid=" + NewMessagesHelper.PortalId });

            //        NewMessagesHelper.Socket = io.connect("http://localhost:5555/"
            //, { query: "from=" + NewMessagesHelper.UserId + "&portalid=" + NewMessagesHelper.PortalId });

        } catch (e) {
            alert(e);
            return;
        }

        if (NewMessagesHelper.Socket != undefined) {
            console.log("OK!");
        }
        else {
            alert("Soket bilgisi boş");
            return;
        }
    },
    "UserId": "",
    "PortalId": "",
    "Token": "",
    "Socket": {}
};

function newMessagesController($scope) {

    $(parent.window).scrollTop(0);

    var parentUrl = $.url(parent.document.location);
    var url = $.url(document.location);

    NewMessagesHelper.Token = parentUrl.param("token");
    $scope.token = parentUrl.param("token");

    $('#divTo').tokenize({
        maxElements: 1,
        datas: CustomServiceUrl + "CrmService.svc/?token=" + NewMessagesHelper.Token,
        newElements: true
    });

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

                NewMessagesHelper.UserId = e.ReturnObject.PortalUserId;
                NewMessagesHelper.PortalId = e.ReturnObject.PortalId;

                $scope.userId = e.ReturnObject.PortalUserId;
                $scope.portalId = e.ReturnObject.PortalId;

                //NewMessagesHelper.ConstructSocketIo();
            }
            else {
                return;
            }
        });
    });

    NewMessagesHelper.GetUserRecentContacts(function (e) {

        $scope.$apply(function () {
            var a = JSON.stringify(e);

            if (e != null && e.length > 0) {

                $scope.recentMessages = e;

                for (var i = 0; i < e.length; i++) {

                    $scope.recentMessages[i].ProfileImage = e[i].FromId.Id == $scope.userId ? e[i].ToImageUrl : e[i].FromImageUrl;
                    $scope.recentMessages[i].UserId = e[i].FromId.Id == $scope.userId ? e[i].ToId.Id : e[i].FromId.Id;
                    $scope.recentMessages[i].UserIdName = e[i].FromId.Id == $scope.userId ? e[i].ToId.Name : e[i].FromId.Name;
                    $scope.recentMessages[i].Content = FindAndReplaceAll(e[i].Content);
                }


                $scope.showErrorHeader = false;
                $scope.showLoading = false;
                $scope.showMain = true;

                setTimeout(function () {
                    parent.IndexHelper.AutoResize("ifrmContent");
                }, 500);

            }
            else {
                $scope.showErrorHeader = true;
                $scope.showLoading = false;
                $scope.showMain = false;
                $scope.errorText = ReturnMessage(parent.IndexHelper.LanguageCode, e.Result);
            }

            setTimeout(function () {
                parent.IndexHelper.AutoResize("ifrmContent");
            }, 500);
        });

    });

    $scope.SendNewMessage = function () {

        var message = $scope.message;
        var toId = $("#divTo option:selected").val();
        var toIdName = $("#divTo option:selected").text();

        if (toId == null || toId == undefined || toId == "" || toId == "undefined") {
            parent.IndexHelper.ToastrShow("warning", "Mesaj göndermek için lütfen Alıcı seçiniz.", "Uyarı");

            return;
        }

        if (message == null || message == undefined || message == "" || message == "undefined") {
            parent.IndexHelper.ToastrShow("warning", "Mesaj göndermek için lütfen içerik giriniz.", "Uyarı");

            return;
        }

        var userId = parent.IndexHelper.UserId;
        var userIdName = parent.IndexHelper.userIdName;

        //$("#ifrmContent", parent.document).attr("src", "http://kaleanahtarcilarkulubu.com.tr:5555/chat?targetuserid=" + toId + "&targetuseridname=" + toIdName + "&userid=" + userId + "&useridname=" + userIdName);
        $("#ifrmContent", parent.document).attr("src", "http://localhost:3000/chat?targetuserid=" + toId + "&targetuseridname=" + toIdName + "&userid=" + userId + "&useridname=" + userIdName);

    };

    $scope.OpenUserChatPage = function (toId, toName) {
        var userId = parent.IndexHelper.UserId;
        var userIdName = parent.IndexHelper.userIdName;

        //$("#ifrmContent", parent.document).attr("src", "http://kaleanahtarcilarkulubu.com.tr:5555/chat?targetuserid=" + toId + "&targetuseridname=" + toName + "&userid=" + userId + "&useridname=" + userIdName);
        $("#ifrmContent", parent.document).attr("src", "http://localhost:3000/chat?targetuserid=" + toId + "&targetuseridname=" + toName + "&userid=" + userId + "&useridname=" + userIdName);
    };
};

function FindAndReplaceAll(text) {
    var normal = new Array("Ä±", "Å?", "Ã¼", "Ã§", "Ã¶", "Ä?", "ÅŸ", "Ã‡", "Ä°", "ÄŸ", "Åž", "Ã–", "Ãœ", "Ä±", "Å?", "Ã§", "Ã¶", "Ä?", "ÅŸ", "Ã‡", "Ä°", "ÄŸ", "Åž", "Ã–", "Ãœ", "Ã¼", "ÄŸ", "Ä", "Å");
    var turkish = new Array("ı", "ş", "ü", "ç", "ö", "ğ", "ş", "Ç", "i", "ğ", "Ş", "Ö", "Ü", "ı", "ş", "ç", "ö", "ğ", "ş", "Ç", "i", "ğ", "Ş", "Ö", "Ü", "ü", "ğ", "ğ", "ü");

    for (var i = 0; i < normal.length; i++) {

        if (text.indexOf(normal[i]) != -1) {

            text = text.replace(normal[i], turkish[i]);
        }
    }

    return text;
}