/// <reference path="mainJS.js" />
/// <reference path="../jquery/jquery.tokenize.js" />
/// <reference path="../jquery-base.min.js" />

var SendMessagesHelper = {
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
        jData.token = SendMessagesHelper.Token;

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

        var jData = {
            token: SendMessagesHelper.Token,
            message: {
                Portal: {
                    Id: SendMessagesHelper.PortalId,
                    LogicalName: "new_portal"
                },
                FromId: {
                    Id: SendMessagesHelper.UserId,
                    Name: encodeURIComponent(parent.IndexHelper.UserName),
                    LogicalName: "new_user"
                },
                ToId: {
                    Id: toId,
                    Name: encodeURIComponent(toName),
                    LogicalName: "new_user"
                },
                Content: encodeURIComponent(message)
            }
        };

        parent.IndexHelper.ShowLoading("Mesajınız gönderiliyor...");

        SendMessagesHelper.Socket.emit("input", jData);

        return;

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
            SendMessagesHelper.Socket = io.connect("http://84.51.31.57:8080/"
                    , { query: "from=" + SendMessagesHelper.From + "&to=" + SendMessagesHelper.To + "&portalid=" + SendMessagesHelper.PortalId });

            //SendMessagesHelper.Socket = io.connect("http://localhost:5555/"
            //    , { query: "from=" + SendMessagesHelper.From + "&to=" + SendMessagesHelper.To + "&portalid=" + SendMessagesHelper.PortalId });

        } catch (e) {
            alert(e);
            return;
        }

        if (SendMessagesHelper.Socket != undefined) {
            console.log("OK!");
        }
        else {
            alert("Soket bilgisi boş");
            return;
        }
    },
    "ListenSocketOldMessages": function (callBackFunction) {

        SendMessagesHelper.Socket.on("oldmessages", function (data) {

            for (var i = 0; i < data.length; i++) {

                if (data[i].CreatedOn != null && data[i].CreatedOn != undefined) {
                    var date = new Date(data[i].CreatedOn);
                    var dateStr = date.getDate() + "." + (date.getMonth() + 1) + "." + date.getFullYear() + " " + date.getHours() + ":" + date.getMinutes();

                    data[i].CreatedOnString = dateStr;
                }
                else {
                    data[i].CreatedOnString = "---";
                }
            }

            callBackFunction(data);

            return;


            var html = "";

            for (var i = 0; i < data.length; i++) {

                html += "<div class='row' style='margin-bottom:10px;'>";
                html += "	<div class='span1' style='width: 56px; height: 56px; float: left; background-size: 100%; background-position: center center; background-repeat: no-repeat; background-image: url(\"" + attachmentUrl + data[i].FromImageUrl + "\"); '>";
                html += "	</div>";
                html += "	<div class='span6'>";
                html += "		<p class='item-title place-left' style='color: #0e9bae;'>" + data[i].From.Name + "</p>";
                html += "		<p class='place-right' style='color: #c5bfbf;'>" + data[i].CreatedOnString + "</p>";
                //html += "		<p class='tertiary-text place-left' style='line-height:2.3rem !important; font-size:medium; margin-top:-5px;'>" + data[i].Content + "</p>";
                html += "	</div>";
                html += "   <div class='span6'>";
                html += "	    <p class='tertiary-text place-left' style='line-height:2.3rem !important; font-size:medium; margin-top:-5px;'>" + data[i].Content + "</p>";
                html += "   </div>";
                html += "</div>";

                if (data[i].To.Id == SendMessagesHelper.UserId && data[i].StatusCode == 1) {
                    SendMessagesHelper.UpdateMessageAsSeen(data[i].Id);
                }
            }

            parent.IndexHelper.GetRecordCounts();

            $("#lstChats").append(html);
            $("#divMess").scrollTop(5000);
        });
    },
    "ListenSocketMessages": function (callBackFunction) {

        var a = "";

        SendMessagesHelper.Socket.on("message", function (data) {

            var comingData = data;
            data = data.message;

            if ((data.FromId.Id == SendMessagesHelper.From && data.ToId.Id == SendMessagesHelper.To) || (data.ToId.Id == SendMessagesHelper.From && data.FromId.Id == SendMessagesHelper.To)) {

                parent.IndexHelper.CloseDialog();

                if (comingData.CreatedOn != null && comingData.CreatedOn != undefined) {
                    var date = new Date(comingData.CreatedOn);
                    var dateStr = date.getDay() + "." + (date.getMonth() + 1) + "." + date.getFullYear() + " " + date.getHours() + ":" + date.getMinutes();

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


            var html = "";

            var now = new Date();
            var nowFormatted = now.format("mmmm d, yyyy HH:mm");

            if ((data.From.Id == SendMessagesHelper.From && data.To.Id == SendMessagesHelper.To) || (data.To.Id == SendMessagesHelper.From && data.From.Id == SendMessagesHelper.To)) {
                html += "<div class='row' style='margin-bottom:10px;'>";
                html += "	<div class='span1' style='width: 56px; height: 56px; float: left; background-size: 100%; background-position: center center; background-repeat: no-repeat; background-image: url(\"" + attachmentUrl + data.FromImageUrl + "\"); '>";
                html += "	</div>";
                html += "	<div class='span6'>";
                html += "		<p class='item-title place-left' style='color: #0e9bae;'>" + data.From.Name + "</p>";
                html += "		<p class='place-right' style='color: #c5bfbf;'>" + data.CreatedOnString + "</p>";
                //html += "		<p class='tertiary-text place-left' style='line-height:2.3rem !important; font-size:medium; margin-top:-5px;'>" + data.message.Content + "</p>";
                html += "	</div>";
                html += "   <div class='span6'>";
                html += "	    <p class='tertiary-text place-left' style='line-height:2.3rem !important; font-size:medium; margin-top:-5px;'>" + data.Content + "</p>";
                html += "   </div>";
                html += "</div>";

                $("#lstChats").append(html);
                $("#divMess").scrollTop(5000);

                if (data.To.Id == SendMessagesHelper.UserId) {

                    SendMessagesHelper.UpdateMessageAsSeen(data.Id);

                    parent.IndexHelper.GetRecordCounts();
                }

                SendMessagesHelper.GetUserRecentContacts();
            }
        });
    },
    "UserId": "",
    "PortalId": "",
    "From": "",
    "To": "",
    "FullName": "",
    "ImgName": "",
    "Token": "",
    "Socket": {}
};

function chatController($scope) {

    $("html").niceScroll();

    $(parent.window).scrollTop(0);

    var parentUrl = $.url(parent.document.location);
    var url = $.url(document.location);

    SendMessagesHelper.Token = parentUrl.param("token");
    $scope.token = parentUrl.param("token");

    //SendMessagesHelper.From = url.param("from");
    //$scope.from = url.param("from");

    SendMessagesHelper.To = url.param("to");
    $scope.to = url.param("to");

    SendMessagesHelper.FullName = url.param("fullname");
    $scope.fullName = url.param("fullname");

    parent.IndexHelper.AutoResize("ifrmContent");

    $scope.attachmentUrl = attachmentUrl;
    $scope.ContentMessages = LanguageMessages[parent.IndexHelper.LanguageCode];

    $scope.showErrorHeader = false;
    $scope.showLoading = true;
    $scope.showMain = false;
    $scope.errorText = "";

    parent.angular.element('html').scope().showMessageNotification = false;

    //$scope.ListenSocketOldMessages = function () {
    //    SendMessagesHelper.ListenSocketOldMessages(function (e) {

    //        var a = JSON.stringify(e);

    //        //var categories = [];

    //        //$.each(e, function (index, value) {

    //        //    if (value.message.FromId.Id != SendMessagesHelper.UserId) {
    //        //        if ($.inArray(value.message.FromId.Id, categories) === -1) {
    //        //            categories.push(value.message.FromId.Id);
    //        //        }
    //        //    }

    //        //    if (value.message.ToId.Id != SendMessagesHelper.UserId) {
    //        //        if ($.inArray(value.message.ToId.Id, categories) === -1) {
    //        //            categories.push(value.message.ToId.Id);
    //        //        }
    //        //    }
    //        //});

    //        //var recent = [];
    //        //for (var i = 0; i < e.length; i++) {

    //        //    if ($.inArray(e[i].message.FromId.Id, categories) === -1) {

    //        //    }
    //        //    else {
    //        //        var mes = {};
    //        //        mes.Id = e[i].message.FromId.Id;
    //        //        mes.Message = e[i].message.Content;

    //        //        recent.push(mes);
    //        //    }
    //        //}



    //        if (e != null) {
    //            $scope.$apply(function () {

    //                $scope.oldMessages = e;

    //                for (var i = 0; i < e.length; i++) {
    //                    e[i].message.Content = FindAndReplaceAll(e[i].message.Content);
    //                }

    //                setTimeout(function () {
    //                    $(".writeComnt").scrollTop(5000);
    //                }, 500);
    //            });
    //        }
    //    });
    //};

    //$scope.ListenSocketMessages = function () {
    //    SendMessagesHelper.ListenSocketMessages(function (e) {

    //        if (e != null) {
    //            $scope.$apply(function () {

    //                $scope.newMessage = e;

    //                e.message.Content = FindAndReplaceAll(e.message.Content);

    //                $scope.oldMessages.push(e);

    //                $scope.message = null;

    //                //parent.angular.element('html').scope().showMessageNotification = true;

    //                setTimeout(function () {
    //                    $(".writeComnt").scrollTop(5000);


    //                }, 500);
    //            });
    //        }
    //    });
    //};

    parent.IndexHelper.CheckSession(true, $scope.token, function (e) {
        $scope.$apply(function () {
            if (e.Success == true) {

                SendMessagesHelper.UserId = e.ReturnObject.PortalUserId;
                SendMessagesHelper.PortalId = e.ReturnObject.PortalId;
                SendMessagesHelper.From = e.ReturnObject.PortalUserId;

                $scope.userId = e.ReturnObject.PortalUserId;
                $scope.portalId = e.ReturnObject.PortalId;
                $scope.from = e.ReturnObject.PortalUserId;

                //SendMessagesHelper.ConstructSocketIo();

                $scope.ListenSocketOldMessages();
                $scope.ListenSocketMessages();

                //SendMessagesHelper.Socket.emit("getoldmessages");
            }
            else {
                return;
            }
        });
    });

    SendMessagesHelper.GetUserRecentContacts(function (e) {

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

        if (message == null || message == undefined || message == "" || message == "undefined") {
            parent.IndexHelper.ToastrShow("warning", "Mesaj göndermek için lütfen içerik giriniz.", "Uyarı");

            return;
        }

        message = message.replace("ş", "s").replace("Ş", "S");

        var jData = {
            token: SendMessagesHelper.Token,
            CreatedOn: new Date(),
            message: {
                Portal: {
                    Id: SendMessagesHelper.PortalId,
                    LogicalName: "new_portal"
                },
                FromId: {
                    Id: SendMessagesHelper.From,
                    Name: parent.IndexHelper.UserName,
                    LogicalName: "new_user"
                },
                ToId: {
                    Id: SendMessagesHelper.To,
                    Name: SendMessagesHelper.FullName,
                    LogicalName: "new_user"
                },
                Content: message
            }
        };

        SendMessagesHelper.Socket.emit("input", jData);

        $scope.message = null;
    };

    $scope.OpenUserChatPage = function (toId, toName) {

        $("#ifrmContent", parent.document).attr("src", "chat.html?to=" + toId + "&fullname=" + toName);
    };
}

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