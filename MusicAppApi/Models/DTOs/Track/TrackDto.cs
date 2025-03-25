using MusicAppApi.Models;

namespace MusicAppApi.DTOs
{
public class TrackDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public bool IsFavorite { get; set; }
        public bool IsListened { get; set; }
        public RatingType Rating { get; set; }
        public int LikesCount { get; set; }     // Новое поле
        public int DislikesCount { get; set; }  // Новое поле
        public int AlbumId { get; set; }
        public AlbumInfoDto? Album { get; set; }
    }
}