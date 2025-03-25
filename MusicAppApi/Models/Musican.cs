using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicAppApi.Models;

public class MusicData
{
    public List<Musician> Musicians { get; set; } = new List<Musician>();
}

public class Musician
{
    [Key]
    public int Id { get; set; }
    [Required]
    public required string Name { get; set; }
    public int Age { get; set; }
    public required string Genre { get; set; }
    public int CareerStartYear { get; set; }
    public ICollection<Album> Albums { get; set; } = new List<Album>();
}