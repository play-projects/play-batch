namespace entities.Models
{
    public partial class Torrent
    {
        public int Id { get; set; }
        public int T411Id { get; set; }
        public int? MovieId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int? Year { get; set; }
        public long? Size { get; set; }
        public int? Seeders { get; set; }
        public int? Leechers { get; set; }
        public int? Completed { get; set; }
        public int CategoryId { get; set; }
        public int LanguageId { get; set; }
        public int QualityId { get; set; }
    }
}
