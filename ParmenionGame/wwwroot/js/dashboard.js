"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();
var flipclock;

function errorReport(err) {
    return console.error(err.toString());
}

connection.on("ShowDashboardJoinGameCode", function (message) {
    $("#join-game-code").html(message.toUpperCase());
    $("#game-ready").show();
    $("#game-over").hide();
    $(".player-list").html("");
});

connection.on("Countdown", function (message) {
    var seconds = message % 60;
    var minutes = (message - seconds) / 60;
    var countdownTime;
    if (minutes > 0) {
        countdownTime = minutes + "m " + seconds + "s";
    }
    else {
        countdownTime = seconds + "s";
    }
    $(".countdown").html(countdownTime);
});

connection.on("ShowDashboardQuestionText", function (message) {
    $("#game-progress").show();
    $("#game-ready").hide();
    $("#question").html(message);
});

connection.on("ShowDashboardPlayerList", function (message) {
    $(".player-list").html(message);
});

connection.start().then(function () {
    connection.invoke("RegisterDashboard");
}).catch(errorReport);

connection.on("ShowGameFinished", function (message) {
    $("#game-over").show();
    $("#game-progress").hide();
});