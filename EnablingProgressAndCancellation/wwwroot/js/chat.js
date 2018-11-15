
"use strict";

let connection = new signalR.HubConnectionBuilder().withUrl("/realtimeupdate").build();

connection.on("UpdateProgressBar", function (user, message) {
    console.log("In UpdateProgressBar");
    let msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    let encodedMsg = user + " says " + msg;
    let li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.start().catch(function(err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function(event) {
    let user = document.getElementById("userInput").value;
    let message = document.getElementById("messageInput").value;
    connection.invoke("SendRealtimeMessage", message).catch(function(err) {
        return console.error(err.toString());
    });
    event.preventDefault();
})