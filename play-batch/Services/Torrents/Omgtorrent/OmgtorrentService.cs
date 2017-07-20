using System.Collections.Generic;
using System.Threading.Tasks;
using batch.Models;
using batch.Services.Web;

namespace batch.Services.Torrents.Omgtorrent
{
    public class OmgtorrentService : ABaseTorrentsService
    {
        public OmgtorrentService(string url)
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

                var idx = 0;
                foreach (var tr in trs)
                {
                    if (idx < 2)
                    {
                        idx += 1;
                        continue;
                    }

                    var tds = _parser.GetTags(tr.Text, "td");
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

        protected override int GetNumberOfPages()
        {
            var content = WebService.Instance.GetContent(_url);
            var div = _parser.GetTagsByClass(content, "div", "pagination");
            if (!div.Success) return 0;

            var a = _parser.GetTags(div[0].Text, "a");
            if (!a.Success) return 0;

            var nb = a[a.Count - 2];
            return int.Parse(nb.GetText());
        }

        protected override string GetPageNumber(int nb)
        {
            var searchUrl = $"{_url}&page={nb}";
            var content = WebService.Instance.GetContent(searchUrl);
            return content;
        }
    }
}
