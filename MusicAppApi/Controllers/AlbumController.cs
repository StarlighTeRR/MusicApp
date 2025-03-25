using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicAppApi.Models;
using MusicAppApi.DTOs; 
using MusicAppApi.Services; 
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicAppApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlbumController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMusicAppLogger _logger; 

        public AlbumController(AppDbContext context, IMusicAppLogger logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Album
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlbumDto>>> GetAlbums()
        {
            // Логируем запрос списка альбомов
            await _logger.LogActionAsync("ViewList", "Album", 0);
            
            var albums = await _context.Albums.Include(a => a.Artist).ToListAsync();
            
            var albumDtos = albums.Select(a => new AlbumDto
            {
                Id = a.Id,
                Title = a.Title,
                Genre = a.Genre,
                ReleaseDate = a.ReleaseDate,
                ArtistId = a.ArtistId,
                ArtistName = a.Artist?.Name
            }).ToList();
            
            return albumDtos;
        }

        // GET: api/Album/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AlbumDto>> GetAlbum(int id)
        {
            var album = await _context.Albums.Include(a => a.Artist)
                                            .FirstOrDefaultAsync(a => a.Id == id);

            if (album == null)
            {
                return NotFound();
            }

            // Логируем просмотр альбома
            await _logger.LogActionAsync("View", "Album", id, $"Title: {album.Title}, Artist: {album.Artist?.Name}");

            var albumDto = new AlbumDto
            {
                Id = album.Id,
                Title = album.Title,
                Genre = album.Genre,
                ReleaseDate = album.ReleaseDate,
                ArtistId = album.ArtistId,
                ArtistName = album.Artist?.Name
            };

            return albumDto;
        }

        // POST: api/Album
        [HttpPost]
        public async Task<ActionResult<Album>> CreateAlbum(CreateAlbumDto createAlbumDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Если ReleaseDate в DTO может быть null, проверяем его наличие.
            if (!createAlbumDto.ReleaseDate.HasValue)
            {
                return BadRequest("ReleaseDate is required.");
            }

            var album = new Album
            {
                Title = createAlbumDto.Title,
                ArtistId = createAlbumDto.ArtistId,
                Artist = null!, 
                Genre = createAlbumDto.Genre,
                ReleaseDate = createAlbumDto.ReleaseDate.Value 
            };

            _context.Albums.Add(album);
            await _context.SaveChangesAsync();

            // Логируем создание альбома
            await _logger.LogActionAsync("Create", "Album", album.Id, 
                $"Title: {album.Title}, Genre: {album.Genre}, ReleaseDate: {album.ReleaseDate}, ArtistId: {album.ArtistId}");

            return CreatedAtAction(nameof(GetAlbum), new { id = album.Id }, album);
        }

        // PUT: api/Album/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAlbum(int id, UpdateAlbumDto updateAlbumDto)
        {
            if (id != updateAlbumDto.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var album = await _context.Albums.FindAsync(id);
            if (album == null)
            {
                return NotFound();
            }

            // Если ReleaseDate в DTO может быть null, проверяем его наличие.
            if (!updateAlbumDto.ReleaseDate.HasValue)
            {
                return BadRequest("ReleaseDate is required.");
            }

            // Сохраняем старые значения для лога
            var oldTitle = album.Title;
            var oldArtistId = album.ArtistId;
            var oldGenre = album.Genre;
            var oldReleaseDate = album.ReleaseDate;

            album.Title = updateAlbumDto.Title;
            album.ArtistId = updateAlbumDto.ArtistId;
            album.Genre = updateAlbumDto.Genre;
            album.ReleaseDate = updateAlbumDto.ReleaseDate.Value;

            _context.Entry(album).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                
                // Логируем обновление альбома
                await _logger.LogActionAsync("Update", "Album", id, 
                    $"Old values - Title: {oldTitle}, ArtistId: {oldArtistId}, Genre: {oldGenre}, ReleaseDate: {oldReleaseDate}. " +
                    $"New values - Title: {album.Title}, ArtistId: {album.ArtistId}, Genre: {album.Genre}, ReleaseDate: {album.ReleaseDate}");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlbumExists(id))
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

        // DELETE: api/Album/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlbum(int id)
        {
            var album = await _context.Albums.FindAsync(id);
            if (album == null)
            {
                return NotFound();
            }

            // Логируем удаление альбома
            await _logger.LogActionAsync("Delete", "Album", id, $"Title: {album.Title}, Genre: {album.Genre}, ArtistId: {album.ArtistId}");

            _context.Albums.Remove(album);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AlbumExists(int id)
        {
            return _context.Albums.Any(e => e.Id == id);
        }
    }
}