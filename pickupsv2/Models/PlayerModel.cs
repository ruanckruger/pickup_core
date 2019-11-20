using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pickupsv2.Models
{
    public class Player : IdentityUser
    {
        public byte[]  Avatar { get; set; }
        public Guid? CurMatch{ get; set; }
        [ForeignKey("steamid")]
        public SteamPlayer SteamPlayer{ get; set; }
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