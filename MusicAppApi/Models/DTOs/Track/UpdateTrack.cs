using System.ComponentModel.DataAnnotations;
using MusicAppApi.Models;

public class UpdateTrackDto
{
    [Required]
    public int Id { get; set; }
    
    [Required]
    public required string Name { get; set; }
    
    [Required]
    public required string Duration { get; set; }

    public bool IsFavorite { get; set; } = false;
    public bool IsListened { get; set; } = false;

    public RatingType Rating { get; set; }

    [Required]
    public int AlbumId { get; set; }
}

