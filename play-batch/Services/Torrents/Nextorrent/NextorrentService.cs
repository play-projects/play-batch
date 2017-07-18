using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using batch.Models;
using batch.Services.Web;

namespace batch.Services.Torrents.Nextorrent
{
    public class NextorrentService : ABaseTorrentsService
    {
        private readonly string _url;

        public NextorrentService(string url)
        {
            _url = url;
        }

        public override List<Torrent> GetMovieTorrents()
        {
            var nbOfPages = GetNumberOfPages(_url, 1);
            var pages = GetPageNumbers(1, nbOfPages);
            var torrents = new List<Torrent>();
            Parallel.ForEach(pages, new ParallelOptions { MaxDegreeOfParallelism = 25 }, nb =>
            {
                var page = GetPageNumber(_url, nb);
                var table = Parser.GetTag(page, "table");
                if (!table.Success) return;

                var trs = Parser.GetTags(table.Value, "tr");
                if (!trs.Success) return;

                foreach (var tr in trs)
                {
                    var tds = Parser.GetTags(tr.Value, "td");
                    if (tds.Count < 4) return;

                    var id = GetId(tds[0].Text);
                    var name = GetName(tds[0].Text);
                    if (id != 0 && !string.IsNullOrEmpty(name))
                    {
                        lock (torrents)
                        {
                            torrents.Add(new Torrent
                            {
                                Id = id,
                                Source = Source.Nextorrent,
                                Name = name,
                                Size = GetSize(tds[1].Text),
                                Seeders = GetNumber(tds[2].Text),
                                Leechers = GetNumber(tds[3].Text)
                            });
                        }
                    }
                }
            });
            return torrents;
        }

        private int GetId(string str)
        {
            var a = Parser.GetTag(str, "a");
            if (!a.Success) return 0;

            var href = a.Attributes.SingleOrDefault(attr => attr.Key == "href")?.Values.FirstOrDefault();
            if (href == null) return 0;

            var pattern = @"/torrent/\d+";
            var match = Regex.Match(href, pattern);
            if (!match.Success) return 0;

            var nb = Regex.Match(match.Value, @"\d+").Value;
            return int.Parse(nb);
        }

        private int GetNumberOfPages(string url, int page)
        {
            while (true)
            {
                var pageUrl = $"{url}/{page}";
                var content = WebService.Instance.GetContent(pageUrl);
                var ul = Parser.GetTagsByClass(content, "ul", "pagination");
                if (!ul.Success) return 0;

                var lis = Parser.GetTags(ul.First().Value, "li");
                if (!lis.Success) return 0;

                var last = lis.Last();
                var txt = last.GetText();
                if (txt.StartsWith("suivant", StringComparison.CurrentCultureIgnoreCase))
                {
                    var lastPage = lis[lis.Count - 2];
                    var nbs = Regex.Matches(lastPage.GetText(), @"\d+");
                    if (nbs.Count == 0) return 0;

                    var nb = int.Parse(nbs[nbs.Count - 1].Value);
                    if (nb % 50 != 0)
                        return int.Parse(nbs[0].Value);
                    page = nb + 1;
                }
            }
        }

        private IEnumerable<int> GetPageNumbers(int start, int max)
        {
            var result = new List<int>();
            for (var i = start; i <= max; i += 50)
            {
                var nb = i;
                if (i + 50 > max)
                    nb = max;
                result.Add(nb);
            }
            return result;
        }

        private string GetPageNumber(string url, int nb)
        {
            var searchUrl = $"{url}/{nb}";
            var content = WebService.Instance.GetContent(searchUrl);
            return content;
        }
    }
}
