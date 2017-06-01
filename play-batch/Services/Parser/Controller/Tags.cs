using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace batch.Services.Parser.Controller
{
    public enum Type
    {
        Id, Class
    }

    public class Tags
    {
        private readonly Attributs Attributs = new Attributs();

        public Match GetTagPos(string tag, string str, bool inline = false)
        {
            var pattern = inline ? $"<{tag}(\"[^\"]*\"|'[^']*'|[^>\"'])*/>" 
                                 : $"<{tag}(\"[^\"]*\"|'[^']*'|[^>\"'])*>";
            var regex = new Regex(pattern);
            return regex.Match(str);
        }

        public Match GetTagPos(string tag, Type type, string name, string str, bool inline = false)
        {
            var pattern = inline ? $"<{tag}(\"[^\"]*\"|'[^']*'|[^>\"'])*/>"
                                 : $"<{tag}(\"[^\"]*\"|'[^']*'|[^>\"'])*>";
            var regex = new Regex(pattern);
            var matches = regex.Matches(str);

            foreach (Match match in matches)
            {
                var attributes = Attributs.GetAttributs(match.Value);
                if (type == Type.Class)
                {
                    if (attributes.Any(a => a.Key.Equals("class") && a.Values.Contains(name)))
                        return match;
                }
                if (type == Type.Id)
                {
                    if (attributes.Any(a => a.Key.Equals("id") && a.Values.Contains(name)))
                        return match;
                }
            }
            return Match.Empty;
        }

        public List<Match> GetTagsPos(string tag, string str, bool inline = false)
        {
            var pattern = inline ? $"<{tag}(\"[^\"]*\"|'[^']*'|[^>\"'])*/>"
                                 : $"<{tag}(\"[^\"]*\"|'[^']*'|[^>\"'])*>";
            var regex = new Regex(pattern);
            var matches = regex.Matches(str);

            return matches.Cast<Match>()
                            .Select(m => m)
                            .ToList();
        }

        public List<Match> GetTagsPos(string tag, Type type, string name, string str, bool inline = false)
        {
            var pattern = inline ? $"<{tag}(\"[^\"]*\"|'[^']*'|[^>\"'])*/>"
                                 : $"<{tag}(\"[^\"]*\"|'[^']*'|[^>\"'])*>";
            var regex = new Regex(pattern);
            var matches = regex.Matches(str);

            var result = new List<Match>();
            foreach (Match match in matches)
            {
                var attributes = Attributs.GetAttributs(match.Value);
                if (type == Type.Class)
                {
                    if (attributes.Any(a => a.Key.Equals("class") && a.Values.Contains(name)))
                        result.Add(match);
                }
                if (type == Type.Id)
                {
                    if (attributes.Any(a => a.Key.Equals("id") && a.Values.Contains(name)))
                        result.Add(match);
                }
            }
            return result;
        }
    }
}
