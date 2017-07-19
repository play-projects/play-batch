using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using batch.Models;
using batch.Services.Web;

namespace batch.Services.Torrents.Omgtorrent
{
    public class OmgtorrentService : ABaseTorrentsService
    {
        private readonly string _url;

        public OmgtorrentService(string url)
        {
            _url = url;
        }

        public override List<Torrent> GetMovieTorrents()
        {
            var nbOfPages = GetNumberOfPages(_url);
            var torrents = new List<Torrent>();
            Parallel.For(1, nbOfPages + 1, new ParallelOptions { MaxDegreeOfParallelism = 25 }, nb =>
            {
                var page = GetPageNumber(_url, nb);
                var table = Parser.GetTag(page, "table");
                if (!table.Success) return;

                var trs = Parser.GetTags(table.Text, "tr");
                if (!trs.Success) return;

                var idx = 0;
                foreach (var tr in trs)
                {
                    if (idx < 2)
                    {
                        idx += 1;
                        continue;
                    }

                    var tds = Parser.GetTags(tr.Text, "td");
                    if (tds.Count < 6) return;

                    lock (torrents)
                    {
                        torrents.Add(new Torrent
                        {
                            Name = GetName(tds[1].Text),
                            Link = GetLink(tds[1].Text),
                            Source = Source.Omgtorrent,
                            Size = GetSize(tds[2].Text),
                            Seeders = GetNumber(tds[3].Text),
                            Leechers = GetNumber(tds[4].Text)
                        });
                    }
                }
            });
            return torrents;
        }

        protected override int GetNumberOfPages(string url)
        {
            var content = WebService.Instance.GetContent(url);
            var div = Parser.GetTagsByClass(content, "div", "pagination");
            if (!div.Success) return 0;

            var a = Parser.GetTags(div[0].Text, "a");
            if (!a.Success) return 0;

            var nb = a[a.Count - 2];
            return int.Parse(nb.GetText());
        }

        protected override string GetPageNumber(string url, int nb)
        {
            var searchUrl = $"{url}&page={nb}";
            var content = WebService.Instance.GetContent(searchUrl);
            return content;
        }
    }
}
