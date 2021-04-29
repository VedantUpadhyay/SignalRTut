"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/hub").build();


connection.on("ReceiveMessage", function (message) {
    var li = $("<li>");
    li.text(message);
    $("#msgs").append(li);
});

connection.start().then(function () {
    console.log("Connected!");
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendBtn").addEventListener("click", function (event) {
    
    connection.invoke("Announce", "Wassup!").then(function () {
        console.log("Success!");
    }).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});