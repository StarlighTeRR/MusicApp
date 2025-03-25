using System.ComponentModel.DataAnnotations;
using MusicAppApi.Models;

namespace MusicAppApi.DTOs 
{ 
    public class CreateTrackDto 
    { 
        [Required] public required string Name { get; set; }

        [Required]
        public required string Duration { get; set; }

      
        public bool IsFavorite { get; set; } = false;
        public bool IsListened { get; set; } = false;

        public RatingType Rating { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "ID альбома не может быть 0")]
        public int AlbumId { get; set; }
}



}