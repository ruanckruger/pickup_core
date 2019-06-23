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

        public IActionResult MatchInfo(Guid matchId)
        {
            using (var db = context)
            {
                var match = db.Matches.FirstOrDefault(m => m.id == matchId);
                var curPlayers = new List<Player>();
                foreach (var player in db.Players.Where(n => n.curMatch == match.id))
                {
                    curPlayers.Add(player);
                }
                match.Players = curPlayers;
                return PartialView("_MatchPartial",match);
            }
        }
        public IActionResult PlayerInfo(Guid playerId, Guid matchId)
        {
            using (var db = context)
            {
                var match = db.Matches.FirstOrDefault(m => m.id == matchId);
                var player = db.Players.FirstOrDefault(p => p.Id == playerId);                
                ViewData["matchAdmin"] = match.Admin;
                return PartialView("_PlayerPartial",player);
            }
        }
        public IActionResult MatchReady(Guid matchId)
        {
            using (var db = context)
            {
                var match = db.Matches.FirstOrDefault(m => m.id == matchId);
                var players = db.Players.Where(p => p.curMatch == matchId);
                match.Players = players.ToList();
                return PartialView(match);
            }
        }
    }
}