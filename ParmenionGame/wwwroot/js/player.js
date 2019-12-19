"use strict";

//Todo - do we want withAutomaticReconnect? The player will pickup a new connectionId, so will look like a new player registering.
var connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").withAutomaticReconnect().build();
connection.serverTimeoutInMilliseconds = 120000; // 2 minutes

function errorReport(err) {
    return console.error(err.toString());
}

connection.start().then(function () {
    $("#loading").hide();
    connection.invoke("IsGameInProgress").then(function (r) {
        if (r) {
            $("#waiting-next-game-view").show();
        } else {
            $("#join-game-view").show();
        }
    });    
}).catch(errorReport);

connection.on("ShowPlayerIncorrectGameCode", function (message) {    
    $("#incorrect-game-code").show();
});

connection.on("ShowPlayerNewGameStarted", function (message) {
    $("#waiting-next-game-view").hide();
    $("#join-game-view").show();
});


connection.on("Countdown", function (message) {
    $("#countdown").html(message);
});

connection.on("Disconnect", function () {
    connection.stop();
    setTimeout(function () { window.location.reload() }, 30);
})

// Wait for jquery to be ready
document.getElementById("join-game-submit").addEventListener("click", function (event) {
    var code = $("#gamecode").val();
    var name = $("#name").val();
    $("#incorrect-game-code").hide();
    connection.invoke("RegisterPlayer", code, name).catch(errorReport);
    event.preventDefault();
});