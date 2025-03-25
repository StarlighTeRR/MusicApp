namespace MusicAppApi.DTOs
{
    public class MusicianDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int Age { get; set; }
        public required string Genre { get; set; }
        public int CareerStartYear { get; set; }
        public List<AlbumInfoDto> Albums { get; set; } = new List<AlbumInfoDto>();
    }
}