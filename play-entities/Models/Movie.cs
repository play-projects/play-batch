using System;

namespace entities.Models
{
    public partial class Movie
    {
        public int Id { get; set; }
        public int? CollectionId { get; set; }
        public int TraktId { get; set; }
        public string ImdbId { get; set; }
        public int TmdbId { get; set; }
        public string Title { get; set; }
        public string TitleSearch { get; set; }
        public string OriginalTitle { get; set; }
        public int Year { get; set; }
        public string OriginalLanguage { get; set; }
        public string Overview { get; set; }
        public string Tagline { get; set; }
        public decimal Popularity { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public decimal VoteAverage { get; set; }
        public int VoteCount { get; set; }
        public int? Runtime { get; set; }
        public string BackdropPath { get; set; }
        public string PosterPath { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
