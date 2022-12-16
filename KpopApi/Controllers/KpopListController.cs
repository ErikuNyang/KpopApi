using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KpopApi.Models;
using KpopApi.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace KpopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KpopListController : ControllerBase
    {
        private readonly KpopContext _context;
                
        public KpopListController(KpopContext context)
        {
            _context = context;
        }

        // ex GET: api/Artists
        [Authorize]
        [HttpGet("Artists")]
        public async Task<ActionResult<IEnumerable<Artist>>> GetArtists()
        {
            return await _context.Artists.ToListAsync();
        }

        [HttpGet("Songs")]
        public async Task<ActionResult<IEnumerable<Song>>> GetSongs()
        {
            return await _context.Songs.ToListAsync();
        }

        // GET: api/Countries/5
        [HttpGet("Artist/{id}")]
        public async Task<ActionResult> GetArtist(int id)
        {
            var artistDTO = await _context.Artists
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.DateDebut,
                })
                .SingleOrDefaultAsync(c => c.Id == id);


            if (artistDTO == null)
            {
                return NotFound();
            }

            return Ok(artistDTO);
        }

        [HttpGet("Song/{id}")]
        public async Task<ActionResult> GetSong(int id)
        {
            var songDTO = await _context.Songs
                .Select(c => new
                {
                    c.Id,
                    c.Title,
                    c.Release,
                    c.Summary,
                    c.Artist.Name
                })
                .SingleOrDefaultAsync(c => c.Id == id);


            if (songDTO == null)
            {
                return NotFound();
            }

            return Ok(songDTO);
        }

        [HttpGet("ArtistSong/{id}")]
        public async Task<ActionResult> GetArtistWithSong(int id)
        {
            /*ArtistSong? artistDTO = await _context.Artists
               .Where(c => c.Id == id)
               .Select(c => new ArtistSong
               {
                   Id = c.Id,
                   Name = c.Name,
                   Title = c.Songs.Select(t => t.Title)
               }).SingleOrDefaultAsync();
            */
            var artistDTO = await _context.Artists
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    SongList = c.Songs

                })
                .SingleOrDefaultAsync(c => c.Id == id);
            

            if (artistDTO == null)
            {
                return NotFound();
            }

            return Ok(artistDTO);
        }

    }
}
