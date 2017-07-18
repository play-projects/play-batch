using batch.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using batch.Services.Torrents.Nextorrent;
using play.Services.Torrents.Torrent9;

namespace batch.Services.Torrents
{
    public class MovieTorrentsService
    {
        private readonly NextorrentService _nextorrent;
        private readonly Torrent9Service _torrent9;

	    public MovieTorrentsService(string next, string torrent9)
	    {
	        _nextorrent = new NextorrentService(next);
            _torrent9 = new Torrent9Service(torrent9);
	    }

	    public IEnumerable<Torrent> GetMovies()
	    {
	        var next = _nextorrent.GetMovieTorrents();
	        var torrent9 = _torrent9.GetMovieTorrents();

	        var result = next.Concat(torrent9).ToList();
	        return GetExtractTorrents(result);
	    }

        private IEnumerable<Torrent> GetExtractTorrents(IEnumerable<Torrent> torrents)
	    {
            var movies = new List<Torrent>();
	        Parallel.ForEach(torrents, t =>
	        {
	            const string name = "(.+)";
	            const string language = "(french|truefrench|vff|vostfr)";
	            const string quality = "(dvdrip|hdrip|webrip|720p|1080p|4k)";
	            const string year = @"(\d{4})";

	            var pattern = string.Empty;
                if (t.Source == Source.Nextorrent || t.Source == Source.Torrent9)
	                pattern = $@"{name}.+{language}.+{quality}.+{year}";
	            if (t.Source == Source.Yggtorrent)
	                pattern = $@"{name}.+{year}.+{language}.+{quality}";

                var regex = new Regex(pattern, RegexOptions.IgnoreCase);
	            var match = regex.Match(t.Name);
	            if (!match.Success) return;

	            string s = string.Empty, l = string.Empty, q = string.Empty, y = string.Empty;
	            if (t.Source == Source.Nextorrent || t.Source == Source.Torrent9)
	            {
	                s = match.Groups[1].Value;
	                l = match.Groups[2].Value;
	                q = match.Groups[3].Value;
	                y = match.Groups[4].Value;
	            }
	            if (t.Source == Source.Yggtorrent)
	            {
	                s = match.Groups[1].Value;
	                y = match.Groups[2].Value;
	                l = match.Groups[3].Value;
	                q = match.Groups[4].Value;
                }

	            t.Slug = GetSlug(s);
	            t.Language = GetLanguage(l);
	            t.Quality = GetQuality(q);
	            t.Year = GetYear(y);
                t.Category = Category.Movie;
	            t.Guid = Guid.NewGuid().ToString();

	            lock (movies)
	            {
	                movies.Add(t);
	                Console.WriteLine($"torrent: {t.Slug} - {t.Year} - {t.Quality}");
	            }
	        });
	        return movies.GroupBy(m => m.Slug).Select(m => m.First());
	    }

	    private string GetSlug(string name)
	    {
            name = name.ToLower().Normalize(NormalizationForm.FormD);
            var chars = name.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            name = new string(chars).Normalize(NormalizationForm.FormC);

	        var slug = Regex.Replace(name, @"\.", " ");
            slug = Regex.Replace(slug, @"[^a-zA-Z0-9' ]", string.Empty);
            slug = slug.Trim();
            slug = Regex.Replace(slug, @"\s+", "-");
	        return slug;
	    }

	    private int GetYear(string year)
	    {
	        return int.Parse(year);
        }

        private Language GetLanguage(string language)
        {
            if (language.Equals("french", StringComparison.CurrentCultureIgnoreCase))
                return Language.VF;
            if (language.Equals("truefrench", StringComparison.CurrentCultureIgnoreCase))
                return Language.VF;
            if (language.Equals("vff", StringComparison.CurrentCultureIgnoreCase))
                return Language.VF;
            return Language.VOSTFR;
        }

        private Quality GetQuality(string quality)
		{
            if (quality.Equals("dvdrip", StringComparison.CurrentCultureIgnoreCase))
                return Quality.Low;
		    if (quality.Equals("webrip", StringComparison.CurrentCultureIgnoreCase))
                return Quality.Low;
		    if (quality.Equals("hdrip", StringComparison.CurrentCultureIgnoreCase))
		        return Quality.Medium;
            if (quality.Equals("720p", StringComparison.CurrentCultureIgnoreCase))
                return Quality.Medium;
		    if (quality.Equals("1080p", StringComparison.CurrentCultureIgnoreCase))
                return Quality.High;
            return Quality.VeryHigh;
		}
	}
}
