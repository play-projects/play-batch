using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using batch.Models;
using System.Threading.Tasks;
using System.Text;
using System.Linq;
using System.Globalization;

namespace batch.Services.T411
{
	public class MovieService : BaseService
	{
		public static MovieService Instance = new MovieService();
		private MovieService() { }

	    public List<Movie> GetMovies()
	    {
	        var french = GetFrenchMovies();
	        var vostfr = GetVostfrMovies();
	        return french.Concat(vostfr).ToList();
	    }

        private List<Movie> GetFrenchMovies()
        {
            var low = GetMovieBySearch(new Search(Criteria.MOVIE, Criteria.VF, Criteria.LOW, Criteria.TWOD));
            var medium = GetMovieBySearch(new Search(Criteria.MOVIE, Criteria.VF, Criteria.MEDIUM, Criteria.TWOD));
            var high = GetMovieBySearch(new Search(Criteria.MOVIE, Criteria.VF, Criteria.HIGH, Criteria.TWOD));
            var veryhigh = GetMovieBySearch(new Search(Criteria.MOVIE, Criteria.VF, Criteria.VERYHIGH, Criteria.TWOD));
            return low.Concat(medium).Concat(high).Concat(veryhigh).ToList();
        }

	    private List<Movie> GetVostfrMovies()
	    {
	        var low = GetMovieBySearch(new Search(Criteria.MOVIE, Criteria.VOSTFR, Criteria.LOW, Criteria.TWOD));
	        var medium = GetMovieBySearch(new Search(Criteria.MOVIE, Criteria.VOSTFR, Criteria.MEDIUM, Criteria.TWOD));
	        var high = GetMovieBySearch(new Search(Criteria.MOVIE, Criteria.VOSTFR, Criteria.HIGH, Criteria.TWOD));
	        var veryhigh = GetMovieBySearch(new Search(Criteria.MOVIE, Criteria.VOSTFR, Criteria.VERYHIGH, Criteria.TWOD));
	        return low.Concat(medium).Concat(high).Concat(veryhigh).ToList();
	    }

        private List<Movie> GetMovieBySearch(Search search)
	    {
	        var url = SearchService.GetSearchUri(search);
	        var searchs = GetSearch(url);
            var movies = new List<Movie>();
	        Parallel.ForEach(searchs, s =>
	        {
	            var movie = GetMovieByName(s.Key, s.Value, search);
	            if (movie != Movie.NotFound)
	            {
	                lock (movies)
	                {
	                    movies.Add(movie);
	                    Console.WriteLine($"movie: {movie.Slug} - {movie.Year} - {movie.Quality}");
	                }
	            }
            });
	        return movies;
	    }

	    private Movie GetMovieByName(int id, string name, Search search)
        {
            var pattern = @"((?:(?!\d{4}).)+)(\d{4})[^p]";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            var match = regex.Match(name);

            if (!match.Success)
                return Movie.NotFound;
			return new Movie
			{
				Id = id,
				Name = name,
                Slug = GetTorrentSlug(match.Groups[1].Value),
                Year = GetTorrentYear(match.Groups[2].Value),
				Language = GetTorrentLanguage(search),
                Category = Category.Movie,
				Quality = GetTorrentQuality(search)
			};
        }

	    private string GetTorrentSlug(string name)
	    {
            name = name.ToLower().Normalize(NormalizationForm.FormD);
            var chars = name.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            name = new string(chars).Normalize(NormalizationForm.FormC);

	        var slug = Regex.Replace(name, @"\.", " ");
            slug = Regex.Replace(slug, @"[^a-zA-Z0-9' ]", string.Empty);
	        return slug.Trim();
	    }

	    private int GetTorrentYear(string year)
	    {
	        return int.Parse(year);
        }

        private Language GetTorrentLanguage(Search search)
        {
            if (search.Language == Criteria.VF)
                return Language.VF;
            if (search.Language == Criteria.VOSTFR)
                return Language.VOSTFR;
            return Language.None;
        }

		private Quality GetTorrentQuality(Search search)
		{
            if (search.Quality == Criteria.LOW)
                return Quality.Low;
		    if (search.Quality == Criteria.MEDIUM)
                return Quality.Medium;
            if (search.Quality == Criteria.HIGH)
                return Quality.High;
		    if (search.Quality == Criteria.VERYHIGH)
                return Quality.VeryHigh;
            return Quality.None;
		}
	}
}
