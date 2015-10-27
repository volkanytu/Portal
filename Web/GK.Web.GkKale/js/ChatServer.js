var fromId;
var toId;
var portalId = "";

var http = require('http');

//var fs = require('fs');


//var options = {
//  pfx: fs.readFileSync('4bizcrm.pfx'),
//  passphrase: '1234'
//};

var server = http.createServer(function (req, res) {
    res.writeHead(200, { 'Content-Type': 'text/plain' });
    res.end('Chat server is working...\n');
}).listen(5555);
 var io = require('socket.io')(server);

// listen for new web clients:
//server.listen(8080);

//var client = require("socket.io").listen(server).sockets;


io.on("connection", function (socket) {

    console.log("Connected");

    var handshakeData = socket.request;

    fromId = handshakeData._query['from'];
    toId = handshakeData._query['to'];
    portalId = handshakeData._query['portalid'];

    console.log("Messaging: " + fromId + " - " + toId + "-P:" + portalId);

    if (fromId != null && fromId != "" && fromId != undefined && toId != null && toId != "" && toId != undefined) {
        CheckOldMessages(function (e) {

            socket.emit("oldmessages", e);

        }, fromId, toId);
    }
    socket.on("input", function (data) {

        console.log("Message Data Received");

        InsertNewMessage(function (e) {

            client.emit("message", e);

        }, data);

    });
});

function CheckOldMessages(callBackFunction, fromUserId, toUserId) {
    var jData = {
        portalId: portalId,
        from: fromId,
        to: toId
    };

    var jSonData = JSON.stringify(jData);

    var headers = {
        'Content-Type': 'application/json',
        'Content-Length': jSonData.length
    };

    var options = {
        host: 'kaleanahtarcilarkulubu.com.tr',
        port: 80,
        path: '/WebServices/CrmService.svc/GetOldMessages',
        method: 'POST',
        headers: headers
    };

    var req = http.request(options, function (res) {
        res.setEncoding('utf-8');

        var responseString = '';

        res.on('data', function (data) {
            responseString += data;
        });

        res.on('end', function () {

            var resultObject = JSON.parse(responseString);

            callBackFunction(resultObject);

            console.log(resultObject.length);
        });
    });

    req.on('error', function (e) {
        console.log("HATA");
    });

    req.write(jSonData);
    req.end();
}

function InsertNewMessage(callBackFunction, comingData) {

    var jData = comingData;

    var jSonData = JSON.stringify(jData);

    var headers = {
        'Content-Type': 'application/json',
        'Content-Length': jSonData.length
    };

    var options = {
        host: 'kaleanahtarcilarkulubu.com.tr',
        port: 80,
        path: '/WebServices/CrmService.svc/CreateMessage',
        method: 'POST',
        headers: headers
    };

    var req = http.request(options, function (res) {
        res.setEncoding('utf-8');

        var responseString = '';

        res.on('data', function (data) {
            responseString += data;
        });

        res.on('end', function () {
            var resultObject = JSON.parse(responseString);

            callBackFunction(resultObject);

            console.log("New Message Inserted");
        });
    });

    req.on('error', function (e) {
        console.log("HATA");
    });

    req.write(jSonData);
    req.end();
}


