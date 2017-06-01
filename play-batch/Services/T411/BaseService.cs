using System;
using System.Linq;
using System.Text.RegularExpressions;
using batch.Services.Parser;
using batch.Services.Web;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace batch.Services.T411
{
    public class BaseService
    {
        private readonly ParserFacade parser = ParserFacade.Instance;

        protected Dictionary<int, string> GetSearch(string url)
        {
            var nbOfPages = GetNumberOfPages(url);
            var movies = new Dictionary<int, string>();
            Parallel.For(0, nbOfPages + 1, new ParallelOptions { MaxDegreeOfParallelism = 25 }, nb =>
            {
                var page = GetPageNumber(url, nb);
                var table = parser.GetTag(page, "tbody");
                var trs = parser.GetTags(table.Value, "tr");
                foreach (var tr in trs)
                {
                    var tds = parser.GetTags(tr.Value, "td");
                    if (tds.Count < 3) continue;

                    var id = GetId(tds[2].Value);
                    var name = GetName(tds[1].Value);
                    if (id != 0 && !string.IsNullOrEmpty(name))
                    {
                        lock (movies)
                        {
                            movies.Add(id, name);
                            Console.WriteLine($"movies: {movies.Count}");
                        }
                    }
                }
            });
            return movies;
        }

        private int GetId(string str)
        {
            var a = parser.GetTag(str, "a");
            if (!a.Success) return 0;

            var href = a.Attributes.SingleOrDefault(attr => attr.Key == "href").Values;
            var pattern = "id=[0-9]+";
            var match = new Regex(pattern).Match(href[0]);
            if (!match.Success) return 0;

            int.TryParse(match.Value.Substring(3), out int id);
            return id;
        }

        private string GetName(string str)
        {
            var a = parser.GetTag(str, "a");
            if (!a.Success) return string.Empty;

            var href = a.Attributes.SingleOrDefault(attr => attr.Key == "title").Values;
            return href.Count > 0 ? href[0] : string.Empty;
        }

        private string GetPageNumber(string searchUrl, int nb)
        {
            var url = $"{searchUrl}&page={nb}#paginator";
            var page = WebService.Instance.GetContent(url);
            return page;
        }

        private int GetNumberOfPages(string searchUrl)
        {
            var page = GetPageNumber(searchUrl, 0);

            var pagebar = parser.GetTagsByClass(page, "div", "pagebar");
            if (pagebar.Count != 1) return 0;

            var a = parser.GetTags(pagebar[0].Value, "a");
            if (a.Count == 0) return 0;

            var last = a[a.Count - 2];
            var href = last.Attributes.SingleOrDefault(attr => attr.Key == "href").Values.First();

            var pattern = "page=[0-9]+";
            var match = new Regex(pattern).Match(href);
            if (!match.Success) return 0;

            int.TryParse(match.Value.Substring(5), out int pages);
            return pages;
        }
    }
}
