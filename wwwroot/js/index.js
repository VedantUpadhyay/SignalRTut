"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/hub").build();
var audioElement;
$("#getOnlineUsers").click(function () {
    connection.invoke("GetUsers").then(function (response) {
        console.log(response);
    })
});



connection.on("ReceiveMessage", function (message) {

    var audio = new Audio('notification.mp3');
    audio.muted = true;
    audio.autoplay = true;
    audio.play();
    var li = $("<li>");
    li.text(`${message}`);
    li.addClass("bg-primary");
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
    connection.invoke("Announce",userName,  messageToSend).then(function () {
        console.log("Success!");
        var li = $("<li>");
        li.text(`${messageToSend}`);
        li.addClass("bg-success");
        $("#msgs").append(li);
    }).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});