using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using platch.Models;
using System.Threading.Tasks;
using System.Text;
using System.Linq;
using System.Globalization;

namespace platch.Services.T411
{
	public class MovieService : BaseService
	{
		public static MovieService Instance = new MovieService();
		private MovieService() { }

		private readonly string moviesUrl = "/torrents/search/?name=&description=&file=&user=&cat=210&subcat=631" +
            "&term%5B17%5D%5B%5D=541&term%5B17%5D%5B%5D=721&term%5B9%5D%5B%5D=22&search=&submit=Recherche"; 

        public List<Movie> GetMovies()
        {
            var searchs = GetSearch(moviesUrl);
			var movies = new List<Movie>();
            Parallel.ForEach(searchs, search =>
            {
                var movie = GetMovieByName(search.Key, search.Value);
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

        private Movie GetMovieByName(int id, string name)
        {
            var pattern = @"((?:(?!\d{4}).)+)(\d{4})[^p]\D+(\d{3,4}p|rip|4k)";
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
				Language = GetTorrentLanguage(name),
                Category = Category.Movie,
				Quality = GetTorrentQuality(match.Groups[3].Value)
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

        private Language GetTorrentLanguage(string name)
        {
            if (Regex.IsMatch(name, "vf|vff|french", RegexOptions.IgnoreCase))
                return Language.VF;
            if (Regex.IsMatch(name, "vostfr", RegexOptions.IgnoreCase))
                return Language.VOSTFR;
            return Language.None;
        }

		private Quality GetTorrentQuality(string quality)
		{
            if (Regex.IsMatch(quality, "720"))
                return Quality.Medium;
            if (Regex.IsMatch(quality, "1080"))
                return Quality.High;
            if (Regex.IsMatch(quality, "4k", RegexOptions.IgnoreCase))
                return Quality.VeryHigh;
		    if (Regex.IsMatch(quality, "rip", RegexOptions.IgnoreCase))
		        return Quality.Low;
            return Quality.None;
		}
	}
}
