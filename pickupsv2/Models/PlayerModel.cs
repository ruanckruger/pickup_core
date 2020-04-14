using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pickupsv2.Models
{
    public class Player : IdentityUser
    {
        public byte[] Avatar { get; set; }
        public string DisplayName { get; set; }
        public Guid? CurMatch { get; set; }

        [ForeignKey("GameId")]
        public List<GameAdmin> AdminFor { get; set; }
        [ForeignKey("steamid")]
        public SteamPlayer SteamPlayer { get; set; }

    }

    [NotMapped]
    public class PlayerEdit
    {
        public Player Player { get; set; }
        public List<Game> Games { get; set; }
        public bool ManageUsers { get; set; }
        public bool ManageGames { get; set; }
        public bool ManageMatches { get; set; }
    }

    public class GameAdmin
    {
        [Key]
        public Guid GameAdminId { get; set; }

        public Game Game { get; set; }
        public Player Player { get; set; }
    }
    public class SteamPlayer
    {
        [Key]
        public string steamid { get; set; }
        public int communityvisibilitystate { get; set; }
        public int profilestate { get; set; }
        public string personaname { get; set; }
        public int lastlogoff { get; set; }
        public string profileurl { get; set; }
        public string avatar { get; set; }
        public string avatarmedium { get; set; }
        public string avatarfull { get; set; }
        public int personastate { get; set; }
        public string realname { get; set; }
        public string primaryclanid { get; set; }
        public int timecreated { get; set; }
        public int personastateflags { get; set; }
        public string loccountrycode { get; set; }
        public string locstatecode { get; set; }
        public int loccityid { get; set; }
    }

    public class Response
    {
        public List<SteamPlayer> players { get; set; }
    }

    public class RootObject
    {
        public Response response { get; set; }
    }
}