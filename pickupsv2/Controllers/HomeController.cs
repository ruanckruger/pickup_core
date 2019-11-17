using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using pickupsv2.Models;
using Microsoft.AspNetCore.Mvc;
using pickupsv2.Data;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;

namespace pickupsv2.Controllers
{
    public class HomeController : Controller
    {
        readonly PickupContext _context;
        readonly UserManager<IdentityUser> umngr;
        public HomeController(PickupContext context, UserManager<IdentityUser> _umngr)
        {
            _context = context;
            umngr = _umngr;
        }
        public IActionResult Index()
        {
            var home = new Home()
            {
                Matches = new List<Match>(),
                Games = new List<Game>()
            };
            using (var db = _context)
            {
                foreach(var game in db.Games)
                {
                    home.Games.Add(game);
                }
                foreach(var match in db.Matches)
                {
                    match.Players = new List<Player>();
                    foreach(var player in db.Players)
                    {
                        Random rnd = new Random();
                        if (player.curMatch == match.MatchId)
                        {
                            int team = rnd.Next(1, 100);
                            if (match.Players.Count < 10) {
                                match.Players.Add(player);
                            }
                        }
                    }
                    home.Matches.Add(match);
                }

            }
            return View(home);
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
