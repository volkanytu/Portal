/// <reference path="../jquery/jquery.min.js" />
/// <reference path="../jquery/jquery.widget.min.js" />
/// <reference path="../jquery/purl.js" />
/// <reference path="GlobalVariables.js" />
/// <reference path="indexJS.js" />
/// <reference path="mainJS.js" />

var EditUserHelper = {
    "OnLoad": function () {

        EditUserHelper.OnClickEvents();
        EditUserHelper.OnChangeEvents();
    },
    "OnClickEvents": function () {

        $("#btnDenyRequest").click(function () {
            EditUserHelper.CloseFriendshipRequest(100000000);
        });

        $("#btnAcceptRequest").click(function () {
            EditUserHelper.CloseFriendshipRequest(2);
        });

        $("#btnCancelRequest").click(function () {
            EditUserHelper.CloseFriendshipRequest(100000001);
        });

        $("#btnSendRequest").click(function () {
            EditUserHelper.CreateFriendshipRequest();
        });

        $("#btnDeleteFriendship").click(function () {
            EditUserHelper.CloseFriendship();
        });

    },
    "OnChangeEvents": function () {
    },
    "GetProfileInfo": function (callBakcFunction) {

        var jData = {};

        jData.token = EditUserHelper.Token;

        jData.userId = EditUserHelper.TargetUserId;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/GetFriendUserInfo",
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

                    callBakcFunction(data);

                    return;

                    //debugger;
                    if (data.Success == true) {

                        if (data.ReturnObject.ContactInfo != null) {
                            $("#txtName").html(data.ReturnObject.ContactInfo.FirstName != "" ? data.ReturnObject.ContactInfo.FirstName : "---");
                            $("#txtSurName").html(data.ReturnObject.ContactInfo.LastName != "" ? data.ReturnObject.ContactInfo.LastName : "---");
                            $("#txtTitle").html(data.ReturnObject.ContactInfo.Title != "" ? data.ReturnObject.ContactInfo.Title : "---");
                            $("#txtFunction").html(data.ReturnObject.ContactInfo.FunctionName != "" ? data.ReturnObject.ContactInfo.FunctionName : "---");
                            $("#txtEmail").html(data.ReturnObject.ContactInfo.EmailAddress != "" ? data.ReturnObject.ContactInfo.EmailAddress : "---");
                            $("#txtMobilePhone").html(data.ReturnObject.ContactInfo.MobilePhone != "" ? data.ReturnObject.ContactInfo.MobilePhone : "---");
                            $("#txtWorkPhone").html(data.ReturnObject.ContactInfo.WorkPhone != "" ? data.ReturnObject.ContactInfo.WorkPhone : "---");

                            EditUserHelper.TargetUserIdName = data.ReturnObject.ContactInfo.FirstName + " " + data.ReturnObject.ContactInfo.LastName;

                            $("#imgUser").attr("src", attachmentUrl + data.ReturnObject.Image);

                            if (data.ReturnObject.ContactInfo.BirthDate != null) {
                                var bdate = new Date(parseInt(data.ReturnObject.ContactInfo.BirthDate.replace("/Date(", "").replace(")/", ""), 10));
                                var birthDate = bdate.getDate() + "." + (bdate.getMonth() + 1) + "." + bdate.getFullYear();

                                $("#txtBirthDate").html(birthDate);
                            }
                            else {
                                $("#txtBirthDate").html("---");
                            }
                        }

                        $("#divMain").show();

                        EditUserHelper.CheckIsUserYourFriend();
                    }
                    else {
                        $("#lblErrorMain").show();
                        $("#lblErrorMain").html("<h1 class='fg-yellow'><i class='icon-warning'></i></h1><br />" + ReturnMessage(IndexHelper.LanguageCode, data.Result));

                        return false;
                    }
                }
                else {
                    $("#lblErrorMain").show();
                    $("#lblErrorMain").html("<h1 class='fg-yellow'><i class='icon-warning'></i></h1><br />" + ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />GetProfileInfo");

                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), c + "<br />GetProfileInfo");
                return false;
            }
        });
    },
    "CheckIsUserYourFriend": function (callBakcFunction) {
        //debugger;
        var jData = {};

        jData.token = EditUserHelper.Token;

        jData.selectedUserId = EditUserHelper.TargetUserId;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/CheckIsUserYourFriend",
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

                    callBakcFunction(data);

                    return;

                    //debugger;
                    if (data.Success == true) {
                        EditUserHelper.FriendshipId = data.CrmId;

                        $("#divFriendshipOperations").show();
                    }
                    else {
                        EditUserHelper.HasUserRequestWithYou();
                        return false;
                    }
                }
                else {
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                alert(a);
                return false;
            }
        });
    },
    "HasUserRequestWithYou": function (callBakcFunction) {
        //debugger;
        var jData = {};

        jData.token = EditUserHelper.Token;

        jData.selectedUserId = EditUserHelper.TargetUserId;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/HasUserRequestWithYou",
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

                    callBakcFunction(data);

                    return;

                    //debugger;
                    if (data.Success == true) {
                        EditUserHelper.RequestId = data.ReturnObject.Id;
                        $("#divRequestOperations").show();

                        if (data.ReturnObject.From != null && data.ReturnObject.From.Id.toLowerCase() == EditUserHelper.UserId.toLowerCase()) {
                            $("#btnCancelRequest").show();
                        }
                        else if (data.ReturnObject.To != null && data.ReturnObject.To.Id.toLowerCase() == EditUserHelper.UserId.toLowerCase()) {
                            $("#btnDenyRequest").show();
                            $("#btnAcceptRequest").show();
                        }
                        else {
                            $("#divSendRequest").show();
                        }
                    }
                    else {
                        $("#divSendRequest").show();
                        return false;
                    }
                }
                else {
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                alert(a);
                return false;
            }
        });
    },
    "CloseFriendshipRequest": function (statusCode, callBakcFunction) {
        var jData = {};
        jData.token = EditUserHelper.Token;
        jData.requestId = EditUserHelper.RequestId;
        jData.statusCode = statusCode;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/CloseFriendshipRequest",
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
                    //debugger;

                    callBakcFunction(data);

                    return;

                    if (data.Success == true) {
                        parent.IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M061"), ReturnMessage(IndexHelper.LanguageCode, "M041"), "green", "white");
                        document.location.reload();
                        $("#ifrmContent", parent.document).attr("src", "friendship.aspx");
                    }
                    else {
                        parent.IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M059"), ReturnMessage(IndexHelper.LanguageCode, data.Result), "red", "white");
                        return false;
                    }
                }
                else {
                    parent.IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M059"), ReturnMessage(IndexHelper.LanguageCode, "M002"), "red", "white");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M059"), c, "red", "white");
                return false;
            }
        });
    },
    "CreateFriendshipRequest": function (callBakcFunction) {
        //debugger;
        var jData = {};

        jData.token = EditUserHelper.Token;

        jData.request = {};

        jData.request.Portal = {};
        jData.request.Portal.Id = EditUserHelper.PortalId;
        jData.request.Portal.LogicalName = "new_portal";

        jData.request.From = {};
        jData.request.From.Id = EditUserHelper.UserId;
        jData.request.From.Name = parent.IndexHelper.UserName;
        jData.request.From.LogicalName = "new_user";

        jData.request.To = {};
        jData.request.To.Id = EditUserHelper.TargetUserId;
        jData.request.To.Name = EditUserHelper.TargetUserIdName;
        jData.request.To.LogicalName = "new_user";

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/CreateFriendshipRequest",
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
                    //debugger;

                    callBakcFunction(data);

                    return;

                    if (data.Success == true) {
                        parent.IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M061"), ReturnMessage(IndexHelper.LanguageCode, data.Result), "green", "white");
                        document.location.reload();
                        $("#ifrmContent", parent.document).attr("src", "friendship.aspx");
                    }
                    else {
                        parent.IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M059"), ReturnMessage(IndexHelper.LanguageCode, data.Result), "red", "white");
                        return false;
                    }
                }
                else {
                    parent.IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M059"), ReturnMessage(IndexHelper.LanguageCode, "M002"), "red", "white");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M059"), c, "red", "white");
                return false;
            }
        });
    },
    "CloseFriendship": function (callBakcFunction) {
        //debugger;
        var jData = {};

        jData.token = EditUserHelper.Token;

        jData.friendshipId = EditUserHelper.FriendshipId;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/CloseFriendship",
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
                    //debugger;

                    callBakcFunction(data);

                    return;

                    if (data.Success == true) {
                        parent.IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M061"), ReturnMessage(IndexHelper.LanguageCode, data.Result), "green", "white");
                        document.location.reload();

                        $("#ifrmContent", parent.document).attr("src", "friendship.aspx");
                    }
                    else {
                        parent.IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M059"), ReturnMessage(IndexHelper.LanguageCode, data.Result), "red", "white");
                        return false;
                    }
                }
                else {
                    parent.IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M059"), ReturnMessage(IndexHelper.LanguageCode, "M002"), "red", "white");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ShowNotify(ReturnMessage(IndexHelper.LanguageCode, "M059"), c, "red", "white");
                return false;
            }
        });
    },
    "UserId": "",
    "PortalId": "",
    "TargetUserId": "",
    "TargetUserIdName": "",
    "FriendshipId": "",
    "RequestId": "",
    "Token": ""
};

