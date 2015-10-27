/// <reference path="../jquery/jquery.min.js" />
/// <reference path="../jquery/jquery.widget.min.js" />
/// <reference path="../jquery/purl.js" />
/// <reference path="GlobalVariables.js" />
/// <reference path="indexJS.js" />
/// <reference path="mainJS.js" />

var _interval;

var CubeQuestionHelper = {
    "OnLoad": function () {
        $(parent.window).scrollTop(0);

        if (CubeQuestionHelper.IsTrust == "true") {
            $("#isTrust").show();
            $("#txtPoint").parent().addClass("fg-red");
        }
        else
            $("#isTrust").hide();

        CubeQuestionHelper.OnClickEvents();
        CubeQuestionHelper.OnChangeEvents();

        parent.IndexHelper.AutoResize("ifrmContent");
    },
    "OnClickEvents": function () {
    },
    "OnChangeEvents": function () {
    },
    "OnPageUnLoad": function () {

        window.onbeforeunload = function () {
            if (CubeQuestionHelper.IsFromButton == false) {

                CubeQuestionHelper.SaveQuestion(true, undefined, CubeQuestionHelper.Point, null, function (e) {

                });
            }
        };
    },
    "SelectQuestion": function (callBackFunction) {
        //debugger;
        var jData = {};
        jData.token = CubeQuestionHelper.Token;

        jData.questionLevelId = CubeQuestionHelper.QuestionLevelId;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/SelectQuestion",
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
                    $("#lblErrorMain").html(ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />SelectQuestion");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                CubeQuestionHelper.IsFromButton = true;
                parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), c + "<br />SelectQuestion");
                return false;
            }
        });
    },
    "StartTimer": function (countdownSecond, callbackFunction) {
        var time = countdownSecond;
        var timeStr = time.toString();
        var timeLength = timeStr.length;

        html = "";
        for (var i = 0; i < timeLength; i++) {
            html += "<span class='timerText timerText1'>" + timeStr[i] + "</span>";
            //html += "<span class='digit-wrapper'>";
            //html += "<span class='digit bg-dark fg-white' style='top: 0px; opacity: 1;'>" + timeStr[i] + "</span>";
            //html += "</span>";
        }

        $(".timer").after(html);

        _interval = setInterval(function () {

            time--;

            timeStr = time.toString();
            var timeLengthNew = timeStr.length;
            var lengthDiff = timeLength - timeLengthNew;

            if (lengthDiff > 0)
                $(".timerText").eq(lengthDiff - 1).html("0");

            for (var i = 0; i < timeLength; i++) {
                $(".timerText").eq(i + lengthDiff).html(timeStr[i]);
            }
            if (time == 0) {
                clearInterval(_interval);
                callbackFunction();
            }

        }, 1000);
    },
    "UserId": "",
    "PortalId": "",
    "QuestionLevelId": "",
    "IsTrust": "",
    "QuestionId": "",
    "IsFromButton": false,
    "Point": 0,
    "SaveQuestion": function (isRefreshOrBack, isTimeOverlap, point, choiceId, callBackFunction) {
        clearInterval(_interval);

        //debugger;
        var jData = {};

        jData.token = CubeQuestionHelper.Token;

        jData.answer = {};

        jData.answer.Point = point;

        jData.answer.User = {};
        jData.answer.User.Id = CubeQuestionHelper.UserId;
        jData.answer.User.Name = parent.IndexHelper.UserName;
        jData.answer.User.LogicalName = "new_user";

        if (choiceId != undefined && choiceId != null && isRefreshOrBack == undefined && isTimeOverlap == undefined) {
            jData.answer.Choice = {};
            jData.answer.Choice.Id = choiceId;
            jData.answer.Choice.LogicalName = "new_questionchoice";
        }

        jData.answer.Question = {};
        jData.answer.Question.Id = CubeQuestionHelper.QuestionId;
        jData.answer.Question.LogicalName = "new_question";

        jData.answer.Portal = {};
        jData.answer.Portal.Id = CubeQuestionHelper.PortalId;
        jData.answer.Portal.LogicalName = "new_portal";

        if (CubeQuestionHelper.IsTrust != undefined && CubeQuestionHelper.IsTrust != "undefined" && CubeQuestionHelper.IsTrust != "")
            jData.answer.IsTrust = CubeQuestionHelper.IsTrust;

        if (isRefreshOrBack == true) {
            jData.answer.IsRefreshOrBack = true;
        }

        if (isTimeOverlap == true) {
            jData.answer.IsTimeOverlap = true;
        }

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/SaveAnswer",
            async: (isRefreshOrBack == true || isTimeOverlap == true) ? false : true,
            dataType: "json",
            contentType: "application/json;",
            type: "POST",
            data: jSonData,
            beforeSend: function () {
                if (isTimeOverlap != true && isRefreshOrBack != true) {
                    parent.IndexHelper.ShowLoading("Cevabınız kontrol ediliyor...");
                }
            },
            complete: function () {
            },
            success: function (data) {
                //debugger;
                if (data != null) {

                    parent.IndexHelper.CloseDialog();
                    if (isTimeOverlap == true || isRefreshOrBack == true) {

                        var closeFunction = null;

                        if (isTimeOverlap) {
                            closeFunction = function () {
                                $("#ifrmContent", parent.document).attr("src", "bilgiKupu.html");
                            };
                        }

                        parent.IndexHelper.ShowDialog(ReturnMessage(parent.IndexHelper.LanguageCode, data.Result), null, null, true, closeFunction);

                    }
                    else {
                        callBackFunction(data);

                        return;
                    }
                }
                else {
                    parent.$.Dialog.close();
                    parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />SaveQuestionWithError");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), c + "<br />SaveQuestionWithError");
                return false;
            }
        });
    },
    "Token": ""
};

