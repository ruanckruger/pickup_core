"use strict";
// #region SignalR
$(document).ready(function () { 
    var game = new signalR.HubConnectionBuilder().withUrl("/PickupHub").build();

    //// #region Recieves
    //game.client.userJoined = function (matchId, userId, newCount) {
    //    $.get('/Matches/PlayerDisplay?playerId=' + userId + '&matchid=' + matchId, function (data, status) {
    //        $("#match-" + matchId).find(".match-player-count").text(newCount);
    //        $("#match-" + matchId).find(".curPlayers").append(data);
    //    });
    //};
    //game.client.userLeft = function (matchId, userId, newCount) {
    //    $("#" + userId).remove();
    //    $("#match-" + matchId).find(".match-player-count").text(newCount);
    //};
    //game.client.gameCreated = function (matchId) {
    //    $.get('/Matches/MatchInfo?matchid=' + matchId, function (data, status) {
    //        $("#matches-container").append(data);
    //    });
    //};
    //game.client.gameEnded = function (matchId) {
    //    $("#match-" + matchId).remove();
    //};
    //var accepted = false;
    //game.client.acceptGame = function (matchId) {
    //    console.log("Accept Match ", matchId);

    //    $.get("/Matches/AcceptMatch", function (data) {
    //        $("body").prepend(data);
    //        $("#acceptMatch").attr("match-id", matchId);
    //        $("#declineMatch").attr("match-id", matchId);
    //        $("#match-" + matchId).find(".join-match").remove();
    //        $("#match-" + matchId).find(".player").addClass("waiting-accept");
    //        gameReadyTimer = setTimeout(function () {
    //            if (accepted == false) {
    //                game.server.leave();
    //            }
    //            $(modalId).fadeOut();
    //        }, 20000)
    //    });

    //}

    //game.client.acceptStatus = function (uId, hasAccepted) {
    //    console.log(uId + " accepted or declined");
    //    if (hasAccepted == true) {
    //        $("#" + uId).addClass("accepted-match");
    //    } else {
    //        $("#" + uId).addClass("declined-match");
    //    }
    //    $("#" + uId).find(".waiting-accept").removeClass("waiting-accept");
    //}
    //game.client.adminFinalize = function (mId) {
    //    setTimeout(function () {
    //        if ($("#match-" + mId).find(".accepted-match").length == 10) {
    //            game.server.fullAccept(mId);
    //        }
    //    }, 22000)
    //}
    //game.client.teams = function (mId, teamA, teamB) {
    //    let players = $("#match-" + mId).find(".player-container");
    //    for (let i = 0; i < 5; i++) {
    //        console.log(players.find("#" + teamA[i]), players.find("#" + teamB[i]));
    //    }
    //}
    //// #endregion

    //// #region Sends
    game.start().then(function () {
    //    $('body').on("click", ".join-match", function () {
    //        $(".leave-match").addClass("join-match").removeClass("leave-match").text("Join Match");
    //        game.server.leave();
    //        game.server.join($(this).attr("match-id"));
    //        $(this).addClass("leave-match").removeClass("join-match").text("Leave Match");
    //    });

    //    $('body').on("click", ".leave-match", function () {
    //        console.log("Leave clicked");
    //        game.server.leave();
    //        $(this).addClass("join-match").removeClass("leave-match").text("Join Match");
    //    });

    //    $("#logout-button").click(function () {
    //        $("#" + $(this).attr("user-id")).remove();
    //        game.server.leave();
    //        $('#logoutForm').submit();
    //    });

    //    $("body").on("click", "#createMatchSubmit", function (e) {
    //        e.preventDefault();
    //        game.server.createGame("DankyDankness");
    //        $(modalId).fadeOut();
    //    });

    $("body").on("click", "#create-new-match", function (e) {
        e.preventDefault();
        console.log("clicked");
        game.invoke("CreateGame", "Dankaroo").catch(function (err){
            return console.error(err.toString());
        });
        //$(modalId).fadeOut();
    });
        //$("body").on("click", ".end-match", function (e) {
        //    e.preventDefault();
        //    game.server.endGame($(this).attr("match-id"));
        //});

        //$("body").on("click", "#acceptMatch", function (e) {
        //    e.preventDefault();
        //    $(modalId).fadeOut();
        //    game.server.acceptMatch();
        //    accepted = true;
        //});
        //$("body").on("click", "#declineMatch", function (e) {
        //    e.preventDefault();
        //    $(modalId).fadeOut();
        //    game.server.declineMatch();
        //});
    
    });
});
    //#endregion

// #endregion
