using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using batch.Models;
using batch.Services.Web;

namespace batch.Services.Torrents.Yggtorrent
{
    public class YggtorrentService : ABaseTorrentsService
    {
        public YggtorrentService(string url)
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
                var table = _parser.GetTag(page, "table");
                if (!table.Success) return;

                var trs = _parser.GetTags(table.Text, "tr");
                foreach (var tr in trs)
                {
                    var tds = _parser.GetTags(tr.Text, "td");
                    if (tds.Count < 5) continue;

                    lock (torrents)
                    {
                        torrents.Add(new Torrent
                        {
                            Name = GetName(tds[0].Text),
                            Link = GetLink(tds[0].Text),
                            Source = Source.Yggtorrent,
                            Size = GetSize(tds[2].Text),
                            Seeders = GetNumber(tds[3].Text),
                            Leechers = GetNumber(tds[4].Text)
                        });
                    }
                }
            });
            return torrents;
        }

        protected override int GetNumberOfPages()
        {
            var content = GetPageNumber(1);
            var ul = _parser.GetTagsByClass(content, "ul", "pagination");
            if (!ul.Success) return 0;

            var lis = _parser.GetTags(ul[0].Text, "li");
            if (!lis.Success) return 0;

            var a = _parser.GetTag(lis.Last().Text, "a");
            if (!a.Success) return 0;

            var data = a.Attributes
                .SingleOrDefault(attr => attr.Key == "data-ci-pagination-page")?
                .Values.FirstOrDefault();
            if (data == null) return 0;

            return int.Parse(data);
        }

        protected override string GetPageNumber(int nb)
        {
            var page = nb * 100 - 100;
            var searchUrl = $"{_url}&page={page}";
            var content = WebService.Instance.GetContent(searchUrl);
            return content;
        }
    }
}
