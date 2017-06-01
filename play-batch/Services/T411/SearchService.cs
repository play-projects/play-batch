using System.Net;
using System.Text;
using batch.Models;

namespace batch.Services.T411
{
    public enum Criteria
    {
        MOVIE,
        VF, VOSTFR,
        LOW, MEDIUM, HIGH, VERYHIGH,
        TWOD,
        NONE
    }

    public class SearchService
    {
        private static readonly string baseUrl = "https://www.t411.al";

        private static readonly string movie = "cat=210&subcat=631";
        private static readonly string vf = "term[17][]=541";
        private static readonly string vostfr = "term[17][]=721";
        private static readonly string low = "term[7][]=8&term[7][]=10&term[7][]=11&term[7][]=1233&term[7][]=19";
        private static readonly string medium = "term[7][]=15&term[7][]=12&term[7][]=1175&term[7][]=1238";
        private static readonly string high = "term[7][]=16&term[7][]=1162&term[7][]=1233&term[7][]=1237";
        private static readonly string veryHigh = "term[7][]=1219&term[7][]=1235&term[7][]=1182&term[7][]=1239";
        private static readonly string twoD = "term[9][]=22";

        public static string GetSearchUri(Search search)
        {
            var sb = new StringBuilder();
            sb.Append($"{baseUrl}/torrents/search/?name=&description=&file=&user=");
            if (search.Category == Criteria.MOVIE)
                sb.Append($"&{movie}");
            if (search.Language == Criteria.VF)
                sb.Append($"&{vf}");
            if (search.Language == Criteria.VOSTFR)
                sb.Append($"&{vostfr}");
            if (search.Quality == Criteria.LOW)
                sb.Append($"&{low}");
            if (search.Quality == Criteria.MEDIUM)
                sb.Append($"&{medium}");
            if (search.Quality == Criteria.HIGH)
                sb.Append($"&{high}");
            if (search.Quality == Criteria.VERYHIGH)
                sb.Append($"&{veryHigh}");
            if (search.Type == Criteria.TWOD)
                sb.Append($"&{twoD}");
            sb.Append("&search=&submit=Recherche");
            return sb.ToString();
        }
    }
}
