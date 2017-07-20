using batch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using batch.Services.Torrents.Cpasbien;
using batch.Services.Torrents.Lientorrent;
using batch.Services.Torrents.Nextorrent;
using batch.Services.Torrents.Omgtorrent;
using batch.Services.Torrents._1337x;
using play.Services.Torrents.Torrent9;

namespace batch.Services.Torrents
{
    public class MovieTorrentsService
    {
        private readonly NextorrentService _nextorrent;
        private readonly Torrent9Service _torrent9;
        private readonly OmgtorrentService _omgtorrent;
        private readonly LientorrentService _lientorrent;
        private readonly CpasbienService _cpasbien;
        private readonly LeetxService _leetx;

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
            { "bdrip", Quality.Low },
            { "hdrip", Quality.Medium },
            { "hdlight", Quality.Medium },
            { "720p", Quality.Medium },
            { "1080p", Quality.High },
            { "4k", Quality.VeryHigh }
        };

	    public MovieTorrentsService(string next, string torrent9, string omg, string lien, string cpasbien, string leetx)
	    {
	        _nextorrent = new NextorrentService(next);
            _torrent9 = new Torrent9Service(torrent9);
            _omgtorrent = new OmgtorrentService(omg);
	        _lientorrent = new LientorrentService(lien);
	        _cpasbien = new CpasbienService(cpasbien);
            _leetx = new LeetxService(leetx);
	    }

	    public IEnumerable<Torrent> GetMovies()
	    {
	        /*var next = _nextorrent.GetMovieTorrents();
	        var torrent9 = _torrent9.GetMovieTorrents();
	        var omg = _omgtorrent.GetMovieTorrents();
	        var lien = _lientorrent.GetMovieTorrents();
	        var cpasbien = _cpasbien.GetMovieTorrents();*/
	        var leetx = _leetx.GetMovieTorrents();

	        var result = /*next.Concat(torrent9)
                .Concat(omg)
                .Concat(lien)
                .Concat(cpasbien)
                .Concat(*/leetx//)
                .ToList();
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
                $@"{name}\s+{language}.+{quality}.+{year}",
                $@"{name}\s+{language}.+{year}.+{quality}",

                $@"{name}\s+{quality}.+{language}.+{year}",
                $@"{name}\s+{quality}.+{year}.+{language}",

                $@"{name}\s+{year}.+{language}.+{quality}",
                $@"{name}\s+{year}.+{quality}.+{language}"
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
	        var slug = Regex.Replace(name, @"\.", " ");
            slug = Regex.Replace(slug, @"[^\w\s\d'-]+", string.Empty);
            slug = slug.Trim();
            slug = Regex.Replace(slug, @"\s+", "-");
	        slug = Regex.Replace(slug, "-{2,}", "-");
            return slug.ToLower();
	    }

	    private int GetYear(string year)
	    {
	        return int.Parse(year);
        }

        private Language GetLanguage(string language)
        {
            language = language.ToLower();
            if (_languages.ContainsKey(language))
                return _languages[language];
            return Language.None;
        }

        private Quality GetQuality(string quality)
        {
            quality = quality.ToLower();
            if (_qualities.ContainsKey(quality))
                return _qualities[quality];
            return Quality.None;
        }
	}
}
