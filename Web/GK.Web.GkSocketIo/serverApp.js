var _ = require("underscore");
var express = require("express");
var app = express();
var server = require("http").createServer(app);
var io = require("socket.io").listen(server);
var users = {};
server.listen(5555);

var sql = require('mssql');

var config = {
    user: "CrmSqlUser",
    password: "CrmSqlPass",
    server: "SQLSERVER", // You can use 'localhost\\instance' to connect to named instance 
    database: "KALE_MSCRM",
    stream: true, // You can enable streaming globally 
    options: {
    }
}

app.use('/static', express.static(__dirname));
app.use('/styles', express.static(__dirname + "/styles"));
app.use('/images', express.static(__dirname + "/images"));

app.get("/", function (req, res) {

    res.sendFile(__dirname + "/login.html");
});

app.get("/chat", function (req, res) {

    res.sendFile(__dirname + "/chat.html");
});

app.get("/sql", function (req, res) {

    res.sendFile(__dirname + "/index2.html");

    //ExecuteSqlQuery('select * from new_message', function (e) {

    //    console.log("Row Count:" + e.length);
    //});
});

app.get("/:userId", function (req, res) {

    userId = req.params.userId;

    res.sendFile(__dirname + "/index2.html");
});

io.sockets.on("connection", function (socket) {

    socket.on("user_login", function (userId) {

        if (userId in users) {


        }
        else {
            socket.userId = userId;

            users[userId] = socket;
        }

        StatusUpdate(socket.userId, true);

        var userCount = Object.keys(users).length;

        console.log("Connected User:" + userId);
        console.log("UserCount:" + userCount);

    });

    socket.on("start_chatwith", function (data, callBack) {

        if (data in users) {

            callBack({ "Success": true, "Result": "Online!" });
        }
        else {

            callBack({ "Success": false, "Result": "Off-line!" });
        }

        UpdateMessagesAsSeen(socket.userId, data, function (e) {

        });

        socket.targetUserId = data;
    });

    socket.on("send_new_message", function (data, callBack) {

        if (data) {
            if (socket.targetUserId in users) {

                users[socket.targetUserId].emit("new_message", data);

                InsertMessageToSql(socket.userId, socket.targetUserId, data, 1, function (e) {

                });

                callBack({ "Success": true, "Result": data });

            }
            else {
                InsertMessageToSql(socket.userId, socket.targetUserId, data, 0, function (e) {

                });

                users[socket.targetUserId].emit("has_message", data);
                callBack({ "Success": true, "Result": "The user is off-line!" });
            }
        }
        else {

            callBack({ "Success": false, "Result": "Please write a message!" });
        }
    });

    socket.on("get_old_messages", function (data, callBack) {

        GetOldMessagesFromSql(socket.userId, socket.targetUserId, function (e) {

            callBack(e);
        });
    });

    socket.on("get_unread_messages", function (data, callBack) {

        GetUnReadMessagesFromSql(socket.userId, function (e) {

            callBack(e);
        });
    });


    socket.on("disconnect", function (data) {

        if (!socket.userId) return;

        delete users[socket.userId];

        StatusUpdate(socket.userId, false);

        var userCount = Object.keys(users).length;

        console.log("Disconnected User:" + socket.userId);
        console.log("UserCount:" + userCount);
    });

    socket.on("user_writing", function (data) {

        UserWriting(socket.userId, data);
    });

    function StatusUpdate(userId, status) {

        var filtered = _.where(users, { targetUserId: userId });

        console.log("StatusLength:" + filtered.length);

        for (var i = 0; i < filtered.length; i++) {

            filtered[i].emit("update_target_status", { "Status": status });

        }
    }

    function UserWriting(userId, isWriting) {
        var filtered = _.where(users, { targetUserId: userId });

        //console.log("StatusLength:" + filtered.length);

        for (var i = 0; i < filtered.length; i++) {

            filtered[i].emit("update_user_writing", isWriting);

        }
    }
});

