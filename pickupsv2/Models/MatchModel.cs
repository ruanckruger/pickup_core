using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pickupsv2.Models
{
    public class Match
    {
        public Guid MatchId { get; set; }
        public Guid GameID { get; set; }
        public string Map { get; set; }
        public List<Player> Players { get; set; }
        public Guid Admin { get; set; }
        public bool NeedHost { get; set; }
        public Guid? Host { get; set; }
    }
    public class Game
    {
        public Guid GameId { get; set; }
        public string Name { get; set; }
        public List<string> Maps { get; set; }
        public string Rules { get; set; }
    }
    public class GameMatches
    {
        public Game Game { get; set; }
        public List<Match> Matches { get; set; }
    }
}