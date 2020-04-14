using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pickupsv2.Data;
using pickupsv2.Models;

namespace pickupsv2.Controllers
{
    public class MatchController : Controller
    {
        readonly ApplicationDbContext context;
        public MatchController(ApplicationDbContext _context)
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
                var match = db.Matches.Include(m => m.Map).Where(m => m.MatchId == matchId).FirstOrDefault();
                var curPlayers = new List<Player>();
                foreach (var player in db.Players.Where(n => n.CurMatch == match.MatchId))
                {
                    curPlayers.Add(player);
                }
                match.Players = curPlayers;
                return PartialView("_MatchPartial",match);
            }
        }
        public IActionResult PlayerInfo(string playerId, Guid matchId)
        {
            using (var db = context)
            {
                var match = db.Matches.FirstOrDefault(m => m.MatchId == matchId);
                var player = db.Players.FirstOrDefault(p => p.Id == playerId);                
                ViewData["matchAdmin"] = match.Admin;
                return PartialView("_PlayerPartial",player);
            }
        }
        public IActionResult MatchReady(Guid matchId)
        {
            using (var db = context)
            {
                var match = db.Matches.FirstOrDefault(m => m.MatchId == matchId);
                var players = db.Players.Where(p => p.CurMatch == matchId);
                match.Players = players.ToList();
                return PartialView(match);
            }
        }
        public IActionResult Match(Guid matchId)
        {
            using (var db = context)
            {
                var match = db.Matches.Include(m => m.Map).FirstOrDefault(m => m.MatchId == matchId);
                var players = db.Players.Where(p => p.CurMatch == matchId);
                match.Players = players.ToList();
                return View(match);
            }
        }
        public IActionResult _MapListPartial(Guid gameId)
        {
            List<Map> maps = context.Maps.Where(m => m.GameId == gameId).ToList();
            if (!maps.Any())
                return null;
            return PartialView(maps);
        }
    }
}