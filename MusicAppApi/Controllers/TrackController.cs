using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using MusicAppApi.Models; 
using MusicAppApi.DTOs;
using MusicAppApi.Services;

namespace MusicAppApi.Controllers 
{
    [ApiController] 
    [Route("api/[controller]")]
    public class TrackController : ControllerBase 
    {
        private readonly AppDbContext _context;
        private readonly IMusicAppLogger _logger;

        public TrackController(AppDbContext context, IMusicAppLogger logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Track/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Track>> GetTrack(int id)
        {
            var track = await _context.Tracks.Include(t => t.Album)
                                             .FirstOrDefaultAsync(t => t.Id == id);

            if (track == null)
            {
                return NotFound();
            }
            await _logger.LogActionAsync("View", "Track", id);

            return track;
        }
        
        // GET: api/Track
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrackDto>>> GetTracks([FromQuery] int? albumId)
        {

            if (albumId.HasValue)
            {
                await _logger.LogActionAsync("ViewList", "Track", 0, $"AlbumId: {albumId.Value}");
            }
            else
            {
                await _logger.LogActionAsync("ViewList", "Track", 0);
            }

            IQueryable<Track> query = _context.Tracks.Include(t => t.Album);
            
            if (albumId.HasValue)
            {
                query = query.Where(t => t.AlbumId == albumId.Value);
            }
            
            var tracks = await query.ToListAsync();
            
            var trackDtos = tracks.Select(t => new TrackDto
            {
                Id = t.Id,
                Name = t.Name,
                Duration = t.Duration,
                IsFavorite = t.IsFavorite,
                IsListened = t.IsListened,
                Rating = t.Rating,
                LikesCount = t.LikesCount,    
                DislikesCount = t.DislikesCount,
                AlbumId = t.AlbumId,
                Album = t.Album != null ? new AlbumInfoDto
                {
                    Id = t.Album.Id,
                    Title = t.Album.Title ?? string.Empty
                } : null!
            }).ToList();
            
            return trackDtos;
        }
        
        // POST: api/Track/{id}/favorite
        [HttpPost("{id}/favorite")]
        public async Task<ActionResult<TrackDto>> AddToFavorites(int id)
        {
            var track = await _context.Tracks.FindAsync(id);
            
            if (track == null)
            {
                return NotFound();
            }
            

            await _logger.LogTrackAddedToFavoritesAsync(id);
            
            track.IsFavorite = true;
            
            _context.Entry(track).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
            var trackDto = new TrackDto
            {
                Id = track.Id,
                Name = track.Name,
                Duration = track.Duration,
                IsFavorite = track.IsFavorite,
                IsListened = track.IsListened,
                Rating = track.Rating,
                LikesCount = track.LikesCount,     
                DislikesCount = track.DislikesCount,
                AlbumId = track.AlbumId
            };
            
            return trackDto;
        }

        // DELETE: api/Track/{id}/favorite
        [HttpDelete("{id}/favorite")]
        public async Task<ActionResult<TrackDto>> RemoveFromFavorites(int id)
        {
            var track = await _context.Tracks.FindAsync(id);
            
            if (track == null)
            {
                return NotFound();
            }
            

            await _logger.LogTrackRemovedFromFavoritesAsync(id);
            
            track.IsFavorite = false;
            
            _context.Entry(track).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
            var trackDto = new TrackDto
            {
                Id = track.Id,
                Name = track.Name,
                Duration = track.Duration,
                IsFavorite = track.IsFavorite,
                IsListened = track.IsListened,
                Rating = track.Rating,
                LikesCount = track.LikesCount,     
                DislikesCount = track.DislikesCount,
                AlbumId = track.AlbumId
            };
                        
            return trackDto;
        }

        // POST: api/Track/{id}/listened
        [HttpPost("{id}/listened")]
        public async Task<ActionResult<TrackDto>> MarkAsListened(int id)
        {
            var track = await _context.Tracks.FindAsync(id);
            
            if (track == null)
            {
                return NotFound();
            }
            
            // Логируем отметку трека как прослушанного
            await _logger.LogTrackMarkedAsListenedAsync(id);
            
            track.IsListened = true;
            
            _context.Entry(track).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
            var trackDto = new TrackDto
            {
                Id = track.Id,
                Name = track.Name,
                Duration = track.Duration,
                IsFavorite = track.IsFavorite,
                IsListened = track.IsListened,
                Rating = track.Rating,
                LikesCount = track.LikesCount,     
                DislikesCount = track.DislikesCount,
                AlbumId = track.AlbumId
            };
            
            return trackDto;
        }

        // POST: api/Track/{id}/rate
        [HttpPost("{id}/rate")]
        public async Task<ActionResult<TrackDto>> RateTrack(int id, [FromBody] RateTrackDto rateTrackDto)
        {
            var track = await _context.Tracks.FindAsync(id);
            
            if (track == null)
            {
                return NotFound();
            }
            
            // Проверяем, что значение рейтинга соответствует перечислению RatingType
            if (!Enum.IsDefined(typeof(RatingType), rateTrackDto.Rating))
            {
                return BadRequest($"Недопустимое значение рейтинга. Допустимые значения: {string.Join(", ", Enum.GetNames(typeof(RatingType)))}");
            }
            
            // Логируем оценку трека
            await _logger.LogTrackRatedAsync(id, rateTrackDto.Rating);
            
            // Обновляем счетчики лайков и дизлайков в зависимости от изменения рейтинга
            if (track.Rating != rateTrackDto.Rating)
            {
                // Уменьшаем счетчик предыдущего рейтинга, если он был
                if (track.Rating == RatingType.Like)
                {
                    track.LikesCount = Math.Max(0, track.LikesCount - 1);
                }
                else if (track.Rating == RatingType.Dislike)
                {
                    track.DislikesCount = Math.Max(0, track.DislikesCount - 1);
                }
                
                // Увеличиваем счетчик нового рейтинга, если он не None
                if (rateTrackDto.Rating == RatingType.Like)
                {
                    track.LikesCount++;
                }
                else if (rateTrackDto.Rating == RatingType.Dislike)
                {
                    track.DislikesCount++;
                }
            }
            
            track.Rating = rateTrackDto.Rating;
            
            _context.Entry(track).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
            var trackDto = new TrackDto
            {
                Id = track.Id,
                Name = track.Name,
                Duration = track.Duration,
                IsFavorite = track.IsFavorite,
                IsListened = track.IsListened,
                Rating = track.Rating,
                LikesCount = track.LikesCount,     
                DislikesCount = track.DislikesCount,
                AlbumId = track.AlbumId
            };
            
            return trackDto;
        }

        // POST: api/Track
        [HttpPost]
        public async Task<ActionResult<TrackDto>> CreateTrack(CreateTrackDto createTrackDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Загружаем альбом для установки в свойство Album
            var album = await _context.Albums.FirstOrDefaultAsync(a => a.Id == createTrackDto.AlbumId);
            if (album == null)
            {
                return BadRequest($"Альбом с ID {createTrackDto.AlbumId} не существует.");
            }

            // Проверяем, что значение рейтинга соответствует перечислению RatingType
            if (!Enum.IsDefined(typeof(RatingType), createTrackDto.Rating))
            {
                return BadRequest($"Недопустимое значение рейтинга. Допустимые значения: {string.Join(", ", Enum.GetNames(typeof(RatingType)))}");
            }

            var track = new Track
                {
                    Name = createTrackDto.Name,
                    Duration = createTrackDto.Duration,
                    IsFavorite = createTrackDto.IsFavorite,
                    IsListened = createTrackDto.IsListened,
                    Rating = createTrackDto.Rating,
                    LikesCount = createTrackDto.Rating == RatingType.Like ? 1 : 0,     
                    DislikesCount = createTrackDto.Rating == RatingType.Dislike ? 1 : 0,
                    AlbumId = createTrackDto.AlbumId,
                    Album = album
                };

            _context.Tracks.Add(track);
            await _context.SaveChangesAsync();

            // Логируем создание трека
            await _logger.LogActionAsync("Create", "Track", track.Id, $"Name: {track.Name}, AlbumId: {track.AlbumId}");

            var trackDto = new TrackDto
            {
                Id = track.Id,
                Name = track.Name,
                Duration = track.Duration,
                IsFavorite = track.IsFavorite,
                IsListened = track.IsListened,
                Rating = track.Rating,
                LikesCount = track.LikesCount,     
                DislikesCount = track.DislikesCount,
                AlbumId = track.AlbumId,
                Album = new AlbumInfoDto
                {
                    Id = album.Id,
                    Title = album.Title
                }
            };

            return CreatedAtAction(nameof(GetTrack), new { id = track.Id }, trackDto);
        }

        // PUT: api/Track/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrack(int id, UpdateTrackDto updateTrackDto)
        {
            if (id != updateTrackDto.Id)
            {
                return BadRequest();
            }

            var track = await _context.Tracks.FindAsync(id);
            if (track == null)
            {
                return NotFound();
            }

            // Проверяем, что значение рейтинга соответствует перечислению RatingType
            if (!Enum.IsDefined(typeof(RatingType), updateTrackDto.Rating))
            {
                return BadRequest($"Недопустимое значение рейтинга. Допустимые значения: {string.Join(", ", Enum.GetNames(typeof(RatingType)))}");
            }

            // Логируем обновление трека
            await _logger.LogActionAsync("Update", "Track", id, $"Name: {updateTrackDto.Name}, AlbumId: {updateTrackDto.AlbumId}");

            // Обновляем счетчики лайков и дизлайков, если рейтинг изменился
            if (track.Rating != updateTrackDto.Rating)
            {
                // Уменьшаем счетчик предыдущего рейтинга, если он был
                if (track.Rating == RatingType.Like)
                {
                    track.LikesCount = Math.Max(0, track.LikesCount - 1);
                }
                else if (track.Rating == RatingType.Dislike)
                {
                    track.DislikesCount = Math.Max(0, track.DislikesCount - 1);
                }
                
                // Увеличиваем счетчик нового рейтинга, если он не None
                if (updateTrackDto.Rating == RatingType.Like)
                {
                    track.LikesCount++;
                }
                else if (updateTrackDto.Rating == RatingType.Dislike)
                {
                    track.DislikesCount++;
                }
            }

            // Обновляем свойства из DTO.
            track.Name = updateTrackDto.Name;
            track.Duration = updateTrackDto.Duration;
            track.IsFavorite = updateTrackDto.IsFavorite;
            track.IsListened = updateTrackDto.IsListened;
            track.Rating = updateTrackDto.Rating;
            track.AlbumId = updateTrackDto.AlbumId;

            _context.Entry(track).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Tracks.Any(t => t.Id == id))
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
        
        // DELETE: api/Track/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrack(int id)
        {
            var track = await _context.Tracks.FindAsync(id);
            if (track == null)
            {
                return NotFound();
            }

            await _logger.LogActionAsync("Delete", "Track", id, $"Name: {track.Name}");

            _context.Tracks.Remove(track);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}