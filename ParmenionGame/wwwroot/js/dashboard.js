"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();

function errorReport(err) {
    return console.error(err.toString());
}

connection.on("ShowDashboardJoinGameCode", function (message) {
    $("#join-game-code").html(message);
    $("#game-ready").show();
    $("#game-over").hide();
});

connection.on("Countdown", function (message) {
    $("#countdown").html(message);
});

connection.on("ShowDashboardQuestionText", function (message) {
    $("#game-progress").show();
    $("#game-ready").hide();
    $("#question").html(message);
});

connection.on("ShowDashboardPlayerList", function (message) {
    $("#player-list").html(message);
});

connection.start().then(function () {
    connection.invoke("RegisterDashboard");
}).catch(errorReport);

connection.on("ShowGameFinished", function (message) {
    $("#game-over").show();
    $("#game-progress").hide();
});