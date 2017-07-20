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
        public NextorrentService(string url)
        {
            _url = url;
        }

        public override List<Torrent> GetMovieTorrents()
        {
            var nbOfPages = GetNumberOfPages();
            var pages = GetPageNumbers(1, nbOfPages);
            var torrents = new List<Torrent>();
            Parallel.ForEach(pages, new ParallelOptions { MaxDegreeOfParallelism = 25 }, nb =>
            {
                var page = GetPageNumber(nb);
                var table = _parser.GetTag(page, "table");
                if (!table.Success) return;

                var trs = _parser.GetTags(table.Value, "tr");
                if (!trs.Success) return;

                foreach (var tr in trs)
                {
                    var tds = _parser.GetTags(tr.Value, "td");
                    if (tds.Count < 4) return;

                    lock (torrents)
                    {
                        torrents.Add(new Torrent
                        {
                            Name = GetName(tds[0].Text),
                            Link = GetLink(tds[0].Text),
                            Source = Source.Nextorrent,
                            Size = GetSize(tds[1].Text),
                            Seeders = GetNumber(tds[2].Text),
                            Leechers = GetNumber(tds[3].Text)
                        });
                    }
                }
            });
            return torrents;
        }

        protected override int GetNumberOfPages()
        {
            var page = 1;
            while (true)
            {
                var pageUrl = $"{_url}/{page}";
                var content = WebService.Instance.GetContent(pageUrl);
                var ul = _parser.GetTagsByClass(content, "ul", "pagination");
                if (!ul.Success) return 0;

                var lis = _parser.GetTags(ul.First().Value, "li");
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

        protected IEnumerable<int> GetPageNumbers(int start, int max)
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

        protected override string GetPageNumber(int nb)
        {
            var searchUrl = $"{_url}/{nb}";
            var content = WebService.Instance.GetContent(searchUrl);
            return content;
        }
    }
}
