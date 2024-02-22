using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Entity;

namespace ReadLater5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookmarkController : ControllerBase
    {
        private readonly ReadLaterDataContext _context;

        public BookmarkController(ReadLaterDataContext context)
        {
            _context = context;
        }

        // GET: api/Bookmark
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bookmark>>> GetBookmark(string UserID, int id)
        {
            if (string.IsNullOrEmpty(UserID))
            {
                //return await _context.Bookmark.ToListAsync();
                return null;
            } else
            {
                if(id == 0)
                {
                    return await _context.Bookmark
                        .Include(x => x.Category)
                        .Where(c => c.UserID == UserID).ToListAsync();

                } else
                {
                    return await _context.Bookmark
                    .Include(x => x.Category)
                    .Where(c => c.UserID == UserID && c.ID == id).ToListAsync();

                }
            }
                
        }

        // PUT: api/Bookmark/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutBookmark(int id, Bookmark bookmark)
        {
            if (id != bookmark.ID)
            {
                return BadRequest();
            }

            _context.Entry(bookmark).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookmarkExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Bookmark
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Bookmark>> PostBookmark(Bookmark bookmark)
        {
            bookmark.CreateDate = DateTime.UtcNow;
            _context.Bookmark.Add(bookmark);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBookmark", new { id = bookmark.ID }, bookmark);
        }

        // DELETE: api/Bookmark/5
        [HttpDelete]
        public async Task<IActionResult> DeleteBookmark(int id)
        {
            var bookmark = await _context.Bookmark.FindAsync(id);
            if (bookmark == null)
            {
                return NotFound();
            }

            _context.Bookmark.Remove(bookmark);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookmarkExists(int id)
        {
            return _context.Bookmark.Any(e => e.ID == id);
        }
    }
}
