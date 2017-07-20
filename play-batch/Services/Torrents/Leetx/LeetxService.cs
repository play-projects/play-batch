using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using batch.Models;
using batch.Services.Web;

namespace batch.Services.Torrents._1337x
{
    public class LeetxService : ABaseTorrentsService
    {
        public LeetxService(string url)
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
                var tbody = _parser.GetTag(page, "tbody");
                if (!tbody.Success) return;

                var trs = _parser.GetTags(tbody.Text, "tr");
                foreach (var tr in trs)
                {
                    var tds = _parser.GetTags(tr.Text, "td");
                    if (tds.Count < 6) return;

                    lock (torrents)
                    {
                        torrents.Add(new Torrent
                        {
                            Name = GetName(tds[0].Text),
                            Link = GetLink(tds[0].Text),
                            Source = Source.Leetx,
                            Size = GetSize(tds[4].Text),
                            Seeders = GetNumber(tds[1].Text),
                            Leechers = GetNumber(tds[2].Text)
                        });
                    }
                }
            });
            return torrents;
        }

        protected override string GetName(string str)
        {
            var a = _parser.GetTags(str, "a");
            if (!a.Success) return string.Empty;

            var last = a.Last();
            return base.GetName(last.Value);
        }

        protected override string GetLink(string str)
        {
            var a = _parser.GetTags(str, "a");
            if (!a.Success) return string.Empty;

            var last = a.Last();
            return base.GetLink(last.Value);
        }

        protected override int GetNumberOfPages()
        {
            var page = GetPageNumber(1);
            var div = _parser.GetTagsByClass(page, "div", "pagination");
            if (!div.Success) return 0;

            var lis = _parser.GetTags(div[0].Text, "li");
            if (!lis.Success) return 0;

            var a = _parser.GetTag(lis.Last().Text, "a");
            if (!a.Success) return 0;

            var href = a.Attributes.SingleOrDefault(attr => attr.Key == "href").Values.FirstOrDefault();
            if (href == null) return 0;

            var nb = Regex.Match(href, @"\d+");
            if (!nb.Success) return 0;

            return int.Parse(nb.Value);
        }

        protected override string GetPageNumber(int nb)
        {
            var searchUrl = $"{_url}/{nb}/";
            var content = WebService.Instance.GetContent(searchUrl);
            return content;
        }
    }
}
