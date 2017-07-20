using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using batch.Models;
using batch.Services.Web;

namespace batch.Services.Torrents.Lientorrent
{
    public class LientorrentService : ABaseTorrentsService
    {
        public LientorrentService(string url)
        {
            _url = url;
        }

        public override List<Torrent> GetMovieTorrents()
        {
            var nbOfPages = GetNumberOfPages();
            var torrents = new List<Torrent>();
            Parallel.For(1, nbOfPages + 1, new ParallelOptions { MaxDegreeOfParallelism = 25 }, nb =>
            {
                var page = GetPageNumber(nb);
                var vignettes = _parser.GetTagById(page, "div", "contenuvignettes");
                if (!vignettes.Success) return;

                var divs = _parser.GetTagsByClass(vignettes.Text, "div", "blocvignette");
                foreach (var div in divs)
                {
                    var title = _parser.GetTagsByClass(div.Text, "div", "titrefiche");
                    if (!title.Success) return;

                    var a = _parser.GetTag(title[0].Text, "a");
                    if (!a.Success) return;

                    var over = a.Attributes.SingleOrDefault(attr => attr.Key == "onmouseover")?.Values.FirstOrDefault();
                    if (over == null) return;

                    var txt = WebUtility.HtmlDecode(over);
                    var spans = _parser.GetTags(txt, "span");
                    if (!spans.Success || spans.Count < 3) return;

                    lock (torrents)
                    {
                        torrents.Add(new Torrent
                        {
                            Name = GetName(txt),
                            Link = GetLink(title[0].Text),
                            Source = Source.Lientorrent,
                            Size = 0,
                            Seeders = GetNumber(spans[0].Text),
                            Leechers = GetNumber(spans[1].Text),
                            Completed = GetNumber(spans[2].Text)
                        });
                    }
                }
            });
            return torrents;
        }

        protected override string GetName(string str)
        {
            var start = str.IndexOf("TITLE");
            var end = str.IndexOf("BGCOLOR");

            if (start <= 0 || end <= 0)
                return string.Empty;

            var sub = str.Substring(start, end - start);
            var first = sub.IndexOf("'");
            var last = sub.LastIndexOf(",");

            if (first <= 0 || last <= 0)
                return string.Empty;

            return sub.Substring(first + 1, last - first - 2);
        }

        protected override int GetNumberOfPages()
        {
            var content = GetPageNumber(1);
            var div = _parser.GetTagsByClass(content, "div", "pagination");
            if (!div.Success) return 0;

            var a = _parser.GetTags(div[0].Text, "a");
            if (!a.Success) return 0;

            var href = a.Last().Attributes.SingleOrDefault(attr => attr.Key == "href")?.Values.FirstOrDefault();
            if (href == null) return 0;

            var pattern = @"page-\d+";
            var match = Regex.Match(href, pattern);
            if (!match.Success) return 0;

            var nb = Regex.Match(match.Value, @"\d+").Value;
            return int.Parse(nb);
        }

        protected override string GetPageNumber(int nb)
        {
            var searchUrl = $"{_url}/page-{nb}.html";
            var content = WebService.Instance.GetContent(searchUrl);
            return content;
        }
    }
}
