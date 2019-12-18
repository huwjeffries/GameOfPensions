"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();

function errorReport(err) {
    return console.error(err.toString());
}

connection.on("JoinGameCode", function (message) {
    document.getElementById("join-game-code").innerHTML = message;
});

connection.on("JoinGameCountdown", function (message) {
    document.getElementById("join-game-countdown").innerHTML = message;
});

connection.on("ShowQuestion", function (message) {
    document.getElementById("join-game-code").hidden = true;
    document.getElementById("question").innerHTML = message;
});

connection.on("UpdatePlayerList", function (message) {
    document.getElementById("player-list").innerHTML = message;
});

connection.start().then(function () {
    connection.invoke("RegisterDashboard");
}).catch(errorReport);
