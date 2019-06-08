"use strict";
// #region SignalR
$(document).ready(function () { 
    var game = new signalR.HubConnectionBuilder().withUrl("/ChatHub").build();
    var modalId = "#modal-overlay";
    //// #region Recieves
    game.on("UserJoined", function (matchId, userId, newCount) {
        $.get('/Match/PlayerInfo?playerId=' + userId + '&matchid=' + matchId, function (data, status) {
            $(".match-container[match-id='" + matchId + "']").find(".curPlayers").append(data);
        });
        $(".match-container[match-id='" + matchId + "']").find(".match-player-count").text(newCount);
        console.log(matchId + " joined " + userId);
    });
    //// #endregion

    //// #region Sends
    game.start().then(function () {
        $('body').on("click", ".join-match", function () {            
            $(".leave-match").addClass("join-match").removeClass("leave-match").text("Join Match");
            game.invoke("Join", $(this).closest(".match-container").attr("match-id"));
            $(this).addClass("leave-match").removeClass("join-match").text("Leave Match");
        });

        
    });
});
    //#endregion

// #endregion
