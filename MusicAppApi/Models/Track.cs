using System.ComponentModel.DataAnnotations;

namespace MusicAppApi.Models;

public enum RatingType
{
    None = 0,
    Like = 1,
    Dislike = 2
}

public class Track
{
    public int Id { get; set; }
    
    [Required]
    public required string Name { get; set; }
    
    [Required]
    [RegularExpression(@"^([0-9]{1,2}):([0-5][0-9])$", ErrorMessage =
    "Продолжительность должна быть в формате MM:SS (например, 3:45 или 10:30)")]
    public required string Duration { get; set; }
    
    public bool IsFavorite { get; set; }
    public bool IsListened { get; set; }
    
    public RatingType Rating { get; set; } = RatingType.None;
    

    public int LikesCount { get; set; } = 0;
    public int DislikesCount { get; set; } = 0;
    
    public int AlbumId { get; set; }
    public required Album Album { get; set; }

    public Track() { }
}
