using System.ComponentModel.DataAnnotations;

namespace MusicAppApi.DTOs
{
    public class UpdateMusicianDto
    {
        [Required]
        public int Id { get; set; }
            
        [Required]
        public required string Name { get; set; }
            
        public int Age { get; set; }
            
        [Required]
        public required string Genre { get; set; }
            
        public int CareerStartYear { get; set; }
    }
}