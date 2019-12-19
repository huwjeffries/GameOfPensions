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

connection.on("ShowPlayerAcceptedGameCode", function (message) {
    $("#incorrect-game-code").hide();
    $("#join-game-view").hide();
    $("#join-game-success").show();
});

connection.on("ShowPlayerGameInProgress", function (message) {
    $("#waiting-next-game-view").show();
    $("#join-game-view").hide();
    $("#incorrect-game-code").hide();
    $("#countdown").hide();
});

connection.on("ShowPlayerNewGameReady", function (message) {
    $("#waiting-next-game-view").hide();
    $("#join-game-view").show();
    $("#countdown").show();
});

connection.on("ShowPlayerQuestionAnswers", function (answers) {
    $("#join-game-view").hide();
    $("#join-game-success").hide();
    $("#answers-view").html("");
    $("#answers-view").show();
    var view = $("#answers-view");

    answers.forEach((answer, i) => {
        var hyperLink = $('<a/>', {
            'class': 'link, answer',
            'href': '#',
            'click': function () { submitAnswer(i); }
        }).text(answer);

        view.append(hyperLink);
    });
});

function submitAnswer(index) {
    connection.invoke("PlayerAnswer", index).catch(errorReport);
}

connection.on("ShowPlayerAnswerAccepted", function (answerIndex) {
    $("#answers-view a").removeClass("confirmedAnswer");
    $("#answers-view a:nth-child(" + (answerIndex+1) + ")").addClass("confirmedAnswer");
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
    $("#countdown").html(countdownTime);
});

connection.on("Disconnect", function () {
    connection.stop();
    setTimeout(function () { window.location.reload(); }, 30);
});

// Wait for jquery to be ready
document.getElementById("join-game-submit").addEventListener("click", function (event) {
    var code = $("#gamecode").val();
    var name = $("#name").val().toUpperCase();
    $("#incorrect-game-code").hide();
    connection.invoke("RegisterPlayer", code, name).catch(errorReport);
    event.preventDefault();
});