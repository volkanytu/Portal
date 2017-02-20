var MessagesHelper = {
    "GetUserRecentContacts": function (callBackFunction) {
        var jData = {};
        jData.token = MessagesHelper.Token;

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
    "UserId": "",
    "PortalId": "",
    "Token": ""
};


function messagesController($scope) {

    $(parent.window).scrollTop(0);

    var parentUrl = $.url(parent.document.location);
    var url = $.url(document.location);

    MessagesHelper.Token = parentUrl.param("token");
    $scope.token = parentUrl.param("token");

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

                MessagesHelper.UserId = e.ReturnObject.PortalUserId;
                MessagesHelper.PortalId = e.ReturnObject.PortalId;

                $scope.userId = e.ReturnObject.PortalUserId;
                $scope.portalId = e.ReturnObject.PortalId;
            }
            else {
                return;
            }
        });
    });

    MessagesHelper.GetUserRecentContacts(function (e) {

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