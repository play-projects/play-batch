using batch.Models;
using batch.Services.Web;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace batch.Services.Tmdb
{
    public class MovieExtractorsService
    {
        public static MovieExtractorsService Instance = new MovieExtractorsService();
        private MovieExtractorsService() { }

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

        public Movie GetMovie(Movie movie)
        {
            var apiKey = "fa25d553584aac70e9db4d47d3636ae9";
            var language = "fr-FR";
            var url = $"https://api.themoviedb.org/3/movie/{movie.TmdbId}?api_key={apiKey}&language={language}";
            var content = WebService.Instance.GetContent(url);
            if (string.IsNullOrEmpty(content)) return Movie.NotFound;

            var json = JObject.Parse(content);
            movie.Title = json["title"].ToString();
            movie.Genres = json["genres"].Select(g => new Genre
            {
                Id = int.Parse(g["id"].ToString()),
                Name = g["name"].ToString()
            }).ToList();
            movie.OriginalLanguage = json["original_language"].ToString();
            movie.Overview = json["overview"].ToString();
            movie.Tagline = json["tagline"].ToString();
            movie.Popularity = double.Parse(json["popularity"].ToString());
            movie.ReleaseDate = DateTime.Parse(json["release_date"].ToString());
            movie.VoteAverage = float.Parse(json["vote_average"].ToString());
            movie.VoteCount = int.Parse(json["vote_count"].ToString());
            movie.Runtime = int.Parse(json["runtime"].ToString());
            movie.BackdropPath = json["backdrop_path"].ToString();
            movie.PosterPath = json["poster_path"].ToString();
            if (json["belongs_to_collection"].HasValues)
            {
                movie.Collection = new Collection
                {
                    Id = int.Parse(json["belongs_to_collection"]["id"].ToString()),
                    Name = json["belongs_to_collection"]["name"].ToString(),
                    PosterPath = json["belongs_to_collection"]["poster_path"].ToString(),
                    BackdropPath = json["belongs_to_collection"]["backdrop_path"].ToString()
                };
            }
            return movie;
        }
    }
}
