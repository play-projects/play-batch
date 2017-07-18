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

        private readonly Dictionary<string, Language> _languages = new Dictionary<string, Language>
        {
            { "french", Language.VF },
            { "truefrench", Language.VF },
            { "vff", Language.VF },
            { "vostfr", Language.VOSTFR }
        };
        private readonly Dictionary<string, Quality> _qualities = new Dictionary<string, Quality>
        {
            { "dvdrip", Quality.Low },
            { "webrip", Quality.Low },
            { "web-dl", Quality.Low },
            { "hdrip", Quality.Medium },
            { "hdlight", Quality.Medium },
            { "720p", Quality.Medium },
            { "1080p", Quality.High },
            { "4k", Quality.VeryHigh }
        };

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
	            var match = IsMatch(t.Name);
	            if (string.IsNullOrEmpty(match.name) && string.IsNullOrEmpty(match.lang)
	                && string.IsNullOrEmpty(match.quality) && string.IsNullOrEmpty(match.year))
	                return;
                    
	            t.Slug = GetSlug(match.name);
	            t.Language = GetLanguage(match.lang);
	            t.Quality = GetQuality(match.quality);
	            t.Year = GetYear(match.year);
                t.Category = Category.Movie;
	            t.Guid = Guid.NewGuid().ToString();

	            lock (movies)
	            {
	                movies.Add(t);
	                Console.WriteLine($"torrent: {t.Slug} - {t.Year} - {t.Quality}");
	            }
	        });
	        return movies;
	    }

        private (string name, string lang, string quality, string year) IsMatch(string torrentName)
        {
            var name = "(?<name>.+)";
            var language = $"(?<lang>{string.Join("|", _languages.Keys.ToArray())})";
            var quality = $"(?<quality>{string.Join("|", _qualities.Keys.ToArray())})";
            var year = @"(?<year>\d{4})";  

            var patterns = new[]
            {
                $@"{name}.+{language}.+{quality}.+{year}",
                $@"{name}.+{language}.+{year}.+{quality}",

                $@"{name}.+{quality}.+{language}.+{year}",
                $@"{name}.+{quality}.+{year}.+{language}",

                $@"{name}.+{year}.+{language}.+{quality}",
                $@"{name}.+{year}.+{quality}.+{language}"
            };

            foreach (var pattern in patterns)
            {
                var regex = new Regex(pattern, RegexOptions.IgnoreCase);
                var match = regex.Match(torrentName);
                if (match.Success)
                {
                    return (
                        match.Groups["name"].Value,
                        match.Groups["lang"].Value,
                        match.Groups["quality"].Value,
                        match.Groups["year"].Value
                    );
                }
            }
            return (string.Empty, string.Empty, string.Empty, string.Empty);
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
            if (_languages.ContainsKey(language))
                return _languages[language];
            return Language.None;
        }

        private Quality GetQuality(string quality)
        {
            if (_qualities.ContainsKey(quality))
                return _qualities[quality];
            return Quality.None;
        }
	}
}