function GetOldMessagesFromSql(fromId, toId, callBack) {

    var sqlQuery = "";
    sqlQuery += "SELECT";
    sqlQuery += " TOP 20";
    sqlQuery += " *";
    sqlQuery += " FROM";
    sqlQuery += " v_Messages";
    sqlQuery += " WHERE";
    sqlQuery += " (ToId='" + fromId + "' AND FromId='" + toId + "')";
    sqlQuery += " OR";
    sqlQuery += " (ToId='" + toId + "' AND FromId='" + fromId + "')";
    sqlQuery += " ORDER BY";
    sqlQuery += "	CreatedOn DESC";

    ExecuteSqlQuery(sqlQuery, function (e) {
        callBack(e);
    });
}

function GetUnReadMessagesFromSql(userId, callBack) {

    var sqlQuery = "";
    sqlQuery += "SELECT";
    sqlQuery += " TOP 20";
    sqlQuery += " *";
    sqlQuery += " FROM";
    sqlQuery += " v_Messages";
    sqlQuery += " WHERE";
    sqlQuery += " ToId='" + userId + "'";
    sqlQuery += " AND";
    sqlQuery += " HasSeen=0";
    sqlQuery += " ORDER BY";
    sqlQuery += "	CreatedOn DESC";

    ExecuteSqlQuery(sqlQuery, function (e) {
        callBack(e);
    });
}

function InsertMessageToSql(fromId, toId, message, hasSeen, callBack) {
    var returnObject = [];

    var sqlQuery = "";
    sqlQuery += "INSERT ";
    sqlQuery += "INTO ";
    sqlQuery += " Tbl_Messages (ToId, FromId, PortalId, [Content],HasSeen)";
    sqlQuery += " VALUES (@toId, @fromId, '068B1218-632E-E511-80C4-000D3A216510', @message, @hasSeen)";

    sql.connect(config, function (err) {
        // ... error checks 

        var request = new sql.Request();

        request.input('toId', toId);
        request.input('fromId', fromId);
        request.input('message', message);
        request.input('hasSeen', hasSeen);

        request.stream = true; // You can set streaming differently for each request 
        request.query(sqlQuery); // or request.execute(procedure); 

        request.on('row', function (row) {
            // Emitted for each row in a recordset 
            //console.log("ROW:" + JSON.stringify(row));
            returnObject.push(row);
        });

        request.on('done', function (returnValue) {

            callBack(returnObject);
        });

        request.on('error', function (err) {
            // May be emitted multiple times 
            console.log("Request Error:" + err);
        });
    });

    sql.on('error', function (err) {
        console.log("SQL Error:" + err);
        // ... error handler 
    });

    //ExecuteSqlQuery(sqlQuery, function (e) {
    //    callBack(e);
    //});
}

function UpdateMessagesAsSeen(fromId, toId, callBack) {
    var returnObject = [];

    var sqlQuery = "";
    sqlQuery += "UPDATE Tbl_Messages SET HasSeen=1 WHERE ToId='" + fromId + "' AND FromId='" + toId + "' AND HasSeen=0 ";

    ExecuteSqlQuery(sqlQuery, function (e) {
        callBack(e);
    });
}

function ExecuteSqlQuery(query, callBack) {

    var returnObject = [];

    sql.connect(config, function (err) {
        // ... error checks 

        var request = new sql.Request();

        request.stream = true; // You can set streaming differently for each request 
        request.query(query); // or request.execute(procedure); 

        request.on('row', function (row) {
            // Emitted for each row in a recordset 
            //console.log("ROW:" + JSON.stringify(row));
            returnObject.push(row);
        });

        request.on('done', function (returnValue) {

            callBack(returnObject);
        });

        request.on('error', function (err) {
            // May be emitted multiple times 
            console.log("Request Error:" + err);
        });
    });

    sql.on('error', function (err) {
        console.log("SQL Error:" + err);
        // ... error handler 
    });
}