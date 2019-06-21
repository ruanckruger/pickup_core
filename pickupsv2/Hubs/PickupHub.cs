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

        //public async override Task OnDisconnectedAsync(Exception exception)
        //{
        //    await Leave();
        //    await base.OnDisconnectedAsync(exception);
        //}

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
                if (newPlayerCount == 10)
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
        [Authorize(Roles = "Admin")]
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
        //[Authorize(Roles = "Admin")]
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
                    await db.SaveChangesAsync();
                }
                
                db.Matches.Attach(match);
                db.Matches.Remove(match);
                await db.SaveChangesAsync();
                await Clients.All.SendAsync("GameEnded",matchId);
            }
        }
        public async Task GameReady(Guid matchId)
        {
            await Clients.Group(matchId.ToString()).SendAsync("AcceptGame",matchId);
            using (var db = context)
            {
                var matchAdmin = db.Matches.FirstOrDefault(m => m.id == matchId).Admin;
                if (Guid.Parse(uManager.GetUserId(Context.User)) == matchAdmin)
                    await Clients.Client(Context.ConnectionId).SendAsync("AdminFinalize",matchId);
            }
        }
        public async Task Adminfy(Guid matchId, Guid userId)
        {
            using (var db = context)
            {
                var match = db.Matches.FirstOrDefault(m => m.id == matchId);
                match.Admin = userId;
                await db.SaveChangesAsync();
                await Clients.All.SendAsync("NewAdmin", matchId, userId);
            }
        }
        public async Task Kick(Guid matchId, Guid userId)
        {
            var db = context;
            Player player = db.Players.FirstOrDefault(p => p.Id == userId);
            var playerCurMatch = player.curMatch;
            player.curMatch = null;
            await db.SaveChangesAsync();

            var newPlayerCount = db.Players.Where(p => p.curMatch == playerCurMatch).Count();
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, playerCurMatch.ToString());

            await Clients.All.SendAsync("UserLeft", playerCurMatch, player.Id, newPlayerCount);

        }
        public async Task AcceptMatch(Guid userId, bool accepted)
        {
            await Clients.All.SendAsync("AcceptStatus",userId, accepted);
        }
        public async Task FullAccept(Guid matchId)
        {
            Guid[] teamA = new Guid[5];
            Guid[] teamB = new Guid[5];
            using (var db = context)
            {
                var players = db.Players.Where(p => p.curMatch == matchId).ToList();
                Random rand = new Random();
                for (int i = 0; i < 5; i++)
                {
                    int randomSpot = rand.Next(players.Count);
                    teamA[i] = players.ElementAt(randomSpot).Id;
                    players.RemoveAt(randomSpot);

                    randomSpot = rand.Next(players.Count);
                    teamB[i] = players.ElementAt(randomSpot).Id;
                    players.RemoveAt(randomSpot);
                }
                await Clients.All.SendAsync("Teams",matchId, teamA, teamB);
            }

        }

        public async Task SendGlobalMessage(string msg)
        {
            using (var db = context)
            {
                var curUserId = Guid.Parse(uManager.GetUserId(Context.User));
                var player = db.Players.FirstOrDefault(p => p.Id == curUserId);

                await Clients.All.SendAsync("RecieveGlobalMessage", player.steamUsername,msg);
            }
        }

        public async Task SendMatchMessage(string msg)
        {
            using (var db = context)
            {
                var curUserId = Guid.Parse(uManager.GetUserId(Context.User));
                var player = db.Players.FirstOrDefault(p => p.Id == curUserId);
                if(player.curMatch != null)
                    await Clients.Group(player.curMatch.ToString()).SendAsync("RecieveMatchMessage", player.steamUsername, msg);
            }
        }
    }
}

