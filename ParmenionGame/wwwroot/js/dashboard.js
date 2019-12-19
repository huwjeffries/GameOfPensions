"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();

function errorReport(err) {
    return console.error(err.toString());
}

connection.on("ShowDashboardJoinGameCode", function (message) {
    document.getElementById("join-game-code").innerHTML = message;
});

connection.on("Countdown", function (message) {
    document.getElementById("countdown").innerHTML = message;
});

connection.on("ShowDashboardQuestionText", function (message) {
    document.getElementById("join-game-code").hidden = true;
    document.getElementById("question").innerHTML = message;
});

connection.on("ShowDashboardPlayerList", function (message) {
    document.getElementById("player-list").innerHTML = message;
});

connection.start().then(function () {
    connection.invoke("RegisterDashboard");
}).catch(errorReport);
