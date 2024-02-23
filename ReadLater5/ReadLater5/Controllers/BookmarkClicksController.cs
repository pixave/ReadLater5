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
    public class BookmarkClicksController : ControllerBase
    {
        private readonly ReadLaterDataContext _context;

        public BookmarkClicksController(ReadLaterDataContext context)
        {
            _context = context;
        }

        // GET: api/BookmarkClicks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookmarkClick>>> GetBookmarkClick()
        {
            return await _context.BookmarkClick.ToListAsync();
        }

        // GET: api/BookmarkClicks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookmarkClick>> GetBookmarkClick(int id)
        {
            var bookmarkClick = await _context.BookmarkClick.FindAsync(id);

            if (bookmarkClick == null)
            {
                return NotFound();
            }

            return bookmarkClick;
        }

        // PUT: api/BookmarkClicks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutBookmarkClick(int id, BookmarkClick bookmarkClick)
        {
            if (id != bookmarkClick.ID)
            {
                return BadRequest();
            }

            _context.Entry(bookmarkClick).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookmarkClickExists(id))
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

        // POST: api/BookmarkClicks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BookmarkClick>> PostBookmarkClick(BookmarkClick bookmarkClick)
        {
            bookmarkClick.ClickDateTime = DateTime.UtcNow;
            _context.BookmarkClick.Add(bookmarkClick);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBookmarkClick", new { id = bookmarkClick.ID }, bookmarkClick);
        }

        // DELETE: api/BookmarkClicks/5
        [HttpDelete]
        public async Task<IActionResult> DeleteBookmarkClick(int id)
        {
            var bookmarkClick = await _context.BookmarkClick.FindAsync(id);
            if (bookmarkClick == null)
            {
                return NotFound();
            }

            _context.BookmarkClick.Remove(bookmarkClick);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookmarkClickExists(int id)
        {
            return _context.BookmarkClick.Any(e => e.ID == id);
        }
    }
}
