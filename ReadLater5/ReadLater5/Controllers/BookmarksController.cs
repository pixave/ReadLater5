using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data;
using Entity;
using System.Security.Claims;

namespace ReadLater5.Controllers
{
    public class BookmarksController : Controller
    {
        private readonly ReadLaterDataContext _context;
        string _UserID;

        public BookmarksController(ReadLaterDataContext context)
        {
            _context = context;

            if (this.User != null)
            {
                ClaimsPrincipal currentUser = this.User;
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (currentUserID != null)
                {
                    _UserID = currentUserID;
                    ViewBag.UserID = _UserID;
                }
            }
        }

        // GET: Bookmarks
        public async Task<IActionResult> Index()
        {
            if (this != null)
            {
                if (this.User != null)
                {
                    ClaimsPrincipal currentUser = this.User;
                    var currentUserData = currentUser.FindFirst(ClaimTypes.NameIdentifier);
                    if (currentUserData != null)
                    {
                        var currentUserID = currentUserData.Value;
                        if (currentUserID != null)
                        {
                            _UserID = currentUserID;
                            ViewBag.UserID = _UserID;
                        }

                    }
                }
            }
            var readLaterDataContext = _context.Bookmark.Include(b => b.Category);
            return View(await readLaterDataContext.ToListAsync());
        }

        // GET: Bookmarks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bookmark == null)
            {
                return NotFound();
            }
            if (this != null)
            {
                if (this.User != null)
                {
                    ClaimsPrincipal currentUser = this.User;
                    var currentUserData = currentUser.FindFirst(ClaimTypes.NameIdentifier);
                    if (currentUserData != null)
                    {
                        var currentUserID = currentUserData.Value;
                        if (currentUserID != null)
                        {
                            _UserID = currentUserID;
                            ViewBag.UserID = _UserID;
                        }

                    }
                }
            }
            var bookmark = await _context.Bookmark
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (bookmark == null)
            {
                return NotFound();
            }

            return View(bookmark);
        }

        // GET: Bookmarks/Create
        public IActionResult Create()
        {
            if (this != null)
            {
                if (this.User != null)
                {
                    ClaimsPrincipal currentUser = this.User;
                    var currentUserData = currentUser.FindFirst(ClaimTypes.NameIdentifier);
                    if (currentUserData != null)
                    {
                        var currentUserID = currentUserData.Value;
                        if (currentUserID != null)
                        {
                            _UserID = currentUserID;
                            ViewBag.UserID = _UserID;
                        }

                    }
                }
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "ID", "ID");
            return View();
        }

        // POST: Bookmarks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,UserID,URL,ShortDescription,CategoryId,CreateDate")] Bookmark bookmark)
        {
            if (ModelState.IsValid)
            {
                if (this != null)
                {
                    if (this.User != null)
                    {
                        ClaimsPrincipal currentUser = this.User;
                        var currentUserData = currentUser.FindFirst(ClaimTypes.NameIdentifier);
                        if (currentUserData != null)
                        {
                            var currentUserID = currentUserData.Value;
                            if (currentUserID != null)
                            {
                                _UserID = currentUserID;
                                ViewBag.UserID = _UserID;
                            }

                        }
                    }
                }
                _context.Add(bookmark);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "ID", "ID", bookmark.CategoryId);
            return View(bookmark);
        }

        // GET: Bookmarks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Bookmark == null)
            {
                return NotFound();
            }
            if (this != null)
            {
                if (this.User != null)
                {
                    ClaimsPrincipal currentUser = this.User;
                    var currentUserData = currentUser.FindFirst(ClaimTypes.NameIdentifier);
                    if (currentUserData != null)
                    {
                        var currentUserID = currentUserData.Value;
                        if (currentUserID != null)
                        {
                            _UserID = currentUserID;
                            ViewBag.UserID = _UserID;
                        }

                    }
                }
            }
            var bookmark = await _context.Bookmark.FindAsync(id);
            if (bookmark == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "ID", "ID", bookmark.CategoryId);
            return View(bookmark);
        }

        // POST: Bookmarks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,UserID,URL,ShortDescription,CategoryId,CreateDate")] Bookmark bookmark)
        {
            if (id != bookmark.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (this != null)
                {
                    if (this.User != null)
                    {
                        ClaimsPrincipal currentUser = this.User;
                        var currentUserData = currentUser.FindFirst(ClaimTypes.NameIdentifier);
                        if (currentUserData != null)
                        {
                            var currentUserID = currentUserData.Value;
                            if (currentUserID != null)
                            {
                                _UserID = currentUserID;
                                ViewBag.UserID = _UserID;
                            }

                        }
                    }
                }
                try
                {
                    _context.Update(bookmark);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookmarkExists(bookmark.ID))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "ID", "ID", bookmark.CategoryId);
            return View(bookmark);
        }

        // GET: Bookmarks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Bookmark == null)
            {
                return NotFound();
            }
            if (this != null)
            {
                if (this.User != null)
                {
                    ClaimsPrincipal currentUser = this.User;
                    var currentUserData = currentUser.FindFirst(ClaimTypes.NameIdentifier);
                    if (currentUserData != null)
                    {
                        var currentUserID = currentUserData.Value;
                        if (currentUserID != null)
                        {
                            _UserID = currentUserID;
                            ViewBag.UserID = _UserID;
                        }

                    }
                }
            }
            var bookmark = await _context.Bookmark
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (bookmark == null)
            {
                return NotFound();
            }

            return View(bookmark);
        }

        // POST: Bookmarks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Bookmark == null)
            {
                return Problem("Entity set 'ReadLaterDataContext.Bookmark'  is null.");
            }
            if (this != null)
            {
                if (this.User != null)
                {
                    ClaimsPrincipal currentUser = this.User;
                    var currentUserData = currentUser.FindFirst(ClaimTypes.NameIdentifier);
                    if (currentUserData != null)
                    {
                        var currentUserID = currentUserData.Value;
                        if (currentUserID != null)
                        {
                            _UserID = currentUserID;
                            ViewBag.UserID = _UserID;
                        }

                    }
                }
            }
            var bookmark = await _context.Bookmark.FindAsync(id);
            if (bookmark != null)
            {
                _context.Bookmark.Remove(bookmark);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookmarkExists(int id)
        {
          return _context.Bookmark.Any(e => e.ID == id);
        }
    }
}
