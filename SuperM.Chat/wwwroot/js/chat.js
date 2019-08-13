"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
var current_token = "";
//Disable send button until connection is established
//document.getElementById("sendButton").disabled = true;
connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});

//connection
connection.on("LoginIn", function (token) {
    current_token = token;
    var encodedMsg = " Token: " + token;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.on("LoginOut", function (message) {
    var encodedMsg = " LoginOut Message: " + message;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.on("SendSingle", function (message) {
    var encodedMsg = " SendSingle Message: " + message;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.on("Send", function (message) {
    var encodedMsg = " Send Message: " + message;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

//click
document.getElementById("loginInButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    connection.invoke("LoginIn", user,"").catch(function (err) {
        return console.error(err.toString());
    });
});

document.getElementById("loginOutButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    connection.invoke("LoginOut", current_token).catch(function (err) {
        return console.error(err.toString());
    });
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var message = document.getElementById("messageInput").value;
    connection.invoke("Send", current_token,  message,"1").catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("sendSingleButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var connectUser = document.getElementById("connectUser").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendSingle", current_token, connectUser, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("stop").addEventListener("click", function (event) {
    connection.stop();
});

