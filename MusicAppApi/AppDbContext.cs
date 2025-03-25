using Microsoft.EntityFrameworkCore;
using MusicAppApi.Models; 

namespace MusicAppApi
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Musician> Musicians { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<UserActionLog> UserActionLogs { get; set; }
    }
}