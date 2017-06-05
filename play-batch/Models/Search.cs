using batch.Services.Torrents;

namespace batch.Models
{
    public class Search
    {
        public Criteria Category { get; set; }
        public Criteria Language { get; set; }
        public Criteria Quality { get; set; }
        public Criteria Type { get; set; }

        public Search(Criteria category, Criteria language, Criteria quality, Criteria type)
        {
            Category = category;
            Language = language;
            Quality = quality;
            Type = type;
        }

        public static Search NotFound = new Search(Criteria.NONE, Criteria.NONE, Criteria.NONE, Criteria.NONE);
    }
}
