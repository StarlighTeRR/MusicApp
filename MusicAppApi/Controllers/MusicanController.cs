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
    public class MusicianController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMusicAppLogger _logger; 

        public MusicianController(AppDbContext context, IMusicAppLogger logger)
        {
            _context = context;
            _logger = logger; 
        }

        // GET: api/Musician
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MusicianDto>>> GetMusicians()
        {

            await _logger.LogActionAsync("ViewList", "Musician", 0);
            
            var musicians = await _context.Musicians.Include(m => m.Albums).ToListAsync();
            
            var musicianDtos = musicians.Select(m => new MusicianDto
            {
                Id = m.Id,
                Name = m.Name,
                Age = m.Age,
                Genre = m.Genre,
                CareerStartYear = m.CareerStartYear,
                Albums = m.Albums.Select(a => new AlbumInfoDto
                {
                    Id = a.Id,
                    Title = a.Title
                }).ToList()
            }).ToList();
            
            return musicianDtos;
        }

        // GET: api/Musician/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MusicianDto>> GetMusician(int id)
        {
            var musician = await _context.Musicians.Include(m => m.Albums)
                                                  .FirstOrDefaultAsync(m => m.Id == id);

            if (musician == null)
            {
                return NotFound();
            }


            await _logger.LogActionAsync("View", "Musician", id, $"Name: {musician.Name}");

            var musicianDto = new MusicianDto
            {
                Id = musician.Id,
                Name = musician.Name,
                Age = musician.Age,
                Genre = musician.Genre,
                CareerStartYear = musician.CareerStartYear,
                Albums = musician.Albums.Select(a => new AlbumInfoDto
                {
                    Id = a.Id,
                    Title = a.Title
                }).ToList()
            };

            return musicianDto;
        }

        // POST: api/Musician
        [HttpPost]
        public async Task<ActionResult<MusicianDto>> CreateMusician(CreateMusicianDto createMusicianDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var musician = new Musician
            {
                Name = createMusicianDto.Name,
                Age = createMusicianDto.Age,
                Genre = createMusicianDto.Genre,
                CareerStartYear = createMusicianDto.CareerStartYear
            };

            _context.Musicians.Add(musician);
            await _context.SaveChangesAsync();


            await _logger.LogActionAsync("Create", "Musician", musician.Id, 
                $"Name: {musician.Name}, Age: {musician.Age}, Genre: {musician.Genre}, CareerStartYear: {musician.CareerStartYear}");

            var musicianDto = new MusicianDto
            {
                Id = musician.Id,
                Name = musician.Name,
                Age = musician.Age,
                Genre = musician.Genre,
                CareerStartYear = musician.CareerStartYear,
                Albums = new List<AlbumInfoDto>() // Пустой список, так как новый музыкант еще не имеет алььомов
            };

            return CreatedAtAction(nameof(GetMusician), new { id = musician.Id }, musicianDto);
        }

        // PUT: api/Musician/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMusician(int id, UpdateMusicianDto updateMusicianDto)
        {
            if (id != updateMusicianDto.Id)
            {
                return BadRequest();
            }

            var musician = await _context.Musicians.FindAsync(id);
            if (musician == null)
            {
                return NotFound();
            }

            // Сохраняем старые значения для лога
            var oldName = musician.Name;
            var oldAge = musician.Age;
            var oldGenre = musician.Genre;
            var oldCareerStartYear = musician.CareerStartYear;

            musician.Name = updateMusicianDto.Name;
            musician.Age = updateMusicianDto.Age;
            musician.Genre = updateMusicianDto.Genre;
            musician.CareerStartYear = updateMusicianDto.CareerStartYear;

            _context.Entry(musician).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                

                await _logger.LogActionAsync("Update", "Musician", id, 
                    $"Old values - Name: {oldName}, Age: {oldAge}, Genre: {oldGenre}, CareerStartYear: {oldCareerStartYear}. " +
                    $"New values - Name: {musician.Name}, Age: {musician.Age}, Genre: {musician.Genre}, CareerStartYear: {musician.CareerStartYear}");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Musicians.Any(m => m.Id == id))
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

        // DELETE: api/Musician/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMusician(int id)
        {
            var musician = await _context.Musicians.FindAsync(id);
            if (musician == null)
            {
                return NotFound();
            }

            await _logger.LogActionAsync("Delete", "Musician", id, $"Name: {musician.Name}, Age: {musician.Age}, Genre: {musician.Genre}");

            _context.Musicians.Remove(musician);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}