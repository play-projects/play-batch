using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using batch.Models;
using batch.Services.Torrents;
using batch.Services.Web;

namespace play.Services.Torrents.Torrent9
{
    public class Torrent9Service : ABaseTorrentsService
    {
        public Torrent9Service(string url)
        {
            _url = url;
        }

        public override List<Torrent> GetMovieTorrents()
        {
            var nbOfPages = GetNumberOfPages();
            var torrents = new List<Torrent>();
            Parallel.For(0, nbOfPages + 1, new ParallelOptions { MaxDegreeOfParallelism = 25 }, nb =>
            {
                var page = GetPageNumber(nb);
                var table = _parser.GetTag(page, "table");
                if (!table.Success) return;

                var tbody = _parser.GetTag(table.Value, "tbody");
                if (!tbody.Success) return;

                var trs = _parser.GetTags(tbody.Value, "tr");
                foreach (var tr in trs)
                {
                    var tds = _parser.GetTags(tr.Value, "td");
                    if (tds.Count < 4) continue;

                    lock (torrents)
                    {
                        var name = GetName(tds[0].Text);
                        Console.WriteLine($"{Source.Torrent9} - {name}");
                        torrents.Add(new Torrent
                        {
                            Name = GetName(tds[0].Text),
                            Link = GetLink(tds[0].Text),
                            Source = Source.Torrent9,
                            Size = GetSize(tds[1].Text),
                            Seeders = GetNumber(tds[2].Text),
                            Leechers = GetNumber(tds[3].Text)
                        });
                    }
                }
            });
            return torrents;
        }

        protected override string GetName(string str)
        {
            var a = _parser.GetTag(str, "a");
            if (!a.Success) return string.Empty;

            var title = a.Attributes.SingleOrDefault(attr => attr.Key == "title")?.Values.FirstOrDefault();
            if (title == null) return string.Empty;

            title = title.Replace("Télécharger", string.Empty);
            title = title.Replace("en Torrent", string.Empty);
            return title.Trim();
        }

        protected override int GetNumberOfPages()
        {
            var content = GetPageNumber(0);
            var ul = _parser.GetTagsByClass(content, "ul", "pagination");
            if (!ul.Success) return 0;

            var lis = _parser.GetTags(ul[0].Text, "li");
            if (!lis.Success) return 0;

            var li = lis[lis.Count - 2];
            var a = _parser.GetTag(li.Text, "a");
            if (!a.Success) return 0;

            var nb = int.Parse(a.GetText());
            return nb - 1;
        }

        protected override string GetPageNumber(int nb)
        {
            var searchUrl = $"{_url},page-{nb}";
            var content = WebService.Instance.GetContent(searchUrl);
            return content;
        }
    }
}
