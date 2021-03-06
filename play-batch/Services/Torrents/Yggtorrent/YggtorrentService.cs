﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            var orders = new List<(string sort, string order)>
            {
                ("publish_date", "asc"),
                ("publish_date", "desc"),
                ("name", "asc"),
                ("name", "desc")
            };

            var result = new List<Torrent>();
            foreach (var order in orders)
            {
                var torrents = GetMovieTorrents(order.sort, order.order);
                result.AddRange(torrents);
            }
            return result;
        }

        private IEnumerable<Torrent> GetMovieTorrents(string sort, string order)
        {
            var nbOfPages = GetNumberOfPages(sort, order);
            var torrents = new List<Torrent>();
            Parallel.For(1, nbOfPages + 1, new ParallelOptions { MaxDegreeOfParallelism = 25 }, nb =>
            {
                var page = GetPageNumber(sort, order, nb);
                var tables = _parser.GetTags(page, "table");
                if (!tables.Success || tables.Count != 2) return;

                var tbody = _parser.GetTag(tables[1].Text, "tbody");
                if (!tbody.Success) return;

                var trs = _parser.GetTags(tbody.Text, "tr");
                foreach (var tr in trs)
                {
                    var tds = _parser.GetTags(tr.Text, "td");
                    if (tds.Count != 9) continue;

                    lock (torrents)
                    {
                        var name = GetName(tds[1].Text);
                        Console.WriteLine($"{Source.Yggtorrent} - {name}");
                        torrents.Add(new Torrent
                        {
                            Name = GetName(tds[1].Text),
                            Link = GetLink(tds[1].Text),
                            Source = Source.Yggtorrent,
                            Size = GetSize(tds[5].Text),
                            Seeders = GetNumber(tds[7].Text),
                            Leechers = GetNumber(tds[8].Text)
                        });
                    }
                }
            });
            return torrents;
        }

        protected override int GetNumberOfPages()
        {
            var content = GetPageNumber(1);
            return GetNumberOfPages(content);
        }

        private int GetNumberOfPages(string sort, string order)
        {
            var content = GetPageNumber(sort, order, 1);
            return GetNumberOfPages(content);
        }

        private int GetNumberOfPages(string content)
        {
            var ul = _parser.GetTagsByClass(content, "ul", "pagination");
            if (!ul.Success) return 0;

            var lis = _parser.GetTags(ul[0].Text, "li");
            if (!lis.Success) return 0;

            var a = _parser.GetTag(lis.Last().Text, "a");
            if (!a.Success) return 0;

            var href = a.Attributes
                .SingleOrDefault(attr => attr.Key == "href")?
                .Values.FirstOrDefault();
            if (href == null) return 0;

            var data = Regex.Match(href, @"page=(\d+)");
            if (!data.Success) return 0;

            if (!int.TryParse(data.Groups[1].Value, out var page))
                return 0;
            return page / 50 + 1;
        }

        protected override string GetPageNumber(int nb)
        {
            var page = nb * 50;
            var searchUrl = $"{_url}&page={page}";
            return GetPageNumber(searchUrl);
        }

        private string GetPageNumber(string sort, string order, int nb)
        {
            var page = nb * 50;
            var searchUrl = $"{_url}&order={order}&sort={sort}&page={page}";
            return GetPageNumber(searchUrl);
        }

        private string GetPageNumber(string url)
        {
            var content = WebService.Instance.GetContent(url);
            return content;
        }
    }
}
