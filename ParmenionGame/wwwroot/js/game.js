"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();

function errorReport(err) {
    return console.error(err.toString());
}

connection.start().then(function () {
    $("#loading").hide();
    $("#join-game-frame").show();
}).catch(errorReport);

connection.on("ShowIncorrectGameCode", function (message) {    
    $("#incorrect-game-code").show();
});

// Wait for jquery to be ready
document.getElementById("join-game-submit").addEventListener("click", function (event) {
    var code = $("#gamecode").val();
    var name = $("#name").val();
    connection.invoke("RegisterPlayer", code, name).catch(errorReport);
    event.preventDefault();
});