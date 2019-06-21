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
    [Route("api/[controller]/[action]")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ApiController]
    public class SteamController : ControllerBase
    {
        PickupContext context;
        UserManager<IdentityUser> umngr;
        public SteamController(PickupContext _context, UserManager<IdentityUser> _umngr)
        {
            context = _context;
            umngr = _umngr;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<bool> SaveSteamDetails(string key, string steamids)
        {
            Console.WriteLine("[Steam]: " + steamids);
            var baseSteamUrl = String.Format("http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={0}&steamids={1}", key, steamids);
            //Response responseData;
	        using (var client = new HttpClient())
                using (var response = client.GetAsync(baseSteamUrl))
                    using (var content = response.Result.Content)
                {
                string result = content.ReadAsStringAsync().Result;
                var responseData = JsonConvert.DeserializeObject<RootObject>(result);

                SteamPlayer pData = responseData.response.players[0];
                var existing = context.Players.FirstOrDefault(p => p.steamId == steamids);
                if ( existing != null)
                {
                    existing.steamUsername = pData.personaname;
                    existing.avatar = pData.avatarfull;
                } else
                {
                    var player = new Player()
                    {
                        Id = Guid.Parse(umngr.GetUserId(User)),
                        avatar = pData.avatarfull,
                        steamId = pData.steamid,
                        steaumUrl = pData.profileurl,
                        steamUsername = pData.personaname,
                        name = pData.realname
                        
                    };
                    context.Players.Add(player);
                }
                context.SaveChanges();
            }
            return true;
        }
    }
}
