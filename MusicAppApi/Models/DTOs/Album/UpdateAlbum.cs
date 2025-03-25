using System.ComponentModel.DataAnnotations;

namespace MusicAppApi.DTOs
{
    public class UpdateAlbumDto
    {
        [Required(ErrorMessage = "Id альбома обязателен.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Название альбома обязательно.")]
        [StringLength(200, ErrorMessage = "Название альбома не должно превышать 200 символов.")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "ArtistId обязателен.")]
        public int ArtistId { get; set; }

        public required string Genre { get; set; }
        public DateTime? ReleaseDate { get; set; } // Сделал nullable, чтобы не требовалось при обновлении
    }
}