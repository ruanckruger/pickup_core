using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using pickupsv2.Models;
using Microsoft.AspNetCore.Mvc;
using pickupsv2.Data;

namespace pickupsv2.Controllers
{
    public class HomeController : Controller
    {
        readonly PickupContext _context;
        public HomeController(PickupContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var matches = new List<Match>();
            using (var db = _context)
            {
                foreach(var match in db.Matches)
                {
                    match.Players = new List<SimplePlayer>();
                    foreach(var player in db.Players)
                    {
                        Random rnd = new Random();
                        if (player.curMatch == match.id)
                        {
                            int team = rnd.Next(1, 100);
                            if (match.Players.Count < 10) {
                                match.Players.Add(player.Simplify());
                            }
                        }
                    }
                    matches.Add(match);
                }
            }
            return View(matches);
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
