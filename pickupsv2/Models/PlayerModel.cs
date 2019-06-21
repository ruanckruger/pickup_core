using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pickupsv2.Models
{
    public class Player
    {
        [Key]
        public Guid Id { get; set; }
        public string steamId{ get; set; }
        public string steamUsername{ get; set; }
        public string name{ get; set; }
        public string steaumUrl{ get; set; }
        public string avatar{ get; set; }
        public Guid? curMatch{ get; set; }
    }
    public class SteamPlayer
    {
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