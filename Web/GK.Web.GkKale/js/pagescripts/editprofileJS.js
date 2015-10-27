/// <reference path="../jquery/jquery.min.js" />
/// <reference path="../jquery/jquery.widget.min.js" />
/// <reference path="../jquery/purl.js" />
/// <reference path="GlobalVariables.js" />
/// <reference path="indexJS.js" />
/// <reference path="mainJS.js" />

var EditProfileHelper = {
    "OnLoad": function () {
        $(parent.window).scrollTop(0);

        EditProfileHelper.OnClickEvents();
        EditProfileHelper.OnChangeEvents();

        parent.IndexHelper.AutoResize("ifrmContent");

        $('#txtMobilePhone').mask("999-9999999");
        $('#txtWorkPhone').mask("999-9999999");
    },
    "OnClickEvents": function () {
    },
    "OnChangeEvents": function () {
    },
    "GetProfileInfo": function (callBackFunction) {
        //debugger;
        var jData = {};

        jData.token = EditProfileHelper.Token;

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
    "AddFile": function () {
        var filepath = $("#txt_file").val();
        if (filepath != null && filepath != "") {
            var file = $("#file")[0].files[0];

            var file = $("#file")[0].files[0];
            var size = (file.size / 1024) / 1024;
            if (size > 4) {
                parent.IndexHelper.ShowAlertDialog(2, ReturnMessage(IndexHelper.LanguageCode, "M060"), ReturnMessage(IndexHelper.LanguageCode, "M077"));
                return false;
            }

            var reader = new FileReader();

            reader.onloadend = function () {
                var data = reader.result;
                data = data.substr(data.indexOf('base64') + 7);

                if (file.name.toLowerCase().indexOf(".jpg") < 0 && file.name.toLowerCase().indexOf(".jpeg") < 0) {
                    $("#lbl_message").text(ReturnMessage(IndexHelper.LanguageCode, "M078"));
                    parent.IndexHelper.ShowAlertDialog(2, ReturnMessage(IndexHelper.LanguageCode, "M059"), ReturnMessage(IndexHelper.LanguageCode, "M078"));
                    return;
                }

                EditProfileHelper.ImageBase64Data = data;

                $("#btnProfileImage img").attr("src", "data:" + file.type + ";base64," + data).attr("mimeType", file.type).attr("fileName", file.name);;
            }

            reader.readAsDataURL(file);

            $("#file").val(null);
        }
        else {
            parent.IndexHelper.ShowAlertDialog(2, ReturnMessage(IndexHelper.LanguageCode, "M060"), ReturnMessage(IndexHelper.LanguageCode, "M079"));
        }
    },
    "UpdateProfile": function (contactInfo, callBackFunction) {
        //debugger;
        var oldPass = contactInfo.OldPassword;
        var newPass = contactInfo.NewPassword;
        var reType = contactInfo.PasswordReType;

        var jData = {};

        jData.newPassword = newPass;
        jData.oldPassword = oldPass;

        if (oldPass != null && oldPass != "" && oldPass != undefined) {
            if (newPass != null && newPass != "" && newPass != undefined) {
                if (newPass != reType) {

                    parent.IndexHelper.ToastrShow("warning", ReturnMessage(parent.IndexHelper.LanguageCode, "M080"), "Şifre Doğrulama");
                    return;
                }
            }
            else {

                parent.IndexHelper.ToastrShow("warning", ReturnMessage(parent.IndexHelper.LanguageCode, "M081"), "Yeni Şifre");
                return;
            }
        }
        else {

            if (newPass != null && newPass != "" && newPass != undefined) {

                parent.IndexHelper.ToastrShow("warning", "Lütfen şifre bilgilerinin doğruluğunu kontrol ediniz!", "Şifre Kontrol");
                return;
            }
        }

        if (contactInfo.MarkContact != null && contactInfo.MarkContact != undefined) {
            if (contactInfo.MarkContact == "1")
                contactInfo.MarkContact = true;
            else if (contactInfo.MarkContact == "0")
                contactInfo.MarkContact = false;
            else
                contactInfo.MarkContact = false;
        }

        jData.token = EditProfileHelper.Token;

        jData.contact = {};
        jData.contact = contactInfo;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/UpdateMyProfile",
            async: true,
            dataType: "json",
            contentType: "application/json;",
            type: "POST",
            data: jSonData,
            beforeSend: function () {
                parent.IndexHelper.ShowLoading("Bilgiler güncelleniyor...", null);
            },
            complete: function () {
                parent.IndexHelper.AutoResize("ifrmContent");
                parent.IndexHelper.CloseDialog();
            },
            success: function (data) {
                //debugger;
                if (data != null) {

                    callBackFunction(data);

                    return;
                }
                else {
                    parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), ReturnMessage(IndexHelper.LanguageCode, "M002") + "<br />UpdateProfile");
                    return false;
                }
            },
            error: function (a, b, c) {
                debugger;
                parent.IndexHelper.ShowAlertDialog(3, ReturnMessage(IndexHelper.LanguageCode, "M059"), c + "<br />UpdateProfile");
                return false;
            }
        });

    },
    "ReadImage": function (input, callBackFunction) {

        if (input.files && input.files[0]) {

            var reader = new FileReader();

            var file = input.files[0];

            reader.readAsDataURL(input.files[0]);

            reader.onload = function (e) {

                callBackFunction(e);

                //$("#imageThumb").attr('src', e.target.result);

                var fileSize = parseInt(file.size / 1024);
                var fileName = file.name;

                var imageFile = {};

                imageFile.blob = file;
                imageFile.fileName = file.name;

                EditProfileHelper.ImageFile = imageFile;

            }
        }

        reader.readAsDataURL(input.files[0]);
    },
    "PostFile": function (formData, callBackFunction) {
        $.ajax({
            url: "uploadHelper.ashx?operation=1&userid=" + EditProfileHelper.UserId,
            type: 'POST',
            async: true,
            complete: function () {
            },
            progress: function (evt) {
            },
            beforeSend: function (e) {
            },
            success: function (data) {
                if (data != null) {

                    callBackFunction(data);

                    return;

                    if (data.Success == true) {

                    }
                    else {
                    }
                }
                else {
                }
            },
            error: function (e) {
            },
            data: formData,
            cache: false,
            contentType: false,
            processData: false
        });
    },
    "GetCitites": function (callBackFunction) {
        //debugger;
        var jData = {};

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/GetCities",
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
    "GetTowns": function (cityId, callBackFunction) {
        //debugger;
        var jData = {};

        jData.cityId = cityId;

        var jSonData = JSON.stringify(jData);

        //debugger;
        $.ajax({
            url: CustomServiceUrl + "CrmService.svc/GetTowns",
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
    "ImageFile": null,
    "UserId": "",
    "PortalId": "",
    "ImageBase64Data": "",
    "ContactId": "",
    "Token": ""
};

function editProfileController($scope, $sce) {

    $.mask.definitions['~'] = "[+-]";
    $("#txtMobilePhone").mask("+90-888-8888888");
    $("#txtWorkPhone").mask("+90-888-8888888");

    $(parent.window).scrollTop(0);

    var parentUrl = $.url(parent.document.location);
    var url = $.url(document.location);

    EditProfileHelper.Token = parentUrl.param("token");
    $scope.token = parentUrl.param("token");

    parent.IndexHelper.AutoResize("ifrmContent");

    $scope.attachmentUrl = attachmentUrl;
    $scope.ContentMessages = LanguageMessages[parent.IndexHelper.LanguageCode];

    $scope.showErrorHeader = false;
    $scope.showLoading = true;
    $scope.showMain = false;
    $scope.errorText = "";

    $("#fileUpload").change(function () {

        EditProfileHelper.ReadImage(this, function (e) {

            $scope.$apply(function () {
                $scope.userInfo.ImageUrl = e.target.result;
            });

        });
    });

    parent.IndexHelper.CheckSession(true, $scope.token, function (e) {
        $scope.$apply(function () {
            if (e.Success == true) {

                EditProfileHelper.UserId = e.ReturnObject.PortalUserId;
                EditProfileHelper.PortalId = e.ReturnObject.PortalId;

                $scope.userId = e.ReturnObject.PortalUserId;
                $scope.portalId = e.ReturnObject.PortalId;
            }
            else {
                return;
            }
        });
    });

    $scope.GetProfileInfo = function () {
        EditProfileHelper.GetProfileInfo(function (e) {

            $scope.$apply(function () {
                var a = JSON.stringify(e);

                if (e.Success == true) {

                    $scope.userInfo = e.ReturnObject;

                    if ($scope.userInfo.ContactInfo.MarkContact == null || $scope.userInfo.ContactInfo.MarkContact == undefined)
                        $scope.userInfo.ContactInfo.MarkContact = "-1";
                    else if ($scope.userInfo.ContactInfo.MarkContact == true)
                        $scope.userInfo.ContactInfo.MarkContact = "1";
                    else if ($scope.userInfo.ContactInfo.MarkContact == false)
                        $scope.userInfo.ContactInfo.MarkContact = "0";

                    if ($scope.userInfo.ContactInfo.CityId != null) {

                        var elementPos = $scope.cities.map(function (x) { return x.Id; }).indexOf($scope.userInfo.ContactInfo.CityId.Id);

                        $scope.userInfo.ContactInfo.CityId = $scope.cities[elementPos];

                        $scope.GetTowns($scope.userInfo.ContactInfo.CityId.Id);
                    }

                    $scope.userInfo.ImageUrl = attachmentUrl + $scope.userInfo.Image;

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
    };

    $scope.GetTowns = function (cityId) {

        EditProfileHelper.GetTowns(cityId, function (e) {

            $scope.$apply(function () {

                $scope.towns = e;

                if ($scope.userInfo.ContactInfo.TownId != null) {
                    var elementPos = $scope.towns.map(function (x) { return x.Id; }).indexOf($scope.userInfo.ContactInfo.TownId.Id);

                    $scope.userInfo.ContactInfo.TownId = $scope.towns[elementPos];
                }
            });
        });
    };

    EditProfileHelper.GetCitites(function (e) {

        $scope.$apply(function () {

            $scope.cities = e;

            setTimeout(function () {
                $scope.GetProfileInfo();
            }, 500);
        });

    });

    $scope.UpdateProfile = function () {

        EditProfileHelper.UpdateProfile($scope.userInfo.ContactInfo, function (e) {

            if (e.Success == true) {

                if (EditProfileHelper.ImageFile != null) {
                    var formData = new FormData();
                    formData.append('file', EditProfileHelper.ImageFile.blob, EditProfileHelper.ImageFile.fileName);

                    EditProfileHelper.PostFile(formData, function (data) {

                        if (data.Success == true) {

                            parent.IndexHelper.ShowDialog(ReturnMessage(parent.IndexHelper.LanguageCode, e.Result), "Başarılı", null, true, function () {

                                parent.document.location.reload();
                            });
                        }
                        else {
                            parent.IndexHelper.ShowNotify(data.Result, "error");
                        }
                    });
                }

            }
            else {
                parent.IndexHelper.ToastrShow("error", ReturnMessage(parent.IndexHelper.LanguageCode, e.Result), "Hata");
            }
        });
    };

    $scope.OpenFileDialog = function () {
        $("#fileUpload").click();
    };

    $scope.CityChange = function () {

        var city = $scope.userInfo.ContactInfo.CityId

        $scope.GetTowns(city.Id);
    };
}