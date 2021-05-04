"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/hub").build();

$("#getOnlineUsers").click(function () {
    connection.invoke("GetUsers").then(function (response) {
        console.log(response);
    })
});
connection.on("ReceiveMessage", function (user,message) {
    var li = $("<li>");
    li.text(`${user} : ${message}`);
    $("#msgs").append(li);
});

connection.on("printConn", function (connTime) {
    console.log("Connected @ " + connTime);
});

connection.on("printDisconn", function (disConnTime) {
    console.log("Disconnected @ " + disConnTime);
});

connection.start().then(function () {
    console.log("Connected!");
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendBtn").addEventListener("click", function (event) {
    var userName = $("#userName").val();
    var messageToSend = $("#msg").val();
    connection.invoke("Announce", userName, messageToSend).then(function () {
        console.log("Success!");
    }).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});