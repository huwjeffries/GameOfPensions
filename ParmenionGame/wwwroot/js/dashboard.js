"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();

function errorReport(err) {
    return console.error(err.toString());
}

connection.on("ShowDashboardJoinGameCode", function (message) {
    $("#join-game-code").html(message);
    $("#join-game-code").show();
});

connection.on("Countdown", function (message) {
    $("#countdown").html(message);
});

connection.on("ShowDashboardQuestionText", function (message) {
    $("#join-game-code").hide();
    $("#question").show();
    $("#question").html(message);
});

connection.on("ShowDashboardPlayerList", function (message) {
    $("#player-list").show();
    $("#player-list").html(message);
});

connection.start().then(function () {
    connection.invoke("RegisterDashboard");
}).catch(errorReport);

connection.on("ShowGameFinished", function (message) {
    $("#game-finished-view").show();
});