function friendDetailController($scope, $sce) {

    $(parent.window).scrollTop(0);

    var parentUrl = $.url(parent.document.location);
    var url = $.url(document.location);

    EditUserHelper.Token = parentUrl.param("token");
    $scope.token = parentUrl.param("token");

    EditUserHelper.TargetUserId = url.param("userid");

    $scope.attachmentUrl = attachmentUrl;
    $scope.ContentMessages = LanguageMessages[parent.IndexHelper.LanguageCode];

    $scope.showErrorHeader = false;
    $scope.showLoading = true;
    $scope.showMain = false;
    $scope.errorText = "";

    parent.IndexHelper.CheckSession(true, $scope.token, function (e) {
        $scope.$apply(function () {
            if (e.Success == true) {

                EditUserHelper.UserId = e.ReturnObject.PortalUserId;
                EditUserHelper.PortalId = e.ReturnObject.PortalId;

                $scope.userId = e.ReturnObject.PortalUserId;
                $scope.portalId = e.ReturnObject.PortalId;
            }
            else {
                return;
            }
        });
    });

    EditUserHelper.GetProfileInfo(function (e) {

        $scope.$apply(function () {
            var a = JSON.stringify(e);

            if (e.Success == true) {

                $scope.userInfo = e.ReturnObject;

                $scope.userInfo.ImageUrl = attachmentUrl + $scope.userInfo.Image;

                $scope.showErrorHeader = false;
                $scope.showLoading = false;
                $scope.showMain = true;

                EditUserHelper.CheckIsUserYourFriend(function (a) {

                    if (a.Success == true) {

                        $scope.$apply(function () {
                            $scope.isFriend = true;
                        });

                        EditUserHelper.FriendshipId = a.CrmId;
                    }
                    else {

                        $scope.$apply(function () {
                            $scope.isFriend = false;
                        });

                        EditUserHelper.HasUserRequestWithYou(function (b) {

                            if (b.Success == true) {

                                //$scope.$apply(function () {
                                //    $scope.hasUserRequest = true;
                                //});

                                EditUserHelper.RequestId = b.ReturnObject.Id;

                                if (b.ReturnObject.From != null && b.ReturnObject.From.Id.toLowerCase() == EditUserHelper.UserId.toLowerCase()) {

                                    $scope.$apply(function () {
                                        $scope.haveYouRequest = true;
                                    });
                                }
                                else if (b.ReturnObject.To != null && b.ReturnObject.To.Id.toLowerCase() == EditUserHelper.UserId.toLowerCase()) {

                                    $scope.$apply(function () {
                                        $scope.haveYouRequest = false;
                                        $scope.hasUserRequest = true;
                                    });
                                }
                                else {
                                    $scope.$apply(function () {
                                        $scope.hasNoRequest = true;
                                    });
                                }
                            }
                            else {

                                $scope.$apply(function () {
                                    $scope.hasUserRequest = false;
                                    $scope.hasNoRequest = true;
                                });
                            }
                        });
                    }
                });
            }
            else {
                $scope.showErrorHeader = true;
                $scope.showLoading = false;
                $scope.showMain = false;
                $scope.errorText = ReturnMessage(parent.IndexHelper.LanguageCode, e.Result);
            }
        });

    });

    $scope.CloseFriendship = function () {

        EditUserHelper.CloseFriendship(function (e) {

            if (e.Success) {
                parent.IndexHelper.ShowNotify(ReturnMessage(parent.IndexHelper.LanguageCode, e.Result), "success");

                document.location.reload();
            }
            else {
                parent.IndexHelper.ShowNotify(ReturnMessage(parent.IndexHelper.LanguageCode, e.Result), "error");

            }
        });
    };

    $scope.CreateFriendshipRequest = function () {

        EditUserHelper.CreateFriendshipRequest(function (e) {

            if (e.Success) {
                parent.IndexHelper.ShowNotify(ReturnMessage(parent.IndexHelper.LanguageCode, e.Result), "success");

                document.location.reload();
            }
            else {
                parent.IndexHelper.ShowNotify(ReturnMessage(parent.IndexHelper.LanguageCode, e.Result), "error");

            }
        });
    };

    $scope.CloseFriendshipRequest = function (statusCode) {

        //Deny:100000000
        //Accept:2
        //Cancel:100000001

        EditUserHelper.CloseFriendshipRequest(statusCode, function (e) {

            if (e.Success) {
                parent.IndexHelper.ShowNotify(ReturnMessage(parent.IndexHelper.LanguageCode, e.Result), "success");

                document.location.reload();
            }
            else {
                parent.IndexHelper.ShowNotify(ReturnMessage(parent.IndexHelper.LanguageCode, e.Result), "error");

            }
        });
    };
}