using batch.Models;
using batch.Services.Web;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace batch.Services.Tmdb
{
    public class MovieExtractorsService
    {
        public static MovieExtractorsService Instance = new MovieExtractorsService();
        private MovieExtractorsService() { }

        public IEnumerable<Movie> GetMoviesIds(IEnumerable<Torrent> torrents)
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
                    Console.WriteLine($"id: {movie.OriginalTitle} - {movie.TmdbId}");
                }
            });

            return movies.GroupBy(m => m.ImdbId).Select(m => new Movie
            {
                OriginalTitle = m.First().OriginalTitle,
                Year = m.First().Year,
                TraktId = m.First().TraktId,
                ImdbId = m.First().ImdbId,
                TmdbId = m.First().TmdbId,
                Torrents = m.SelectMany(t => t.Torrents).OrderBy(t => t.Quality).ThenBy(t => t.Size).ToList()
            }).OrderBy(m => m.OriginalTitle).ToList();
        }

        private Movie GetMovieIds(Torrent torrent)
        {
			var baseUrl = ConfigurationService.GetValue("trakt_url");
			var url = $"{baseUrl}/search/movie?query={torrent.Slug.Replace("-", " ")}";
            var header = new Dictionary<string, string>
            {
				{ "trakt-api-version", ConfigurationService.GetValue("trakt_version") },
				{ "trakt-api-key", ConfigurationService.GetValue("trakt_key") }
            };

            var content = WebService.Instance.GetContent(url, header);
            if (string.IsNullOrEmpty(content)) return Movie.NotFound;

            var json = JArray.Parse(content);
            if (json.Count == 0) return Movie.NotFound;

            foreach (var token in json)
            {
                int.TryParse(token["movie"]["year"].ToString(), out int year);
				if (torrent.Year != year) continue;

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

        public IEnumerable<Movie> GetMovies(IEnumerable<Movie> movies)
        {
            var result = new List<Movie>();
            Parallel.ForEach(movies, new ParallelOptions { MaxDegreeOfParallelism = 50 }, movie =>
            {
                var m = GetMovie(movie);
                if (m != Movie.NotFound)
                {
                    lock (result)
                    {
                        result.Add(m);
                        Console.WriteLine($"movie: {m.Title} - {m.Popularity}");
                    }
                }
            });
            return result;
        }

        private Movie GetMovie(Movie movie)
        {
			var baseUrl = ConfigurationService.GetValue("tmdb_url");
			var key = ConfigurationService.GetValue("tmdb_key");
			var lang = ConfigurationService.GetValue("tmdb_lang");

            var url = $"{baseUrl}/movie/{movie.TmdbId}?api_key={key}&language={lang}";
            var content = WebService.Instance.GetContentTmdb(url);
            if (string.IsNullOrEmpty(content)) return Movie.NotFound;

            var json = JObject.Parse(content);
            movie.Title = json["title"].ToString();
            movie.TitleSearch = GetSearchTitle(json["title"].ToString());
            movie.Genres = json["genres"].Select(g => new Genre
            {
                Id = GetProperty<int>(g["id"].ToString()),
                Name = g["name"].ToString()
            }).ToList();
            movie.OriginalLanguage = json["original_language"].ToString();
            movie.Overview = json["overview"].ToString();
            movie.Tagline = json["tagline"].ToString();
            movie.Popularity = GetProperty<double>(json["popularity"].ToString());
            movie.ReleaseDate = GetProperty<DateTime>(json["release_date"].ToString());
            movie.VoteAverage = GetProperty<float>(json["vote_average"].ToString());
            movie.VoteCount = GetProperty<int>(json["vote_count"].ToString());
            movie.Runtime = GetProperty<int>(json["runtime"].ToString());
            movie.BackdropPath = json["backdrop_path"].ToString();
            movie.PosterPath = json["poster_path"].ToString();
            if (json["belongs_to_collection"].HasValues)
            {
                movie.Collection = new Collection
                {
                    Id = GetProperty<int>(json["belongs_to_collection"]["id"].ToString()),
                    Name = json["belongs_to_collection"]["name"].ToString(),
                    PosterPath = json["belongs_to_collection"]["poster_path"].ToString(),
                    BackdropPath = json["belongs_to_collection"]["backdrop_path"].ToString()
                };
            }
            return movie;
        }

        private string GetSearchTitle(string str)
        {
            var text = str.ToLower().Normalize(NormalizationForm.FormD);
            var chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            return new string(chars).Normalize(NormalizationForm.FormC);
        }

        private T GetProperty<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
                return default(T);
            var property = (T)Convert.ChangeType(json, typeof(T));
            return property;
        }
    }
}
