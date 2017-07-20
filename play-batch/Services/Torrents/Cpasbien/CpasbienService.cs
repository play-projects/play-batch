using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using batch.Models;
using batch.Services.Web;

namespace batch.Services.Torrents.Cpasbien
{
    public class CpasbienService : ABaseTorrentsService
    {
        public CpasbienService(string url)
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
                var gauche = _parser.GetTagById(page, "div", "gauche");
                if (!gauche.Success) return;

                var ligne0 = _parser.GetTagsByClass(gauche.Text, "div", "ligne0");
                var ligne1 = _parser.GetTagsByClass(gauche.Text, "div", "ligne1");
                var lignes = ligne0.Concat(ligne1);
                foreach (var ligne in lignes)
                {
                    var poid = _parser.GetTagsByClass(ligne.Text, "div", "poid");
                    var seed = _parser.GetTagsByClass(ligne.Text, "span", "seed_ok");
                    var down = _parser.GetTagsByClass(ligne.Text, "div", "down");

                    if (!poid.Success || !seed.Success || !down.Success)
                        continue;

                    lock (torrents)
                    {
                        var name = GetName(ligne.Text);
                        Console.WriteLine($"{Source.Cpasbien} - {name}");
                        torrents.Add(new Torrent
                        {
                            Name = name,
                            Link = GetLink(ligne.Text),
                            Source = Source.Cpasbien,
                            Size = GetSize(poid[0].Text),
                            Seeders = GetNumber(seed[0].Text),
                            Leechers = GetNumber(down[0].Text)
                        });
                    }
                }
            });
            return torrents;
        }

        protected override int GetNumberOfPages()
        {
            var page = GetPageNumber(1);
            var div = _parser.GetTagById(page, "div", "pagination");
            if (!div.Success) return 0;

            var a = _parser.GetTags(div.Text, "a");
            if (!a.Success) return 0;

            var nb = a[a.Count - 2].GetText();
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
