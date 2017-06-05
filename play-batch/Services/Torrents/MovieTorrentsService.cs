using batch.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace batch.Services.Torrents
{
    public class MovieTorrentsService : BaseTorrentsService
	{
		public static MovieTorrentsService Instance = new MovieTorrentsService();
		private MovieTorrentsService() { }

	    public List<Torrent> GetMovies()
	    {
	        var french = GetFrenchMovies();
	        var vostfr = GetVostfrMovies();
	        return french.Concat(vostfr).ToList();
	    }

        private IEnumerable<Torrent> GetFrenchMovies()
        {
            var low = GetMovieBySearch(new Search(Criteria.MOVIE, Criteria.VF, Criteria.LOW, Criteria.TWOD));
            var medium = GetMovieBySearch(new Search(Criteria.MOVIE, Criteria.VF, Criteria.MEDIUM, Criteria.TWOD));
            var high = GetMovieBySearch(new Search(Criteria.MOVIE, Criteria.VF, Criteria.HIGH, Criteria.TWOD));
            var veryhigh = GetMovieBySearch(new Search(Criteria.MOVIE, Criteria.VF, Criteria.VERYHIGH, Criteria.TWOD));
            return low.Concat(medium).Concat(high).Concat(veryhigh).ToList();
        }

	    private IEnumerable<Torrent> GetVostfrMovies()
	    {
	        var low = GetMovieBySearch(new Search(Criteria.MOVIE, Criteria.VOSTFR, Criteria.LOW, Criteria.TWOD));
	        var medium = GetMovieBySearch(new Search(Criteria.MOVIE, Criteria.VOSTFR, Criteria.MEDIUM, Criteria.TWOD));
	        var high = GetMovieBySearch(new Search(Criteria.MOVIE, Criteria.VOSTFR, Criteria.HIGH, Criteria.TWOD));
	        var veryhigh = GetMovieBySearch(new Search(Criteria.MOVIE, Criteria.VOSTFR, Criteria.VERYHIGH, Criteria.TWOD));
	        return low.Concat(medium).Concat(high).Concat(veryhigh).ToList();
	    }

        private IEnumerable<Torrent> GetMovieBySearch(Search search)
	    {
	        var url = SearchTorrentsService.GetSearchUri(search);
	        var searchs = GetSearch(url);
            var movies = new List<Torrent>();
	        Parallel.ForEach(searchs, s =>
	        {
	            var torrent = GetMovieByName(s, search);
	            if (torrent == Torrent.NotFound) return;
	            lock (movies)
	            {
	                movies.Add(torrent);
	                Console.WriteLine($"torrent: {torrent.Slug} - {torrent.Year} - {torrent.Quality}");
	            }
	        });
	        return movies.GroupBy(m => m.Slug).Select(m => m.First());
	    }

	    private Torrent GetMovieByName(Torrent torrent, Search search)
        {
            var pattern = @"(.+)\D(\d{4})[^p]";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            var match = regex.Match(torrent.Name);

            if (!match.Success)
                return Torrent.NotFound;
            torrent.Slug = GetTorrentSlug(match.Groups[1].Value);
            torrent.Year = GetTorrentYear(match.Groups[2].Value);
            torrent.Language = GetTorrentLanguage(search);
            torrent.Category = Category.Movie;
            torrent.Quality = GetTorrentQuality(search);
            return torrent;
        }

	    private string GetTorrentSlug(string name)
	    {
            name = name.ToLower().Normalize(NormalizationForm.FormD);
            var chars = name.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            name = new string(chars).Normalize(NormalizationForm.FormC);

	        var slug = Regex.Replace(name, @"\.", " ");
            slug = Regex.Replace(slug, @"[^a-zA-Z0-9' ]", string.Empty);
            slug = Regex.Replace(slug, @"\s+", "-");
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
