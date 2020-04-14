using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using pickupsv2.Data;
using pickupsv2.Models;

namespace pickupsv2.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Player> _userManager;

        public UsersController(ApplicationDbContext context, UserManager<Player> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Players.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Players.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
       
        // GET: Users/Edit/5
        [Authorize(Roles = "UserAdmin")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            PlayerEdit player = new PlayerEdit();
            player.Player = await _context.Players.FindAsync(id);            
            if (player.Player == null)
            {
                return NotFound();
            }
            player.ManageUsers = await _userManager.IsInRoleAsync(player.Player, "UserAdmin");
            player.ManageGames = await _userManager.IsInRoleAsync(player.Player, "GameAdmin");
            player.ManageMatches = await _userManager.IsInRoleAsync(player.Player, "MatchAdmin");
            if (player.ManageGames)
            {
                player.Games = await _context.Games.ToListAsync();
            }
            return View(player);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, PlayerEdit editedUser)
        {
            if (id != editedUser.Player.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(editedUser.Player.Id);
                    if (editedUser.ManageUsers)
                        await _userManager.AddToRoleAsync(user, "UserAdmin");
                    else
                        await _userManager.RemoveFromRoleAsync(user, "UserAdmin");

                    if (editedUser.ManageGames)
                        await _userManager.AddToRoleAsync(user, "GameAdmin");
                    else
                        await _userManager.RemoveFromRoleAsync(user, "GameAdmin");

                    if (editedUser.ManageMatches)
                        await _userManager.AddToRoleAsync(user, "MatchAdmin");
                    else
                        await _userManager.RemoveFromRoleAsync(user, "MatchAdmin");

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerExists(editedUser.Player.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(editedUser);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Players
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var player = await _context.Players.FindAsync(id);
            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlayerExists(string id)
        {
            return _context.Players.Any(e => e.Id == id);
        }
    }
}
