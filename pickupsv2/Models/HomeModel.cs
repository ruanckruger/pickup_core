using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pickupsv2.Models
{
    public class Home
    {
        public List<Game> Games { get; set; }
        public List<Match> Matches { get; set; }
    }
}