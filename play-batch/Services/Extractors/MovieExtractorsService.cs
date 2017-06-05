using batch.Models;
using batch.Services.Parser;
using batch.Services.Web;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace batch.Services.Tmdb
{
    public class MovieExtractorsService
    {
        public static MovieExtractorsService Instance = new MovieExtractorsService();
        private MovieExtractorsService() { }

        private ParserFacade parser = ParserFacade.Instance;

        public List<Movie> GetMoviesIds(List<Torrent> torrents)
        {
            var movies = new List<Movie>();
            Parallel.ForEach(torrents, new ParallelOptions { MaxDegreeOfParallelism = 50 }, torrent =>
            {
                var movie = GetMovieIds(torrent);
                if (movie == Movie.NotFound)
                    return;
                lock (movies)
                {
                    movies.Add(movie);
                    Console.WriteLine($"movie: {movie.OriginalTitle} - {movie.TmdbId}");
                }
            });

            return movies.GroupBy(m => m.ImdbId).Select(m => new Movie
            {
                OriginalTitle = m.First().OriginalTitle,
                Year = m.First().Year,
                TraktId = m.First().TraktId,
                ImdbId = m.First().ImdbId,
                TmdbId = m.First().TmdbId,
                Torrents = m.SelectMany(t => t.Torrents).ToList()
            }).OrderBy(m => m.OriginalTitle).ToList();
        }

        private Movie GetMovieIds(Torrent torrent)
        {
            var url = $"https://api.trakt.tv/search/movie?query={torrent.Slug}";
            var header = new Dictionary<string, string>
            {
                { "trakt-api-version", "2" },
                { "trakt-api-key", "9cec2a64b9ccc726e00c0f5b73aaaaaaf9f443e7131916307bef33b83b0a6b5e" }
            };

            var content = WebService.Instance.GetContent(url, header);
            if (string.IsNullOrEmpty(content)) return Movie.NotFound;

            var json = JArray.Parse(content);
            if (json.Count == 0) return Movie.NotFound;

            foreach (var token in json)
            {
                int.TryParse(token["movie"]["year"].ToString(), out int year);
                if (torrent.Year != year)
                    return Movie.NotFound;

                var name = token["movie"]["title"].ToString();
                var trakt = int.Parse(token["movie"]["ids"]["tmdb"].ToString());
                var imdb = token["movie"]["ids"]["imdb"].ToString();
                var tmdb = int.Parse(token["movie"]["ids"]["tmdb"].ToString());
                return new Movie
                {
                    OriginalTitle = name,
                    Year = year,
                    TraktId = trakt,
                    ImdbId = imdb,
                    TmdbId = tmdb,
                    Torrents = new List<Torrent> { torrent }
                };
            }
            return Movie.NotFound;
        }
    }
}
