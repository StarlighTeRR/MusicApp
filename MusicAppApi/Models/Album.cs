// Models/Album.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace MusicAppApi.Models
{
    public class Album
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public required string Title { get; set; }

        [Required]
        public int ArtistId { get; set; } 
        public required  Musician Artist { get; set; } 

        public required string Genre { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}