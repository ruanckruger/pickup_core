using pickupsv2.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using pickupsv2.Data;
using Microsoft.AspNetCore.Identity;

namespace pickupsv2.Hubs
{
    //[Authorize]
    public class PickupHub : Hub
    {
        readonly PickupContext context;
        UserManager<IdentityUser> uManager;
        public PickupHub(PickupContext _context, UserManager<IdentityUser> umngr) {
            context = _context;
            uManager = umngr;
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            await Leave();
            await base.OnDisconnectedAsync(exception);
        }

        public async Task Join(Guid matchId)
        {
            using (var db = context)
            {
                var curUserId = Guid.Parse(uManager.GetUserId(Context.User));
                var players = db.Players.Select(p => p);
                var player = db.Players.Where(p => p.Id == curUserId).FirstOrDefault();
                if (player.curMatch != null)
                    await Leave();

                await Groups.AddToGroupAsync(Context.ConnectionId, matchId.ToString());
                player.curMatch = matchId;
                await db.SaveChangesAsync();

                var newPlayerCount = db.Players.Where(p => p.curMatch == matchId).Count();
                await Clients.All.SendAsync("UserJoined", matchId, player.Id, newPlayerCount);
                if (newPlayerCount == 2)
                {
                    await GameReady(matchId);
                }
            }
        }
        public async Task Leave()
        {
            var db = context;
            var curUserId = Guid.Parse(uManager.GetUserId(Context.User));
            Player player = db.Players.FirstOrDefault(p => p.Id == curUserId);
            var playerCurMatch = player.curMatch;
            player.curMatch = null;
            await db.SaveChangesAsync();

            var newPlayerCount = db.Players.Where(p => p.curMatch == playerCurMatch).Count();
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, playerCurMatch.ToString());

            await Clients.All.SendAsync("UserLeft",playerCurMatch, player.Id, newPlayerCount);
            
        }
        public async Task CreateGame(string Map)
        {
            using (var db = context)
            {
                Match match = new Match();

                match.Map = Map;
                match.Admin = Guid.Parse(uManager.GetUserId(Context.User));
                
                db.Matches.Add(match);
                await db.SaveChangesAsync();

                await Clients.All.SendAsync("GameCreated",match.id);
            }
        }
        public async Task EndGame(Guid matchId)
        {
            using (var db = context)
            {
                Match match = new Match
                {
                    id = matchId
                };
                List<Player> removePlayers = db.Players.Where(p => p.curMatch == matchId).ToList();
                foreach (var remPlayer in removePlayers)
                {
                    remPlayer.curMatch = null;
                }
                
                db.Matches.Attach(match);
                db.Matches.Remove(match);
                await db.SaveChangesAsync();
                await Clients.All.SendAsync("GameEnded",matchId);
            }
        }
        public async Task GameReady(Guid matchId)
        {
            await Clients.Group(matchId.ToString()).SendAsync("acceptGame",matchId);
            using (var db = context)
            {
                var matchAdmin = db.Matches.FirstOrDefault(m => m.id == matchId).Admin;
                if (Guid.Parse(uManager.GetUserId(Context.User)) == matchAdmin)
                    await Clients.Client(Context.ConnectionId).SendAsync("adminFinalize",matchId);
            }
        }
        public async Task AcceptMatch()
        {
            var curUserId = uManager.GetUserId(Context.User);
            await Clients.All.SendAsync("acceptStatus",curUserId, true);
        }
        public async Task DeclineMatch()
        {
            var curUserId = uManager.GetUserId(Context.User);
            await Clients.All.SendAsync("acceptStatus",curUserId, false);
        }
        public async Task FullAccept(Guid matchId)
        {
            string[] teamA = new string[5];
            string[] teamB = new string[5];
            using (var db = context)
            {
                var players = db.Players.Where(p => p.curMatch == matchId).ToList();
                Random rand = new Random();
                for (int i = 0; i < 5; i++)
                {
                    int randomSpot = rand.Next(players.Count);
                    teamA[i] = players.ElementAt(randomSpot).Id.ToString();
                    players.RemoveAt(randomSpot);

                    randomSpot = rand.Next(players.Count);
                    teamB[i] = players.ElementAt(randomSpot).Id.ToString();
                    players.RemoveAt(randomSpot);
                }
                await Clients.All.SendAsync("Teams",matchId, teamA, teamB);
            }

        }
    }
}