function DisableRightClick() {

    function clickIE() {
        if (document.all) {
            (message);
            return false;
        }
    }

    function clickNS(e) {
        if (document.layers || (document.getElementById && !document.all)) {
            if (e.which == 2 || e.which == 3) {

                parent.IndexHelper.ShowNotify(ReturnMessage(parent.IndexHelper.LanguageCode, "M073"), "error");
                return false;
            }
        }
    }

    if (document.layers) {
        document.captureEvents(Event.MOUSEDOWN);
        document.onmousedown = clickNS;
    } else {
        document.onmouseup = clickNS;
        document.oncontextmenu = clickIE;
    }

    document.oncontextmenu = new Function("return false");
}

function cubeQuestionController($scope, $sce) {

    $scope.volkan = null;

    $(parent.window).scrollTop(0);

    DisableRightClick();

    CubeQuestionHelper.OnPageUnLoad();

    var parentUrl = $.url(parent.document.location);
    var url = $.url(document.location);

    CubeQuestionHelper.Token = parentUrl.param("token");
    $scope.token = parentUrl.param("token");

    CubeQuestionHelper.QuestionLevelId = url.param("objectid");
    CubeQuestionHelper.IsTrust = url.param("istrust");

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

                CubeQuestionHelper.UserId = e.ReturnObject.PortalUserId;
                CubeQuestionHelper.PortalId = e.ReturnObject.PortalId;

                $scope.userId = e.ReturnObject.PortalUserId;
                $scope.portalId = e.ReturnObject.PortalId;
            }
            else {
                return;
            }
        });
    });

    CubeQuestionHelper.SelectQuestion(function (e) {

        $scope.$apply(function () {
            var a = JSON.stringify(e);

            if (e.Success == true) {

                $scope.questionInfo = e.ReturnObject;
                CubeQuestionHelper.QuestionId = e.ReturnObject.Id;
                CubeQuestionHelper.Point = e.ReturnObject.Point;

                CubeQuestionHelper.StartTimer(e.ReturnObject.Time, function () {

                    $scope.SaveAnswer(undefined, true);
                });

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


    $scope.SaveAnswer = function (isRefreshOrBack, isTimeOverlap) {

        CubeQuestionHelper.IsFromButton = true;

        var choiceId = $scope.choiceId;
        var point = $scope.questionInfo.Point;

        CubeQuestionHelper.SaveQuestion(isRefreshOrBack, isTimeOverlap, point, choiceId, function (e) {

            parent.IndexHelper.ShowDialog(ReturnMessage(parent.IndexHelper.LanguageCode, e.Result), null, null, true, function () {
                $("#ifrmContent", parent.document).attr("src", "bilgiKupu.html");
            });
        });
    };
}