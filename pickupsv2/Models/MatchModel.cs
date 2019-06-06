using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pickupsv2.Models
{
    public class Match
    {
        public Guid id { get; set; }
        public string Map { get; set; }
        public List<Player> Players { get; set; }
        public Guid Admin { get; set; }
    }
}