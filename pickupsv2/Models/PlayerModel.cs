using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace pickupsv2.Models
{
    public class Player: IdentityUser
    {
        public SimplePlayer Simplify()
        {
            return new SimplePlayer()
            {
                steamId = this.steamId,
                steamUsername = this.steamUsername,
                avatar = this.avatar,
                curMatch = this.curMatch,
                steaumUrl = this.steaumUrl,
                Id = Guid.Parse(this.Id)
                
            };
        }
        public string steamId{ get; set; }
        public string steamUsername{ get; set; }
        public string name{ get; set; }
        public string surname{ get; set; }
        public string steaumUrl{ get; set; }
        public string avatar{ get; set; }
        public Guid? curMatch{ get; set; }
    }
    public class SimplePlayer
    {
        public Guid Id { get; set; }
        public string steamId { get; set; }
        public string steamUsername { get; set; }
        public string steaumUrl { get; set; }
        public string avatar { get; set; }
        public Guid? curMatch { get; set; }
    }
}