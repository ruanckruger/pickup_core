using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pickupsv2.Models
{
    public class Match
    {
        [Key]
        public Guid MatchId { get; set; }
        public Game Game { get; set; }
        [ForeignKey("MapId")]
        public Map Map { get; set; }
        [ForeignKey("CurMatch")]
        public List<Player> Players { get; set; }
        public Guid Admin { get; set; }
        public bool NeedHost { get; set; }
        public Guid? Host { get; set; }
    }

    public class Game
    {
        [Key]
        public Guid GameId { get; set; }
        public string Name { get; set; }
        public string Rules { get; set; }
        public string ImageExtension { get; set; }
        public List<Map> Maps { get; set; }
        [ForeignKey("Id")]
        public List<GameAdmin> Admins { get; set; }
    }

    [NotMapped]
    public class GameCreate
    {
        public Game Game { get; set; }
        public IFormFile Image { get; set; }
    }

    public class Map
    {
        [Key]
        public Guid MapId { get; set; }
        [ForeignKey("GameId")]
        public Guid GameId { get; set; }
        public string Name { get; set; }
        public string ImageExtension { get; set; }
    }

    [NotMapped]
    public class MapCreate
    {
        public Map Map { get; set; }
        public IFormFile Image { get; set; }
    }

    public class GameMatches
    {
        public Game Game { get; set; }
        public List<Match> Matches { get; set; }
    }
}