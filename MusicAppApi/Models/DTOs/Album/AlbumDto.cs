namespace MusicAppApi.DTOs
{
    public class AlbumDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Genre { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ArtistId { get; set; }
        public string? ArtistName { get; set; }
    }
}