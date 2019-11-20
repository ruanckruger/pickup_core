using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using pickupsv2.Data;
using pickupsv2.Models;

namespace pickupsv2.Controllers
{
    //[Authorize]
    public class SteamController : Controller
    {
        ApplicationDbContext context;
        UserManager<Player> umngr;
        SignInManager<Player> sgnIn;
        public SteamController(ApplicationDbContext _context, UserManager<Player> _umngr, SignInManager<Player> _sgn)
        {
            context = _context;
            umngr = _umngr;
            sgnIn = _sgn;
        }
        public ActionResult SaveSteamDetails(string steamids)
        {
            Console.WriteLine("[Steam]: " + steamids);
            var baseSteamUrl = String.Format("http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={0}&steamids={1}", "99219D4659300FAE38AC15F0071C72AF", steamids);
            //Response responseData;
	        using (var client = new HttpClient())
                using (var response = client.GetAsync(baseSteamUrl))
                    using (var content = response.Result.Content)
                {
                string result = content.ReadAsStringAsync().Result;
                var responseData = JsonConvert.DeserializeObject<RootObject>(result);

                SteamPlayer pData = responseData.response.players[0];
                var existing = context.Players.FirstOrDefault(p => p.SteamPlayer.steamid == steamids);
                if ( existing != null)
                {
                    existing.SteamPlayer = pData;
                    existing.UserName = pData.personaname;
                } else
                {
                    var curUser = umngr.GetUserId(User);
                    var player = new Player()
                    {
                        Id = curUser,
                        SteamPlayer = pData,
                        UserName = pData.personaname                        
                    };
                    context.Players.Add(player);
                }
                context.SaveChanges();
            }
            return LocalRedirect("/Home/Index");
        }
    }
}
