using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using pickupsv2.Data;
using pickupsv2.Models;

namespace pickupsv2.Controllers
{
    public class MatchController : Controller
    {
        PickupContext context;
        public MatchController(PickupContext _context)
        {
            context = _context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult MatchInfo(Match match)
        {
            using (var db = context)
            {
                //var match = db.Matches.FirstOrDefault(m => m.id == matchId);

                var curPlayers = new List<Player>();
                foreach (var player in db.Players.Where(n => n.curMatch == match.id))
                {
                    curPlayers.Add(player);
                }

                match.Players = curPlayers;
                return PartialView(match);
            }
        }
        public IActionResult PlayerInfo(Guid playerId, Guid matchId)
        {
            using (var db = context)
            {
                var match = db.Matches.FirstOrDefault(m => m.id == matchId);

                var player = db.Players.FirstOrDefault(p => p.Id == playerId);
                var players = new List<Player>();
                players.Add(player);
                var matches = new Match()
                {
                    id = matchId,
                    Admin = match.Admin,
                    Players = players
                };
                return PartialView(matches);
            }
        }
    }
}