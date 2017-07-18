using System.Collections.Generic;
using System.Threading.Tasks;
using batch.Models;
using batch.Services.Torrents;
using batch.Services.Web;

namespace play.Services.Torrents.Torrent9
{
    public class Torrent9Service : ABaseTorrentsService
    {
        private readonly string _url;

        public Torrent9Service(string url)
        {
            _url = url;
        }

        public override List<Torrent> GetMovieTorrents()
        {
            var nbOfPages = GetNumberOfPages(_url);
            var torrents = new List<Torrent>();
            Parallel.For(0, nbOfPages + 1, new ParallelOptions { MaxDegreeOfParallelism = 25 }, nb =>
            {
                var page = GetPageNumber(_url, nb);
                var table = Parser.GetTag(page, "table");
                if (!table.Success) return;

                var tbody = Parser.GetTag(table.Value, "tbody");
                if (!tbody.Success) return;

                var trs = Parser.GetTags(tbody.Value, "tr");
                if (!trs.Success) return;

                foreach (var tr in trs)
                {
                    var tds = Parser.GetTags(tr.Value, "td");
                    if (tds.Count < 4) return;

                    var name = GetName(tds[0].Text);
                    if (!string.IsNullOrEmpty(name))
                    {
                        lock (torrents)
                        {
                            torrents.Add(new Torrent
                            {
                                Id = 0,
                                Source = Source.Torrent9,
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

        private int GetNumberOfPages(string url)
        {
            var content = GetPageNumber(url, 0);
            var ul = Parser.GetTagsByClass(content, "ul", "pagination");
            if (!ul.Success) return 0;

            var lis = Parser.GetTags(ul[0].Text, "li");
            if (!lis.Success) return 0;

            var li = lis[lis.Count - 2];
            var a = Parser.GetTag(li.Text, "a");
            if (!a.Success) return 0;

            var nb = int.Parse(a.GetText());
            return nb - 1;
        }

        private string GetPageNumber(string url, int nb)
        {
            var searchUrl = $"{url},page-{nb}";
            var content = WebService.Instance.GetContent(searchUrl);
            return content;
        }
    }
}
