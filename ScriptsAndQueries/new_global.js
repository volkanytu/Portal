/// <reference path="../web/gk.web.gkportal/scripts/jquery/jquery.min.js" />
/// <reference path="../web/gk.web.gkportal/scripts/jquery/jquery.widget.min.js" />
/// <reference path="../web/gk.web.gkportal/scripts/jquery/purl.js" />
/// <reference path="../web/gk.web.gkportal/scripts/pagescripts/indexjs.js" />


var GlobalHelper = {
    "LoadJsCssFile": function (filename, filetype) {
        if (filetype == "js") {
            var fileref = document.createElement('script');
            fileref.setAttribute("type", "text/javascript");
            fileref.setAttribute("src", filename);
        }
        else if (filetype == "css") {
            var fileref = document.createElement("link");
            fileref.setAttribute("rel", "stylesheet");
            fileref.setAttribute("type", "text/css");
            fileref.setAttribute("href", filename);
        }
        if (typeof fileref != "undefined")
            document.getElementsByTagName("head")[0].appendChild(fileref);
    },
    "LoadScript": function (url, callback) {
        var script = document.createElement("script");
        script.type = "text/javascript";

        if (script.readyState) { //IE
            script.onreadystatechange = function () {

                if (script.readyState == "loaded" || script.readyState == "complete") {
                    script.onreadystatechange = null;
                    callback();
                }
            };
        }
        else {
            script.onload = function () {
                callback();
            };
        }

        script.src = url;
        document.getElementsByTagName("head")[0].appendChild(script);
    }
};