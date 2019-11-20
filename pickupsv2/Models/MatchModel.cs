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
        public Guid GameID { get; set; }
        public string Map { get; set; }
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
        public List<Map> Maps { get; set; }
    }

    public class Map
    {
        [Key]
        public Guid MapId { get; set; }
        [ForeignKey("GameId")]
        public Guid GameId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
    }

    public class MapExtraModel{
        public List<Game> Games { get; set; }
        public string MapName { get; set; }
        public string Image { get; set; }
    }

    public class GameMatches
    {
        public Game Game { get; set; }
        public List<Match> Matches { get; set; }
    }
}