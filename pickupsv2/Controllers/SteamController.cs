using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using pickupsv2.Data;
using pickupsv2.Models;

namespace pickupsv2.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
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
        [ProducesResponseType(typeof(string),(int)HttpStatusCode.OK)]
        async public Task<string> SaveSteamDetails([FromQuery]string steamids, [FromQuery]string key)
        {
            var baseSteamUrl = String.Format("http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key{0}=&steamids={1}", key, steamids);
            Response responseData;
            using (var client = new HttpClient())
                using (var response = await client.GetAsync(baseSteamUrl))
                    using (var content = response.Content)
            {
                string result = await content.ReadAsStringAsync();
                responseData = JsonConvert.DeserializeObject<Response>(result);

                SteamPlayer pData = responseData.player;
                var player = new Player()
                {
                    Id = Guid.Parse(umngr.GetUserId(User)),
                    avatar = pData.avatar,
                    steamId = pData.steamid,
                    steaumUrl = pData.profileurl,
                    steamUsername = pData.personaname,
                    name = pData.realname
                };
                context.Players.Add(player);
                await context.SaveChangesAsync();
            }
            return "dank";
        }
    }
}