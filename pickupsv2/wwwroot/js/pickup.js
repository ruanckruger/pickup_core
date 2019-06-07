"use strict";
// #region SignalR
$(document).ready(function () { 
    var game = new signalR.HubConnectionBuilder().withUrl("/PickupHub").build();

    //// #region Recieves
    game.on("UserJoined", function (matchId, userId, newCount) {
        $.get('/Match/PlayerInfo?playerId=' + userId + '&matchid=' + matchId, function (data, status) {
            $(".match-container[match-id='" + matchId + "']").find(".curPlayers").append(data);
        });
        $(".match-container[match-id='" + matchId + "']").find(".match-player-count").text(newCount);
        console.log(matchId + " joined " + userId);
    });
    game.on("UserLeft", function (matchId, userId, newCount) {
        $("#" + userId).remove();
        $(".match-container[match-id='" + matchId + "']").find(".match-player-count").text(newCount);
    });
    game.on("NewAdmin", function (matchId, userId) {
        $(".match-container[match-id='" + matchId + "']").find(".match-admin").removeClass("match-admin");
        $("#" + userId).addClass("match-admin");
    });
    game.on("GameCreated", function (matchId) {
        $.get('/Match/MatchInfo?matchId=' + matchId, function (data, status) {
            $("#matches-container").append(data);
        });
    });
    game.on("GameEnded", function (matchId) {
        $(".match-container[match-id='" + matchId + "']").remove();
    });
    var accepted = false;
    game.on("AcceptGame", function (matchId) {
        console.log("Accept Match ", matchId);

        $.get("/Matches/AcceptMatch", function (data) {
            $("body").prepend(data);
            $("#acceptMatch").attr("match-id", matchId);
            $("#declineMatch").attr("match-id", matchId);
            $("#match-" + matchId).find(".join-match").remove();
            $("#match-" + matchId).find(".player").addClass("waiting-accept");
            gameReadyTimer = setTimeout(function () {
                if (accepted == false) {
                    game.server.leave();
                }
                $(modalId).fadeOut();
            }, 20000)
        });

    });

    game.on("AcceptStatus", function (uId, hasAccepted) {
        console.log(uId + " accepted or declined");
        if (hasAccepted == true) {
            $("#" + uId).addClass("accepted-match");
        } else {
            $("#" + uId).addClass("declined-match");
        }
        $("#" + uId).find(".waiting-accept").removeClass("waiting-accept");
    });
    game.on("AdminFinalize", function (mId) {
        setTimeout(function () {
            if ($("#match-" + mId).find(".accepted-match").length == 10) {
                game.server.fullAccept(mId);
            }
        }, 22000)
    });
    game.on("Teams", function (mId, teamA, teamB) {
        let players = $("#match-" + mId).find(".player-container");
        for (let i = 0; i < 5; i++) {
            console.log(players.find("#" + teamA[i]), players.find("#" + teamB[i]));
        }
    });
    //// #endregion

    //// #region Sends
    game.start().then(function () {
        $('body').on("click", ".join-match", function () {            
            $(".leave-match").addClass("join-match").removeClass("leave-match").text("Join Match");
            game.invoke("Join", $(this).closest(".match-container").attr("match-id"));
            $(this).addClass("leave-match").removeClass("join-match").text("Leave Match");
        });

        $('body').on("click", ".leave-match", function () {
            game.invoke("Leave");
            $(this).addClass("join-match").removeClass("leave-match").text("join match");
        });

        $("#logout-button").click(function () {
            game.invoke("Leave");
            $('#logoutForm').submit();
        });
        
        $("body").on("click", "#create-new-match", function (e) {
            e.preventDefault();
            game.invoke("CreateGame", $("#map-list").val()).catch(function (err){
                return console.error(err.toString());
            });
            //$(modalId).fadeOut();
        });
        $("body").on("click", ".end-match", function (e) {
            e.preventDefault();
            console.log("clicked", $(this).closest(".match-container").attr("match-id"));
            game.invoke("EndGame", $(this).closest(".match-container").attr("match-id"));
        });
        $("body").on("click", ".adminfy", function (e) {
            e.preventDefault();
            game.invoke("Adminfy", $(this).closest(".match-container").attr("match-id"), $(this).attr("player-id"));
        });
        $("body").on("click", ".kick", function (e) {
            e.preventDefault();
            game.invoke("Kick", $(this).closest(".match-container").attr("match-id"), $(this).attr("player-id"));
        });
        $("body").on("click", "#acceptMatch", function (e) {
            e.preventDefault();
            $(modalId).fadeOut();
            game.invoke("AcceptMatch");
            accepted = true;
        });
        $("body").on("click", "#declineMatch", function (e) {
            e.preventDefault();
            $(modalId).fadeOut();
            game.invoke("DeclineMatch");
        });
    
    });
});
    //#endregion

// #endregion
