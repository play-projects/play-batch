using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using batch.Models;
using batch.Services.Parser;

namespace batch.Services.Torrents
{
    public abstract class ABaseTorrentsService
    {
        protected readonly ParserFacade Parser = ParserFacade.Instance;

        public abstract List<Torrent> GetMovieTorrents();

        protected abstract int GetNumberOfPages(string url);
        protected abstract string GetPageNumber(string url, int nb);

        protected virtual string GetLink(string str)
        {
            var a = Parser.GetTag(str, "a");
            if (!a.Success) return string.Empty;

            var href = a.Attributes.SingleOrDefault(attr => attr.Key == "href")?.Values.FirstOrDefault();
            return href ?? string.Empty;
        }

        protected virtual string GetName(string str)
        {
            var a = Parser.GetTag(str, "a");
            if (!a.Success) return string.Empty;
            return a.GetText();
        }

        protected virtual double GetSize(string str)
        {
            var unite = Regex.Match(str, @"[a-zA-Z]+");
            if (!unite.Success) return 0;

            var nb = Regex.Match(str, @"\d+(\.\d+)?");
            if (!nb.Success) return 0;

            double.TryParse(nb.Value.Replace(".", ","), out double size);
            switch (unite.Value.ToUpper())
            {
                case "GB":
                case "GO":
                    size *= 1024 * 1024 * 1024;
                    break;
                case "MB":
                case "MO":
                    size *= 1024 * 1024;
                    break;
                case "KB":
                case "KO":
                    size *= 1024;
                    break;
            }
            return Math.Round(size);
        }

        protected virtual int GetNumber(string str)
        {
            str = str.Replace(".", string.Empty)
                .Replace(" ", string.Empty)
                .Replace(",", string.Empty);

            var match = Regex.Match(str, @"\d+");
            if (!match.Success) return 0;

            int.TryParse(match.Value, out int nb);
            return nb;
        }
    }
}
