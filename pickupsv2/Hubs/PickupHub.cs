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
        readonly ApplicationDbContext context;
        UserManager<Player> uManager;
        public PickupHub(ApplicationDbContext _context, UserManager<Player> umngr) {
            context = _context;
            uManager = umngr;
        }

        //public async override Task OnDisconnectedAsync(Exception exception)
        //{
        //    await Leave();
        //    await base.OnDisconnectedAsync(exception);
        //}
        [Authorize]
        public async Task Reconnect()
        {
            using (var db = context)
            {
                var curUserId = uManager.GetUserId(Context.User);
                var player = db.Players.Where(p => p.Id == curUserId).FirstOrDefault();
                if(player.CurMatch != null)
                    await Groups.AddToGroupAsync(Context.ConnectionId, player.CurMatch.ToString());
            }
        }


        public async Task Join(Guid matchId)
        {
            using (var db = context)
            {
                var curUserId = uManager.GetUserId(Context.User);
                var player = db.Players.Where(p => p.Id == curUserId).FirstOrDefault();
                if (player.CurMatch != null)
                    await Leave();

                await Groups.AddToGroupAsync(Context.ConnectionId, matchId.ToString());
                player.CurMatch = matchId;
                await db.SaveChangesAsync();
                var newPlayerCount = db.Players.Where(p => p.CurMatch == matchId).Count();
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
            var curUserId = uManager.GetUserId(Context.User);
            Player player = db.Players.FirstOrDefault(p => p.Id == curUserId);
            var playerCurMatch = player.CurMatch;
            player.CurMatch = null;
            await db.SaveChangesAsync();

            var newPlayerCount = db.Players.Where(p => p.CurMatch == playerCurMatch).Count();
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

                await Clients.All.SendAsync("GameCreated",match.MatchId);
            }
        }
        public async Task EndGame(Guid matchId)
        {
            using (var db = context)
            {
                Match match = new Match
                {
                    MatchId = matchId
                };
                List<Player> removePlayers = db.Players.Where(p => p.CurMatch == matchId).ToList();
                foreach (var remPlayer in removePlayers)
                {
                    remPlayer.CurMatch = null;
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
                var matchAdmin = db.Matches.FirstOrDefault(m => m.MatchId == matchId).Admin;
                if (Guid.Parse(uManager.GetUserId(Context.User)) == matchAdmin)
                    await Clients.Client(Context.ConnectionId).SendAsync("AdminFinalize",matchId);
            }
        }
        public async Task Adminfy(Guid matchId, Guid userId)
        {
            using (var db = context)
            {
                var match = db.Matches.FirstOrDefault(m => m.MatchId == matchId);
                match.Admin = userId;
                await db.SaveChangesAsync();
                await Clients.All.SendAsync("NewAdmin", matchId);
            }
        }
        public async Task Kick(Guid matchId, string userId)
        {
            var db = context;
            Player player = db.Players.FirstOrDefault(p => p.Id == userId);
            var playerCurMatch = player.CurMatch;
            player.CurMatch = null;
            await db.SaveChangesAsync();

            var newPlayerCount = db.Players.Where(p => p.CurMatch == playerCurMatch).Count();
            await Groups.RemoveFromGroupAsync(Context.ConnectionId,matchId.ToString());

            await Clients.All.SendAsync("UserLeft", playerCurMatch, player.Id, newPlayerCount);
        }
        public async Task AcceptMatch(bool accepted)
        {
            var curUserId = Guid.Parse(uManager.GetUserId(Context.User));
            await Clients.All.SendAsync("AcceptStatus", curUserId, accepted);
        }
        public async Task FullAccept(Guid matchId)
        {             
             await Clients.All.SendAsync("Teams",matchId);
        }

        // #region chat
        public async Task SendGlobalMessage(string msg)
        {
            using (var db = context)
            {
                var curUserId = uManager.GetUserId(Context.User);
                var player = db.Players.FirstOrDefault(p => p.Id == curUserId);

                await Clients.All.SendAsync("RecieveGlobalMessage", player.UserName, msg);
            }
        }

        public async Task SendMatchMessage(string msg)
        {
            using (var db = context)
            {
                var curUserId = uManager.GetUserId(Context.User);
                var player = db.Players.FirstOrDefault(p => p.Id == curUserId);
                if(player.CurMatch != null)
                    await Clients.Group(player.CurMatch.ToString()).SendAsync("RecieveMatchMessage", player.UserName, msg);
            }
        }
        // #endregion
    }
}

