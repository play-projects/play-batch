using System;
using System.Collections.Generic;

namespace batch.Models
{
    public class Movie
    {
        public int TraktId { get; set; }
        public string ImdbId { get; set; }
        public int TmdbId { get; set; }

        public List<Torrent> Torrents { get; set; }

        public string Title { get; set; }
        public string TitleSearch { get; set; }
        public string OriginalTitle { get; set; }
        public int Year { get; set; }
        public List<Genre> Genres { get; set; }
        public string OriginalLanguage { get; set; }
        public string Overview { get; set; }
        public string Tagline { get; set; }
        public double Popularity { get; set; }
        public DateTime ReleaseDate { get; set; }
        public float VoteAverage { get; set; }
        public int VoteCount { get; set; }
        public int Runtime { get; set; }

        public string BackdropPath { get; set; }
        public string PosterPath { get; set; }

        public Collection Collection { get; set; }
        public List<Person> Casting { get; set; }

        public static Movie NotFound = new Movie
        {
            TraktId = 0,
            ImdbId = string.Empty,
            TmdbId = 0,
            Torrents = new List<Torrent>(),

            Title = string.Empty,
            OriginalTitle = string.Empty,
            Year = 0,
            Genres = new List<Genre>(),
            OriginalLanguage = string.Empty,
            Overview = string.Empty,
            Tagline = string.Empty,
            Popularity = 0,
            ReleaseDate = DateTime.MinValue,
            VoteAverage = 0,
            VoteCount = 0,
            Runtime = 0,

            BackdropPath = string.Empty,
            PosterPath = string.Empty,

            Collection = null,
            Casting = new List<Person>()
        };
    }
}
