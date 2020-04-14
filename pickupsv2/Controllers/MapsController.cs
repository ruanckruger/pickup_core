using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using pickupsv2.Data;
using pickupsv2.Helpers;
using pickupsv2.Models;

namespace pickupsv2.Controllers
{
    public class MapsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MapsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Maps
        public async Task<IActionResult> Index()
        {

            var games = await _context.Games.ToListAsync();
            ViewBag.games = games;
            return View(await _context.Maps.ToListAsync());
        }

        // GET: Maps/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var map = await _context.Maps.FirstOrDefaultAsync(m => m.MapId == id);
            if (map == null)
            {
                return NotFound();
            }

            return View(map);
        }

        // GET: Maps/Create/GameId
        public async Task<IActionResult> Create(Guid? gameId)
        {
            if (gameId == null)
            {
                return NotFound();
            }
            var game = await _context.Games.FirstOrDefaultAsync(g => g.GameId == gameId);
            if(game == null)
            {
                return NotFound();
            }
            var map = new Map()
            {
                GameId = gameId.Value
            };
            MapCreate mapCreate = new MapCreate()
            {
                Map = map
            };
            return View(mapCreate);
        }

        // POST: Maps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MapCreate mapCreate)
        {   
            if (ModelState.IsValid)
            {
                mapCreate.Map.MapId = Guid.NewGuid();
                if (mapCreate.Image.Length > 0)
                {
                    mapCreate.Map.ImageExtension = await WriteHelper.UploadImage(mapCreate.Image, "maps", mapCreate.Map.MapId.ToString());
                    var newMap = _context.Add(mapCreate.Map);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(mapCreate);
        }

        // GET: Maps/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var mapCreate = new MapCreate();
            mapCreate.Map = await _context.Maps.FindAsync(id);
            if (mapCreate.Map == null)
            {
                return NotFound();
            }
            return View(mapCreate);
        }

        // POST: Maps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MapCreate mapCreate)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (mapCreate.Image.Length > 0)
                    {
                        mapCreate.Map.ImageExtension = await WriteHelper.UploadImage(mapCreate.Image, "maps", mapCreate.Map.MapId.ToString());
                        var newMap = _context.Update(mapCreate.Map);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MapExists(mapCreate.Map.MapId))
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
            return View(mapCreate);
        }

        // GET: Maps/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var map = await _context.Maps
                .FirstOrDefaultAsync(m => m.MapId == id);
            if (map == null)
            {
                return NotFound();
            }

            return View(map);
        }

        // POST: Maps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var map = await _context.Maps.FindAsync(id);
            _context.Maps.Remove(map);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MapExists(Guid id)
        {
            return _context.Maps.Any(e => e.MapId == id);
        }
    }
}
