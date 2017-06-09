namespace entities.Models
{
    public partial class Collection
    {
        public int Id { get; set; }
        public int? TmdbId { get; set; }
        public string Name { get; set; }
        public string PosterPath { get; set; }
        public string BackdropPath { get; set; }
    }
}
