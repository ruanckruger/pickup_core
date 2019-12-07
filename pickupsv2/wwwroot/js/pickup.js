"use strict";
// #region SignalR
$(document).ready(function () { 
    var game = new signalR.HubConnectionBuilder().withUrl("/PickupHub").build();
    var modalId = "#modal-overlay";


    // Chat Code
    var chatId = "#chat-container";
    $("#chat-toggle").click(function () {
        if ($(chatId).hasClass("open"))
            $(chatId).removeClass("open");
        else {
            $(chatId).addClass("open");
            $("#chat-toggle").addClass("new-message");
        }        
    });
    var globalMessages = "";
    var matchMessages = "";
    game.on("RecieveGlobalMessage", function (userName, message) {
        var formattedMessage = `<p class="chat-message"><b>[` + userName + `]: </b> 
                            `+message+`
                        </p>`;
        globalMessages += formattedMessage;
        if ($("#global-chat").hasClass("current-chat-tab"))
            $("#chat-window").html(globalMessages);
        $("#chat-toggle").addClass("new-message");
    });
    game.on("RecieveMatchMessage", function (userName, message) {
        var formattedMessage = `<p class="chat-message"><b>[` + userName + `]: </b> 
                            `+ message + `
                        </p>`;
        matchMessages += formattedMessage;
        if ($("#match-chat").hasClass("current-chat-tab"))
            $("#chat-window").html(matchMessages);
        $("#chat-toggle").addClass("new-message");
    });
    $("#global-chat").click(function () {
        $("#match-chat").removeClass("current-chat-tab");
        $(this).addClass("current-chat-tab");
        $("#chat-window").html(globalMessages);
    });
    $("#match-chat").click(function () {
        $("#global-chat").removeClass("current-chat-tab");
        $(this).addClass("current-chat-tab");
        $("#chat-window").html(matchMessages);
    });

    // #region Recieves
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
    game.on("NewAdmin", function (matchId) {
        $.get("/Match/MatchInfo?matchId=" + matchId, function (data) {
            $(".match-container[match-id='" + matchId + "']").replaceWith(data);
        });
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
    var gameReadyTimer;
    game.on("AcceptGame", function (matchId) {
        accepted = false;
        console.log("Accept Match ", matchId);
        $.get("/Match/MatchReady?matchId=" + matchId, function (data) {
            $("body").prepend(data);
            $("#accept-match").attr("match-id", matchId);
            $("#decline-match").attr("match-id", matchId);
            $(".match-container[match-id='" + matchId + "']").find(".player-container").addClass("waiting-accept");

            $(modalId).fadeIn();
            var readyClip = new Audio('../audio/ready.mp3');
            readyClip.play();
            gameReadyTimer = setTimeout(function () {
                if (accepted === false) {
                    game.invoke("Leave");
                }
                $(modalId).fadeOut();
                $(modalId).remove();
            }, 20000)
        });
    });

    game.on("AcceptStatus", function (uId, hasAccepted) {
        console.log(uId + " accepted? " + hasAccepted);

        if (hasAccepted) {
            $("#" + uId).addClass("accepted-match");
        } else {
            $("#" + uId).addClass("declined-match");
        }
        $("#" + uId).removeClass("waiting-accept");
    });
    game.on("AdminFinalize", function (mId) {
        console.log("Yo, admin guy, you got dis");
        setTimeout(function () {$(".match-container[match-id='" + mId + "']")
            if ($(".match-container[match-id='" + mId + "']").find(".accepted-match").length === 10) {
                game.invoke("FullAccept", mId);
            } else {
                $.each($(".match-container[match-id='" + mId + "']").find(".declined-match"), function () {
                    game.invoke("Kick", mId, $(this).attr("id"));
                });
                $.each($(".match-container[match-id='" + mId + "']").find(".waiting-accept"), function () {
                    game.invoke("Kick", mId, $(this).attr("id"));
                });
            }
        }, 22000)
    });
    game.on("Teams", function (mId) {
        $.get("/Match/MatchReady?matchId=" + matchId, function (data) {
            $(".match-container[match-id='" + mId + "']").html(data);
        });
    });
    // #endregion

    // #region Sends
    game.start().then(function () {

        // Reconnecting
        game.invoke("Reconnect");

        $('body').on("click", "#create-new-match", function () {
            game.invoke("CreateGame", $("#map-list").find("option:selected").attr('value'), $("#game-list").find("option:selected").attr('value'));
        });

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
        $("body").on("click", "#accept-match", function (e) {
            e.preventDefault();
            $(modalId).fadeOut();
            $(modalId).remove();
            game.invoke("AcceptMatch", true);
            accepted = true;
        });
        $("body").on("click", "#decline-match", function (e) {
            e.preventDefault();
            $(modalId).fadeOut();
            $(modalId).remove();
            game.invoke("AcceptMatch", false);
        });

        // Chat Code
        $('#send-message').click(function () {
            var message = $("#chat-message").val();
            console.log("Sending message", );
            if ($("#global-chat").hasClass("current-chat-tab"))
                game.invoke("SendGlobalMessage", message);
            else if ($("#match-chat").hasClass("current-chat-tab"))
                game.invoke("SendMatchMessage", message);
        });
    });
});
    //#endregion

// #endregion
