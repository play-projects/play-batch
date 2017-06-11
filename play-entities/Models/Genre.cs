using System;

namespace entities.Models
{
    public partial class Genre
    {
        public int Id { get; set; }
        public int TmdbId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
